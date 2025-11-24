using AgroindustryManagementWeb.Models;
using AgroindustryManagementWeb.Services.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Reflection.PortableExecutable;

namespace AgroindustryManagementWeb.Controllers
{
    public class MachineController : Controller
    {
        private readonly IAGDatabaseService _databaseService;
        public MachineController(IAGDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public IActionResult Index()
        {
            try
            {
                var machines = _databaseService.GetAllMachines();
                return View(machines);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"]="Помилка при завантаженні: "+ex.Message;
                return View(Enumerable.Empty<Machine>());
            }

        }
        public IActionResult Details(int id)
        {
            try
            {
                var machine = _databaseService.GetMachineById(id);
                return View(machine);
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }

        }
        public IActionResult Create()
        {
            ViewBag.ResourceList = new SelectList(_databaseService.GetAllResources(), "Id", "CultureType");
            ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Machine newMachine)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ResourceList = new SelectList(_databaseService.GetAllResources(), "Id", "CultureType");
                ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
                return View(newMachine);
            }
            try
            {
                _databaseService.AddMachine(newMachine);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Помилка при додаванні" + ex.Message);
                ViewBag.ResourceList = new SelectList(_databaseService.GetAllResources(), "Id", "CultureType");
                ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
                return View(newMachine);
            }
        }
        public IActionResult Edit(int id)
        {
            try
            {
                var machine = _databaseService.GetMachineById(id);
                ViewBag.ResourceList = new SelectList(_databaseService.GetAllResources(), "Id", "CultureType");
                ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
                return View(machine);
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");

            }
        }
        [HttpPost]
        public IActionResult Edit(int id, Machine machine)
        {
            if (id!=machine.Id)
            {
                TempData["ErrorMessage"]="Id не співпадають";
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.ResourceList = new SelectList(_databaseService.GetAllResources(), "Id", "CultureType");
                ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
                return View(machine);
            }
            try
            {
                _databaseService.UpdateMachine(machine);
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Помилка при редагуванні" + ex.Message);
                ViewBag.ResourceList = new SelectList(_databaseService.GetAllResources(), "Id", "CultureType");
                ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
                return View(machine);
            }
        }
        public IActionResult Delete(int id)
        {
            try
            {
                var machine = _databaseService.GetMachineById(id);
                return View(machine);
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
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
                _databaseService.DeleteMachine(id);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"]="Помилка при видаленні: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
