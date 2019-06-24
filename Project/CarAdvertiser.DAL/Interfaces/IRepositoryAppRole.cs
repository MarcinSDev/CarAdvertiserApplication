using System;
using System.Threading.Tasks;
using CarAdvertiser.DAL.Identity;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface IRepositoryAppRole<T> : IRepository<T>, IDisposable where T : AppRole
    {
        void Create(string name);
        bool Delete(string name);
        void AddUsersToRoles(string[] usernames, string[] roleNames);
        string[] FindUsersInRole(string roleName, string usernameToMatch);
        string[] GetAllRoles();
        string[] GetRolesForUser(string username);
        string[] GetUsersInRole(string roleName);
        bool IsUserInRole(string username, string roleName);
        void RemoveUsersFromRoles(string[] usernames, string[] roleNames);
        bool RoleExists(string roleName);
    }
}