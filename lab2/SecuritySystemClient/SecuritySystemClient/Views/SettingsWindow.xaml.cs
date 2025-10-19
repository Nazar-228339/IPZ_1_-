using System;
using System.Windows;

namespace SecuritySystemClient.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валідація email
                string email = EmailTextBox.Text;
                if (!string.IsNullOrWhiteSpace(email) && !IsValidEmail(email))
                {
                    MessageBox.Show("Невірний формат email адреси", "Помилка валідації",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Валідація днів зберігання
                if (!int.TryParse(RetentionDaysTextBox.Text, out int days) || days < 1 || days > 365)
                {
                    MessageBox.Show("Тривалість зберігання повинна бути від 1 до 365 днів",
                        "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Симуляція збереження налаштувань
                MessageBox.Show("Налаштування успішно збережено!", "Успіх",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження налаштувань: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Скинути всі налаштування до стандартних значень?",
                    "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Скидання до стандартних значень
                    SensitivitySlider.Value = 70;
                    EmailTextBox.Text = "security@example.com";
                    QualityComboBox.SelectedIndex = 1;
                    RetentionDaysTextBox.Text = "30";

                    MessageBox.Show("Налаштування скинуто до стандартних", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка скидання налаштувань: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}