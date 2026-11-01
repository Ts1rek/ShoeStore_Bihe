using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ShoeStore_Бихе
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void BtnVhod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = GetContext.model.User.FirstOrDefault(x => x.Login == TBLogin.Text && x.Password == PBPass.Password);
                if(userObj==null)
                {
                    MessageBox.Show("Такого пользователя нет!", "Ошибка при авторизации!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    AccountHelpClass.Id = userObj.Id;
                    AccountHelpClass.AuthUser = userObj;
                    MainMenuWindow mainmenu = new MainMenuWindow(userObj);
                    switch(userObj.RoleId)
                    {
                        case 1:MessageBox.Show("Здравствуйте администратор!" + userObj.FIO + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        case 2:MessageBox.Show("Здравствуйте Менеджер" + userObj.FIO + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        case 3:MessageBox.Show("Здравствуйте Авторизованный клиент" + userObj.FIO + "!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        default: MessageBox.Show("Данные не обнаруженны!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                            break;
                    }    
                    mainmenu.Show();
                    this.Close();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ошибка" + Ex.Message.ToString() + "Критическая работа приложения!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnVhodgost_Click(object sender, RoutedEventArgs e)
        {
            AccountHelpClass.Id = 0;
            AccountHelpClass.AuthUser = null;
            MainMenuWindow mainMenu = new MainMenuWindow(null);
            MessageBox.Show("Здравствуйте Гость!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            mainMenu.Show();
            this.Close();
        }
    }
}
