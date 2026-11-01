using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace ShoeStore_Бихе
{
    
    public partial class MainMenuWindow : Window
    {
        private int _sortDirection = 0;
        private User _currentuser;
        public MainMenuWindow(User user)
        {
            InitializeComponent();
            _currentuser = user;
            var context = GetContext.model;
            ListTovar.ItemsSource = context.Tovar.ToList();
            CBPostav.SelectedValuePath = "Id";
            CBPostav.DisplayMemberPath = "Postavshik1";
            CBPostav.ItemsSource = context.Postavshik.ToList();


            if (_currentuser != null)
            {
                TBlockFio.Text = _currentuser.FIO;
                SPPanel.Visibility = Visibility.Visible;
                SPTX.Visibility = Visibility.Visible;
                if(user.RoleId != 1)
                {
                    BtnAddtovar.Visibility = Visibility.Hidden;
                }
            }
            else 
            {
                TBlockFio.Text = "Гость";
                SPPanel.Visibility = Visibility.Hidden;
                SPTX.Visibility = Visibility.Hidden;
                
            }
        }

        private void Filtr()
        {
            var SearchList = GetContext.model.Tovar.ToList();

            
            if (CBPostav.SelectedIndex > 0)
            {
                int postavId = Convert.ToInt32(CBPostav.SelectedValue);
                SearchList = SearchList.Where(x => x.PostavshikId == postavId).ToList();
            }

            if (!string.IsNullOrWhiteSpace(TbSearch.Text))
            {
                string q = TbSearch.Text.ToLower();

                SearchList = SearchList.Where(x =>
                    (x.TovarName != null && x.TovarName.ToLower().Contains(q)) ||
                    (x.OpisTovar != null && x.OpisTovar.ToLower().Contains(q)) ||
                    (x.Kategory != null && x.Kategory.Kategory1 != null && x.Kategory.Kategory1.ToLower().Contains(q)) ||
                    (x.Proizvod != null && x.Proizvod.Proizvod1 != null && x.Proizvod.Proizvod1.ToLower().Contains(q)) ||
                    (x.Postavshik != null && x.Postavshik.Postavshik1 != null && x.Postavshik.Postavshik1.ToLower().Contains(q)) ||
                    (x.EdinichIzmeren != null && x.EdinichIzmeren.ToLower().Contains(q))
                ).ToList();   
            }

            if (_sortDirection == 1)
                SearchList = SearchList.OrderBy(x => x.CountSklade).ToList();
            else if (_sortDirection == 2)
                SearchList = SearchList.OrderByDescending(x => x.CountSklade).ToList();

            ListTovar.ItemsSource = SearchList;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow avrhor = new AuthorizationWindow();
            avrhor.Show();
            this.Close();
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            _sortDirection = 1; Filtr();
        }

        private void BtnSbrosfiltr_Click(object sender, RoutedEventArgs e)
        {
            TbSearch.Text = "";
            CBPostav.SelectedIndex = 0;
            _sortDirection = 0;
            Filtr();
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            _sortDirection = 2; Filtr();
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtr();
        }

        private void CBPostav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) Filtr();
        }

        private void BtnAddtovar_Click(object sender, RoutedEventArgs e)
        {
            AddEditTovarWindow w = new AddEditTovarWindow(null);
            w.Show();
        }

        private void ListTovar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tovar = (sender as ListView)?.SelectedItem as Tovar;
            AddEditTovarWindow addedtiwWind = new AddEditTovarWindow(tovar);
            addedtiwWind.Show();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var pfdel = ListTovar.SelectedItems.Cast<Tovar>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить сл. элементы ({pfdel.Count()})", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (Tovar p in pfdel)
                    {
                        GetContext.model.Tovar.Remove(p);
                    }
                    GetContext.model.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    ListTovar.ItemsSource = GetContext.model.Tovar.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Btnzakaz_Click(object sender, RoutedEventArgs e)
        {
            ZakazWindow zakaz = new ZakazWindow(_currentuser);
            zakaz.Show();
        }
    }
}
