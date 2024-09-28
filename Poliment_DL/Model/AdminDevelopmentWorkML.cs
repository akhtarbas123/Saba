using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliment_DL.Model
{
    public class AdminDevelopmentWorkML
    {
        public int Id { get; set; }
        public string DevelopmentHeading { get; set; }
        public string DevelopmentDescription { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public string DisplayCreatedDate { get; set; }

    }
}
