using GamersSpace.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GamersSpace.Controllers {

    public class GamesController : Controller {

        // Create the list of games
        private List<Game> GameList() {
            /**Game[] gameList = new Game[5];
            gameList[0] = new Game() { id = 1, name = "Super Meat Boy", genre = "Platform", publisher = "Team Meat", rating = 9.0, release = 2010 };
            gameList[1] = new Game() { id = 2, name = "Gish", genre = "Side-scrolling, Platform", publisher = "Chronic Logic", rating = 7.2, release = 2004 };
            gameList[2] = new Game() { id = 3, name = "Bastion", genre = "Action", publisher = "Supergiant Games", rating = 8.6, release = 2011 };
            gameList[3] = new Game() { id = 4, name = "Hyper Light Drifter", genre = "Adventure", publisher = "Heart Machine", rating = 8.8, release = 2016 };
            gameList[4] = new Game() { id = 5, name = "Gone Home", genre = "Adventure", publisher = "The Fullbright Company", rating = 7.7, release = 2013 };
    **/
            List<Game> games = new List<Game>() {
                new Game() { id = 1, name = "Super Meat Boy", genre = "Platform", publisher = "Team Meat", rating = 9.0, release = 2010 },
                new Game() { id = 2, name = "Gish", genre = "Side-scrolling, Platform", publisher = "Chronic Logic", rating = 7.2, release = 2004 },
                new Game() { id = 3, name = "Bastion", genre = "Action", publisher = "Supergiant Games", rating = 8.6, release = 2011 },
                new Game() { id = 4, name = "Hyper Light Drifter", genre = "Adventure", publisher = "Heart Machine", rating = 8.8, release = 2016 },
                new Game() { id = 5, name = "Gone Home", genre = "Adventure", publisher = "The Fullbright Company", rating = 7.7, release = 2013 }
            };
            return games;
        }

        // Return the view 
        public ActionResult Index() {
            ViewBag.Games = GameList();
            return View();
        }

    }

}