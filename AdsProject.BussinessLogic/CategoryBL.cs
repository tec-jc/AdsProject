using AdsProject.BussinessEntities;
using AdsProject.DataAccessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.BussinessLogic
{
    public class CategoryBL
    {
        public async Task<int> CreateAsync(Category category)
        {
            return await CategoryDAL.CreateAsync(category);
        }

        public async Task<int> UpdateAsync(Category category)
        {
            return await CategoryDAL.UpdateAsync(category);
        }

        public async Task<int> DeleteAsync(Category category)
        {
            return await CategoryDAL.DeleteAsync(category);
        }

        public async Task<Category> GetByIdAsync(Category category)
        {
            return await CategoryDAL.GetByIdAsync(category);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await CategoryDAL.GetAllAsync();
        }

        public async Task<List<Category>> SearchAsync(Category category)
        {
            return await CategoryDAL.SearchAsync(category);
        }
    }
}
