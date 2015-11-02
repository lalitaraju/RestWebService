using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using DirectResponse.Catalog.BusinessObjects;
using System.Web.Script.Services;


namespace CatalogService
{
    [ServiceContract]
    public interface ICatalogService
    {

        [OperationContract]
        [WebGet(UriTemplate = "/")]
        Message GetRoot();

        [OperationContract]
        [WebGet(BodyStyle=WebMessageBodyStyle.Bare,
                RequestFormat=WebMessageFormat.Xml,
                ResponseFormat=WebMessageFormat.Xml,
                UriTemplate = "/{domain}")]
        Message GetCompany(string domain);

        [OperationContract]
        [WebGet(UriTemplate = "/{domain}/{Division}/")]
        Message GetDivision(string domain, string Division);

    
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare,
                RequestFormat = WebMessageFormat.Xml,
                ResponseFormat = WebMessageFormat.Xml,
                UriTemplate = "/{domain}/{Division}/{Catalog}/categories?" +
                "sort={sort}&search={query}")]
         Message GetCategories(string domain, string Division, string Catalog,
           string sort, string Query);
      
        [OperationContract]
        [WebGet(UriTemplate = "/{domain}/{Division}/{Catalog}/products?" +
                "sort={sort}&pagesize={pagesize}&page={page}&inclusive={inclusive}&search={query}")]
        Message GetProducts( string domain, string Division, string Catalog,
                string sort, int pagesize, int page, bool inclusive, string query); 

//Level 0 for Category_0 

         [OperationContract(Name="levelC_0")]
         [WebGet(UriTemplate = "/{domain}/{Division}/{Catalog}/{Category_0}/categories?" +
                "sort={sort}&search={query}")]
        Message GetCategories(string domain, string Division, string Catalog, string Category_0,
                 string sort, string query);
        
        [OperationContract(Name= "levelP_0")]
        [WebGet(UriTemplate = "/{domain}/{Division}/{Catalog}/{Category_0}/products?" +
                "sort={sort}&pagesize={pagesize}&page={page}&inclusive={inclusive}&search={query}")]
        Message GetProducts(string domain, string Division, string Catalog, string Category_0,
                string sort, int pagesize, int page, bool inclusive, string query); 

        
        [OperationContract(Name="level_0")]
        [WebGet(UriTemplate = "/{domain}/{Division}/{Catalog}/{Category_0}")]
        Message GetCategory(string domain, string Division, string Catalog, string Category_0);

//Level 1

        [OperationContract(Name="levelC_1")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}/categories?" +
            "sort={sort}&search={query}")]
        Message GetCategories(string Company, string Division, string Catalog, string Category_0, string Category_1,
            string sort, string query);

       
        [OperationContract(Name="levelP_1")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}/products?" +
            "sort={sort}&pagesize={pagesize}&page={page}&inclusive={inclusive}&search={query}")]
        Message GetProducts(string Company, string Division, string Catalog, string Category_0, string Category_1,
            string sort, int pagesize, int page, bool inclusive, string query);

       
        [OperationContract(Name="level_1")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}")]
        Message GetCategory(string Company, string Division, string Catalog, string Category_0, string Category_1);

   
        //level 2 

 
        [OperationContract(Name ="LevelC_2")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}/{Category_2}/categories?" +
            "sort={sort}&search={query}")]
        Message GetCategories(string Company, string Division, string Catalog, string Category_0, string Category_1, string Category_2,
            string sort, string query);

        
        [OperationContract(Name ="LevelP_2")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}/{Category_2}/products" +
            "?sort={sort}&pagesize={pagesize}&page={page}&inclusive={inclusive}&search={query}")]
        Message GetProducts(string Company, string Division, string Catalog, string Category_0, string Category_1,string Category_2,
            string sort, int pagesize, int page, bool inclusive, string query);

        // Returns a Category and a summary list of all Sub Categories and Products 

        [OperationContract(Name="Level_2")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}/{Category_2}")]
        Message GetCategory(string Company, string Division, string Catalog, string Category_0, string Category_1, string Category_2);

//level 3

        [OperationContract( Name ="levelC_3")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}/{Category_2}/{Category_3}/categories?" +
            "sort={sort}&search={query}")]
        Message GetCategories(string Company, string Division, string Catalog, string Category_0, string Category_1, string Category_2, string Category_3,
            string sort, string query);


        [OperationContract(Name="levelP_3")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}/{Category_2}/{Category_3}/products" +
            "?sort={sort}&pagesize={pagesize}&page={page}&inclusive={inclusive}&search={query}")]
        Message GetProducts(string Company, string Division, string Catalog, string Category_0, string Category_1, string Category_2,
           string Category_3,string sort, int pagesize, int page, bool inclusive, string query);

        // Returns a Category and a summary list of all Sub Categories and Products 
        
        [OperationContract(Name="Level_3")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}/{Category_2}/{Category_3}")]
        Message GetCategory(string Company, string Division, string Catalog, string Category_0, string Category_1, string Category_2,string Category_3);


        //level 4


        [OperationContract(Name = "levelC_4")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}/{Category_2}/{Category_3}/{Category_4}/categories?" +
            "sort={sort}&search={query}")]
        Message GetCategories(string Company, string Division, string Catalog, string Category_0, string Category_1, string Category_2, string Category_3,
            string Category_4,string sort, string query);


        [OperationContract(Name = "levelP_4")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}/{Category_2}/{Category_3}/{Category_4}/products" +
            "?sort={sort}&pagesize={pagesize}&page={page}&inclusive={inclusive}&search={query}")]
        Message GetProducts(string Company, string Division, string Catalog, string Category_0, string Category_1, string Category_2,
           string Category_3,string Category_4,string sort, int pagesize, int page, bool inclusive, string query);
      
        // Returns a Category and a summary list of all Sub Categories and Products 
       
        [OperationContract(Name = "Level_4")]
        [WebGet(UriTemplate = "/{Company}/{Division}/{Catalog}/{Category_0}/{Category_1}/{Category_2}/{Category_3}/{Category_4}")]
        Message GetCategory(string Company, string Division, string Catalog, string Category_0, string Category_1, string Category_2, string Category_3,string Category_4);

     
        
    }

   //Domain dataitems
    [DataContract(Namespace = "")]
    public class Domain
    {
        [DataMember]
        public string Name;
        [DataMember]
        public string Uri;
        [DataMember(Name="Hyperlink")]
        public string Hyperlink;
        
    }
    [CollectionDataContract(Name = "Domains", Namespace = "")]

    public class DomainList : List<Domain>
    { }
    
//division dataitems 

    [DataContract(Namespace = "", Name = "Company")]
    public class DivisionData
    {
        [DataMember( Name="domain")]
        public string CompanyName;
        [DataMember]
       public  string Division;
        [DataMember(Name = "Hyperlink")]
        public string Hyperlink;
    }
    public class DivisionList : List<DivisionData>
    { }

    //Catalog dataitems
    [DataContract(Name = "Catalog", Namespace = "")]

    public class CatalogData
    {
        [DataMember(Name = "Domain")]
        public string Company;

        [DataMember(Name = "Division")]
        public string Division;

       
        [DataMember (Name="Catalog") ]
        public string Catalog;
        [DataMember]
        public Uri Hyperlink;
        
    }
    public class CatalogInfo : List<CatalogData>
    { }

   
    //define the datamembers of Catalog categories sorting, passing query
    public class CategoriesData
    {
        [DataMember(Name = "Domain")]
        public string Company;
        
        [DataMember(Name = "Division")]
        public string Division;

       
        [DataMember (Name="Catalog") ]
        public string Catalog;

        [DataMember]
        public int CategoryId;

       //DataMember]
      //  public int CategoryId;
       
        [DataMember]
        public string Sort;

        [DataMember]
        public string Search;

        [DataMember(Name = "Hyperlink")]
        public string Hyperlink;
    }
    
   public class CategoriesInfo : List<CategoriesData>
   {
       internal void Add(Catalog catalog)
       {
           throw new NotImplementedException();
       }
   }

   //string sort, int pagesize, int page, bool inclusive, string query
   public class ProductsData
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember]
       public int Id;

       [DataMember]
       public int ProductId;
       [DataMember] 
       public string Sort;

       [DataMember]
       public string Search;

       [DataMember]
       public int Pagesize;

       [DataMember]
       public int Page;

       [DataMember]
       public bool Inclusive;
   
   }
   public class ProductsInfo1 : List<CatalogProduct>
   { }
   public class ProductsInfo : List<ProductsData>
   { }

   public class Categories_0Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember]
       public int Id;

       [DataMember]
       public int CategoryId;

       [DataMember]
       public string Sort;

       [DataMember]
       public string Search;
   }
   public class Categories_0Info : List<Categories_0Data>
   { }

   public class Products_0Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember (Name="Category_0")]
       public string Category_0;

       [DataMember]
       public int Id;

       [DataMember]
       public int ProductId;


       [DataMember]
       public string Sort;

       [DataMember]
       public string Search;

       [DataMember]
       public int Pagesize;

       [DataMember]
       public int Page;

       [DataMember]
       public bool Inclusive;

   }
   public class Products_0Info : List<Products_0Data>
   { }

   public class CategoryData
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category")]
       public string Category;
       
       [DataMember]
       public int Id;

       [DataMember]
       public int CategoryId;

   }
   public class CategoryInfo : List<CategoryData>
   { }
   public class Categories_1Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember(Name = "Category_1")]
       public string Category_1;

       [DataMember]
       public int Id;

       [DataMember]
       public int CategoryId;
       [DataMember]
       public string Sort;

       [DataMember]
       public string Search;
   }
   public class Categories_1Info : List<Categories_1Data>
   { }
   public class Products_1Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;
       
       [DataMember(Name = "Category_1")]
       public string Category_1;

       [DataMember]
       public int Id;

       [DataMember]
       public int ProductId;

       [DataMember]
       public string Sort;

       [DataMember]
       public string Search;

       [DataMember]
       public int Pagesize;

       [DataMember]
       public int Page;

       [DataMember]
       public bool Inclusive;

   }
   public class Products_1Info : List<Products_1Data>
   { }


   public class Category_1Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember(Name = "Category_1")]
       public string Category_1;
      
       [DataMember]
       public int Id;

       [DataMember]
       public int CategoryId;

       

   }
   public class Category_1Info : List<Category_1Data>
   { }

   public class Categories_2Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember(Name = "Category_1")]
       public string Category_1;


       [DataMember(Name = "Category_2")]
       public string Category_2;
      
       [DataMember]
       public int Id;

       [DataMember]
       public int CategoryId;
      
       [DataMember]
       public string Sort;

       [DataMember]
       public string Search;
   }
   public class Categories_2Info : List<Categories_2Data>
   { }
   public class Products_2Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember(Name = "Category_1")]
       public string Category_1;


       [DataMember(Name = "Category_2")]
       public string Category_2;
       [DataMember]
       public int Id;

       [DataMember]
       public int ProductId;

       [DataMember]
       public string Sort;

       [DataMember]
       public string Search;

       [DataMember]
       public int Pagesize;

       [DataMember]
       public int Page;

       [DataMember]
       public bool Inclusive;

   }
   public class Products_2Info : List<Products_2Data>
   { }


   public class Category_2Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember(Name = "Category_1")]
       public string Category_1;


       [DataMember(Name = "Category_2")]
       public string Category_2;

       [DataMember]
       public int Id;

       [DataMember]
       public int CategoryId;
   }
   public class Category_2Info : List<Category_2Data>
   { }

   public class Categories_3Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember(Name = "Category_1")]
       public string Category_1;


       [DataMember(Name = "Category_2")]
       public string Category_2;
       
       [DataMember(Name = "Category_3")]
       public string Category_3;

       [DataMember]
       public int Id;

       [DataMember]
       public int CategoryId;
       [DataMember]
       public string Sort;

       [DataMember]
       public string Search;
   }
   public class Categories_3Info : List<Categories_3Data>
   { }
   public class Products_3Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember(Name = "Category_1")]
       public string Category_1;


       [DataMember(Name = "Category_2")]
       public string Category_2;
      
       [DataMember(Name = "Category_3")]
       public string Category_3;
      
       [DataMember]
       public int Id;

       [DataMember]
       public int ProductId;

       [DataMember]
       public string Sort;

       [DataMember]
       public string Search;

       [DataMember]
       public int Pagesize;

       [DataMember]
       public int Page;

       [DataMember]
       public bool Inclusive;

   }
   public class Products_3Info : List<Products_3Data>
   { }


   public class Category_3Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember(Name = "Category_1")]
       public string Category_1;


       [DataMember(Name = "Category_2")]
       public string Category_2;

       [DataMember(Name = "Category_3")]
       public string Category_3;
       
       [DataMember]
       public int Id;

       [DataMember]
       public int CategoryId;

   }
   public class Category_3Info : List<Category_3Data>
   { }
   public class Categories_4Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember(Name = "Category_1")]
       public string Category_1;


       [DataMember(Name = "Category_2")]
       public string Category_2;

       [DataMember(Name = "Category_3")]
       public string Category_3;
      
       [DataMember(Name = "Category_4)")]
       public string Category_4;

       [DataMember]
       public int Id;

       [DataMember]
       public int CategoryId;

       [DataMember]
       public string Sort;

       [DataMember]
       public string Search;
   }
   public class Categories_4Info : List<Categories_4Data>
   { }
   public class Products_4Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember(Name = "Category_1")]
       public string Category_1;


       [DataMember(Name = "Category_2")]
       public string Category_2;

       [DataMember(Name = "Category_3")]
       public string Category_3;

       [DataMember(Name = "Category_4")]
       public string Category_4;
     
       [DataMember]
       public int Id;

       [DataMember]
       public int ProductId;

       [DataMember]
       public string Sort;

       [DataMember]
       public string Search;

       [DataMember]
       public int Pagesize;

       [DataMember]
       public int Page;

       [DataMember]
       public bool Inclusive;

   }
   public class Products_4Info : List<Products_4Data>
   { }


   public class Category_4Data
   {
       [DataMember(Name = "Domain")]
       public string Company;

       [DataMember(Name = "Division")]
       public string Division;


       [DataMember(Name = "Catalog")]
       public string Catalog;

       [DataMember(Name = "Category_0")]
       public string Category_0;

       [DataMember(Name = "Category_1")]
       public string Category_1;


       [DataMember(Name = "Category_2")]
       public string Category_2;

       [DataMember(Name = "Category_3")]
       public string Category_3;

       [DataMember(Name = "Category_4")]
       public string Category_4;
       [DataMember]
       public int Id;

       [DataMember]
       public int CategoryId;
   }
   public class Category_4Info : List<Category_4Data>
   { }
   

}

