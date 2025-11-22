using Microsoft.AspNetCore.Mvc;
using AgroindustryManagementWeb.Models;
using AgroindustryManagementWeb.Services.Database;

namespace AgroindustryManagementWeb.Controllers
{

    public class WorkerController : Controller
    {
        private readonly IAGDatabaseService _databaseService;
        public WorkerController(IAGDatabaseService databaseService)
        {
            _databaseService=databaseService;
        }
        public IActionResult Index()
        {
            try
            {
                var workers = _databaseService.GetAllWorkers();
                return View(workers);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"]="Помилка при завантаженні: "+ex.Message;
                return View(Enumerable.Empty<Worker>());
            }
        }
        public IActionResult Details(int id)
        {
            try
            {
                var worker = _databaseService.GetWorkerById(id);
                return View(worker);
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
            ViewBag.AvailableTasks = _databaseService.GetAllWorkerTasks();

            return View();
        }
        [HttpPost]
        public IActionResult Create(Worker worker, List<int> SelectedTaskIds)
        {
            if (!ModelState.IsValid)
            {
                return View(worker);
            }
            try
            {
                worker.Tasks = new List<WorkerTask>();
                if (SelectedTaskIds != null)
                {
                    foreach (var taskId in SelectedTaskIds)
                    {
                        worker.Tasks.Add(_databaseService.GetWorkerTaskById(taskId));
                    }
                }
                _databaseService.AddWorker(worker);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Помилка при додаванні" + ex.Message);
                ViewBag.AvailableTasks = _databaseService.GetAllWorkerTasks();
                return View(worker);
            }
        }
        public IActionResult Edit(int workerId)
        {
            try
            {
                var worker = _databaseService.GetWorkerById(workerId);
                ViewBag.AvailableTasks = _databaseService.GetAllWorkerTasks();
                return View(worker);
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
        public IActionResult Edit(int id, Worker worker, List<int> SelectedTaskIds)
        {
            if (id!=worker.Id)
            {
                TempData["ErrorMessage"] = "Id не співпадають";
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                return View(worker);
            }
            try
            {
                worker.Tasks = new List<WorkerTask>();
                if (SelectedTaskIds != null)
                {
                    foreach (var taskId in SelectedTaskIds)
                    {
                        worker.Tasks.Add(_databaseService.GetWorkerTaskById(taskId));
                    }
                }
                _databaseService.UpdateWorker(worker);
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
                ViewBag.AvailableTasks = _databaseService.GetAllWorkerTasks();
                return View(worker);
            }

        }
        // GET: /Worker/Delete/5
        public IActionResult Delete(int workerId)
        {
            try
            {
                var worker = _databaseService.GetWorkerById(workerId);
                return View(worker);
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
                _databaseService.DeleteWorker(id);
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
