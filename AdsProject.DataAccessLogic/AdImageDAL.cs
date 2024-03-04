using AdsProject.BussinessEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.DataAccessLogic
{
    public class AdImageDAL
    {
        public static async Task<int> CreateAsync(AdImage adImage)
        {
            int result = 0;
            using (var dbContext = new ApplicationDbContext())
            {
                dbContext.Add(adImage);
                result = await dbContext.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<int> UpdateAsync(AdImage adImage)
        {
            int result = 0;
            using (var dbContext = new ApplicationDbContext())
            {
                var adImageDB = await dbContext.AdImage.FirstOrDefaultAsync(s => s.Id == adImage.Id);
                if (adImageDB != null)
                {
                    adImageDB.IdAd = adImage.IdAd;
                    adImageDB.Path = adImage.Path;
                    dbContext.Update(adImageDB);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<int> DeleteAsync(AdImage adImage)
        {
            int result = 0;
            using (var dbContext = new ApplicationDbContext())
            {
                var adImageDB = await dbContext.AdImage.FirstOrDefaultAsync(s => s.Id == adImage.Id);
                if (adImageDB != null)
                {
                    dbContext.AdImage.Remove(adImageDB);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<AdImage> GetByIdAsync(AdImage adImage)
        {
            var adImageDB = new AdImage();
            using (var dbContext = new ApplicationDbContext())
            {
                adImageDB = await dbContext.AdImage.FirstOrDefaultAsync(s => s.Id == adImage.Id);
            }
            return adImageDB!;
        }

        public static async Task<List<AdImage>> GetAllAsync()
        {
            var images = new List<AdImage>();
            using (var dbContext = new ApplicationDbContext())
            {
                images = await dbContext.AdImage.ToListAsync();
            }
            return images;
        }

        internal static IQueryable<AdImage> QuerySelect(IQueryable<AdImage> query, AdImage adImage)
        {
            if (adImage.Id > 0)
                query = query.Where(s => s.Id == adImage.Id);

            if (adImage.IdAd > 0)
                query = query.Where(s => s.IdAd == adImage.IdAd);

            if (!string.IsNullOrWhiteSpace(adImage.Path))
                query = query.Where(s => s.Path.Contains(adImage.Path));

            query = query.OrderByDescending(s => s.Id).AsQueryable();

            if (adImage.Top_Aux > 0)
                query = query.Take(adImage.Top_Aux).AsQueryable();

            return query;
        }

        public static async Task<List<AdImage>> SearchAsync(AdImage adImage)
        {
            var images = new List<AdImage>();
            using (var dbContext = new ApplicationDbContext())
            {
                var select = dbContext.AdImage.AsQueryable();
                select = QuerySelect(select, adImage);
                images = await select.ToListAsync();
            }
            return images;
        }

        public static async Task<List<AdImage>> SearchIncludeAdAsync(AdImage adImage)
        {
            var images = new List<AdImage>();
            using (var dbContext = new ApplicationDbContext())
            {
                var select = dbContext.AdImage.AsQueryable();
                select = QuerySelect(select, adImage).Include(s => s.Ad).AsQueryable();
                images = await select.ToListAsync();
            }
            return images;
        }
    }
}
