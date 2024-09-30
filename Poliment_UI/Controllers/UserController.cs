using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Poliment_DL;
using Poliment_DL.Model;
using Poliment_UI.Models;
using System.IO;
using System.Configuration;
using System.Web.UI;
using RestSharp;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Net.Mail;
namespace Poliment_UI.Controllers
{
    public class UserController : Controller
    {
        private UserDL userDL = new UserDL();
        private AdminDL adminDL = new AdminDL();
        private CommonDL commonDL = new CommonDL();
        string error = string.Empty;
        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string userName, string passWord)
        {
            string updateResult = string.Empty;
            UserML userML = new UserML();
            UserVM userVM = new UserVM();
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
            {
                userML = userDL.GetLogin(userName, passWord);
            }
            else
            {
                if (string.IsNullOrEmpty(userName))
                {
                    ViewBag.UserName = "Please enter user name";
                }
                if (string.IsNullOrEmpty(passWord))
                {
                    ViewBag.Password = "Please enter password";
                }
                return View();
            }

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord) && userML != null)
            {
                if (!string.IsNullOrEmpty(userML.ErrorMessage))
                {
                    ViewBag.Error = userML.ErrorMessage;
                    return View();
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(userName, false);
                    userVM.UserML = userML;
                    Session["UserId"] = userML.Id;
                    Session["UserName"] = userName;
                    updateResult = userDL.SetUpdateCountAfterLogin(userML.Id);
                    return RedirectToAction("Dashboard");
                }
            }
            else
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string resetUserName)
        {

            UserML userML = new UserML();
            AdminOtpML adminOtp = new AdminOtpML(); // Here using AdminOtpML because user and admin have same model
            AdminOtpML getAdminOtp = new AdminOtpML();

            LayoutVM layoutVM = new LayoutVM();
            string result = string.Empty;

            string messageApi = Convert.ToString(ConfigurationManager.AppSettings["MessageApi"]);
            string country = Convert.ToString(ConfigurationManager.AppSettings["country"]);
            string sender = Convert.ToString(ConfigurationManager.AppSettings["sender"]);
            string route = Convert.ToString(ConfigurationManager.AppSettings["transactionlRoute"]);
            string authkey = Convert.ToString(ConfigurationManager.AppSettings["authkey"]);
            string mobiles = string.Empty;
            string message = string.Empty;
            string restMessageUrl = string.Empty;
            string emailId = string.Empty;

            if (!string.IsNullOrEmpty(resetUserName))
            {
                userML = userDL.GetUserByUserName(resetUserName);
            }
            else
            {
                if (string.IsNullOrEmpty(resetUserName))
                {
                    ViewBag.ResetUserName = "Please enter user name";
                }
                return View("Index");
            }
            if (!string.IsNullOrEmpty(resetUserName) && userML != null)
            {
                if (!string.IsNullOrEmpty(userML.ErrorMessage))
                {
                    ViewBag.ResetError = userML.ErrorMessage;
                }
                else
                {
                    #region Send otp through SMS 

                    //int resetPasswordOtpCount = Convert.ToInt32(ConfigurationManager.AppSettings["ResetPasswordOtpCount"]);
                    //mobiles = "91" + userML.Mobile;
                    //Random random = new Random();
                    //int randomNumber = random.Next(0, 999999);
                    //message = CommonUIResource.ResetPassword + randomNumber;
                    //restMessageUrl = messageApi + "?country=" + country + "&sender=" + sender + "&route=" + route + "&mobiles=" + mobiles + "&authkey=" + authkey + "&message=" + message;
                    //getAdminOtp = userDL.GetUserOtp(userML.Id);
                    //if (getAdminOtp.AdminId > 0)
                    //{
                    //    int? updateCount = getAdminOtp.UpdateCount;
                    //    if (updateCount <= resetPasswordOtpCount)
                    //    {
                    //        var smsClient = new RestClient(restMessageUrl);
                    //        var request = new RestRequest(Method.GET);
                    //        IRestResponse response = smsClient.Execute(request);
                    //        string statusCode = Convert.ToString(response.StatusCode);
                    //        if (response.IsSuccessful == true && statusCode == "OK")
                    //        {
                    //            adminOtp.AdminId = userML.Id;
                    //            adminOtp.Mobile = userML.Mobile;
                    //            adminOtp.OtpValue = randomNumber;
                    //            result = userDL.SaveUserOtp(adminOtp);
                    //            if (result == "success")
                    //            {
                    //                ViewBag.ShowOtpDiv = userML.Id;
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        ViewBag.ReachedPasswordOtp = "Maximum limits of sending otp has ended for today";
                    //    }
                    //}
                    //else
                    //{
                    //    var smsClient = new RestClient(restMessageUrl);
                    //    var request = new RestRequest(Method.GET);
                    //    IRestResponse response = smsClient.Execute(request);
                    //    string statusCode = Convert.ToString(response.StatusCode);
                    //    if (response.IsSuccessful == true && statusCode == "OK")
                    //    {
                    //        adminOtp.AdminId = userML.Id;
                    //        adminOtp.Mobile = userML.Mobile;
                    //        adminOtp.OtpValue = randomNumber;
                    //        result = userDL.SaveUserOtp(adminOtp);
                    //        if (result == "success")
                    //        {
                    //            ViewBag.ShowOtpDiv = userML.Id;
                    //        }
                    //    }
                    //}

                    #endregion

                    #region Send otp through Email
                    string mailResult = string.Empty;
                    emailId = userML.Email;
                    Random random = new Random();
                    int randomNumber = random.Next(0, 999999);
                    message = CommonUIResource.ResetPassword + randomNumber;

                    getAdminOtp = userDL.GetUserOtp(userML.Id);
                    if (getAdminOtp.AdminId > 0)
                    {
                        mailResult = SendOtpMail(message, emailId);
                        if (mailResult == "success")
                        {
                            adminOtp.AdminId = userML.Id;
                            adminOtp.Mobile = userML.Mobile;
                            adminOtp.OtpValue = randomNumber;
                            result = userDL.SaveUserOtp(adminOtp);
                            if (result == "success")
                            {
                                ViewBag.ShowOtpDiv = userML.Id;
                            }
                        }
                    }
                    else
                    {
                        mailResult = SendOtpMail(message, emailId);
                        if (mailResult == "success")
                        {
                            adminOtp.AdminId = userML.Id;
                            adminOtp.Mobile = userML.Mobile;
                            adminOtp.OtpValue = randomNumber;
                            result = userDL.SaveUserOtp(adminOtp);
                            if (result == "success")
                            {
                                ViewBag.ShowOtpDiv = userML.Id;
                            }
                        }
                    }
                    #endregion

                }

            }
            return View("Index");
        }

        private string SendOtpMail(string message, string emailId)
        {
            string resultMail = string.Empty;
            string smtp = Convert.ToString(ConfigurationManager.AppSettings["Smtp"]);
            string fromEmail = Convert.ToString(ConfigurationManager.AppSettings["fromEmail"]);
            string mailPassword = Convert.ToString(ConfigurationManager.AppSettings["mailPassword"]);
            string subject = Convert.ToString(ConfigurationManager.AppSettings["subject"]);
            int port = Convert.ToInt32(Convert.ToString(ConfigurationManager.AppSettings["port"]));
            try
            {
                SmtpClient SmtpServer = new SmtpClient(smtp);
                var mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(emailId);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = message;
                SmtpServer.Port = port;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, mailPassword);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                resultMail = "success";
            }
            catch (Exception ex)
            {
                error = ex.Message;
                resultMail = "error";
            }
            return resultMail;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidateAdminOtp(string resetOtp, string adminId)
        {
            AdminOtpML adminOtpML = new AdminOtpML();
            try
            {
                if (string.IsNullOrEmpty(resetOtp))
                {
                    ViewBag.ResetOtpNull = "Otp value can not be null";
                    return View("Index");
                }
                else
                {
                    if (!string.IsNullOrEmpty(adminId))
                    {
                        int Id = Convert.ToInt32(adminId);
                        int resetOtpValue = Convert.ToInt32(resetOtp);
                        adminOtpML = userDL.GetUserOtp(Id);
                        if (adminOtpML.AdminId > 0)
                        {
                            if (adminOtpML.OtpValue == resetOtpValue)
                            {
                                ViewBag.adminId = adminId;
                                return View("ResetPasswordInterface");
                            }
                            else
                            {
                                ViewBag.WrongAdminOtp = "Wrong otp entered";
                            }
                        }
                        else
                        {
                            ViewBag.InvalidAdminOtp = "Invalid otp entered";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return View("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPasswordInterface(string passWord, string hdnAdminId)
        {
            // Here hdnAdminId contains user id
            string result = string.Empty;
            int userId = 0;
            try
            {
                if (!string.IsNullOrEmpty(passWord) && !string.IsNullOrEmpty(hdnAdminId))
                {
                    userId = Convert.ToInt32(hdnAdminId);
                    result = userDL.ChangeUserPassword(userId, passWord);
                    if (result == "success")
                    {
                        ViewBag.PasswordChangedSuccessfully = "Password Changed Successfully";
                    }
                    else
                    {
                        ViewBag.PasswordChangedError = "An error occured";
                    }
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return View("Index");
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult UserChangePassword()
        {
            return View();
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UserChangePassword(string passWord)
        {
            string result = string.Empty;
            int userId = 0;
            try
            {
                if (!string.IsNullOrEmpty(passWord))
                {
                    userId = Convert.ToInt32(Session["UserId"]);
                    result = userDL.ChangeUserPassword(userId, passWord);
                    if (result == "success")
                    {
                        ViewBag.UserPasswordChangedSuccessfully = "Password Changed Successfully";
                    }
                    else
                    {
                        ViewBag.UserPasswordChangedError = "An error occured";
                    }
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return View("UserChangePassword");
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult UserBankDetails()
        {
            UserBankDetailsML userBankDetailsML = new UserBankDetailsML();
            int userId = Convert.ToInt32(Session["UserId"]);
            try
            {
                userBankDetailsML = commonDL.GetUserBankDetails(userId);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(userBankDetailsML);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UserBankDetails(string accountNumber, string ifscCode, string bankName, string branch)
        {
            UserBankDetailsML userBankDetailsML = new UserBankDetailsML();
            string result = string.Empty;
            int userId = 0;
            try
            {
                if (!string.IsNullOrEmpty(accountNumber) && !string.IsNullOrEmpty(ifscCode))
                {
                    userId = Convert.ToInt32(Session["UserId"]);
                    userBankDetailsML.UserId = userId;
                    userBankDetailsML.AccountNumber = accountNumber;
                    userBankDetailsML.IfscCode = ifscCode;
                    userBankDetailsML.BankName = bankName;
                    userBankDetailsML.BranchAddress = branch;
                    result = commonDL.SaveUserBankDetails(userBankDetailsML);
                    if (result == "success")
                    {
                        TempData["BankAccSaved"] = "Account details saved successfully";
                    }
                    else
                    {
                        TempData["BankAccError"] = "An error occured";
                    }
                    return RedirectToAction("UserBankDetails");
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_UserError");
        }


        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]

        public ActionResult SendMessage()
        {
            return View();
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SendMessage(HttpPostedFileBase postedFile, string txtSubject, string txtMessagebody)
        {
            UserMessageML userMessageML = new UserMessageML();
            string dbImagePath = string.Empty;
            string fileName = string.Empty;
            string result = string.Empty;
            int userId = 0;

            try
            {
                if (postedFile != null)
                {
                    if (postedFile.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(postedFile.FileName);
                        long dateValue = DateTime.Now.ToFileTime();
                        string[] arrName = fileName.Split('.');
                        if (arrName.Length == 2)
                        {
                            fileName = arrName[0] + dateValue + "." + arrName[1];
                            dbImagePath = fileName;
                        }
                        string path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        postedFile.SaveAs(path);
                    }
                }

                if (!string.IsNullOrEmpty(txtSubject) && !string.IsNullOrEmpty(txtMessagebody))
                {
                    userMessageML.MessageSubject = txtSubject;
                    userMessageML.MessageBody = txtMessagebody;
                    userMessageML.FileName = fileName;
                    userId = Convert.ToInt32(Session["UserId"]);
                    UserML userDetails = commonDL.GetUserDetails(userId);
                    if (userDetails != null)
                    {
                        userMessageML.UserId = Convert.ToInt32(Session["UserId"]);
                        userMessageML.UserFullName = userDetails.FirstName + " " + userDetails.LastName;
                    }
                    result = userDL.SaveMessage(userMessageML);
                    if (result == "success")
                    {
                        ViewBag.UserMsgSaved = "User message saved successfully";
                    }
                    else
                    {
                        ViewBag.UserMsgError = "User message error";
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                result = "error";
            }
            return View();
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]

        public ActionResult SentMessage(int page = 1)
        {
            AdminMessageVM adminMessageVM = new AdminMessageVM();
            List<UserMessageML> lstUserMessageML = new List<UserMessageML>();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["UserMessageTake"]);
            int skip = (page - 1) * take;
            int userId = Convert.ToInt32(Session["UserId"]);
            try
            {
                adminMessageVM.PageSize = take;
                adminMessageVM.TotalRows = userDL.GetUserMessageCount(userId);
                lstUserMessageML = userDL.GetUserMessage(userId, skip, take);
                adminMessageVM.UserMessageML = lstUserMessageML;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(adminMessageVM);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]

        public ActionResult Inbox(int page = 1)
        {
            AdminMessageVM adminMessageVM = new AdminMessageVM();
            List<UserMessageML> lstUserMessageML = new List<UserMessageML>();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["UserMessageTake"]);
            int skip = (page - 1) * take;
            int userId = Convert.ToInt32(Session["UserId"]);
            try
            {
                adminMessageVM.PageSize = take;
                adminMessageVM.TotalRows = userDL.GetAdminMessageForUserCount(userId);
                lstUserMessageML = userDL.GetAdminMessageForUser(userId, skip, take);
                adminMessageVM.UserMessageML = lstUserMessageML;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(adminMessageVM);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult InboxMessageDetails(int id)
        {
            UserMessageML userMessageML = new UserMessageML();
            int messageId = id;
            try
            {
                if (id > 0)
                {
                    userMessageML = adminDL.GetAdminSentMessageDetails(messageId);
                    if (userMessageML != null)
                    {
                        userMessageML.AttachmentPath = Constant.AbsalutePath + CommonUIResource.FolderPath + "/" + userMessageML.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(userMessageML);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult UpdateUserDetails()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            UserML userML = new UserML();
            try
            {
                userML = userDL.GetUserById(userId);
                return View("UpdateUserDetails", userML);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_UserError");
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult MessageDetails(int id)
        {
            UserMessageML userMessageML = new UserMessageML();
            int messageId = id;
            try
            {
                if (id > 0)
                {
                    userMessageML = adminDL.GetMessageDetails(messageId);
                    if (userMessageML != null)
                    {
                        userMessageML.AttachmentPath = Constant.AbsalutePath + CommonUIResource.FolderPath + "/" + userMessageML.FileName;
                        userMessageML.ReplyFilePath = Constant.AbsalutePath + CommonUIResource.FolderPath + "/" + userMessageML.ReplyFileName;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(userMessageML);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpPost]

        public JsonResult UpdateUserDetails(UserML userML)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string result = string.Empty;
            try
            {
                if (userML != null)
                {
                    userML.Id = userId;
                    result = userDL.UpdateUserDetails(userML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return Json(result);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpPost]
        public JsonResult CheckMobileExist(string mobileValue)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(mobileValue))
                {
                    result = commonDL.CheckUserMobile(mobileValue);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(result);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpPost]
        public JsonResult CheckVotersMobile(string mobileValue)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(mobileValue))
                {
                    result = commonDL.CheckVotersMobile(mobileValue);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(result);
        }


        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpPost]
        public JsonResult CheckEmailExist(string emailValue)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(emailValue))
                {
                    result = commonDL.CheckUserEmail(emailValue);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(result);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult CreateUser()
        {
            UserML userML = new UserML();
            int userId = Convert.ToInt32(Session["UserId"]);
            userML = userDL.GetUserById(userId);
            CreateUserVM createUserVM = new CreateUserVM();
            if (userML.IsSuperUser == true)
            {
                try
                {
                    createUserVM.BlockML = adminDL.GetBlock();
                    string blockName = createUserVM.BlockML[0].BlockName;
                    createUserVM.AreaML = adminDL.GetArea(blockName);

                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                return View(createUserVM);
            }
            else
            {
                TempData["NotSuperUser"] = "Not super user";
                return RedirectToAction("Dashboard");
            }
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpPost]
        public JsonResult CreateUser(CreateUserVM createUserVM)
        {
            UserML userML = new UserML();
            string plainPassword = string.Empty;
            int userId = 0;
            // Saving Image in folder
            string fname = string.Empty;
            // creating object to save value in database

            userML.Id = createUserVM.Id;
            userML.BlockId = createUserVM.BlockId;
            userML.BlockName = createUserVM.BlockName;
            userML.AreaId = createUserVM.AreaId;
            userML.AreaName = createUserVM.AreaName;
            userML.FirstName = createUserVM.FirstName;
            userML.LastName = createUserVM.LastName;
            userML.UserName = createUserVM.UserName;
            plainPassword = createUserVM.Password;
            userML.Password = CryptorEngine.Encrypt(plainPassword, true);
            userML.DOB = createUserVM.DOB;
            userML.Gender = createUserVM.Gender;
            userML.Mobile = createUserVM.Mobile;
            userML.Email = createUserVM.Email;
            userML.CreatedBy = Convert.ToString(Session["UserName"]);
            if (createUserVM.Id == 0)
            {
                userML.Image = null;
            }
            else
            {
                UserML user = new UserML();
                user = adminDL.GetUser(createUserVM.Id);
                userML.Image = user.Image;
            }
            userML.Designation = createUserVM.Designation;
            userML.PoliticalParty = createUserVM.PoliticalParty;
            userId = userDL.SaveUser(userML);
            return Json(userId);
        }


        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult EnterVoterDetails()
        {
            CreateUserVM createUserVM = new CreateUserVM();
            try
            {
                createUserVM.BlockML = adminDL.GetBlock();
                string blockName = createUserVM.BlockML[0].BlockName;
                int delhiBlockId = createUserVM.BlockML[0].Id;
                createUserVM.AreaML = adminDL.GetArea(blockName);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(createUserVM);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpPost]
        public ActionResult EnterVoterDetails(CreateUserVM createUserVM)
        {
            VidhanSabhaVotersML vidhanSabhaVotersML = new VidhanSabhaVotersML();
            string userName = Convert.ToString(Session["UserName"]);
            string result = string.Empty;
            try
            {
                vidhanSabhaVotersML.BlockId = createUserVM.BlockId;
                vidhanSabhaVotersML.BlockName = createUserVM.BlockName;
                vidhanSabhaVotersML.AreaId = createUserVM.AreaId;
                vidhanSabhaVotersML.AreaName = createUserVM.AreaName;
                vidhanSabhaVotersML.Name = createUserVM.Name;
                vidhanSabhaVotersML.Mobile = createUserVM.Mobile;
                vidhanSabhaVotersML.Gender = createUserVM.Gender;
                vidhanSabhaVotersML.VoterId = createUserVM.VoterId;
                vidhanSabhaVotersML.Address = createUserVM.Address;
                vidhanSabhaVotersML.PinCode = createUserVM.PinCode;
                vidhanSabhaVotersML.CreatedBy = userName;
                result = userDL.SaveVoterDetails(vidhanSabhaVotersML);
                return Json(result);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(createUserVM);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult UploadFiles()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpPost]
        public JsonResult UploadFiles(string userId)
        {
            string fname = string.Empty;
            string dbImagePath = string.Empty;
            string returnMessage = string.Empty;
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory + "UserImage/";
                        string filename = Path.GetFileName(Request.Files[i].FileName);

                        HttpPostedFileBase file = files[i];

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        long dateValue = DateTime.Now.ToFileTime();
                        string[] arrName = fname.Split('.');
                        if (arrName.Length == 2)
                        {
                            fname = arrName[0] + dateValue + "." + arrName[1];
                            dbImagePath = fname;
                        }
                        fname = Path.Combine(Server.MapPath("~/UserImage/"), fname);
                        file.SaveAs(fname);
                    }

                    string result = adminDL.SaveImagePath(Convert.ToInt32(userId), dbImagePath);
                    returnMessage = "success";
                }
                catch (Exception)
                {
                    returnMessage = "error";
                }
            }
            return Json(returnMessage);

        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpPost]
        public JsonResult CheckUserNameExist(string userNameValue)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(userNameValue))
                {
                    result = commonDL.CheckUserNameExist(userNameValue);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(result);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpPost]
        public JsonResult GetArea(string blockName)
        {
            List<AreaML> lstAreaML = new List<AreaML>();
            try
            {
                if (!string.IsNullOrEmpty(blockName))
                {
                    lstAreaML = adminDL.GetArea(blockName);
                }

            }
            catch (Exception)
            {
                return Json("error");
            }
            return Json(lstAreaML);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult SearchInActiveUserByBlockArea(int page = 1)
        {
            string userName = Convert.ToString(Session["UserName"]);
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["SearchByBlockAreaTakeForUsers"]);
            int skip = (page - 1) * take;
            string blockName = (Request.QueryString["BlockName"] != null) ? Convert.ToString(Request.QueryString["BlockName"]) : string.Empty;
            string areaName = (Request.QueryString["AreaName"] != null) ? Convert.ToString(Request.QueryString["AreaName"]) : string.Empty;
            string sort = (Request.QueryString["sort"] != null) ? Convert.ToString(Request.QueryString["sort"]) : string.Empty;
            string sortDir = (Request.QueryString["sortdir"] != null) ? Convert.ToString(Request.QueryString["sortdir"]) : string.Empty;

            CreateUserVM createUserVM = new CreateUserVM();
            if (!string.IsNullOrEmpty(blockName))
            {
                createUserVM.PageSize = take;
                createUserVM.TotalRows = userDL.GetInActiveUserByBlockAreaCount(blockName, areaName, userName);
                createUserVM.ListUserML = userDL.GetInActiveUserByBlockArea(blockName, areaName, userName, skip, take, sort, sortDir);
                int blockId = Convert.ToInt32(blockName);
                createUserVM.BlockML = adminDL.GetBlock();
                string blockNameOfId = commonDL.GetBlockById(blockId);
                createUserVM.AreaML = adminDL.GetArea(blockNameOfId);
                // Here BlockName and AreaName contains id value
                createUserVM.BlockName = blockName;
                createUserVM.AreaName = areaName;
            }
            else
            {
                createUserVM.BlockML = adminDL.GetBlock();
                string defaultBlockName = createUserVM.BlockML[0].BlockName;
                createUserVM.AreaML = adminDL.GetArea(defaultBlockName);
            }
            return View(createUserVM);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]

        public ActionResult ShowUserImage(int id)
        {
            UserML userML = new UserML();
            CreateUserVM createUserVM = new CreateUserVM();
            try
            {
                userML = adminDL.GetUser(id);
                createUserVM.BlockML = adminDL.GetBlock();
                if (userML != null)
                {
                    createUserVM.AreaML = adminDL.GetArea(userML.BlockName);
                    createUserVM.BlockName = Convert.ToString(userML.BlockId);
                    createUserVM.AreaName = Convert.ToString(userML.AreaId);
                    createUserVM.Id = userML.Id;
                    createUserVM.FirstName = userML.FirstName;
                    createUserVM.LastName = userML.LastName;
                    createUserVM.UserName = userML.UserName;
                    createUserVM.Password = CryptorEngine.Decrypt(userML.Password, true);
                    createUserVM.Mobile = userML.Mobile;
                    createUserVM.Email = userML.Email;
                    createUserVM.Gender = userML.Gender;
                    createUserVM.DOB = userML.DOB;
                    createUserVM.Designation = userML.Designation;
                    createUserVM.PoliticalParty = userML.PoliticalParty;
                    createUserVM.Image = Constant.AbsalutePath + "UserImage/" + userML.Image;
                    createUserVM.AbsolutePath = Constant.AbsalutePath;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View("ShowUserImage", createUserVM);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]

        public ActionResult DeleteUser(int id)
        {
            CreateUserVM createUserVM = new CreateUserVM();
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeleteUser(id);
                    if (result == "success")
                    {
                        ViewBag.UserDeleted = "User deleted successfully";
                        createUserVM.BlockML = adminDL.GetBlock();
                        string defaultBlockName = createUserVM.BlockML[0].BlockName;
                        createUserVM.AreaML = adminDL.GetArea(defaultBlockName);
                    }
                    return View("SearchInActiveUserByBlockArea", createUserVM);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_UserError");
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult SearchActiveUserByBlockArea(int page = 1)
        {
            string userName = Convert.ToString(Session["UserName"]);
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["SearchByBlockAreaTakeForUsers"]);
            int skip = (page - 1) * take;
            string blockName = (Request.QueryString["BlockName"] != null) ? Convert.ToString(Request.QueryString["BlockName"]) : string.Empty;
            string areaName = (Request.QueryString["AreaName"] != null) ? Convert.ToString(Request.QueryString["AreaName"]) : string.Empty;
            string sort = (Request.QueryString["sort"] != null) ? Convert.ToString(Request.QueryString["sort"]) : string.Empty;
            string sortDir = (Request.QueryString["sortdir"] != null) ? Convert.ToString(Request.QueryString["sortdir"]) : string.Empty;

            CreateUserVM createUserVM = new CreateUserVM();
            if (!string.IsNullOrEmpty(blockName))
            {
                createUserVM.PageSize = take;
                createUserVM.TotalRows = userDL.GetActiveUserByBlockAreaCount(blockName, areaName, userName);
                createUserVM.ListUserML = userDL.GetActiveUserByBlockArea(blockName, areaName, userName, skip, take, sort, sortDir);
                int blockId = Convert.ToInt32(blockName);
                createUserVM.BlockML = adminDL.GetBlock();
                string blockNameOfId = commonDL.GetBlockById(blockId);
                createUserVM.AreaML = adminDL.GetArea(blockNameOfId);
                // Here BlockName and AreaName contains id value
                createUserVM.BlockName = blockName;
                createUserVM.AreaName = areaName;
            }
            else
            {
                createUserVM.BlockML = adminDL.GetBlock();
                string defaultBlockName = createUserVM.BlockML[0].BlockName;
                createUserVM.AreaML = adminDL.GetArea(defaultBlockName);
            }
            return View(createUserVM);
        }

        [UserSessionActionFilter]
        [Authorize(Roles = "UserFirRl")]
        [HttpGet]
        public ActionResult SearchByUserName(int page = 1)
        {
            string user = Convert.ToString(Session["UserName"]);
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["SearchByBlockAreaTakeForUsers"]);
            int skip = (page - 1) * take;
            string userName = (Request.QueryString["userName"] != null) ? Convert.ToString(Request.QueryString["userName"]) : string.Empty;
            string sort = (Request.QueryString["sort"] != null) ? Convert.ToString(Request.QueryString["sort"]) : string.Empty;
            string sortDir = (Request.QueryString["sortdir"] != null) ? Convert.ToString(Request.QueryString["sortdir"]) : string.Empty;

            CreateUserVM createUserVM = new CreateUserVM();
            if (!string.IsNullOrEmpty(userName))
            {
                createUserVM.PageSize = take;
                createUserVM.TotalRows = userDL.GetUserByUserNameCount(userName, user);
                createUserVM.ListUserML = userDL.GetUserByUserName(userName, user, skip, take, sort, sortDir);
                if (createUserVM.ListUserML.Count == 0)
                {
                    ViewBag.NoRecordFound = "No record found";
                }

            }
            return View(createUserVM);
        }

        public ActionResult AkhtarTest()
        {
            return View();
        }

    }
}