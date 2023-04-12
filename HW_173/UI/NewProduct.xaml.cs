using System.Windows;

namespace HW_173
{
    /// <summary>
    /// Логика взаимодействия для NewProduct.xaml
    /// </summary>
    public partial class NewProduct : Window
    {
        private Products Data;

        public NewProduct(ref Products data)
        {
            Data = data;
            InitializeComponent();
            
            if (data != null)
            {
                tbxProductGroup.Text = data.Group;
                tbxProductCode.Text = data.ProductCode;
                tbxProductName.Text = data.ProductName;
            }
        }

        /// <summary>
        /// Проверка пользовательского ввода
        /// </summary>
        /// <returns></returns>
        private bool Troubleshouting()
        {
            string msg = "";
            bool isCorrect = true;
            if (tbxProductGroup.Text.ToCharArray().Length > 5)
            {
                msg = "В поле Группа должно быть не более 5 символов.\n";
                isCorrect = false;
            }
            if (tbxProductCode.Text.ToCharArray().Length > 50)
            {
                msg += "В поле Код продукта должно быть не более 50 символов.\n";
                isCorrect = false;
            }
            if (tbxProductName.Text.ToCharArray().Length > 50)
            {
                msg += "В поле Имя продукта должно быть не более 150 символов.\n";
                isCorrect = false;
            }
            if (tbxProductGroup.Text == "" &&
            tbxProductCode.Text == "" &&
            tbxProductName.Text == "")
            {
                msg += "Все поля обязательны для заполнения.\n";
                isCorrect = false;
            }
            if (!isCorrect) MessageBox.Show(msg);
            return isCorrect;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!Troubleshouting()) return;
            Data.Group = tbxProductGroup.Text;
            Data.ProductCode = tbxProductCode.Text;
            Data.ProductName = tbxProductName.Text;
            this.Close();
        }
    }
}
