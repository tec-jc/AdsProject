using AdsProject.BussinessEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.DataAccessLogic
{
    public class CategoryDAL
    {
        public static async Task<int> CreateAsync(Category category)
        {
            int result = 0;
            using(var dbContext = new ApplicationDbContext())
            {
                dbContext.Category.Add(category);
                result = await dbContext.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<int> UpdateAsync(Category category)
        {
            int result = 0;
            using(var dbContext = new ApplicationDbContext())
            {
                var categoryDb = await dbContext.Category.FirstOrDefaultAsync(c => c.Id == category.Id);
                if (categoryDb != null)
                {
                    categoryDb.Name = category.Name;
                    dbContext.Category.Update(categoryDb);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<int> DeleteAsync(Category category)
        {
            int result = 0;
            using( var dbContext = new ApplicationDbContext())
            {
                var categoryDb = await dbContext.Category.FirstOrDefaultAsync(c => c.Id == category.Id);
                if (categoryDb != null)
                {
                    dbContext.Category.Remove(categoryDb);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<Category> GetByIdAsync(Category category)
        {
            var categoryDb = new Category();
            using( var dbContext = new ApplicationDbContext())
            {
                categoryDb = await dbContext.Category.FirstOrDefaultAsync(c => c.Id == category.Id);
            }
            return categoryDb!;
        }

        public static async Task<List<Category>> GetAllAsync()
        {
            var categories = new List<Category>();
            using( var dbContext = new ApplicationDbContext())
            {
                categories = await dbContext.Category.ToListAsync();
            }
            return categories;
        }

        internal static IQueryable<Category> QuerySelect(IQueryable<Category> query, Category category)
        {
            if(category.Id > 0)
                query = query.Where(c => c.Id == category.Id);

            if(!string.IsNullOrWhiteSpace(category.Name))
                query = query.Where(c => c.Name.Contains(category.Name));

            query = query.OrderByDescending(c => c.Id);

            if (category.Top_Aux > 0)
                query = query.Take(category.Top_Aux).AsQueryable();

            return query;
        }

        public static async Task<List<Category>> SearchAsync(Category category)
        {
            var categories = new List<Category>();
            using(var dbContext = new ApplicationDbContext())
            {
                var select = dbContext.Category.AsQueryable();
                select = QuerySelect(select, category);
                categories = await select.ToListAsync();
            }
            return categories;
        }

    }
}
