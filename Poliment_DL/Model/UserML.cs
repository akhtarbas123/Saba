using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Poliment_DL.Model
{
    public class UserML
    {
        public int Id { get; set; }
        public string ddlBlcok { get; set; }
        public string ddlArea { get; set; }
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
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public int? BlockId { get; set; }
        public string BlockName { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public string ErrorMessage { get; set; }
        public bool? IsSuperUser { get; set; }
        public string UserRole { get; set; }
        public string DisplayCreatedDate { get; set; }
        public string DisplayModifyDate { get; set; }
    }
}
