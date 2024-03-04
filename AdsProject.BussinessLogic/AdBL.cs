using AdsProject.BussinessEntities;
using AdsProject.DataAccessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.BussinessLogic
{
    public class AdBL
    {
        public async Task<int> CreateAsync(Ad ad)
        {
            return await AdDAL.CreateAsync(ad);
        }

        public async Task<int> UpdateAsync(Ad ad)
        {
            return await AdDAL.UpdateAsync(ad);
        }

        public async Task<int> DeleteAsync(Ad ad)
        {
            return await AdDAL.DeleteAsync(ad);
        }

        public async Task<Ad> GetByIdAsync(Ad ad)
        {
            return await AdDAL.GetByIdAsync(ad);
        }

        public async Task<List<Ad>> GetAllAsync()
        {
            return await AdDAL.GetAllAsync();
        }

        public async Task<List<Ad>> SearchAsync(Ad ad)
        {
            return await AdDAL.SearchAsync(ad);
        }

        public async Task<List<Ad>> SearchIncludeCategoryAsync(Ad ad)
        {
            return await AdDAL.SearchIncludeCategoryAsync(ad);
        }
    }
}
