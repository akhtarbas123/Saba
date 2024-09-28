using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Poliment_DL.Model;

namespace Poliment_UI.Models
{
    public class CreateUserVM
    {
        public UserML UserML { get; set; }
        public List<UserML> ListUserML { get; set; }
        public List<VotingSurveyML> VotingSurveyML { get; set; }
        public List<BlockML> BlockML { get; set; }
        public List<AreaML> AreaML { get; set; }
        public List<VidhanSabhaVotersML> VidhanSabhaVotersML { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Designation { get; set; }
        public string PoliticalParty { get; set; }
        public int? BlockId { get; set; }
        public string BlockName { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int TotalVoterSurveyCount { get; set; }
        public string BlockNameForDisplay { get; set; }
        public int TotalVoterSurveyCountOfBlock { get; set; }
        public string AbsolutePath { get; set; }
        public int? PinCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string VoterId { get; set; }

    }
}