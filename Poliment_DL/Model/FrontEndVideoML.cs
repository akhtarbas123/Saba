using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliment_DL.Model
{
    public class FrontEndVideoML
    {
        public int Id { get; set; }
        public string VideoName { get; set; }
        public string VideoType { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public string DisplayCreatedDate { get; set; }
        public string VideoPath { get; set; }
        public string VideoDescription { get; set; }
        public long? VideoLength { get; set; }
    }
}
