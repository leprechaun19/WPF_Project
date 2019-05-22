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
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace HotelBooking
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        Model.Autorization auto;
        public Registration()
        {
            InitializeComponent();
            auto = new Model.Autorization();
            DataContext = auto;
        }

        public void Registration_Click(object sender, EventArgs e)
        {
            try
            {
                bool check = (bool)AreWorker.IsChecked;
                foreach (Model.Autorization a in Manipulation.db.Autorizations.ToList<Model.Autorization>())
                {
                    if (a.Login == Login.Text)
                    {
                        throw new Exception("Логин уже занят, пожалуйста, выберите другой!");
                    }
                }
                if (check == true)
                {
                    BindingExpression b = Employee.GetBindingExpression(TextBoxWatermarked.TextProperty);
                    if (b.HasValidationError)
                    {
                        throw new Exception("Исправьте неправильно введенные данные!");
                    }

                    Model.Worker worker = Manipulation.db.Workers.Find(int.Parse(Employee.Text));

                    if (worker == null)
                    {
                        throw new Exception("Сотрудника с таким кодом не существует!");
                    }
                    else
                    {
                        Model.Autorization autor = new Model.Autorization();
                        autor.Login = Login.Text;
                        autor.Userr = Employee.Text;
                        autor.Password = Password.Text;
                        Manipulation.db.Autorizations.Add(autor);
                        Manipulation.db.SaveChanges();
                        MessageBox.Show("Поздравляем, Вы зарегестрированы!");
                        ClearForm();
                    }
                }
                else
                {
                    BindingExpression b = Client.GetBindingExpression(TextBoxWatermarked.TextProperty);
                    if (b.HasValidationError)
                    {
                        throw new Exception("Исправьте неправильно введенные данные!");
                    }

                    Model.Autorization autor = new Model.Autorization
                    {
                        Login = Login.Text,
                        Userr = Client.Text,
                        Password = Password.Text
                    };
                    Manipulation.db.Autorizations.Add(autor);
                    Manipulation.db.SaveChanges();
                    MessageBox.Show("Поздравляем, Вы зарегестрированы!");
                    ClearForm();
                }
            }
            catch (DbEntityValidationException ex) {
                foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                {
                    MessageBox.Show("Object: " + validationError.Entry.Entity.ToString());
                    MessageBox.Show(" ");
                    foreach (DbValidationError err in validationError.ValidationErrors)
                    {
                        MessageBox.Show(err.ErrorMessage + " ");
                    }
                }
                MessageBox.Show("Заполните все данные!"); }
            catch(NullReferenceException ex) { MessageBox.Show("Заполните поля!", "Упс.."); }
            catch (System.Data.Entity.Infrastructure.DbUpdateException exp) { MessageBox.Show("Пользователь с таким логином уже существует, пожалуйста, придумайте другой.", "Упс.."); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Упс.."); }
        }

        public void ClearForm()
        {
            Login.Clear();
            Password.Clear();
            Employee.Clear();
            Client.Clear();
        }

        private void AreWorker_Unchecked(object sender, RoutedEventArgs e)
        {
            Employee.Visibility = Visibility.Collapsed;
            Client.Visibility = Visibility.Visible;
        }
        private void AreWorker_Checked(object sender, RoutedEventArgs e)
        {
            Employee.Visibility = Visibility.Visible;
            Client.Visibility = Visibility.Collapsed;
        }
    }
}
