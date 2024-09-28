using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliment_DL.Model
{
    public class ManageHomeScreenML
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public string UpdateName { get; set; }
        public string AddFirstAddress { get; set; }
        public string AddSecondAddress { get; set; }
        public string VotingSurveyHeading { get; set; }

    }
}
