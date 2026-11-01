using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для AddEditZakazWindow.xaml
    /// </summary>
    public partial class AddEditZakazWindow : Window
    {
        private Zakaz _currentZakaz;
        private User _currentuser;

        public AddEditZakazWindow(Zakaz zakaz, User user)
        {
            InitializeComponent();
            _currentuser = user;

            CBStatus.SelectedValuePath = "Id";
            CBStatus.DisplayMemberPath = "Status1";
            CBStatus.ItemsSource = GetContext.model.Status.ToList();

            CBAddress.SelectedValuePath = "Id";
            CBAddress.DisplayMemberPath = "Adress_Pynkta_vidachi1";
            CBAddress.ItemsSource = GetContext.model.Adress_Pynkta_vidachi.ToList();

            if (zakaz == null)
            {
                _currentZakaz = new Zakaz();
                TBOrderDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
                TBDeliveryDate.Text = DateTime.Now.AddDays(3).ToString("dd.MM.yyyy");

                if (_currentuser != null)
                    _currentZakaz.UserId = _currentuser.Id;
            }
            else
            {
                _currentZakaz = zakaz;

                TBCode.Text = _currentZakaz.Code;
                TBOrderDate.Text = _currentZakaz.Date_zakaz;
                TBDeliveryDate.Text = _currentZakaz.Date_dostavca;
                if (_currentZakaz.Sostav_zakaza != null)
                {
                    TBSostav.Text = _currentZakaz.Sostav_zakaza.ArticylZakaza;
                }

                CBAddress.SelectedItem = GetContext.model.Adress_Pynkta_vidachi.ToList().FirstOrDefault(a => a.Id == _currentZakaz.Adress_pyncta_vidachiId);
                
                    CBStatus.SelectedItem = GetContext.model.Status.ToList().FirstOrDefault(s => s.Id == _currentZakaz.StatusId);
            }

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TBCode.Text))
                {
                    MessageBox.Show("Введите артикул заказа!");
                    return;
                }

                var selectedAddress = CBAddress.SelectedItem as Adress_Pynkta_vidachi;
                var selectedStatus = CBStatus.SelectedItem as Status;

                if (selectedAddress == null)
                {
                    MessageBox.Show("Выберите адрес пункта выдачи!");
                    return;
                }
                if (selectedStatus == null)
                {
                    MessageBox.Show("Выберите статус заказа!");
                    return;
                }

                _currentZakaz.Code = TBCode.Text;
                _currentZakaz.Adress_pyncta_vidachiId = selectedAddress.Id;
                _currentZakaz.StatusId = selectedStatus.Id;
                _currentZakaz.Date_zakaz = TBOrderDate.Text;
                _currentZakaz.Date_dostavca = TBDeliveryDate.Text;

                var ctx = GetContext.model;

                if (_currentZakaz.Id == 0) 
                {

                    if (_currentZakaz.UserId == 0)
                    {
                        if (_currentuser != null)
                            _currentZakaz.UserId = _currentuser.Id;
                        else
                        {
                            MessageBox.Show("Невозможно создать заказ: пользователь не авторизован!");
                            return;
                        }
                    }

                    var newSostav = new Sostav_zakaza
                    {
                        Nimer_zakaza = TBCode.Text,           
                        ArticylZakaza = TBSostav.Text         
                    };
                    ctx.Sostav_zakaza.Add(newSostav);
                    ctx.SaveChanges();

                    _currentZakaz.Sostav_zakazaId = newSostav.Id;

                    ctx.Zakaz.Add(_currentZakaz);
                }
                else
                {

                    if (_currentZakaz.Sostav_zakaza != null)
                    {
                        _currentZakaz.Sostav_zakaza.ArticylZakaza = TBSostav.Text;
                    }
                }

                ctx.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка сохранения",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}