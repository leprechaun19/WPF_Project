using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;

namespace HotelBooking
{
    public partial class MainWindow : Window
    {
        Model.Autorization auto;

        public MainWindow()
        {
            try
            {   
                InitializeComponent();
                auto = new Model.Autorization();
                DataContext = auto;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "/" + ex.InnerException); }
        }

        public void Registration_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.Owner = this;
            registration.ShowDialog();
        }
        public void In_Click(object sender, EventArgs e)
        {
            try
            {
                BindingExpression b = Login.GetBindingExpression(TextBoxWatermarked.TextProperty);
                if (b.HasValidationError)
                {
                    throw new Exception("Исправьте неправильно введенные данные!");
                }
                auto = Manipulation.db.Enter(Login.Text, Password.Password).ToList<Model.Autorization>().FirstOrDefault();

                if (auto == null)
                {
                    throw new Exception("Такого пользователя нет в базе!\nЛибо вы ввели неправильный пароль");
                }

                string patternPassport = @"^[0-9]";
                if (Regex.IsMatch(auto.Userr, patternPassport, RegexOptions.IgnoreCase))
                {
                        Pages.Admin admin = new Pages.Admin(auto);
                        admin.Show();
                        Close();
                }
                else
                {
                    Model.Client Client = Manipulation.db.Clients
                                           .Where(user => user.Passport_number == auto.Userr)
                                           .FirstOrDefault();

                    if (Client == null)
                    {
                        Pages.LastFormForClient form = new Pages.LastFormForClient(auto);
                        form.Show();
                        Close();
                    }
                    else
                    {
                        Pages.NewClient client = new Pages.NewClient(Client);
                        client.Show();
                        Close();
                    }
                }           
            }
            catch (NullReferenceException ex) { MessageBox.Show("Зaполните все данные!"); }
            catch (DbEntityValidationException ex) { MessageBox.Show("Зaполните все данные!"); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
    }
    static class Manipulation
    {
        public static Model.HotelBookingEntities1 db = new Model.HotelBookingEntities1();

        public static IQueryable<TEntity> Select<TEntity>()
            where TEntity : class
        {
            db.Database.Log =
                (s => System.Diagnostics.Debug.WriteLine(s));

            return db.Set<TEntity>();
        }

        public static void Delete<TEntity>(TEntity entity)
    where TEntity : class
        {
            db.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            db.Entry<TEntity>(entity).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public static void Update<TEntity>(TEntity entity)
    where TEntity : class
        {
            db.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            db.Entry<TEntity>(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            db.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            db.Entry(entity).State = EntityState.Added;
            db.SaveChanges();
        }

    }
   
}
