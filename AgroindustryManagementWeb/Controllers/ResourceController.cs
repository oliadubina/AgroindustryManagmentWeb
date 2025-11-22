using AgroindustryManagementWeb.Models;
using AgroindustryManagementWeb.Services.Database;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace AgroindustryManagementWeb.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IAGDatabaseService _databaseService;
        public ResourceController(IAGDatabaseService databaseService)
        {
            _databaseService=databaseService;
        }
        public IActionResult Index()
        {
            try
            {
                var resources = _databaseService.GetAllResources();
                return View(resources);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"]="Помилка при завантаженні: "+ex.Message;
                return View(Enumerable.Empty<Resource>());

            }
        }
        public IActionResult Details(int id)
        {
            try
            {
                var resource = _databaseService.GetResourceById(id);
                return View(resource);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Create(Resource resource)
        {
            if (!ModelState.IsValid)
            {
                return View(resource);
            }
            try
            {
                _databaseService.AddResource(resource);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Помилка при додаванні: " +  ex.Message);
                return View(resource);
            }
        }
        public IActionResult Edit(int id)
        {
            try
            {
                var resource = _databaseService.GetResourceById(id);
                return View(resource);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
        }
        public IActionResult Edit(int id, Resource resource)
        {
            if (id!=resource.Id)
            {
                TempData["ErrorMessage"]="Id не співпадають";
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                return View(resource);
            }
            try
            {
                _databaseService.UpdateResource(resource);
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Помилка при оновленні"+ex.Message);
                return View(resource);
            }

        }
        public IActionResult Delete(int id)
        {
            try
            {
                var resource = _databaseService.GetResourceById(id);
                return View(resource);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }

        }
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _databaseService.DeleteResource(id);
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "Помилка при видаленні: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }

        }
    }
}
