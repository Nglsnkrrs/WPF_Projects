using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace WindowController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        const uint WM_SETTEXT = 0x000C;
        const uint WM_CLOSE = 0x0010;  

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnExecute_Click(object sender, RoutedEventArgs e)
        {
            string targetTitle = txtTargetTitle.Text.Trim();

            if (string.IsNullOrEmpty(targetTitle))
            {
                MessageBox.Show("Введите заголовок окна для поиска!");
                return;
            }

            IntPtr hWnd = FindWindow(null, targetTitle);

            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Окно не найдено!");
                return;
            }

            if (rbChangeTitle.IsChecked == true)
            {
                string newTitle = txtNewTitle.Text.Trim();
                if (string.IsNullOrEmpty(newTitle))
                {
                    MessageBox.Show("Введите новый заголовок!");
                    return;
                }

                SendMessage(hWnd, WM_SETTEXT, IntPtr.Zero, newTitle);
                MessageBox.Show("Заголовок окна изменён!");
            }
            else if (rbCloseWindow.IsChecked == true)
            {
                SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                MessageBox.Show("Окно закрыто!");
            }
        }
    }
}
