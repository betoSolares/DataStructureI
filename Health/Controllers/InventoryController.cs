using DataStructure;
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
        private static BTree<Meds, string> tree;

        // Shop cart list
        private static List<Meds> shopCart = new List<Meds>();

        // Removed products list
        private static List<Meds> removedProducts = new List<Meds>();

        // Tree for the orders
        private static BTree<Orders, Guid> orders;

        // Return the main view
        [HttpGet]
        public ActionResult InventoryLoad() {
            try {
                if (tree.IsEmpty())
                    ViewBag.Empty = "empty";
                else
                    ViewBag.Empty = "noEmpty";
            } catch (Exception) {
                ViewBag.Empty = "empty";
            }
            return View();
        }

        // Load or download the file.
        [HttpPost]
        public ActionResult InventoryLoad(HttpPostedFileBase PathFile, string formAction, int? order) {
            switch (formAction) {
                case "Cargar Inventario":
                    tree = new BTree<Meds, string>((int)order);
                    orders = new BTree<Orders, Guid>((int)order);
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

        // Remove a certain amount of product from the shop cart
        [HttpPost]
        public JsonResult PartialRemove(string name, int quantity) {
            return Json(new { data = DeleteCart("partial", name, quantity) }, JsonRequestBehavior.AllowGet);
        }

        // Remove a product from the shop cart
        [HttpPost]
        public JsonResult CompleteRemove(string name) {
            return Json(new { data = DeleteCart("complete", name, null) }, JsonRequestBehavior.AllowGet);
        }

        // Get the information for the order
        [HttpPost]
        public ActionResult MakeOrder(string name, string address, string nit) {
            CreateOrder(name, address, nit);
            TempData["state"] = "noCart";
            return View();
        }

        // Return the view for the reload tree functionality.
        [HttpGet]
        public ActionResult ReloadTree() {
            ViewBag.NewProducts = null;
            return View();
        }

        // Return the view with the products reload
        [HttpPost]
        public ActionResult ReloadProducts() {
            if(removedProducts.Count > 0) {
                ViewBag.NewProducts = NewProductsList();
            } else {
                ViewBag.NewProducts = null;
            }
            return View("ReloadTree");
        }

        // Return the view to find orders
        [HttpGet]
        public ActionResult SearchOrder() {
            if (orders.IsEmpty()) {
                TempData["state"] = "empty";
            } else {
                TempData["state"] = "noSearched";
            }
            return View();
        }

        // Return a list with the orders
        [HttpPost]
        public ActionResult SearchOrder(string search) {
            ViewBag.Orders = OrdersList(search);
            TempData["state"] = "searched";
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

        /**
         * @desc: Remove a product from the shop cart.
         * @param: string type - Type of the remove (Partial/Total).
         * @param: string name - The name of the product.
         * @param: int? quantity - The amount of products to remove(can be null).
         * @return: bool - Succes or Failed.
        **/
        private bool DeleteCart(string type, string name, int? quantity) {
            bool value = false;
            if (type.Equals("partial")) {
                value = PartialDelete(name, (int)quantity);
            } else {
                value = CompleteDelete(name);
            }
            return value;
        }

        /**
         * @desc: Remove the amount of product from the shop cart
         * @param: string name - Name of the product.
         * @param: int quantity - The amount of products.
         * @return: bool value - Success or failed.
        **/
        private bool PartialDelete(string name, int quantity) {
            bool value = false;
            Meds product = shopCart.Find(x => x.name == name);
            try {
                if (product.stock == quantity) {
                    tree.Find(name).stock += product.stock;
                    shopCart.Remove(product);
                } else {
                    tree.Find(name).stock += quantity;
                    shopCart.Find(x => x.name == product.name).stock -= quantity;
                    float newPrice = tree.Find(name).price * quantity;
                    shopCart.Find(x => x.name == product.name).price -= newPrice;
                }
                value = true;
            } catch (Exception) {
                Meds newProduct = removedProducts.Find(x => x.name == name);
                tree.Insert(newProduct.name, newProduct);
                if (product.stock == quantity) {
                    tree.Find(product.name).stock += product.stock;
                    shopCart.Remove(product);
                } else {
                    tree.Find(product.name).stock += quantity;
                    shopCart.Find(x => x.name == name).stock -= quantity;
                    float newPrice = tree.Find(name).price * quantity;
                    shopCart.Find(x => x.name == product.name).price -= newPrice;
                }
                removedProducts.Remove(newProduct);
                value = true;
            }
            return value;
        }

        /**
         * @desc: Remove the product from the shop cart
         * @param: string name - Name of the product.
         * @return: bool value - Success or failed.
        **/
        private bool CompleteDelete(string name) {
            bool value = false;
            Meds product = shopCart.Find(x => x.name == name);
            try {
                tree.Find(product.name).stock += product.stock;
                shopCart.Remove(product);
                value = true;
            } catch (Exception) {
                Meds newProduct = removedProducts.Find(x => x.name == name);
                tree.Insert(newProduct.name, newProduct);
                tree.Find(product.name).stock += product.stock;
                shopCart.Remove(product);
                removedProducts.Remove(newProduct);
                value = true;
            }
            return value;
        }

        // Create a list with the new products
        private List<Meds> NewProductsList() {
            List<Meds> newProducts = new List<Meds>();
            foreach(var item in removedProducts) {
                item.stock = new Random().Next(1, 15);
                newProducts.Add(item);
                tree.Insert(item.name, item);
            }
            removedProducts.Clear();
            return newProducts;
        }

        // Create a new order and insert in the tree
        private void CreateOrder(string name, string address, string nit) {
            Guid guid = new Guid();
            Orders order = new Orders { name = name, address = address, nit = nit, products = shopCart, guid = guid };
            orders.Insert(guid, order);
            shopCart.Clear();
        }

        /**
         * @desc: Verify if there is an element in the tree with the same guid.
         * @param: string name - The name to search.
         * @return: List<Orders> - The list with the elements.
        **/
        private List<Orders> OrdersList(string guid) {
            List<Orders> _orders = new List<Orders>();
            foreach (var element in orders.ToList()) {
                if (element.guid.ToString().Contains(guid)) {
                    _orders.Add(element);
                }
            }
            return _orders;
        }

    }

}
