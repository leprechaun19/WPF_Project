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
    /// Логика взаимодействия для Worker.xaml
    /// </summary>
    public partial class Workerr : Window
    {
        Model.Worker worker;
        public Workerr()
        {
            InitializeComponent();
            worker = new Model.Worker();
            DataContext = new Model.Search_Result();
        }

        public void Add_Click(object sender, EventArgs e)
        {
            try
            {
                BindingExpression b = Code.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression f = Fuul_Name.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression p = Position.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression s = Salary.GetBindingExpression(TextBoxWatermarked.TextProperty);
                if (b.HasValidationError || p.HasValidationError || s.HasValidationError || f.HasValidationError)
                {
                    throw new Exception("Исправьте неправильно введенные данные!");
                }
                worker.Worker_code = int.Parse(Code.Text);
                worker.Full_name = Fuul_Name.Text;
                worker.Position = Position.Text;
                worker.Salary = int.Parse(Salary.Text);
                worker.Birthdate = DateTime.Parse(DateBorn.Text);
                Manipulation.Insert<Model.Worker>(worker);
                MessageBox.Show("Вы зарегестрировали работника!");
                ClearForm();
            }
            catch (FormatException ex) { MessageBox.Show("Введите правильные даные!", "Упс"); }
            catch (NullReferenceException ex) { MessageBox.Show("Зполните все данные!"); }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex) { MessageBox.Show("Зполните все данные!"); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Упс.."); }
        }

        public void ClearForm()
        {
            Fuul_Name.Text = "";
            Position.Text = "";
            Code.Text = "";
            Salary.Text = "";
        }
    }
}
