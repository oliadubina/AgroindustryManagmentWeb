using AgroindustryManagementWeb.Models;
using AgroindustryManagementWeb.Services.Calculations;
using AgroindustryManagementWeb.Services.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace AgroindustryManagementWeb.Controllers
{
    public class FieldController : Controller
    {
        private readonly IAGDatabaseService _databaseService;
        private readonly IAGCalculationService _aGCalculationService;
        public FieldController(IAGDatabaseService databaseService,IAGCalculationService aGCalculationService)
        {
            _databaseService = databaseService;
            _aGCalculationService = aGCalculationService;
        }
        public IActionResult Index()
        {
            try
            {
                var fields = _databaseService.GetAllFields();
                return View(fields);
            }
            catch (Exception)
            {
                //TempData["ErrorMessage"] = "Помилка при завантаженні полів: " + ex.Message;
                return View(Enumerable.Empty<Field>());
            }

        }

        public IActionResult Details(int fieldId)
        {
            try
            {
                var field = _databaseService.GetFieldById(fieldId);
                var resource = _databaseService.GetResourceByCultureType(field.Culture);
                var workersCount= _aGCalculationService.CalculateRequiredWorkers(resource, field.Area);
                ViewBag.RequiredWorkers = _aGCalculationService.CalculateRequiredWorkers(resource, field.Area);
                ViewBag.RequiredMachinery = _aGCalculationService.CalculateRequiredMachineryCount(resource);
                ViewBag.SeedAmount = _aGCalculationService.CalculateSeedAmount(resource, field.Area);
                ViewBag.FertilizerAmount = _aGCalculationService.CalculateFertilizerAmount(resource, field.Area);
                ViewBag.EstimatedYield = _aGCalculationService.EstimateYield(resource, field.Area);
                
                double fuelConsumption = 0;
                foreach (var machine in field.Machines)
                {
                    fuelConsumption += _aGCalculationService.EstimateFuelConsumption(machine, field.Area);
                }
                ViewBag.EstimatedFuelConsumption = fuelConsumption;
                double duration = 0;
                foreach (var machine in field.Machines)
                {
                    duration += _aGCalculationService.EstimateWorkDuration(field.Area, workersCount, machine, resource);
                }
                ViewBag.EstimateWorkDuration = Math.Ceiling(duration);
                return View(field);
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
            
            ViewBag.AvailableWorkers = _databaseService.GetAllWorkers();
            ViewBag.AvailableMachines = _databaseService.GetAllMachines();
            ViewBag.AvailableTasks = _databaseService.GetAllWorkerTasks();

            return View();
        }
        [HttpPost]
        public IActionResult Create(Field field, List<int> SelectedWorkerIds, List<int> SelectedMachineIds, List<int> SelectedTaskIds)
        {
            if (!ModelState.IsValid)
            {
                return View(field);
            }
            try
            {
                field.Workers = new List<Worker>();
                if (SelectedWorkerIds != null)
                {
                    foreach (var workerId in SelectedWorkerIds)
                    {
                        // Викликаємо одиничний метод для кожного ID
                        field.Workers.Add(_databaseService.GetWorkerById(workerId));
                    }
                }

                field.Machines = new List<Machine>();
                if (SelectedMachineIds != null)
                {
                    foreach (var machineId in SelectedMachineIds)
                    {
                        field.Machines.Add(_databaseService.GetMachineById(machineId));
                    }
                }

                field.Tasks = new List<WorkerTask>();
                if (SelectedTaskIds != null)
                {
                    foreach (var taskId in SelectedTaskIds)
                    {
                        field.Tasks.Add(_databaseService.GetWorkerTaskById(taskId));
                    }
                }
                _databaseService.AddField(field);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Помилка при додаванні: " + ex.Message);
                ViewBag.AvailableWorkers = _databaseService.GetAllWorkers();
                ViewBag.AvailableMachines = _databaseService.GetAllMachines();
                ViewBag.AvailableTasks = _databaseService.GetAllWorkerTasks();
                return View(field);
            }
        }
        public IActionResult Edit(int fieldId)
        {
            try
            {
                var field = _databaseService.GetFieldById(fieldId);

                // Отримання ВСІХ доступних даних для вибору у View
                ViewBag.AvailableWorkers = _databaseService.GetAllWorkers();
                ViewBag.AvailableMachines = _databaseService.GetAllMachines();
                ViewBag.AvailableTasks = _databaseService.GetAllWorkerTasks();
                return View(field);
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
        public IActionResult Edit(int id, Field field, List<int> SelectedWorkerIds, List<int> SelectedMachineIds, List<int> SelectedTaskIds)
        {
            if (id!=field.Id)
            {
                TempData["ErrorMessage"] = "Id не співпадають";
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                return View(field);
            }
            try
            {
                field.Workers = new List<Worker>();
                if (SelectedWorkerIds != null)
                {
                    foreach (var workerId in SelectedWorkerIds)
                    {
                        // Викликаємо одиничний метод для кожного ID
                        field.Workers.Add(_databaseService.GetWorkerById(workerId));
                    }
                }

                field.Machines = new List<Machine>();
                if (SelectedMachineIds != null)
                {
                    foreach (var machineId in SelectedMachineIds)
                    {
                        field.Machines.Add(_databaseService.GetMachineById(machineId));
                    }
                }

                field.Tasks = new List<WorkerTask>();
                if (SelectedTaskIds != null)
                {
                    foreach (var taskId in SelectedTaskIds)
                    {
                        field.Tasks.Add(_databaseService.GetWorkerTaskById(taskId));
                    }
                }
                _databaseService.UpdateField(field);
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
                ViewBag.AvailableWorkers = _databaseService.GetAllWorkers();
                ViewBag.AvailableMachines = _databaseService.GetAllMachines();
                ViewBag.AvailableTasks = _databaseService.GetAllWorkerTasks();
                return View(field);
            }

        }
        // GET: /Field/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var field = _databaseService.GetFieldById(id);
                return View(field);
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
                _databaseService.DeleteField(id);
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