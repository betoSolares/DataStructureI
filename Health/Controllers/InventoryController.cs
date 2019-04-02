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

        // Shop cart list
        private static List<Meds> shopCart = new List<Meds>();

        // Removed products list
        private static List<Meds> removedProducts = new List<Meds>();

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
                    return DownloadFile("tree");
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
        
        // Return JSON format data with the information of the product
        [HttpGet]
        public JsonResult ProductInfo(string name) {
            try {
                Meds product = tree.Find(name);
                return Json(new { name = product.name, description = product.description, production = product.production, price = product.price, stock = product.stock }, JsonRequestBehavior.AllowGet);
            } catch (Exception) {
                return Json(new { name = "null"}, JsonRequestBehavior.AllowGet);
            }
        }
        
        // Serialize the product to a JSON and download it.
        [HttpGet]
        public ActionResult DownloadProduct(string file) {
            return DownloadFile(file);
        }

        // Add the product to the shop cart
        [HttpPost]
        public JsonResult AddProductCart(string name, int quantity) {
            int maxProducts = AddCart(name, quantity);
            return Json(new { max = maxProducts }, JsonRequestBehavior.AllowGet);
        }

        // Return the view for make the order.
        [HttpGet]
        public ActionResult MakeOrder() {
            ViewBag.products = null;
            if (tree.IsEmpty()) {
                TempData["state"] = "empty";
            } else if(shopCart.Count == 0){
                TempData["state"] = "noCart";
            } else {
                TempData["state"] = "makeOrder";
                ViewBag.products = shopCart;
            }
            return View();
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
        
        // Method that downloads the json files
        private ActionResult DownloadFile(string name) {
            string fileName = name + ".json";
            string path = Path.Combine(Server.MapPath("~/App_Data/Files/") + fileName);
            if (name.Equals("tree"))
                System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(tree.ToList(), Formatting.Indented));
            else
                System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(tree.Find(name), Formatting.Indented));
            return File(System.IO.File.ReadAllBytes(path), "application/octet-stream", fileName);
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

        /**
         * @desc: Add a new product to the shop cart and return the items remanings.
         * @param: string name - The name of the product.
         * @param: int quantity - The number of elements to insert in the shop cart.
         * @return: int - The items remanings in the tree.
        **/
        private int AddCart(string name, int quantity) {
            Meds product = tree.Find(name);
            int index = shopCart.FindIndex(x => x.name == name);
            float total = product.price * quantity;
            if(index >= 0) {
                shopCart[index].stock += quantity;
                shopCart[index].price += total;
            } else {
                shopCart.Add(new Meds { name = name, stock = quantity, price = total });
            }
            product.stock = product.stock - quantity;
            ProductUpdate(product);
            return product.stock;
        }

        /**
         * @desc: Verify if the product has to be deleted of the tree.
         * @param: Meds product - The product to verify.
        **/
        private void ProductUpdate(Meds product) {
            if (product.stock == 0) {
                removedProducts.Add(product);
                tree.Remove(product.name);
            } else {
                tree.Update(product.name, product);
            }
        }

    }

}
