using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopperholics.Models;

namespace Shopperholics.Repositories
{
    public interface IShopperholicsRepository //Repository used for order control drop down to edit and delete and create
    { //can also be used for search advanced
        IEnumerable<CustomerOrder> GetOrders();
        IEnumerable<Products> GetProducts();
        IEnumerable<productCategory> getProductCategory();
        IEnumerable<productsubcategory> GetProductsubcategories();

        CustomerOrder GetOrderbyId(int id);

        Products GetProductsbyId(int id);

        Products GetProductsCatbyId(int id);

        productCategory GetProductSubCategory(int id);

        void AddCustomers(Customers customer);
        void AddOrder(CustomerOrder order);
        void AddProduct(Products product);
        void AddProductCategory(productCategory productcat);
        void AddProductSubCategory(productsubcategory productsubcat);
        void RemoveOrder(int id);
        void removeProduct(int id);
        void removeProductCategory(int id);
        void RemoveProductsubCategory(int id);

        void SaveChanges();

        IQueryable<CustomerOrder> PopulateCustomerOrdersdropdownList();
       
        IQueryable<productCategory> PopulateProductCategories();

        IQueryable<productsubcategory> PopulatesubProductCategories();

        IQueryable<Vendors> PopulateProductVendors();
    }
}
