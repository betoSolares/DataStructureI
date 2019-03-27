using DataStructures;
using Health.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        // Load or download the file.
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
                    return DownloadTree();
            }
            return View();
        }

        // Get the search view
        [HttpGet]
        public ActionResult SearchProduct() {
            if (tree.IsEmpty()) {
                TempData["state"] = "empty";
            } else {
                TempData["state"] = "noSearched";
            }
            return View();
        }

        // Return a list with the elements
        [HttpPost]
        public ActionResult SearchProduct(string search) {
            ViewBag.Products = ProductsList(search);
            TempData["state"] = "searched";
            return View();
        }
        
        // Return JSON file with the information of the product
        public JsonResult ProductInfo(string name) {
            Meds product = tree.Find(name);
            return Json(new { name = product.name, description = product.description, production = product.production, price = product.price, stock = product.stock }, JsonRequestBehavior.AllowGet);
        }

        /**
         * @desc: Verify if there is a file and load to the tree the elements.
         * @param: HttpPostedFileBase fileUpload - the file to upload.
         * @return: bool - True if there is a file, else false.
        **/
        private bool LoadFile(HttpPostedFileBase fileUpload) {
            bool valid = false;
            string path = string.Empty;
            // Check if the fileUpload is not empty
            if (fileUpload != null) {
                // Check if the file is a csv
                if (".csv".Equals(Path.GetExtension(fileUpload.FileName), StringComparison.OrdinalIgnoreCase)) {
                    // Store the file
                    string fileName = Path.GetFileName(fileUpload.FileName);
                    path = Path.Combine(Server.MapPath("~/App_Data/Files"), fileName);
                    fileUpload.SaveAs(path);
                    // Read the file and split each element
                    string file = System.IO.File.ReadAllText(path);
                    foreach (string line in file.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] items = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                            // Create a new object and insert it into the tree
                            try {
                                Meds newMed = new Meds()
                                {
                                    id = int.Parse(items[0]),
                                    name = items[1],
                                    description = items[2],
                                    production = items[3],
                                    price = float.Parse(items[4].Trim('$')),
                                    stock = int.Parse(items[5])
                                };
                                tree.Insert(newMed.name, newMed);
                                valid = true;
                            } catch(Exception) {
                                tree.Clear();
                                return false;
                            }
                        }
                    }
                }
            }
            return valid;
        }

        // Serialize the tree to a JSON and download it.
        private ActionResult DownloadTree() {
            string path = Path.Combine(Server.MapPath("~/App_Data/Files/") + "tree.json");
            System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(tree.ToList(), Formatting.Indented));
            return File(System.IO.File.ReadAllBytes(path), "application/octet-stream", "tree.json");
        }

        /**
         * @desc: Verify if there is an element in the tree with the same name.
         * @param: string name - The name to search.
         * @return: List<Meds> - The list with the elements.
        **/
        private List<Meds> ProductsList(string name) {
            List<Meds> meds = new List<Meds>();
            foreach(var element in tree.ToList()) {
                if (element.name.ToLower().Contains(name.ToLower())) {
                    meds.Add(element);
                }
            }
            return meds;
        }

    }

}
