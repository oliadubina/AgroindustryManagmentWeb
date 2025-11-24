using AgroindustryManagementWeb.Models;
using AgroindustryManagementWeb.Services.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgroindustryManagementWeb.Controllers
{
    public class WorkerTaskController : Controller
    {
        private readonly IAGDatabaseService _databaseService;
        public WorkerTaskController(IAGDatabaseService databaseService)
            {
                _databaseService = databaseService;
            }
            public IActionResult Index(string searchDate)
            {
                try
                {
                    ViewBag.CurrentFilter = searchDate;
                    var workerTasks= _databaseService.GetAllWorkerTasks(searchDate);
                    return View(workerTasks);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Помилка при завантаженні полів: " + ex.Message;
                    return View(Enumerable.Empty<WorkerTask>());
                }

            }

            public IActionResult Details(int id)
            {
                try
                {
                    var task = _databaseService.GetWorkerTaskById(id);
                    return View(task);
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
                ViewBag.WorkersList = new SelectList(_databaseService.GetAllWorkers(), "Id", "FirstName");
                ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
            return View();
            }
            [HttpPost]
            public IActionResult Create(WorkerTask workerTask)
            {
                if (!ModelState.IsValid)
                {
                //var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
                ViewBag.WorkersList = new SelectList(_databaseService.GetAllWorkers(), "Id", "FirstName");
                    ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
                    return View(workerTask);
                }
                try
                {
                    

                    _databaseService.AddWorkerTask(workerTask);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Помилка при додаванні: " + ex.Message);
                    ViewBag.WorkersList = new SelectList(_databaseService.GetAllWorkers(), "Id", "FirstName");
                    ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
                    return View(workerTask);
                }
            }
            public IActionResult Edit(int id)
            {
                try
                {
                    ViewBag.WorkersList = new SelectList(_databaseService.GetAllWorkers(), "Id", "FirstName");
                    ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
                    var workerTask = _databaseService.GetWorkerTaskById(id);
                    return View(workerTask);
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
            public IActionResult Edit(int id, WorkerTask workerTask)
            {
                if (id!=workerTask.Id)
                {
                    TempData["ErrorMessage"] = "Id не співпадають";
                    return RedirectToAction("Index");
                }
                if (!ModelState.IsValid)
                {
                ViewBag.WorkersList = new SelectList(_databaseService.GetAllWorkers(), "Id", "FirstName");
                ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
                return View(workerTask);
                }
                try
                {
                    _databaseService.UpdateWorkerTask(workerTask);
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
                ViewBag.WorkersList = new SelectList(_databaseService.GetAllWorkers(), "Id", "FirstName");
                ViewBag.FieldsList = new SelectList(_databaseService.GetAllFields(), "Id", "Area");
                return View(workerTask);
                }

            }
            
            public IActionResult Delete(int id)
            {
                try
                {
                    var workerTask = _databaseService.GetWorkerTaskById(id);
                    return View(workerTask);
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
                    _databaseService.DeleteWorkerTask(id);
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


