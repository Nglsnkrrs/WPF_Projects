using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace TextFormatterDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FontSizeCombo.SelectedIndex = 1; // по умолчанию 12
            ColorCombo.SelectedIndex = 0;   // черный
            UpdateButtonsState();
        }

        // Проверка: есть ли выделение
        private bool HasSelection()
        {
            return !Editor.Selection.IsEmpty;
        }

        private void UpdateButtonsState()
        {
            bool active = HasSelection();
            BoldButton.IsEnabled = active;
            ItalicButton.IsEnabled = active;
            UnderlineButton.IsEnabled = active;
            ClearButton.IsEnabled = active;
            FontSizeCombo.IsEnabled = active;
            ColorCombo.IsEnabled = active;
        }

        private void Editor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateButtonsState();
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasSelection()) return;
            Editor.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasSelection()) return;
            Editor.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasSelection()) return;
            Editor.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasSelection()) return;
            Editor.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            Editor.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            Editor.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            Editor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            Editor.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, 12.0);
        }

        private void FontSizeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!HasSelection()) return;
            if (FontSizeCombo.SelectedItem is ComboBoxItem item && double.TryParse(item.Content.ToString(), out double size))
            {
                Editor.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, size);
            }
        }

        private void ColorCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!HasSelection()) return;
            if (ColorCombo.SelectedItem is ComboBoxItem item)
            {
                Brush color = Brushes.Black;
                switch (item.Content.ToString())
                {
                    case "Red": color = Brushes.Red; break;
                    case "Green": color = Brushes.Green; break;
                    case "Blue": color = Brushes.Blue; break;
                    case "Orange": color = Brushes.Orange; break;
                }
                Editor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            }
        }
    }
}
