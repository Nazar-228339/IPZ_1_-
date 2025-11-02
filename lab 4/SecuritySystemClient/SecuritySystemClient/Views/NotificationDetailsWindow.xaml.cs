using System;
using System.Collections.ObjectModel;
using System.Windows;
using SecuritySystem.Models;

namespace SecuritySystem.Views
{
    public partial class NotificationDetailsWindow : Window
    {
        private ObservableCollection<Notification> _notifications;

        public NotificationDetailsWindow(ObservableCollection<Notification> notifications)
        {
            InitializeComponent();
            _notifications = notifications;
            NotificationsItemsControl.ItemsSource = _notifications;
            UpdateCount();
        }

        private void UpdateCount()
        {
            CountText.Text = $"({_notifications.Count})";
        }

        private void DeleteNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as System.Windows.Controls.Button;
                if (button?.Tag is Notification notification)
                {
                    _notifications.Remove(notification);
                    UpdateCount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка видалення сповіщення: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Видалити всі сповіщення?",
                    "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _notifications.Clear();
                    UpdateCount();
                    MessageBox.Show("Всі сповіщення видалено", "Успіх",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка очищення: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}