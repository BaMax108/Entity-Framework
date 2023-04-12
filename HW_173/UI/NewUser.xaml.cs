using System.Windows;

namespace HW_173
{
    /// <summary>
    /// Логика взаимодействия для NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        private Users User;

        public NewUser(ref Users user)
        {
            User = user;
            InitializeComponent();

            if (user.Email != null)
            {
                btnCreate.Content = "Обновить";
                tbxSecondName.Text = user.SecondName;
                tbxFirstName.Text = user.Name;
                tbxLastName.Text = user.LastName;
                tbxPhoneNumber.Text = user.PhoneNumber;
                tbxEmail.Text = user.Email;
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
            if (tbxLastName.Text.ToCharArray().Length > 30)
            {
                msg = "В поле Фамилия должно быть не более 30 символов.\n";
                isCorrect = false;
            }
            if (tbxFirstName.Text.ToCharArray().Length > 30)
            {
                msg += "В поле Имя должно быть не более 30 символов.\n";
                isCorrect = false;
            }
            if (tbxSecondName.Text.ToCharArray().Length > 30)
            {
                msg += "В поле Отчество должно быть не более 30 символов.\n";
                isCorrect = false;
            }
            if (tbxPhoneNumber.Text.ToCharArray().Length > 20)
            {
                msg += "В поле Номер телефона должно быть не более 20 символов.\n";
                isCorrect = false;
            }
            if (tbxEmail.Text.ToCharArray().Length > 30)
            {
                msg += "В поле Email должно быть не более 30 символов.\n";
                isCorrect = false;
            }
            if (tbxLastName.Text == "" && tbxFirstName.Text == "" &&
            tbxSecondName.Text == "" && tbxEmail.Text == "")
            {
                msg += "Все поля обязательны для заполнения, кроме номера телефона.";
                isCorrect = false;
            }
            if (!isCorrect) MessageBox.Show(msg);
            return isCorrect;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Troubleshouting()) return;
            User.LastName = tbxLastName.Text;
            User.Name = tbxFirstName.Text;
            User.SecondName = tbxSecondName.Text;
            User.PhoneNumber = tbxPhoneNumber.Text;
            User.Email = tbxEmail.Text;
            this.Close();
        }
    }
}
