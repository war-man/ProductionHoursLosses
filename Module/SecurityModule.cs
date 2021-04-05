using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ProductionHoursLosses.Models;


namespace ProductionHoursLosses.Module
{
    public class SecurityModule
    {
        public static void VerifyAuthenticationCookie(HttpRequestBase Request, Permission permission)
        {
            var authCookie = Request.Cookies[".ProductionAuthCookie"];

            if (GenerateAuthenticationCookie(Request, permission))
                return;

            FormsAuthentication.RedirectFromLoginPage("PermissionDenied", false);
        }

        public static bool HasPermission(HttpRequestBase Request, Permission permission)
        {
            var userName = Request.LogonUserIdentity.Name.Substring(Request.LogonUserIdentity.Name.LastIndexOf("\\") + 1);

            var directorySearcher = new DirectorySearcher();

            directorySearcher.Filter = "(&(!(userAccountControl:1.2.840.113556.1.4.803:=2))(objectCategory=user)(samaccountname=" + userName + "))";

            directorySearcher.PropertiesToLoad.Add("memberOf");

            SearchResult searchResult;

            try
            {
                searchResult = directorySearcher.FindOne();
            }
            catch (Exception)
            {
                return false;
            }

            if (searchResult == null)
                return false;

            for (var propertyCounter = 0; propertyCounter <= searchResult.Properties["memberOf"].Count - 1; propertyCounter++)
            {
                var match = Regex.Matches(searchResult.Properties["memberOf"][propertyCounter].ToString(), string.Format("={0},", permission.Detail.Name));

                if (match.Count > 0)
                    return true;
            }

            return false;
        }

        public static bool GenerateAuthenticationCookie(HttpRequestBase Request, Permission permission)
        {
            var userName = Request.LogonUserIdentity.Name.Substring(Request.LogonUserIdentity.Name.LastIndexOf("\\") + 1);

            var directorySearcher = new DirectorySearcher();

            directorySearcher.Filter = "(&(!(userAccountControl:1.2.840.113556.1.4.803:=2))(objectCategory=user)(samaccountname=" + userName + "))";

            directorySearcher.PropertiesToLoad.Add("memberOf");

            SearchResult searchResult;

            try
            {
                searchResult = directorySearcher.FindOne();
            }
            catch (Exception)
            {
                return false;
            }

            if (searchResult == null)
                return false;

            var groupName = string.Empty;

            for (var propertyCounter = 0; propertyCounter <= searchResult.Properties["memberOf"].Count - 1; propertyCounter++)
            {

                var match = Regex.Matches(searchResult.Properties["memberOf"][propertyCounter].ToString(), string.Format("={0},", permission.Detail.Name));

                if (match.Count > 0)
                {
                    groupName = permission.Detail.Name;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(groupName))
            {
                return false;
            }

            FormsAuthentication.SetAuthCookie(userName + "-" + groupName, false);
            return true;
        }

        public static string GetUserSecurityGroup(HttpRequestBase Request)
        {
            var authCookie = Request.Cookies[".ProductionAuthCookie"];

            if (authCookie != null)
            {
                var authCookieName = FormsAuthentication.Decrypt(authCookie.Value).Name;

                if (authCookieName != "PermissionDenied")
                    return authCookieName.Substring(authCookieName.IndexOf("-") + 1);
            }

            return string.Empty;
        }

        public static bool VerifyUser(string username, string password)
        {
            bool isValid = false;

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, @"MEDOCHEMIE"))
            {
                isValid = pc.ValidateCredentials(Helper.Helper.GetUserNameWithoutDomain(username), password);
            }

            return isValid;
        }
    }
}