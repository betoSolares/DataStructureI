using Album.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Album.Controllers {

    public class AlbumController : Controller {

        // Dictionaries for the album and the stickers
        private static Dictionary<string, Player> album = new Dictionary<string, Player>();
        private static Dictionary<Sticker, State> collection = new Dictionary<Sticker, State>();

        // Return the view to load the files
        public ActionResult LoadFiles(){
            if(album.Count > 0 && collection.Count > 0) {
                ViewBag.Dictionaries = "notEmpty";
            } else {
                ViewBag.Dictionaries = "empty";
            }
            return View();
        }
        
    }

}
