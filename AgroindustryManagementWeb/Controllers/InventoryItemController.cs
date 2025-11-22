using AgroindustryManagementWeb.Models;
using AgroindustryManagementWeb.Services.Database;
using Microsoft.AspNetCore.Mvc;

namespace AgroindustryManagementWeb.Controllers
{
    public class InventoryItemController : Controller
    {
        private readonly IAGDatabaseService _databaseService;
        public InventoryItemController(IAGDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public IActionResult Index()
        {
            try
            {
                var items = _databaseService.GetAllInventoryItems();
                return View(items);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Помилка при завантаженні: " + ex.Message;
                return View(Enumerable.Empty<InventoryItem>());
            }

        }

        public IActionResult Details(int id)
        {
            try
            {
                var item = _databaseService.GetInventoryItemById(id);
                return View(item);
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
        [HttpPost]
        public IActionResult Create(InventoryItem item) 
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            try
            {
                _databaseService.AddInventoryItem(item);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Помилка при додаванні: " + ex.Message);
                return View(item);
            }
        }
        public IActionResult Edit(int id)
        {
            try
            {
                var item = _databaseService.GetInventoryItemById(id);
                return View(item);
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
        [HttpPost]
        public IActionResult Edit(int id,InventoryItem item)
        {
            if (id!=item.Id)
            {
                TempData["ErrorMessage"] = "Id не співпадають";
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            try
            {
                _databaseService.UpdateInventoryItem(item);
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Помилка при оновленні: " + ex.Message);
                return View(item);
            }

        }
        public IActionResult Delete(int id)
        {
            try
            {
                var item = _databaseService.GetInventoryItemById(id);
                return View(item);
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
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _databaseService.DeleteInventoryItem(id);
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Помилка при видаленні: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
    

