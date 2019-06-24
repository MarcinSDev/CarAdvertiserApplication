using System;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;

namespace CarAdvertiser.DAL.Identity
{
    public class AppRoleProvider : RoleProvider, IAppRoleProvider
    {
        private readonly ICarAdvertiserContext _context;
        public AppRoleProvider()
        {
            _context = new CarAdvertiserContext();
        }

        public AppRoleProvider(ICarAdvertiserContext context)
        {
            _context = context;
        }

        public override string ApplicationName { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (string.IsNullOrEmpty(name)) name = "AppRoleProvider";
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Application Role Provider");
            }

            base.Initialize(name, config);

            ApplicationName = GetConfigValue(config["applicationName"],
                System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            return string.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            IQueryable<AppRole> roles = _context.GetDbSet<AppRole>().Where(x => roleNames.Contains(x.Name));
            if (!roles.Any()) return;

            foreach (string username in usernames)
            {
                AppUserV2 user = _context.GetDbSet<AppUserV2>().SingleOrDefault(x => x.UserName.Equals(username));
                if (user == null) continue;
                foreach (AppRole role in roles)
                {
                    if (!IsUserInRole(user.UserName, role.Name))
                    {
                        _context.GetDbSet<AppUserRole>().Add(new AppUserRole
                        {
                            UserId = user.Id,
                            RoleId = role.Id
                        });
                    }
                }
            }
        }

        public override void CreateRole(string roleName)
        {
            if (!_context.GetDbSet<AppRole>().Any(x => x.Name.ToLower().Equals(roleName.Trim().ToLower())))
            {
                _context.GetDbSet<AppRole>().Add(new AppRole
                {
                    Name = roleName.Trim()
                });
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            AppRole role = _context.GetDbSet<AppRole>()
                .SingleOrDefault(x => x.Name.ToLower().Equals(roleName.Trim().ToLower()));
            if (role == null) return false;

            IQueryable<AppUserRole> userRoles = _context.GetDbSet<AppUserRole>().Where(x => x.RoleId.Equals(role.Id));
            foreach (AppUserRole appUserRole in userRoles)
            {
                _context.GetDbSet<AppUserRole>().Remove(appUserRole);
            }

            _context.GetDbSet<AppRole>().Remove(role);

            return true;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            AppRole role = _context.GetDbSet<AppRole>()
                .SingleOrDefault(x => x.Name.ToLower().Equals(roleName.Trim().ToLower()));
            if (role == null) return new string[] { };

            IQueryable<int> users = _context.GetDbSet<AppUserV2>()
                .Where(x => x.UserName.ToLower().Contains(usernameToMatch.Trim().ToLower()))
                .Select(x => x.Id); //get the user list where input 'usernameToMatch' is contained, not equals
            if (!users.Any()) return new string[] { };
            
            IQueryable<int> userRoles = _context.GetDbSet<AppUserRole>()
                .Where(x => x.RoleId.Equals(role.Id) && users.Contains(x.UserId))
                .Select(x => x.UserId); //get the matches from connection table where role ID matches the user ID list
            if (!userRoles.Any()) return new string[] { };
            
            return _context.GetDbSet<AppUserV2>().Where(x => userRoles.Contains(x.Id)).Select(x => x.UserName).ToArray();
        }

        public override string[] GetAllRoles()
        {
            return _context.GetDbSet<AppRole>().Select(x => x.Name).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            AppUserV2 user = _context.GetDbSet<AppUserV2>()
                .SingleOrDefault(x => x.UserName.ToLower().Equals(username.Trim().ToLower()));
            if (user == null) return new string[] { };

            IQueryable<int> userRoles = _context.GetDbSet<AppUserRole>().Where(x => x.UserId.Equals(user.Id)).Select(x => x.RoleId);//from the connection table we get all the role IDs that user has
            
            IQueryable<string> roles = _context.GetDbSet<AppRole>().Where(x => userRoles.Contains(x.Id)).Select(x => x.Name);//we get all the role names from the role table that matches the role ID list from the previous method
            
            if (!userRoles.Any() || !roles.Any())
            {
                return new string[] { };
            }

            return roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            AppRole role = _context.GetDbSet<AppRole>()
                .SingleOrDefault(x => x.Name.ToLower().Equals(roleName.Trim().ToLower()));
            if (role == null) return new string[] { };

            IQueryable<int> userRoles = _context.GetDbSet<AppUserRole>().Where(x => x.RoleId.Equals(role.Id)).Select(x => x.UserId);
            
            return _context.GetDbSet<AppUserV2>().Where(x => userRoles.Contains(x.Id)).Select(x => x.UserName)
                .ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            AppUserV2 user = _context.GetDbSet<AppUserV2>()
                .SingleOrDefault(x => x.UserName.ToLower().Equals(username.Trim().ToLower()));
            if (user == null) return false;

            AppRole role = _context.GetDbSet<AppRole>()
                .SingleOrDefault(x => x.Name.ToLower().Equals(roleName.Trim().ToLower()));
            if (role == null) return false;

            return _context.GetDbSet<AppUserRole>().Any(x => x.UserId.Equals(user.Id) && x.RoleId.Equals(role.Id));
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            IQueryable<int> roles = _context.GetDbSet<AppRole>().Select(x => x.Id);
            IQueryable<int> users = _context.GetDbSet<AppUserV2>().AsQueryable().Select(x => x.Id);
            IQueryable<AppUserRole> userRoles = _context.GetDbSet<AppUserRole>().Where(x => roles.Contains(x.RoleId) && users.Contains(x.UserId));
            foreach (AppUserRole appUserRole in userRoles)
            {
                _context.GetDbSet<AppUserRole>().Remove(appUserRole);
            }
        }

        public override bool RoleExists(string roleName)
        {
            return _context.GetDbSet<AppRole>().Any(x => x.Name.Equals(roleName));
        }
    }
}