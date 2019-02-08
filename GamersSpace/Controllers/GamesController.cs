using GamersSpace.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GamersSpace.Controllers {

    public class GamesController : Controller {

        // Create the list of games
        private static List<Game> games = new List<Game>() {
            new Game() { id = 1, name = "Super Meat Boy", genre = "Platform", publisher = "Team Meat", rating = 9.0, release = 2010 },
            new Game() { id = 2, name = "Gish", genre = "Side-scrolling, Platform", publisher = "Chronic Logic", rating = 7.2, release = 2004 },
            new Game() { id = 3, name = "Bastion", genre = "Action", publisher = "Supergiant Games", rating = 8.6, release = 2011 },
            new Game() { id = 4, name = "Hyper Light Drifter", genre = "Adventure", publisher = "Heart Machine", rating = 8.8, release = 2016 },
            new Game() { id = 5, name = "Gone Home", genre = "Adventure", publisher = "The Fullbright Company", rating = 7.7, release = 2013 }
        };

        // Return the main view 
        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        // Return the game information
        [HttpPost]
        public ActionResult GameInfo(string inputValue) {
            return MathcID(inputValue);
	    }

        // Return the view to the ModifyGame form
        [HttpPost]
        public ActionResult ModifyGame(string inputValue) {
            return MathcID(inputValue);
        }

        // Modify the game data
        [HttpPost]
        public ActionResult GameModification(int idValue, string nameValue, string publisherValue, double? rateValue, int? releaseValue, string genreValue) {
            if(GameDataChange(idValue, nameValue, publisherValue, rateValue, releaseValue, genreValue)) {
                TempData["state"] = "changeSucces";
            }
            else {
                TempData["state"] = "changeFailed";
            }
            return RedirectToAction("Index");
        }

        /**
         * @desc: Check if there is a game with the same id, and return the correct view.
         * @param: string id - The id to match.
         * @return: View - The view to show.
        **/
        private ActionResult MathcID(string id)
        {
            if (int.TryParse(id, out int number))
            {
                if (number <= games.Count)
                {
                    return View(games[number - 1]);
                }
                else
                {
                    TempData["state"] = "incorrectID";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["state"] = "noNumber";
                return RedirectToAction("Index");
            }
        }

        /**
         * @desc: Change the values of the given item in the  list.
         * @param: int id - The id of the item in the list.
         * @param: string name - Value of the new name.
         * @param: string publisher - Value of the new publisher.
         * @param: double rate - Value of the new rating.
         * @param: int release - Value of the new release.
         * @param: string genre - Value of the new genre.
         * @return: bool - True if no errors in the operations.
        **/
        private bool GameDataChange(int id, string name, string publisher, double? rate, int? release, string genre) {
            try {
                if(!String.IsNullOrEmpty(name)) {
                    games[id - 1].name = name;
                }
                if (!String.IsNullOrEmpty(publisher))
                {
                    games[id - 1].publisher = publisher;
                }
                if (!String.IsNullOrEmpty(rate.ToString()))
                {
                    games[id - 1].rating = (double)rate;
                }
                if (!String.IsNullOrEmpty(release.ToString()))
                {
                    games[id - 1].release = (int)release;
                }
                if (!String.IsNullOrEmpty(genre))
                {
                    games[id - 1].genre = genre;
                }
                return true;
            } catch {
                return false;
            }
        }

    }

}
