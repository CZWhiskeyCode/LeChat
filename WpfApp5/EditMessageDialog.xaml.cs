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

namespace WpfApp5
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EditMessageDialog : Window
    {
        public string NewMessage { get; private set; }

        public EditMessageDialog(string initialMessage,string userName)
        {
            InitializeComponent();
            messageTextBox.Text = initialMessage;
            userNameTextBox.Text = userName;
            userNameTextBox.IsReadOnly = true;
        
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(messageTextBox.Text.Trim()))
            {
                NewMessage = messageTextBox.Text;
                DialogResult = true; // Закрываем окно и возвращаем результат диалога
            }
            else
            {
                MessageBox.Show("Message cannot be empty. Please enter a message.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Закрываем окно без сохранения изменений
        }
    }
}
