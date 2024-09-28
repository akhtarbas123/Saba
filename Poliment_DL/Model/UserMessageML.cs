using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poliment_DL.Model
{
    public class UserMessageML
    {
        public int Id { get; set; }
        public string MessageSubject { get; set; }
        public string MessageBody { get; set; }
        public string FileName { get; set; }
        public int UserId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public string UserFullName { get; set; }
        public string DisplayCreatedDate { get; set; }
        public string AttachmentPath { get; set; }
        public string ReplyMessage { get; set; }
        public string ReplyFileName { get; set; }
        public string ReplyFilePath { get; set; }
        public string UserName { get; set; }
    }
}
