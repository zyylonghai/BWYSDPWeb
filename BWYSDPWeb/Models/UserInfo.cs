using SDPCRL.COM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BWYSDPWeb.Models
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string UserNm { get; set; }
        public Language Language { get; set; }

    }

    //public class IdentityCredential
    //{
    //    public string CertificateID { get; set; }

    //    public string UserNm { get; set; }

    //    public bool HasAdminRole { get; set; }
    //}
}