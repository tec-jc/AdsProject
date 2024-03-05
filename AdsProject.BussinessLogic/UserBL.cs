using AdsProject.BussinessEntities;
using AdsProject.DataAccessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.BussinessLogic
{
    public class UserBL
    {
        public async Task<int> CreateAsync(User user)
        {
            return await UserDAL.CreateAsync(user);
        }

        public async Task<int> UpdateAsync(User user)
        {
            return await UserDAL.UpdateAsync(user);
        }

        public async Task<int> DeleteAsync(User user)
        {
            return await UserDAL.DeleteAsync(user);
        }

        public async Task<User> GetByIdAsync(User user)
        {
            return await UserDAL.GetByIdAsync(user);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await UserDAL.GetAllAsync();
        }

        public async Task<List<User>> SearchAsync(User user)
        {
            return await UserDAL.SearchAsync(user);
        }

        public async Task<List<User>> SearchIncludeRoleAsync(User user)
        {
            return await UserDAL.SearchIncludeRoleAsync(user);
        }

        public async Task<User> LoginAsync(User user)
        {
            return await UserDAL.LoginAsync(user);
        }

        public async Task<int> ChangePasswordAsync(User user, string oldPassword)
        {
            return await UserDAL.ChangePasswordAsync(user, oldPassword);
        }
    }
}
