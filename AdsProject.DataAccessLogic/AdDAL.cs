using AdsProject.BussinessEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.DataAccessLogic
{
    public class AdDAL
    {
        public static async Task<int> CreateAsync(Ad ad)
        {
            int result = 0;
            using (var dbContext = new ApplicationDbContext())
            {
                dbContext.Ad.Add(ad);
                await dbContext.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<int> UpdateAsync(Ad ad)
        {
            int result = 0;
            using (var dbContext = new ApplicationDbContext())
            {
                var adDb = await dbContext.Ad.FirstOrDefaultAsync(a => a.Id == ad.Id);
                if(adDb != null)
                {
                    adDb.IdCategory = ad.IdCategory;
                    adDb.Title = ad.Title;
                    adDb.Description = ad.Description;
                    adDb.Price = ad.Price;
                    adDb.RegistrationDate = ad.RegistrationDate;
                    adDb.State = ad.State;
                    
                    dbContext.Update(ad);
                    result = await dbContext.SaveChangesAsync();
                }                
            }
            return result;
        }

        public static async Task<int> DeleteAsync(Ad ad)
        {
            int result = 0;
            using (var dbContext = new ApplicationDbContext())
            {
                var adDb = await dbContext.Ad.FirstOrDefaultAsync(a => a.Id == ad.Id);
                if(adDb != null)
                {
                    dbContext.Ad.Remove(adDb);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<Ad> GetByIdAsync(Ad ad)
        {
            var adDb = new Ad();
            using(var dbContext = new ApplicationDbContext())
            {
                adDb = await dbContext.Ad.FirstOrDefaultAsync(a => a.Id == ad.Id);
            }
            return adDb!;
        }

        public static async Task<List<Ad>> GetAllAsync()
        {
            var ads = new List<Ad>();
            using (var dbContext = new ApplicationDbContext())
            {
                ads = await dbContext.Ad.ToListAsync();
            }
            return ads;
        }

        internal static IQueryable<Ad> QuerySelect(IQueryable<Ad> query, Ad ad)
        {
            if (ad.Id > 0)
                query = query.Where(a => a.Id == ad.Id);

            if (ad.IdCategory > 0)
                query = query.Where(a => a.IdCategory == ad.IdCategory);

            if (!string.IsNullOrWhiteSpace(ad.Title))
                query = query.Where(a => a.Title.Contains(ad.Title));

            if (!string.IsNullOrWhiteSpace(ad.Description))
                query = query.Where(a => a.Description.Contains(ad.Description));

            if (ad.Price > 0)
                query = query.Where(a => a.Price == ad.Price);

            if (ad.RegistrationDate.Year > 1900)
            {
                DateTime initialDate = new DateTime(ad.RegistrationDate.Year,
                    ad.RegistrationDate.Month, ad.RegistrationDate.Day, 0, 0, 0);
                DateTime finalDate = initialDate.AddDays(1).AddMicroseconds(-1);

                query = query.Where(a => a.RegistrationDate > initialDate &&
                    a.RegistrationDate <= finalDate);
            }

            if (!string.IsNullOrWhiteSpace(ad.State))
                query = query.Where(a => a.State.Contains(ad.State));

            query = query.OrderByDescending(a => a.Id).AsQueryable();

            if (ad.Top_Aux > 0)
                query = query.Take(ad.Top_Aux).AsQueryable();

            return query;
        }

        public static async Task<List<Ad>> SearchAsync(Ad ad)
        {
            var ads = new List<Ad>();
            using (var dbContext = new ApplicationDbContext())
            {
                var select = dbContext.Ad.AsQueryable();
                select = QuerySelect(select, ad);
                ads = await select.ToListAsync();
            }
            return ads;
        }

        public static async Task<List<Ad>> SearchIncludeCategoryAsync(Ad ad)
        {
            var ads = new List<Ad>();
            using (var dbContext = new ApplicationDbContext())
            {
                var select = dbContext.Ad.AsQueryable();
                select = QuerySelect(select, ad).Include(a => a.Category).AsQueryable();
                ads = await select.ToListAsync();
            }
            return ads;
        }
    }
}
