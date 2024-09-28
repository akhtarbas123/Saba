using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliment_DL.Model
{
    public class AdminOtpML
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string Mobile { get; set; }
        public int OtpValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public bool? IsExpired { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime Modifydate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public int? UpdateCount { get; set; }
    }
}
