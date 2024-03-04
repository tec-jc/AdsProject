using AdsProject.BussinessEntities;
using AdsProject.BussinessLogic;
using AdsProject.GraphicUserInterface.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdsProject.GraphicUserInterface.Controllers
{
    public class AdController : Controller
    {
        // creación de objetos de acceso a la capa BL
        AdBL adBL = new AdBL();
        CategoryBL categoryBL = new CategoryBL();
        AdImageBL adImageBL = new AdImageBL();

        // Acción que muestra la página principal de anuncios
        public async Task<IActionResult> Index(Ad ad = null)
        {
            if (ad == null)
                ad = new Ad();
            if (ad.Top_Aux == 0)
                ad.Top_Aux = 10;
            else if (ad.Top_Aux == -1)
                ad.Top_Aux = 0;

            var ads = await adBL.SearchIncludeCategoryAsync(ad);
            var categories = await categoryBL.GetAllAsync();

            ViewBag.Top = ad.Top_Aux;
            ViewBag.Categories = categories;
            return View(ads);
        }

        // Acción que muestra los detalles de un registro
        public async Task<IActionResult> Details(int id)
        {
            var ad = await adBL.GetByIdAsync(new Ad { Id = id });
            ad.Category = await categoryBL.GetByIdAsync(new Category { Id = ad.IdCategory });
            ad.AdImages = await adImageBL.SearchAsync(new AdImage() { IdAd = ad.Id });
            return View(ad);
        }

        // Acción que muestra el formulario para crear una nueva empresa
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await categoryBL.GetAllAsync();
            ViewBag.Error = "";
            return View();
        }

        // Acción que recibe los datos del formulario y los envia a la bd
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ad ad, List<IFormFile> formFiles)
        {
            try
            {
                List<AdImage> images = new List<AdImage>(); // declaración de la lista para almacenar las imágenes

                foreach (IFormFile file in formFiles) // recorremos en caso que vengan dos o más imágenes
                {
                    AdImage adImage = new AdImage();
                    adImage.IdAd = ad.Id;
                    adImage.Path = await ImageHelper.SubirArchivo(file.OpenReadStream(), file.FileName);
                    images.Add(adImage);
                }

                ad.AdImages = images;
                ad.RegistrationDate = DateTime.Now;
                int result = await adBL.CreateAsync(ad);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Categories = await categoryBL.GetAllAsync();
                return View(ad);
            }
        }

        // GET: AdController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Acción que muestra los datos del registro para confirmar la eliminación
        public async Task<IActionResult> Delete(Ad ad)
        {
            var adDB = await adBL.GetByIdAsync(ad);
            adDB.Category = await categoryBL.GetByIdAsync(new Category { Id = adDB.IdCategory });
            ViewBag.Error = "";
            return View(adDB);
        }

        // Acción que recibe la confirmación para eliminar el registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Ad ad)
        {
            try
            {
                Ad adDB = await adBL.GetByIdAsync(ad);
                adDB.AdImages = await adImageBL.SearchAsync(new AdImage() { IdAd = adDB.Id });
                if (adDB.AdImages.Count() > 0)
                {
                    foreach (var adImage in adDB.AdImages)
                    {
                        await adImageBL.DeleteAsync(adImage);
                    }
                }
                int result = await adBL.DeleteAsync(adDB);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var adBD = await adBL.GetByIdAsync(ad);
                if (adBD == null)
                    adBD = new Ad();
                if (adBD.Id > 0)
                    adBD.Category = await categoryBL.GetByIdAsync(new Category { Id = adBD.IdCategory });
                return View(adBD);
            }
        }
    }
}
