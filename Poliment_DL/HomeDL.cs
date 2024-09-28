using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliment_DL.Model;

namespace Poliment_DL
{
    public class HomeDL
    {
        private PoliEntities poli = new PoliEntities();
        private CommonDL commonDL = new CommonDL();
        string error = string.Empty;

        public AdminNewsML GetNewsById(int id)
        {
            string result = string.Empty;
            AdminNewsML adminNewsML = new AdminNewsML();
            PL_AdminNews plAdminNews = new PL_AdminNews();

            try
            {
                plAdminNews = poli.PL_AdminNews.Single(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
                if (plAdminNews != null)
                {
                    adminNewsML.Id = plAdminNews.Id;
                    adminNewsML.NewsHeading = plAdminNews.NewsHeading;
                    adminNewsML.NewsDescription = plAdminNews.NewsDescription;
                    adminNewsML.IsActive = plAdminNews.IsActive;
                    adminNewsML.IsDeleted = plAdminNews.IsDeleted;
                    adminNewsML.CreatedDate = plAdminNews.CreatedDate;
                    adminNewsML.DisplayCreatedDate = plAdminNews.CreatedDate.Value.ToLongDateString();
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return adminNewsML;
        }

        public int SaveGuestQuery(GuestQueryML guestQueryML)
        {
            PL_GuestQuery plGuestQuery = new PL_GuestQuery();
            string result = string.Empty;
            int guestQueryId = 0;
            try
            {
                plGuestQuery.QueryTypeId = guestQueryML.QueryTypeId;
                plGuestQuery.QueryTypeValue = guestQueryML.QueryTypeValue;
                plGuestQuery.BlockId = guestQueryML.BlockId;
                plGuestQuery.BlockName = guestQueryML.BlockName;
                plGuestQuery.AreaId = guestQueryML.AreaId;
                plGuestQuery.AreaName = guestQueryML.AreaName;
                plGuestQuery.Name = guestQueryML.Name;
                plGuestQuery.Mobile = guestQueryML.Mobile;
                plGuestQuery.Email = guestQueryML.Email;
                plGuestQuery.FourDigitPin = guestQueryML.FourDigitPin;
                plGuestQuery.Address = guestQueryML.Address;
                plGuestQuery.QueryHeading = guestQueryML.QueryHeading;
                plGuestQuery.QueryDescription = guestQueryML.QueryDescription;
                plGuestQuery.IsActive = true;
                plGuestQuery.IsDeleted = false;
                plGuestQuery.CreatedDate = DateTime.Now;
                plGuestQuery.ModifyDate = DateTime.Now;
                poli.PL_GuestQuery.Add(plGuestQuery);
                poli.SaveChanges();
                guestQueryId = plGuestQuery.Id;

            }
            catch (Exception ex)
            {
                guestQueryId = 0;
                error = ex.Message;
            }
            return guestQueryId;
        }

        public GuestQueryML GetGuestQueryById(int guestId, int fourDigitPin)
        {
            PL_GuestQuery plGuestQuery = new PL_GuestQuery();
            GuestQueryML guestQueryML = new GuestQueryML();
            try
            {
                plGuestQuery = poli.PL_GuestQuery.FirstOrDefault(x => x.Id == guestId && x.FourDigitPin == fourDigitPin && x.IsActive == true && x.IsDeleted == false);
                if (plGuestQuery != null)
                {
                    guestQueryML.Id = plGuestQuery.Id;
                    guestQueryML.QueryTypeId = plGuestQuery.QueryTypeId;
                    guestQueryML.QueryTypeValue = plGuestQuery.QueryTypeValue;
                    guestQueryML.BlockId = plGuestQuery.BlockId;
                    guestQueryML.BlockName = plGuestQuery.BlockName;
                    guestQueryML.AreaId = plGuestQuery.AreaId;
                    guestQueryML.AreaName = plGuestQuery.AreaName;
                    guestQueryML.Name = plGuestQuery.Name;
                    guestQueryML.Mobile = plGuestQuery.Mobile;
                    guestQueryML.Email = plGuestQuery.Email;
                    guestQueryML.FourDigitPin = plGuestQuery.FourDigitPin;
                    guestQueryML.Address = plGuestQuery.Address;
                    guestQueryML.QueryHeading = plGuestQuery.QueryHeading;
                    guestQueryML.QueryDescription = plGuestQuery.QueryDescription;
                    guestQueryML.IsActive = plGuestQuery.IsActive;
                    guestQueryML.IsDeleted = plGuestQuery.IsDeleted;
                    guestQueryML.CreatedDate = plGuestQuery.CreatedDate;
                    guestQueryML.DisplayCreatedDate = plGuestQuery.CreatedDate.Value.ToShortDateString();
                    guestQueryML.ModifyDate = plGuestQuery.ModifyDate;
                    guestQueryML.AdminResponseMessage = plGuestQuery.AdminResponseMessage;
                }
                else
                {
                    guestQueryML.ErrorMessage = "Please enter valid credential";
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return guestQueryML;
        }

        public List<AdminDevelopmentWorkML> GetAllDevelopmentWorkByDate(int skip, int take)
        {

            List<PL_AdminDevelopmentWork> lstAdminDevelopmentWorkPL = new List<PL_AdminDevelopmentWork>();
            List<AdminDevelopmentWorkML> lstAdminDevelopmentWorkML = new List<AdminDevelopmentWorkML>();
            AdminDevelopmentWorkML adminDevelopmentWorkML;
            try
            {
                lstAdminDevelopmentWorkPL = poli.PL_AdminDevelopmentWork.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).Skip(skip).Take(take).ToList();
                if (lstAdminDevelopmentWorkPL != null && lstAdminDevelopmentWorkPL.Count > 0)
                {
                    foreach (var item in lstAdminDevelopmentWorkPL)
                    {
                        adminDevelopmentWorkML = new AdminDevelopmentWorkML();
                        adminDevelopmentWorkML.Id = item.Id;
                        adminDevelopmentWorkML.DevelopmentHeading = item.DevelopmentHeading;
                        adminDevelopmentWorkML.DevelopmentDescription = item.DevelopmentDescription;
                        adminDevelopmentWorkML.IsActive = item.IsActive;
                        adminDevelopmentWorkML.IsDeleted = item.IsDeleted;
                        adminDevelopmentWorkML.CreatedDate = item.CreatedDate;
                        adminDevelopmentWorkML.DisplayCreatedDate = item.CreatedDate.Value.ToLongDateString();
                        adminDevelopmentWorkML.ModifyDate = item.ModifyDate;
                        lstAdminDevelopmentWorkML.Add(adminDevelopmentWorkML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstAdminDevelopmentWorkML;
        }

        public string SaveContactMessage(ContactMessageML contactMessageML)
        {
            string result = string.Empty;
            PL_ContactMessage plContactMessage = new PL_ContactMessage();
            try
            {
                plContactMessage.FullName = contactMessageML.FullName;
                plContactMessage.Mobie = contactMessageML.Mobie;
                plContactMessage.Email = contactMessageML.Email;
                plContactMessage.Message = contactMessageML.Message;
                plContactMessage.IsActive = true;
                plContactMessage.IsDeleted = false;
                plContactMessage.CreatedDate = DateTime.Now;
                plContactMessage.ModifyDate = DateTime.Now;
                poli.PL_ContactMessage.Add(plContactMessage);
                poli.SaveChanges();
                result = "success";

            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return result;
        }

        public List<FrontEndVideoML> GetAllVideoByDate(int skip, int take)
        {

            List<PL_FrontEndVideo> lstFrontEndVideoPL = new List<PL_FrontEndVideo>();
            List<FrontEndVideoML> lstFrontEndVideoML = new List<FrontEndVideoML>();
            FrontEndVideoML frontEndVideoML;
            try
            {
                lstFrontEndVideoPL = poli.PL_FrontEndVideo.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).Skip(skip).Take(take).ToList();
                if (lstFrontEndVideoPL != null && lstFrontEndVideoPL.Count > 0)
                {
                    foreach (var item in lstFrontEndVideoPL)
                    {
                        frontEndVideoML = new FrontEndVideoML();
                        frontEndVideoML.Id = item.Id;
                        frontEndVideoML.VideoName = item.VideoName;
                        frontEndVideoML.VideoType = item.VideoType;
                        frontEndVideoML.VideoDescription = item.VideoDescription;
                        frontEndVideoML.IsActive = item.IsActive;
                        frontEndVideoML.IsDeleted = item.IsDeleted;
                        frontEndVideoML.CreatedDate = item.CreatedDate;
                        frontEndVideoML.DisplayCreatedDate = item.CreatedDate.Value.ToLongDateString();
                        frontEndVideoML.ModifyDate = item.ModifyDate;
                        lstFrontEndVideoML.Add(frontEndVideoML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstFrontEndVideoML;
        }

        public string SaveVotingSurvey(VotingSurveyML votingSurveyML)
        {
            string result = string.Empty;
            PL_VotingSurvey plVotingSurvey = new PL_VotingSurvey();
            try
            {
                plVotingSurvey.BlockId = votingSurveyML.BlockId;
                plVotingSurvey.BlockName = votingSurveyML.BlockName;
                plVotingSurvey.AreaId = votingSurveyML.AreaId;
                plVotingSurvey.AreaName = votingSurveyML.AreaName;
                plVotingSurvey.Name = votingSurveyML.Name;
                plVotingSurvey.Mobile = votingSurveyML.Mobile;
                plVotingSurvey.Address = votingSurveyML.Address;
                plVotingSurvey.VoterIdNumber = votingSurveyML.VoterIdNumber;
                plVotingSurvey.IsActive = true;
                plVotingSurvey.IsDeleted = false;
                plVotingSurvey.CreatedDate = DateTime.Now;
                plVotingSurvey.ModifyDate = DateTime.Now;
                poli.PL_VotingSurvey.Add(plVotingSurvey);
                poli.SaveChanges();
                result = "success";

            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return result;
        }

    }
}
