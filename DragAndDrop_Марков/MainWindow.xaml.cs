using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace DragAndDrop_Марков
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DispatcherTimer dispatcherTimer = new DispatcherTimer();;
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new System.TimeSpan(0, 0, 0, 0, 1000 / 60);
        }
        private void DispatcherTimer_Tick(object sender, System.EventArgs e)
        {
            Image.Margin = new Thickness(Mouse.GetPosition(this).X - 25, Mouse.GetPosition(this).Y - 25, 0, 0);
        }
        private void Image_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

    }
}