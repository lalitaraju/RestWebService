using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using CatalogService;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Xml;
using System.Runtime.Serialization;


namespace SelfHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            bool succeed = false;
            string baseUri = "http://localhost:8081/NBTY";
            WebServiceHost sh = new WebServiceHost(typeof(CatalogServiceType),
                              new Uri(baseUri));
            try
            {
              
                DataContractSerializer ser = new DataContractSerializer(typeof(CatalogServiceType));                
                sh.Open();
                succeed = true;
                Console.WriteLine("Service is Running");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ServiceHost failed to open {0}", ex.ToString());

            }
            finally 
            {
                //call Abort since the object will be in the Faulted state
                if (!succeed)
                    sh.Abort();

            }


        }
    }

     
}
