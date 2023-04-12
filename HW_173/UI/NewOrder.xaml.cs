using System.Collections.Generic;
using System.Windows;

namespace HW_173
{
    /// <summary>
    /// Логика взаимодействия для NewOrder.xaml
    /// </summary>
    public partial class NewOrder : Window
    {
        private Orders Data;

        public NewOrder(ref Orders data, List<Products> ProductsList)
        {
            Data = data;
            InitializeComponent();
            cbProducts.ItemsSource = ProductsList;
            if (data != null)
            {
                tbCount.Text = $"{data.ProductCount} шт.";
                sldrCount.Value = data.ProductCount;
                cbProducts.Text = $"";
                foreach (Products p in ProductsList)
                {
                    if (data.ProductId == p.Id)
                    {
                        cbProducts.Text = p.ToString();
                        break;
                    }
                }
            }
        }

        private void SldrCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbCount.Text = $"{sldrCount.Value.ToString()} шт.";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(cbProducts.SelectedValue != null &&
                sldrCount.Value != 0)
            {
                Products product = (Products)cbProducts.SelectedItem;
                Data.ProductId = product.Id;
                Data.ProductCount = (byte)sldrCount.Value;
            }
            this.Close();
        }
    }
}
