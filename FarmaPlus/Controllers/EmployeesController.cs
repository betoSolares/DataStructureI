using FarmaPlus.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FarmaPlus.Controllers {

    public class EmployeesController : Controller {

        // LinkedList of employees
        private static LinkedList<employee> employees = new LinkedList<employee>();

        // Return the form for insert a new employee
        [HttpGet]
        public ActionResult RegisterEmployee() {
            return View();
        }

        // Add a new employee
        [HttpPost]
        public ActionResult RegisterEmployee(string name, int id) {
            if(NewEmployee(name, id)) {
                TempData["state"] = "success";
            }
            else {
                TempData["state"] = "failed";
            }
            return RedirectToAction("RegisterEmployee");
        }

        // Return the view for the arrival control form
        [HttpGet]
        public ActionResult ArrivalControl() {
            return View();
        }

        /**
         * @desc: Create a new employee.
         * @param: string name - Name of the new employee.
         * @param: int id - ID of the new employee.
         * @return: bool - Success or failed.
        **/
        private bool NewEmployee(string name, int id) {
            if(employees.Any(x => x.id == id)) {
                return false;
            }
            else {
                employees.AddLast(new employee { name = name, id = id });
                return true;
            }
        }

    }

}