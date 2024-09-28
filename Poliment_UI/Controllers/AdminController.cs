using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poliment_DL;
using Poliment_DL.Model;
using Poliment_UI.Models;
using System.Web.Optimization;
using Poliment_UI;
using System.IO;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using RestSharp;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using NAudio.Wave;
using System.Net.Mail;
using System.Web.UI.WebControls;
using OfficeOpenXml;

namespace Poliment_UI.Controllers
{
    public class AdminController : Controller
    {
        private AdminDL adminDL = new AdminDL();
        private CommonDL commonDL = new CommonDL();
        string error = string.Empty;
        // GET: Admin


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
            AdminML adminML = new AdminML();
            LayoutVM layoutVM = new LayoutVM();
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
            {
                adminML = adminDL.GetLogin(userName, passWord);
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

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord) && adminML != null)
            {
                if (!string.IsNullOrEmpty(adminML.ErrorMessage))
                {
                    ViewBag.Error = adminML.ErrorMessage;
                    return View();
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(userName, false);
                    layoutVM.AdminML = adminML;
                    layoutVM.BlockML = adminDL.GetBlock();
                    Session["AdminId"] = adminML.Id;
                    Session["AdminUserName"] = userName;
                    updateResult = adminDL.SetUpdateCountAfterLogin(adminML.Id);
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
            AdminML adminML = new AdminML();
            AdminOtpML adminOtp = new AdminOtpML();
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
                adminML = adminDL.GetAdminByUserName(resetUserName);
            }
            else
            {
                if (string.IsNullOrEmpty(resetUserName))
                {
                    ViewBag.ResetUserName = "Please enter user name";
                }
                return View("Index");
            }
            if (!string.IsNullOrEmpty(resetUserName) && adminML != null)
            {
                if (!string.IsNullOrEmpty(adminML.ErrorMessage))
                {
                    ViewBag.ResetError = adminML.ErrorMessage;
                }
                else
                {
                    #region Send otp through SMS
                    //int resetPasswordOtpCount = Convert.ToInt32(ConfigurationManager.AppSettings["ResetPasswordOtpCount"]);
                    //mobiles = "91" + adminML.Mobile;
                    //Random random = new Random();
                    //int randomNumber = random.Next(0, 999999);
                    //message = CommonUIResource.ResetPassword + randomNumber;
                    //restMessageUrl = messageApi + "?country=" + country + "&sender=" + sender + "&route=" + route + "&mobiles=" + mobiles + "&authkey=" + authkey + "&message=" + message;
                    //getAdminOtp = adminDL.GetAdminOtp(adminML.Id);
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
                    //            adminOtp.AdminId = adminML.Id;
                    //            adminOtp.Mobile = adminML.Mobile;
                    //            adminOtp.OtpValue = randomNumber;
                    //            result = adminDL.SaveAdminOtp(adminOtp);
                    //            if (result == "success")
                    //            {
                    //                ViewBag.ShowOtpDiv = adminML.Id;
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
                    //        adminOtp.AdminId = adminML.Id;
                    //        adminOtp.Mobile = adminML.Mobile;
                    //        adminOtp.OtpValue = randomNumber;
                    //        result = adminDL.SaveAdminOtp(adminOtp);
                    //        if (result == "success")
                    //        {
                    //            ViewBag.ShowOtpDiv = adminML.Id;
                    //        }
                    //    }
                    //}

                    #endregion

                    #region Send otp through Email
                    string mailResult = string.Empty;
                    emailId = adminML.Email;
                    Random random = new Random();
                    int randomNumber = random.Next(0, 999999);
                    message = CommonUIResource.ResetPassword + randomNumber;
                    getAdminOtp = adminDL.GetAdminOtp(adminML.Id);
                    if (getAdminOtp.AdminId > 0)
                    {
                        mailResult = SendOtpMail(message, emailId);
                        if (mailResult == "success")
                        {
                            adminOtp.AdminId = adminML.Id;
                            adminOtp.Mobile = adminML.Mobile;
                            adminOtp.OtpValue = randomNumber;
                            result = adminDL.SaveAdminOtp(adminOtp);
                            if (result == "success")
                            {
                                ViewBag.ShowOtpDiv = adminML.Id;
                            }
                        }
                    }
                    else
                    {
                        mailResult = SendOtpMail(message, emailId);
                        if (mailResult == "success")
                        {
                            adminOtp.AdminId = adminML.Id;
                            adminOtp.Mobile = adminML.Mobile;
                            adminOtp.OtpValue = randomNumber;
                            result = adminDL.SaveAdminOtp(adminOtp);
                            if (result == "success")
                            {
                                ViewBag.ShowOtpDiv = adminML.Id;
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
                        adminOtpML = adminDL.GetAdminOtp(Id);
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
            string result = string.Empty;
            int adminId = 0;
            try
            {
                if (!string.IsNullOrEmpty(passWord) && !string.IsNullOrEmpty(hdnAdminId))
                {
                    adminId = Convert.ToInt32(hdnAdminId);
                    result = adminDL.ChangeAdminPassword(adminId, passWord);
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


        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
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

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
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

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
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

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult AdminChangePassword()
        {
            return View();
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminChangePassword(string passWord)
        {
            string result = string.Empty;
            int adminId = 0;
            try
            {
                if (!string.IsNullOrEmpty(passWord))
                {
                    adminId = Convert.ToInt32(Session["AdminId"]);
                    result = adminDL.ChangeAdminPassword(adminId, passWord);
                    if (result == "success")
                    {
                        ViewBag.AdminPasswordChangedSuccessfully = "Password Changed Successfully";
                    }
                    else
                    {
                        ViewBag.AdminPasswordChangedError = "An error occured";
                    }
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return View("AdminChangePassword");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult UpdateAdminDetails()
        {
            AdminML adminML = new AdminML();
            try
            {
                adminML = adminDL.GetAdminDetails();
                return View("UpdateAdminDetails", adminML);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpPost]

        public JsonResult UpdateAdminDetails(AdminML adminML)
        {
            string result = string.Empty;
            try
            {
                if (adminML != null)
                {
                    result = adminDL.UpdateAdminDetails(adminML);
                }
            }
            catch (Exception ex)
            {
                result = "error";
                error = ex.Message;
            }
            return Json(result);
        }

        //     [NoDirectAccess]
        public ActionResult Test()
        {
            return View();
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult Dashboard()
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            int? totalVoiceCall = 0;
            int? totalMessageSent = 0;
            int adminOtp = 0;
            int userOtp = 0;
            int? messageSent = 0;
            try
            {
                totalVoiceCall = commonDL.TotalAudioCall();
                messageSent = commonDL.TotalSentMessage();
                adminOtp = commonDL.AdminOtpCount();
                userOtp = commonDL.UserOtpCount();
                totalMessageSent = messageSent + adminOtp + userOtp;
                adminDashboard.TotalVoiceCall = totalVoiceCall;
                adminDashboard.TotalMessage = totalMessageSent;
                adminDashboard.TotalVideoSpace = commonDL.GetVideoSpace();
                return View(adminDashboard);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }


        [HttpPost]
        public JsonResult GetArea(string blockName, string blockId)
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

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult CreateUser()
        {
            CreateUserVM createUserVM = new CreateUserVM();
            createUserVM.BlockML = adminDL.GetBlock();
            string blockName = createUserVM.BlockML[0].BlockName;
            int blockId = createUserVM.BlockML[0].Id;
            createUserVM.AreaML = adminDL.GetArea(blockName);
            return View(createUserVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
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
            userId = adminDL.SaveUser(userML);
            return Json(userId);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult UploadFiles()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
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

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult ExportAllMobileNumber()
        {
            List<string> lstMobile = new List<string>();
            lstMobile = adminDL.GetAllUsersMobile();
            try
            {
                if (lstMobile.Count > 0)
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        excel.Workbook.Worksheets.Add("Sheet1");
                        var worksheet = excel.Workbook.Worksheets["Sheet1"];
                        worksheet.Cells["A1"].LoadFromCollection<string>(lstMobile);
                        var stream = new MemoryStream(excel.GetAsByteArray());
                        string excelName = "MobileList.xlsx";
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return RedirectToAction("SearchByBlockArea");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult ExportAllMobileNumberByBlock(int id)
        {
            string block = Request.Form["BlockName"];
            string block1 = Request.QueryString["BlockName"];
            List<string> lstMobile = new List<string>();
            if (id > 0)
            {
                lstMobile = adminDL.GetAllUsersMobileByBlock(id);
                try
                {
                    if (lstMobile.Count > 0)
                    {
                        using (ExcelPackage excel = new ExcelPackage())
                        {
                            excel.Workbook.Worksheets.Add("Sheet1");
                            var worksheet = excel.Workbook.Worksheets["Sheet1"];
                            worksheet.Cells["A1"].LoadFromCollection<string>(lstMobile);
                            var stream = new MemoryStream(excel.GetAsByteArray());
                            string excelName = "MobileList.xlsx";
                            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            return RedirectToAction("SearchByBlockArea");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult SearchByBlockArea(int page = 1)
        {
            int delhiBlockId = 0;
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["SearchByBlockAreaTake"]);
            int skip = (page - 1) * take;
            string blockName = (Request.QueryString["BlockName"] != null) ? Convert.ToString(Request.QueryString["BlockName"]) : string.Empty;
            string areaName = (Request.QueryString["AreaName"] != null) ? Convert.ToString(Request.QueryString["AreaName"]) : string.Empty;
            string sort = (Request.QueryString["sort"] != null) ? Convert.ToString(Request.QueryString["sort"]) : string.Empty;
            string sortDir = (Request.QueryString["sortdir"] != null) ? Convert.ToString(Request.QueryString["sortdir"]) : string.Empty;

            CreateUserVM createUserVM = new CreateUserVM();
            if (!string.IsNullOrEmpty(blockName))
            {
                createUserVM.PageSize = take;
                createUserVM.TotalRows = adminDL.GetUserByBlockAreaCount(blockName, areaName);
                createUserVM.ListUserML = adminDL.GetUserByBlockArea(blockName, areaName, skip, take, sort, sortDir);
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
                delhiBlockId = createUserVM.BlockML[0].Id;
                createUserVM.AreaML = adminDL.GetArea(defaultBlockName);
                string url = Request.Url.AbsoluteUri;
                if (url.Contains("BlockName") && url.Contains("AreaName"))
                {
                    ViewBag.ErrorMessage = "Please select block to search";
                }


            }
            return View(createUserVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult GetVotersDetails(int page = 1)
        {
            int delhiBlockId = 0;
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["SearchByBlockAreaTake"]);
            int skip = (page - 1) * take;
            string blockName = (Request.QueryString["BlockName"] != null) ? Convert.ToString(Request.QueryString["BlockName"]) : string.Empty;
            string areaName = (Request.QueryString["AreaName"] != null) ? Convert.ToString(Request.QueryString["AreaName"]) : string.Empty;

            CreateUserVM createUserVM = new CreateUserVM();
            if (!string.IsNullOrEmpty(blockName))
            {
                createUserVM.PageSize = take;
                createUserVM.TotalRows = adminDL.GetVotersDetailsCount(blockName, areaName);
                createUserVM.VidhanSabhaVotersML = adminDL.GetVotersDetailsByBlockArea(blockName, areaName, skip, take);
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
                delhiBlockId = createUserVM.BlockML[0].Id;
                createUserVM.AreaML = adminDL.GetArea(defaultBlockName);
                string url = Request.Url.AbsoluteUri;
                if (url.Contains("BlockName") && url.Contains("AreaName"))
                {
                    ViewBag.ErrorMessage = "Please select block to search";
                }
            }
            return View(createUserVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeleteVoterDetails(int id)
        {
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeleteVoterDetails(id);
                    if (result == "success")
                    {
                        TempData["VoterDeleted"] = "Voter deleted successfuly";
                    }
                    else
                    {
                        TempData["VoterDeletedError"] = "Voter deleted error";
                    }
                    return RedirectToAction("GetVotersDetails");
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult ExportAllVoterMobileNumber()
        {
            List<string> lstMobile = new List<string>();
            lstMobile = adminDL.GetAllVoterUsersMobile();
            try
            {
                if (lstMobile.Count > 0)
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        excel.Workbook.Worksheets.Add("Sheet1");
                        var worksheet = excel.Workbook.Worksheets["Sheet1"];
                        worksheet.Cells["A1"].LoadFromCollection<string>(lstMobile);
                        var stream = new MemoryStream(excel.GetAsByteArray());
                        string excelName = "MobileList.xlsx";
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return RedirectToAction("GetVotersDetails");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult ExportAllVoterMobileNumberByBlock(int id)
        {
            string block = Request.Form["BlockName"];
            string block1 = Request.QueryString["BlockName"];
            List<string> lstMobile = new List<string>();
            if (id > 0)
            {
                lstMobile = adminDL.GetAllVoterUsersMobileByBlock(id);
                try
                {
                    if (lstMobile.Count > 0)
                    {
                        using (ExcelPackage excel = new ExcelPackage())
                        {
                            excel.Workbook.Worksheets.Add("Sheet1");
                            var worksheet = excel.Workbook.Worksheets["Sheet1"];
                            worksheet.Cells["A1"].LoadFromCollection<string>(lstMobile);
                            var stream = new MemoryStream(excel.GetAsByteArray());
                            string excelName = "MobileList.xlsx";
                            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            return RedirectToAction("GetVotersDetails");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult SearchByUserName(int page = 1)
        {
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["SearchByBlockAreaTake"]);
            int skip = (page - 1) * take;
            string userName = (Request.QueryString["userName"] != null) ? Convert.ToString(Request.QueryString["userName"]) : string.Empty;
            string sort = (Request.QueryString["sort"] != null) ? Convert.ToString(Request.QueryString["sort"]) : string.Empty;
            string sortDir = (Request.QueryString["sortdir"] != null) ? Convert.ToString(Request.QueryString["sortdir"]) : string.Empty;

            CreateUserVM createUserVM = new CreateUserVM();
            if (!string.IsNullOrEmpty(userName))
            {
                createUserVM.PageSize = take;
                createUserVM.TotalRows = adminDL.GetUserByUserNameCount(userName);
                createUserVM.ListUserML = adminDL.GetUserByUserName(userName, skip, take, sort, sortDir);
                if (createUserVM.ListUserML.Count == 0)
                {
                    ViewBag.NoRecordFound = "No record found";
                }

            }
            return View(createUserVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult SearchVotingSurveyByBlockArea(int page = 1)
        {
            int delhiBlockId = 0;
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["SearchVotingSurvey"]);
            int skip = (page - 1) * take;
            string blockName = (Request.QueryString["BlockName"] != null) ? Convert.ToString(Request.QueryString["BlockName"]) : string.Empty;
            string areaName = (Request.QueryString["AreaName"] != null) ? Convert.ToString(Request.QueryString["AreaName"]) : string.Empty;

            CreateUserVM createUserVM = new CreateUserVM();
            if (!string.IsNullOrEmpty(blockName))
            {
                createUserVM.PageSize = take;
                createUserVM.TotalRows = adminDL.GetVotingSurveyCount(blockName, areaName);
                createUserVM.VotingSurveyML = adminDL.GetVotingSurveyByBlockArea(blockName, areaName, skip, take);
                int blockId = Convert.ToInt32(blockName);
                createUserVM.BlockML = adminDL.GetBlock();
                string blockNameOfId = commonDL.GetBlockById(blockId);
                createUserVM.AreaML = adminDL.GetArea(blockNameOfId);
                // Here BlockName and AreaName contains id value
                createUserVM.BlockName = blockName;
                createUserVM.AreaName = areaName;
                createUserVM.TotalVoterSurveyCountOfBlock = adminDL.GetVotingSurveyCountByBlock(blockName);
                createUserVM.BlockNameForDisplay = blockNameOfId;
                ViewBag.DisplayBlockCount = "Display";
                createUserVM.TotalVoterSurveyCount = adminDL.GetTotalVotingSurveyCount();
            }
            else
            {
                createUserVM.TotalVoterSurveyCount = adminDL.GetTotalVotingSurveyCount();
                createUserVM.BlockML = adminDL.GetBlock();
                string defaultBlockName = createUserVM.BlockML[0].BlockName;
                delhiBlockId = createUserVM.BlockML[0].Id;
                createUserVM.AreaML = adminDL.GetArea(defaultBlockName);
                string url = Request.Url.AbsoluteUri;
                if (url.Contains("BlockName") && url.Contains("AreaName"))
                {
                    ViewBag.ErrorMessage = "Please select block to search";
                }
            }
            return View(createUserVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult EditUser(int id)
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
                    createUserVM.Image = userML.Image;
                    createUserVM.AbsolutePath = Constant.AbsalutePath;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View("CreateUser", createUserVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
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

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeactivateActiveUser(int id)
        {
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeActivateActiveUser(id);
                    if (result == "success")
                    {
                        TempData["DeActivateUser"] = "User deactivated successfully";
                        return RedirectToAction("SearchByBlockArea");
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }


        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeleteUser(int id)
        {
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeleteUser(id);
                    if (result == "success")
                    {
                        TempData["UserDeleted"] = "User deleted successfuly";
                    }
                    else
                    {
                        TempData["UserDeletedError"] = "User deleted error";
                    }
                    return RedirectToAction("SearchByBlockArea");
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index");
        }


        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult AllUserMessage(int page = 1)
        {
            AdminMessageVM adminMessageVM = new AdminMessageVM();
            List<UserMessageML> lstUserMessageML = new List<UserMessageML>();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["AllUserMessageTake"]);
            int skip = (page - 1) * take;
            try
            {
                adminMessageVM.PageSize = take;
                adminMessageVM.TotalRows = adminDL.GetAllUserMessageCount();
                lstUserMessageML = adminDL.GetAllUserMessage(skip, take);
                adminMessageVM.UserMessageML = lstUserMessageML;

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(adminMessageVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult Details(int id)
        {
            TempData["AdminId"] = id;
            return RedirectToAction("MessageDetails");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult MessageDetails()
        {
            UserMessageML userMessageML = new UserMessageML();
            int messageId = Convert.ToInt32(TempData["AdminId"]);
            try
            {
                userMessageML = adminDL.GetMessageDetails(messageId);
                if (userMessageML != null)
                {
                    userMessageML.AttachmentPath = Constant.AbsalutePath + CommonUIResource.FolderPath + "/" + userMessageML.FileName;
                    userMessageML.ReplyFilePath = Constant.AbsalutePath + CommonUIResource.FolderPath + "/" + userMessageML.ReplyFileName;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(userMessageML);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MessageDetails(HttpPostedFileBase ReplyFile, string txtSubject, string txtMessagebody, string txtReplyMessagebody, string hdnId)
        {
            UserMessageML userMessageML = new UserMessageML();
            string dbFilePath = string.Empty;
            string fileName = string.Empty;
            string result = string.Empty;
            int messageId = Convert.ToInt32(hdnId);
            try
            {
                if (ReplyFile != null)
                {
                    if (ReplyFile.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(ReplyFile.FileName);
                        long dateValue = DateTime.Now.ToFileTime();
                        string[] arrName = fileName.Split('.');
                        if (arrName.Length == 2)
                        {
                            fileName = arrName[0] + dateValue + "." + arrName[1];
                            dbFilePath = fileName;
                        }
                        string path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        ReplyFile.SaveAs(path);
                    }
                }
                if (!string.IsNullOrEmpty(txtSubject) && !string.IsNullOrEmpty(txtMessagebody) && !string.IsNullOrEmpty(txtReplyMessagebody))
                {
                    userMessageML.Id = messageId;
                    userMessageML.MessageSubject = txtSubject;
                    userMessageML.MessageBody = txtMessagebody;
                    userMessageML.ReplyFileName = dbFilePath;
                    userMessageML.ReplyMessage = txtReplyMessagebody;
                    userMessageML.FileName = fileName;
                    result = adminDL.UpdateMessageDetails(userMessageML);
                    if (result == "success")
                    {
                        TempData["ReplyDetailsSaved"] = "Reply details saved successfully";
                    }
                    else
                    {
                        TempData["ReplyDetailsError"] = "Reply details error occured";
                    }
                    return RedirectToAction("AllUserMessage");
                }
                else
                {
                    return View("_AdminError");
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeleteMessage(int id)
        {
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeleteMessage(id);
                    if (result == "success")
                    {
                        TempData["UserMessageDeleted"] = "User message deleted successfuly";
                    }
                    else
                    {
                        TempData["UserMessageDeletedError"] = "User message deleted error";
                    }
                    return RedirectToAction("AllUserMessage");
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult SendMessageByAdmin()
        {
            AdminMessageVM adminMessageVM = new AdminMessageVM();
            adminMessageVM.BlockML = adminDL.GetBlock();
            return View(adminMessageVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SendMessageByAdmin(HttpPostedFileBase postedFile, string chkAllUser, string BlockName, string AdminMessage, string MessageSubject)
        {
            string dbImagePath = string.Empty;
            string fileName = string.Empty;
            string result = string.Empty;
            string path = string.Empty;
            List<UserML> lstUserML = new List<UserML>();
            UserMessageML userMessageML = new UserMessageML();
            int count = 0;
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
                        path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        postedFile.SaveAs(path);
                    }
                }
                if (!string.IsNullOrEmpty(chkAllUser) && chkAllUser.Equals("on"))
                {
                    lstUserML = commonDL.GetAllUser();
                }
                else
                {
                    int blockId = 0;
                    if (!string.IsNullOrEmpty(BlockName))
                    {
                        blockId = Convert.ToInt32(BlockName);
                    }
                    lstUserML = commonDL.GetAllUser(blockId);
                }
                if (!string.IsNullOrEmpty(AdminMessage) && !string.IsNullOrEmpty(MessageSubject))
                {
                    foreach (var user in lstUserML)
                    {
                        userMessageML = new UserMessageML();
                        userMessageML.MessageBody = AdminMessage;
                        userMessageML.MessageSubject = MessageSubject;
                        userMessageML.UserId = user.Id;
                        userMessageML.UserName = user.UserName;
                        userMessageML.FileName = dbImagePath;
                        result = adminDL.SaveAdminMessage(userMessageML);
                        count++;
                    }
                }
                if (count > 0)
                {
                    TempData["MessageSaved"] = "Message saved successfully";
                }
                else
                {
                    TempData["MessageError"] = "Error occured";
                }
                return RedirectToAction("SendMessageByAdmin");
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult SentMessageByAdmin(int page = 1)
        {
            AdminMessageVM adminMessageVM = new AdminMessageVM();
            List<UserMessageML> lstUserMessageML = new List<UserMessageML>();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["AllUserMessageTake"]);
            int skip = (page - 1) * take;
            try
            {
                adminMessageVM.PageSize = take;
                adminMessageVM.TotalRows = adminDL.GetSentMessageByAdminCount();
                lstUserMessageML = adminDL.GetSentMessageByAdmin(skip, take);
                adminMessageVM.UserMessageML = lstUserMessageML;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(adminMessageVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult AdminSentMessageDetails(int id)
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

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeleteAdminSentMessage(int id)
        {
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeleteAdminSentMessage(id);
                    if (result == "success")
                    {
                        TempData["SentMessageDeleted"] = "Message deleted successfully";
                    }
                    else
                    {
                        TempData["SentMessageError"] = "An error occured";
                    }
                    return RedirectToAction("SentMessageByAdmin");
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult SendSmsByAdmin()
        {
            AdminMessageVM adminMessageVM = new AdminMessageVM();
            adminMessageVM.BlockML = adminDL.GetBlock();
            return View(adminMessageVM);
        }

        // Here BlockName contains id value
        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SendSmsByAdmin(string chkAllUser, string BlockName, string AdminMessage)
        {
            DeSerializeMessageResult messageResult;
            List<UserML> lstUserML = new List<UserML>();
            List<string> lstMobiles = new List<string>();
            RootObject rootObject = new RootObject();
            Sm sm = new Sm();
            List<Sm> lstSm = new List<Sm>();
            string postJson = string.Empty;
            string messageApi = Convert.ToString(ConfigurationManager.AppSettings["MessageApi"]);
            string postMessageApi = Convert.ToString(ConfigurationManager.AppSettings["PostMessageApi"]);
            string country = Convert.ToString(ConfigurationManager.AppSettings["country"]);
            string sender = Convert.ToString(ConfigurationManager.AppSettings["sender"]);
            string route = Convert.ToString(ConfigurationManager.AppSettings["route"]);
            string authkey = Convert.ToString(ConfigurationManager.AppSettings["authkey"]);
            string uniCode = "1";
            string mobiles = string.Empty;
            string message = string.Empty;
            string restMessageUrl = string.Empty;
            string result = string.Empty;
            SentSmsDetailML sentSmsDetailML = new SentSmsDetailML();
            UserML userML;
            int messageLength = 0;
            try
            {
                if (!string.IsNullOrEmpty(chkAllUser) && chkAllUser.Equals("on"))
                {
                    lstUserML = commonDL.GetAllUser();
                    sentSmsDetailML.AllUser = "All User";
                    sentSmsDetailML.BlockId = null;
                }
                else
                {
                    int blockId = 0;
                    if (!string.IsNullOrEmpty(BlockName))
                    {
                        blockId = Convert.ToInt32(BlockName);
                    }
                    lstUserML = commonDL.GetAllUser(blockId);
                    sentSmsDetailML.AllUser = null;
                    sentSmsDetailML.BlockId = BlockName;
                }

                foreach (var user in lstUserML)
                {
                    mobiles = "91" + user.Mobile;
                    lstMobiles.Add(mobiles);
                }

                // To send large number of sms -- use post
                if (lstMobiles != null && lstMobiles.Count > 0 && !string.IsNullOrEmpty(AdminMessage))
                {
                    messageLength = AdminMessage.Length;
                    if (messageLength <= 2400)
                    {
                        AdminMessage = AdminMessage.Replace("\"", "\'");
                        var smsClient = new RestClient(postMessageApi);
                        var request = new RestRequest(Method.POST);
                        request.RequestFormat = DataFormat.Json;
                        request.AddHeader("content-type", "application/json");
                        request.AddHeader("authkey", authkey);
                        sm.message = AdminMessage;
                        sm.to = lstMobiles;
                        lstSm.Add(sm);
                        rootObject.sender = sender;
                        rootObject.route = route;
                        rootObject.country = country;
                        rootObject.sms = lstSm;
                        rootObject.unicode = "1";
                        postJson = JsonConvert.SerializeObject(rootObject);
                        request.AddParameter("application/json", postJson, ParameterType.RequestBody);
                        IRestResponse response = smsClient.Execute(request);
                        if (response.Content != null)
                        {
                            messageResult = new DeSerializeMessageResult();
                            messageResult = JsonConvert.DeserializeObject<DeSerializeMessageResult>(response.Content);
                            if (messageResult != null)
                            {
                                if (messageResult.type == "success")
                                {
                                    TempData["MessageSent"] = "Message sent successfully";
                                    sentSmsDetailML.SentSms = AdminMessage;
                                    sentSmsDetailML.TotalSentNumber = lstMobiles.Count;
                                    result = adminDL.SaveSentMessageDetails(sentSmsDetailML);
                                    return RedirectToAction("SendSmsByAdmin");
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult SendSmsByAdminToVoters()
        {
            AdminMessageVM adminMessageVM = new AdminMessageVM();
            adminMessageVM.BlockML = adminDL.GetBlock();
            return View(adminMessageVM);
        }

        // Here BlockName contains id value
        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SendSmsByAdminToVoters(string chkAllUser, string BlockName, string AdminMessage)
        {
            DeSerializeMessageResult messageResult;
            List<VidhanSabhaVotersML> lstUserML = new List<VidhanSabhaVotersML>();
            List<string> lstMobiles = new List<string>();
            RootObject rootObject = new RootObject();
            Sm sm = new Sm();
            List<Sm> lstSm = new List<Sm>();
            string postJson = string.Empty;
            string messageApi = Convert.ToString(ConfigurationManager.AppSettings["MessageApi"]);
            string postMessageApi = Convert.ToString(ConfigurationManager.AppSettings["PostMessageApi"]);
            string country = Convert.ToString(ConfigurationManager.AppSettings["country"]);
            string sender = Convert.ToString(ConfigurationManager.AppSettings["sender"]);
            string route = Convert.ToString(ConfigurationManager.AppSettings["route"]);
            string authkey = Convert.ToString(ConfigurationManager.AppSettings["authkey"]);
            string uniCode = "1";
            string mobiles = string.Empty;
            string message = string.Empty;
            string restMessageUrl = string.Empty;
            string result = string.Empty;
            SentSmsDetailML sentSmsDetailML = new SentSmsDetailML();
            VidhanSabhaVotersML userML;
            int messageLength = 0;
            try
            {
                if (!string.IsNullOrEmpty(chkAllUser) && chkAllUser.Equals("on"))
                {
                    lstUserML = commonDL.GetAllVoters();
                    sentSmsDetailML.AllUser = "All User";
                    sentSmsDetailML.BlockId = null;
                }
                else
                {
                    int blockId = 0;
                    if (!string.IsNullOrEmpty(BlockName))
                    {
                        blockId = Convert.ToInt32(BlockName);
                    }
                    lstUserML = commonDL.GetAllVoters(blockId);
                    sentSmsDetailML.AllUser = null;
                    sentSmsDetailML.BlockId = BlockName;
                }

                foreach (var user in lstUserML)
                {
                    mobiles = "91" + user.Mobile;
                    lstMobiles.Add(mobiles);
                }

                // To send large number of sms -- use post
                if (lstMobiles != null && lstMobiles.Count > 0 && !string.IsNullOrEmpty(AdminMessage))
                {
                    messageLength = AdminMessage.Length;
                    if (messageLength <= 2400)
                    {
                        AdminMessage = AdminMessage.Replace("\"", "\'");
                        var smsClient = new RestClient(postMessageApi);
                        var request = new RestRequest(Method.POST);
                        request.RequestFormat = DataFormat.Json;
                        request.AddHeader("content-type", "application/json");
                        request.AddHeader("authkey", authkey);
                        sm.message = AdminMessage;
                        sm.to = lstMobiles;
                        lstSm.Add(sm);
                        rootObject.sender = sender;
                        rootObject.route = route;
                        rootObject.country = country;
                        rootObject.sms = lstSm;
                        rootObject.unicode = "1";
                        postJson = JsonConvert.SerializeObject(rootObject);
                        request.AddParameter("application/json", postJson, ParameterType.RequestBody);
                        IRestResponse response = smsClient.Execute(request);
                        if (response.Content != null)
                        {
                            messageResult = new DeSerializeMessageResult();
                            messageResult = JsonConvert.DeserializeObject<DeSerializeMessageResult>(response.Content);
                            if (messageResult != null)
                            {
                                if (messageResult.type == "success")
                                {
                                    TempData["MessageSent"] = "Message sent successfully";
                                    sentSmsDetailML.SentSms = AdminMessage;
                                    sentSmsDetailML.TotalSentNumber = lstMobiles.Count;
                                    sentSmsDetailML.CreatedBy = Convert.ToString(Session["AdminUserName"]);
                                    sentSmsDetailML.ModifyBy = Convert.ToString(Session["AdminUserName"]);
                                    result = adminDL.SaveSentMessageDetails(sentSmsDetailML);
                                    return RedirectToAction("SendSmsByAdminToVoters");
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }


        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult SendVoiceCallByAdmin()
        {
            AdminMessageVM adminMessageVM = new AdminMessageVM();
            adminMessageVM.BlockML = adminDL.GetBlock();
            return View(adminMessageVM);
        }

        // Here BlockName contains Id value
        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SendVoiceCallByAdmin(HttpPostedFileBase postedFile, string chkAllUser, string BlockName)
        {
            VoiceCallDetailsML voiceCallDetailsML;
            string fileName = string.Empty;
            string dbFilePath = string.Empty;
            string path = string.Empty;
            string allUser = string.Empty;
            string blockName = string.Empty;
            string voiceDetailsSaveResult = string.Empty;

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
                        dbFilePath = fileName;
                    }
                    path = Path.Combine(Server.MapPath("~/AdminAudio"), fileName);
                    postedFile.SaveAs(path);
                }
            }
            if (!string.IsNullOrEmpty(chkAllUser))
            {
                allUser = "All User";
                blockName = null;
            }
            else
            {
                blockName = BlockName;
                allUser = null;
            }
            Mp3FileReader mp3Reader = new Mp3FileReader(path);
            TimeSpan audioSpan = mp3Reader.TotalTime;
            int hour = audioSpan.Hours;
            int minutes = audioSpan.Minutes;
            int seconds = audioSpan.Seconds;

            List<UserML> lstUserML = new List<UserML>();
            UserML userML;
            VoiceCallDeSerialize voiceCallDeSerialize = new VoiceCallDeSerialize();
            string mobiles = string.Empty;
            int voiceCallRepeatCondition = Convert.ToInt32(ConfigurationManager.AppSettings["VoiceCallRepeatCondition"]);
            int voiceCallValueLimit = Convert.ToInt32(ConfigurationManager.AppSettings["VoiceCallValueLimit"]);
            string voiceCallUrl = Convert.ToString(ConfigurationManager.AppSettings["voiceCallUrl"]);
            string userId = Convert.ToString(ConfigurationManager.AppSettings["userId"]);
            string passWord = Convert.ToString(ConfigurationManager.AppSettings["password"]);
            string audioType = Convert.ToString(ConfigurationManager.AppSettings["audioType"]);
            string sendMethod = Convert.ToString(ConfigurationManager.AppSettings["sendMethod"]);
            string duplicateCheck = Convert.ToString(ConfigurationManager.AppSettings["duplicateCheck"]);
            string reDial = Convert.ToString(ConfigurationManager.AppSettings["reDial"]);
            string redialInterval = Convert.ToString(ConfigurationManager.AppSettings["redialInterval"]);
            string format = Convert.ToString(ConfigurationManager.AppSettings["format"]);
            string audioFileType = Convert.ToString(ConfigurationManager.AppSettings["audioFileType"]);
            int maximumAudioMinute = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumAudioCallMinute"]);
            if (hour == 0 && minutes <= maximumAudioMinute)
            {
                try
                {
                    if (!string.IsNullOrEmpty(chkAllUser) && chkAllUser.Equals("on"))
                    {
                        lstUserML = commonDL.GetAllUser();
                    }
                    else
                    {
                        int blockId = 0;
                        if (!string.IsNullOrEmpty(BlockName))
                        {
                            blockId = Convert.ToInt32(BlockName);
                        }
                        lstUserML = commonDL.GetAllUser(blockId);
                    }
                    if (lstUserML.Count > 0)
                    {
                        byte[] Bytes = new byte[postedFile.InputStream.Length + 1];
                        postedFile.InputStream.Read(Bytes, 0, Bytes.Length);
                        var client = new RestClient(voiceCallUrl);
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("cache-control", "no-cache");
                        request.AddHeader("content-type", "multipart/form-data;");
                        request.AddParameter("userId", userId);
                        request.AddParameter("password", passWord);
                        request.AddParameter("audioType", audioType);
                        request.AddParameter("sendMethod", sendMethod);
                        request.AddParameter("duplicateCheck", duplicateCheck);
                        request.AddFile("audioTrack", Bytes, postedFile.FileName, audioFileType);
                        request.AddParameter("reDial", reDial);
                        request.AddParameter("redialInterval", redialInterval);
                        request.AddParameter("format", format);
                        if (lstUserML.Count > voiceCallValueLimit)
                        {
                            voiceCallDetailsML = new VoiceCallDetailsML();
                            voiceCallDetailsML.AllUser = allUser;
                            voiceCallDetailsML.BlockId = blockName;
                            voiceCallDetailsML.FileName = fileName;
                            voiceCallDetailsML.Hour = hour;
                            voiceCallDetailsML.Minutes = minutes;
                            voiceCallDetailsML.Seconds = seconds;
                            int totalUser = lstUserML.Count;
                            int repeatInitialValue = 0;
                            int repeatCondition = voiceCallRepeatCondition;
                            while (totalUser > voiceCallValueLimit)
                            {
                                for (int i = repeatInitialValue; i <= repeatCondition; i++)
                                {
                                    mobiles = mobiles + "," + "91" + lstUserML[i].Mobile;
                                }
                                if (!string.IsNullOrEmpty(mobiles))
                                {
                                    mobiles = mobiles.Substring(1);
                                }
                                request.AddParameter("mobile", mobiles);
                                IRestResponse response = client.Execute(request);
                                if (!string.IsNullOrEmpty(response.Content))
                                {
                                    voiceCallDeSerialize = new VoiceCallDeSerialize();
                                    voiceCallDeSerialize = JsonConvert.DeserializeObject<VoiceCallDeSerialize>(response.Content);
                                }
                                voiceCallDetailsML.TotalSentNumber = voiceCallValueLimit;
                                if (voiceCallDeSerialize.status == "success")
                                {
                                    voiceDetailsSaveResult = adminDL.SaveVoiceCallDetails(voiceCallDetailsML);
                                }

                                mobiles = null;
                                totalUser = totalUser - voiceCallValueLimit;
                                if (totalUser > voiceCallValueLimit)
                                {
                                    repeatInitialValue = repeatCondition + 1;
                                    repeatCondition = repeatCondition + voiceCallValueLimit;
                                }
                                else
                                {
                                    repeatInitialValue = repeatCondition + 1;
                                    repeatCondition = repeatCondition + totalUser;
                                    for (int i = repeatInitialValue; i <= repeatCondition; i++)
                                    {
                                        mobiles = mobiles + "," + "91" + lstUserML[i].Mobile;
                                    }
                                    if (!string.IsNullOrEmpty(mobiles))
                                    {
                                        mobiles = mobiles.Substring(1);
                                    }
                                    request.AddParameter("mobile", mobiles);
                                    IRestResponse responseSecond = client.Execute(request);
                                    if (!string.IsNullOrEmpty(responseSecond.Content))
                                    {
                                        voiceCallDeSerialize = new VoiceCallDeSerialize();
                                        voiceCallDeSerialize = JsonConvert.DeserializeObject<VoiceCallDeSerialize>(responseSecond.Content);
                                    }
                                    voiceCallDetailsML.TotalSentNumber = totalUser;
                                    if (voiceCallDeSerialize.status == "success")
                                    {
                                        voiceDetailsSaveResult = adminDL.SaveVoiceCallDetails(voiceCallDetailsML);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var user in lstUserML)
                            {
                                mobiles = mobiles + "," + "91" + user.Mobile;
                            }
                            if (!string.IsNullOrEmpty(mobiles))
                            {
                                mobiles = mobiles.Substring(1);
                            }
                            request.AddParameter("mobile", mobiles);
                            IRestResponse response = client.Execute(request);
                            if (!string.IsNullOrEmpty(response.Content))
                            {
                                voiceCallDeSerialize = JsonConvert.DeserializeObject<VoiceCallDeSerialize>(response.Content);
                            }
                            if (voiceCallDeSerialize.status == "success")
                            {
                                voiceCallDetailsML = new VoiceCallDetailsML();
                                voiceCallDetailsML.AllUser = allUser;
                                voiceCallDetailsML.BlockId = blockName;
                                voiceCallDetailsML.FileName = fileName;
                                voiceCallDetailsML.Hour = hour;
                                voiceCallDetailsML.Minutes = minutes;
                                voiceCallDetailsML.Seconds = seconds;
                                voiceCallDetailsML.TotalSentNumber = lstUserML.Count;
                                voiceDetailsSaveResult = adminDL.SaveVoiceCallDetails(voiceCallDetailsML);
                            }
                        }
                    }
                    TempData["VoiceCallSent"] = "Voice call sent successfully";
                    return RedirectToAction("SendVoiceCallByAdmin");
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            else
            {
                TempData["VoiceCallLengthExceeded"] = "Voice call length exceeded";
                return RedirectToAction("SendVoiceCallByAdmin");
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult SendVoiceCallByAdminToVoters()
        {
            AdminMessageVM adminMessageVM = new AdminMessageVM();
            adminMessageVM.BlockML = adminDL.GetBlock();
            return View(adminMessageVM);
        }

        // Here BlockName contains Id value
        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SendVoiceCallByAdminToVoters(HttpPostedFileBase postedFile, string chkAllUser, string BlockName)
        {
            VoiceCallDetailsML voiceCallDetailsML;
            string fileName = string.Empty;
            string dbFilePath = string.Empty;
            string path = string.Empty;
            string allUser = string.Empty;
            string blockName = string.Empty;
            string voiceDetailsSaveResult = string.Empty;

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
                        dbFilePath = fileName;
                    }
                    path = Path.Combine(Server.MapPath("~/AdminAudio"), fileName);
                    postedFile.SaveAs(path);
                }
            }
            if (!string.IsNullOrEmpty(chkAllUser))
            {
                allUser = "All User";
                blockName = null;
            }
            else
            {
                blockName = BlockName;
                allUser = null;
            }
            Mp3FileReader mp3Reader = new Mp3FileReader(path);
            TimeSpan audioSpan = mp3Reader.TotalTime;
            int hour = audioSpan.Hours;
            int minutes = audioSpan.Minutes;
            int seconds = audioSpan.Seconds;

            List<VidhanSabhaVotersML> lstUserML = new List<VidhanSabhaVotersML>();
            VidhanSabhaVotersML userML;
            VoiceCallDeSerialize voiceCallDeSerialize = new VoiceCallDeSerialize();
            string mobiles = string.Empty;
            int voiceCallRepeatCondition = Convert.ToInt32(ConfigurationManager.AppSettings["VoiceCallRepeatCondition"]);
            int voiceCallValueLimit = Convert.ToInt32(ConfigurationManager.AppSettings["VoiceCallValueLimit"]);
            string voiceCallUrl = Convert.ToString(ConfigurationManager.AppSettings["voiceCallUrl"]);
            string userId = Convert.ToString(ConfigurationManager.AppSettings["userId"]);
            string passWord = Convert.ToString(ConfigurationManager.AppSettings["password"]);
            string audioType = Convert.ToString(ConfigurationManager.AppSettings["audioType"]);
            string sendMethod = Convert.ToString(ConfigurationManager.AppSettings["sendMethod"]);
            string duplicateCheck = Convert.ToString(ConfigurationManager.AppSettings["duplicateCheck"]);
            string reDial = Convert.ToString(ConfigurationManager.AppSettings["reDial"]);
            string redialInterval = Convert.ToString(ConfigurationManager.AppSettings["redialInterval"]);
            string format = Convert.ToString(ConfigurationManager.AppSettings["format"]);
            string audioFileType = Convert.ToString(ConfigurationManager.AppSettings["audioFileType"]);
            int maximumAudioMinute = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumAudioCallMinute"]);
            if (hour == 0 && minutes <= maximumAudioMinute)
            {
                try
                {
                    if (!string.IsNullOrEmpty(chkAllUser) && chkAllUser.Equals("on"))
                    {
                        lstUserML = commonDL.GetAllVoters();
                    }
                    else
                    {
                        int blockId = 0;
                        if (!string.IsNullOrEmpty(BlockName))
                        {
                            blockId = Convert.ToInt32(BlockName);
                        }
                        lstUserML = commonDL.GetAllVoters(blockId);
                    }
                    if (lstUserML.Count > 0)
                    {
                        byte[] Bytes = new byte[postedFile.InputStream.Length + 1];
                        postedFile.InputStream.Read(Bytes, 0, Bytes.Length);
                        var client = new RestClient(voiceCallUrl);
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("cache-control", "no-cache");
                        request.AddHeader("content-type", "multipart/form-data;");
                        request.AddParameter("userId", userId);
                        request.AddParameter("password", passWord);
                        request.AddParameter("audioType", audioType);
                        request.AddParameter("sendMethod", sendMethod);
                        request.AddParameter("duplicateCheck", duplicateCheck);
                        request.AddFile("audioTrack", Bytes, postedFile.FileName, audioFileType);
                        request.AddParameter("reDial", reDial);
                        request.AddParameter("redialInterval", redialInterval);
                        request.AddParameter("format", format);
                        if (lstUserML.Count > voiceCallValueLimit)
                        {
                            voiceCallDetailsML = new VoiceCallDetailsML();
                            voiceCallDetailsML.AllUser = allUser;
                            voiceCallDetailsML.BlockId = blockName;
                            voiceCallDetailsML.FileName = fileName;
                            voiceCallDetailsML.Hour = hour;
                            voiceCallDetailsML.Minutes = minutes;
                            voiceCallDetailsML.Seconds = seconds;
                            int totalUser = lstUserML.Count;
                            int repeatInitialValue = 0;
                            int repeatCondition = voiceCallRepeatCondition;
                            while (totalUser > voiceCallValueLimit)
                            {
                                for (int i = repeatInitialValue; i <= repeatCondition; i++)
                                {
                                    mobiles = mobiles + "," + "91" + lstUserML[i].Mobile;
                                }
                                if (!string.IsNullOrEmpty(mobiles))
                                {
                                    mobiles = mobiles.Substring(1);
                                }
                                request.AddParameter("mobile", mobiles);
                                IRestResponse response = client.Execute(request);
                                if (!string.IsNullOrEmpty(response.Content))
                                {
                                    voiceCallDeSerialize = new VoiceCallDeSerialize();
                                    voiceCallDeSerialize = JsonConvert.DeserializeObject<VoiceCallDeSerialize>(response.Content);
                                }
                                voiceCallDetailsML.TotalSentNumber = voiceCallValueLimit;
                                if (voiceCallDeSerialize.status == "success")
                                {
                                    voiceDetailsSaveResult = adminDL.SaveVoiceCallDetails(voiceCallDetailsML);
                                }

                                mobiles = null;
                                totalUser = totalUser - voiceCallValueLimit;
                                if (totalUser > voiceCallValueLimit)
                                {
                                    repeatInitialValue = repeatCondition + 1;
                                    repeatCondition = repeatCondition + voiceCallValueLimit;
                                }
                                else
                                {
                                    repeatInitialValue = repeatCondition + 1;
                                    repeatCondition = repeatCondition + totalUser;
                                    for (int i = repeatInitialValue; i <= repeatCondition; i++)
                                    {
                                        mobiles = mobiles + "," + "91" + lstUserML[i].Mobile;
                                    }
                                    if (!string.IsNullOrEmpty(mobiles))
                                    {
                                        mobiles = mobiles.Substring(1);
                                    }
                                    request.AddParameter("mobile", mobiles);
                                    IRestResponse responseSecond = client.Execute(request);
                                    if (!string.IsNullOrEmpty(responseSecond.Content))
                                    {
                                        voiceCallDeSerialize = new VoiceCallDeSerialize();
                                        voiceCallDeSerialize = JsonConvert.DeserializeObject<VoiceCallDeSerialize>(responseSecond.Content);
                                    }
                                    voiceCallDetailsML.TotalSentNumber = totalUser;
                                    voiceCallDetailsML.CreatedBy = Convert.ToString(Session["AdminUserName"]);
                                    voiceCallDetailsML.ModifyBy = Convert.ToString(Session["AdminUserName"]);
                                    if (voiceCallDeSerialize.status == "success")
                                    {
                                        voiceDetailsSaveResult = adminDL.SaveVoiceCallDetails(voiceCallDetailsML);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var user in lstUserML)
                            {
                                mobiles = mobiles + "," + "91" + user.Mobile;
                            }
                            if (!string.IsNullOrEmpty(mobiles))
                            {
                                mobiles = mobiles.Substring(1);
                            }
                            request.AddParameter("mobile", mobiles);
                            IRestResponse response = client.Execute(request);
                            if (!string.IsNullOrEmpty(response.Content))
                            {
                                voiceCallDeSerialize = JsonConvert.DeserializeObject<VoiceCallDeSerialize>(response.Content);
                            }
                            if (voiceCallDeSerialize.status == "success")
                            {
                                voiceCallDetailsML = new VoiceCallDetailsML();
                                voiceCallDetailsML.AllUser = allUser;
                                voiceCallDetailsML.BlockId = blockName;
                                voiceCallDetailsML.FileName = fileName;
                                voiceCallDetailsML.Hour = hour;
                                voiceCallDetailsML.Minutes = minutes;
                                voiceCallDetailsML.Seconds = seconds;
                                voiceCallDetailsML.TotalSentNumber = lstUserML.Count;
                                voiceCallDetailsML.CreatedBy = Convert.ToString(Session["AdminUserName"]);
                                voiceCallDetailsML.ModifyBy = Convert.ToString(Session["AdminUserName"]);
                                voiceDetailsSaveResult = adminDL.SaveVoiceCallDetails(voiceCallDetailsML);
                            }
                        }
                    }
                    TempData["VoiceCallSent"] = "Voice call sent successfully";
                    return RedirectToAction("SendVoiceCallByAdminToVoters");
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            else
            {
                TempData["VoiceCallLengthExceeded"] = "Voice call length exceeded";
                return RedirectToAction("SendVoiceCallByAdminToVoters");
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult ManageFrontImage()
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            return View(manageUserInterfaceVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageFrontImage(HttpPostedFileBase postedFile)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            FrontEndImageML frontEndImageML = new FrontEndImageML();
            string dbImagePath = string.Empty;
            string fileName = string.Empty;
            string result = string.Empty;
            string path = string.Empty;
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
                        path = Path.Combine(Server.MapPath("~/FrontImage"), fileName);
                        postedFile.SaveAs(path);
                    }
                }
                frontEndImageML.ImageName = dbImagePath;
                result = adminDL.SaveFrontImage(frontEndImageML);
                ViewBag.ResultSuccess = "Image uploaded successfully";
                return View(manageUserInterfaceVM);

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult GetAllFrontImage(int page = 1)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            List<FrontEndImageML> lstFrontEndImageML = new List<FrontEndImageML>();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["FrontEndImage"]);
            int skip = (page - 1) * take;

            try
            {
                manageUserInterfaceVM.PageSize = take;
                manageUserInterfaceVM.TotalRows = adminDL.GetAllFrontImageCount();
                lstFrontEndImageML = adminDL.GetAllFrontImage(skip, take);
                manageUserInterfaceVM.FrontEndImageML = lstFrontEndImageML;
                return View("ManageFrontImage", manageUserInterfaceVM);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeleteFrontImage(int id)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeleteFrontImage(id);
                    if (result == "success")
                    {
                        ViewBag.FrontImageDeleted = "Image deleted successfully";
                    }
                    return View("ManageFrontImage", manageUserInterfaceVM);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult ShowFrontImage(int id)
        {
            string result = string.Empty;
            FrontEndImageML frontEndImageML = new FrontEndImageML();
            try
            {
                if (id > 0)
                {
                    frontEndImageML = adminDL.GetFrontImage(id);
                    frontEndImageML.ImagePath = Constant.AbsalutePath + "/FrontImage/" + frontEndImageML.ImageName;
                    return View("ShowFrontImage", frontEndImageML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult ManageFrontVideo()
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            return View(manageUserInterfaceVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageFrontVideo(HttpPostedFileBase postedFile, string videoDescription)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            FrontEndVideoML frontEndVideoML = new FrontEndVideoML();
            string dbVideoPath = string.Empty;
            string fileName = string.Empty;
            string fileType = string.Empty;
            string result = string.Empty;
            string path = string.Empty;
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
                            dbVideoPath = fileName;
                            fileType = arrName[1];
                        }
                        path = Path.Combine(Server.MapPath("~/AdminVideo"), fileName);
                        postedFile.SaveAs(path);
                    }
                }
                if (!string.IsNullOrEmpty(videoDescription))
                {
                    frontEndVideoML.VideoDescription = videoDescription;
                }
                FileInfo fileInfo = new FileInfo(path);
                long fileLength = fileInfo.Length;
                long firstDivide = fileLength / 1024;
                long mbLength = 0;
                if (firstDivide > 1024)
                {
                    mbLength = firstDivide / 1024;
                }
                else
                {
                    mbLength = 1;
                }
                frontEndVideoML.VideoLength = mbLength;
                frontEndVideoML.VideoName = dbVideoPath;
                frontEndVideoML.VideoType = fileType;
                result = adminDL.SaveFrontVideo(frontEndVideoML);
                ViewBag.ResultSuccess = "Video uploaded successfully";
                return View(manageUserInterfaceVM);

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult GetAllFrontVideo(int page = 1)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            List<FrontEndVideoML> lstFrontEndVideoML = new List<FrontEndVideoML>();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["FrontEndImage"]);
            int skip = (page - 1) * take;
            try
            {
                manageUserInterfaceVM.PageSize = take;
                manageUserInterfaceVM.TotalRows = adminDL.GetAllFrontVideoCount();
                lstFrontEndVideoML = adminDL.GetAllFrontVideo(skip, take);
                manageUserInterfaceVM.FrontEndVideoML = lstFrontEndVideoML;
                return View("ManageFrontVideo", manageUserInterfaceVM);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeleteFrontVideo(int id)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            FrontEndVideoML frontEndVideoML = new FrontEndVideoML();
            string result = string.Empty;
            string path = string.Empty;
            try
            {
                if (id > 0)
                {
                    frontEndVideoML = adminDL.GetFrontVideo(id);
                    if (frontEndVideoML != null)
                    {
                        path = Path.Combine(Server.MapPath("~/AdminVideo"), frontEndVideoML.VideoName);
                        if ((System.IO.File.Exists(path)))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    result = adminDL.DeleteFrontVideo(id);
                    if (result == "success")
                    {
                        ViewBag.FrontVideoDeleted = "Video deleted successfully";
                    }
                    return View("ManageFrontVideo", manageUserInterfaceVM);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult ShowFrontVideo(int id)
        {
            string result = string.Empty;
            FrontEndVideoML frontEndVideoML = new FrontEndVideoML();
            try
            {
                if (id > 0)
                {
                    frontEndVideoML = adminDL.GetFrontVideo(id);
                    frontEndVideoML.VideoPath = Constant.AbsalutePath + "AdminVideo/" + frontEndVideoML.VideoName;
                    frontEndVideoML.VideoType = "video/" + frontEndVideoML.VideoType;
                    return View("ShowFrontVideo", frontEndVideoML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }


        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult ManageHomeScreen()
        {
            return View();
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpPost]
        public JsonResult SaveManageHomeScreen(ManageHomeScreenML homeScreenML)
        {
            ManageHomeScreenML manageHomeScreenML = new ManageHomeScreenML();
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(homeScreenML.Heading))
                {
                    manageHomeScreenML.Heading = homeScreenML.Heading;
                }
                if (!string.IsNullOrEmpty(homeScreenML.ShortDescription))
                {
                    manageHomeScreenML.ShortDescription = homeScreenML.ShortDescription;
                }
                if (!string.IsNullOrEmpty(homeScreenML.LongDescription))
                {
                    manageHomeScreenML.LongDescription = homeScreenML.LongDescription;
                }
                if (!string.IsNullOrEmpty(homeScreenML.UpdateName))
                {
                    manageHomeScreenML.UpdateName = homeScreenML.UpdateName;
                }
                if (!string.IsNullOrEmpty(homeScreenML.AddFirstAddress))
                {
                    if (homeScreenML.AddFirstAddress != "0" && homeScreenML.AddFirstAddress != "1")
                    {
                        manageHomeScreenML.AddFirstAddress = homeScreenML.AddFirstAddress;
                    }

                }
                else
                {
                    manageHomeScreenML.AddFirstAddress = "1";
                }
                if (!string.IsNullOrEmpty(homeScreenML.AddSecondAddress))
                {
                    if (homeScreenML.AddSecondAddress != "0" && homeScreenML.AddSecondAddress != "1")
                    {
                        manageHomeScreenML.AddSecondAddress = homeScreenML.AddSecondAddress;
                    }
                }
                else
                {
                    manageHomeScreenML.AddSecondAddress = "1";
                }
                if (!string.IsNullOrEmpty(homeScreenML.VotingSurveyHeading))
                {
                    if (homeScreenML.VotingSurveyHeading != "0" && homeScreenML.VotingSurveyHeading != "1")
                    {
                        manageHomeScreenML.VotingSurveyHeading = homeScreenML.VotingSurveyHeading;
                    }
                }
                else
                {
                    manageHomeScreenML.VotingSurveyHeading = "1";
                }
                result = adminDL.SaveHomeScreenData(manageHomeScreenML);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return Json(result);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult ManageNews()
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            return View(manageUserInterfaceVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpPost]
        public JsonResult ManageNews(string newsHeadingValue, string newsDescriptionValue)
        {
            string result = string.Empty;
            AdminNewsML adminNewsML = new AdminNewsML();
            try
            {
                adminNewsML.NewsHeading = newsHeadingValue;
                adminNewsML.NewsDescription = newsDescriptionValue;
                result = adminDL.SaveAdminNews(adminNewsML);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(result);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult GetAllNews(int page = 1)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            List<AdminNewsML> lstAdminNewsML = new List<AdminNewsML>();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["AdminNewsTake"]);
            int skip = (page - 1) * take;

            try
            {
                manageUserInterfaceVM.PageSize = take;
                manageUserInterfaceVM.TotalRows = adminDL.GetAllNewsCount();
                lstAdminNewsML = adminDL.GetAllNews(skip, take);
                manageUserInterfaceVM.AdminNewsML = lstAdminNewsML;
                return View("ManageNews", manageUserInterfaceVM);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeleteNews(int id)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeleteNews(id);
                    if (result == "success")
                    {
                        ViewBag.NewsDeleted = "News deleted successfully";
                    }
                    return View("ManageNews", manageUserInterfaceVM);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult ShowAdminNews(int id)
        {
            string result = string.Empty;
            FrontEndImageML frontEndImageML = new FrontEndImageML();
            AdminNewsML adminNewsML = new AdminNewsML();
            try
            {
                if (id > 0)
                {
                    adminNewsML = adminDL.GetNewsById(id);
                    return View("ShowAdminNews", adminNewsML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult UserBankDetails(int id)
        {
            string result = string.Empty;
            UserBankDetailsML userBankDetailsML = new UserBankDetailsML();
            try
            {
                if (id > 0)
                {
                    userBankDetailsML = commonDL.GetUserBankDetails(id);
                    return View("UserBankDetails", userBankDetailsML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult ManageDevelopmentWork()
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            return View(manageUserInterfaceVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpPost]
        public JsonResult ManageDevelopmentWork(string developmentHeadingValue, string developmentDescriptionValue)
        {
            string result = string.Empty;
            AdminDevelopmentWorkML adminDevelopmentWorkML = new AdminDevelopmentWorkML();

            try
            {
                if (!string.IsNullOrEmpty(developmentHeadingValue) && !string.IsNullOrEmpty(developmentDescriptionValue))
                {
                    adminDevelopmentWorkML.DevelopmentHeading = developmentHeadingValue;
                    adminDevelopmentWorkML.DevelopmentDescription = developmentDescriptionValue;
                    result = adminDL.SaveDevelopmentWork(adminDevelopmentWorkML);
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(result);
        }


        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult GetAllDevelopmentWork(int page = 1)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            List<AdminDevelopmentWorkML> lstAdminDevelopmentWorkML = new List<AdminDevelopmentWorkML>();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["AdminNewsTake"]);
            int skip = (page - 1) * take;

            try
            {
                manageUserInterfaceVM.PageSize = take;
                manageUserInterfaceVM.TotalRows = adminDL.GetAllDevelopmentWorkCount();
                lstAdminDevelopmentWorkML = adminDL.GetAllDevelopmentWork(skip, take);
                manageUserInterfaceVM.AdminDevelopmentWorkML = lstAdminDevelopmentWorkML;
                return View("ManageDevelopmentWork", manageUserInterfaceVM);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeleteDevelopmentWork(int id)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeleteDevelopmentWork(id);
                    if (result == "success")
                    {
                        ViewBag.DevWorkDeleted = "Development work deleted successfully";
                    }
                    return View("ManageDevelopmentWork", manageUserInterfaceVM);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }


        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult ShowDevelopmentWork(int id)
        {
            string result = string.Empty;
            AdminDevelopmentWorkML adminDevelopmentWorkML = new AdminDevelopmentWorkML();

            try
            {
                if (id > 0)
                {
                    adminDevelopmentWorkML = adminDL.GetDevelopmentWorkById(id);
                    return View("ShowDevelopmentWork", adminDevelopmentWorkML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult ManageGuestQuery(int page = 1)
        {
            List<GuestQueryML> lstGuestQueryML = new List<GuestQueryML>();
            AdminMessageVM adminMessageVM = new AdminMessageVM();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["AllUserMessageTake"]);
            int skip = (page - 1) * take;
            try
            {
                adminMessageVM.PageSize = take;
                adminMessageVM.TotalRows = adminDL.GetAllGuestQueryCount();
                lstGuestQueryML = adminDL.GetAllGuestQuery(skip, take);
                adminMessageVM.GuestQueryML = lstGuestQueryML;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(adminMessageVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeleteQuery(int id)
        {
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeleteQuery(id);
                    if (result == "success")
                    {
                        TempData["QueryDeleted"] = "Query deleted successfully";
                        return RedirectToAction("ManageGuestQuery");
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult ShowGuestQuery(int id)
        {
            string result = string.Empty;
            GuestQueryML guestQueryML = new GuestQueryML();
            try
            {
                if (id > 0)
                {
                    guestQueryML = adminDL.GetGuestQueryById(id);
                    return View("ShowGuestQuery", guestQueryML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpPost]
        public JsonResult SaveQueryResponse(string queryResponseValue, int queryIdValue)
        {
            string result = string.Empty;
            try
            {

                result = adminDL.SaveQueryResponse(queryResponseValue, queryIdValue);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(result);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult ManageContactMessage(int page = 1)
        {
            List<ContactMessageML> lstContactMessageML = new List<ContactMessageML>();
            AdminMessageVM adminMessageVM = new AdminMessageVM();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["AllUserMessageTake"]);
            int skip = (page - 1) * take;
            try
            {
                adminMessageVM.PageSize = take;
                adminMessageVM.TotalRows = adminDL.GetAllContactMessageCount();
                lstContactMessageML = adminDL.GetAllContactMessage(skip, take);
                adminMessageVM.ContactMessageML = lstContactMessageML;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(adminMessageVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeleteContactMessage(int id)
        {
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeleteContactMessage(id);
                    if (result == "success")
                    {
                        TempData["MessageDeleted"] = "Query deleted successfully";
                        return RedirectToAction("ManageContactMessage");
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult ShowContactMessage(int id)
        {
            string result = string.Empty;
            ContactMessageML contactMessageML = new ContactMessageML();
            try
            {
                if (id > 0)
                {
                    contactMessageML = adminDL.GetContactMessageById(id);
                    return View("ShowContactMessage", contactMessageML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }


        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]
        public ActionResult SearchInActiveUserByBlockArea(int page = 1)
        {
            int delhiBlockId = 0;
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
                createUserVM.TotalRows = adminDL.GetInActiveUserByBlockAreaCount(blockName, areaName);
                createUserVM.ListUserML = adminDL.GetInActiveUserByBlockArea(blockName, areaName, skip, take, sort, sortDir);
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
                delhiBlockId = createUserVM.BlockML[0].Id;
                createUserVM.AreaML = adminDL.GetArea(defaultBlockName);
               
            }
            return View(createUserVM);
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult ActivateUser(int id)
        {
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.ActivateUser(id);
                    if (result == "success")
                    {
                        TempData["UserActivatedSuccessfully"] = "User activated successfully";
                        return RedirectToAction("SearchInActiveUserByBlockArea");
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult ActivateWithSuperUser(int id)
        {
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.ActivateWithSuperUser(id);
                    if (result == "success")
                    {
                        TempData["SuperUserActivatedSuccessfully"] = "User activated successfully";
                        return RedirectToAction("SearchInActiveUserByBlockArea");
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }


        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult DeleteInActivateUser(int id)
        {
            string result = string.Empty;
            try
            {
                if (id > 0)
                {
                    result = adminDL.DeleteInActivateUser(id);
                    if (result == "success")
                    {
                        TempData["UserDeletedSuccessfully"] = "User deleted successfully";
                        return RedirectToAction("SearchInActiveUserByBlockArea");
                    }

                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult GetAllVoiceCallSentDetails(int page = 1)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            List<VoiceCallDetailsML> lstVoiceCallDetailsML = new List<VoiceCallDetailsML>();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["SentCallDetails"]);
            int skip = (page - 1) * take;
            try
            {
                manageUserInterfaceVM.PageSize = take;
                manageUserInterfaceVM.TotalRows = adminDL.GetAllVoiceCallSentDetailsCount();
                lstVoiceCallDetailsML = adminDL.GetAllVoiceCallSentDetails(skip, take);
                manageUserInterfaceVM.VoiceCallDetailsML = lstVoiceCallDetailsML;
                manageUserInterfaceVM.totalVoiceCall = commonDL.TotalAudioCall();
                return View("GetAllVoiceCallSentDetails", manageUserInterfaceVM);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }

        [AdminSessionActionFilter]
        [Authorize(Roles = "AdminFirRl")]
        [HttpGet]

        public ActionResult GetAllMessageSentDetails(int page = 1)
        {
            ManageUserInterfaceVM manageUserInterfaceVM = new ManageUserInterfaceVM();
            List<SentSmsDetailML> lstSentSmsDetailsML = new List<SentSmsDetailML>();
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["SentCallDetails"]);
            int skip = (page - 1) * take;
            try
            {
                manageUserInterfaceVM.PageSize = take;
                manageUserInterfaceVM.TotalRows = adminDL.GetAllSentSmsDetailsCount();
                lstSentSmsDetailsML = adminDL.GetAllSentSmsDetails(skip, take);
                manageUserInterfaceVM.SentSmsDetailML = lstSentSmsDetailsML;
                manageUserInterfaceVM.totalMessageSent = commonDL.TotalSentMessage();
                return View("GetAllMessageSentDetails", manageUserInterfaceVM);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_AdminError");
        }






    }
}