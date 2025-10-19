using SecuritySystem.Views;
using System;
using System.Windows;

namespace SecuritySystemClient.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Просто зберігаємо пароль
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валідація даних
                if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
                {
                    ShowError("Будь ласка, введіть логін");
                    return;
                }

                if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                {
                    ShowError("Будь ласка, введіть пароль");
                    return;
                }

                // Симуляція перевірки авторизації
                if (AuthenticateUser(UsernameTextBox.Text, PasswordBox.Password))
                {
                    // Створюємо користувача вручну
                    var user = new SecuritySystem.Models.User
                    {
                        Username = UsernameTextBox.Text,
                        IsAuthenticated = true,
                        Role = "Оператор"
                    };

                    // Відкриття головного вікна
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ShowError("Невірний логін або пароль");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Помилка авторизації: {ex.Message}");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RegisterWindow registerWindow = new RegisterWindow();
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
                ShowError($"Помилка відкриття вікна реєстрації: {ex.Message}");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            // Симуляція перевірки в базі даних
            return username == "admin" && password == "admin123" ||
                   username == "operator" && password == "oper123";
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}