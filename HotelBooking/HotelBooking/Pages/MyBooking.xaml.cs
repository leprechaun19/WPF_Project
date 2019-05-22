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
using System.Text.RegularExpressions;

namespace HotelBooking.Pages
{
    /// <summary>
    /// Логика взаимодействия для MyBooking.xaml
    /// </summary>
    public partial class MyBooking : Page
    {

        public MyBooking()
        {
            try
            {
                InitializeComponent();
                DataContext = new Model.Autorization();
            }
            catch (Exception exp) { MessageBox.Show(exp.Message + " " + exp.InnerException); };
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BindingExpression name = Name.GetBindingExpression(TextBoxWatermarked.TextProperty);
                if (name.HasValidationError)
                {
                    throw new Exception("Исправьте неправильно введенные данные!");
                }
                string pattern = Name.Text;
                List<Model.SelectBooking> selectBooking = new List<Model.SelectBooking>();
                IEnumerable<Model.SelectBooking> select = Manipulation.db.SelectBookings().ToList<Model.SelectBooking>();

                foreach (Model.SelectBooking b in select)
                {
                    if (Regex.IsMatch(b.Full_name, pattern, RegexOptions.IgnoreCase))
                    {
                        selectBooking.Add(b);
                    }
                }

                if (selectBooking.Count == 0) { throw new Exception("На данный момент брони отсутствуют!"); }
                Booking.ItemsSource = selectBooking;
                Booking.Visibility = Visibility.Visible;
                Name.Clear();

            }
            catch (FormatException ex) { MessageBox.Show(ex.Message); }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex) { MessageBox.Show("Зполните все данные!"); }
            catch (Exception exp) { MessageBox.Show(exp.Message + " " + exp.InnerException); };
        }
    }
}
