using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColorButtonsDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (var child in ColorButtonsPanel.Children)
            {
                if (child is Button button)
                {
                    SetButtonColor(button);
                }
            }

            UpdateContainerWidth();
        }

        private void SetButtonColor(Button button)
        {
            string colorName = button.Content.ToString();

            try
            {
                var prop = typeof(Colors).GetProperty(colorName);
                if (prop != null)
                {
                    Color color = (Color)prop.GetValue(null);
                    button.Foreground = new SolidColorBrush(color);
                }
                else
                {
                    button.Foreground = Brushes.Black;
                }
            }
            catch
            {
                button.Foreground = Brushes.Black;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateContainerWidth();
        }

        private void UpdateContainerWidth()
        {
            ButtonsContainer.Width = this.ActualWidth * 2 / 3;
        }
    }
}
