using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Database;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Le_Chat
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            EnterEmail enterEmailWindow = new EnterEmail();
            enterEmailWindow.Show();

        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationForm();
            registrationWindow.Show();

            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Text;

            var user = Program.FindUser(login, password);

            if (user is not null)
            {
                MessageBox.Show("Вход успешен!");
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }
    }
}