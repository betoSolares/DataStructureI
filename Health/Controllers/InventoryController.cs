﻿using DataStructures;
using Health.Models;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Health.Controllers {

    public class InventoryController : Controller {

        // Tree for the meds
        private static BST<Meds, string> tree = new BST<Meds, string>();

        // Return the main view
        [HttpGet]
        public ActionResult InventoryLoad() {
            if (tree.IsEmpty())
                ViewBag.Empty = "empty";
            else
                ViewBag.Empty = "noEmpty";
            return View();
        }

        // Load or download the file
        [HttpPost]
        public ActionResult InventoryLoad(HttpPostedFileBase PathFile, string formAction) {
            switch (formAction) {
                case "Cargar Inventario":
                    if (LoadFile(PathFile))
                        TempData["state"] = "success";
                    else
                        TempData["state"] = "failed";
                    if (tree.IsEmpty())
                        ViewBag.Empty = "empty";
                    else
                        ViewBag.Empty = "noEmpty";
                    break;
                case "Descargar Inventario":
                    break;
            }
            return View();
        }


        /**
         * @desc: Verify if there is a file and load to the tree the elements.
         * @param: HttpPostedFileBase fileUpload - the file to upload.
         * @return: bool - True if there is a file, else false.
        **/
        private bool LoadFile(HttpPostedFileBase fileUpload) {
            string path = string.Empty;
            // Check if the fileUpload is not empty
            if(fileUpload != null) {
                // Store the file
                string fileName = Path.GetFileName(fileUpload.FileName);
                path = Path.Combine(Server.MapPath("~/App_Data/Files"), fileName);
                fileUpload.SaveAs(path);
                // Read the file and split each element
                string file = System.IO.File.ReadAllText(path);
                foreach(string line in file.Split('\n')) {
                    if (!string.IsNullOrEmpty(line)) {
                        string[] items = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        // Create a new object and insert it into the tree
                        Meds newMed = new Meds() {
                            id = int.Parse(items[0]),
                            name = items[1],
                            description = items[2],
                            production = items[3],
                            price = float.Parse(items[4].Trim('$')),
                            stock = int.Parse(items[5])
                        };
                        tree.Insert(newMed.name, newMed);
                    }
                }
                return true;
            }
            return false;
        }

    }

}
