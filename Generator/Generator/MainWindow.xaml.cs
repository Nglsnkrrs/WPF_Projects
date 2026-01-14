using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread primeThread;
        private Thread fiboThread;

        private bool stopPrimes = false;
        private bool stopFibo = false;

        private ManualResetEvent pausePrimes = new ManualResetEvent(true);
        private ManualResetEvent pauseFibo = new ManualResetEvent(true);


        private int start;
        private int? end;

        public MainWindow()
        {
            InitializeComponent();
        }

        // ======================== Запуск потоков ========================
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartThreads();
        }

        private void StartThreads()
        {
            // Очистка списка
            PrimeListBox.Items.Clear();
            FiboListBox.Items.Clear();

            stopPrimes = false;
            stopFibo = false;
            pausePrimes.Set();
            pauseFibo.Set();

            // Чтение границ
            start = string.IsNullOrWhiteSpace(FromTextBox.Text) ? 2 : int.Parse(FromTextBox.Text);
            end = string.IsNullOrWhiteSpace(ToTextBox.Text) ? (int?)null : int.Parse(ToTextBox.Text);

            // Создание потоков
            primeThread = new Thread(GeneratePrimes) { IsBackground = true };
            fiboThread = new Thread(GenerateFibonacci) { IsBackground = true };

            primeThread.Start();
            fiboThread.Start();
        }

        // ======================== Рестарт ========================
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            // Полная остановка текущих потоков
            stopPrimes = true;
            stopFibo = true;

            pausePrimes.Set(); // разблокировать если паузa
            pauseFibo.Set();

            // Ждём завершения потоков
            primeThread?.Join();
            fiboThread?.Join();

            // Запуск с новыми границами
            StartThreads();
        }

        // ======================== Потоки ========================
        private void GeneratePrimes()
        {
            int number = start;

            while (!stopPrimes && (!end.HasValue || number <= end.Value))
            {
                pausePrimes.WaitOne();

                if (IsPrime(number))
                {
                    Dispatcher.Invoke(() => PrimeListBox.Items.Add(number));
                    Thread.Sleep(200);
                }
                number++;
            }
        }

        private void GenerateFibonacci()
        {
            long a = 0, b = 1;

            while (!stopFibo)
            {
                pauseFibo.WaitOne();

                Dispatcher.Invoke(() => FiboListBox.Items.Add(a));

                long next = a + b;
                a = b;
                b = next;

                Thread.Sleep(300);
            }
        }

        private bool IsPrime(int n)
        {
            if (n < 2) return false;
            for (int i = 2; i * i <= n; i++)
                if (n % i == 0) return false;
            return true;
        }

        // ======================== Управление потоками ========================
        // Простые числа
        private void PausePrimesButton_Click(object sender, RoutedEventArgs e) => pausePrimes.Reset();
        private void ResumePrimesButton_Click(object sender, RoutedEventArgs e) => pausePrimes.Set();
        private void StopPrimesButton_Click(object sender, RoutedEventArgs e) => stopPrimes = true;

        // Фибоначчи
        private void PauseFiboButton_Click(object sender, RoutedEventArgs e) => pauseFibo.Reset();
        private void ResumeFiboButton_Click(object sender, RoutedEventArgs e) => pauseFibo.Set();
        private void StopFiboButton_Click(object sender, RoutedEventArgs e) => stopFibo = true;

        protected override void OnClosed(EventArgs e)
        {
            stopPrimes = true;
            stopFibo = true;
            pausePrimes.Set();
            pauseFibo.Set();
            base.OnClosed(e);
        }
    }
}
