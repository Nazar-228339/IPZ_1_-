using System;
using System.Collections.Generic;
using System.Windows;

namespace SecuritySystemClient.Views
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
        private List<ArchiveRecord> _records;

        public ArchiveWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _records = new List<ArchiveRecord>
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
                }
            };

            ArchiveDataGrid.ItemsSource = _records;
            StatusText.Text = $"Знайдено: {_records.Count} записів";
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пошук працює!", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Відтворення...", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Завантаження...", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Видалено!", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}