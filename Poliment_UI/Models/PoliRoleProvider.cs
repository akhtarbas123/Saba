using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Poliment_DL;
using Poliment_DL.Model;
using Poliment_UI;
using System.Configuration;

namespace Poliment_UI.Models
{
    public class PoliRoleProvider : RoleProvider
    {
        private UserDL userDL = new UserDL();
        private AdminDL adminDL = new AdminDL();
        private CommonDL commonDL = new CommonDL();
        string error = string.Empty;
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            AdminML adminML = new AdminML();
            UserML userML = new UserML();
            string role = string.Empty;
            try
            {
                adminML = adminDL.GetAdminByUserName(username);
                if(string.IsNullOrEmpty(adminML.ErrorMessage))
                {
                    role = adminML.AdminRole;
                }
                else
                {
                    userML = userDL.GetUserByUserName(username);
                    if(string.IsNullOrEmpty(userML.ErrorMessage))
                    {
                        role = userML.UserRole;
                    }
                }
            }
            catch(Exception ex)
            {
                error = ex.Message;
            }
            string[] resultS = { role };
            return resultS;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}