using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml;
using DirectResponse.Catalog.BusinessObjects;
using DirectResponse.Catalog.BusinessServices;
using System.Collections;
using System.Xml.Schema;
using System.Xml.Serialization;



namespace  CatalogService
{
  
     public class CatalogServiceType : ICatalogService
    {

        public Message GetRoot()
        
        {  
               // Company p = new Company(11);
              //  Message realret = Message.CreateMessage(MessageVersion.None, "*", p.GetDataContractXml());
               // return realret;

            
             DomainList ret = new DomainList();
             string[] domains = new string[] { "Puritan", "VitaminWorld" };
             foreach (string domain in domains)
             {  
               
                ret.Add(new Domain { Name = domain, Uri =domain,Hyperlink="http://localhost:1770/nbty/"+domain});
               
             }
            Message realret = Message.CreateMessage(MessageVersion.None, "*", ret);
            return realret;               

           
        }

        
        public Message GetCompany(string domain)
        {
            DivisionList ret = new DivisionList();
            string[] divisions = new string[] { "com", "jp", "ch", "at" };
          
             foreach( string division in divisions)       
            {
                ret.Add(new DivisionData { CompanyName = domain, Division = division, Hyperlink = "http://localhost:1770/nbty/" +domain+"/"+division});
             
            }

            Message realret = Message.CreateMessage(MessageVersion.None, "*",ret);
            
            
            return realret;

        }
   
        public Message GetDivision(string domain, string Division)
        {
            CatalogInfo ret = new CatalogInfo();
            string[] subdivisions = new string[] { "CatalogCategory", "CatalogCategoryProducts", "CatalogProduct", "CatalogBrand" };
            foreach (string subdivision in subdivisions)
            {
                ret.Add(new CatalogData { Company = domain, Division = Division, Catalog = subdivision });
            }
            Message realret = Message.CreateMessage(MessageVersion.None, "*", ret);
            return realret;

        }
        
        static string _query = null;
       
  
      
        public Message GetCategories(string domain, string Division, string Catalog, string sort, string query)
        {                        

                 CategoriesInfo ret = new CategoriesInfo();
            
                 List<CatalogCategory> list = DirectResponse.Catalog.BusinessServices.CatalogService.GetCatalog(1000).Categories.ToList();

                 XmlDocument xmldoc = new XmlDocument();                 
                 
                 DirectResponse.Catalog.BusinessServices.CatalogService.GetCatalog(1000);

                 xmldoc.LoadXml(DirectResponse.Catalog.BusinessServices.CatalogService.GetCatalog(1000).CreateCatalogFrag.ToString());
                 XmlElement root = xmldoc.DocumentElement;

                 
                //Get subcategories of categorylist

                 List<CatalogCategory> list1 =new List<CatalogCategory>();

                 for (int i = 0; i < list.Count(); i++)
                 {
                   List<CatalogCategory> list2 = DirectResponse.Catalog.BusinessServices.CatalogService.GetCatalog(1000).Categories[i].SubCategories.ToList();
                    if(list2.Count ()!=0)
                    {
                     list1.Add(list2.ElementAt(i-i));
                    }
                 } 
                    
                  
                 Message realret = Message.CreateMessage(OperationContext.Current.IncomingMessageVersion, "*",GetItems(list,list1,xmldoc)); ;
                 return realret;
            }

      
         static int _pagesize =0;
        
         static int _page     = 1;
        
         static bool _inclusive = false;
       


        public Message GetProducts(string domain, string Division, string Catalog,
                string sort, int pagesize, int page, bool inclusive, string query)
        
        {
                _inclusive = inclusive;

                 ProductsInfo ret = new ProductsInfo();                
                 Catalog p = new Catalog(Int32.Parse(Catalog));

                 CatalogProductFactory cf    = new CatalogProductFactory();
                 List<CatalogProduct>  list  = cf.GetItems(page,Catalog.ToInt(),0);                         
                 List<CatalogCategory> list1 = p.Categories.ToList();
               
                
                // List<CatalogCategory> list4 = p.Categories[0].SubCategories[0].SubCategories.ToList();

                 List<CatalogCategory > categories= new List<CatalogCategory>();
                 List<CatalogProduct> products= new  List<CatalogProduct>();

                 //add all the categories to categories list
                 categories= list1.ToList();


                 if (inclusive == true)
                 {
                     
                     //add subcategories to the categories list

                     for (int i = 0; i < list1.Count(); i++)
                     {
                         List<CatalogCategory> list2 = p.Categories[i].SubCategories.ToList();
                         if (list2.Count() != 0)
                         {
                             categories.Add(list2.ElementAt(i - i));
                         }
                     }

                 }

                 //add catalogproducts to products
                 products=list.ToList();

                 for (int i = 0; i < list.Count(); i++)
                 {
                    List<CatalogProduct>  list3 = p.Categories[i].Products.ToList();

                    if(list3.Count ()!=0)
                    {
                    products.Add(list3.ElementAt(i-i));
                    }
                 } 
                   
                 XmlDocument xmldoc = new XmlDocument();         
                            

                 xmldoc.LoadXml(DirectResponse.Catalog.BusinessServices.CatalogService.GetCatalog(1000).CreateCatalogFrag.ToString());
                 XmlElement root = xmldoc.DocumentElement;
 
                 
                _inclusive = inclusive;
                _page = page;
                _pagesize = list.Count();
                _query = query;

                p.PageNo = page;
             
                Message realret = Message.CreateMessage(OperationContext.Current.IncomingMessageVersion, "*",GetItems(products,categories,xmldoc));

                return realret;
            
           
         
        }

 
        public Message GetCategories(string domain, string Division, string Catalog, string Category_0,
            string sort, string query)
        {
           
             Catalog c = new Catalog(Int32.Parse(Catalog));
             Category cat=new Category(Int32 .Parse(Category_0));

             CatalogCategory cc= new CatalogCategory(Int32.Parse(Category_0));
             List<CatalogCategory> list = cc.SubCategories.ToList();
             List<CatalogCategory> list1 = cc.SubCategories[0].SubCategories.ToList();

            //Gets catalog description
             XmlDocument xmldoc = new XmlDocument();
             xmldoc.LoadXml(c.CreateCatalogFrag.ToString());

             //add xmlfragment to the main Document and add category description
             XmlDocumentFragment docfrag1 = xmldoc.CreateDocumentFragment();
             docfrag1.InnerXml = cat.CreateCategoryFrag.ToString();
             xmldoc.DocumentElement.AppendChild(docfrag1);

             XmlElement root = xmldoc.DocumentElement;
       
             Message realret = Message.CreateMessage(OperationContext.Current.IncomingMessageVersion, "*", GetItems(list,xmldoc) );
             return realret;
                   

           
        }
        public Message GetProducts(string domain, string Division, string Catalog, string Category_0,
               string sort, int pagesize, int page, bool inclusive, string query)
        
        {
           
            //return list of all products in catalog category
            CatalogCategory c1 = new CatalogCategory(Int32.Parse(Category_0));
            Category c = new Category(Category_0.ToInt());
            CatalogProductFactory cf= new CatalogProductFactory();
                       
            
                List<CatalogProduct> list = cf.GetItems(page, Catalog.ToInt(), Category_0.ToInt());

                List<CatalogCategory> list1 = c1.SubCategories.ToList();

                List<CatalogProduct> list2 = c1.SubCategories[0].Products.ToList();

                List<CatalogCategory> list3 = c1.SubCategories[0].SubCategories.ToList();

                List<CatalogProduct> list4 = c1.SubCategories[0].SubCategories[0].Products.ToList();

                List<CatalogCategory> list5 = c1.SubCategories[0].SubCategories[0].SubCategories.ToList();
          
             _inclusive = inclusive;
             _page = page;
             _pagesize = pagesize;
             _query = query;
           
            
              
                
            DataContractSerializer sel;

            MemoryStream memoryStream = new MemoryStream();

            try
            {
                if (inclusive == true)
                {
                    DataContractSerializer sel6 = new DataContractSerializer(c.GetType());
                    sel6.WriteObject(memoryStream, c);  

                    sel = new DataContractSerializer(list.GetType());
                    sel.WriteObject(memoryStream, list);     
                  
                    DataContractSerializer sel1 = new DataContractSerializer(list1.GetType());
                    sel1.WriteObject(memoryStream, list1);
                   
                    DataContractSerializer sel2 = new DataContractSerializer(list2.GetType());
                    sel2.WriteObject(memoryStream, list2);
                    
                    DataContractSerializer sel3 = new DataContractSerializer(list3.GetType());
                    sel3.WriteObject(memoryStream, list3);
                    
                    DataContractSerializer sel4= new DataContractSerializer(list4.GetType());
                    sel4.WriteObject(memoryStream, list4);
                    
                    DataContractSerializer sel5 = new DataContractSerializer(list5.GetType());
                    sel3.WriteObject(memoryStream, list5);
                   
                }
                else
                {
                    sel = new DataContractSerializer(list.GetType());

                    sel.WriteObject(memoryStream ,list);
                }
            }
            catch (Exception e)
            {   
                Message r=Message.CreateMessage(MessageVersion.None, "*", e.ToString());
                return r;
            }

            memoryStream.Position = 0;

            StreamReader stream = new StreamReader(memoryStream);

            string xmlstring = stream.ReadToEnd();
           
          
            Message realret = Message.CreateMessage(MessageVersion.None , "*",GetStream(xmlstring,memoryStream));
            
            return realret;
            
     
        }

    

        public Message GetCategory(string domain, string Division, string Catalog, string Category_0)
        {   

            CategoryInfo ret = new CategoryInfo();
            Category c = new Category(Int32.Parse(Category_0));
            DataContractSerializer sel = new DataContractSerializer(c.GetType());
            MemoryStream memoryStream = new MemoryStream();
            sel.WriteObject(memoryStream, c);
            
            Message realret = Message.CreateMessage(MessageVersion.None, "*",GetStream(memoryStream));
            return realret;
        }
        public Message GetCategories(string domain, string Division, string Catalog, string Category_0, string Category_1,
                                     string sort, string query)
         {
            Catalog        c       = new Catalog(Int32.Parse(Catalog));
            Category       cat     = new Category(Int32.Parse(Category_0));
            Category       subcat  = new Category(Int32.Parse(Category_1));

            CatalogCategory cc = new CatalogCategory(Int32.Parse(Catalog), Int32.Parse(Category_0));

            CatalogCategory subc = new CatalogCategory(Int32.Parse(Category_1));

           
            
                List<CatalogCategory> list = cc.SubCategories[0].SubCategories.ToList();
            
        
                List<CatalogCategory> list1 = subc.SubCategories.ToList();
            
           
                List<CatalogCategory> list2 = subc.SubCategories[0].SubCategories.ToList();
                    

            //Gets catalog description
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(c.CreateCatalogFrag.ToString());

           // add xmlfragment to the main Document and add category description
            XmlDocumentFragment docfrag1 = xmldoc.CreateDocumentFragment();
            docfrag1.InnerXml = cat.CreateCategoryFrag.ToString();
            xmldoc.DocumentElement.AppendChild(docfrag1);

            XmlDocumentFragment docfrag2 = xmldoc.CreateDocumentFragment();
            docfrag2.InnerXml = subcat.CreateCategoryFrag.ToString();
            xmldoc.DocumentElement.AppendChild(docfrag2);

            Message realret = Message.CreateMessage(MessageVersion.None, "*",  GetItems( list1,list2,xmldoc));
            return realret;
        
            }

        public Message GetProducts(string domain, string Division, string Catalog, string Category_0, string Category_1,
            string sort, int pagesize, int page, bool inclusive, string query)
        {
            //rewritten products to get the products and subcategories
                        
            CatalogCategory p = new CatalogCategory(Int32.Parse(Catalog),Int32.Parse(Category_0));
            List<CatalogCategory> list = p.SubCategories.ToList();
            List<CatalogProduct>  list1 = p.SubCategories[0].Products.ToList();
            List<CatalogCategory> list2 = p.SubCategories[0].SubCategories.ToList();
            
           
            _inclusive = inclusive;
            _page = page;
            _pagesize = pagesize;
            _query = query;

           
             MemoryStream memoryStream = new MemoryStream();

             if (inclusive == true)
             {
                 DataContractSerializer sel = new DataContractSerializer(list.GetType(), new Type[] { typeof(CatalogCategoryProduct) });
                 sel.WriteObject(memoryStream, list);
                 DataContractSerializer sel2 = new DataContractSerializer(list1.GetType());
                 sel2.WriteObject(memoryStream, list1);
                 DataContractSerializer sel3= new DataContractSerializer(list2.GetType());
                 sel3.WriteObject(memoryStream, list2);
                 //DataContractSerializer sel3 = new DataContractSerializer(typeof(ArrayList), new Type[] { typeof(ArrayList), typeof(CatalogCategory), typeof(CatalogCategoryProduct) });
                 //sel3.WriteObject(memoryStream, allItems);
             
             }
             else
             {
                 DataContractSerializer sel = new DataContractSerializer(list.GetType(), new Type[] { typeof(CatalogCategoryProduct) });
                 sel.WriteObject(memoryStream, list);
             }

            memoryStream.Position = 0;
            StreamReader stream = new StreamReader(memoryStream);
            string xmlstring = stream.ReadToEnd();
            Message realret = Message.CreateMessage(MessageVersion.None, "*",GetStream(xmlstring,memoryStream));
            return realret;
        }

        public Message GetCategory(string domain, string Division, string Catalog, string Category_0, string Category_1)
        {
            Category c = new Category(Int32.Parse(Category_1));
            DataContractSerializer sel = new DataContractSerializer(c.GetType());
            MemoryStream memoryStream = new MemoryStream();
            sel.WriteObject(memoryStream, c);

            Message realret = Message.CreateMessage(MessageVersion.None, "*", GetStream(memoryStream));
            return realret;
        }
        public Message GetCategories(string domain, string Division, string Catalog, string Category_0, string Category_1, string Category_2,
                                 string sort, string query)
        {
            Categories_2Info ret = new Categories_2Info();

            CatalogCategory c = new CatalogCategory(Int32.Parse(Catalog), Int32.Parse(Category_0));
            List<CatalogCategory> list = c.SubCategories[0].SubCategories[0].SubCategories.ToList();
          
            DataContractSerializer sel = new DataContractSerializer(list.GetType());
            MemoryStream memoryStream = new MemoryStream();
            sel.WriteObject(memoryStream, list);
            _query = query;

            switch (sort)
            {
                case "A-Z":
                    ret.Add(new Categories_2Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2, Sort = sort, Search = _query });
                    break;
                case "Z-A":
                    ret.Add(new Categories_2Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2, Sort = sort, Search = _query });
                    break;
                case "Most Popular":
                    ret.Add(new Categories_2Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2, Sort = sort, Search = _query });

                    break;
                case "Price High to Low":
                    ret.Add(new Categories_2Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2, Sort = sort, Search = _query });

                    break;
                case "Price Low to High":
                    ret.Add(new Categories_2Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2, Sort = sort, Search = _query });

                    break;
                default:
                    break;

            }// end of swich statement
            Message realret = Message.CreateMessage(MessageVersion.None, "*", GetStream(memoryStream));
            return realret;
        }

        public Message GetProducts(string domain, string Division, string Catalog, string Category_0, string Category_1, string Category_2,
            string sort, int pagesize, int page, bool inclusive, string query)
        {
            Products_2Info ret = new Products_2Info();

            CatalogCategory c = new CatalogCategory(Int32.Parse(Catalog), Int32.Parse(Category_0));
            List<CatalogCategory> list = c.SubCategories[0].SubCategories[0].SubCategories.ToList();
            List<CatalogProduct> list1 = c.SubCategories[0].SubCategories[0].Products.ToList();
            List<CatalogCategory> list2 = c.SubCategories[0].SubCategories[0].SubCategories[0].SubCategories.ToList();
            List<CatalogProduct> list3 = c.SubCategories[0].SubCategories[0].SubCategories[0].Products.ToList();
           
            DataContractSerializer sel = new DataContractSerializer(list.GetType());
            DataContractSerializer sel1 = new DataContractSerializer(list1.GetType());
            DataContractSerializer sel2 = new DataContractSerializer(list2.GetType());
            DataContractSerializer sel3 = new DataContractSerializer(list3.GetType());


            MemoryStream memoryStream = new MemoryStream();
            sel.WriteObject(memoryStream, list);
            sel1.WriteObject(memoryStream, list1);
            sel2.WriteObject(memoryStream, list2);
            sel3.WriteObject(memoryStream, list3);

            _query = query;
            _inclusive = inclusive;
            _page = page;
            _pagesize = pagesize;
            _query = query;

            {
               
                switch (sort)
                {
                    case "A-Z":
                        ret.Add(new Products_2Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Z-A":
                        ret.Add(new Products_2Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Most Popular":
                        ret.Add(new Products_2Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Price High to Low":
                        ret.Add(new Products_2Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Price Low to High":
                        ret.Add(new Products_2Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    default:
                        break;



                }
                ret.Add(new Products_2Data
                {
                    Company = domain,
                    Division = Division,
                    Catalog = Catalog,
                    Category_0 = Category_0,
                    Category_1 = Category_1,
                    Category_2 = Category_2,
                    Sort = sort,
                    Pagesize = _pagesize,
                    Page = _page,
                    Inclusive = _inclusive,
                    Search = _query
                });
            }

            memoryStream.Position = 0;
            StreamReader stream = new StreamReader(memoryStream);
            string xmlstring = stream.ReadToEnd();
            Message realret = Message.CreateMessage(OperationContext.Current.IncomingMessageVersion, "*", GetStream(xmlstring, memoryStream));
            return realret;
        }

        public Message GetCategory(string domain, string Division, string Catalog, string Category_0, string Category_1, string Category_2)
        {
            Category_2Info ret = new Category_2Info();
            Category c = new Category(Int32.Parse(Category_2));
            DataContractSerializer sel = new DataContractSerializer(c.GetType());

            MemoryStream memoryStream = new MemoryStream();
            sel.WriteObject(memoryStream, c);


            {
                ret.Add(new Category_2Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2 });
            }
            Message realret = Message.CreateMessage(MessageVersion.None, "*", GetStream(memoryStream));
            return realret;
        }

        public Message GetCategories(string domain, string Division, string Catalog, string Category_0, string Category_1, string Category_2, string Category_3,
                                     string sort, string query)
        {
            Categories_3Info ret = new Categories_3Info();
            CatalogCategory c = new CatalogCategory(Int32.Parse(Catalog), Int32.Parse(Category_0));
            List<CatalogCategory> list = c.SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].SubCategories.ToList();

            DataContractSerializer sel = new DataContractSerializer(list.GetType());
            MemoryStream memoryStream = new MemoryStream();
            sel.WriteObject(memoryStream, list);

            _query = query;

            switch (sort)
            {
                case "A-Z":
                    ret.Add(new Categories_3Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2, Category_3 = Category_3, Sort = sort, Search = _query });
                    break;
                case "Z-A":
                    ret.Add(new Categories_3Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2, Category_3 = Category_3, Sort = sort, Search = _query });
                    break;
                case "Most Popular":
                    ret.Add(new Categories_3Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2, Category_3 = Category_3, Sort = sort, Search = _query });

                    break;
                case "Price High to Low":
                    ret.Add(new Categories_3Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2, Category_3 = Category_3, Sort = sort, Search = _query });

                    break;
                case "Price Low to High":
                    ret.Add(new Categories_3Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2, Category_3 = Category_3, Sort = sort, Search = _query });

                    break;
                default:
                    break;

            }// end of swich statement
            Message realret = Message.CreateMessage(MessageVersion.None, "*", GetStream(memoryStream));
            return realret;
        }

        public Message GetProducts(string domain, string Division, string Catalog, string Category_0, string Category_1, string Category_2,
               string Category_3, string sort, int pagesize, int page, bool inclusive, string query)
        {
            Products_3Info ret = new Products_3Info();
            Category c1 = new Category(Int32.Parse(Category_3));
            CatalogCategory c = new CatalogCategory(Int32.Parse(Catalog), Int32.Parse(Category_0));
            List<CatalogCategory> list = c.SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].SubCategories.ToList();
            List<CatalogProduct> list1 = c.SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].Products.ToList();
            List<CatalogCategory> list2 = c.SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].SubCategories.ToList();
            List<CatalogProduct> list3 = c.SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].Products.ToList();

            DataContractSerializer sel4 = new DataContractSerializer(c1.GetType());
            DataContractSerializer sel = new DataContractSerializer(list.GetType());
            DataContractSerializer sel1 = new DataContractSerializer(list1.GetType());
            DataContractSerializer sel2 = new DataContractSerializer(list2.GetType());
            DataContractSerializer sel3 = new DataContractSerializer(list3.GetType());

            MemoryStream memoryStream = new MemoryStream();

            sel4.WriteObject (memoryStream,c1);
            sel.WriteObject (memoryStream, list);
            sel1.WriteObject (memoryStream ,list1);
            sel2.WriteObject (memoryStream ,list2);
            sel3.WriteObject(memoryStream ,list3);
           
            _inclusive = inclusive;
            _page = page;
            _pagesize = pagesize;
            _query = query;

            {
              
                switch (sort)
                {
                    case "A-Z":
                        ret.Add(new Products_3Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Category_3 = Category_3,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Z-A":
                        ret.Add(new Products_3Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Category_3 = Category_3,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Most Popular":
                        ret.Add(new Products_3Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Category_3 = Category_3,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Price High to Low":
                        ret.Add(new Products_3Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Category_3 = Category_3,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Price Low to High":
                        ret.Add(new Products_3Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Category_3 = Category_3,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    default:
                        break;



                }
                ret.Add(new Products_3Data
                {
                    Company = domain,
                    Division = Division,
                    Catalog = Catalog,
                    Category_0 = Category_0,
                    Category_1 = Category_1,
                    Category_2 = Category_2,
                    Category_3 = Category_3,
                    Sort = sort,
                    Pagesize = _pagesize,
                    Page = _page,
                    Inclusive = _inclusive,
                    Search = _query
                });
            }

            memoryStream.Position = 0;
            StreamReader stream = new StreamReader(memoryStream);
            string xmlstring = stream.ReadToEnd();
            Message realret = Message.CreateMessage(OperationContext.Current.IncomingMessageVersion, "*", GetStream(xmlstring, memoryStream));
            return realret;
        }



        public Message GetCategory(string domain, string Division, string Catalog, string Category_0, string Category_1, string Category_2, string Category_3)
        {
            Category_3Info ret = new Category_3Info();
            Category c = new Category(Int32.Parse(Category_3));
            DataContractSerializer sel = new DataContractSerializer(c.GetType());
            MemoryStream memoryStream = new MemoryStream();
            sel.WriteObject(memoryStream, c);


            {
                ret.Add(new Category_3Data { Company = domain, Division = Division, Catalog = Catalog, Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2 });
            }
            Message realret = Message.CreateMessage(MessageVersion.None, "*", GetStream(memoryStream));
            return realret;
        }
        public Message GetCategories(string domain, string Division, string Catalog, string Category_0, string Category_1, string Category_2, string Category_3,
                                   string Category_4, string sort, string query)
        {

            Categories_4Info ret = new Categories_4Info();

           
            CatalogCategory c = new CatalogCategory(Int32.Parse(Catalog), Int32.Parse(Category_0));
            List<CatalogCategory> list = c.SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].SubCategories.ToList();

            DataContractSerializer sel = new DataContractSerializer(list.GetType());
            MemoryStream memoryStream = new MemoryStream();
            sel.WriteObject(memoryStream, list);
            _query = query;

            switch (sort)
            {
                case "A-Z":
                    ret.Add(new Categories_4Data { Company = domain, Division = Division, Catalog = Catalog,
                        Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2,
                                                   Category_3 = Category_3,
                                                   Category_4 = Category_4,
                                                   Sort = sort,
                                                   Search = _query
                    });
                    break;
                case "Z-A":
                    ret.Add(new Categories_4Data { Company = domain, Division = Division, 
                        Catalog = Catalog, Category_0 = Category_0, 
                        Category_1 = Category_1, Category_2 = Category_2,
                                                   Category_3 = Category_3,
                                                   Category_4= Category_4,
                                                   Sort = sort,
                                                   Search = _query
                    });
                    break;
                case "Most Popular":
                    ret.Add(new Categories_4Data { Company = domain, Division = Division, Catalog = Catalog,
                        Category_0 = Category_0, Category_1 = Category_1,
                                                   Category_2 = Category_2,
                                                   Category_3 = Category_3,
                                                   Category_4 = Category_4, 
                        Sort = sort, Search = _query });

                    break;
                case "Price High to Low":
                    ret.Add(new Categories_4Data { Company = domain, Division = Division,
                        Catalog = Catalog, Category_0 = Category_0,
                        Category_1 = Category_1, Category_2 = Category_2,
                                                   Category_3 = Category_3,
                                                   Category_4 = Category_4,
                        Sort = sort, Search = _query });

                    break;
                case "Price Low to High":
                    ret.Add(new Categories_4Data { Company = domain, Division = Division,
                        Catalog = Catalog, Category_0 = Category_0,
                                                   Category_1 = Category_1,
                                                   Category_2 = Category_2,
                                                   Category_3 = Category_3,
                                                   Category_4 = Category_4,
                        Sort = sort, Search = _query });

                    break;
                default:
                    break;

            }// end of swich statement
            Message realret = Message.CreateMessage(MessageVersion.None, "*", GetStream(memoryStream));
            return realret;
        }

        public Message GetProducts(string domain, string Division, string Catalog, string Category_0, string Category_1, string Category_2,
                     string Category_3, string Category_4,string sort, int pagesize, int page, bool inclusive, string query)
        {
            Products_4Info ret = new Products_4Info();

            Category c1 = new Category(Int32.Parse(Category_3));
            CatalogCategory c = new CatalogCategory(Int32.Parse(Catalog), Int32.Parse(Category_0));


            List<CatalogCategory> list = c.SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].SubCategories.ToList();
            List<CatalogProduct> list1 = c.SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].SubCategories[0].Products.ToList();
            DataContractSerializer sel = new DataContractSerializer(c1.GetType());
            DataContractSerializer sel1= new DataContractSerializer(list.GetType());
            DataContractSerializer sel2 = new DataContractSerializer(list1.GetType());

            MemoryStream memoryStream = new MemoryStream();

            sel.WriteObject(memoryStream, c1);
            sel1.WriteObject(memoryStream, list);
            sel2.WriteObject(memoryStream, list1);




            _inclusive = inclusive;
            _page = page;
            _pagesize = pagesize;
            _query = query;

            {
                for (int i = 0; i < 200; i++)
                { pagesize = (_pagesize+i); }
                switch (sort)
                {
                    case "A-Z":
                        ret.Add(new Products_4Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Category_3 = Category_3,
                            Category_4 = Category_4,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Z-A":
                        ret.Add(new Products_4Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Category_3 = Category_3,
                            Category_4 = Category_4,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Most Popular":
                        ret.Add(new Products_4Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Category_3 = Category_3,
                            Category_4 = Category_4,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Price High to Low":
                        ret.Add(new Products_4Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Category_3 = Category_3,
                            Category_4 = Category_4,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    case "Price Low to High":
                        ret.Add(new Products_4Data
                        {
                            Company = domain,
                            Division = Division,
                            Catalog = Catalog,
                            Category_0 = Category_0,
                            Category_1 = Category_1,
                            Category_2 = Category_2,
                            Category_3 = Category_3,
                            Category_4 = Category_4,
                            Sort = sort,
                            Pagesize = _pagesize,
                            Page = _page,
                            Inclusive = _inclusive,
                            Search = _query
                        });
                        break;
                    default:
                        break;



                }
                ret.Add(new Products_4Data
                {
                    Company = domain,
                    Division = Division,
                    Catalog = Catalog,
                    Category_0 = Category_0,
                    Category_1 = Category_1,
                    Category_2 = Category_2,
                    Category_3 = Category_3,
                    Category_4 = Category_4,
                    Sort = sort,
                    Pagesize = _pagesize,
                    Page = _page,
                    Inclusive = _inclusive,
                    Search = _query
                });
            }

            memoryStream.Position = 0;
            StreamReader stream = new StreamReader(memoryStream);
            string xmlstring = stream.ReadToEnd();
            Message realret = Message.CreateMessage(OperationContext.Current.IncomingMessageVersion, "*", GetStream(xmlstring, memoryStream));
            return realret;
        }

        public Message GetCategory(string domain, string Division, string Catalog, string Category_0, string Category_1, string Category_2, string Category_3,string Category_4)
        {
            Category_4Info ret = new Category_4Info();
            Category c = new Category(Int32.Parse(Category_4));
            DataContractSerializer sel = new DataContractSerializer(c.GetType());
            MemoryStream memoryStream = new MemoryStream();
            sel.WriteObject(memoryStream, c);
            {
                ret.Add(new Category_4Data { Company = domain, Division = Division, Catalog = Catalog,
                    Category_0 = Category_0, Category_1 = Category_1, Category_2 = Category_2,Category_3=Category_3,Category_4=Category_4
                });
            }
            Message realret = Message.CreateMessage(MessageVersion.None, "*", GetStream(memoryStream));
            return realret;
        }

         /// <summary>
         /// this method is used to return maximum number of record per page
         /// maximum page size is 200( here for testing 10) so per page 10 records are displayed
         /// if page=1, then page should display records from record 1 to 10
         /// page 2 should display 11-20
         /// </summary>
         /// <param name="Page"></param>
         /// <param name="Catalog"></param>
         /// <param name="Pagesize"></param>
         /// <returns></returns>
      
         public List<CatalogProduct> GetItems(int pageNo,int catalog)
          {
              if (pageNo == 0)
                { 
                  pageNo = 1; 
                }

             Catalog c = new Catalog(catalog);

             List<CatalogProduct> productList = c.Products.ToList();  

             List<CatalogProduct> pageProducts = new List<CatalogProduct>();

                int pageSize = 200;
                
                int firstRecord = (pageNo - 1) * pageSize + 1;

                int totalNumProducts = productList.Count();


                //if first record is >totalnum of products
                if (firstRecord > totalNumProducts)
                        return null;

                for (int i = firstRecord ; i< firstRecord+pageSize; )
                {
                    pageProducts.Add(productList.ElementAt(i-1));               
                    pageProducts.Count();
                    if (++i > totalNumProducts)
                    break;
                }
                
                return pageProducts;
               

        }

         /// <summary>
         /// returns xmldictionary reader 
         /// </summary>
         /// <param name="memoryStream"></param>
         /// <returns></returns>

         private XmlDictionaryReader GetStream(MemoryStream memoryStream)
         {
             memoryStream.Position = 0;
             
             StreamReader streamReader = new StreamReader(memoryStream);
             
             XmlReader reader = null;

             reader = new XmlTextReader(streamReader);
             string v = null;
             string s = null;
             StringBuilder buff = new StringBuilder();
             XmlDocument doc = new XmlDocument();
             while (reader.Read())
             {

               //  v += s;
                 switch (reader.NodeType)
                 {
                     case XmlNodeType.Element:
                         bool isEmpty = reader.IsEmptyElement;
                         buff.Append("<" + reader.Name);
                       //  s = "<" + reader.Name;
                         //read attribute
                         while (reader.MoveToNextAttribute())
                             buff.Append(" " + reader.Name + "='" + reader.Value + "'");
                             //s = s + " " + reader.Name + "='" + reader.Value + "'";
                         if (isEmpty)
                         {
                             buff.Append("/>");
                           //  s = s + "/>";
                         }
                         else
                             buff.Append(">");

                             //s = s + ">";
                         break;
                     case XmlNodeType.Text: //Display the text in each element.
                         buff.Append(reader.Value);
                        // s = reader.Value;
                         break;
                     case XmlNodeType.EndElement://Display End of element
                         buff.Append("</" + reader.Name + ">");
                        // s = ("</" + reader.Name);
                        // s = s + ">";
                         break;

                 }
                
                
             }
             //v += s;
           
             doc.Load(buff.ToString());
             doc.Save(memoryStream);

            FileStream stream = new FileStream("@cat.xml", FileMode.Open);
            XmlDictionaryReader xdr =
               XmlDictionaryReader.CreateTextReader(stream,
                                new XmlDictionaryReaderQuotas());
             return xdr;
         }
         private XmlDictionaryReader GetStream(string xmlString,MemoryStream memoryStream)
         {
             memoryStream.Position = 0;

           //  StreamReader streamReader = new StreamReader(memoryStream);

             StringReader str = new StringReader("<string>"+xmlString+"</string>");
             XmlReader reader = null;

             reader = new XmlTextReader(str);
             string v = null;
             string s = null;
            StringBuilder buff = new StringBuilder();
             while (reader.Read())
             {

               //  v += s;
                 switch (reader.NodeType)
                 {
                     case XmlNodeType.Element:
                         bool isEmpty = reader.IsEmptyElement;
                         buff.Append("<" + reader.Name);
                       //  s = "<" + reader.Name;
                         //read attribute
                         while (reader.MoveToNextAttribute())
                             buff.Append(" " + reader.Name + "='" + reader.Value + "'");
                             //s = s + " " + reader.Name + "='" + reader.Value + "'";
                         if (isEmpty)
                         {
                             buff.Append("/>");
                           //  s = s + "/>";
                         }
                         else
                             buff.Append(">");

                             //s = s + ">";
                         break;
                     case XmlNodeType.Text: //Display the text in each element.
                         buff.Append(reader.Value);
                        // s = reader.Value;
                         break;
                     case XmlNodeType.EndElement://Display End of element
                         buff.Append("</" + reader.Name + ">");
                        // s = ("</" + reader.Name);
                        // s = s + ">";
                         break;

                 }

             }
             //v += s;
             XmlDocument doc = new XmlDocument();
             doc.LoadXml(buff.ToString());

             doc.Save(@"cat.xml");

             FileStream stream = new FileStream(@"cat.xml", FileMode.Open);
             XmlDictionaryReader xdr =
                    XmlDictionaryReader.CreateTextReader(stream,
                                new XmlDictionaryReaderQuotas());
             return xdr;
         }
         public class XmlElementBodyWriter : BodyWriter
         {

             XmlElement xmlElement;
             public XmlElementBodyWriter(XmlElement xmlElement):base(true)
         {
             this.xmlElement = xmlElement;
            
         }
             protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
             {
                 this.xmlElement.WriteContentTo(writer);
                 writer.WriteEndDocument();
             }
         

         }

         private List<CatalogProduct>  GetItems1(int pageNo, int p)
         {
             CatalogCategory c = new CatalogCategory(p);

             List<CatalogProduct> productList = c.Products.ToList();
             List<CatalogCategory> subcategoryList = c.SubCategories.ToList();
             List<CatalogProduct> subcategoryProductList = c.SubCategories[0].Products.ToList();
             List<CatalogProduct> pageProducts= new List<CatalogProduct>();
             ArrayList pageProduct= new ArrayList();
            // List<CatalogCategory> pageCategories = new List<CatalogCategory>();
             if (pageNo == 0)
             { pageNo = 1; }
             int pageSize = 10;
             int firstRecord = (pageNo - 1) * pageSize + 1;

               int totalNumProducts = productList.Count();
           //  int totalNumProducts = productList.Count() + subcategoryList.Count() + subcategoryProductList.Count();

             //if first record is >totalnum of products
             if (firstRecord > totalNumProducts)
                 return null;

             for (int i = firstRecord; i < firstRecord + pageSize; )
             {   // handle null exception
                
                 pageProducts.Add(productList.ElementAt(i - 1));
                 pageProducts.Count();
               
                 if (++i > totalNumProducts)
                     break;
             }
            
             return pageProducts;

             
         }


         public XmlDictionaryReader GetItems(List<CatalogCategory > list,XmlDocument xmldoc)

         {
                 foreach (CatalogCategory c in list)
                 {
                     string s = c.CreateCategorylistFrag.ToString();
                     XmlDocumentFragment docfrag = xmldoc.CreateDocumentFragment();
                     docfrag.InnerXml = s;
                     xmldoc.DocumentElement.AppendChild(docfrag);
                   
                 }

                 string xmlstring = xmldoc.OuterXml;
                     
                 StringReader str = new StringReader(xmlstring);
                 XmlReader reader = null;

                 reader = new XmlTextReader(str);

                 StringBuilder buff = new StringBuilder();
                 while (reader.Read())
                 {


                     switch (reader.NodeType)
                     {
                         case XmlNodeType.Element:
                             bool isEmpty = reader.IsEmptyElement;
                             buff.Append("<" + reader.Name);
                             while (reader.MoveToNextAttribute())
                                 buff.Append(" " + reader.Name + "='" + reader.Value + "'");

                             if (isEmpty)
                             {
                                 buff.Append("/>");

                             }
                             else
                                 buff.Append(">");


                             break;
                         case XmlNodeType.Text: //Display the text in each element.
                             buff.Append(reader.Value);

                             break;
                         case XmlNodeType.EndElement://Display End of element
                             buff.Append("</" + reader.Name + ">");

                             break;

                     }

                 }
                 XmlDocument doc = new XmlDocument();
                 doc.LoadXml(buff.ToString());
                
                 MemoryStream ms = new MemoryStream();
                 xmldoc.Save(ms);
                 XmlDictionaryWriter xw = XmlDictionaryWriter.CreateTextWriter(ms);
                 xw.WriteStartDocument();
                           
                 ms.Position = 0;
                 XmlDictionaryReader xdr =
                   XmlDictionaryReader.CreateTextReader(ms,
                               new XmlDictionaryReaderQuotas());

                 return xdr;


            }
         public XmlDictionaryReader GetItems(List<CatalogCategory> list,List<CatalogCategory> list1, XmlDocument xmldoc)
         {
             foreach (CatalogCategory c in list)
             {
                 string s = c.CreateCategorylistFrag.ToString();
                 XmlDocumentFragment docfrag = xmldoc.CreateDocumentFragment();
                 docfrag.InnerXml = s;
                 xmldoc.DocumentElement.AppendChild(docfrag);

             }
             foreach (CatalogCategory c in list1)
             {
                 string s = c.CreateCategorylistFrag.ToString();
                 XmlDocumentFragment docfrag = xmldoc.CreateDocumentFragment();
                 docfrag.InnerXml = s;
                 xmldoc.DocumentElement.AppendChild(docfrag);

             }


             string xmlstring = xmldoc.OuterXml;

             StringReader str = new StringReader(xmlstring);
             XmlReader reader = null;

             reader = new XmlTextReader(str);

             StringBuilder buff = new StringBuilder();
             while (reader.Read())
             {


                 switch (reader.NodeType)
                 {
                     case XmlNodeType.Element:
                         bool isEmpty = reader.IsEmptyElement;
                         buff.Append("<" + reader.Name);
                         while (reader.MoveToNextAttribute())
                             buff.Append(" " + reader.Name + "='" + reader.Value + "'");

                         if (isEmpty)
                         {
                             buff.Append("/>");

                         }
                         else
                             buff.Append(">");


                         break;
                     case XmlNodeType.Text: //Display the text in each element.
                         buff.Append(reader.Value);

                         break;
                     case XmlNodeType.EndElement://Display End of element
                         buff.Append("</" + reader.Name + ">");

                         break;

                 }

             }
             XmlDocument doc = new XmlDocument();
             doc.LoadXml(buff.ToString());

             MemoryStream ms = new MemoryStream();
             xmldoc.Save(ms);
             XmlDictionaryWriter xw = XmlDictionaryWriter.CreateTextWriter(ms);
             xw.WriteStartDocument();

             ms.Position = 0;
             XmlDictionaryReader xdr =
               XmlDictionaryReader.CreateTextReader(ms,
                           new XmlDictionaryReaderQuotas());

             return xdr;


         }

         public XmlDictionaryReader GetItems(List<CatalogProduct> products, List<CatalogCategory> categories, XmlDocument xmldoc)
         {
             foreach (CatalogProduct c in products)
             {
                 string s = c.CreateProductlistFrag .ToString();
                 XmlDocumentFragment docfrag = xmldoc.CreateDocumentFragment();
                 docfrag.InnerXml = s;
                 xmldoc.DocumentElement.AppendChild(docfrag);

             }
             foreach (CatalogCategory c in categories)
             {
                 string s = c.CreateCategorylistFrag.ToString();
                 XmlDocumentFragment docfrag = xmldoc.CreateDocumentFragment();
                 docfrag.InnerXml = s;
                 xmldoc.DocumentElement.AppendChild(docfrag);

             }


             string xmlstring = xmldoc.OuterXml;

             StringReader str = new StringReader(xmlstring);
             XmlReader reader = null;

             reader = new XmlTextReader(str);

             StringBuilder buff = new StringBuilder();
             while (reader.Read())
             {


                 switch (reader.NodeType)
                 {
                     case XmlNodeType.Element:
                         bool isEmpty = reader.IsEmptyElement;
                         buff.Append("<" + reader.Name);
                         while (reader.MoveToNextAttribute())
                             buff.Append(" " + reader.Name + "='" + reader.Value + "'");

                         if (isEmpty)
                         {
                             buff.Append("/>");

                         }
                         else
                             buff.Append(">");


                         break;
                     case XmlNodeType.Text: //Display the text in each element.
                         buff.Append(reader.Value);

                         break;
                     case XmlNodeType.EndElement://Display End of element
                         buff.Append("</" + reader.Name + ">");

                         break;

                 }

             }
             XmlDocument doc = new XmlDocument();
             doc.LoadXml(buff.ToString());

             MemoryStream ms = new MemoryStream();
             xmldoc.Save(ms);
             XmlDictionaryWriter xw = XmlDictionaryWriter.CreateTextWriter(ms);
             xw.WriteStartDocument();

             ms.Position = 0;
             XmlDictionaryReader xdr =
               XmlDictionaryReader.CreateTextReader(ms,
                           new XmlDictionaryReaderQuotas());

             return xdr;


         }
    }

}
