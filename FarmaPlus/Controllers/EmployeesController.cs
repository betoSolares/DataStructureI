using System.Web.Mvc;

namespace FarmaPlus.Controllers {

    public class EmployeesController : Controller {

        // Return the form for insert a new employee
        [HttpGet]
        public ActionResult RegisterEmployee() {
            return View();
        }

    }

}