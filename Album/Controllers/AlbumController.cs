using Album.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Album.Controllers {

    public class AlbumController : Controller {

        // Dictionaries for the album and the stickers
        private static Dictionary<string, List<Player>> album = new Dictionary<string, List<Player>>();
        private static Dictionary<Sticker, State> collection = new Dictionary<Sticker, State>();

        // Return the view to load the files
        [HttpGet]
        public ActionResult LoadFiles(){
            if(album.Count > 0 && collection.Count > 0) {
                ViewBag.Dictionaries = "notEmpty";
            } else {
                ViewBag.Dictionaries = "empty";
            }
            return View();
        }

        // Save the data in the dictionaries
        [HttpPost]
        public ActionResult LoadFiles(HttpPostedFileBase album, HttpPostedFileBase state, string formAction) {
            switch (formAction) {
                case "UploadFiles":
                    if(ValidateFiles(album, state)) {
                        ViewBag.Dictionaries = "notEmpty";
                        TempData["state"] = "success";
                    } else {
                        ViewBag.Dictionaries = "empty";
                        TempData["state"] = "failed";
                    }
                    break;
                case "DownloadFiles":
                    return DownloadFile("album_players");
            }
            return View();
        }

        // Return the view to search stickers
        [HttpGet]
        public ActionResult SearchSticker() {
            if(album.Count == 0 && collection.Count == 0) {
                ViewBag.Dictionaries = "empty";
            } else {
                ViewBag.Dictionaries = "notEmpty";
            }
            return View();
        }

        // Return the list of products
        [HttpPost]
        public ActionResult SearchSticker(string search, string type) {
            switch (type) {
                case "Team":
                    if(string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search)) {
                        ViewBag.Stickers = StickersList();
                    } else {
                        ViewBag.Stickers = StickersList(search);
                    }
                    break;
                case "Special":
                    ViewBag.Stickers = SpecialStickers();
                    break;
            }
            return View();
        }

        /**
         * @desc: Validate and save the data in each file in the directories.
         * @param: HttpPostedFileBase album - The file with the album data.
         * @param: HttpPostedFileBase collection - The file with the collection data.
         * @return: bool - Success or failed.
        **/
        private bool ValidateFiles(HttpPostedFileBase album, HttpPostedFileBase collection) {
            bool value = false;
            if(album != null && collection != null) {
                if(CheckExtension(album) && CheckExtension(collection)) {
                    string albumPath = StoreFile(album);
                    string collectionPath = StoreFile(collection);
                    try {
                        if(LoadData(albumPath, "album") && LoadData(collectionPath, "collection")) {
                            value = true;
                        }
                    } catch(Exception) {
                        return false;
                    }
                }
            }
            return value;
        }

        /**
         * @desc: Check the extension of the file.
         * @param: HttpPostedFileBase file - The file with to check the extension.
         * @return: bool - Success or failed.
        **/
        private bool CheckExtension(HttpPostedFileBase file) {
            if(".csv".Equals(Path.GetExtension(file.FileName), StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        /**
         * @desc: Store the files and save the data.
         * @param: HttpPostedFileBase album - The file with the album data.
         * @param: HttpPostedFileBase collection - The file with the collection data.
         * @return: bool - Success or failed.
        **/
        private string StoreFile(HttpPostedFileBase file) {
            string name = Path.GetFileName(file.FileName);
            string path = Path.Combine(Server.MapPath("~/App_Data/"), name);
            file.SaveAs(path);
            return path;
        }

        /**
         * @desc: Try to save the data in the album dictionary.
         * @para: string path - The path to the file.
         * @return: bool - Success or faided.
        **/
        private bool LoadData(string path, string type) {
            bool value = false;
            StreamReader reader = new StreamReader(path);
            reader.ReadLine();
            string line;
            while((line = reader.ReadLine()) != null) {
                string[] items = line.Split(',');
                if (type.Equals("album")) {
                    if (SaveAlbum(items)) {
                        value = true;
                    } else {
                        reader.Close();
                        return false;
                    }
                } else {
                    if (SaveCollection(items)) {
                        value = true;
                    } else {
                        reader.Close();
                        return false;
                    }
                }
            }
            reader.Close();
            System.IO.File.Delete(path);
            return value;
        }

        /**
         * @desc: Create and object and save to the album dictionary.
         * @param: string[] items - The collection of items.
        **/
        private bool SaveAlbum(string[] items) {
            try {
                Player player = new Player() {
                    sticker = new Sticker() { id = int.Parse(items[0]), club = items[5], clubLogo = items[6] },
                    name = items[1],
                    age = int.Parse(items[2]),
                    photo = items[3],
                    nationality = items[4],
                    position = items[7],
                    number = int.Parse(items[8]),
                    height = items[9],
                    weight = items[10]
                };
                if (album.ContainsKey(player.sticker.club)) {
                    List<Player> players = album[player.sticker.club];
                    players.Add(player);
                    album[player.sticker.club] = players;
                } else {
                    album.Add(player.sticker.club, new List<Player>() { player });
                }
                return true;
            } catch (Exception) {
                album.Clear();
                collection.Clear();
                return false;
            }
        }

        /**
         * @desc: Create and object and save to the collection dictionary.
         * @param: string[] items - The collection of items.
        **/
        private bool SaveCollection(string[] items) {
            try {
                Sticker sticker = new Sticker() { id = int.Parse(items[0]), club = items[1], clubLogo = items[2] };
                State state = new State() { collected = items[3].Equals("TRUE"), quantity = int.Parse(items[4]) };
                collection.Add(sticker, state);
                return true;
            } catch (Exception) {
                album.Clear();
                collection.Clear();
                return false;
            }
        }

        /**
         * @desc: Create the file and download them.
         * @param: string name - The name of the file-
         * @return: File - The file to download.
        **/
        private ActionResult DownloadFile(string name) {
            string fileName = name + ".json";
            string path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
            if (!name.Equals("Player")) {
                string content = "Album\n" + JsonConvert.SerializeObject(album, Formatting.Indented) + "\nPlayers\n" + JsonConvert.SerializeObject(collection, Formatting.Indented);
                content = content.Replace("\n", Environment.NewLine);
                System.IO.File.WriteAllText(path, content);
            }
            FileContentResult file = File(System.IO.File.ReadAllBytes(path), "application/octet-stream", fileName);
            System.IO.File.Delete(path);
            return file;
        }

        /**
         * @desc: Return a list with all the elements that contains the name.
         * @return: List<Player> - The stickers found.
        **/
        private List<Player> StickersList() {
            List<Player> stickers = new List<Player>();
            foreach(KeyValuePair<string, List<Player>> club in album) {
                if(club.Key != "Special") {
                    foreach(Player player in club.Value) {
                        if (!IsCollected(player.sticker.id)) {
                            stickers.Add(player);
                        }
                    }
                }
            }
            return stickers;
        }

        /**
         * @desc: Return a list with all the elements that contains the name.
         * @param: string search - The name of the club to search.
         * @return: List<Player> - The stickers found.
        **/
        private List<Player> StickersList(string search) {
            List<Player> stickers = new List<Player>();
            foreach(KeyValuePair<string, List<Player>> club in album) {
                if(club.Key.ToLower().Contains(search.ToLower()) && club.Key != "Special") {
                    foreach(Player player in club.Value) {
                        if (!IsCollected(player.sticker.id)) {
                            stickers.Add(player);
                        }
                    }
                }
            } 
            return stickers;
        }

        /**
         * @desc: Return a list with all the special stickers that dont are collected.
         * @return: List<Player> - The stickers found.
        **/
        private List<Player> SpecialStickers() {
            List<Player> stickers = new List<Player>();
            foreach (KeyValuePair<string, List<Player>> club in album) {
                if (club.Key.Equals("Special")) {
                    foreach(Player player in club.Value) {
                        if (!IsCollected(player.sticker.id)) {
                            stickers.Add(player);
                        }
                    }
                }
            }
            return stickers;
        }

        /**
         * @desc: Check if the elementwith the id is collected.
         * @param: int id - The id to check.
         * @return: bool - True or false.
        **/
        private bool IsCollected(int id) {
            foreach(KeyValuePair<Sticker, State> sticker in collection) {
                if(sticker.Key.id == id) {
                    return sticker.Value.collected;
                }
            }
            return false;
        }

    }

}
