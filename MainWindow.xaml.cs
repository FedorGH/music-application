using NAudio.Wave;
using System;
using MaterialDesignThemes.Wpf;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using MahApps.Metro.IconPacks;

namespace MusicApplication
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += timer_Tick;

            timer.Start();
        }

        private void OnPreviousClick(object sender, RoutedEventArgs e) { }
        private void OnPlayPauseClick(object sender, RoutedEventArgs e) { }
        private void OnNextClick(object sender, RoutedEventArgs e) { }

        //private void gif_MediaEnded(object sender, RoutedEventArgs e)
        //{
        //    gif.Position = new TimeSpan(0, 0, 1);
        //    gif.Play();
        //}
        //private void gif_MediaOpened(object sender, RoutedEventArgs e)
        //{
        //    if (gif.Stretch != Stretch.Uniform)
        //    {
        //        gif.Stretch = Stretch.Uniform;
        //    }
        //    else
        //    {
        //        gif.Stretch = Stretch.Uniform;
        //    }
        //}

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Button_CloseForm(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
                Close();
        }

        private void Button_FoldForm(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                WindowState = WindowState.Minimized;
        }
        private void Button_MaxForm(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }






















































        private bool isDragging = false; // Флаг для отслеживания перетаскивания слайдера
        private void AddFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Source = new Uri(openFileDialog.FileName);
                foreach (var file in openFileDialog.FileNames)
                {
                    Playlist.Items.Add(file);
                }
            }
        }

        // Очистка плейлиста
        private void ClearPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            Playlist.Items.Clear();
            mediaPlayer.Source = null;
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

        // Пауза трека
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


        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan) && (!isDragging))
            {
                ProgressSlider.Minimum = 0;
                ProgressSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressSlider.Value = mediaPlayer.Position.TotalSeconds;
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
    }
}
