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

namespace HotelBooking.Pages
{
    /// <summary>
    /// Логика взаимодействия для LastFormForClient.xaml
    /// </summary>
    public partial class LastFormForClient : Window
    {
        Model.Autorization autorization;

        public LastFormForClient(Model.Autorization auto)
        {
            try
            {
                autorization = auto;
                InitializeComponent();
                DataContext = new Model.Client();
            }
            catch (Exception exp) { MessageBox.Show(exp.Message); }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BindingExpression b = Phone.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression f = Full_Name.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression p = Email.GetBindingExpression(TextBoxWatermarked.TextProperty);
                if (b.HasValidationError || p.HasValidationError|| f.HasValidationError)
                {
                    throw new Exception("Исправьте неправильно введенные данные!");
                }
                Random rand = new Random();
                int code_client = Randomizer(rand, 1000, 100000);
                Model.Client cl = Manipulation.db.Clients.Find(code_client);
                if (cl != null) { code_client = Randomizer(rand, 1000, 100000); }

                Model.Client client = new Model.Client
                {
                    Client_code = code_client,
                    Full_name = Full_Name.Text,
                    Phone = Phone.Text,
                    Email = Email.Text,
                    Passport_number = autorization.Userr,
                    Birthdate = DateTime.Parse(DateBornC.Text)
                };
                Manipulation.Insert<Model.Client>(client);

                Pages.NewClient Client = new Pages.NewClient(client);
                Client.Show();

                Close();
            }
            catch(ArgumentNullException ex) { MessageBox.Show("Зaполните все данные!"); }
            catch(NullReferenceException ex) { MessageBox.Show("Зaполните все данные!"); }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex) { MessageBox.Show("Зaполните все данные!"); }
            catch (Exception exp) { MessageBox.Show(exp.Message); }
}
        public int Randomizer(Random rand, int min, int max)
        {
            int number = rand.Next(min, max);
            return number;
        }
     
    }
}
