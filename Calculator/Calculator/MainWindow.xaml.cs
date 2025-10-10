using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        private bool isNewNumber = true;
        private double firstNumber = 0;
        private string currentOperation = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Number(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string number = button.Content.ToString();

            if (isNewNumber || MainDisplay.Text == "0")
            {
                MainDisplay.Text = number;
                if (string.IsNullOrEmpty(currentOperation))
                {
                    HistoryDisplay.Text = number;
                }
                else
                {
                    HistoryDisplay.Text += number;
                }
                isNewNumber = false;
            }
            else
            {
                MainDisplay.Text += number;
                if (!string.IsNullOrEmpty(currentOperation))
                {
                    HistoryDisplay.Text += number;
                }
                else
                {
                    HistoryDisplay.Text = MainDisplay.Text;
                }
            }
        }

        private void Button_Click_Plus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MainDisplay.Text)) return;

            firstNumber = Convert.ToDouble(MainDisplay.Text);
            currentOperation = "+";
            isNewNumber = true;
            HistoryDisplay.Text += $" {currentOperation} ";
        }

        private void Button_Click_Minus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MainDisplay.Text)) return;

            firstNumber = Convert.ToDouble(MainDisplay.Text);
            currentOperation = "-";
            isNewNumber = true;
            HistoryDisplay.Text += $" {currentOperation} ";
        }

        private void Button_Click_Multiply(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MainDisplay.Text)) return;

            firstNumber = Convert.ToDouble(MainDisplay.Text);
            currentOperation = "*";
            isNewNumber = true;
            HistoryDisplay.Text += $" {currentOperation} ";
        }

        private void Button_Click_Divide(object sender, RoutedEventArgs e) 
        {
            if (string.IsNullOrEmpty(MainDisplay.Text)) return;

            firstNumber = Convert.ToDouble(MainDisplay.Text);
            currentOperation = "/";
            isNewNumber = true;
            HistoryDisplay.Text += $" {currentOperation} ";
        }

        private void Button_Click_Equally(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentOperation) || string.IsNullOrEmpty(MainDisplay.Text))
                return;

            double secondNumber = Convert.ToDouble(MainDisplay.Text);
            double result = 0;

            switch (currentOperation)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "-":
                    result = firstNumber - secondNumber;
                    break;
                case "*":
                    result = firstNumber * secondNumber;
                    break;
                case "/":
                    if (secondNumber == 0)
                    {
                        MessageBox.Show("Ошибка: деление на ноль!", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    result = firstNumber / secondNumber;
                    break;
            }

            MainDisplay.Text = FormatResult(result);
            HistoryDisplay.Text = MainDisplay.Text;

            currentOperation = "";
            isNewNumber = true;
        }

        private string FormatResult(double result)
        {
            return result % 1 == 0 ? result.ToString("0") : result.ToString("0.##########");
        }

        private void Button_Click_CE(object sender, RoutedEventArgs e)
        {
            MainDisplay.Text = "0";
            isNewNumber = true;

            if (!string.IsNullOrEmpty(currentOperation))
            {
                string history = HistoryDisplay.Text;
                int lastOperationIndex = history.LastIndexOf($" {currentOperation} ");

                if (lastOperationIndex >= 0)
                {
                    HistoryDisplay.Text = history.Substring(0, lastOperationIndex + 3);
                }
            }
            else
            {
                HistoryDisplay.Text = "";
            }
        }

        private void Button_Click_C(object sender, RoutedEventArgs e)
        {
            MainDisplay.Text = "0";
            HistoryDisplay.Text = "";
            isNewNumber = true;
            firstNumber = 0;
            currentOperation = "";
        }

        private void Button_Click_Clear_Last(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainDisplay.Text) && MainDisplay.Text != "0")
            {
                string currentText = MainDisplay.Text;

                if (currentText.Length > 1)
                {
                    MainDisplay.Text = currentText.Substring(0, currentText.Length - 1);
                }
                else
                {
                    MainDisplay.Text = "0";
                    isNewNumber = true;
                }

                if (!string.IsNullOrEmpty(HistoryDisplay.Text))
                {
                    string history = HistoryDisplay.Text.TrimEnd();

                    if (!history.EndsWith(" +") && !history.EndsWith(" -") &&
                        !history.EndsWith(" *") && !history.EndsWith(" /"))
                    {
                        if (history.Length > 1)
                            HistoryDisplay.Text = history.Substring(0, history.Length - 1);
                        else
                            HistoryDisplay.Text = "";
                    }
                }
            }
        }


        private void Button_Click_Dot(object sender, RoutedEventArgs e)
        {
            if (isNewNumber || MainDisplay.Text == "0")
            {
                MainDisplay.Text = "0,";
                HistoryDisplay.Text = "0,";
                isNewNumber = false;
            }
            else if (!MainDisplay.Text.Contains(","))
            {
                MainDisplay.Text += ",";
                if (!string.IsNullOrEmpty(currentOperation))
                {
                    HistoryDisplay.Text += ",";
                }
                else
                {
                    HistoryDisplay.Text = MainDisplay.Text;
                }
            }
        }
    }
}
