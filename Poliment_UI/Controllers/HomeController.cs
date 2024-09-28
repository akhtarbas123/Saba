using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poliment_UI.Models;
using Poliment_DL.Model;
using Poliment_DL;
using System.Configuration;

namespace Poliment_UI.Controllers
{
    public class HomeController : Controller
    {
        private AdminDL adminDL = new AdminDL();
        private HomeDL homeDL = new HomeDL();
        private CommonDL commonDL = new CommonDL();
        string error = string.Empty;

        // GET: Home
        public ActionResult Index(int id = 1)
        {
            // Test Code -- Start
       //     string myPass = CryptorEngine.Decrypt("jJeZ02aSzr0=", true);
       //     string res = myPass;
            // Test Code -- End

            List<FrontEndImageML> lstFrontEndImageML = new List<FrontEndImageML>();
            HomeVM homeVM = new HomeVM();
            lstFrontEndImageML = commonDL.GetAllFrontImage();
            homeVM.FrontEndImageML = lstFrontEndImageML;
            homeVM.AbsolutePath = Constant.AbsalutePath;
            homeVM.ManageHomeScreenML = adminDL.GetHomeScreenData();
            Session["UpdatedAdminName"] = homeVM.ManageHomeScreenML.UpdateName;
            // News code start
            int take = Convert.ToInt32(ConfigurationManager.AppSettings["AdminNewsFrontEndTake"]);
            int skip = (id - 1) * take;
            ViewBag.PageValue = id;
            homeVM.AdminNewsML = adminDL.GetAllNewsByDate(skip, take);
            // News code end
            return View(homeVM);
        }

        [HttpGet]
        public ActionResult ShowAdminNews(int id)
        {
            string result = string.Empty;
            AdminNewsML adminNewsML = new AdminNewsML();
            try
            {
                if (id > 0)
                {
                    adminNewsML = homeDL.GetNewsById(id);
                    return View("ShowAdminNews", adminNewsML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return View("_HomeError");
        }

        [HttpGet]

        public ActionResult SendQuery()
        {
            CreateUserVM createUserVM = new CreateUserVM();
            HomeVM homeVM = new HomeVM();
            try
            {
                homeVM.BlockML = adminDL.GetBlock();
                string blockName = homeVM.BlockML[0].BlockName;
                int delhiBlockId = homeVM.BlockML[0].Id;
                homeVM.AreaML = adminDL.GetArea(blockName);

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(homeVM);
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

        [HttpPost]
        public JsonResult SaveGuestQuery(GuestQueryML guestQueryML)
        {
            int guestQueryId = 0;
            try
            {
                if (guestQueryML != null)
                {
                    guestQueryId = homeDL.SaveGuestQuery(guestQueryML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(guestQueryId);
        }

        [HttpGet]
        public ActionResult GuestQueryResponse()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GuestQueryResponse(string referenceNumberValue, string fourDigitPinValue)
        {
            GuestQueryML guestQueryML = new GuestQueryML();
            int referenceNumber = 0;
            int fourDigitPin = 0;
            try
            {
                if (!string.IsNullOrEmpty(referenceNumberValue) && !string.IsNullOrEmpty(fourDigitPinValue))
                {
                    referenceNumber = Convert.ToInt32(referenceNumberValue);
                    fourDigitPin = Convert.ToInt32(fourDigitPinValue);
                    guestQueryML = homeDL.GetGuestQueryById(referenceNumber, fourDigitPin);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(guestQueryML);
        }

        [HttpGet]
        public ActionResult DevelopmentWork(int id = 1)
        {
            HomeVM homeVM = new HomeVM();
            try
            {
                // Dev work code start
                homeVM.AbsolutePath = Constant.AbsalutePath;
                int take = Convert.ToInt32(ConfigurationManager.AppSettings["AdminDevelopmentWorkFrontEndTake"]);
                int skip = (id - 1) * take;
                ViewBag.PageValue = id;
                homeVM.AdminDevelopmentWorkML = homeDL.GetAllDevelopmentWorkByDate(skip, take);
                // Dev work code end
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(homeVM);
        }

        [HttpGet]
        public ActionResult Contact()
        {
            HomeVM homeVM = new HomeVM();
            try
            {
                homeVM.ManageHomeScreenML = adminDL.GetHomeScreenData();
                return View("Contact", homeVM);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View("_HomeError");
        }

        [HttpPost]
        public JsonResult Contact(ContactMessageML contactMessageML)
        {
            string result = string.Empty;
            try
            {
                if (contactMessageML != null)
                {
                    result = homeDL.SaveContactMessage(contactMessageML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(result);
        }

        [HttpGet]
        public ActionResult GetFrontVideo(int id = 1)
        {
            HomeVM homeVM = new HomeVM();
            try
            {
                homeVM.AbsolutePath = Constant.AbsalutePath;
                int take = Convert.ToInt32(ConfigurationManager.AppSettings["FrontVideoTake"]);
                int skip = (id - 1) * take;
                ViewBag.PageValue = id;
                homeVM.AbsolutePath = Constant.AbsalutePath;
                homeVM.FrontEndVideoML = homeDL.GetAllVideoByDate(skip, take);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(homeVM);
        }

        [HttpGet]
        public ActionResult VotingSurvey()
        {
            CreateUserVM createUserVM = new CreateUserVM();
            HomeVM homeVM = new HomeVM();
            try
            {
                homeVM.ManageHomeScreenML = adminDL.GetHomeScreenData();
                homeVM.BlockML = adminDL.GetBlock();
                string blockName = homeVM.BlockML[0].BlockName;
                int delhiBlockId = homeVM.BlockML[0].Id;
                homeVM.AreaML = adminDL.GetArea(blockName);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return View(homeVM);
        }

        [HttpPost]
        public JsonResult SaveVotingSurvey(VotingSurveyML votingSurveyML)
        {
            string result = string.Empty;
            int mobileCount = 0;
            try
            {
                if (votingSurveyML != null)
                {
                    mobileCount = commonDL.CheckVotingSurveyMobileCount(votingSurveyML.Mobile);
                    if (mobileCount <= 3)
                    {
                        result = homeDL.SaveVotingSurvey(votingSurveyML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult CheckVotingSurveyMobileCount(string mobileValue)
        {
            int count = 0;
            try
            {
                if (!string.IsNullOrEmpty(mobileValue))
                {
                    count = commonDL.CheckVotingSurveyMobileCount(mobileValue);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                count = 0;
            }
            return Json(count);
        }

        [HttpPost]
        public JsonResult CheckVoterIdExist(string voterIdValue)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(voterIdValue))
                {
                    result = commonDL.CheckVoterId(voterIdValue);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return Json(result);
        }
    }
}