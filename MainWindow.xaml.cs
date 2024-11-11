using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnPreviousClick(object sender, RoutedEventArgs e) { }
        private void OnPlayPauseClick(object sender, RoutedEventArgs e) { }
        private void OnNextClick(object sender, RoutedEventArgs e) { }

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

    }
}
