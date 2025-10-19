using System;
using System.Windows;

namespace SecuritySystemClient.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Просто обробник події
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
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

                // Успішна реєстрація
                MessageBox.Show($"Користувач {UsernameTextBox.Text} успішно зареєстрований!",
                    "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
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