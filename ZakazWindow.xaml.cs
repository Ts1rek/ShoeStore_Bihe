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
using System.Windows.Shapes;

namespace ShoeStore_Бихе
{
    /// <summary>
    /// Логика взаимодействия для ZakazWindow.xaml
    /// </summary>
    public partial class ZakazWindow : Window
    {
        private User _currentuser;
        public ZakazWindow(User user)
        {
            InitializeComponent();
            _currentuser = user;
            LoadOrders();
            ConfigureAccess();
        }

        private void ConfigureAccess()
        {
            if (_currentuser != null && _currentuser.RoleId != 1)
            {
                BtnAdd.Visibility = Visibility.Hidden;
                BtnEdit.Visibility = Visibility.Hidden;
                BtnDelete.Visibility = Visibility.Hidden;
            }
            InitializeComponent();
        }
        private void LoadOrders()
        {
            ListZakaz.ItemsSource = GetContext.model.Zakaz.ToList();
        }

        private void ListOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSelected();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditZakazWindow w = new AddEditZakazWindow(null, _currentuser);
            w.ShowDialog();
            LoadOrders();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelected();
        }

        private void EditSelected()
        {
            var zakaz = ListZakaz.SelectedItem as Zakaz;
            if (zakaz != null)
            {
                AddEditZakazWindow w = new AddEditZakazWindow(zakaz, _currentuser);
                w.ShowDialog();
                LoadOrders();
            }
            else
            {
                MessageBox.Show("Выберите заказ для редактирования.");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = ListZakaz.SelectedItems.Cast<Zakaz>().ToList();

            if (selected.Count == 0)
            {
                MessageBox.Show("Выберите заказ для удаления.");
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить сл. элементы ({selected.Count})",
                "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (Zakaz z in selected)
                    {
                        GetContext.model.Zakaz.Remove(z);
                    }
                    GetContext.model.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    LoadOrders();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
