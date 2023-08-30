using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCodeGenLib.ViewModel
{
   public class LdapConfig
    {
        //public LdapConfig()
        //{
        //    this.BindCredentials = ConfigurationManager.AppSettings["MailServer"];
        //}
        public string Url { get; set; } = ConfigurationManager.AppSettings["url"];
        public string BindDn { get; set; } = ConfigurationManager.AppSettings["bindDn"];
        public string BindCredentials { get; set; } = ConfigurationManager.AppSettings["bindCredentials"];
        public string SearchBase { get; set; } = ConfigurationManager.AppSettings["searchBase"];
        public string SearchFilter { get; set; } = ConfigurationManager.AppSettings["searchFilter"];
        public string searchFilterLogin { get; set; } = ConfigurationManager.AppSettings["searchFilterLogin"];
        public string LdapPageSize { get; set; } = ConfigurationManager.AppSettings["LdapPageSize"];
        public string AdminCn { get; set; } = ConfigurationManager.AppSettings["adminCn"];
        public string DisplayNameAttribute { get; set; } = ConfigurationManager.AppSettings["DisplayNameAttribute"];
        public string FirstNameAttribute { get; set; } = ConfigurationManager.AppSettings["FirstNameAttribute"];
        public string MiddleNameAttribute { get; set; } = ConfigurationManager.AppSettings["MiddleNameAttribute"];
        public string LastNameAttribute { get; set; } = ConfigurationManager.AppSettings["LastNameAttribute"];
        public string UserNameAttribute { get; set; } = ConfigurationManager.AppSettings["UserNameAttribute"];
        public string MobileNumberAttribute { get; set; } = ConfigurationManager.AppSettings["MobileNumberAttribute"];
        public string EmailAttribute { get; set; } = ConfigurationManager.AppSettings["EmailAttribute"];
        public string BranchAttribute { get; set; } = ConfigurationManager.AppSettings["BranchAttribute"];
    }
}
