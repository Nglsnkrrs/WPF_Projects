using System.Windows;

namespace AuthApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            // Пример простой проверки (в реальности — сверка с БД)
            if (username == "admin" && password == "1234")
            {
                MessageBox.Show("Добро пожаловать, администратор!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true; // можно закрыть окно
            }
            else
            {
                ErrorText.Text = "Неверное имя пользователя или пароль.";
            }
        }
    }
}
