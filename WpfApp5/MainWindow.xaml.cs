using System.Diagnostics;
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
using Microsoft.AspNetCore.SignalR.Client;

namespace WpfApp5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection hubConnection;
        private List<TextBlock> selectedMessage = new List<TextBlock>();    

        public MainWindow()
        {
            InitializeComponent();
            ConnectToHubAsync();
        }

        private async void ConnectToHubAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5257/chat")
                .Build();

            hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AddMessageWithContextMenu($"{user}: {message}");
                });
            });

            try
            {
                await hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to hub: {ex.Message}");
            }
        }

        private void AddMessageWithContextMenu(string message)
        {
            TextBlock messageTextBlock = new TextBlock()
            {
                Text = message,
                Margin = new Thickness(0, 0, 0, 5),
                TextWrapping = TextWrapping.Wrap
            };

            messageTextBlock.MouseDown += MessageTextBlock_MouseDown;

            chatPanel.Children.Add(messageTextBlock);
        }

        private void MessageTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                foreach (TextBlock child in chatPanel.Children)
                {
                    child.Background = Brushes.Transparent;
                }

                textBlock.Background = Brushes.LightBlue;
            }
        }

        private async void ActionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;

            string selectedAction = selectedItem.Content.ToString();

            switch (selectedAction)
            {
                case "Очистить":
                    chatPanel.Children.Clear();
                    break;
                case "Редактировать":
                    EditSelectedMessage();
                    break;
                case "Удалить":
                    DeleteSelectedMessage();
                    break;
                default:
                    break;
            }
        }

        private async void EditSelectedMessage()
        {
            TextBlock selectedMessage = chatPanel.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Background == Brushes.LightBlue);
            if (selectedMessage != null)
            {
                Debug.WriteLine("Selected message: " + selectedMessage.Text);

                if (!string.IsNullOrEmpty(selectedMessage.Text))
                {
                    EditMessageDialog dialog = new EditMessageDialog(selectedMessage.Text, "DefaultUserName");
                    if (dialog.ShowDialog() == true)
                    {
                        selectedMessage.Text = dialog.NewMessage;
                    }
                }
                else
                {
                    MessageBox.Show("Please select a message to edit.");
                }
            }
            else
            {
                MessageBox.Show("Please select a message to edit.");
            }
        }

        private void DeleteSelectedMessage()
        {
            TextBlock selectedMessage = chatPanel.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Background == Brushes.LightBlue);
            if (selectedMessage != null)
            {
                chatPanel.Children.Remove(selectedMessage);
            }
        }

        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = messageTextBox.Text.Trim();

                if (!string.IsNullOrEmpty(message))
                {
                    await hubConnection.SendAsync("SendMessage", "User", message);
                    messageTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Please enter a message to send.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}");
            }
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            chatPanel.Children.Clear();
        }

        private void MessageTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Send_Click(sender, e);
            }
        }
    }
}