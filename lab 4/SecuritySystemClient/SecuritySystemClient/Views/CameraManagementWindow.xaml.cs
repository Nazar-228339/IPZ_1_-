using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SecuritySystem.Models;
using SecuritySystemClient.Services;

namespace SecuritySystemClient.Views
{
    public partial class CameraManagementWindow : Window
    {
        private ObservableCollection<Camera> _cameras;
        private ServerConnection _serverConnection;

        public CameraManagementWindow(ObservableCollection<Camera> cameras, ServerConnection serverConnection)
        {
            InitializeComponent();
            _cameras = cameras;
            _serverConnection = serverConnection;
            CamerasDataGrid.ItemsSource = _cameras;
        }

        private void CamerasDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int newId = _cameras.Any() ? _cameras.Max(c => c.Id) + 1 : 1;
                string newName = $"Нова камера {newId}";

                var response = await _serverConnection.AddCameraAsync(newName, "Не вказано", false);

                if (response.Success)
                {
                    // Перезавантажуємо список з сервера
                    var camerasResponse = await _serverConnection.GetCamerasAsync();
                    if (camerasResponse.Success)
                    {
                        _cameras.Clear();
                        foreach (var cam in camerasResponse.Data)
                        {
                            _cameras.Add(new Camera
                            {
                                Id = cam.Id,
                                Name = cam.Name,
                                Location = cam.Location,
                                IsActive = cam.IsActive,
                                Status = cam.Status
                            });
                        }
                    }

                    MessageBox.Show("Камеру додано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CamerasDataGrid.SelectedItem is Camera selected)
                {
                    var result = MessageBox.Show($"Видалити камеру '{selected.Name}'?",
                        "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        var response = await _serverConnection.DeleteCameraAsync(selected.Id);
                        if (response.Success)
                        {
                            _cameras.Remove(selected);
                            MessageBox.Show("Камеру видалено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть камеру", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var camera in _cameras)
                {
                    await _serverConnection.UpdateCameraAsync(camera.Id, camera.Name, camera.Location, camera.IsActive);
                }

                MessageBox.Show("Збережено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}