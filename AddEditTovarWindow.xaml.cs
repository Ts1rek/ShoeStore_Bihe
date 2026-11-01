using Microsoft.Win32;
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
using System.IO;
using System.Diagnostics;

namespace ShoeStore_Бихе
{
    /// <summary>
    /// Логика взаимодействия для AddEditTovarWindow.xaml
    /// </summary>
    public partial class AddEditTovarWindow : Window
    {
        private Tovar _currentTovar;
        private string _photoPath = null;

        private static readonly string TargetDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
        public AddEditTovarWindow(Tovar tovar)
        {
            InitializeComponent();

            CBKatTov.SelectedValuePath = "Id";
            CBKatTov.DisplayMemberPath = "Kategory1";
            CBKatTov.ItemsSource = GetContext.model.Kategory.ToList();

            CBPostavshik.SelectedValuePath = "Id";
            CBPostavshik.DisplayMemberPath = "Postavshik1";
            CBPostavshik.ItemsSource = GetContext.model.Postavshik.ToList();

            CBProizvod.SelectedValuePath = "Id";
            CBProizvod.DisplayMemberPath = "Proizvod1";
            CBProizvod.ItemsSource = GetContext.model.Proizvod.ToList();

            if (tovar == null)
            {
                _currentTovar = new Tovar();
            }
            else
            {
                _currentTovar = tovar;

                TBNameTov.Text = _currentTovar.TovarName;
                TBOpisTov.Text = _currentTovar.OpisTovar;
                TBPrice.Text = _currentTovar.Price.ToString();
                TBEdIzm.Text = _currentTovar.EdinichIzmeren;
                TBColnasklade.Text = _currentTovar.CountSklade.ToString();
                TBSkidka.Text = _currentTovar.Skidka.ToString();

                CBKatTov.SelectedValue = _currentTovar.CategoryId;
                CBProizvod.SelectedValue = _currentTovar.ProizvodId;
                CBPostavshik.SelectedValue = _currentTovar.PostavshikId;

                if (!string.IsNullOrWhiteSpace(_currentTovar.Picture))
                {
                    _photoPath = _currentTovar.Picture;
                    string full = System.IO.Path.Combine(TargetDirectory, _photoPath);
                    if (File.Exists(full))
                    {
                        TBImage.Text = _photoPath;
                        ImgTov.Source = LoadImageSafe(full);
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentTovar.TovarName = TBNameTov.Text;
                _currentTovar.OpisTovar = TBOpisTov.Text;
                _currentTovar.Price = Convert.ToDecimal(TBPrice.Text);
                _currentTovar.EdinichIzmeren = TBEdIzm.Text;
                _currentTovar.CountSklade = Convert.ToInt32(TBColnasklade.Text);
                _currentTovar.Skidka = int.TryParse(TBSkidka.Text, out int sk) ? sk : 0;
                _currentTovar.CategoryId = (int)CBKatTov.SelectedValue;
                _currentTovar.ProizvodId = (int)CBProizvod.SelectedValue;
                _currentTovar.PostavshikId = (int)CBPostavshik.SelectedValue;
                _currentTovar.Picture = _photoPath;

                var ctx = GetContext.model;


                if (_currentTovar.Id == 0)
                {
                    ctx.Tovar.Add(_currentTovar);
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

        private void BtnFoto_Click(object sender, RoutedEventArgs e)
        {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".png";
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (dialog.ShowDialog() == true)
                try
                {
                    ImgTov.Source = null;

                    if (!string.IsNullOrWhiteSpace(_photoPath))
                    {
                        string oldFull = System.IO.Path.Combine(TargetDirectory, _photoPath);
                        if (File.Exists(oldFull))
                        {
                            File.Delete(oldFull); 
                        }
                    }

                    string newFileName = Guid.NewGuid().ToString()+ System.IO.Path.GetExtension(dialog.FileName);
                    string newFullPath = System.IO.Path.Combine(TargetDirectory, newFileName);

                    File.Copy(dialog.FileName, newFullPath, overwrite: true);

                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.UriSource = new Uri(newFullPath);
                    bmp.EndInit();
                    bmp.Freeze();
                    ImgTov.Source = bmp;

                    _photoPath = newFileName;
                    TBImage.Text = newFileName;

                    MessageBox.Show("Изображение товара успешно установлено!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка работы программы: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }
        private BitmapImage LoadImageSafe(string path)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.UriSource = new Uri(path);
            bmp.EndInit();
            bmp.Freeze();
            return bmp;
        }
    }
}
