using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdsProject.BussinessLogic;
using AdsProject.BussinessEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace AdsProject.GraphicUserInterface.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        // instancias de acceso a las clases BL
        UserBL userBL = new UserBL();
        RoleBL roleBL = new RoleBL();

        // acción que muestra el listado de registros
        public async Task<IActionResult> Index(User user = null)
        {
            if(user == null)
                user = new User();
            if(user.Top_Aux == 0)
                user.Top_Aux = 10; // setear la cantidad de registros a mostrar predeterminadamente
            else if(user.Top_Aux == -1)
                user.Top_Aux = 0;

            var users = await userBL.SearchIncludeRoleAsync(user);
            ViewBag.Top = user.Top_Aux;
            ViewBag.Roles = await roleBL.GetAllAsync();
            return View(users);
        }

        // acción que muestra el detalle de un registro
        public async Task<IActionResult> Details(int id)
        {
            var user = await userBL.GetByIdAsync(new User { Id = id });
            user.Role = await roleBL.GetByIdAsync(new Role { Id = user.IdRole });
            return View(user);
        }

        // acción que muestra el formulario para agregar un nuevo registro
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await roleBL.GetAllAsync();
            ViewBag.Error = "";
            return View();
        }

        // acción que recibe los datos del formulario y los envía a la bd
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                int result = await userBL.CreateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Roles = await roleBL.GetAllAsync();
                return View(user);
            }
        }

        // acción que muestra los datos cargados en el formulario para modificar
        public async Task<IActionResult> Edit(int id)
        {
            var userDb = await userBL.GetByIdAsync(new User { Id = id });
            ViewBag.Roles = await roleBL.GetAllAsync();
            ViewBag.Error = "";
            return View(userDb);
        }

        // acción que recibe los datos modificados y los envía a la bd
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            try
            {
                int result = await userBL.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Roles = await roleBL.GetAllAsync();
                return View(user);
            }
        }

        // acción que muestra los datos para confirmar la eliminación
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userBL.GetByIdAsync(new User { Id = id });
            user.Role = await roleBL.GetByIdAsync(new Role { Id = user.IdRole });
            ViewBag.Error = "";
            return View(user);
        }

        // acción que recibe la confirmación y elimina los datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, User user)
        {
            try
            {
                int result = await userBL.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                var userDb = await userBL.GetByIdAsync(user);
                if(userDb == null)
                    userDb = new User();
                if(userDb.Id > 0)
                    userDb.Role = await roleBL.GetByIdAsync(new Role { Id=userDb.IdRole });
                return View(userDb);
            }
        }

        // acción que muestra el formulario de inicio de sesión
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.Url = returnUrl;
            ViewBag.Error = "";
            return View();
        }

        // acción que recibe los datos de inicio de sesión y ejecuta la autenticación
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User user, string returnUrl = null)
        {
            try
            {
                var userDb = await userBL.LoginAsync(user);
                if(userDb != null && userDb.Id > 0 && userDb.Login == user.Login)
                {
                    userDb.Role = await roleBL.GetByIdAsync(new Role { Id = userDb.IdRole });
                    var claims = new[] {new Claim(ClaimTypes.Name, userDb.Login),
                                new Claim(ClaimTypes.Role, userDb.Role.Name)};
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                }
                else
                    throw new Exception("Credenciales de usuario incorrectas");
                
                if(!string.IsNullOrWhiteSpace(returnUrl)) 
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Url = returnUrl;
                ViewBag.Error = ex.Message;
                return View(new User { Login = user.Login});
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        //acción que muestra el formulario para cambiar contraseña
        public async Task<IActionResult> ChangePassword()
        {
            var users = await userBL.SearchAsync(new User { Login = User.Identity.Name, Top_Aux = 1});
            var actualUser = users.FirstOrDefault();
            ViewBag.Error = "";
            return View(actualUser);
        }

        //acción que recibe los datos de la nueva contraseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(User user, string oldPassword)
        {
            try
            {
                int result = await userBL.ChangePasswordAsync(user, oldPassword);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "User");
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                var users = await userBL.SearchAsync(new User { Login = User.Identity.Name, Top_Aux = 1});
                var actualUser = users.FirstOrDefault();
                return View(actualUser);
            }
        }
    }
}
