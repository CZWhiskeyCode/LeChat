using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Mail;
using System.Net;
using Database;

namespace Le_Chat
{
    public partial class EnterEmail : Window
    {
        public EnterEmail()
        {
            InitializeComponent();
        }

        private void SendEmailButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string emailAddress = txtEmail.Text;

                User user = Program.FindUserByEmail(emailAddress);
                if (user is not null)
                {
                    MailAddress from = new MailAddress("zemanarthur@gmail.com", "Artur");
                    MailAddress to = new MailAddress("dimapyatnichenko09@gmail.com");
                    MailMessage m = new MailMessage(from, to);
                    m.Subject = "Your Password Recovery";
                    m.Body = $"<h2>Your password: {user.Password}</h2>";
                    m.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new NetworkCredential("zemanarthur@gmail.com", "fuya byvi xyiq canu");
                    smtp.EnableSsl = true;

                    smtp.Send(m);

                    MessageBox.Show("Password sent to your email!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("User not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
