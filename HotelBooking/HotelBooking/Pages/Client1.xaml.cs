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
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client1 : Window
    {
        public Client1()
        {
            InitializeComponent();
            DataContext = new Model.Client();
        }

        public void Add_Click(object sender, EventArgs e)
        {
            try
            {
                BindingExpression b = Full_Name.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression f = Phone.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression p = Email.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression s = PassNumber.GetBindingExpression(TextBoxWatermarked.TextProperty);
                if (b.HasValidationError || p.HasValidationError || s.HasValidationError || f.HasValidationError)
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
                    Passport_number = PassNumber.Text,
                    Birthdate = DateTime.Parse(DateBornC.Text)
                };
                Manipulation.Insert<Model.Client>(client);
                MessageBox.Show("Вы зарегестрировали клиента!");
                ClearForm();
            }
            catch (FormatException ex) { MessageBox.Show("Введите правильные даные!", "Упс"); }
            catch (NullReferenceException ex) { MessageBox.Show("Зполните все данные!"); }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex) { MessageBox.Show("Зполните все данные!"); }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void ClearForm()
        {
            Full_Name.Text = "";
            PassNumber.Text = "";
            Phone.Text = "";
            Email.Text = "";
        }
        public int Randomizer( Random rand, int min, int max)
        {
            int number = rand.Next(min, max);
            return number;
        }
    }
}
