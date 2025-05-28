using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> CreateUserAsync(User user, string password, List<string> roles);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<bool> AssignRoleAsync(Guid userId, string roleName);
        Task<bool> RemoveRoleAsync(Guid userId, string roleName);
        Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
    }
}
