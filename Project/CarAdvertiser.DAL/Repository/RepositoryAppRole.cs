using System;
using System.Threading.Tasks;
using System.Web;
using CarAdvertiser.DAL.Identity;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;

namespace CarAdvertiser.DAL.Repository
{
    public class RepositoryAppRole<T> : Repository<T>, IRepositoryAppRole<T> where T : AppRole
    {
        private bool _disposed;
        private IAppRoleProvider RoleProvider { get; }

        public RepositoryAppRole(ICarAdvertiserContext context) : base(context)
        {
            RoleProvider = new AppRoleProvider(context);
        }

        public RepositoryAppRole(ICarAdvertiserContext context, IAppRoleProvider appRoleProvider) : base(context)
        {
            RoleProvider = appRoleProvider;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Context?.Dispose();
                Context = null;
            }

            _disposed = true;
        }

        public void Create(string name)
        {
            RoleProvider.CreateRole(name);
        }

        public bool Delete(string name)
        {
            return RoleProvider.DeleteRole(name, false);
        }

        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            RoleProvider.AddUsersToRoles(usernames, roleNames);
        }

        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return RoleProvider.FindUsersInRole(roleName, usernameToMatch);
        }

        public string[] GetAllRoles()
        {
            return RoleProvider.GetAllRoles();
        }

        public string[] GetRolesForUser(string username)
        {
            return RoleProvider.GetRolesForUser(username.Trim().ToLower());
        }

        public string[] GetUsersInRole(string roleName)
        {
            return RoleProvider.GetUsersInRole(roleName);
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return RoleProvider.IsUserInRole(username.Trim().ToLower(), roleName);
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            RoleProvider.RemoveUsersFromRoles(usernames, roleNames);
        }

        public bool RoleExists(string roleName)
        {
            return RoleProvider.RoleExists(roleName);
        }
    }
}