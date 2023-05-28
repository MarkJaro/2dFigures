using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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


namespace WpfApp
{
    public partial class MainWindow : Window
    {
        Color black = Color.FromRgb(0, 0, 0);
        public MainWindow()
        {
            InitializeComponent();

            Screen screen = new Screen(800, 600);
            screen.Fill(black);

            FileReader reader = new FileReader();
            reader.ReadData(screen);

            // To Test PictureCircle
            //PictureCircle c = new PictureCircle(new Point2i(300, 300), 100, Color.FromRgb(255, 0, 0), Color.FromRgb(0, 255, 0));
            //c.Draw(screen);

            BitmapSource src = BitmapSource.Create(800, 600, 96, 96, PixelFormats.Rgb24, null, screen.Pixels, 800 * 3);
            Window.Source = src;
        }
    }
}
