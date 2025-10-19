using SecuritySystem.Models;
using SecuritySystemClient.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace SecuritySystem.Views
{
    public partial class MainWindow : Window
    {
        private User _currentUser;
        private ObservableCollection<Camera> _cameras;
        private ObservableCollection<Notification> _notifications;
        private DispatcherTimer _timer;

        public MainWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeData();
            InitializeTimer();
            UpdateUserInfo();
        }

        private void InitializeData()
        {
            try
            {
                // Ініціалізація камер
                _cameras = new ObservableCollection<Camera>
                {
                    new Camera { Id = 1, Name = "Камера 1", Location = "Головний вхід", IsActive = true, Status = "Активна" },
                    new Camera { Id = 2, Name = "Камера 2", Location = "Паркінг", IsActive = true, Status = "Активна" },
                    new Camera { Id = 3, Name = "Камера 3", Location = "Склад", IsActive = false, Status = "Неактивна" },
                    new Camera { Id = 4, Name = "Камера 4", Location = "Офіс А", IsActive = true, Status = "Активна" },
                    new Camera { Id = 5, Name = "Камера 5", Location = "Коридор 1", IsActive = true, Status = "Активна" }
                };

                CamerasDataGrid.ItemsSource = _cameras;

                // Ініціалізація сповіщень
                _notifications = new ObservableCollection<Notification>
                {
                    new Notification
                    {
                        Id = 1,
                        Title = "⚠️ Виявлено рух",
                        Message = "Камера 1 - Головний вхід",
                        Timestamp = DateTime.Now.AddMinutes(-5),
                        Severity = "Warning"
                    },
                    new Notification
                    {
                        Id = 2,
                        Title = "✅ Система активна",
                        Message = "Всі камери працюють нормально",
                        Timestamp = DateTime.Now.AddMinutes(-15),
                        Severity = "Info"
                    }
                };

                NotificationsListBox.ItemsSource = _notifications;
                UpdateStatus($"Завантажено {_cameras.Count} камер");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка ініціалізації даних: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTimeText.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        private void UpdateUserInfo()
        {
            UserInfoText.Text = $"Користувач: {_currentUser.Username} | Роль: {_currentUser.Role}";
        }

        private void UpdateStatus(string message)
        {
            StatusText.Text = message;
        }

        private void CamerasButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CameraManagementWindow cameraWindow = new CameraManagementWindow(_cameras);
                cameraWindow.Owner = this;
                cameraWindow.ShowDialog();
                UpdateStatus("Оновлено список камер");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відкриття вікна камер: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ArchiveWindow archiveWindow = new ArchiveWindow();
                archiveWindow.Owner = this;
                archiveWindow.ShowDialog();
                UpdateStatus("Переглянуто архів");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відкриття архіву: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NotificationDetailsWindow notifWindow = new NotificationDetailsWindow(_notifications);
                notifWindow.Owner = this;
                notifWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відкриття сповіщень: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SettingsWindow settingsWindow = new SettingsWindow();
                settingsWindow.Owner = this;
                settingsWindow.ShowDialog();
                UpdateStatus("Налаштування оновлено");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відкриття налаштувань: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Ви впевнені, що хочете вийти?",
                    "Підтвердження виходу", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _timer.Stop();
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка виходу: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CamerasDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (CamerasDataGrid.SelectedItem is Camera selectedCamera)
                {
                    CameraViewWindow viewWindow = new CameraViewWindow(selectedCamera);
                    viewWindow.Owner = this;
                    viewWindow.ShowDialog();
                    UpdateStatus($"Переглянуто камеру: {selectedCamera.Name}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відкриття перегляду камери: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearNotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Очистити всі сповіщення?",
                    "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _notifications.Clear();
                    UpdateStatus("Сповіщення очищено");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка очищення сповіщень: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}