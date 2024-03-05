using AdsProject.BussinessEntities;
using AdsProject.DataAccessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.BussinessLogic
{
    public class RoleBL
    {
        public async Task<int> CreateAsync(Role role)
        {
            return await RoleDAL.CreateAsync(role);
        }

        public async Task<int> UpdateAsync(Role role)
        {
            return await RoleDAL.UpdateAsync(role);
        }

        public async Task<int> DeleteAsync(Role role)
        {
            return await RoleDAL.DeleteAsync(role);
        }

        public async Task<Role> GetByIdAsync(Role role)
        {
            return await RoleDAL.GetByIdAsync(role);
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await RoleDAL.GetAllAsync();
        }

        public async Task<List<Role>> SearchAsync(Role role)
        {
            return await RoleDAL.SearchAsync(role);
        }
    }
}
