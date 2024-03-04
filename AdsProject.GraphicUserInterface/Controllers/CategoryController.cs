using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AdsProject.BussinessEntities;
using AdsProject.BussinessLogic;

namespace AdsProject.GraphicUserInterface.Controllers
{
    public class CategoryController : Controller
    {
        CategoryBL categoryBL = new CategoryBL();

        // acción muestra el listado de registros
        public async Task<IActionResult> Index(Category category = null)
        {
            if (category == null)
                category = new Category();
            if (category.Top_Aux == 0)
                category.Top_Aux = 10; // setear el número de registros a mostrar
            else if (category.Top_Aux == -1)
                category.Top_Aux = 0;

            var categories = await categoryBL.SearchAsync(category);
            ViewBag.Top = category.Top_Aux;

            return View(categories);
        }

        // acción que muestra los detalles de un registro
        public async Task<IActionResult> Details(int id)
        {
            var category = await categoryBL.GetByIdAsync(new Category { Id = id });
            return View(category);
        }

        // acción que muestra el formulario para agregar una nueva categoría
        public IActionResult Create()
        {
            ViewBag.Error = "";
            return View();
        }

        // acción que recibe los datos del formulario y los envía a la bd
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                int result = await categoryBL.CreateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(category);
            }
        }

        // // acción que muestra el formulario con los datos cargados para modificar
        public async Task<IActionResult> Edit(int id)
        {
            var category = await categoryBL.GetByIdAsync(new Category { Id = id });
            ViewBag.Error = "";
            return View(category);
        }

        // acción que recibe los datos modificados y los envía a la bd
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            try
            {
                int result = await categoryBL.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(category);
            }
        }

        // acción que muestra los datos para confirmar la eliminación
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryBL.GetByIdAsync(new Category { Id = id });
            ViewBag.Error = "";
            return View(category);
        }

        // acción que recibe la confirmación de eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Category category)
        {
            try
            {
                int result = await categoryBL.DeleteAsync(category);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(category);
            }
        }
    }
}
