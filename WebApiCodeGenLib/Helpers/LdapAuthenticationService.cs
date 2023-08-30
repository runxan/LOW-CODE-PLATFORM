using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using Novell.Directory.Ldap.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCodeGenLib.DTO;
using WebApiCodeGenLib.ViewModel;

namespace WebApiCodeGenLib.Helpers
{
   public class LdapAuthenticationService 
    {
        private readonly LdapConfig _config;
        private readonly LdapConnection _connection;
       // private readonly ILoggerManager _LoggerService;

        public LdapAuthenticationService()
        {
            //_config = config.Value;
            _connection = new LdapConnection
            {
                SecureSocketLayer = false
            };
            _config = new LdapConfig();
          //  _LoggerService = LoggerService;
        }

        public string Login(string username, string password)
        {
            _connection.Connect(_config.Url, LdapConnection.DefaultPort);
            _connection.Bind(_config.BindDn, _config.BindCredentials);
            var searchFilter = string.Format(_config.searchFilterLogin, username);
            //var data = _connection.Search(_config.SearchBase,
            //    LdapConnection.ScopeSub, searchFilter);
            // var data = _connection
            // string[] reqattr = { "cn", "sn", "uid" };
            var result = _connection.Search(
                _config.SearchBase,
                LdapConnection.ScopeSub,
                searchFilter,
                null,
                false
            );
            try
            {
                var user = result.Next();
                if (user != null)
                {
                    _connection.Bind(user.Dn, password);
                    if (_connection.Bound)
                    {
                        _connection.Disconnect();
                        return "success";
                    }
                    else
                    {
                        _connection.Disconnect();
                        return "failed";

                    }
                }
            }
            catch (Exception ex)
            {
                _connection.Disconnect();
              //  _LoggerService.LogError("Failed To login With AD", ex);
                //  throw new Exception("Login failed.");
                return "failed";
            }
            return "failed";
        }


        public List<AdUserDto> GetAllUser()
        {
            try
            {
                _connection.Connect(_config.Url, LdapConnection.DefaultPort);
                _connection.Bind(_config.BindDn, _config.BindCredentials);
                var Userlist = new List<AdUserDto>();
                var searchFilter = string.Format(_config.SearchFilter, "Approver1 Approver1");
                //string[] reqattr = { "cn", "sn", "uid" };
                string[] reqattr = { _config.DisplayNameAttribute,_config.UserNameAttribute,_config.FirstNameAttribute
                    ,_config.MiddleNameAttribute,_config.LastNameAttribute,_config.EmailAttribute,_config.MobileNumberAttribute};

                try
                {
                    var cntRead = 0;                            // Total users read.
                    int? cntTotal = null;                       // Users available.
                    var curPage = 0;                            // Current page.
                    var pageSize = Convert.ToInt32(this._config.LdapPageSize);   // Users per page.
                    if (pageSize == 0)
                    {

                        var result2 = _connection.Search(
                                  _config.SearchBase,
                                  LdapConnection.ScopeSub,
                                  searchFilter,
                                  reqattr,
                                  false);
                        var listofusers = result2.ToList();
                        if (listofusers.Count > 0)
                        {

                            foreach (var user in listofusers)
                            {
                                var users = new AdUserDto();
                                if (user.GetAttributeSet().ContainsKey(_config.DisplayNameAttribute) == true)
                                {
                                    users.DisplayName = user.GetAttribute(_config.DisplayNameAttribute).StringValue;
                                }
                                if (user.GetAttributeSet().ContainsKey(_config.UserNameAttribute) == true)
                                {
                                    users.Username = user.GetAttribute(_config.UserNameAttribute).StringValue;
                                }
                                if (user.GetAttributeSet().ContainsKey(_config.EmailAttribute) == true)
                                {
                                    users.Email = user.GetAttribute(_config.EmailAttribute).StringValue;
                                }
                                if (user.GetAttributeSet().ContainsKey(_config.MobileNumberAttribute) == true)
                                {
                                    users.MobileNumber = user.GetAttribute(_config.MobileNumberAttribute).StringValue;
                                }
                                if (user.GetAttributeSet().ContainsKey(_config.FirstNameAttribute) == true)
                                {
                                    users.FullName = user.GetAttribute(_config.FirstNameAttribute).StringValue;
                                    if (user.GetAttributeSet().ContainsKey(_config.MiddleNameAttribute) == true)
                                    {
                                        users.FullName += " " + user.GetAttribute(_config.MiddleNameAttribute).StringValue;
                                    }
                                    if (user.GetAttributeSet().ContainsKey(_config.LastNameAttribute) == true)
                                    {
                                        users.FullName += " " + user.GetAttribute(_config.LastNameAttribute).StringValue;
                                    }
                                }
                                Userlist.Add(users);

                            }
                        }
                    }
                    else
                    {
                        do
                        {
                            var constraints = new LdapSearchConstraints();
                            constraints.SetControls(new[] {
                        new LdapSortControl(new LdapSortKey("cn"), true),
                        GetListControl(curPage, pageSize)
                         });
                            var results = _connection.Search(
                                    _config.SearchBase,
                                    LdapConnection.ScopeSub,
                                    searchFilter,
                                    reqattr,
                                    false,
                                    constraints);

                            while (results.HasMore() && ((cntTotal == null) || (cntRead < cntTotal)))
                            {
                                ++cntRead;
                                try
                                {
                                    var users = new AdUserDto();
                                    var user = results.Next();
                                    if (user != null)
                                    {

                                        if (user.GetAttributeSet().ContainsKey(_config.DisplayNameAttribute) == true)
                                        {
                                            users.DisplayName = user.GetAttribute(_config.DisplayNameAttribute).StringValue;
                                        }
                                        if (user.GetAttributeSet().ContainsKey(_config.UserNameAttribute) == true)
                                        {
                                            users.Username = user.GetAttribute(_config.UserNameAttribute).StringValue;
                                        }
                                        if (user.GetAttributeSet().ContainsKey(_config.EmailAttribute) == true)
                                        {
                                            users.Email = user.GetAttribute(_config.EmailAttribute).StringValue;
                                        }
                                        if (user.GetAttributeSet().ContainsKey(_config.MobileNumberAttribute) == true)
                                        {
                                            users.MobileNumber = user.GetAttribute(_config.MobileNumberAttribute).StringValue;
                                        }
                                        if (user.GetAttributeSet().ContainsKey(_config.FirstNameAttribute) == true)
                                        {
                                            users.FullName = user.GetAttribute(_config.FirstNameAttribute).StringValue;
                                            if (user.GetAttributeSet().ContainsKey(_config.MiddleNameAttribute) == true)
                                            {
                                                users.FullName += " " + user.GetAttribute(_config.MiddleNameAttribute).StringValue;
                                            }
                                            if (user.GetAttributeSet().ContainsKey(_config.LastNameAttribute) == true)
                                            {
                                                users.FullName += " " + user.GetAttribute(_config.LastNameAttribute).StringValue;
                                            }
                                        }
                                        Userlist.Add(users);
                                    }
                                }
                                catch (LdapReferralException)
                                {
                                    continue;
                                }

                                //yield return user;
                            }

                            ++curPage;
                            cntTotal = GetTotalCount(results);
                        } while ((cntTotal != null) && (cntRead < cntTotal));
                    }

                }
                catch (Exception ex)
                {
                   // _LoggerService.LogError("Failed To login With AD", ex);
                    throw new Exception("Login failed.");
                }
                _connection.Disconnect();
                return Userlist;
            }
            catch (Exception ex)
            {
                //_LoggerService.LogError("Failed To Get User List With AD", ex);
                return new List<AdUserDto>();
            }
        }

        private static LdapControl GetListControl(int page, int pageSize)
        {
            var index = page * pageSize + 1;
            var before = 0;
            var after = pageSize - 1;
            var count = 0;
            return new LdapVirtualListControl(index, before, after, count);
        }

        private static int? GetTotalCount(ILdapSearchResults results)
        {
            if (results.ResponseControls != null)
            {
                var r = (from c in results.ResponseControls
                         let d = c as LdapVirtualListResponse
                         where (d != null)
                         select (LdapVirtualListResponse)c).SingleOrDefault();
                if (r != null)
                {
                    return r.ContentCount;
                }
            }

            return null;
        }
    }
}
