using System;
using System.Windows;
using SecuritySystemClient.Services;

namespace SecuritySystemClient.Views
{
    public partial class LoginWindow : Window
    {
        private ServerConnection _serverConnection;

        public LoginWindow()
        {
            InitializeComponent();
            _serverConnection = new ServerConnection();
            ConnectToServer();
        }

        private async void ConnectToServer()
        {
            try
            {
                bool connected = await _serverConnection.ConnectAsync();
                if (connected)
                {
                    Title = "Авторизація - Підключено ✓";
                }
                else
                {
                    Title = "Авторизація - Немає з'єднання ✗";
                    ShowError("Не вдалося підключитися до сервера");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Помилка підключення: {ex.Message}");
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
                {
                    ShowError("Введіть логін");
                    return;
                }

                if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                {
                    ShowError("Введіть пароль");
                    return;
                }

                if (!_serverConnection.IsConnected)
                {
                    ShowError("Немає з'єднання з сервером");
                    return;
                }

                var response = await _serverConnection.LoginAsync(UsernameTextBox.Text, PasswordBox.Password);

                if (response.Success)
                {
                    var user = new SecuritySystem.Models.User
                    {
                        Username = UsernameTextBox.Text,
                        IsAuthenticated = true,
                        Role = "Оператор"
                    };

                    MainWindow mainWindow = new MainWindow(user, _serverConnection);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ShowError(response.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Помилка: {ex.Message}");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegisterWindow registerWindow = new RegisterWindow(_serverConnection);
                registerWindow.Owner = this;
                bool? result = registerWindow.ShowDialog();

                if (result == true)
                {
                    MessageBox.Show("Реєстрація успішна! Тепер ви можете увійти.",
                        "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Помилка відкриття реєстрації: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        protected override void OnClosed(EventArgs e)
        {
            // НЕ відключаємось - передаємо з'єднання в MainWindow
            // _serverConnection?.Disconnect();
            base.OnClosed(e);
        }
    }
}