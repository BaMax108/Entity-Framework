using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace HW_173
{
    class SqlConnections
    {
        public ProductsDBEntities ProductContext { get; private set; }
        public UsersDBEntities UserContext { get; private set; }

        public List<Users> ListUsers { get; private set; }
        public List<Products> ListProducts { get; private set; }
        public List<Orders> ListInitialOrders { get; private set; }
        public List<LINQOrder> ListOrders { get; private set; }

        public SqlConnections()
        {
            ListUsers = new List<Users>();
            ListProducts = new List<Products>();
            ListOrders = new List<LINQOrder>();

            UserContext = new UsersDBEntities();
            ProductContext = new ProductsDBEntities();
        }

        /// <summary>
        /// Получение данных из таблицы Products базы данных ProductsDB
        /// </summary>
        public void SelectProducts()
        {
            ListProducts = ProductContext.Products.ToList();
        }

        /// <summary>
        /// Получение данных из таблицы Users базы данных UsersDB
        /// </summary>
        public void SelectUsers()
        {
            ListUsers = UserContext.Users.ToList();
        }

        /// <summary>
        /// Сопоставление данных из таблицы Orders базы данных UsersDB с данными из других БД
        /// </summary>
        public void SelectOrders()
        {
            ListInitialOrders = UserContext.Orders.ToList();
            ListOrders = new List<LINQOrder>();
            foreach (var l in ListInitialOrders)
            {
                ListOrders.Add(new LINQOrder()
                {
                    Id = l.Id,
                    Email = ListUsers.Find(e => e.Id == l.UserId).Email,
                    Product = ListProducts.Find(e => e.Id == l.ProductId).ProductName,
                    Count = l.ProductCount
                });
            }
        }

        #region Remove
        /// <summary>
        /// Удаление выбранной строки
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Users item)
        {
            UserContext.Users.Remove(item);
            UserContext.SaveChanges();
            SelectUsers();
        }
        /// <summary>
        /// Удаление выбранной строки
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Orders item)
        {
            UserContext.Orders.Remove(item);
            UserContext.SaveChanges();
            SelectOrders();
        }
        /// <summary>
        /// Удаление выбранной строки
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Products item)
        {
            ProductContext.Products.Remove(item);
            ProductContext.SaveChanges();
            SelectProducts();
        }
        #endregion

        #region Create/Update
        /// <summary>
        /// Добавление или обновление строки
        /// </summary>
        /// <param name="item"></param>
        public void CreateOrUpdate(Users item)
        {
            UserContext.Users.AddOrUpdate(item, item);
            UserContext.SaveChanges();
        }
        /// <summary>
        /// Добавление или обновление строки
        /// </summary>
        /// <param name="item"></param>
        public void CreateOrUpdate(Orders item)
        {
            UserContext.Orders.AddOrUpdate(item, item);
            UserContext.SaveChanges();
        }
        /// <summary>
        /// Добавление или обновление строки
        /// </summary>
        /// <param name="item"></param>
        public void CreateOrUpdate(Products item)
        {
            ProductContext.Products.AddOrUpdate(item);
            ProductContext.SaveChanges();
        }
        #endregion
    }

    public class LINQOrder
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Product { get; set; }
        public int Count { get; set; }
    }
}