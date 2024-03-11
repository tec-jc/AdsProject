using AdsProject.BussinessEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdsProject.DataAccessLogic
{
    public class UserDAL
    {
        private static void EncryptMD5(User user)
        {
            using(var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(user.Password));
                var encryptedStr = "";
                for(int i = 0; i < result.Length; i++)
                {
                    encryptedStr += result[i].ToString("x2").ToLower();
                }
                user.Password = encryptedStr;
            }
        }

        private static async Task<bool> ExistsLogin(User user, ApplicationDbContext context)
        {
            bool result = false;
            var userLoginExists = await context.User.FirstOrDefaultAsync(u => u.Login == user.Login && u.Id != user.Id);
            if (userLoginExists != null && userLoginExists.Id > 0 && userLoginExists.Login == user.Login)
                result = true;
            
            return result;
        }

        public static async Task<int> CreateAsync(User user)
        {
            int result = 0;
            using(var dbContext = new ApplicationDbContext())
            {
                bool existsLogin = await ExistsLogin(user, dbContext);
                if (existsLogin == false)
                {
                    user.RegistrationDate = DateTime.Now;
                    EncryptMD5(user);
                    dbContext.User.Add(user);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("El nombre de usuario ya existe");
            }
            return result;
        }

        public static async Task<int> UpdateAsync(User user)
        {
            int result = 0;
            using(var dbContext = new ApplicationDbContext())
            {
                bool existsLogin = await ExistsLogin(user, dbContext);
                if (existsLogin == false)
                {
                    var userDb = await dbContext.User.FirstOrDefaultAsync(u => u.Id == user.Id);
                    userDb.IdRole = user.IdRole;
                    userDb.Name = user.Name;
                    userDb.LastName = user.LastName;
                    userDb.Status = user.Status;
                    userDb.Login = user.Login;

                    dbContext.User.Update(userDb);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("El nombre de usuario ya existe");
            }
            return result;
        }

        public static async Task<int> DeleteAsync(User user)
        {
            int result = 0;
            using(var dbContext = new ApplicationDbContext())
            {
                var userDb = await dbContext.User.FirstOrDefaultAsync(u => u.Id == user.Id);
                dbContext.User.Remove(userDb);
                result = await dbContext.SaveChangesAsync();
            }
            return result;
        }

        public static async Task<User> GetByIdAsync(User user)
        {
            var userDb = new User();
            using(var dbContext = new ApplicationDbContext())
            {
                userDb = await dbContext.User.FirstOrDefaultAsync(u => u.Id == user.Id);
            }
            return userDb!;
        }

        public static async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();
            using(var dbContext = new ApplicationDbContext())
            {
                users = await dbContext.User.ToListAsync();
            }
            return users;
        }

        internal static IQueryable<User> QuerySelect(IQueryable<User> query, User user)
        {
            if (user.Id > 0)
                query = query.Where(u => u.Id == user.Id);

            if (user.IdRole > 0)
                query = query.Where(u => u.IdRole == user.IdRole);

            if (!string.IsNullOrWhiteSpace(user.Name))
                query = query.Where(u => u.Name.Contains(user.Name));

            if (!string.IsNullOrWhiteSpace(user.LastName))
                query = query.Where(u => u.LastName.Contains(user.LastName));

            if (!string.IsNullOrWhiteSpace(user.Login))
                query = query.Where(u => u.Login.Contains(user.Login));

            if (user.Status > 0)
                query = query.Where(u => u.Status == user.Status);

            if (user.RegistrationDate.Year > 1000)
            {
                DateTime inicialDate = new DateTime(user.RegistrationDate.Year, user.RegistrationDate.Month, user.RegistrationDate.Day, 0, 0, 0);
                DateTime finalDate = inicialDate.AddDays(1).AddMilliseconds(-1);
                query = query.Where(u => u.RegistrationDate >= inicialDate && u.RegistrationDate <= finalDate);
            }

            query = query.OrderByDescending(u => u.Id).AsQueryable();

            if (user.Top_Aux > 0)
                query = query.Take(user.Top_Aux).AsQueryable();

            return query;
        }

        public static async Task<List<User>> SearchAsync(User user)
        {
            var users = new List<User>();
            using(var dbContext = new ApplicationDbContext())
            {
                var select = dbContext.User.AsQueryable();
                select = QuerySelect(select, user);
                users = await select.ToListAsync();
            }
            return users;
        }

        public static async Task<List<User>> SearchIncludeRoleAsync(User user)
        {
            var users = new List<User>();
            using (var dbContext = new ApplicationDbContext())
            {
                var select = dbContext.User.AsQueryable();
                select = QuerySelect(select, user).Include(u => u.Role).AsQueryable();
                users = await select.ToListAsync();
            }
            return users;
        }

        public static async Task<User> LoginAsync(User user)
        {
            var userDb = new User();
            using(var dbContext = new ApplicationDbContext())
            {
                EncryptMD5(user);
                userDb = await dbContext.User.FirstOrDefaultAsync(u => u.Login == user.Login && 
                u.Password == user.Password && u.Status == (byte)User_Status.ACTIVO);
            }
            return userDb!;
        }

        public static async Task<int> ChangePasswordAsync(User user, string oldPassword)
        {
            int result = 0;
            var userOldPass = new User { Password = oldPassword };
            EncryptMD5(userOldPass);
            using(var dbContext = new ApplicationDbContext())
            {
                var userDb = await dbContext.User.FirstOrDefaultAsync(u => u.Id == user.Id);
                if (userOldPass.Password == userDb.Password)
                {
                    EncryptMD5(user);
                    userDb.Password = user.Password;
                    dbContext.User.Update(userDb);
                    result = await dbContext.SaveChangesAsync();
                }
                else
                    throw new Exception("La contraseña actual es inválida");
            }
            return result;
        }
    }
}
