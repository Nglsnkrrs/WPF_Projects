using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

namespace BeepApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("kernel32.dll")]
        static extern bool Beep(uint dwFreq, uint dwDuration);

        [DllImport("user32.dll")]
        static extern bool MessageBeep(uint uType);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!uint.TryParse(txtInterval.Text, out uint interval))
            {
                MessageBox.Show("Введите корректный интервал!");
                return;
            }

            if (!int.TryParse(txtCount.Text, out int count))
            {
                MessageBox.Show("Введите корректное количество сигналов!");
                return;
            }

            Thread thread = new Thread(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    Beep(800 + (uint)(i * 100), 300);
                    Thread.Sleep((int)interval);
                    MessageBeep(0xFFFFFFFF);
                    Thread.Sleep((int)interval);
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }
    }
}
