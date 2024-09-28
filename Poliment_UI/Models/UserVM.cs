using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Poliment_DL.Model;
using System.Net.Http;

namespace Poliment_UI.Models
{
    public class UserVM
    {
        public UserML UserML { get; set; }
    }

    public class SaveAudioCall
    {
       
        public string userId { get; set; }
        public string password { get; set; }
        public string sendMethod { get; set; }
        public string audioType { get; set; }
        public string mobile { get; set; }
      //  public string duplicateCheck { get; set; }
        public MultipartFormDataContent audioTrack { get; set; }
   //     public string libraryName { get; set; }
    //    public string saveAudio { get; set; }
        public string reDial { get; set; }
        public string redialInterval { get; set; }
        public string format { get; set; }


    }

}