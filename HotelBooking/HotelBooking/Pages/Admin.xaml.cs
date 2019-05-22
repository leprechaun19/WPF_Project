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
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace HotelBooking.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        Model.Autorization autorization;

        public Admin(Model.Autorization autor)
        {
            try
            {
                autorization = new Model.Autorization();
                autorization = autor;

                InitializeComponent();
                DataContext = new Model.SelectBooking();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginAdmin.Content = String.Concat("Здравствуйте, ", autorization.Login, "!");
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                UpdateDataGrid();

            }
            catch (Exception exp) { MessageBox.Show(exp.Message, "Упс"); }
        }

        private void AddClient_Click(object sender, EventArgs e)
        {
            try
            {
                Client1 client1 = new Client1
                {
                    Owner = this
                };
                client1.ShowDialog();
                UpdateDataGrid();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
        private void AddWorker_Click(object sender, EventArgs e)
        {
            try
            {
                Workerr workerr = new Workerr
                {
                    Owner = this
                };
                workerr.ShowDialog();
                UpdateDataGrid();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
        private void AddBooking_Click(object sender, EventArgs e)
        {
            Pages.Booking1 booking1 = new Pages.Booking1
            {
                Owner = this
            };
            booking1.ShowDialog();
            UpdateDataGrid();
        }

        private void DeleteBooking_Click(object sender, EventArgs e) 
        {
            try
            {
                Model.Booking book = new Model.Booking();
                Model.SelectBooking booking = (Model.SelectBooking)Booking.SelectedItem;
                book = Manipulation.db.Bookings.Find(booking.Booking_number);
                book.Room.Booking = "Нет";

                Manipulation.Delete<Model.Booking>(book);
                UpdateDataGrid();
                MessageBox.Show("Запись удалена!");
            }
            catch (NullReferenceException ex) { MessageBox.Show("Выберите бронь, чтобы удалить ее!"); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
        private void DeleteWorker_Click(object sender, EventArgs e)
        {
            try
            {               
                Model.Worker worker = new Model.Worker();
                worker = (Model.Worker)Workers.SelectedItem;
                if (worker.Worker_code == 7142476)
                {
                    throw new Exception("Нельзя удалить главного Администратора!");
                }

                Manipulation.Delete<Model.Worker>(worker);
                UpdateDataGrid();
                MessageBox.Show("Запись удалена!");
            }
            catch (ArgumentNullException ex) { MessageBox.Show("Выберите работника, чтобы удалить его!"); }
            catch (Exception ex) { MessageBox.Show(ex.Message+ex.InnerException.ToString()); }

        }
        private void DeleteClient_Click(object sender, EventArgs e)
        {
            try
            {
                Model.Client client = new Model.Client();
                client = (Model.Client)Clients.SelectedItem;
                IEnumerable<Model.Booking> bok = Manipulation.db.Bookings.ToList<Model.Booking>();
                Model.Booking one = bok
                    .Where(b => b.Client_code == client.Client_code)
                    .FirstOrDefault();
                
                if (one != null)
                {
                    throw new Exception("Сначала удалите все брони клиента, затем самого клиента!");
                }
            foreach (Model.Pay p in client.Pays.ToList<Model.Pay>())
            {
                Manipulation.Delete<Model.Pay>(p);
            }
            
                Manipulation.Delete<Model.Client>(client);
                UpdateDataGrid();
                MessageBox.Show("Запись удалена!");
        }
            catch(NullReferenceException ex) { MessageBox.Show("Выберите клиента, чтобы удалить его!"); }
            catch (ArgumentNullException ex) { MessageBox.Show("Выберите клиента, чтобы удалить его!"); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void PlusButton_Click(object sender, EventArgs e)
        {
            try
            {
                Collapsed();
                ArrivalP.Visibility = Visibility.Visible;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void PlusButtonD_Click(object sender, EventArgs e)
        {
            try
            {
                Collapsed();
                Departure.Visibility = Visibility.Visible;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void PlusButtonF_Click(object sender, EventArgs e)
        {
            try
            {
                Collapsed();
                FreeRooms.Visibility = Visibility.Visible;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Workers_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                Manipulation.db.SaveChanges();
            }
            catch (Exception exp) { MessageBox.Show(exp.Message, "Упс"); }
        }
        private void Booking_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                Manipulation.db.SaveChanges();
            }
            catch (Exception exp) { MessageBox.Show(exp.Message, "Упс"); }
        }
        private void Clients_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
                {
                    try
                    {
                        Manipulation.db.SaveChanges();
                    }
                    catch (Exception exp) { MessageBox.Show(exp.Message, "Упс"); }
                }

        private void Arrivals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BindingExpression p = ArrivalBox.GetBindingExpression(TextBoxWatermarked.TextProperty);              
                if (p.HasValidationError) { throw new Exception("Исправьте неправильно введенные данные!"); }
                List<Model.SelectBooking> selectBooking = new List<Model.SelectBooking>();
                IEnumerable<Model.SelectBooking> select = Manipulation.db.SelectArrival(DateTime.Parse(ArrivalBox.Text)).ToList<Model.SelectBooking>();

                foreach (Model.SelectBooking b in select)
                {
                    selectBooking.Add(b);
                }

                See.Columns.Clear();
                See.Items.Clear();
                ArrivalBox.Clear();
                if (selectBooking.Count == 0) { throw new Exception("На текущую дату заездов нет."); }
                See.Visibility = Visibility.Visible;

                GenerateTable();

                foreach (Model.SelectBooking b in select)
                {
                    See.Items.Add(b);
                }
            }
            catch(FormatException ex) { MessageBox.Show("Введите правильную дату!", "Упс"); }
            catch (NullReferenceException ex) { MessageBox.Show("Введите правильную дату!", "Упс"); }            
            catch (Exception exp) { MessageBox.Show(exp.Message, "Упс"); }
        }
        private void Departure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BindingExpression p = DepartureBox.GetBindingExpression(TextBoxWatermarked.TextProperty);
                if (p.HasValidationError) { throw new Exception("Исправьте неправильно введенные данные!"); }
                List<Model.SelectBooking> selectBooking = new List<Model.SelectBooking>();
                IEnumerable<Model.SelectBooking> select = Manipulation.db.SelectDeparture(DateTime.Parse(DepartureBox.Text)).ToList<Model.SelectBooking>();

                foreach (Model.SelectBooking b in select)
                {
                    selectBooking.Add(b);
                }

                See.Columns.Clear();
                See.Items.Clear();
                DepartureBox.Clear();
                if (selectBooking.Count == 0) { throw new Exception("На текущую дату заездов нет."); }
                See.Visibility = Visibility.Visible;

                GenerateTable();

                foreach (Model.SelectBooking b in select)
                {
                    See.Items.Add(b);
                }
            }
            catch (NullReferenceException ex) { MessageBox.Show("Введите правильную дату!", "Упс"); }
            catch (FormatException ex) { MessageBox.Show("Введите правильную дату!", "Упс"); }
            catch (Exception exp) { MessageBox.Show(exp.Message, "Упс"); }
        }
        private void GetFreeRooms_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BindingExpression p = FreeC.GetBindingExpression(TextBoxWatermarked.TextProperty);
                if (p.HasValidationError) { throw new Exception("Исправьте неправильно введенные данные!"); }
                List<Model.FreeRooms> selectRooms = new List<Model.FreeRooms>();
                IEnumerable<Model.FreeRooms> select = Manipulation.db.FreeRooms(DateTime.Parse(FreeC.Text)).ToList<Model.FreeRooms>();

                foreach (Model.FreeRooms b in select)
                {
                    selectRooms.Add(b);
                }

                See.Columns.Clear();
                See.Items.Clear();
                FreeC.Clear();
                if (selectRooms.Count == 0) { throw new Exception("На текущую дату свободных комнат нет."); }
                See.Visibility = Visibility.Visible;

                #region a
                DataGridTextColumn t1 = new DataGridTextColumn();
                t1.Header = "Номер ";
                t1.Binding = new Binding("Room_code");
                See.Columns.Add(t1);
                DataGridTextColumn t2 = new DataGridTextColumn();
                t2.Header = "Тип номера";
                t2.Binding = new Binding("Category_name");
                See.Columns.Add(t2);
                DataGridTextColumn t3 = new DataGridTextColumn();
                t3.Header = "Количесво мест";
                t3.Binding = new Binding("NumberOfSeats");
                See.Columns.Add(t3);
                #endregion

                foreach (Model.FreeRooms b in select)
                {
                    See.Items.Add(b);
                }
            }
            catch (FormatException ex) { MessageBox.Show("Введите правильную дату!", "Упс"); }
            catch (NullReferenceException ex) { MessageBox.Show("Введите правильную дату!", "Упс"); }
            catch (Exception exp) { MessageBox.Show(exp.Message, "Упс"); }
        }

        private void Collapsed()
        {
            ArrivalP.Visibility = Visibility.Collapsed;
            Departure.Visibility = Visibility.Collapsed;
            FreeRooms.Visibility = Visibility.Collapsed;
        }
        public void UpdateDataGrid()
        {
            try
            {
                List<Model.SelectBooking> selectBooking = new List<Model.SelectBooking>();
                IEnumerable<Model.SelectBooking> select = Manipulation.db.SelectBookings().ToList<Model.SelectBooking>();
                foreach (Model.SelectBooking b in select)
                {
                    selectBooking.Add(b);
                }
                Manipulation.db.SaveChanges();
                Booking.ItemsSource = selectBooking;
                Workers.ItemsSource = Manipulation.db.Workers.ToList<Model.Worker>();
                Clients.ItemsSource = Manipulation.db.Clients.ToList<Model.Client>();

            }
            catch (Exception exp) { MessageBox.Show(exp.Message, "Упс"); }
        }
        public void GenerateTable()
        {
            try
            {
                DataGridTextColumn t1 = new DataGridTextColumn
                {
                    Header = "№ Брони",
                    Binding = new Binding("Booking_number")
                };
                See.Columns.Add(t1);
                DataGridTextColumn t2 = new DataGridTextColumn
                {
                    Header = "Номер",
                    Binding = new Binding("Room_code")
                };
                See.Columns.Add(t2);
                DataGridTextColumn t3 = new DataGridTextColumn
                {
                    Header = "Гость ",
                    Binding = new Binding("Full_name")
                };
                See.Columns.Add(t3);
                DataGridTextColumn t4 = new DataGridTextColumn
                {
                    Header = "Тип питания",
                    Binding = new Binding("Type_name")
                };
                See.Columns.Add(t4);
                DataGridTextColumn t5 = new DataGridTextColumn
                {
                    Header = "№ Платежа",
                    Binding = new Binding("Payment_number")
                };
                See.Columns.Add(t5);
                DataGridTextColumn t6 = new DataGridTextColumn
                {
                    Header = "Дата бронирования",
                    Binding = new Binding("DateOfBooking")
                };
                See.Columns.Add(t6);
                DataGridTextColumn t7 = new DataGridTextColumn
                {
                    Header = "Заезд ",
                    Binding = new Binding("Arrivall")
                };
                See.Columns.Add(t7);
                DataGridTextColumn t8 = new DataGridTextColumn
                {
                    Header = "Выезд ",
                    Binding = new Binding("Departuree")
                };
                See.Columns.Add(t8);
                DataGridTextColumn t9 = new DataGridTextColumn
                {
                    Header = "Сумма",
                    Binding = new Binding("Summ")
                };
                See.Columns.Add(t9);
            }
            catch (Exception exp) { MessageBox.Show(exp.Message + "/" +exp.InnerException); }
        }
        private void Out_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }
    }
}
