using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliment_DL.Model;
using System.Text.RegularExpressions;

namespace Poliment_DL
{
    public class CommonDL
    {
        private PoliEntities poli = new PoliEntities();
        string error = string.Empty;
        public string GetBlockById(int blockId)
        {
            string blockName = string.Empty;
            PL_Block plBlock = new PL_Block();
            try
            {
                plBlock = poli.PL_Block.Single(x => x.Id == blockId);
                blockName = plBlock.BlockName;
            }
            catch (Exception)
            {
                blockName = "error";
            }
            return blockName;
        }
        public string GetAreaById(int areaId)
        {
            string areaName = string.Empty;
            PL_Area plArea = new PL_Area();
            try
            {
                plArea = poli.PL_Area.Single(x => x.Id == areaId);
                areaName = plArea.AreaName;
            }
            catch (Exception)
            {
                areaName = "error";
            }
            return areaName;
        }

        public UserML GetUserDetails(int userId)
        {
            PL_User plUser = new PL_User();
            UserML userML = new UserML();
            try
            {
                plUser = poli.PL_User.Single(x => x.Id == userId);
                if (plUser != null)
                {
                    userML.Id = plUser.Id;
                    userML.FirstName = plUser.FirstName;
                    userML.LastName = plUser.LastName;
                    userML.UserName = plUser.UserName;
                    userML.Password = plUser.Password;
                    userML.DOB = Convert.ToString(plUser.DOB);
                    userML.Mobile = plUser.Mobile;
                    userML.Email = plUser.Email;
                    userML.Gender = plUser.Gender;
                    userML.Image = plUser.Image;
                    userML.IsActive = plUser.IsActive;
                    userML.IsDeleted = plUser.IsDeleted;
                    userML.AreaId = plUser.AreaId;
                    userML.AreaName = plUser.AreaName;
                    userML.BlockId = plUser.BlockId;
                    userML.BlockName = plUser.BlockName;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return userML;
        }

        public List<UserML> GetAllUser()
        {
            PL_User plUser = new PL_User();
            List<PL_User> lstUserPL = new List<PL_User>();
            List<UserML> lstUserML = new List<UserML>();
            UserML userML;
            try
            {
                lstUserPL = poli.PL_User.Where(x => x.IsActive == true).ToList();
                if (lstUserPL.Count > 0)
                {
                    foreach (var item in lstUserPL)
                    {
                        userML = new UserML();
                        userML.Id = item.Id;
                        userML.FirstName = item.FirstName;
                        userML.LastName = item.LastName;
                        userML.UserName = item.UserName;
                        userML.Password = item.Password;
                        userML.DOB = Convert.ToString(item.DOB);
                        userML.Mobile = item.Mobile;
                        userML.Email = item.Email;
                        userML.Gender = item.Gender;
                        userML.Image = item.Image;
                        userML.IsActive = item.IsActive;
                        userML.IsDeleted = item.IsDeleted;
                        userML.AreaId = item.AreaId;
                        userML.AreaName = item.AreaName;
                        userML.BlockId = item.BlockId;
                        userML.BlockName = item.BlockName;
                        lstUserML.Add(userML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstUserML;
        }

        public List<UserML> GetAllUser(int blockId)
        {
            PL_User plUser = new PL_User();
            List<PL_User> lstUserPL = new List<PL_User>();
            List<UserML> lstUserML = new List<UserML>();
            UserML userML;
            try
            {
                lstUserPL = poli.PL_User.Where(x => x.BlockId == blockId && x.IsActive == true).ToList();
                if (lstUserPL.Count > 0)
                {
                    foreach (var item in lstUserPL)
                    {
                        userML = new UserML();
                        userML.Id = item.Id;
                        userML.FirstName = item.FirstName;
                        userML.LastName = item.LastName;
                        userML.UserName = item.UserName;
                        userML.Password = item.Password;
                        userML.DOB = Convert.ToString(item.DOB);
                        userML.Mobile = item.Mobile;
                        userML.Email = item.Email;
                        userML.Gender = item.Gender;
                        userML.Image = item.Image;
                        userML.IsActive = item.IsActive;
                        userML.IsDeleted = item.IsDeleted;
                        userML.AreaId = item.AreaId;
                        userML.AreaName = item.AreaName;
                        userML.BlockId = item.BlockId;
                        userML.BlockName = item.BlockName;
                        lstUserML.Add(userML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstUserML;
        }

        public List<VidhanSabhaVotersML> GetAllVoters()
        {
            PL_VidhanSabhaVoters plVoters = new PL_VidhanSabhaVoters();
            List<PL_VidhanSabhaVoters> lstVotersPL = new List<PL_VidhanSabhaVoters>();
            List<VidhanSabhaVotersML> lstVotersML = new List<VidhanSabhaVotersML>();
            VidhanSabhaVotersML vidhanSabhaVotersML;

            try
            {
                lstVotersPL = poli.PL_VidhanSabhaVoters.Where(x => x.IsActive == true).ToList();
                if (lstVotersPL.Count > 0)
                {
                    foreach (var item in lstVotersPL)
                    {
                        vidhanSabhaVotersML = new VidhanSabhaVotersML();
                        vidhanSabhaVotersML.Id = item.Id;
                        vidhanSabhaVotersML.Name = item.Name;
                        vidhanSabhaVotersML.Mobile = item.Mobile;
                        vidhanSabhaVotersML.Gender = item.Gender;
                        vidhanSabhaVotersML.VoterId = item.VoterId;
                        vidhanSabhaVotersML.Address = item.Address;
                        vidhanSabhaVotersML.PinCode = item.PinCode;
                        vidhanSabhaVotersML.IsActive = item.IsActive;
                        vidhanSabhaVotersML.IsDeleted = item.IsDeleted;
                        vidhanSabhaVotersML.CreatedDate = item.CreatedDate;
                        vidhanSabhaVotersML.AreaId = item.AreaId;
                        vidhanSabhaVotersML.AreaName = item.AreaName;
                        vidhanSabhaVotersML.BlockId = item.BlockId;
                        vidhanSabhaVotersML.BlockName = item.BlockName;
                        lstVotersML.Add(vidhanSabhaVotersML);

                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstVotersML;
        }

        public List<VidhanSabhaVotersML> GetAllVoters(int blockId)
        {
            PL_VidhanSabhaVoters plVoters = new PL_VidhanSabhaVoters();
            List<PL_VidhanSabhaVoters> lstVotersPL = new List<PL_VidhanSabhaVoters>();
            List<VidhanSabhaVotersML> lstVotersML = new List<VidhanSabhaVotersML>();
            VidhanSabhaVotersML vidhanSabhaVotersML;
            try
            {
                lstVotersPL = poli.PL_VidhanSabhaVoters.Where(x => x.BlockId == blockId && x.IsActive == true).ToList();
                if (lstVotersPL.Count > 0)
                {
                    foreach (var item in lstVotersPL)
                    {
                        vidhanSabhaVotersML = new VidhanSabhaVotersML();
                        vidhanSabhaVotersML.Id = item.Id;
                        vidhanSabhaVotersML.Name = item.Name;
                        vidhanSabhaVotersML.Mobile = item.Mobile;
                        vidhanSabhaVotersML.Gender = item.Gender;
                        vidhanSabhaVotersML.VoterId = item.VoterId;
                        vidhanSabhaVotersML.Address = item.Address;
                        vidhanSabhaVotersML.PinCode = item.PinCode;
                        vidhanSabhaVotersML.IsActive = item.IsActive;
                        vidhanSabhaVotersML.IsDeleted = item.IsDeleted;
                        vidhanSabhaVotersML.CreatedDate = item.CreatedDate;
                        vidhanSabhaVotersML.AreaId = item.AreaId;
                        vidhanSabhaVotersML.AreaName = item.AreaName;
                        vidhanSabhaVotersML.BlockId = item.BlockId;
                        vidhanSabhaVotersML.BlockName = item.BlockName;
                        lstVotersML.Add(vidhanSabhaVotersML);

                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstVotersML;
        }

        public List<FrontEndImageML> GetAllFrontImage()
        {
            List<PL_FrontEndImage> lstFrontEndImagePL = new List<PL_FrontEndImage>();
            List<FrontEndImageML> lstFrontEndImageML = new List<FrontEndImageML>();
            FrontEndImageML frontEndImageML;
            try
            {
                lstFrontEndImagePL = poli.PL_FrontEndImage.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                if (lstFrontEndImagePL != null && lstFrontEndImagePL.Count > 0)
                {
                    frontEndImageML = new FrontEndImageML();
                    foreach (var item in lstFrontEndImagePL)
                    {
                        frontEndImageML = new FrontEndImageML();
                        frontEndImageML.Id = item.Id;
                        frontEndImageML.ImageName = item.ImageName;
                        frontEndImageML.IsActive = item.IsActive;
                        frontEndImageML.IsDeleted = item.IsDeleted;
                        frontEndImageML.ModifyDate = item.ModifyDate;
                        frontEndImageML.ModifyBy = item.ModifyBy;
                        frontEndImageML.CreatedDate = item.CreatedDate;
                        frontEndImageML.CreatedBy = item.CreatedBy;
                        lstFrontEndImageML.Add(frontEndImageML);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return lstFrontEndImageML;
        }

        public string CheckVotersMobile(string mobile)
        {
            string result = string.Empty;
            List<PL_VidhanSabhaVoters> lstUser = new List<PL_VidhanSabhaVoters>();
            try
            {
                lstUser = poli.PL_VidhanSabhaVoters.Where(x => x.Mobile == mobile).ToList();
                if (lstUser != null && lstUser.Count > 0)
                {
                    result = "Exist";
                }
                else
                {
                    result = "NotExist";
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "Error";
            }
            return result;
        }

        public string CheckUserNameExist(string userName)
        {
            string result = string.Empty;
            List<PL_User> lstUser = new List<PL_User>();
            try
            {
                lstUser = poli.PL_User.Where(x => x.UserName == userName).ToList();
                result = CheckUserNameExistInAdminTable(userName);
                if (result == "NotExist")
                {
                    if (lstUser != null && lstUser.Count > 0)
                    {
                        result = "Exist";
                    }
                    else
                    {
                        result = "NotExist";
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "Error";
            }
            return result;
        }

        public string CheckUserMobile(string mobile)
        {
            string result = string.Empty;
            List<PL_User> lstUser = new List<PL_User>();
            try
            {
                lstUser = poli.PL_User.Where(x => x.Mobile == mobile).ToList();
                if (lstUser != null && lstUser.Count > 0)
                {
                    result = "Exist";
                }
                else
                {
                    result = "NotExist";
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "Error";
            }
            return result;
        }

        public string CheckUserEmail(string email)
        {
            string result = string.Empty;
            List<PL_User> lstUser = new List<PL_User>();
            try
            {
                lstUser = poli.PL_User.Where(x => x.Email == email).ToList();
                if (lstUser != null && lstUser.Count > 0)
                {
                    result = "Exist";
                }
                else
                {
                    result = "NotExist";
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "Error";
            }
            return result;
        }

        public string CheckUserNameExistInAdminTable(string userName)
        {
            string result = string.Empty;
            List<PL_Admin> lstAdmin = new List<PL_Admin>();
            try
            {
                lstAdmin = poli.PL_Admin.Where(x => x.UserName == userName).ToList();
                if (lstAdmin != null && lstAdmin.Count > 0)
                {
                    result = "Exist";
                }
                else
                {
                    result = "NotExist";
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "Error";
            }
            return result;
        }

        public int CheckVotingSurveyMobileCount(string mobile)
        {
            int count = 0;
            try
            {
                count = poli.PL_VotingSurvey.Where(x => x.Mobile == mobile && x.IsActive == true).ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                count = 0;
            }
            return count;
        }

        public string CheckVoterId(string voterId)
        {
            string result = string.Empty;
            List<PL_VotingSurvey> lstVotingSurvey = new List<PL_VotingSurvey>();
            try
            {
                lstVotingSurvey = poli.PL_VotingSurvey.Where(x => x.VoterIdNumber == voterId && x.IsActive == true).ToList();
                if (lstVotingSurvey != null && lstVotingSurvey.Count > 0)
                {
                    result = "Exist";
                }
                else
                {
                    result = "NotExist";
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = "Error";
            }
            return result;
        }
        public int? TotalAudioCall()
        {
            int? oneRowTotalMinute;
            int? totalCallMinute = 0;
            List<int?> lstTotalMinute = new List<int?>();
            List<PL_VoiceCallDetails> lstVoiceCallDetails = new List<PL_VoiceCallDetails>();
            try
            {
                lstVoiceCallDetails = poli.PL_VoiceCallDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                if (lstVoiceCallDetails != null)
                {
                    if (lstVoiceCallDetails.Count > 0)
                    {
                        foreach (var item in lstVoiceCallDetails)
                        {
                            oneRowTotalMinute = 0;
                            if (item.Minutes > 0)
                            {
                                switch (item.Minutes)
                                {
                                    case 1:
                                        oneRowTotalMinute = item.TotalSentNumber * 2;
                                        break;
                                    case 2:
                                        oneRowTotalMinute = item.TotalSentNumber * 3;
                                        break;
                                    case 3:
                                        oneRowTotalMinute = item.TotalSentNumber * 4;
                                        break;
                                    case 4:
                                        oneRowTotalMinute = item.TotalSentNumber * 5;
                                        break;
                                    case 5:
                                        oneRowTotalMinute = item.TotalSentNumber * 6;
                                        break;
                                    case 6:
                                        oneRowTotalMinute = item.TotalSentNumber * 7;
                                        break;
                                    case 7:
                                        oneRowTotalMinute = item.TotalSentNumber * 8;
                                        break;
                                    case 8:
                                        oneRowTotalMinute = item.TotalSentNumber * 9;
                                        break;
                                    case 9:
                                        oneRowTotalMinute = item.TotalSentNumber * 10;
                                        break;
                                    case 10:
                                        oneRowTotalMinute = item.TotalSentNumber * 11;
                                        break;
                                    case 11:
                                        oneRowTotalMinute = item.TotalSentNumber * 12;
                                        break;
                                    case 12:
                                        oneRowTotalMinute = item.TotalSentNumber * 13;
                                        break;
                                    case 13:
                                        oneRowTotalMinute = item.TotalSentNumber * 14;
                                        break;
                                    case 14:
                                        oneRowTotalMinute = item.TotalSentNumber * 15;
                                        break;
                                    case 15:
                                        oneRowTotalMinute = item.TotalSentNumber * 16;
                                        break;
                                    case 16:
                                        oneRowTotalMinute = item.TotalSentNumber * 17;
                                        break;
                                    case 17:
                                        oneRowTotalMinute = item.TotalSentNumber * 18;
                                        break;
                                    case 18:
                                        oneRowTotalMinute = item.TotalSentNumber * 19;
                                        break;
                                    case 19:
                                        oneRowTotalMinute = item.TotalSentNumber * 20;
                                        break;
                                    case 20:
                                        oneRowTotalMinute = item.TotalSentNumber * 21;
                                        break;
                                    case 21:
                                        oneRowTotalMinute = item.TotalSentNumber * 22;
                                        break;
                                    case 22:
                                        oneRowTotalMinute = item.TotalSentNumber * 23;
                                        break;
                                    case 23:
                                        oneRowTotalMinute = item.TotalSentNumber * 24;
                                        break;
                                    case 24:
                                        oneRowTotalMinute = item.TotalSentNumber * 25;
                                        break;
                                    case 25:
                                        oneRowTotalMinute = item.TotalSentNumber * 26;
                                        break;
                                    case 26:
                                        oneRowTotalMinute = item.TotalSentNumber * 27;
                                        break;
                                    case 27:
                                        oneRowTotalMinute = item.TotalSentNumber * 28;
                                        break;
                                    case 28:
                                        oneRowTotalMinute = item.TotalSentNumber * 29;
                                        break;
                                    case 29:
                                        oneRowTotalMinute = item.TotalSentNumber * 30;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                oneRowTotalMinute = item.TotalSentNumber * 1;
                            }
                            lstTotalMinute.Add(oneRowTotalMinute);
                        }

                        for (int i = 0; i < lstTotalMinute.Count; i++)
                        {
                            totalCallMinute = totalCallMinute + lstTotalMinute[i];
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                totalCallMinute = 0;
            }
            return totalCallMinute;
        }

        public static bool IsEnglish(string inputstring)
        {
            Regex regex = new Regex(@"[A-Za-z0-9 ;:?<>/|_*&^%$#@!`~.',-=+''(){}\[\]\\\r\n]");
            MatchCollection matches = regex.Matches(inputstring);
            if (matches.Count.Equals(inputstring.Length))
                return true;
            else
                return false;
        }
        public int? TotalSentMessage()
        {
            List<SentSmsDetail> lstSentSmsDetail = new List<SentSmsDetail>();
            List<int?> lstTotalMessage = new List<int?>();
            int? oneRowTotalMessage;
            int? totalMessageMinute = 0;
            int smsLength = 0;
            try
            {
                lstSentSmsDetail = poli.SentSmsDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                if (lstSentSmsDetail != null)
                {
                    if (lstSentSmsDetail.Count > 0)
                    {
                        bool isEnglish;
                        foreach (var item in lstSentSmsDetail)
                        {
                            isEnglish = CommonDL.IsEnglish(item.SentSms);
                            oneRowTotalMessage = 0;
                            smsLength = item.SentSms.Length;
                            if (isEnglish == true)
                            {
                                if (smsLength <= 120)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 1;
                                }
                                else if (smsLength > 120 && smsLength <= 240)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 2;
                                }
                                else if (smsLength > 240 && smsLength <= 360)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 3;
                                }
                                else if (smsLength > 360 && smsLength <= 480)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 4;
                                }
                                else if (smsLength > 480 && smsLength <= 600)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 5;
                                }
                                else if (smsLength > 600 && smsLength <= 720)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 6;
                                }
                                else if (smsLength > 720 && smsLength <= 840)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 7;
                                }
                                else if (smsLength > 840 && smsLength <= 960)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 8;
                                }
                                else if (smsLength > 960 && smsLength <= 1080)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 9;
                                }
                                else if (smsLength > 1080 && smsLength <= 1200)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 10;
                                }
                                else if (smsLength > 1200 && smsLength <= 1320)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 11;
                                }
                                else if (smsLength > 1320 && smsLength <= 1440)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 12;
                                }
                                else if (smsLength > 1440 && smsLength <= 1560)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 13;
                                }
                                else if (smsLength > 1560 && smsLength <= 1680)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 14;
                                }
                                else if (smsLength > 1680 && smsLength <= 1800)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 15;
                                }
                                else if (smsLength > 1800 && smsLength <= 1920)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 16;
                                }
                                else if (smsLength > 1920 && smsLength <= 2040)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 17;
                                }
                                else if (smsLength > 2040 && smsLength <= 2160)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 18;
                                }
                                else if (smsLength > 2160 && smsLength <= 2280)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 19;
                                }
                                else if (smsLength > 2280 && smsLength <= 2400)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 20;
                                }
                            }
                            else
                            {
                                if (smsLength <= 60)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 1;
                                }
                                else if (smsLength > 60 && smsLength <= 120)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 2;
                                }
                                else if (smsLength > 120 && smsLength <= 180)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 3;
                                }
                                else if (smsLength > 180 && smsLength <= 240)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 4;
                                }
                                else if (smsLength > 240 && smsLength <= 300)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 5;
                                }
                                else if (smsLength > 300 && smsLength <= 360)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 6;
                                }
                                else if (smsLength > 360 && smsLength <= 420)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 7;
                                }
                                else if (smsLength > 420 && smsLength <= 480)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 8;
                                }
                                else if (smsLength > 480 && smsLength <= 540)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 9;
                                }
                                else if (smsLength > 540 && smsLength <= 600)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 10;
                                }
                                else if (smsLength > 600 && smsLength <= 660)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 11;
                                }
                                else if (smsLength > 660 && smsLength <= 720)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 12;
                                }
                                else if (smsLength > 720 && smsLength <= 780)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 13;
                                }
                                else if (smsLength > 780 && smsLength <= 840)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 14;
                                }
                                else if (smsLength > 840 && smsLength <= 900)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 15;
                                }
                                else if (smsLength > 900 && smsLength <= 960)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 16;
                                }
                                else if (smsLength > 960 && smsLength <= 1020)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 17;
                                }
                                else if (smsLength > 1020 && smsLength <= 1080)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 18;
                                }
                                else if (smsLength > 1080 && smsLength <= 1140)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 19;
                                }
                                else if (smsLength > 1140 && smsLength <= 1200)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 20;
                                }
                                else if (smsLength > 1200 && smsLength <= 1260)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 21;
                                }
                                else if (smsLength > 1260 && smsLength <= 1320)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 22;
                                }
                                else if (smsLength > 1320 && smsLength <= 1380)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 23;
                                }
                                else if (smsLength > 1380 && smsLength <= 1440)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 24;
                                }
                                else if (smsLength > 1440 && smsLength <= 1500)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 25;
                                }
                                else if (smsLength > 1500 && smsLength <= 1560)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 26;
                                }
                                else if (smsLength > 1560 && smsLength <= 1620)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 27;
                                }
                                else if (smsLength > 1620 && smsLength <= 1680)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 28;
                                }
                                else if (smsLength > 1680 && smsLength <= 1740)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 29;
                                }
                                else if (smsLength > 1740 && smsLength <= 1800)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 30;
                                }
                                else if (smsLength > 1800 && smsLength <= 1860)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 31;
                                }
                                else if (smsLength > 1860 && smsLength <= 1920)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 32;
                                }
                                else if (smsLength > 1920 && smsLength <= 1980)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 33;
                                }
                                else if (smsLength > 1980 && smsLength <= 2040)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 34;
                                }
                                else if (smsLength > 2040 && smsLength <= 2100)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 35;
                                }
                                else if (smsLength > 2100 && smsLength <= 2160)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 36;
                                }
                                else if (smsLength > 2160 && smsLength <= 2220)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 37;
                                }
                                else if (smsLength > 2220 && smsLength <= 2280)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 38;
                                }
                                else if (smsLength > 2280 && smsLength <= 2340)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 39;
                                }
                                else if (smsLength > 2340 && smsLength <= 2400)
                                {
                                    oneRowTotalMessage = item.TotalSentNumber * 40;
                                }
                            }
                            lstTotalMessage.Add(oneRowTotalMessage);
                        }
                        for (int i = 0; i < lstTotalMessage.Count; i++)
                        {
                            totalMessageMinute = totalMessageMinute + lstTotalMessage[i];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                totalMessageMinute = 0;
            }
            return totalMessageMinute;
        }

        public int AdminOtpCount()
        {
            int count = 0;
            try
            {
                count = poli.PL_AdminOtpRecord.ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }

        public int UserOtpCount()
        {
            int count = 0;
            try
            {
                count = poli.PL_UserOtpRecord.ToList().Count;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return count;
        }

        public long? GetVideoSpace()
        {
            List<PL_FrontEndVideo> lstFrontEndVideoPL = new List<PL_FrontEndVideo>();
            long? videoSpace = 0;
            long? totalVideoSpace = 0;
            try
            {
                lstFrontEndVideoPL = poli.PL_FrontEndVideo.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                if (lstFrontEndVideoPL != null)
                {
                    if (lstFrontEndVideoPL.Count > 0)
                    {
                        int count = lstFrontEndVideoPL.Count;
                        for (int i = 0; i < count; i++)
                        {
                            videoSpace = totalVideoSpace + lstFrontEndVideoPL[i].VideoLength;
                        }
                        if (videoSpace > 1024)
                        {
                            totalVideoSpace = videoSpace / 1024;
                            totalVideoSpace = totalVideoSpace + 1;
                        }
                        else
                        {
                            totalVideoSpace = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                totalVideoSpace = 0;
            }
            return totalVideoSpace;
        }

        public UserBankDetailsML GetUserBankDetails(int userId)
        {
            List<PL_UserBankDetails> lstUserBankDetailsPL = new List<PL_UserBankDetails>();
            UserBankDetailsML userBankDetailsML = new UserBankDetailsML();
            try
            {
                lstUserBankDetailsPL = poli.PL_UserBankDetails.Where(x => x.UserId == userId && x.IsActive == true).ToList();
                if (lstUserBankDetailsPL.Count == 1)
                {
                    userBankDetailsML.Id = lstUserBankDetailsPL[0].Id;
                    userBankDetailsML.UserId = lstUserBankDetailsPL[0].UserId;
                    userBankDetailsML.AccountNumber = lstUserBankDetailsPL[0].AccountNumber;
                    userBankDetailsML.IfscCode = lstUserBankDetailsPL[0].IfscCode;
                    userBankDetailsML.BankName = lstUserBankDetailsPL[0].BankName;
                    userBankDetailsML.BranchAddress = lstUserBankDetailsPL[0].BranchAddress;
                    userBankDetailsML.IsActive = lstUserBankDetailsPL[0].IsActive;
                    userBankDetailsML.IsDeleted = lstUserBankDetailsPL[0].IsDeleted;
                    userBankDetailsML.CreatedDate = lstUserBankDetailsPL[0].CreatedDate;
                    userBankDetailsML.DisplayCreatedDate = lstUserBankDetailsPL[0].CreatedDate.Value.ToShortDateString();
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return userBankDetailsML;
        }

        public string SaveUserBankDetails(UserBankDetailsML bankDetailsML)
        {
            PL_UserBankDetails userBankDetailsPL = new PL_UserBankDetails();
            List<PL_UserBankDetails> lstUserBankDetailsPL = new List<PL_UserBankDetails>();
            UserBankDetailsML userBankDetailsML = new UserBankDetailsML();
            string result = string.Empty;
            try
            {
                lstUserBankDetailsPL = poli.PL_UserBankDetails.Where(x => x.UserId == bankDetailsML.UserId && x.IsActive == true).ToList();
                if (lstUserBankDetailsPL.Count == 0)
                {
                    userBankDetailsPL = new PL_UserBankDetails();
                    userBankDetailsPL.UserId = bankDetailsML.UserId;
                    userBankDetailsPL.AccountNumber = bankDetailsML.AccountNumber;
                    userBankDetailsPL.IfscCode = bankDetailsML.IfscCode;
                    userBankDetailsPL.BankName = bankDetailsML.BankName;
                    userBankDetailsPL.BranchAddress = bankDetailsML.BranchAddress;
                    userBankDetailsPL.IsActive = true;
                    userBankDetailsPL.IsDeleted = false;
                    userBankDetailsPL.CreatedDate = DateTime.Now;
                    poli.PL_UserBankDetails.Add(userBankDetailsPL);
                    poli.SaveChanges();
                }
                else
                {
                    userBankDetailsPL = new PL_UserBankDetails();
                    userBankDetailsPL = poli.PL_UserBankDetails.Single(x => x.UserId == bankDetailsML.UserId);
                    userBankDetailsPL.AccountNumber = bankDetailsML.AccountNumber;
                    userBankDetailsPL.IfscCode = bankDetailsML.IfscCode;
                    userBankDetailsPL.BankName = bankDetailsML.BankName;
                    userBankDetailsPL.BranchAddress = bankDetailsML.BranchAddress;
                    userBankDetailsPL.ModifyDate = DateTime.Now;
                    poli.SaveChanges();
                }
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
