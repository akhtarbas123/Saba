using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliment_DL.Model;

namespace Poliment_DL
{

    public class UserDL
    {
        private PoliEntities poli = new PoliEntities();
        private CommonDL commonDL = new CommonDL();
        string error = string.Empty;

        public UserML GetLogin(string userName, string passWord)
        {
            List<PL_User> lstUser = new List<PL_User>();
            UserML userML = new UserML();
            string dbPassword = string.Empty;

            try
            {
                lstUser = poli.PL_User.Where(x => x.UserName == userName).ToList();
                if (lstUser != null && lstUser.Count > 0)
                {
                    dbPassword = lstUser[0].Password;
                    passWord = CryptorEngine.Encrypt(passWord, true);
                    if (dbPassword == passWord)
                    {
                        userML.Id = lstUser[0].Id;
                        userML.FirstName = lstUser[0].FirstName;
                        userML.LastName = lstUser[0].LastName;
                        userML.UserName = lstUser[0].UserName;
                        userML.Password = lstUser[0].Password;
                        userML.DOB = Convert.ToString(lstUser[0].DOB);
                        userML.Mobile = lstUser[0].Mobile;
                        userML.Email = lstUser[0].Email;
                        userML.Gender = lstUser[0].Gender;
                        userML.Image = lstUser[0].Image;
                        userML.IsActive = lstUser[0].IsActive;
                        userML.IsDeleted = lstUser[0].IsDeleted;
                        userML.AreaId = lstUser[0].AreaId;
                        userML.AreaName = lstUser[0].AreaName;
                        userML.BlockId = lstUser[0].BlockId;
                        userML.BlockName = lstUser[0].BlockName;

                    }
                    else
                    {
                        userML.ErrorMessage = CommonResource.NotCorrectPassword;
                    }
                }
                else
                {
                    userML.ErrorMessage = CommonResource.NotValidCredential;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                userML.ErrorMessage = CommonResource.ErrorOccured;
            }
            return userML;
        }

        public string SaveMessage(UserMessageML userMessageML)
        {
            PL_UserMessage userMessage = new PL_UserMessage();
            string result = string.Empty;
            try
            {
                userMessage.MessageSubject = userMessageML.MessageSubject;
                userMessage.MessageBody = userMessageML.MessageBody;
                userMessage.FileName = userMessageML.FileName;
                userMessage.UserId = userMessageML.UserId;
                userMessage.UserFullName = userMessageML.UserFullName;
                userMessage.IsActive = true;
                userMessage.IsDeleted = false;
                userMessage.CreatedDate = DateTime.Now;
                poli.PL_UserMessage.Add(userMessage);
                poli.SaveChanges();
                result = "success";

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                result = "error";
            }
            return result;
        }

        public int GetUserMessageCount(int userId)
        {
            int count = 0;
            try
            {
                count = poli.PL_UserMessage.Where(x => x.UserId == userId && x.IsActive == true).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        public List<UserMessageML> GetUserMessage(int userId, int skip, int take)
        {
            List<PL_UserMessage> lstUserMessage = new List<PL_UserMessage>();
            List<UserMessageML> lstUserMessageML = new List<UserMessageML>();
            UserMessageML userMessageML;
            try
            {
                lstUserMessage = poli.PL_UserMessage.Where(x => x.UserId == userId && x.IsActive == true)
                                 .OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                if (lstUserMessage.Count > 0)
                {
                    foreach (var item in lstUserMessage)
                    {
                        userMessageML = new UserMessageML();
                        userMessageML.Id = item.Id;
                        userMessageML.MessageSubject = item.MessageSubject;
                        userMessageML.MessageBody = item.MessageBody;
                        userMessageML.FileName = item.FileName;
                        userMessageML.UserId = item.UserId;
                        userMessageML.UserFullName = item.UserFullName;
                        userMessageML.IsActive = item.IsActive;
                        userMessageML.IsDeleted = item.IsDeleted;
                        userMessageML.CreatedDate = item.CreatedDate;
                        userMessageML.ReplyMessage = item.ReplyMessage;
                        userMessageML.ReplyFileName = item.ReplyFileName;
                        userMessageML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                        lstUserMessageML.Add(userMessageML);

                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstUserMessageML;
        }

        public int GetAdminMessageForUserCount(int userId)
        {
            int count = 0;
            try
            {
                count = poli.PL_AdminMessage.Where(x => x.UserId == userId && x.IsActive == true && x.IsDeleted == false).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        public List<UserMessageML> GetAdminMessageForUser(int userId, int skip, int take)
        {
            List<PL_AdminMessage> lstAdminMessage = new List<PL_AdminMessage>();
            List<UserMessageML> lstUserMessageML = new List<UserMessageML>();
            UserMessageML userMessageML;
            try
            {
                lstAdminMessage = poli.PL_AdminMessage.Where(x => x.UserId == userId && x.IsActive == true && x.IsDeleted == false)
                                 .OrderBy(x => x.Id).OrderByDescending(x => x.CreatedDate).Skip(skip).Take(take).ToList();
                if (lstAdminMessage.Count > 0)
                {
                    foreach (var item in lstAdminMessage)
                    {
                        userMessageML = new UserMessageML();
                        userMessageML.Id = item.Id;
                        userMessageML.MessageSubject = item.MessageSubject;
                        userMessageML.MessageBody = item.MessageBody;
                        userMessageML.FileName = item.FileName;
                        userMessageML.UserId = item.UserId;
                        userMessageML.UserName = item.UserName;
                        userMessageML.IsActive = item.IsActive;
                        userMessageML.IsDeleted = item.IsDeleted;
                        userMessageML.CreatedDate = item.CreatedDate;
                        userMessageML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                        lstUserMessageML.Add(userMessageML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstUserMessageML;
        }

        public UserML GetUserByUserName(string userName)
        {
            List<PL_User> lstUserPL = new List<PL_User>();
            UserML userML = new UserML();
            string dbPassword = string.Empty;
            try
            {
                lstUserPL = poli.PL_User.Where(x => x.UserName == userName && x.IsActive == true && x.IsDeleted == false).ToList();
                if (lstUserPL != null && lstUserPL.Count > 0)
                {
                    userML.Id = lstUserPL[0].Id;
                    userML.FirstName = lstUserPL[0].FirstName;
                    userML.LastName = lstUserPL[0].LastName;
                    userML.Id = lstUserPL[0].Id;
                    userML.UserName = lstUserPL[0].UserName;
                    userML.Password = lstUserPL[0].Password;
                    userML.Email = lstUserPL[0].Email;
                    userML.Mobile = lstUserPL[0].Mobile;
                    userML.DOB = Convert.ToString(lstUserPL[0].DOB);
                    userML.IsActive = lstUserPL[0].IsActive;
                    userML.IsDeleted = lstUserPL[0].IsDeleted;
                    userML.CreatedDate = lstUserPL[0].CreatedDate;
                    userML.ModifyDate = lstUserPL[0].ModifyDate;
                    userML.PoliticalParty = lstUserPL[0].PoliticalParty;
                    userML.Designation = lstUserPL[0].Designation;
                    userML.UserRole = lstUserPL[0].UserRole;
                }
                else
                {
                    userML.ErrorMessage = CommonResource.UserNotFound;
                }
            }
            catch (Exception)
            {
                userML.ErrorMessage = CommonResource.ErrorOccured;
            }
            return userML;
        }

        public UserML GetUserById(int userId)
        {
            PL_User plUser = new PL_User();
            UserML userML = new UserML();
            string dbPassword = string.Empty;
            try
            {
                plUser = poli.PL_User.Single(x => x.Id == userId && x.IsActive == true && x.IsDeleted == false);
                if (plUser != null)
                {
                    userML.Id = plUser.Id;
                    userML.FirstName = plUser.FirstName;
                    userML.LastName = plUser.LastName;
                    userML.Id = plUser.Id;
                    userML.UserName = plUser.UserName;
                    userML.Password = CryptorEngine.Decrypt(plUser.Password, true);
                    userML.Email = plUser.Email;
                    userML.Mobile = plUser.Mobile;
                    userML.DOB = plUser.DOB.Value.ToShortDateString();
                    userML.IsActive = plUser.IsActive;
                    userML.IsDeleted = plUser.IsDeleted;
                    userML.CreatedDate = plUser.CreatedDate;
                    userML.ModifyDate = plUser.ModifyDate;
                    userML.PoliticalParty = plUser.PoliticalParty;
                    userML.Designation = plUser.Designation;
                    userML.BlockId = plUser.BlockId;
                    userML.BlockName = plUser.BlockName;
                    userML.AreaId = plUser.AreaId;
                    userML.AreaName = plUser.AreaName;
                    userML.IsSuperUser = plUser.IsSuperUser;
                }
                else
                {
                    userML.ErrorMessage = CommonResource.UserNotFound;
                }
            }
            catch (Exception)
            {
                userML.ErrorMessage = CommonResource.ErrorOccured;
            }
            return userML;
        }

        public string UpdateUserDetails(UserML userML)
        {
            PL_User plUser = new PL_User();
            string result = string.Empty;
            try
            {
                plUser = poli.PL_User.Single(x => x.Id == userML.Id && x.IsActive == true && x.IsDeleted == false);
                if (plUser != null)
                {
                    plUser.FirstName = userML.FirstName;
                    plUser.LastName = userML.LastName;
                    plUser.Password = CryptorEngine.Encrypt(userML.Password, true);
                    plUser.Email = userML.Email;
                    plUser.Mobile = userML.Mobile;
                    plUser.DOB = Convert.ToDateTime(userML.DOB);
                    plUser.IsActive = true;
                    plUser.IsDeleted = false;
                    plUser.ModifyDate = DateTime.Now;
                    plUser.Gender = userML.Gender;
                    plUser.PoliticalParty = userML.PoliticalParty;
                    plUser.Designation = userML.Designation;
                    poli.SaveChanges();
                    result = "success";

                }
                else
                {
                    userML.ErrorMessage = CommonResource.UserNotFound;
                }
            }
            catch (Exception)
            {
                userML.ErrorMessage = CommonResource.ErrorOccured;
                result = "error";
            }
            return result;
        }

        public string SaveUserOtp(AdminOtpML adminOtpML)
        {
            PL_UserOtp plUserOtp = new PL_UserOtp();
            string result = string.Empty;
            try
            {
                plUserOtp = poli.PL_UserOtp.SingleOrDefault(x => x.UserId == adminOtpML.AdminId && x.IsActive == true && x.IsDeleted == false);
                if (plUserOtp != null)
                {
                    plUserOtp.Mobile = adminOtpML.Mobile;
                    plUserOtp.OtpValue = adminOtpML.OtpValue;
                    plUserOtp.ExpiredDate = DateTime.Now.AddDays(2);
                    plUserOtp.Modifydate = DateTime.Now;
                    plUserOtp.UpdateCount = plUserOtp.UpdateCount + 1;
                }
                else
                {
                    plUserOtp = new PL_UserOtp();
                    plUserOtp.UserId = adminOtpML.AdminId;
                    plUserOtp.Mobile = adminOtpML.Mobile;
                    plUserOtp.OtpValue = adminOtpML.OtpValue;
                    plUserOtp.CreatedDate = DateTime.Now;
                    plUserOtp.Modifydate = DateTime.Now;
                    plUserOtp.ExpiredDate = DateTime.Now.AddDays(2);
                    plUserOtp.IsActive = true;
                    plUserOtp.IsDeleted = false;
                    plUserOtp.IsExpired = false;
                    plUserOtp.UpdateCount = 0;
                    poli.PL_UserOtp.Add(plUserOtp);

                }
                poli.SaveChanges();
                result = "success";
                // Saving record for future purpose and to count total sms
                PL_UserOtpRecord plUserOtpRecord = new PL_UserOtpRecord();
                plUserOtpRecord.UserId = adminOtpML.AdminId; // here AdminId means UserId
                plUserOtpRecord.Mobile = adminOtpML.Mobile;
                plUserOtpRecord.OtpValue = adminOtpML.OtpValue;
                plUserOtpRecord.IsActive = true;
                plUserOtpRecord.IsDeleted = false;
                plUserOtpRecord.CreatedDate = DateTime.Now;
                poli.PL_UserOtpRecord.Add(plUserOtpRecord);
                poli.SaveChanges();

            }
            catch (Exception ex)
            {
                result = "error";
                error = ex.Message;
            }
            return result;
        }

        // Here adminId means userId -- Here we are using AdminOtpML because both have same model for this purpose
        public AdminOtpML GetUserOtp(int adminId)
        {
            PL_UserOtp plUserOtp = new PL_UserOtp();
            AdminOtpML adminOtpML = new AdminOtpML();
            DateTime currentDate = DateTime.Now;

            try
            {
                plUserOtp = poli.PL_UserOtp.SingleOrDefault(x => x.UserId == adminId && x.IsActive == true && x.IsExpired == false && x.IsDeleted == false);
                if (plUserOtp != null)
                {
                    DateTime expiredDate = plUserOtp.ExpiredDate;
                    if (expiredDate.Date >= currentDate.Date)
                    {
                        adminOtpML.Id = plUserOtp.Id;
                        adminOtpML.AdminId = plUserOtp.UserId;
                        adminOtpML.Mobile = plUserOtp.Mobile;
                        adminOtpML.OtpValue = plUserOtp.OtpValue;
                        adminOtpML.CreatedDate = plUserOtp.CreatedDate;
                        adminOtpML.ExpiredDate = plUserOtp.ExpiredDate;
                        adminOtpML.Modifydate = plUserOtp.Modifydate;
                        adminOtpML.IsExpired = plUserOtp.IsExpired;
                        adminOtpML.IsActive = plUserOtp.IsActive;
                        adminOtpML.IsDeleted = plUserOtp.IsDeleted;
                        adminOtpML.UpdateCount = plUserOtp.UpdateCount;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return adminOtpML;
        }

        public string ChangeUserPassword(int userId, string passWord)
        {
            UserML userML = new UserML();
            PL_User plUser = new PL_User();
            PL_UserOtp plUserOtp = new PL_UserOtp();
            string result = string.Empty;
            string encryptPassword = string.Empty;
            try
            {
                userML = GetUserById(userId);
                if (userML.Id > 0)
                {
                    plUser = poli.PL_User.Single(x => x.Id == userId && x.IsActive == true && x.IsDeleted == false);
                    plUserOtp = poli.PL_UserOtp.FirstOrDefault(x => x.UserId == userId && x.IsActive == true && x.IsDeleted == false);
                    if (plUser != null && plUserOtp != null)
                    {
                        encryptPassword = CryptorEngine.Encrypt(passWord, true);
                        plUser.Password = encryptPassword;
                        plUserOtp.UpdateCount = 0;
                        poli.SaveChanges();
                        result = "success";
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return result;
        }

        public string SetUpdateCountAfterLogin(int userId)
        {
            PL_UserOtp plUserOtp = new PL_UserOtp();
            string result = string.Empty;
            try
            {
                if (userId > 0)
                {
                    plUserOtp = poli.PL_UserOtp.FirstOrDefault(x => x.UserId == userId && x.IsActive == true && x.IsDeleted == false);
                    if (plUserOtp.Id > 0)
                    {
                        plUserOtp.UpdateCount = 0;
                        poli.SaveChanges();
                        result = "success";
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return result;
        }

        public int SaveUser(UserML userML)
        {
            int userId = 0;
            PL_User plUser = new PL_User();
            if (userML.Id > 0)
            {
                plUser = poli.PL_User.Single(x => x.Id == userML.Id);
            }
            plUser.BlockId = userML.BlockId;
            plUser.BlockName = userML.BlockName;
            plUser.AreaId = userML.AreaId;
            plUser.AreaName = userML.AreaName;
            plUser.FirstName = userML.FirstName;
            plUser.LastName = userML.LastName;
            plUser.UserName = userML.UserName;
            plUser.Password = userML.Password;
            plUser.DOB = Convert.ToDateTime(userML.DOB);
            plUser.Gender = userML.Gender;
            plUser.Mobile = userML.Mobile;
            plUser.Email = userML.Email;
            plUser.Image = userML.Image;
            plUser.Designation = userML.Designation;
            plUser.PoliticalParty = userML.PoliticalParty;
            plUser.ModifyDate = DateTime.Now;
            plUser.IsActive = false;
            plUser.IsSuperUser = false;
            plUser.IsDeleted = false;
            plUser.UserRole = "UserFirRl";
            plUser.CreatedBy = userML.CreatedBy;
            try
            {
                if (userML.Id == 0)
                {
                    plUser.CreatedDate = DateTime.Now;
                    poli.PL_User.Add(plUser);
                }
                poli.SaveChanges();
                userId = plUser.Id;
            }
            catch (Exception)
            {
                userId = 0;
            }
            return userId;
        }

        public string SaveVoterDetails(VidhanSabhaVotersML vidhanSabhaVotersML)
        {
            string result = string.Empty;
            PL_VidhanSabhaVoters PlVidhanSabhaVoters = new PL_VidhanSabhaVoters();
            PlVidhanSabhaVoters.BlockId = vidhanSabhaVotersML.BlockId;
            PlVidhanSabhaVoters.BlockName = vidhanSabhaVotersML.BlockName;
            PlVidhanSabhaVoters.AreaId = vidhanSabhaVotersML.AreaId;
            PlVidhanSabhaVoters.AreaName = vidhanSabhaVotersML.AreaName;
            PlVidhanSabhaVoters.Name = vidhanSabhaVotersML.Name;
            PlVidhanSabhaVoters.Gender = vidhanSabhaVotersML.Gender;
            PlVidhanSabhaVoters.Mobile = vidhanSabhaVotersML.Mobile;
            PlVidhanSabhaVoters.Address = vidhanSabhaVotersML.Address;
            PlVidhanSabhaVoters.PinCode = vidhanSabhaVotersML.PinCode;
            PlVidhanSabhaVoters.VoterId = vidhanSabhaVotersML.VoterId;
            PlVidhanSabhaVoters.CreatedDate = DateTime.Now;
            PlVidhanSabhaVoters.ModifyDate = DateTime.Now;
            PlVidhanSabhaVoters.IsActive = true;
            PlVidhanSabhaVoters.IsDeleted = false;
            PlVidhanSabhaVoters.CreatedBy = vidhanSabhaVotersML.CreatedBy;
            try
            {
                poli.PL_VidhanSabhaVoters.Add(PlVidhanSabhaVoters);
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

        public int GetInActiveUserByBlockAreaCount(string blockName, string areaName, string userName)
        {
            int totalRecordCount = 0;
            int blockId = (!string.IsNullOrEmpty(blockName)) ? Convert.ToInt32(blockName) : 0;
            int areaId = (!string.IsNullOrEmpty(areaName)) ? Convert.ToInt32(areaName) : 0;

            try
            {
                if (blockId > 0 && areaId > 0)
                {
                    totalRecordCount = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).ToList().Count;
                }
                else
                {
                    totalRecordCount = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).ToList().Count();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return totalRecordCount;
        }

        public List<UserML> GetInActiveUserByBlockArea(string blockName, string areaName, string userName, int skip, int take, string sort, string sortDir)
        {
            List<PL_User> lstUserPL = new List<PL_User>();
            List<UserML> lstUserML = new List<UserML>();
            UserML userML;
            int blockId = (!string.IsNullOrEmpty(blockName)) ? Convert.ToInt32(blockName) : 0;
            int areaId = (!string.IsNullOrEmpty(areaName)) ? Convert.ToInt32(areaName) : 0;

            try
            {
                if (blockId > 0 && areaId > 0)
                {
                    if (!string.IsNullOrEmpty(sort))
                    {
                        // First name sort code
                        if (sort.Equals("FirstName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // Last name sort code
                        if (sort.Equals("LastName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // User name sort code
                        if (sort.Equals("UserName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                        }
                    }
                    else
                    {
                        lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                            .OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                    }

                }
                else
                {

                    if (!string.IsNullOrEmpty(sort))
                    {
                        // First name sort code
                        if (sort.Equals("FirstName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // Last name sort code
                        if (sort.Equals("LastName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // User name sort code
                        if (sort.Equals("UserName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                        }
                    }
                    else
                    {
                        lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false)
                                    .OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                    }

                }
                foreach (var item in lstUserPL)
                {
                    userML = new UserML();
                    userML.Id = item.Id;
                    userML.FirstName = item.FirstName;
                    userML.LastName = item.LastName;
                    userML.UserName = item.UserName;
                    userML.Password = item.Password;
                    userML.DOB = item.DOB.Value.ToShortDateString();
                    userML.Gender = item.Gender;
                    userML.Mobile = item.Mobile;
                    userML.Email = item.Email;
                    userML.Image = item.Image;
                    userML.PoliticalParty = item.PoliticalParty;
                    userML.Designation = item.Designation;
                    userML.IsActive = item.IsActive;
                    userML.IsSuperUser = item.IsSuperUser;
                    userML.IsDeleted = item.IsDeleted;
                    userML.BlockId = item.BlockId;
                    userML.BlockName = item.BlockName;
                    userML.AreaId = item.AreaId;
                    userML.AreaName = item.AreaName;
                    userML.UserRole = item.UserRole;
                    userML.CreatedDate = item.CreatedDate;
                    userML.ModifyDate = item.ModifyDate;
                    userML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                    userML.CreatedBy = item.CreatedBy;
                    userML.ModifyBy = item.ModifyBy;
                    lstUserML.Add(userML);
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return lstUserML;
        }

        public int GetActiveUserByBlockAreaCount(string blockName, string areaName, string userName)
        {
            int totalRecordCount = 0;
            int blockId = (!string.IsNullOrEmpty(blockName)) ? Convert.ToInt32(blockName) : 0;
            int areaId = (!string.IsNullOrEmpty(areaName)) ? Convert.ToInt32(areaName) : 0;

            try
            {
                if (blockId > 0 && areaId > 0)
                {
                    totalRecordCount = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).ToList().Count;
                }
                else
                {
                    totalRecordCount = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).ToList().Count();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return totalRecordCount;
        }

        public List<UserML> GetActiveUserByBlockArea(string blockName, string areaName, string userName, int skip, int take, string sort, string sortDir)
        {
            List<PL_User> lstUserPL = new List<PL_User>();
            List<UserML> lstUserML = new List<UserML>();
            UserML userML;
            int blockId = (!string.IsNullOrEmpty(blockName)) ? Convert.ToInt32(blockName) : 0;
            int areaId = (!string.IsNullOrEmpty(areaName)) ? Convert.ToInt32(areaName) : 0;

            try
            {
                if (blockId > 0 && areaId > 0)
                {
                    if (!string.IsNullOrEmpty(sort))
                    {
                        // First name sort code
                        if (sort.Equals("FirstName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // Last name sort code
                        if (sort.Equals("LastName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // User name sort code
                        if (sort.Equals("UserName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                        }
                    }
                    else
                    {
                        lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                            .OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                    }

                }
                else
                {

                    if (!string.IsNullOrEmpty(sort))
                    {
                        // First name sort code
                        if (sort.Equals("FirstName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // Last name sort code
                        if (sort.Equals("LastName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // User name sort code
                        if (sort.Equals("UserName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                        }
                    }
                    else
                    {
                        lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.CreatedBy == userName && x.IsActive == true && x.IsDeleted == false)
                                    .OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                    }

                }
                foreach (var item in lstUserPL)
                {
                    userML = new UserML();
                    userML.Id = item.Id;
                    userML.FirstName = item.FirstName;
                    userML.LastName = item.LastName;
                    userML.UserName = item.UserName;
                    userML.Password = item.Password;
                    userML.DOB = item.DOB.Value.ToShortDateString();
                    userML.Gender = item.Gender;
                    userML.Mobile = item.Mobile;
                    userML.Email = item.Email;
                    userML.Image = item.Image;
                    userML.PoliticalParty = item.PoliticalParty;
                    userML.Designation = item.Designation;
                    userML.IsActive = item.IsActive;
                    userML.IsSuperUser = item.IsSuperUser;
                    userML.IsDeleted = item.IsDeleted;
                    userML.BlockId = item.BlockId;
                    userML.BlockName = item.BlockName;
                    userML.AreaId = item.AreaId;
                    userML.AreaName = item.AreaName;
                    userML.UserRole = item.UserRole;
                    userML.CreatedDate = item.CreatedDate;
                    userML.ModifyDate = item.ModifyDate;
                    userML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                    userML.CreatedBy = item.CreatedBy;
                    userML.ModifyBy = item.ModifyBy;
                    lstUserML.Add(userML);
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return lstUserML;
        }

        public int GetUserByUserNameCount(string userName, string user)
        {
            int totalRecordCount = 0;
            try
            {
                totalRecordCount = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.CreatedBy == user && x.IsActive == true && x.IsDeleted == false).ToList().Count();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return totalRecordCount;
        }

        public List<UserML> GetUserByUserName(string userName, string user, int skip, int take, string sort, string sortDir)
        {
            List<PL_User> lstUserPL = new List<PL_User>();
            List<UserML> lstUserML = new List<UserML>();
            UserML userML;
            try
            {
                if (!string.IsNullOrEmpty(sort))
                {
                    // First name sort code
                    if (sort.Equals("FirstName"))
                    {
                        if (sortDir.Equals("ASC"))
                        {
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.CreatedBy == user && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderBy(x => x.FirstName).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.CreatedBy == user && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderByDescending(x => x.FirstName).Skip(skip).Take(take).ToList();
                        }
                    }
                    // Last name sort code
                    if (sort.Equals("LastName"))
                    {
                        if (sortDir.Equals("ASC"))
                        {
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.CreatedBy == user && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderBy(x => x.LastName).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.CreatedBy == user && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderByDescending(x => x.LastName).Skip(skip).Take(take).ToList();
                        }
                    }
                    // User name sort code
                    if (sort.Equals("UserName"))
                    {
                        if (sortDir.Equals("ASC"))
                        {
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.CreatedBy == user && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderBy(x => x.UserName).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.CreatedBy == user && x.CreatedBy == user && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderByDescending(x => x.UserName).Skip(skip).Take(take).ToList();
                        }
                    }
                }
                else
                {
                    lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.CreatedBy == user && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                        .OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                }

                foreach (var item in lstUserPL)
                {
                    userML = new UserML();
                    userML.Id = item.Id;
                    userML.FirstName = item.FirstName;
                    userML.LastName = item.LastName;
                    userML.UserName = item.UserName;
                    userML.Password = item.Password;
                    userML.DOB = item.DOB.Value.ToShortDateString();
                    userML.Gender = item.Gender;
                    userML.Mobile = item.Mobile;
                    userML.Email = item.Email;
                    userML.Image = item.Image;
                    userML.PoliticalParty = item.PoliticalParty;
                    userML.Designation = item.Designation;
                    userML.IsActive = item.IsActive;
                    userML.IsSuperUser = item.IsSuperUser;
                    userML.IsDeleted = item.IsDeleted;
                    userML.BlockId = item.BlockId;
                    userML.BlockName = item.BlockName;
                    userML.AreaId = item.AreaId;
                    userML.AreaName = item.AreaName;
                    userML.UserRole = item.UserRole;
                    userML.CreatedDate = item.CreatedDate;
                    userML.ModifyDate = item.ModifyDate;
                    userML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                    userML.CreatedBy = item.CreatedBy;
                    userML.ModifyBy = item.ModifyBy;
                    lstUserML.Add(userML);
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return lstUserML;
        }

        public string AkhtarTest()
        {
            return "Akhtar Test";
        }


    }
}
