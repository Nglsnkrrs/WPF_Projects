using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Game2048
{
    public partial class MainWindow : Window
    {
        private int[,] board = new int[4, 4];
        private Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            InitGrid();
            StartNewGame();
        }

        private void InitGrid()
        {
            GameGrid.Children.Clear();
            for (int i = 0; i < 16; i++)
            {
                Border cell = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(205, 193, 180)),
                    CornerRadius = new CornerRadius(5),
                    Margin = new Thickness(5),
                    Child = new TextBlock
                    {
                        Text = "",
                        FontSize = 28,
                        FontWeight = FontWeights.Bold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    }
                };
                GameGrid.Children.Add(cell);
            }
        }

        private void StartNewGame()
        {
            board = new int[4, 4];
            AddRandomTile();
            AddRandomTile();
            UpdateUI();
        }

        private void AddRandomTile()
        {
            var empty = (from r in Enumerable.Range(0, 4)
                         from c in Enumerable.Range(0, 4)
                         where board[r, c] == 0
                         select (r, c)).ToList();

            if (empty.Count == 0) return;

            var (row, col) = empty[rnd.Next(empty.Count)];
            board[row, col] = rnd.NextDouble() < 0.9 ? 2 : 4;
        }

        private void UpdateUI()
        {
            for (int i = 0; i < 16; i++)
            {
                int r = i / 4, c = i % 4;
                var border = (Border)GameGrid.Children[i];
                var tb = (TextBlock)border.Child;

                int value = board[r, c];
                tb.Text = value == 0 ? "" : value.ToString();
                border.Background = GetTileColor(value);
            }
        }
        private Brush GetTileColor(int v)
        {
            SolidColorBrush brush;
            switch (v)
            {
                case 0: brush = new SolidColorBrush(Color.FromRgb(205, 193, 180)); break;
                case 2: brush = new SolidColorBrush(Color.FromRgb(238, 228, 218)); break;
                case 4: brush = new SolidColorBrush(Color.FromRgb(237, 224, 200)); break;
                case 8: brush = new SolidColorBrush(Color.FromRgb(242, 177, 121)); break;
                case 16: brush = new SolidColorBrush(Color.FromRgb(245, 149, 99)); break;
                case 32: brush = new SolidColorBrush(Color.FromRgb(246, 124, 95)); break;
                case 64: brush = new SolidColorBrush(Color.FromRgb(246, 94, 59)); break;
                case 128: brush = new SolidColorBrush(Color.FromRgb(237, 207, 114)); break;
                case 256: brush = new SolidColorBrush(Color.FromRgb(237, 204, 97)); break;
                case 512: brush = new SolidColorBrush(Color.FromRgb(237, 200, 80)); break;
                case 1024: brush = new SolidColorBrush(Color.FromRgb(237, 197, 63)); break;
                case 2048: brush = new SolidColorBrush(Color.FromRgb(237, 194, 46)); break;
                default: brush = Brushes.Black; break;
            }
            return brush;
        }


        private bool MoveLeft()
        {
            bool moved = false;
            for (int r = 0; r < 4; r++)
            {
                int[] row = Enumerable.Range(0, 4).Select(c => board[r, c]).ToArray();
                int[] filtered = row.Where(x => x != 0).ToArray();

                var merged = new System.Collections.Generic.List<int>();
                int i = 0;
                while (i < filtered.Length)
                {
                    if (i < filtered.Length - 1 && filtered[i] == filtered[i + 1])
                    {
                        merged.Add(filtered[i] * 2);
                        i += 2;
                    }
                    else
                    {
                        merged.Add(filtered[i]);
                        i++;
                    }
                }

                while (merged.Count < 4) merged.Add(0);

                for (int c = 0; c < 4; c++)
                {
                    if (board[r, c] != merged[c]) moved = true;
                    board[r, c] = merged[c];
                }
            }
            return moved;
        }

        private bool MoveRight()
        {
            Rotate180();
            bool moved = MoveLeft();
            Rotate180();
            return moved;
        }

        private bool MoveUp()
        {
            RotateLeft();
            bool moved = MoveLeft();
            RotateRight();
            return moved;
        }

        private bool MoveDown()
        {
            RotateRight();
            bool moved = MoveLeft();
            RotateLeft();
            return moved;
        }

        private void RotateLeft()
        {
            int[,] newBoard = new int[4, 4];
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    newBoard[3 - c, r] = board[r, c];
            board = newBoard;
        }

        private void RotateRight()
        {
            int[,] newBoard = new int[4, 4];
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    newBoard[c, 3 - r] = board[r, c];
            board = newBoard;
        }

        private void Rotate180()
        {
            for (int r = 0; r < 2; r++)
                for (int c = 0; c < 4; c++)
                {
                    int tmp = board[r, c];
                    board[r, c] = board[3 - r, 3 - c];
                    board[3 - r, 3 - c] = tmp;
                }
        }

        private void TryMove(Func<bool> move)
        {
            bool moved = move();
            if (moved)
            {
                AddRandomTile();
                UpdateUI();
                if (!CanMove())
                    MessageBox.Show("Игра окончена!");
            }
        }

        private bool CanMove()
        {
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                {
                    if (board[r, c] == 0) return true;
                    if (c < 3 && board[r, c] == board[r, c + 1]) return true;
                    if (r < 3 && board[r, c] == board[r + 1, c]) return true;
                }
            return false;
        }

        // --- Управление ---
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left: TryMove(MoveLeft); break;
                case Key.Right: TryMove(MoveRight); break;
                case Key.Up: TryMove(MoveUp); break;
                case Key.Down: TryMove(MoveDown); break;
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e) => StartNewGame();
        private void Left_Click(object sender, RoutedEventArgs e) => TryMove(MoveLeft);
        private void Right_Click(object sender, RoutedEventArgs e) => TryMove(MoveRight);
        private void Up_Click(object sender, RoutedEventArgs e) => TryMove(MoveUp);
        private void Down_Click(object sender, RoutedEventArgs e) => TryMove(MoveDown);
    }
}
