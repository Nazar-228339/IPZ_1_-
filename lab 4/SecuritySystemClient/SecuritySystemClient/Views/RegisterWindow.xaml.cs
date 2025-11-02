using System;
using System.Windows;
using SecuritySystemClient.Services;

namespace SecuritySystemClient.Views
{
    public partial class RegisterWindow : Window
    {
        private ServerConnection _serverConnection;

        public RegisterWindow(ServerConnection serverConnection)
        {
            InitializeComponent();
            _serverConnection = serverConnection;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Обробник події
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валідація
                if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
                {
                    ShowError("Введіть логін");
                    return;
                }

                if (UsernameTextBox.Text.Length < 4)
                {
                    ShowError("Логін повинен містити мінімум 4 символи");
                    return;
                }

                if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                {
                    ShowError("Введіть пароль");
                    return;
                }

                if (PasswordBox.Password.Length < 6)
                {
                    ShowError("Пароль повинен містити мінімум 6 символів");
                    return;
                }

                if (PasswordBox.Password != ConfirmPasswordBox.Password)
                {
                    ShowError("Паролі не співпадають");
                    return;
                }

                // Перевірка підключення
                if (!_serverConnection.IsConnected)
                {
                    ShowError("Немає з'єднання з сервером");
                    return;
                }

                // Відправка запиту на сервер
                var response = await _serverConnection.RegisterAsync(
                    UsernameTextBox.Text,
                    PasswordBox.Password,
                    "Оператор"
                );

                if (response.Success)
                {
                    MessageBox.Show($"Користувач {UsernameTextBox.Text} успішно зареєстрований!",
                        "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    ShowError(response.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Помилка реєстрації: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}