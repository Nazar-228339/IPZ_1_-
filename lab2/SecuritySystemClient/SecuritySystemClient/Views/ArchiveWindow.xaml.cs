using System;
using System.Collections.Generic;
using System.Windows;

namespace SecuritySystem.Views
{
    public class ArchiveRecord
    {
        public int Id { get; set; }
        public string CameraName { get; set; }
        public DateTime Timestamp { get; set; }
        public string Duration { get; set; }
        public string EventType { get; set; }
    }

    public partial class ArchiveWindow : Window
    {
        private List<ArchiveRecord> _archiveRecords;

        public ArchiveWindow()
        {
            InitializeComponent();
            LoadArchiveData();
        }

        private void LoadArchiveData()
        {
            try
            {
                // Симуляція завантаження даних з БД
                _archiveRecords = new List<ArchiveRecord>
                {
                    new ArchiveRecord
                    {
                        Id = 1,
                        CameraName = "Камера 1 - Головний вхід",
                        Timestamp = DateTime.Now.AddHours(-2),
                        Duration = "00:15:30",
                        EventType = "Рух виявлено"
                    },
                    new ArchiveRecord
                    {
                        Id = 2,
                        CameraName = "Камера 2 - Паркінг",
                        Timestamp = DateTime.Now.AddHours(-5),
                        Duration = "00:45:12",
                        EventType = "Загроза"
                    },
                    new ArchiveRecord
                    {
                        Id = 3,
                        CameraName = "Камера 4 - Офіс А",
                        Timestamp = DateTime.Now.AddDays(-1),
                        Duration = "01:20:45",
                        EventType = "Плановий запис"
                    },
                    new ArchiveRecord
                    {
                        Id = 4,
                        CameraName = "Камера 1 - Головний вхід",
                        Timestamp = DateTime.Now.AddDays(-2),
                        Duration = "00:08:15",
                        EventType = "Рух виявлено"
                    }
                };

                ArchiveDataGrid.ItemsSource = _archiveRecords;
                StatusText.Text = $"Знайдено записів: {_archiveRecords.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження архіву: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime? startDate = StartDatePicker.SelectedDate;
                DateTime? endDate = EndDatePicker.SelectedDate;

                if (!startDate.HasValue || !endDate.HasValue)
                {
                    MessageBox.Show("Виберіть діапазон дат", "Увага",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (startDate > endDate)
                {
                    MessageBox.Show("Початкова дата не може бути пізніше кінцевої",
                        "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Фільтрація даних
                var filteredRecords = _archiveRecords.FindAll(r =>
                    r.Timestamp.Date >= startDate.Value.Date &&
                    r.Timestamp.Date <= endDate.Value.Date);

                ArchiveDataGrid.ItemsSource = filteredRecords;
                StatusText.Text = $"Знайдено записів: {filteredRecords.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка пошуку: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("Відтворення запису...", "Інформація",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка відтворення: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("Запис завантажується...", "Інформація",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Видалити цей запис?",
                    "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Запис видалено", "Успіх",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка видалення: {ex.Message}",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}