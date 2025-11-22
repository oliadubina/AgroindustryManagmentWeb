using AgroindustryManagementWeb.Models;
using AgroindustryManagementWeb.Services.Database;
using Microsoft.AspNetCore.Mvc;

namespace AgroindustryManagementWeb.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IAGDatabaseService _databaseService;
        public WarehouseController(IAGDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public IActionResult Index()
        {
            try
            {
                var warehouses = _databaseService.GetAllWarehouses();
                return View(warehouses);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Помилка при завантаженні полів: " + ex.Message;
                return View(Enumerable.Empty<Warehouse>());
            }

        }

        public IActionResult Details(int id)
        {
            try
            {
                var warehouse = _databaseService.GetWarehouseById(id);
                return View(warehouse);
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
        public IActionResult Create(Warehouse warehouse)
        {
            if (!ModelState.IsValid)
            {
                return View(warehouse);
            }
            try
            {
                _databaseService.AddWarehouse(warehouse);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Помилка при додаванні: " + ex.Message);
                return View(warehouse);
            }
        }
        public IActionResult Edit(int id)
        {
            try
            {
                var warehouse = _databaseService.GetWarehouseById(id);
                return View(warehouse);
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
        public IActionResult Edit(int id, Warehouse warehouse)
        {
            if (id!=warehouse.Id)
            {
                TempData["ErrorMessage"] = "Id не співпадають";
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                return View(warehouse);
            }
            try
            {
                _databaseService.UpdateWarehouse(warehouse);
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
                return View(warehouse);
            }

        }

        public IActionResult Delete(int id)
        {
            try
            {
                var warehouse = _databaseService.GetWarehouseById(id);
                return View(warehouse);
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
                _databaseService.DeleteWarehouse(id);
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

