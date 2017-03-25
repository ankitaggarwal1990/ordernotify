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

namespace todo
{
    public class nav
    {
        NetworkCredential nc = new NetworkCredential("sunil.yadav", "sunil_123", "TRIDENTDELHI");
        iot.IOTFunctions IOTF = new iot.IOTFunctions();
        void add()
        {
            string a;
            IOTF.Credentials = nc;
            IOTF.GetOrderSummary();
            try
            {

                DateTime dt = new DateTime(2017, 01, 18);
                DateTime dt1 = new DateTime(2017, 01, 17);
                a = "" + IOTF.SendPackedOrderInfo("RO3141001", "ARFD01MT", "FG03091", 20, 10, "ARFD0123", 1);
                IOTF.GetOrderSummaryShipped("RO301RS");
                IOTF.GetOrderinfoShipped("RO3141001", "ARFD01MT");


                WebClient request = new WebClient();
                string url = "ftp://192.168.10.24/Order.csv";
                request.Credentials = nc;
                byte[] newFileData = request.DownloadData(url);
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                Console.WriteLine(fileString);
            }
            catch (Exception ex)
            {
                a = ex.GetBaseException().ToString();
            }
        }        
    }
}