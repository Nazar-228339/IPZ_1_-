using System;
using System.Windows;
using System.Windows.Threading;
using SecuritySystem.Models;

namespace SecuritySystem.Views
{
    public partial class CameraViewWindow : Window
    {
        private Camera _camera;
        private DispatcherTimer _timer;
        private bool _isRecording = false;

        public CameraViewWindow(Camera camera)
        {
            InitializeComponent();
            _camera = camera;
            DataContext = _camera;
            InitializeCamera();
            InitializeTimer();
        }

        private void InitializeCamera()
        {
            try
            {
                CameraNameText.Text = _camera.Name;
                CameraLocationText.Text = $"Розташування: {_camera.Location}";
                StreamStatusText.Text = _camera.IsActive ? "🟢 Активний потік" : "🔴 Потік недоступний";
                ResolutionText.Text = "1920x1080 @ 25 FPS";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка ініціалізації камери: {ex.Message}",
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
            TimeText.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _camera.IsActive = true;
                StreamStatusText.Text = "🟢 Активний потік";
                MessageBox.Show("Відеопотік запущено", "Інформація",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка запуску потоку: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamStatusText.Text = "⏸️ Потік призупинено";
                MessageBox.Show("Відеопотік призупинено", "Інформація",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка паузи: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _camera.IsActive = false;
                StreamStatusText.Text = "🔴 Потік зупинено";
                MessageBox.Show("Відеопотік зупинено", "Інформація",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка зупинки потоку: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filename = $"snapshot_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                MessageBox.Show($"Знімок збережено: {filename}", "Успіх",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка створення знімку: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _isRecording = !_isRecording;

                if (_isRecording)
                {
                    MessageBox.Show("Запис розпочато", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string filename = $"recording_{DateTime.Now:yyyyMMdd_HHmmss}.mp4";
                    MessageBox.Show($"Запис зупинено та збережено: {filename}", "Успіх",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка запису: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _timer.Stop();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка закриття вікна: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _timer?.Stop();
            base.OnClosed(e);
        }
    }
}