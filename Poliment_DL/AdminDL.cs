using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliment_DL.Model;


namespace Poliment_DL
{
    public class AdminDL
    {
        private PoliEntities poli = new PoliEntities();
        private CommonDL commonDL = new CommonDL();
        string error = string.Empty;
        public AdminML GetLogin(string userName, string passWord)
        {
            List<PL_Admin> lstAdmin = new List<PL_Admin>();
            AdminML adminML = new AdminML();
            string dbPassword = string.Empty;

            try
            {
                lstAdmin = poli.PL_Admin.Where(x => x.UserName == userName && x.IsActive == true && x.IsDelete == false).ToList();
                if (lstAdmin != null && lstAdmin.Count > 0)
                {
                    dbPassword = lstAdmin[0].Password;
                    passWord = CryptorEngine.Encrypt(passWord, true);
                    if (dbPassword == passWord)
                    {
                        adminML.FirstName = lstAdmin[0].FirstName;
                        adminML.LastName = lstAdmin[0].LastName;
                        adminML.Id = lstAdmin[0].Id;
                    }
                    else
                    {
                        adminML.ErrorMessage = CommonResource.NotCorrectPassword;
                    }
                }
                else
                {
                    adminML.ErrorMessage = CommonResource.NotValidCredential;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                adminML.ErrorMessage = CommonResource.ErrorOccured;
            }
            return adminML;
        }

        public AdminML GetAdminByUserName(string userName)
        {
            List<PL_Admin> lstAdmin = new List<PL_Admin>();
            AdminML adminML = new AdminML();
            string dbPassword = string.Empty;

            try
            {
                lstAdmin = poli.PL_Admin.Where(x => x.UserName == userName && x.IsActive == true && x.IsDelete == false).ToList();
                if (lstAdmin != null && lstAdmin.Count > 0)
                {
                    adminML.FirstName = lstAdmin[0].FirstName;
                    adminML.LastName = lstAdmin[0].LastName;
                    adminML.Id = lstAdmin[0].Id;
                    adminML.UserName = lstAdmin[0].UserName;
                    adminML.Password = lstAdmin[0].Password;
                    adminML.Email = lstAdmin[0].Email;
                    adminML.Mobile = lstAdmin[0].Mobile;
                    adminML.DOB = lstAdmin[0].DOB;
                    adminML.IsActive = lstAdmin[0].IsActive;
                    adminML.IsDeleted = lstAdmin[0].IsDelete;
                    adminML.CreatedDate = lstAdmin[0].CreatedDate;
                    adminML.ModifyDate = lstAdmin[0].ModifyDate;
                    adminML.AdminRole = lstAdmin[0].AdminRole;
                }
                else
                {
                    adminML.ErrorMessage = CommonResource.UserNotFound;
                }
            }
            catch (Exception)
            {
                adminML.ErrorMessage = CommonResource.ErrorOccured;
            }
            return adminML;
        }

        public AdminML GetAdminById(int adminId)
        {
            PL_Admin plAdmin = new PL_Admin();
            AdminML adminML = new AdminML();
            string dbPassword = string.Empty;

            try
            {
                plAdmin = poli.PL_Admin.Single(x => x.Id == adminId && x.IsActive == true && x.IsDelete == false);
                if (plAdmin != null)
                {
                    adminML.FirstName = plAdmin.FirstName;
                    adminML.LastName = plAdmin.LastName;
                    adminML.Id = plAdmin.Id;
                    adminML.UserName = plAdmin.UserName;
                    adminML.Password = plAdmin.Password;
                    adminML.Email = plAdmin.Email;
                    adminML.Mobile = plAdmin.Mobile;
                    adminML.DOB = plAdmin.DOB;
                    adminML.IsActive = plAdmin.IsActive;
                    adminML.IsDeleted = plAdmin.IsDelete;
                    adminML.CreatedDate = plAdmin.CreatedDate;
                    adminML.ModifyDate = plAdmin.ModifyDate;
                }
                else
                {
                    adminML.ErrorMessage = CommonResource.UserNotFound;
                }
            }
            catch (Exception)
            {
                adminML.ErrorMessage = CommonResource.ErrorOccured;
            }
            return adminML;
        }

        public AdminML GetAdminDetails()
        {
            PL_Admin plAdmin = new PL_Admin();
            AdminML adminML = new AdminML();
            string dbPassword = string.Empty;

            try
            {
                plAdmin = poli.PL_Admin.FirstOrDefault(x => x.IsActive == true && x.IsDelete == false);
                if (plAdmin != null)
                {
                    adminML.FirstName = plAdmin.FirstName;
                    adminML.LastName = plAdmin.LastName;
                    adminML.Id = plAdmin.Id;
                    adminML.UserName = plAdmin.UserName;
                    adminML.Password = CryptorEngine.Decrypt(plAdmin.Password, true);
                    adminML.Email = plAdmin.Email;
                    adminML.Mobile = plAdmin.Mobile;
                    adminML.DOB = plAdmin.DOB;
                    adminML.IsActive = plAdmin.IsActive;
                    adminML.IsDeleted = plAdmin.IsDelete;
                    adminML.CreatedDate = plAdmin.CreatedDate;
                    adminML.ModifyDate = plAdmin.ModifyDate;
                    adminML.DOB = plAdmin.DOB;
                    adminML.Gender = plAdmin.Gender;
                    adminML.PoliticalParty = plAdmin.PoliticalParty;
                    adminML.DateOfBirth = plAdmin.DOB.Value.ToShortDateString();
                }
                else
                {
                    adminML.ErrorMessage = CommonResource.UserNotFound;
                }
            }
            catch (Exception)
            {
                adminML.ErrorMessage = CommonResource.ErrorOccured;
            }
            return adminML;
        }

        public string UpdateAdminDetails(AdminML adminML)
        {
            PL_Admin plAdmin = new PL_Admin();
            string dbPassword = string.Empty;
            string result = string.Empty;
            try
            {
                plAdmin = poli.PL_Admin.FirstOrDefault(x => x.IsActive == true && x.IsDelete == false);
                if (plAdmin != null)
                {
                    plAdmin.FirstName = adminML.FirstName;
                    plAdmin.LastName = adminML.LastName;
                    plAdmin.Password = CryptorEngine.Encrypt(adminML.Password, true);
                    plAdmin.Email = adminML.Email;
                    plAdmin.Mobile = adminML.Mobile;
                    plAdmin.DOB = adminML.DOB;
                    plAdmin.IsActive = true;
                    plAdmin.IsDelete = false;
                    plAdmin.ModifyDate = DateTime.Now;
                    plAdmin.Gender = adminML.Gender;
                    plAdmin.PoliticalParty = adminML.PoliticalParty;
                    poli.SaveChanges();
                    result = "success";

                }
                else
                {
                    adminML.ErrorMessage = CommonResource.UserNotFound;
                }
            }
            catch (Exception)
            {
                adminML.ErrorMessage = CommonResource.ErrorOccured;
                result = "error";
            }
            return result;
        }
        public List<BlockML> GetBlock()
        {
            List<PL_Block> lstBlockPL = new List<PL_Block>();
            List<BlockML> lstBlockML = new List<BlockML>();
            BlockML blockML;
            try
            {
                lstBlockPL = poli.PL_Block.ToList();
                foreach (var item in lstBlockPL)
                {
                    blockML = new BlockML();
                    blockML.Id = item.Id;
                    blockML.BlockName = item.BlockName;
                    lstBlockML.Add(blockML);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstBlockML;
        }

        public List<AreaML> GetArea(string blockName)
        {
            List<AreaML> lstAreaML = new List<AreaML>();
            AreaML areaML;
            try
            {
                var areaPL = poli.PL_Area.Where(x => x.SUBDISTRICTNAME.Equals(blockName)).
                       Select(x => new { x.Id, x.AreaName }).ToList();
                if (areaPL != null)
                {
                    foreach (var item in areaPL)
                    {
                        areaML = new AreaML();
                        areaML.Id = item.Id;
                        areaML.AreaName = item.AreaName;
                        lstAreaML.Add(areaML);
                    }
                }

            }
            catch (Exception)
            {

            }
            return lstAreaML;
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
            plUser.IsActive = true;
            plUser.IsSuperUser = true;
            plUser.IsDeleted = false;
            plUser.UserRole = "UserFirRl";
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

        public string SaveImagePath(int userId, string imagePath)
        {
            PL_User plUser = new PL_User();
            string result = string.Empty;
            try
            {
                plUser = poli.PL_User.SingleOrDefault(x => x.Id == userId);
                plUser.Image = imagePath;
                poli.SaveChanges();
                result = "success";
            }
            catch (Exception)
            {
                result = "error";
            }
            return result;

        }

        public int GetUserByBlockAreaCount(string blockName, string areaName)
        {
            int totalRecordCount = 0;

            int blockId = (!string.IsNullOrEmpty(blockName)) ? Convert.ToInt32(blockName) : 0;
            int areaId = (!string.IsNullOrEmpty(areaName)) ? Convert.ToInt32(areaName) : 0;

            try
            {
                if (blockId > 0 && areaId > 0)
                {
                    totalRecordCount = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true && x.IsDeleted == false).ToList().Count;
                }
                else
                {
                    totalRecordCount = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == true && x.IsDeleted == false).ToList().Count();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return totalRecordCount;
        }

        public List<UserML> GetUserByBlockArea(string blockName, string areaName, int skip, int take, string sort, string sortDir)
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
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // Last name sort code
                        if (sort.Equals("LastName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // User name sort code
                        if (sort.Equals("UserName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                        }
                    }
                    else
                    {
                        lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
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
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // Last name sort code
                        if (sort.Equals("LastName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // User name sort code
                        if (sort.Equals("UserName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                        }
                    }
                    else
                    {
                        lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == true && x.IsDeleted == false)
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
                    lstUserML.Add(userML);
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return lstUserML;
        }

        public int GetUserByUserNameCount(string userName)
        {
            int totalRecordCount = 0;
            try
            {
                totalRecordCount = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.IsActive == true && x.IsDeleted == false).ToList().Count();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return totalRecordCount;
        }

        public List<UserML> GetUserByUserName(string userName, int skip, int take, string sort, string sortDir)
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
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderBy(x => x.FirstName).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderByDescending(x => x.FirstName).Skip(skip).Take(take).ToList();
                        }
                    }
                    // Last name sort code
                    if (sort.Equals("LastName"))
                    {
                        if (sortDir.Equals("ASC"))
                        {
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderBy(x => x.LastName).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderByDescending(x => x.LastName).Skip(skip).Take(take).ToList();
                        }
                    }
                    // User name sort code
                    if (sort.Equals("UserName"))
                    {
                        if (sortDir.Equals("ASC"))
                        {
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderBy(x => x.UserName).Skip(skip).Take(take).ToList();
                        }
                        else
                        {
                            lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .OrderByDescending(x => x.UserName).Skip(skip).Take(take).ToList();
                        }
                    }
                }
                else
                {
                    lstUserPL = poli.PL_User.Where(x => x.UserName.Contains(userName) && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
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
                    lstUserML.Add(userML);
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return lstUserML;
        }

        public int GetTotalVotingSurveyCount()
        {
            int totalRecordCount = 0;
            try
            {
                totalRecordCount = poli.PL_VotingSurvey.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return totalRecordCount;
        }

        public int GetVotingSurveyCountByBlock(string blockName)
        {
            int totalRecordCount = 0;
            int blockId = (!string.IsNullOrEmpty(blockName)) ? Convert.ToInt32(blockName) : 0;
            try
            {
                if (blockId > 0)
                {
                    totalRecordCount = poli.PL_VotingSurvey.Where(x => x.BlockId == blockId && x.IsActive == true && x.IsDeleted == false).ToList().Count();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return totalRecordCount;
        }

        public int GetVotingSurveyCount(string blockName, string areaName)
        {
            int totalRecordCount = 0;

            int blockId = (!string.IsNullOrEmpty(blockName)) ? Convert.ToInt32(blockName) : 0;
            int areaId = (!string.IsNullOrEmpty(areaName)) ? Convert.ToInt32(areaName) : 0;

            try
            {
                if (blockId > 0 && areaId > 0)
                {
                    totalRecordCount = poli.PL_VotingSurvey.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true).ToList().Count;
                }
                else
                {
                    totalRecordCount = poli.PL_VotingSurvey.Where(x => x.BlockId == blockId && x.IsActive == true).ToList().Count();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return totalRecordCount;
        }

        public List<VotingSurveyML> GetVotingSurveyByBlockArea(string blockName, string areaName, int skip, int take)
        {
            List<PL_VotingSurvey> lstVotingSurveyPL = new List<PL_VotingSurvey>();
            List<VotingSurveyML> lstVotingSurveyML = new List<VotingSurveyML>();
            VotingSurveyML votingSurveyML;
            int blockId = (!string.IsNullOrEmpty(blockName)) ? Convert.ToInt32(blockName) : 0;
            int areaId = (!string.IsNullOrEmpty(areaName)) ? Convert.ToInt32(areaName) : 0;
            try
            {
                if (blockId > 0 && areaId > 0)
                {
                    lstVotingSurveyPL = poli.PL_VotingSurvey.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .Skip(skip).Take(take).ToList();
                }
                else
                {
                    lstVotingSurveyPL = poli.PL_VotingSurvey.Where(x => x.BlockId == blockId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                    .Skip(skip).Take(take).ToList();
                }
                foreach (var item in lstVotingSurveyPL)
                {
                    votingSurveyML = new VotingSurveyML();
                    votingSurveyML.Id = item.Id;
                    votingSurveyML.Name = item.Name;
                    votingSurveyML.Mobile = item.Mobile;
                    votingSurveyML.Address = item.Address;
                    votingSurveyML.VoterIdNumber = item.VoterIdNumber;
                    votingSurveyML.IsActive = item.IsActive;
                    votingSurveyML.IsDeleted = item.IsDeleted;
                    votingSurveyML.BlockId = item.BlockId;
                    votingSurveyML.BlockName = item.BlockName;
                    votingSurveyML.AreaId = item.AreaId;
                    votingSurveyML.AreaName = item.AreaName;
                    votingSurveyML.CreatedDate = item.CreatedDate;
                    votingSurveyML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                    lstVotingSurveyML.Add(votingSurveyML);
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return lstVotingSurveyML;
        }

        public int GetVotersDetailsCount(string blockName, string areaName)
        {
            int totalRecordCount = 0;
            int blockId = (!string.IsNullOrEmpty(blockName)) ? Convert.ToInt32(blockName) : 0;
            int areaId = (!string.IsNullOrEmpty(areaName)) ? Convert.ToInt32(areaName) : 0;
            try
            {
                if (blockId > 0 && areaId > 0)
                {
                    totalRecordCount = poli.PL_VidhanSabhaVoters.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true).ToList().Count;
                }
                else
                {
                    totalRecordCount = poli.PL_VidhanSabhaVoters.Where(x => x.BlockId == blockId && x.IsActive == true).ToList().Count();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return totalRecordCount;
        }

        public List<VidhanSabhaVotersML> GetVotersDetailsByBlockArea(string blockName, string areaName, int skip, int take)
        {
            List<PL_VidhanSabhaVoters> lstVidhanSabhaPL = new List<PL_VidhanSabhaVoters>();
            List<VidhanSabhaVotersML> lstVidhanSabhaML = new List<VidhanSabhaVotersML>();
            VidhanSabhaVotersML vidhanSabhaVotersML;

            int blockId = (!string.IsNullOrEmpty(blockName)) ? Convert.ToInt32(blockName) : 0;
            int areaId = (!string.IsNullOrEmpty(areaName)) ? Convert.ToInt32(areaName) : 0;
            try
            {
                if (blockId > 0 && areaId > 0)
                {
                    lstVidhanSabhaPL = poli.PL_VidhanSabhaVoters.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                                        .Skip(skip).Take(take).ToList();
                }
                else
                {
                    lstVidhanSabhaPL = poli.PL_VidhanSabhaVoters.Where(x => x.BlockId == blockId && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id)
                    .Skip(skip).Take(take).ToList();
                }
                foreach (var item in lstVidhanSabhaPL)
                {
                    vidhanSabhaVotersML = new VidhanSabhaVotersML();
                    vidhanSabhaVotersML.Id = item.Id;
                    vidhanSabhaVotersML.Name = item.Name;
                    vidhanSabhaVotersML.Mobile = item.Mobile;
                    vidhanSabhaVotersML.Address = item.Address;
                    vidhanSabhaVotersML.VoterId = item.VoterId;
                    vidhanSabhaVotersML.IsActive = item.IsActive;
                    vidhanSabhaVotersML.IsDeleted = item.IsDeleted;
                    vidhanSabhaVotersML.BlockId = item.BlockId;
                    vidhanSabhaVotersML.BlockName = item.BlockName;
                    vidhanSabhaVotersML.AreaId = item.AreaId;
                    vidhanSabhaVotersML.AreaName = item.AreaName;
                    vidhanSabhaVotersML.CreatedDate = item.CreatedDate;
                    vidhanSabhaVotersML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                    vidhanSabhaVotersML.CreatedBy = item.CreatedBy;
                    vidhanSabhaVotersML.ModifyBy = item.ModifyBy;
                    vidhanSabhaVotersML.Gender = item.Gender;
                    vidhanSabhaVotersML.PinCode = item.PinCode;
                    lstVidhanSabhaML.Add(vidhanSabhaVotersML);
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return lstVidhanSabhaML;
        }

        public UserML GetUser(int id)
        {
            UserML userML = new UserML();
            PL_User plUser = new PL_User();
            try
            {
                plUser = poli.PL_User.Single(x => x.Id == id);
                userML.Id = plUser.Id;
                userML.FirstName = plUser.FirstName;
                userML.LastName = plUser.LastName;
                userML.UserName = plUser.UserName;
                userML.Password = plUser.Password;
                userML.DOB = plUser.DOB.Value.ToShortDateString();
                userML.Gender = plUser.Gender;
                userML.Mobile = plUser.Mobile;
                userML.Email = plUser.Email;
                userML.Image = plUser.Image;
                userML.PoliticalParty = plUser.PoliticalParty;
                userML.Designation = plUser.Designation;
                userML.IsActive = plUser.IsActive;
                userML.IsSuperUser = plUser.IsSuperUser;
                userML.IsDeleted = plUser.IsDeleted;
                userML.BlockId = plUser.BlockId;
                userML.BlockName = plUser.BlockName;
                userML.AreaId = plUser.AreaId;
                userML.AreaName = plUser.AreaName;
                userML.UserRole = plUser.UserRole;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return userML;
        }

        public string DeActivateActiveUser(int id)
        {
            string result = string.Empty;
            PL_User plUser = new PL_User();
            try
            {
                plUser = poli.PL_User.Single(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
                if (plUser != null)
                {
                    plUser.IsActive = false;
                    plUser.IsDeleted = false;
                    plUser.IsSuperUser = false;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }

        public string DeleteUser(int id)
        {
            string result = string.Empty;
            PL_User plUser = new PL_User();
            try
            {
                plUser = poli.PL_User.Single(x => x.Id == id);
                if (plUser != null)
                {
                    plUser.IsActive = false;
                    plUser.IsDeleted = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }

        public string DeleteVoterDetails(int id)
        {
            string result = string.Empty;
            PL_VidhanSabhaVoters plUser = new PL_VidhanSabhaVoters();
            try
            {
                plUser = poli.PL_VidhanSabhaVoters.Single(x => x.Id == id);
                if (plUser != null)
                {
                    plUser.IsActive = false;
                    plUser.IsDeleted = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }

        public int GetAllUserMessageCount()
        {
            int count = 0;
            try
            {
                count = poli.PL_UserMessage.Where(x => x.IsActive == true).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        public List<UserMessageML> GetAllUserMessage(int skip, int take)
        {
            List<PL_UserMessage> lstUserMessage = new List<PL_UserMessage>();
            List<UserMessageML> lstUserMessageML = new List<UserMessageML>();
            UserMessageML userMessageML;
            try
            {
                lstUserMessage = poli.PL_UserMessage.Where(x => x.IsActive == true).OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
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

        public int GetSentMessageByAdminCount()
        {
            int count = 0;
            try
            {
                count = poli.PL_AdminMessage.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        // Admin and user have similar model so using here UserMessageML
        public List<UserMessageML> GetSentMessageByAdmin(int skip, int take)
        {
            List<PL_AdminMessage> lstAdminMessage = new List<PL_AdminMessage>();
            List<UserMessageML> lstUserMessageML = new List<UserMessageML>();
            UserMessageML userMessageML;
            try
            {
                lstAdminMessage = poli.PL_AdminMessage.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                if (lstAdminMessage.Count > 0)
                {
                    foreach (var item in lstAdminMessage)
                    {
                        userMessageML = new UserMessageML();
                        userMessageML.Id = item.Id;
                        userMessageML.MessageSubject = item.MessageSubject;
                        userMessageML.MessageBody = item.MessageBody;
                        userMessageML.MessageSubject = item.MessageSubject;
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

        public UserMessageML GetAdminSentMessageDetails(int messageId)
        {
            PL_AdminMessage plAdminMessage = new PL_AdminMessage();
            UserMessageML userMessageML = new UserMessageML();
            try
            {
                plAdminMessage = poli.PL_AdminMessage.Single(x => x.Id == messageId && x.IsActive == true && x.IsDeleted == false);
                if (plAdminMessage != null)
                {
                    userMessageML.Id = plAdminMessage.Id;
                    userMessageML.MessageSubject = plAdminMessage.MessageSubject;
                    userMessageML.MessageBody = plAdminMessage.MessageBody;
                    userMessageML.FileName = plAdminMessage.FileName;
                    userMessageML.UserId = plAdminMessage.UserId;
                    userMessageML.UserName = plAdminMessage.UserName;
                    userMessageML.IsActive = plAdminMessage.IsActive;
                    userMessageML.IsDeleted = plAdminMessage.IsDeleted;
                    userMessageML.CreatedDate = plAdminMessage.CreatedDate;
                    userMessageML.DisplayCreatedDate = plAdminMessage.CreatedDate.Value.ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return userMessageML;
        }
        public UserMessageML GetMessageDetails(int messageId)
        {
            PL_UserMessage userMessagePL = new PL_UserMessage();
            UserMessageML userMessageML = new UserMessageML();
            try
            {
                userMessagePL = poli.PL_UserMessage.Single(x => x.Id == messageId && x.IsActive == true && x.IsDeleted == false);
                if (userMessagePL != null)
                {
                    userMessageML.Id = userMessagePL.Id;
                    userMessageML.MessageSubject = userMessagePL.MessageSubject;
                    userMessageML.MessageBody = userMessagePL.MessageBody;
                    userMessageML.FileName = userMessagePL.FileName;
                    userMessageML.UserId = userMessagePL.UserId;
                    userMessageML.UserFullName = userMessagePL.UserFullName;
                    userMessageML.IsActive = userMessagePL.IsActive;
                    userMessageML.IsDeleted = userMessagePL.IsDeleted;
                    userMessageML.CreatedDate = userMessagePL.CreatedDate;
                    userMessageML.ReplyMessage = userMessagePL.ReplyMessage;
                    userMessageML.ReplyFileName = userMessagePL.ReplyFileName;
                    userMessageML.DisplayCreatedDate = userMessagePL.CreatedDate.Value.ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return userMessageML;
        }

        public string UpdateMessageDetails(UserMessageML userMessageML)
        {
            PL_UserMessage userMessagePL = new PL_UserMessage();
            string result = string.Empty;
            try
            {
                userMessagePL = poli.PL_UserMessage.Single(x => x.Id == userMessageML.Id);
                if (userMessagePL != null)
                {
                    userMessagePL.MessageSubject = userMessageML.MessageSubject;
                    userMessagePL.MessageBody = userMessageML.MessageBody;
                    userMessagePL.ReplyMessage = userMessageML.ReplyMessage;
                    userMessagePL.ReplyFileName = userMessageML.ReplyFileName;
                    userMessagePL.ModifyDate = DateTime.Now;
                    poli.SaveChanges();
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return result;
        }

        public string DeleteMessage(int id)
        {
            string result = string.Empty;
            PL_UserMessage plUserMessage = new PL_UserMessage();
            try
            {
                plUserMessage = poli.PL_UserMessage.Single(x => x.Id == id);
                if (plUserMessage != null)
                {
                    plUserMessage.IsActive = false;
                    plUserMessage.IsDeleted = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }
        public string DeleteAdminSentMessage(int id)
        {
            string result = string.Empty;
            PL_AdminMessage plAdminMessage = new PL_AdminMessage();
            try
            {
                plAdminMessage = poli.PL_AdminMessage.Single(x => x.Id == id);
                if (plAdminMessage != null)
                {
                    plAdminMessage.IsActive = false;
                    plAdminMessage.IsDeleted = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }

        // User message and admin message have similar model so using -- UserMessageML
        public string SaveAdminMessage(UserMessageML userMessageML)
        {
            PL_AdminMessage adminMessage = new PL_AdminMessage();
            string result = string.Empty;
            try
            {
                adminMessage.MessageBody = userMessageML.MessageBody;
                adminMessage.MessageSubject = userMessageML.MessageSubject;
                adminMessage.UserName = userMessageML.UserName;
                adminMessage.FileName = userMessageML.FileName;
                adminMessage.UserId = userMessageML.UserId;
                adminMessage.IsActive = true;
                adminMessage.IsDeleted = false;
                adminMessage.CreatedDate = DateTime.Now;
                poli.PL_AdminMessage.Add(adminMessage);
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

        public string SaveFrontImage(FrontEndImageML frontEndImageML)
        {
            PL_FrontEndImage plFrontEndImage = new PL_FrontEndImage();
            string result = string.Empty;
            try
            {
                plFrontEndImage.ImageName = frontEndImageML.ImageName;
                plFrontEndImage.IsActive = true;
                plFrontEndImage.IsDeleted = false;
                plFrontEndImage.CreatedDate = DateTime.Now;
                poli.PL_FrontEndImage.Add(plFrontEndImage);
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

        public int GetAllFrontImageCount()
        {
            int count = 0;
            try
            {
                count = poli.PL_FrontEndImage.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        public List<FrontEndImageML> GetAllFrontImage(int skip, int take)
        {
            List<PL_FrontEndImage> lstFrontImagePL = new List<PL_FrontEndImage>();
            List<FrontEndImageML> lstFrontImageML = new List<FrontEndImageML>();
            FrontEndImageML frontEndImageML;
            try
            {
                lstFrontImagePL = poli.PL_FrontEndImage.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                if (lstFrontImagePL != null && lstFrontImagePL.Count > 0)
                {
                    foreach (var item in lstFrontImagePL)
                    {
                        frontEndImageML = new FrontEndImageML();
                        frontEndImageML.Id = item.Id;
                        frontEndImageML.ImageName = item.ImageName;
                        frontEndImageML.IsActive = item.IsActive;
                        frontEndImageML.IsDeleted = item.IsDeleted;
                        frontEndImageML.CreatedDate = item.CreatedDate;
                        frontEndImageML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                        frontEndImageML.ModifyDate = item.ModifyDate;
                        lstFrontImageML.Add(frontEndImageML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstFrontImageML;
        }

        public string DeleteFrontImage(int id)
        {
            string result = string.Empty;
            PL_FrontEndImage plFrontEndImage = new PL_FrontEndImage();
            try
            {
                plFrontEndImage = poli.PL_FrontEndImage.Single(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
                if (plFrontEndImage != null)
                {
                    plFrontEndImage.IsActive = false;
                    plFrontEndImage.IsDeleted = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }
        public FrontEndImageML GetFrontImage(int id)
        {
            string result = string.Empty;
            PL_FrontEndImage plFrontEndImage = new PL_FrontEndImage();
            FrontEndImageML frontEndImageML = new FrontEndImageML();
            try
            {
                plFrontEndImage = poli.PL_FrontEndImage.Single(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
                if (plFrontEndImage != null)
                {
                    frontEndImageML.ImageName = plFrontEndImage.ImageName;
                    frontEndImageML.IsActive = plFrontEndImage.IsActive;
                    frontEndImageML.IsDeleted = plFrontEndImage.IsDeleted;
                    frontEndImageML.CreatedDate = plFrontEndImage.CreatedDate;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return frontEndImageML;
        }

        public string SaveFrontVideo(FrontEndVideoML frontEndVideoML)
        {
            PL_FrontEndVideo plFrontEndVideo = new PL_FrontEndVideo();
            string result = string.Empty;
            try
            {
                plFrontEndVideo.VideoDescription = frontEndVideoML.VideoDescription;
                plFrontEndVideo.VideoName = frontEndVideoML.VideoName;
                plFrontEndVideo.VideoType = frontEndVideoML.VideoType;
                plFrontEndVideo.VideoLength = frontEndVideoML.VideoLength;
                plFrontEndVideo.IsActive = true;
                plFrontEndVideo.IsDeleted = false;
                plFrontEndVideo.CreatedDate = DateTime.Now;
                plFrontEndVideo.ModifyDate = DateTime.Now;
                poli.PL_FrontEndVideo.Add(plFrontEndVideo);
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

        public int GetAllFrontVideoCount()
        {
            int count = 0;
            try
            {
                count = poli.PL_FrontEndVideo.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        public List<FrontEndVideoML> GetAllFrontVideo(int skip, int take)
        {
            List<PL_FrontEndVideo> lstFrontVideoPL = new List<PL_FrontEndVideo>();
            List<FrontEndVideoML> lstFrontVideoML = new List<FrontEndVideoML>();
            FrontEndVideoML frontEndVideoML;
            try
            {
                lstFrontVideoPL = poli.PL_FrontEndVideo.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                if (lstFrontVideoPL != null && lstFrontVideoPL.Count > 0)
                {
                    foreach (var item in lstFrontVideoPL)
                    {
                        frontEndVideoML = new FrontEndVideoML();
                        frontEndVideoML.Id = item.Id;
                        frontEndVideoML.VideoName = item.VideoName;
                        frontEndVideoML.VideoType = item.VideoType;
                        frontEndVideoML.IsActive = item.IsActive;
                        frontEndVideoML.IsDeleted = item.IsDeleted;
                        frontEndVideoML.VideoDescription = item.VideoDescription;
                        frontEndVideoML.CreatedDate = item.CreatedDate;
                        frontEndVideoML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                        frontEndVideoML.ModifyDate = item.ModifyDate;
                        frontEndVideoML.VideoLength = item.VideoLength;
                        lstFrontVideoML.Add(frontEndVideoML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstFrontVideoML;
        }

        public string DeleteFrontVideo(int id)
        {
            string result = string.Empty;
            PL_FrontEndVideo plFrontEndVideo = new PL_FrontEndVideo();
            try
            {
                plFrontEndVideo = poli.PL_FrontEndVideo.Single(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
                if (plFrontEndVideo != null)
                {
                    plFrontEndVideo.IsActive = false;
                    plFrontEndVideo.IsDeleted = true;
                    plFrontEndVideo.ModifyDate = DateTime.Now;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }
        public FrontEndVideoML GetFrontVideo(int id)
        {
            string result = string.Empty;

            PL_FrontEndVideo plFrontEndVideo = new PL_FrontEndVideo();
            FrontEndVideoML frontEndVideoML = new FrontEndVideoML();
            try
            {
                plFrontEndVideo = poli.PL_FrontEndVideo.Single(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
                if (plFrontEndVideo != null)
                {
                    frontEndVideoML.VideoDescription = plFrontEndVideo.VideoDescription;
                    frontEndVideoML.VideoName = plFrontEndVideo.VideoName;
                    frontEndVideoML.VideoType = plFrontEndVideo.VideoType;
                    frontEndVideoML.IsActive = plFrontEndVideo.IsActive;
                    frontEndVideoML.IsDeleted = plFrontEndVideo.IsDeleted;
                    frontEndVideoML.CreatedDate = plFrontEndVideo.CreatedDate;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return frontEndVideoML;
        }

        public string SaveHomeScreenData(ManageHomeScreenML manageHomeScreenML)
        {
            PL_ManageHomeScreen plManageHomeScreen = new PL_ManageHomeScreen();
            string result = string.Empty;
            try
            {
                plManageHomeScreen = poli.PL_ManageHomeScreen.Single(x => x.Id == 1);
                if (!string.IsNullOrEmpty(manageHomeScreenML.Heading))
                {
                    plManageHomeScreen.Heading = manageHomeScreenML.Heading;
                }
                if (!string.IsNullOrEmpty(manageHomeScreenML.ShortDescription))
                {
                    plManageHomeScreen.ShortDescription = manageHomeScreenML.ShortDescription;
                }
                if (!string.IsNullOrEmpty(manageHomeScreenML.LongDescription))
                {
                    plManageHomeScreen.LongDescription = manageHomeScreenML.LongDescription;
                }
                if (!string.IsNullOrEmpty(manageHomeScreenML.UpdateName))
                {
                    plManageHomeScreen.UpdateName = manageHomeScreenML.UpdateName;
                }
                if (!string.IsNullOrEmpty(manageHomeScreenML.AddFirstAddress))
                {
                    if (manageHomeScreenML.AddFirstAddress != "0" && manageHomeScreenML.AddFirstAddress != "1")
                    {
                        plManageHomeScreen.AddFirstAddress = manageHomeScreenML.AddFirstAddress;
                    }
                    else if (manageHomeScreenML.AddFirstAddress == "1")
                    {
                        plManageHomeScreen.AddFirstAddress = null;
                    }

                }
                if (!string.IsNullOrEmpty(manageHomeScreenML.AddSecondAddress))
                {
                    if (manageHomeScreenML.AddSecondAddress != "0" && manageHomeScreenML.AddSecondAddress != "1")
                    {
                        plManageHomeScreen.AddSecondAddress = manageHomeScreenML.AddSecondAddress;
                    }
                    else if (manageHomeScreenML.AddSecondAddress == "1")
                    {
                        plManageHomeScreen.AddSecondAddress = null;
                    }
                }
                if (!string.IsNullOrEmpty(manageHomeScreenML.VotingSurveyHeading))
                {
                    if (manageHomeScreenML.VotingSurveyHeading != "0" && manageHomeScreenML.VotingSurveyHeading != "1")
                    {
                        plManageHomeScreen.VotingSurveyHeading = manageHomeScreenML.VotingSurveyHeading;
                    }
                    else if (manageHomeScreenML.VotingSurveyHeading == "1")
                    {
                        plManageHomeScreen.VotingSurveyHeading = null;
                    }
                }
                plManageHomeScreen.ModifyDate = DateTime.Now;
                plManageHomeScreen.IsActive = true;
                plManageHomeScreen.IsDeleted = false;
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

        public ManageHomeScreenML GetHomeScreenData()
        {
            ManageHomeScreenML manageHomeScreenML = new ManageHomeScreenML();
            PL_ManageHomeScreen plManageHomeScreen = new PL_ManageHomeScreen();
            try
            {
                plManageHomeScreen = poli.PL_ManageHomeScreen.Single(x => x.Id == 1);
                if (plManageHomeScreen != null)
                {
                    manageHomeScreenML.Id = plManageHomeScreen.Id;
                    manageHomeScreenML.Heading = plManageHomeScreen.Heading;
                    manageHomeScreenML.ShortDescription = plManageHomeScreen.ShortDescription;
                    manageHomeScreenML.LongDescription = plManageHomeScreen.LongDescription;
                    manageHomeScreenML.IsActive = plManageHomeScreen.IsActive;
                    manageHomeScreenML.IsDeleted = plManageHomeScreen.IsDeleted;
                    manageHomeScreenML.CreatedDate = plManageHomeScreen.CreatedDate;
                    manageHomeScreenML.ModifyDate = plManageHomeScreen.ModifyDate;
                    manageHomeScreenML.UpdateName = plManageHomeScreen.UpdateName;
                    manageHomeScreenML.AddFirstAddress = plManageHomeScreen.AddFirstAddress;
                    manageHomeScreenML.AddSecondAddress = plManageHomeScreen.AddSecondAddress;
                    manageHomeScreenML.VotingSurveyHeading = plManageHomeScreen.VotingSurveyHeading;
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return manageHomeScreenML;
        }

        public string SaveAdminOtp(AdminOtpML adminOtpML)
        {
            PL_AdminOtp plAdminOtp = new PL_AdminOtp();
            string result = string.Empty;
            try
            {
                plAdminOtp = poli.PL_AdminOtp.SingleOrDefault(x => x.AdminId == adminOtpML.AdminId && x.IsActive == true && x.IsDeleted == false);
                if (plAdminOtp != null)
                {
                    plAdminOtp.Mobile = adminOtpML.Mobile;
                    plAdminOtp.OtpValue = adminOtpML.OtpValue;
                    plAdminOtp.ExpiredDate = DateTime.Now.AddDays(2);
                    plAdminOtp.Modifydate = DateTime.Now;
                    plAdminOtp.UpdateCount = plAdminOtp.UpdateCount + 1;
                }
                else
                {
                    plAdminOtp = new PL_AdminOtp();
                    plAdminOtp.AdminId = adminOtpML.AdminId;
                    plAdminOtp.Mobile = adminOtpML.Mobile;
                    plAdminOtp.OtpValue = adminOtpML.OtpValue;
                    plAdminOtp.CreatedDate = DateTime.Now;
                    plAdminOtp.Modifydate = DateTime.Now;
                    plAdminOtp.ExpiredDate = DateTime.Now.AddDays(2);
                    plAdminOtp.IsActive = true;
                    plAdminOtp.IsDeleted = false;
                    plAdminOtp.IsExpired = false;
                    plAdminOtp.UpdateCount = 0;
                    poli.PL_AdminOtp.Add(plAdminOtp);

                }
                poli.SaveChanges();
                result = "success";
                // Saving record for future purpose and to count total sms
                PL_AdminOtpRecord plAdminOtpRecord = new PL_AdminOtpRecord();
                plAdminOtpRecord.AdminId = adminOtpML.AdminId;
                plAdminOtpRecord.Mobile = adminOtpML.Mobile;
                plAdminOtpRecord.OtpValue = adminOtpML.OtpValue;
                plAdminOtpRecord.IsActive = true;
                plAdminOtpRecord.IsDeleted = false;
                plAdminOtpRecord.CreatedDate = DateTime.Now;
                poli.PL_AdminOtpRecord.Add(plAdminOtpRecord);
                poli.SaveChanges();

            }
            catch (Exception ex)
            {
                result = "error";
                error = ex.Message;
            }
            return result;
        }

        public AdminOtpML GetAdminOtp(int adminId)
        {
            PL_AdminOtp plAdminOtp = new PL_AdminOtp();
            AdminOtpML adminOtpML = new AdminOtpML();
            DateTime currentDate = DateTime.Now;

            try
            {
                plAdminOtp = poli.PL_AdminOtp.SingleOrDefault(x => x.AdminId == adminId && x.IsActive == true && x.IsExpired == false && x.IsDeleted == false);
                if (plAdminOtp != null)
                {
                    DateTime expiredDate = plAdminOtp.ExpiredDate;
                    if (expiredDate.Date >= currentDate.Date)
                    {
                        adminOtpML.Id = plAdminOtp.Id;
                        adminOtpML.AdminId = plAdminOtp.AdminId;
                        adminOtpML.Mobile = plAdminOtp.Mobile;
                        adminOtpML.OtpValue = plAdminOtp.OtpValue;
                        adminOtpML.CreatedDate = plAdminOtp.CreatedDate;
                        adminOtpML.ExpiredDate = plAdminOtp.ExpiredDate;
                        adminOtpML.Modifydate = plAdminOtp.Modifydate;
                        adminOtpML.IsExpired = plAdminOtp.IsExpired;
                        adminOtpML.IsActive = plAdminOtp.IsActive;
                        adminOtpML.IsDeleted = plAdminOtp.IsDeleted;
                        adminOtpML.UpdateCount = plAdminOtp.UpdateCount;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return adminOtpML;
        }

        public string ChangeAdminPassword(int adminId, string passWord)
        {
            AdminML adminML = new AdminML();
            PL_Admin plAdmin = new PL_Admin();
            PL_AdminOtp plAdminOtp = new PL_AdminOtp();
            string result = string.Empty;
            string encryptPassword = string.Empty;
            try
            {
                adminML = GetAdminById(adminId);
                if (adminML.Id > 0)
                {
                    plAdmin = poli.PL_Admin.Single(x => x.Id == adminId && x.IsActive == true && x.IsDelete == false);
                    plAdminOtp = poli.PL_AdminOtp.FirstOrDefault(x => x.AdminId == adminId && x.IsActive == true && x.IsDeleted == false);
                    if (plAdmin != null && plAdminOtp != null)
                    {
                        encryptPassword = CryptorEngine.Encrypt(passWord, true);
                        plAdmin.Password = encryptPassword;
                        plAdminOtp.UpdateCount = 0;
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

        public string SetUpdateCountAfterLogin(int adminId)
        {
            PL_AdminOtp plAdminOtp = new PL_AdminOtp();
            string result = string.Empty;
            try
            {
                if (adminId > 0)
                {
                    plAdminOtp = poli.PL_AdminOtp.FirstOrDefault(x => x.AdminId == adminId && x.IsActive == true && x.IsDeleted == false);
                    if (plAdminOtp.Id > 0)
                    {
                        plAdminOtp.UpdateCount = 0;
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
        public string SaveAdminNews(AdminNewsML adminNewsML)
        {
            string result = string.Empty;
            PL_AdminNews plAdminNews = new PL_AdminNews();
            try
            {
                plAdminNews.NewsHeading = adminNewsML.NewsHeading;
                plAdminNews.NewsDescription = adminNewsML.NewsDescription;
                plAdminNews.CreatedDate = DateTime.Now;
                plAdminNews.IsActive = true;
                plAdminNews.IsDeleted = false;
                poli.PL_AdminNews.Add(plAdminNews);
                poli.SaveChanges();
                result = "success";
            }
            catch (Exception ex)
            {
                result = "error";
                error = ex.Message;
            }
            return result;
        }

        public int GetAllNewsCount()
        {
            int count = 0;
            try
            {
                count = poli.PL_AdminNews.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        public List<AdminNewsML> GetAllNews(int skip, int take)
        {
            List<PL_AdminNews> lstAdminNewsPL = new List<PL_AdminNews>();
            List<AdminNewsML> lstAdminNewsML = new List<AdminNewsML>();
            AdminNewsML adminNewsML;
            try
            {
                lstAdminNewsPL = poli.PL_AdminNews.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                if (lstAdminNewsPL != null && lstAdminNewsPL.Count > 0)
                {
                    foreach (var item in lstAdminNewsPL)
                    {
                        adminNewsML = new AdminNewsML();
                        adminNewsML.Id = item.Id;
                        adminNewsML.NewsHeading = item.NewsHeading;
                        adminNewsML.NewsDescription = item.NewsDescription;
                        adminNewsML.IsActive = item.IsActive;
                        adminNewsML.IsDeleted = item.IsDeleted;
                        adminNewsML.CreatedDate = item.CreatedDate;
                        adminNewsML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                        adminNewsML.ModifyDate = item.ModifyDate;
                        lstAdminNewsML.Add(adminNewsML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstAdminNewsML;
        }

        public string DeleteNews(int id)
        {
            string result = string.Empty;
            PL_AdminNews plAdminNews = new PL_AdminNews();
            try
            {
                plAdminNews = poli.PL_AdminNews.Single(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
                if (plAdminNews != null)
                {
                    plAdminNews.IsActive = false;
                    plAdminNews.IsDeleted = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }

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

        public List<AdminNewsML> GetAllNewsByDate(int skip, int take)
        {

            List<PL_AdminNews> lstAdminNewsPL = new List<PL_AdminNews>();
            List<AdminNewsML> lstAdminNewsML = new List<AdminNewsML>();
            AdminNewsML adminNewsML;
            try
            {
                lstAdminNewsPL = poli.PL_AdminNews.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).Skip(skip).Take(take).ToList();
                if (lstAdminNewsPL != null && lstAdminNewsPL.Count > 0)
                {
                    foreach (var item in lstAdminNewsPL)
                    {
                        adminNewsML = new AdminNewsML();
                        adminNewsML.Id = item.Id;
                        adminNewsML.NewsHeading = item.NewsHeading;
                        adminNewsML.NewsDescription = item.NewsDescription;
                        adminNewsML.IsActive = item.IsActive;
                        adminNewsML.IsDeleted = item.IsDeleted;
                        adminNewsML.CreatedDate = item.CreatedDate;
                        adminNewsML.DisplayCreatedDate = item.CreatedDate.Value.ToLongDateString();
                        adminNewsML.ModifyDate = item.ModifyDate;
                        lstAdminNewsML.Add(adminNewsML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstAdminNewsML;
        }

        public int GetAllDevelopmentWorkCount()
        {
            int count = 0;
            try
            {
                count = poli.PL_AdminDevelopmentWork.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        public List<AdminDevelopmentWorkML> GetAllDevelopmentWork(int skip, int take)
        {
            List<PL_AdminDevelopmentWork> lstAdminDevelopmentWorkPL = new List<PL_AdminDevelopmentWork>();
            List<AdminDevelopmentWorkML> lstAdminDevelopmentWorkML = new List<AdminDevelopmentWorkML>();
            AdminDevelopmentWorkML adminDevelopmentWorkML;
            try
            {
                lstAdminDevelopmentWorkPL = poli.PL_AdminDevelopmentWork.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
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
                        adminDevelopmentWorkML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
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

        public string SaveDevelopmentWork(AdminDevelopmentWorkML adminDevelopmentWorkML)
        {
            string result = string.Empty;
            PL_AdminNews plAdminNews = new PL_AdminNews();
            PL_AdminDevelopmentWork plAdminDevelopmentWork = new PL_AdminDevelopmentWork();
            try
            {
                plAdminDevelopmentWork.DevelopmentHeading = adminDevelopmentWorkML.DevelopmentHeading;
                plAdminDevelopmentWork.DevelopmentDescription = adminDevelopmentWorkML.DevelopmentDescription;
                plAdminDevelopmentWork.CreatedDate = DateTime.Now;
                plAdminDevelopmentWork.IsActive = true;
                plAdminDevelopmentWork.IsDeleted = false;
                poli.PL_AdminDevelopmentWork.Add(plAdminDevelopmentWork);
                poli.SaveChanges();
                result = "success";
            }
            catch (Exception ex)
            {
                result = "error";
                error = ex.Message;
            }
            return result;
        }

        public string DeleteDevelopmentWork(int id)
        {
            string result = string.Empty;
            PL_AdminDevelopmentWork plAdminDevelopmentWork = new PL_AdminDevelopmentWork();

            try
            {
                plAdminDevelopmentWork = poli.PL_AdminDevelopmentWork.Single(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
                if (plAdminDevelopmentWork != null)
                {
                    plAdminDevelopmentWork.IsActive = false;
                    plAdminDevelopmentWork.IsDeleted = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }

        public AdminDevelopmentWorkML GetDevelopmentWorkById(int id)
        {
            string result = string.Empty;
            PL_AdminDevelopmentWork plAdminDevelopmentWork = new PL_AdminDevelopmentWork();
            AdminDevelopmentWorkML adminDevelopmentWorkML = new AdminDevelopmentWorkML();
            try
            {
                plAdminDevelopmentWork = poli.PL_AdminDevelopmentWork.Single(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
                if (plAdminDevelopmentWork != null)
                {
                    adminDevelopmentWorkML.Id = plAdminDevelopmentWork.Id;
                    adminDevelopmentWorkML.DevelopmentHeading = plAdminDevelopmentWork.DevelopmentHeading;
                    adminDevelopmentWorkML.DevelopmentDescription = plAdminDevelopmentWork.DevelopmentDescription;
                    adminDevelopmentWorkML.IsActive = plAdminDevelopmentWork.IsActive;
                    adminDevelopmentWorkML.IsDeleted = plAdminDevelopmentWork.IsDeleted;
                    adminDevelopmentWorkML.CreatedDate = plAdminDevelopmentWork.CreatedDate;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return adminDevelopmentWorkML;
        }
        public int GetAllGuestQueryCount()
        {
            int count = 0;
            try
            {
                count = poli.PL_GuestQuery.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        public List<GuestQueryML> GetAllGuestQuery(int skip, int take)
        {
            List<GuestQueryML> lstGuestQueryML = new List<GuestQueryML>();
            List<PL_GuestQuery> lstGuestQueryPL = new List<PL_GuestQuery>();
            GuestQueryML guestQueryML;
            try
            {
                lstGuestQueryPL = poli.PL_GuestQuery.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                if (lstGuestQueryPL != null && lstGuestQueryPL.Count > 0)
                {
                    foreach (var item in lstGuestQueryPL)
                    {
                        guestQueryML = new GuestQueryML();
                        guestQueryML.Id = item.Id;
                        guestQueryML.QueryTypeId = item.QueryTypeId;
                        guestQueryML.QueryTypeValue = item.QueryTypeValue;
                        guestQueryML.BlockId = item.BlockId;
                        guestQueryML.BlockName = item.BlockName;
                        guestQueryML.AreaId = item.AreaId;
                        guestQueryML.AreaName = item.AreaName;
                        guestQueryML.Name = item.Name;
                        guestQueryML.Mobile = item.Mobile;
                        guestQueryML.Email = item.Email;
                        guestQueryML.FourDigitPin = item.FourDigitPin;
                        guestQueryML.Address = item.Address;
                        guestQueryML.QueryHeading = item.QueryHeading;
                        guestQueryML.QueryDescription = item.QueryDescription;
                        guestQueryML.IsActive = item.IsActive;
                        guestQueryML.IsDeleted = item.IsDeleted;
                        guestQueryML.CreatedDate = item.CreatedDate;
                        guestQueryML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                        guestQueryML.ModifyDate = item.ModifyDate;
                        guestQueryML.AdminResponseMessage = item.AdminResponseMessage;
                        lstGuestQueryML.Add(guestQueryML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstGuestQueryML;
        }

        public string DeleteQuery(int id)
        {
            string result = string.Empty;
            PL_GuestQuery plGuestQuery = new PL_GuestQuery();
            try
            {
                plGuestQuery = poli.PL_GuestQuery.Single(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
                if (plGuestQuery != null)
                {
                    plGuestQuery.IsActive = false;
                    plGuestQuery.IsDeleted = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }

        public GuestQueryML GetGuestQueryById(int guestId)
        {
            PL_GuestQuery plGuestQuery = new PL_GuestQuery();
            GuestQueryML guestQueryML = new GuestQueryML();
            try
            {
                plGuestQuery = poli.PL_GuestQuery.Single(x => x.Id == guestId && x.IsActive == true && x.IsDeleted == false);
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
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return guestQueryML;
        }

        public string SaveQueryResponse(string queryResponseMessage, int queryId)
        {
            PL_GuestQuery plGuestQuery = new PL_GuestQuery();
            string result = string.Empty;
            try
            {
                plGuestQuery = poli.PL_GuestQuery.Single(x => x.Id == queryId && x.IsActive == true && x.IsDeleted == false);
                if (plGuestQuery != null)
                {
                    plGuestQuery.AdminResponseMessage = queryResponseMessage;
                    poli.SaveChanges();
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "error";
            }
            return result;
        }

        public int GetAllContactMessageCount()
        {
            int count = 0;
            try
            {
                count = poli.PL_ContactMessage.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        public List<ContactMessageML> GetAllContactMessage(int skip, int take)
        {
            List<ContactMessageML> lstContactMessageML = new List<ContactMessageML>();
            List<PL_ContactMessage> lstContactMessagePL = new List<PL_ContactMessage>();
            ContactMessageML contactMessageML;
            try
            {
                lstContactMessagePL = poli.PL_ContactMessage.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).Skip(skip).Take(take).ToList();
                if (lstContactMessagePL != null && lstContactMessagePL.Count > 0)
                {
                    foreach (var item in lstContactMessagePL)
                    {
                        contactMessageML = new ContactMessageML();
                        contactMessageML.Id = item.Id;
                        contactMessageML.FullName = item.FullName;
                        contactMessageML.Mobie = item.Mobie;
                        contactMessageML.Email = item.Email;
                        contactMessageML.Message = item.Message;
                        contactMessageML.IsActive = item.IsActive;
                        contactMessageML.IsDeleted = item.IsDeleted;
                        contactMessageML.CreatedDate = item.CreatedDate;
                        contactMessageML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                        contactMessageML.ModifyDate = item.ModifyDate;
                        lstContactMessageML.Add(contactMessageML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstContactMessageML;
        }

        public string DeleteContactMessage(int id)
        {
            string result = string.Empty;
            PL_ContactMessage plContactMessage = new PL_ContactMessage();
            try
            {
                plContactMessage = poli.PL_ContactMessage.Single(x => x.Id == id && x.IsActive == true && x.IsDeleted == false);
                if (plContactMessage != null)
                {
                    plContactMessage.IsActive = false;
                    plContactMessage.IsDeleted = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }

        public ContactMessageML GetContactMessageById(int contactId)
        {
            ContactMessageML contactMessageML = new ContactMessageML();
            PL_ContactMessage plContactMessage = new PL_ContactMessage();
            try
            {
                plContactMessage = poli.PL_ContactMessage.Single(x => x.Id == contactId && x.IsActive == true && x.IsDeleted == false);
                if (plContactMessage != null)
                {
                    contactMessageML.Id = plContactMessage.Id;
                    contactMessageML.FullName = plContactMessage.FullName;
                    contactMessageML.Mobie = plContactMessage.Mobie;
                    contactMessageML.Email = plContactMessage.Email;
                    contactMessageML.Message = plContactMessage.Message;
                    contactMessageML.IsActive = plContactMessage.IsActive;
                    contactMessageML.IsDeleted = plContactMessage.IsDeleted;
                    contactMessageML.CreatedDate = plContactMessage.CreatedDate;
                    contactMessageML.ModifyDate = plContactMessage.ModifyDate;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return contactMessageML;
        }

        public int GetInActiveUserByBlockAreaCount(string blockName, string areaName)
        {
            int totalRecordCount = 0;
            int blockId = (!string.IsNullOrEmpty(blockName)) ? Convert.ToInt32(blockName) : 0;
            int areaId = (!string.IsNullOrEmpty(areaName)) ? Convert.ToInt32(areaName) : 0;

            try
            {
                if (blockId > 0 && areaId > 0)
                {
                    totalRecordCount = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).ToList().Count;
                }
                else
                {
                    totalRecordCount = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).ToList().Count();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return totalRecordCount;
        }

        public List<UserML> GetInActiveUserByBlockArea(string blockName, string areaName, int skip, int take, string sort, string sortDir)
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
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // Last name sort code
                        if (sort.Equals("LastName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // User name sort code
                        if (sort.Equals("UserName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                        }
                    }
                    else
                    {
                        lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.AreaId == areaId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
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
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.FirstName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // Last name sort code
                        if (sort.Equals("LastName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.LastName).Skip(skip).Take(take).ToList();
                            }
                        }
                        // User name sort code
                        if (sort.Equals("UserName"))
                        {
                            if (sortDir.Equals("ASC"))
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderBy(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                            else
                            {
                                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false).OrderBy(x => x.Id)
                                            .OrderByDescending(x => x.UserName).Skip(skip).Take(take).ToList();
                            }
                        }
                    }
                    else
                    {
                        lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == false && x.IsDeleted == false && x.IsSuperUser == false)
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
                    userML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                    userML.ModifyDate = item.ModifyDate;
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

        public string ActivateUser(int id)
        {
            string result = string.Empty;
            PL_User plUser = new PL_User();
            try
            {
                plUser = poli.PL_User.Single(x => x.Id == id && x.IsActive == false && x.IsDeleted == false);
                if (plUser != null)
                {
                    plUser.IsActive = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }

        public string ActivateWithSuperUser(int id)
        {
            string result = string.Empty;
            PL_User plUser = new PL_User();
            try
            {
                plUser = poli.PL_User.Single(x => x.Id == id && x.IsActive == false && x.IsDeleted == false);
                if (plUser != null)
                {
                    plUser.IsActive = true;
                    plUser.IsSuperUser = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }

        public string DeleteInActivateUser(int id)
        {
            string result = string.Empty;
            PL_User plUser = new PL_User();
            try
            {
                plUser = poli.PL_User.Single(x => x.Id == id && x.IsActive == false && x.IsDeleted == false);
                if (plUser != null)
                {
                    plUser.IsDeleted = true;
                    poli.SaveChanges();
                    result = "success";
                }

            }
            catch (Exception ex)
            {
                result = "error";
                string error = ex.Message;
            }
            return result;
        }

        public string SaveVoiceCallDetails(VoiceCallDetailsML voiceCallDetailsML)
        {
            string result = string.Empty;
            PL_VoiceCallDetails plVoiceCall = new PL_VoiceCallDetails();
            try
            {
                plVoiceCall.FileName = voiceCallDetailsML.FileName;
                plVoiceCall.AllUser = voiceCallDetailsML.AllUser;
                plVoiceCall.BlockId = voiceCallDetailsML.BlockId;
                plVoiceCall.Hour = voiceCallDetailsML.Hour;
                plVoiceCall.Minutes = voiceCallDetailsML.Minutes;
                plVoiceCall.Seconds = voiceCallDetailsML.Seconds;
                plVoiceCall.TotalSentNumber = voiceCallDetailsML.TotalSentNumber;
                plVoiceCall.IsActive = true;
                plVoiceCall.IsDeleted = false;
                plVoiceCall.CreatedDate = DateTime.Now;
                plVoiceCall.CreatedBy = voiceCallDetailsML.CreatedBy;
                plVoiceCall.ModifyBy = voiceCallDetailsML.ModifyBy;
                poli.PL_VoiceCallDetails.Add(plVoiceCall);
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

        public string SaveSentMessageDetails(SentSmsDetailML sentSmsDetailML)
        {
            string result = string.Empty;
            SentSmsDetail sentSmsDetail = new SentSmsDetail();
            try
            {
                sentSmsDetail.SentSms = sentSmsDetailML.SentSms;
                sentSmsDetail.AllUser = sentSmsDetailML.AllUser;
                sentSmsDetail.BlockId = sentSmsDetailML.BlockId;
                sentSmsDetail.TotalSentNumber = sentSmsDetailML.TotalSentNumber;
                sentSmsDetail.IsActive = true;
                sentSmsDetail.IsDeleted = false;
                sentSmsDetail.CreatedDate = DateTime.Now;
                sentSmsDetail.CreatedBy = sentSmsDetailML.CreatedBy;
                sentSmsDetail.ModifyBy = sentSmsDetailML.ModifyBy;
                poli.SentSmsDetails.Add(sentSmsDetail);
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

        public int GetAllVoiceCallSentDetailsCount()
        {
            int count = 0;
            try
            {
                count = poli.PL_VoiceCallDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        public List<VoiceCallDetailsML> GetAllVoiceCallSentDetails(int skip, int take)
        {
            List<PL_VoiceCallDetails> lstVoiceCallDetailsPL = new List<PL_VoiceCallDetails>();
            List<VoiceCallDetailsML> lstVoiceCallDetailsML = new List<VoiceCallDetailsML>();
            VoiceCallDetailsML voiceCallDetailsML;
            try
            {
                lstVoiceCallDetailsPL = poli.PL_VoiceCallDetails.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                if (lstVoiceCallDetailsPL != null && lstVoiceCallDetailsPL.Count > 0)
                {
                    foreach (var item in lstVoiceCallDetailsPL)
                    {
                        voiceCallDetailsML = new VoiceCallDetailsML();
                        voiceCallDetailsML.Id = item.Id;
                        voiceCallDetailsML.FileName = item.FileName;
                        voiceCallDetailsML.Hour = item.Hour;
                        voiceCallDetailsML.Minutes = item.Minutes;
                        voiceCallDetailsML.Seconds = item.Seconds;
                        voiceCallDetailsML.IsActive = item.IsActive;
                        voiceCallDetailsML.IsDeleted = item.IsDeleted;
                        voiceCallDetailsML.TotalSentNumber = item.TotalSentNumber;
                        voiceCallDetailsML.CreatedDate = item.CreatedDate;
                        voiceCallDetailsML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                        lstVoiceCallDetailsML.Add(voiceCallDetailsML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstVoiceCallDetailsML;
        }
        public int GetAllSentSmsDetailsCount()
        {
            int count = 0;
            try
            {
                count = poli.SentSmsDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }
        public List<SentSmsDetailML> GetAllSentSmsDetails(int skip, int take)
        {
            List<SentSmsDetail> lstSentSmsDetails = new List<SentSmsDetail>();
            List<SentSmsDetailML> lstSentSmsDetailsML = new List<SentSmsDetailML>();
            SentSmsDetailML sentSmsDetailML;
            try
            {
                lstSentSmsDetails = poli.SentSmsDetails.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Id).Skip(skip).Take(take).ToList();
                if (lstSentSmsDetails != null && lstSentSmsDetails.Count > 0)
                {
                    foreach (var item in lstSentSmsDetails)
                    {
                        sentSmsDetailML = new SentSmsDetailML();
                        sentSmsDetailML.Id = item.Id;
                        sentSmsDetailML.SentSms = item.SentSms;
                        sentSmsDetailML.IsActive = item.IsActive;
                        sentSmsDetailML.IsDeleted = item.IsDeleted;
                        sentSmsDetailML.TotalSentNumber = item.TotalSentNumber;
                        sentSmsDetailML.CreatedDate = item.CreatedDate;
                        sentSmsDetailML.DisplayCreatedDate = item.CreatedDate.Value.ToShortDateString();
                        lstSentSmsDetailsML.Add(sentSmsDetailML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstSentSmsDetailsML;
        }

        public List<string> GetAllUsersMobile()
        {

            List<string> lstMobile = new List<string>();
            string mobile = string.Empty;
            List<PL_User> lstPlUser = new List<PL_User>();
            try
            {
                lstPlUser = poli.PL_User.Where(x => x.IsActive == true).ToList();
                if (lstPlUser.Count > 0)
                {
                    foreach (var item in lstPlUser)
                    {
                        mobile = item.Mobile;
                        lstMobile.Add(mobile);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return lstMobile;
        }
        public List<string> GetAllUsersMobileByBlock(int blockId)
        {
            List<string> lstMobile = new List<string>();
            string mobile = string.Empty;
            List<PL_User> lstPlUser = new List<PL_User>();
            try
            {
                lstPlUser = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == true).ToList();
                if (lstPlUser.Count > 0)
                {
                    foreach (var item in lstPlUser)
                    {
                        mobile = item.Mobile;
                        lstMobile.Add(mobile);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return lstMobile;
        }

        public List<string> GetAllVoterUsersMobile()
        {

            List<string> lstMobile = new List<string>();
            string mobile = string.Empty;
            List<PL_VidhanSabhaVoters> lstPlUser = new List<PL_VidhanSabhaVoters>();

            try
            {
                lstPlUser = poli.PL_VidhanSabhaVoters.Where(x => x.IsActive == true).ToList();
                if (lstPlUser.Count > 0)
                {
                    foreach (var item in lstPlUser)
                    {
                        mobile = item.Mobile;
                        lstMobile.Add(mobile);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return lstMobile;
        }
        public List<string> GetAllVoterUsersMobileByBlock(int blockId)
        {
            List<string> lstMobile = new List<string>();
            string mobile = string.Empty;
            List<PL_VidhanSabhaVoters> lstPlUser = new List<PL_VidhanSabhaVoters>();
            try
            {
                lstPlUser = poli.PL_VidhanSabhaVoters.Where(x => x.BlockId == blockId && x.IsActive == true).ToList();
                if (lstPlUser.Count > 0)
                {
                    foreach (var item in lstPlUser)
                    {
                        mobile = item.Mobile;
                        lstMobile.Add(mobile);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return lstMobile;
        }
    }
}
