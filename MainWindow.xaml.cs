using NAudio.Wave;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using MahApps.Metro.IconPacks;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Diagnostics;

namespace MusicApplication
{
    public partial class MainWindow : Window
    {
        private bool isDragging = false; // Флаг для отслеживания перетаскивания слайдера
        string fileName; // хранение названия аудио файла

        public MainWindow()
        {
            InitializeComponent();

            // запуск таймера для проигрывания аудио
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            // начальные настройки приложения
            MainGrid.Visibility = Visibility.Visible;
            PlaylistGrid.Visibility = Visibility.Hidden;
        }

        // Gif-анимация
        private void gif_MediaEnded(object sender, RoutedEventArgs e)
        {
            gif.Position = new TimeSpan(0, 0, 1);
            gif.Play();
        }
        private void gif_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (gif.Stretch != Stretch.Uniform)
            {
                gif.Stretch = Stretch.Uniform;
            }
            else
            {
                gif.Stretch = Stretch.Uniform;
            }
        }
        

        // Перетаскивание формы
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


        
        /// <summary>
        /// Панель управления окном
        /// </summary>

        // Закрыть форму (реализовано для панели управления окном)
        private void Button_CloseForm(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
                Close();
        }

        // Скрыть форму (реализовано для панели управления окном)
        private void Button_FoldForm(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                WindowState = WindowState.Minimized;
        }

        // Раскрыть форму во весь экран (реализовано для панели управления окном)
        private void Button_MaxForm(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }



        /// <summary>
        /// Реализация функционала проигрывателя
        /// </summary>

        // Добавление файлов в listbox

        private void AddFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(file);

                    if (fileName.Length > 40)
                        fileName = fileName.Substring(0, 40) + "...";

                    var listBoxItem = new ListBoxItem
                    {
                        Content = fileName,
                        Tag = file // Полный путь сохраняется в Tag
                    };

                    Playlist.Items.Add(listBoxItem);

                    // Если список был пуст, выбираем и начинаем воспроизведение первого трека
                    if (Playlist.Items.Count == 1)
                    {
                        Playlist.SelectedIndex = 0;
                        PlaySelectedTrack();
                    }
                }
            }
        }

        private void PlaySelectedTrack()
        {
            if (Playlist.SelectedItem is ListBoxItem selectedItem && selectedItem.Tag is string filePath)
            {
                mediaPlayer.Source = new Uri(filePath);

                // Обновляем отображение текущего файла
                FileNameTextBlock.Text = selectedItem.Content.ToString();
            }
        }


        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (Playlist.Items.Count > 0) // Убедимся, что плейлист не пуст
            {
                Playlist.SelectedIndex = (Playlist.SelectedIndex + 1) % Playlist.Items.Count;
                PlaySelectedTrack();
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (Playlist.Items.Count > 0) // Убедимся, что плейлист не пуст
            {
                Playlist.SelectedIndex = (Playlist.SelectedIndex - 1 + Playlist.Items.Count) % Playlist.Items.Count;
                PlaySelectedTrack();
            }
        }



        // Очистка плейлиста
        private void ClearPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            Playlist.Items.Clear();
            mediaPlayer.Source = null;
        }

        // Прогресс проигрывания трека
        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Progress.Text = TimeSpan.FromSeconds(ProgressSlider.Value).ToString(@"mm\:ss");
        }

        //Воспроизведение трека
        //private void PlayButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(selectedTrackPath))
        //    {
        //        if (mediaPlayer.Source == null || mediaPlayer.Source.ToString() != new Uri(selectedTrackPath).ToString())
        //        {
        //            ProgressSlider.Value = 0;
        //            mediaPlayer.Source = new Uri(selectedTrackPath);
        //            mediaPlayer.Play();
        //        }
        //        else
        //        {
        //            if (mediaPlayer.Position == TimeSpan.Zero || mediaPlayer.Clock == null)
        //            {
        //                mediaPlayer.Play(); // Начинаем воспроизведение, если ранее не воспроизводился
        //            }
        //        }
        //        StartProgressTimer();
        //    }
        //}


        // Пауза и пуск трека
        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var icon = PlayPauseIcon; // Ensure this name matches the XAML name

            if (button?.Tag?.ToString() == "Play")
            {
                mediaPlayer.Play();
                icon.Kind = PackIconMaterialKind.Pause; // Updated usage
                button.Tag = "Pause";
            }
            else if (button?.Tag?.ToString() == "Pause")
            {
                mediaPlayer.Pause();
                icon.Kind = PackIconMaterialKind.Play; // Updated usage
                button.Tag = "Play";
            }
        }

        // Обновление прогресс-бара
        private void Timer_Tick(object sender, EventArgs e)
        {
            if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan) && (!isDragging))
            {
                ProgressSlider.Minimum = 0;
                ProgressSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressSlider.Value = mediaPlayer.Position.TotalSeconds;
                Remains.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            }
        }

        // Начало перетаскивания слайдера
        private void ProgressSlider_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isDragging = true;
        }

        // Завершение перетаскивания слайдера
        private void ProgressSlider_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isDragging = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
        }

        // Изменение громкости
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Volume = VolumeSlider.Value; // Устанавливаем громкость от 0 до 1
            }
        }


        /// <summary>
        /// Управление окнами меню
        /// </summary>
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MainGrid.Visibility == Visibility.Hidden)
            {
                PlaylistGrid.Visibility = Visibility.Hidden;
                MainGrid.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (PlaylistGrid.Visibility == Visibility.Hidden)
            {
                MainGrid.Visibility = Visibility.Hidden;
                PlaylistGrid.Visibility = Visibility.Visible;
            }
        }

    }
}
