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
    /// Логика взаимодействия для Booking.xaml
    /// </summary>
    public partial class Booking1 : Window
    {
        int Nights;
        public Booking1()
        {
            InitializeComponent();
            DataContext = new Model.CategoryOfRoom();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
            List<string> typesOfFood = new List<string>();
            List<string> clients = new List<string>();

            foreach (Model.TypeOfFood t in Manipulation.db.TypeOfFoods)
            {
                typesOfFood.Add(t.Type_code);
            }
            foreach (Model.Client c in Manipulation.db.Clients)
            {
                clients.Add(c.Full_name);
            }

            Type_food.ItemsSource = typesOfFood;
            ListOfClients.ItemsSource = clients;
            dateOfPay.DisplayDateStart = DateTime.Today;
            Arrival.DisplayDateStart = DateTime.Today;
            Departure.DisplayDateStart = DateTime.Today;
        }

        public void Add_Click(object sender, EventArgs e)
        {
            try
            {
                Model.Client client = new Model.Client();
                Model.Pay pay = new Model.Pay();
                Model.Room room = new Model.Room();
                Model.TypeOfFood type = new Model.TypeOfFood();
                Model.Booking booking = new Model.Booking();
                string numberOfRoom = (string)Room_Code.SelectedItem;
                string[] split = numberOfRoom.Split(' ');
                Random rand = new Random();

                room = Manipulation.db.Rooms.Find(split[0]);
                type = Manipulation.db.TypeOfFoods.Find((string)Type_food.SelectedItem);

                if (ClientExist.IsChecked == true)
                {
                BindingExpression b = card_number.GetBindingExpression(TextBoxWatermarked.TextProperty);
                if (b.HasValidationError)
                {
                    throw new Exception("Исправьте неправильно введенные данные!");
                }
                IEnumerable<Model.Client> cli = Manipulation.db.Clients.Where(w => w.Full_name == (string)ListOfClients.SelectedItem);
                    client = cli.First();
                }
                else
                {
                BindingExpression b = card_number.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression f = Full_Name.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression p = Email.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression s = Phone.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression a = PassNumber.GetBindingExpression(TextBoxWatermarked.TextProperty);
                if (b.HasValidationError || p.HasValidationError || s.HasValidationError || f.HasValidationError || a.HasValidationError)
                {
                    throw new Exception("Исправьте неправильно введенные данные!");
                }

                int code_client = Randomizer(rand, 1000, 100000);

                    client.Client_code = code_client;
                    client.Full_name = Full_Name.Text;
                    client.Phone = Phone.Text;
                    client.Email = Email.Text;
                    client.Passport_number = PassNumber.Text;
                    client.Birthdate = DateTime.Parse(DateBornC.Text);

                    Manipulation.db.Clients.Add(client);

                }
                pay.Payment_number = Randomizer(rand, 10, 500);
                pay.Card_number = card_number.Text;
                pay.Summ = type.Summa + room.CategoryOfRoom.Cost*Nights;
                pay.Payer = client.Client_code;
                pay.DateOfPay = DateTime.Parse(dateOfPay.Text);
                client.Pays.Add(pay);

                Manipulation.db.Pays.Add(pay);
                room.Booking = "Да";

                booking.Booking_number = Randomizer(rand, 1000, 100000);
                booking.Client_code = client.Client_code;
                booking.Client = client;
                booking.Arrivall = DateTime.Parse(Arrival.Text);
                booking.DateOfBooking = DateTime.Today;
                booking.Departuree = DateTime.Parse(Departure.Text);
                booking.Pay = pay;
                booking.Payment_number = pay.Payment_number;
                booking.Room = room;
                booking.Room_code = room.Room_code;
                booking.TypeOfFood = type.Type_name;
                booking.TypeOfFood1 = type;


                Manipulation.db.Bookings.Add(booking);

                MessageBox.Show("Вы зарегестрировали бронь!");
                Manipulation.db.SaveChanges();
                Close();

            }
            catch (NullReferenceException ex) { MessageBox.Show("Зaполните все данные!"); }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex) { MessageBox.Show("Зaполните все данные!"); }
            catch (Exception exp) { MessageBox.Show(exp.Message); }
        }

        private void ClientExist_UnChecked(object sender, RoutedEventArgs e)
        {
            Phone.Visibility = Visibility.Visible;
            PhoneT.Visibility = Visibility.Visible;
            Full_Name.Visibility = Visibility.Visible;
            Full_nameT.Visibility = Visibility.Visible;
            PassNumber.Visibility = Visibility.Visible;
            PassNumberT.Visibility = Visibility.Visible;
            DateBornC.Visibility = Visibility.Visible;
            DateBornT.Visibility = Visibility.Visible;
            Email.Visibility = Visibility.Visible;
            EmailT.Visibility = Visibility.Visible;

            ListOfClients.Visibility = Visibility.Collapsed;
            ListOfClientsT.Visibility = Visibility.Collapsed;
        }
        private void ClientExist_Checked(object sender, RoutedEventArgs e)
        {
            Phone.Visibility = Visibility.Collapsed;
            PhoneT.Visibility = Visibility.Collapsed;
            Full_Name.Visibility = Visibility.Collapsed;
            Full_nameT.Visibility = Visibility.Collapsed;
            PassNumber.Visibility = Visibility.Collapsed;
            PassNumberT.Visibility = Visibility.Collapsed;
            DateBornC.Visibility = Visibility.Collapsed;
            DateBornT.Visibility = Visibility.Collapsed;
            Email.Visibility = Visibility.Collapsed;
            EmailT.Visibility = Visibility.Collapsed;

            ListOfClients.Visibility = Visibility.Visible;
            ListOfClientsT.Visibility = Visibility.Visible;
        }

        public int Randomizer(Random rand, int min, int max)
        {
            int number = rand.Next(min, max);
            return number;
        }
        static public int CountsOfNights(DateTime dateArr, DateTime dateDep)
        {
            int Nights;
            TimeSpan date = dateDep.Subtract(dateArr);
            
            Nights = date.Days;
            if(Nights < 0) { return 0; }
            return Nights;           
        }
        private void Departure_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                List<string> rooms = new List<string>();
                IEnumerable<Model.FreeRoomsForBooking> room = Manipulation.db.FreeRoomsForBooking(DateTime.Parse(Arrival.Text), DateTime.Parse(Departure.Text)).ToList<Model.FreeRoomsForBooking>();

                foreach (Model.FreeRoomsForBooking r in room)
                {
                    string str = String.Concat(r.Room_code, " ", r.Category_name);
                    rooms.Add(str);
                }

                Room_Code.ItemsSource = rooms;

                Room_Code.IsEnabled = true;
                Nights = CountsOfNights((DateTime)Arrival.SelectedDate, (DateTime)Departure.SelectedDate);
                CountOfNights.Content = String.Concat("Количество ночей:          ", Nights);
            }
            catch (Exception exp) { MessageBox.Show(exp.Message); }
        }

        
    }
}
