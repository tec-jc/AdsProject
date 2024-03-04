using AdsProject.BussinessEntities;
using AdsProject.DataAccessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.BussinessLogic
{
    public class AdImageBL
    {
        public async Task<int> CreateAsync(AdImage adImage)
        {
            return await AdImageDAL.CreateAsync(adImage);
        }

        public async Task<int> UpdateAsync(AdImage adImage)
        {
            return await AdImageDAL.UpdateAsync(adImage);
        }

        public async Task<int> DeleteAsync(AdImage adImage)
        {
            return await AdImageDAL.DeleteAsync(adImage);
        }

        public async Task<AdImage> GetByIdAsync(AdImage adImage)
        {
            return await AdImageDAL.GetByIdAsync(adImage);
        }

        public async Task<List<AdImage>> GetAllAsync()
        {
            return await AdImageDAL.GetAllAsync();
        }

        public async Task<List<AdImage>> SearchAsync(AdImage adImage)
        {
            return await AdImageDAL.SearchAsync(adImage);
        }

        public async Task<List<AdImage>> SearchIncludeAdAsync(AdImage adImage)
        {
            return await AdImageDAL.SearchIncludeAdAsync(adImage);
        }
    }
}
