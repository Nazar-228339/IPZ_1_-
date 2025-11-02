using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using SecuritySystem.Models;
using SecuritySystemClient.Services;

namespace SecuritySystemClient.Views
{
    public partial class MainWindow : Window
    {
        private User _currentUser;
        private ServerConnection _serverConnection;
        private ObservableCollection<Camera> _cameras;
        private ObservableCollection<Notification> _notifications;
        private DispatcherTimer _timer;

        public MainWindow(User user, ServerConnection serverConnection)
        {
            InitializeComponent();
            _currentUser = user;
            _serverConnection = serverConnection;

            _cameras = new ObservableCollection<Camera>();
            _notifications = new ObservableCollection<Notification>();

            InitializeData();
            InitializeTimer();
            UpdateUserInfo();
        }

        private async void InitializeData()
        {
            try
            {
                var camerasResponse = await _serverConnection.GetCamerasAsync();
                if (camerasResponse.Success)
                {
                    _cameras.Clear();
                    foreach (var cameraData in camerasResponse.Data)
                    {
                        _cameras.Add(new Camera
                        {
                            Id = cameraData.Id,
                            Name = cameraData.Name,
                            Location = cameraData.Location,
                            IsActive = cameraData.IsActive,
                            Status = cameraData.Status
                        });
                    }
                    CamerasDataGrid.ItemsSource = _cameras;
                }

                var notificationsResponse = await _serverConnection.GetNotificationsAsync();
                if (notificationsResponse.Success)
                {
                    _notifications.Clear();
                    foreach (var notifData in notificationsResponse.Data)
                    {
                        _notifications.Add(new Notification
                        {
                            Id = notifData.Id,
                            Title = notifData.Title,
                            Message = notifData.Message,
                            Timestamp = notifData.Timestamp,
                            CameraId = notifData.CameraId,
                            Severity = notifData.Severity
                        });
                    }
                    NotificationsListBox.ItemsSource = _notifications;
                }

                UpdateStatus($"Завантажено {_cameras.Count} камер з сервера");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                CameraManagementWindow cameraWindow = new CameraManagementWindow(_cameras, _serverConnection);
                cameraWindow.Owner = this;
                cameraWindow.ShowDialog();

                // Оновлення списку після закриття
                InitializeData();
                UpdateStatus("Оновлено список камер");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відкриття вікна камер: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show($"Помилка відкриття архіву: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Просте вікно зі списком сповіщень
                var message = "Сповіщення:\n\n";
                foreach (var notif in _notifications)
                {
                    message += $"• {notif.Title}\n  {notif.Message}\n  {notif.TimeString}\n\n";
                }

                if (_notifications.Count == 0)
                {
                    message = "Немає сповіщень";
                }

                MessageBox.Show(message, "Сповіщення", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("Налаштування:\n\n• Сервер: 127.0.0.1:5000\n• Статус: Підключено\n• Користувач: " + _currentUser.Username,
                    "Налаштування", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateStatus("Переглянуто налаштування");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Ви впевнені, що хочете вийти?", "Підтвердження виходу",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _timer.Stop();
                    _serverConnection.Disconnect();

                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка виходу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CamerasDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (CamerasDataGrid.SelectedItem is Camera selectedCamera)
                {
                    MessageBox.Show($"Камера: {selectedCamera.Name}\nРозташування: {selectedCamera.Location}\nСтатус: {selectedCamera.StatusText}",
                        "Інформація про камеру", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateStatus($"Переглянуто камеру: {selectedCamera.Name}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearNotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Очистити всі сповіщення?", "Підтвердження",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _notifications.Clear();
                    UpdateStatus("Сповіщення очищено");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _timer?.Stop();
            _serverConnection?.Disconnect();
            base.OnClosed(e);
        }
    }
}