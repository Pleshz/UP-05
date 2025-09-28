using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP.Classes.Context;

namespace UP.Pages
{
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        public void LoginBtn(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, заполните логин и пароль.");
                return;
            }

            UserContext userContext = new UserContext();
            var allUsers = userContext.AllUsers();

            var user = allUsers.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.");
                return;
            }

            MainWindow.CurrentUser = (UserContext)user;
            MainWindow.init.OpenPages(MainWindow.pages.profile);
        }
    }
}
