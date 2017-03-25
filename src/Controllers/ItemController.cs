namespace todo.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using System.Net;
    using System.IO;
    using iot = todo.IOT_SERVICE;
    

    public class ItemController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            
            //var items = await DocumentDBRepository<Item>.GetItemsAsync(d=> (d.ProductID== "FG02425"));
            NetworkCredential nc = new NetworkCredential("sunil.yadav", "sunil_123", "TRIDENTDELHI");
            iot.IOTFunctions IOTF = new iot.IOTFunctions();
            string a;
            IOTF.Credentials = nc;
            bool flag = false;
            IEnumerable<Item> items = new List<todo.Models.Item>();
            //string fileLocCopy = @"d:\documentdb-dotnet-todo-app-master\src\Order.txt";
            //StreamWriter aw = new StreamWriter(fileLocCopy);
            //aw.Write("");
            //aw.Close();

            try
            {

                IOTF.GetOrderSummary();
                //IOTF.GetOrderInfo("RO3141001");
                //  a = "" + IOTF.SendPackedOrderInfo("RO3141001", "ARFD01MT", "FG03091", 20, 10, "ARFD0123", 1);
                WebClient request = new WebClient();
                string url = "ftp://192.168.10.24/Order.csv";
                request.Credentials = nc;
                byte[] newFileData = request.DownloadData(url);
                String[] xx = System.Text.Encoding.UTF8.GetString(newFileData).Split('\n');

                String[] xx1 = null;
                int count = 0;
                List<String> Destination = new List<string>();
                List<String> OrderId = new List<string>();

                for (int i = 0; i < xx.Length; i++)
                {
                    xx1 = xx[i].Split(',');
                    xx1[0] = xx1[0].Replace("\"", String.Empty);
                    xx1[1] = xx1[1].Replace("\"", String.Empty);
                    if (!Destination.Contains(xx1[0]))
                    { Destination.Add(xx1[0]); }
                    OrderId.Add(xx1[1]);
                }

                ViewBag.xxn = Destination;
                ViewBag.xxn1 = OrderId;
                

                for (int o = 0; o < OrderId.Count; o++)
                {

                    IOTF.GetOrderInfo(OrderId[o]);
                    System.Diagnostics.Debug.WriteLine("Order ID :" + OrderId[o]);
                    string url1 = "ftp://192.168.10.24/Product.csv";
                    request.Credentials = nc;
                    byte[] newFileData1 = request.DownloadData(url1);
                    String[] product = System.Text.Encoding.UTF8.GetString(newFileData1).Split('\n');
                    List<String> ProductList = new List<string>();
                    List<String> QuantityList = new List<string>();
                    String[] product1 = null;

                    for (int i = 0; i < product.Length; i++)
                    {
                        product1 = product[i].Split(',');
                        product1[0] = product1[0].Replace("\"", String.Empty);
                        product1[1] = product1[1].Replace("\"", String.Empty);
                        //if (!l1.Contains(xx1[0]))
                        {
                            ProductList.Add(product1[0]);
                        }
                        QuantityList.Add(product1[1]);
                    }
                    String[] ab = null;
                    for (var k = 0; k < ProductList.Count; k++)
                    {
                        System.Diagnostics.Debug.WriteLine("Product ID :" + ProductList[k]);
                        items = await DocumentDBRepository<Item>.GetItemsAsync(d => (d.ProductID == ProductList[k] && d.DeviceID == "RFID_READER_001"));
                        var total_w = 0;
                        foreach (var item in items)
                        {
                            Int32 i = 0;
                            Int32.TryParse(item.ProductWeight, out i);
                            total_w = total_w + i;
                        }
                        float weight = total_w / 1000;
                        System.Diagnostics.Debug.WriteLine("Total Inventory" + weight);
                        Int32 j = 0;
                        Int32.TryParse(QuantityList[0], out j);
                        System.Diagnostics.Debug.WriteLine("Order Inventory " +j);
                        if (weight > j)
                        {
                            System.Diagnostics.Debug.WriteLine("TRUE");
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                            break;
                        }


                    }

                    ViewBag.Product = ProductList;
                    ViewBag.quantity = QuantityList;

                    String order = OrderId[o];
                    //System.IO.FileStream fp;
                   // if (flag == true)
                   // {
                      //  if (System.IO.File.Exists(fileLocCopy))
                       // {
                           // using (StreamWriter sw = new StreamWriter(fileLocCopy,true))
                           // {
                                sw.WriteLine(order+","+DateTime.Now+" ");
                                
                           // }
                       // }
                   // }

                }
            }
            catch (Exception ex)
            {
                ViewBag.xxn = ex.GetBaseException().ToString();
                ViewBag.xxn1 = ex.GetBaseException().ToString();
                ViewBag.Product = ex.GetBaseException().ToString();
                ViewBag.quantity = ex.GetBaseException().ToString();
            }



            System.Diagnostics.Debug.WriteLine(items);
            return View(items);
        }

        

        [ActionName("edit")]
        public async Task<ActionResult> EditAsync()
        {
            var items = await DocumentDBRepository<Item>.GetItemsAsync(d => true);
            //System.Console.WriteLine(items);
            return View(items);
        }



#pragma warning disable 1998
        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {

            var items = await DocumentDBRepository<Item>.GetItemsAsync(d => true);
            //System.Console.WriteLine(items);
            return View(items);
        }
#pragma warning restore 1998

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Name,Description,Completed")] Item item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.CreateItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

       

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id")] string id)
        {
            await DocumentDBRepository<Item>.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            Item item = await DocumentDBRepository<Item>.GetItemAsync(id);
            return View(item);
        }

        

    }
}
