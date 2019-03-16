using DataStructures;
using Health.Models;
using System.Web.Mvc;

namespace Health.Controllers {

    public class InventoryController : Controller {

        // Tree for the meds
        private static BST<Meds, string> tree = new BST<Meds, string>();

        // Return the main view
        [HttpGet]
        public ActionResult InventoryLoad() {
            if (tree.IsEmpty()) {
                ViewBag.Empty = "empty";
            } else {
                ViewBag.Empty = "noEmpty";
            }
            return View();
        }

    }

}
