using System;
using System.Linq;
using System.Web.Mvc;
using EmployeeManagement.DataAccess_layer;
using EmployeeManagement.Models;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDataAccess empdal = new EmployeeDataAccess();

        // GET: Employee
        public ActionResult Index()
        {
            var empList = empdal.GetAllEmployeeList();
            if (empList.Count == 0)
            {
                TempData["InfoMessage"] = "No Employee Present!";
            }
            return View(empList);
        }

        // GET: Employee/Create or Employee/Edit/5
        public ActionResult Create(int? id)
        {
            Employee employee = null;
            if (id.HasValue)
            {
                employee = empdal.GetEmployeeById(id.Value).FirstOrDefault();
                if (employee == null)
                {
                    TempData["InfoMessage"] = "Employee not found with ID: " + id.ToString();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                employee = new Employee();
            }
            return PartialView("Create", employee);
        }

        // POST: Employee/Create or Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if employee with provided ID exists
                    var existingEmployee = empdal.GetEmployeeById(employee.Id).FirstOrDefault();
                    if (existingEmployee == null)
                    {
                        // If not, insert
                        bool isInserted = empdal.InsertNewEmployee(employee);
                        TempData["SuccessMessage"] = isInserted ? "Employee details saved successfully." : "Failed to save employee details.";
                    }
                    else
                    {
                        // If exists, update
                        bool isUpdated = empdal.UpdateEmployee(employee);
                        TempData["SuccessMessage"] = isUpdated ? "Employee details updated successfully." : "Failed to update employee details.";
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var emp = empdal.GetEmployeeById(id).FirstOrDefault();
                if (emp == null)
                {
                    TempData["InfoMessage"] = "Employee ! available with this ID" + id.ToString();
                    return RedirectToAction("Index");
                }
                return View(emp);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessgae"] = ex.Message;
                return PartialView();
            }

        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult DeleteEmployee(int id)
        {
            try
            {
                bool isDeleted = empdal.DeleteEmployee(id);
                if (isDeleted)
                {
                    TempData["SuccessMessage"] = "Employee details deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while deleting the employee.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}