using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliment_DL.Model
{
    public class VotingSurveyML
    {
        public int Id { get; set; }
        public int? BlockId { get; set; }
        public string BlockName { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public string AdminResponseMessage { get; set; }
        public string VoterIdNumber { get; set; }

        public string DisplayCreatedDate { get; set; }
    }
}
