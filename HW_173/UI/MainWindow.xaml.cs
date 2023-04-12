using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HW_173
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnections sql = new SqlConnections();

        private Products CurrentProduct;
        private Users CurrentUser;
        private Orders CurrentOrder;

        public MainWindow()
        {
            InitializeComponent();
            SelectProducts();
            SelectUsers();
            SelectOrders();
        }

        /// <summary>
        /// Обновление списка продуктов
        /// </summary>
        private void SelectProducts()
        {
            sql.SelectProducts();
            dgData.ItemsSource = sql.ListProducts;
        }
        /// <summary>
        /// Обновление списка пользователей
        /// </summary>
        private void SelectUsers()
        {
            sql.SelectUsers();
            dgUsers.ItemsSource = sql.ListUsers;

        }
        /// <summary>
        /// Обновление списков заказов
        /// </summary>
        private void SelectOrders()
        {
            sql.SelectOrders();
            dgOrdersTop.ItemsSource = sql.ListInitialOrders;
            dgOrdersBottom.ItemsSource = sql.ListOrders;
        }

        #region Scrolling
        // Объединение вертикальной прокрутки 2х списков

        private ScrollViewer ScrollingTop, ScrollingBottom;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollingTop = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this.dgOrdersTop, 0), 0) as ScrollViewer;
            ScrollingBottom = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this.dgOrdersBottom, 0), 0) as ScrollViewer;

            ScrollingTop.ScrollChanged += new ScrollChangedEventHandler(OrderScrollingTop_ScrollChanged);
            ScrollingBottom.ScrollChanged += new ScrollChangedEventHandler(OrderScrollingBottom_ScrollChanged);
        }

        private void OrderScrollingTop_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollingBottom.ScrollToVerticalOffset(ScrollingTop.VerticalOffset);
        }

        private void OrderScrollingBottom_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollingTop.ScrollToVerticalOffset(ScrollingBottom.VerticalOffset);
        }
        #endregion

        #region SelectionChanged
        // Отслеживание выбора строк в таблицах

        private void DgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = dgUsers.SelectedItem;
            if (item != null) CurrentUser = (Users)item;
        }

        private void DgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = dgData.SelectedItem;
            if (item != null) CurrentProduct = (Products)item;
        }

        private void DgOrdersTop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = dgOrdersTop.SelectedItem;
            if (item != null) CurrentOrder = (Orders)item;
        }

        private void DgOrdersBottom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = dgOrdersBottom.SelectedItem;
            if (item != null) CurrentOrder = sql.ListInitialOrders.Find(i => i.Id == ((LINQOrder)item).Id); ;
        }
        #endregion

        #region Create
        /// <summary>
        /// Определение первого свободного Id
        /// </summary>
        /// <param name="item"></param>
        private void SetID(Users item)
        {
            item.Id = 1;
            List<Users> list = (sql.ListUsers.OrderBy(i => i.Id)).ToList();
            for (int i = 0; i < sql.ListUsers.Count; i++)
            {
                if (list[i].Id == item.Id) item.Id++;
                else break;
            }
        }
        /// <summary>
        /// Определение первого свободного Id
        /// </summary>
        /// <param name="item"></param>
        private void SetID(Products item)
        {
            item.Id = 1;
            List<Products> list = (sql.ListProducts.OrderBy(i => i.Id)).ToList();
            for (int i = 0; i < sql.ListProducts.Count; i++)
            {
                if (list[i].Id == item.Id) item.Id++;
                else break;
            }
        }
        /// <summary>
        /// Определение первого свободного Id
        /// </summary>
        /// <param name="item"></param>
        private void SetID(Orders item)
        {
            item.Id = 1;
            List<Orders> list = (sql.ListInitialOrders.OrderBy(i => i.Id)).ToList();
            for (int i = 0; i < sql.ListInitialOrders.Count; i++)
            {
                if (list[i].Id == item.Id) item.Id++;
                else break;
            }
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = new Users();
            NewUser wnd = new NewUser(ref CurrentUser);
            wnd.ShowDialog();
            if (CurrentUser.Email == null) return;
            SetID(CurrentUser);
            sql.CreateOrUpdate(CurrentUser);
            SelectUsers();
        }

        private void BtnAddData_Click(object sender, RoutedEventArgs e)
        {
            CurrentProduct = new Products();
            NewProduct wnd = new NewProduct(ref CurrentProduct);
            wnd.ShowDialog();
            if (CurrentProduct.Group == null) return;
            SetID(CurrentProduct);
            sql.CreateOrUpdate(CurrentProduct);
            SelectProducts();
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null)
            {
                MessageBox.Show("Пользователь не выбран");
                return;
            }
            CurrentOrder = new Orders();
            NewOrder wnd = new NewOrder(ref CurrentOrder, sql.ListProducts);
            wnd.ShowDialog();
            if (CurrentOrder.ProductId == 0) return;
            SetID(CurrentOrder);
            CurrentOrder.UserId = CurrentUser.Id;
            sql.CreateOrUpdate(CurrentOrder);
            SelectOrders();
        }
        #endregion

        #region Update
        private void BtnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null)
            {
                MessageBox.Show("Пользователь не выбран");
                return;
            }
            NewUser wnd = new NewUser(ref CurrentUser);
            wnd.ShowDialog();
            sql.CreateOrUpdate(CurrentUser);
            SelectUsers();
        }

        private void BtnUpdateData_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProduct == null)
            {
                MessageBox.Show("Продукт не выбран");
                return;
            }
            NewProduct wnd = new NewProduct(ref CurrentProduct);
            wnd.ShowDialog();
            sql.CreateOrUpdate(CurrentProduct);
            SelectProducts();
        }

        private void BtnUpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOrder == null)
            {
                MessageBox.Show("Заказ не выбран");
                return;
            }
            NewOrder wnd = new NewOrder(ref CurrentOrder, sql.ListProducts);
            wnd.ShowDialog();
            sql.CreateOrUpdate(CurrentOrder);
            SelectOrders();
        }
        #endregion

        #region Delete
        private void BtnRemoveData_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProduct == null) return;
            sql.Remove(CurrentProduct);
            SelectProducts();
        }

        private void BtnRemoveUser_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null) return;
            sql.Remove(CurrentUser);
            SelectUsers();
            
        }

        private void BtnRemoveOrder_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOrder == null) return;
            sql.Remove(CurrentOrder);
            SelectOrders();
        }
        #endregion
    }
}
