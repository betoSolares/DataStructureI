using System.Web.Mvc;

namespace Album.Controllers {

    public class AlbumController : Controller {

        // Return the view to load the files
        public ActionResult LoadFiles(){
            return View();
        }
        
    }

}
