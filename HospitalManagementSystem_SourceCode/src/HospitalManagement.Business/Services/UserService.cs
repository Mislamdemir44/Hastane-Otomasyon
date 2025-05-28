using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using HospitalManagement.Business.Interfaces;
using HospitalManagement.DataAccess.Interfaces;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<UserRole> _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IGenericRepository<User> userRepository,
            IGenericRepository<Role> roleRepository,
            IGenericRepository<UserRole> userRoleRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetSingleAsync(u => u.UserName == username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetSingleAsync(u => u.Email == email);
        }

        public async Task<bool> CreateUserAsync(User user, string password, List<string> roles)
        {
            // Check if username or email already exists
            if (await _userRepository.ExistsAsync(u => u.UserName == user.UserName || u.Email == user.Email))
            {
                return false;
            }

            // Hash password
            user.PasswordHash = HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;

            // Add user
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Assign roles
            if (roles != null && roles.Count > 0)
            {
                foreach (var roleName in roles)
                {
                    var role = await _roleRepository.GetSingleAsync(r => r.Name == roleName);
                    if (role != null)
                    {
                        await _userRoleRepository.AddAsync(new UserRole
                        {
                            UserId = user.Id,
                            RoleId = role.Id
                        });
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                return false;
            }

            // Check if username or email is being changed and if it conflicts with existing users
            if ((user.UserName != existingUser.UserName && await _userRepository.ExistsAsync(u => u.UserName == user.UserName)) ||
                (user.Email != existingUser.Email && await _userRepository.ExistsAsync(u => u.Email == user.Email)))
            {
                return false;
            }

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.IsActive = user.IsActive;
            existingUser.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(existingUser);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            // Soft delete
            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Verify current password
            if (user.PasswordHash != HashPassword(currentPassword))
            {
                return false;
            }

            // Update password
            user.PasswordHash = HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignRoleAsync(Guid userId, string roleName)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var role = await _roleRepository.GetSingleAsync(r => r.Name == roleName);
            if (role == null)
            {
                return false;
            }

            // Check if user already has this role
            if (await _userRoleRepository.ExistsAsync(ur => ur.UserId == userId && ur.RoleId == role.Id))
            {
                return true; // Already assigned
            }

            await _userRoleRepository.AddAsync(new UserRole
            {
                UserId = userId,
                RoleId = role.Id
            });
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRoleAsync(Guid userId, string roleName)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var role = await _roleRepository.GetSingleAsync(r => r.Name == roleName);
            if (role == null)
            {
                return false;
            }

            var userRole = await _userRoleRepository.GetSingleAsync(ur => ur.UserId == userId && ur.RoleId == role.Id);
            if (userRole == null)
            {
                return true; // Role not assigned
            }

            _userRoleRepository.Delete(userRole);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
        {
            var userRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == userId);
            var roleNames = new List<string>();

            foreach (var userRole in userRoles)
            {
                var role = await _roleRepository.GetByIdAsync(userRole.RoleId);
                if (role != null)
                {
                    roleNames.Add(role.Name);
                }
            }

            return roleNames;
        }

        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await _userRepository.GetSingleAsync(u => u.UserName == username && u.IsActive);
            if (user == null)
            {
                return false;
            }

            if (user.PasswordHash != HashPassword(password))
            {
                return false;
            }

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
