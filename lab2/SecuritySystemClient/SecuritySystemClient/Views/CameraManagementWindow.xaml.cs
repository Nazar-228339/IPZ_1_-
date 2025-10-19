using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SecuritySystem.Models;

namespace SecuritySystem.Views
{
    public partial class CameraManagementWindow : Window
    {
        private ObservableCollection<Camera> _cameras;

        public CameraManagementWindow(ObservableCollection<Camera> cameras)
        {
            InitializeComponent();
            _cameras = cameras;
            CamerasDataGrid.ItemsSource = _cameras;
        }

        private void CamerasDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Автоматичне оновлення полів через Data Binding
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int newId = _cameras.Any() ? _cameras.Max(c => c.Id) + 1 : 1;

                Camera newCamera = new Camera
                {
                    Id = newId,
                    Name = $"Нова камера {newId}",
                    Location = "Не вказано",
                    IsActive = false,
                    Status = "Неактивна"
                };

                _cameras.Add(newCamera);
                CamerasDataGrid.SelectedItem = newCamera;

                MessageBox.Show("Камеру додано. Відредагуйте дані.",
                    "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка додавання камери: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CamerasDataGrid.SelectedItem is Camera selectedCamera)
                {
                    var result = MessageBox.Show(
                        $"Видалити камеру '{selectedCamera.Name}'?",
                        "Підтвердження",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        _cameras.Remove(selectedCamera);
                        MessageBox.Show("Камеру видалено", "Успіх",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть камеру для видалення",
                        "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка видалення камери: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Симуляція збереження в БД
                MessageBox.Show("Зміни збережено успішно!", "Успіх",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}