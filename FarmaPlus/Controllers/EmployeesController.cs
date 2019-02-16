using FarmaPlus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FarmaPlus.Controllers {

    public class EmployeesController : Controller {

        // Data structures of the project
        private static LinkedList<employee> employees = new LinkedList<employee>();
        private static Stack<arrival> arrivals = new Stack<arrival>();

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
            return View();
        }

        // Return the view for the arrival control form
        [HttpGet]
        public ActionResult ArrivalControl() {
            return View();
        }

        // Add a new employee in the arrival control stack
        [HttpPost]
        public ActionResult ArrivalControl(string name, int id, TimeSpan? time) {
            if(NewArrival(id, time)) {
                TempData["state"] = "successArrive";
            }
            else {
                TempData["state"] = "failedArrive";
            }
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

        /**
         * @desc: Add a new arrival to the stack
         * @param: int id - The id of the employee.
         * @param: TimeSpane? - The time of the arrive, can be null.
         * @return: bool - Succes of failed.
        **/
        private bool NewArrival(int id, TimeSpan? time) {
            if (!employees.Any(x => x.id == id) || arrivals.Any(x => x.employee.id == id)) {
                return false;
            }
            else {
                employee employee = employees.Where(x => x.id == id).FirstOrDefault();
                employees.Find(employee).Value.inOffice = true;
                if(time != null) {
                    arrivals.Push(new arrival { employee = employee, entryTime = (TimeSpan)time, appointments = new Random().Next(1, 5) });
                }
                else {
                    arrivals.Push(new arrival { employee = employee, entryTime = DateTime.Now.TimeOfDay, appointments = new Random().Next(1, 5) });
                }
                return true;
            }
        }

    }

}