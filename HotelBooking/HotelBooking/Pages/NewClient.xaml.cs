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
    /// Логика взаимодействия для NewClient.xaml
    /// </summary>
    public partial class NewClient : Window
    {
        public Model.Client client1;
        int Nights;

        public NewClient(Model.Client cl)
        {
            try
            {
                client1 = cl;
                InitializeComponent();
                DataContext = new Model.Client();
            }
            catch (Exception exp) { MessageBox.Show(exp.Message + "/" + exp.InnerException); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> typesOfFood = new List<string>();

                foreach (Model.TypeOfFood t in Manipulation.db.TypeOfFoods)
                {
                    typesOfFood.Add(t.Type_code);
                }
                TypeOfFood.ItemsSource = typesOfFood;

                LoginCient.Content = String.Concat("Здравствуйте, ", client1.Full_name, "!");
                arrival.DisplayDateStart = DateTime.Today;
                arrival.DisplayDateEnd = DateTime.Parse("31.12.2019");
                departure.DisplayDateStart = DateTime.Today;
                departure.DisplayDateEnd = DateTime.Parse("31.12.2019");
            }
            catch (Exception exp) { MessageBox.Show(exp.Message + "/" + exp.InnerException); }
        }
        private void Out_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }
    
        private void Departure_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (arrival.SelectedDate != null && departure.SelectedDate != null)
                {
                    int nig = Booking1.CountsOfNights(DateTime.Parse(arrival.Text), DateTime.Parse(departure.Text));
                    if (nig > 1)
                    {
                        CountOfNights.Content = String.Concat(nig, " ночей");
                    }
                }
            }
            catch (Exception exp) { MessageBox.Show(exp.Message); }
        }

        private void Found_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BindingExpression b = Adults.GetBindingExpression(TextBoxWatermarked.TextProperty);
                BindingExpression f = Children.GetBindingExpression(TextBoxWatermarked.TextProperty);
                if (b.HasValidationError || f.HasValidationError)
                {
                    throw new Exception("Исправьте неправильно введенные данные!");
                }
                int people = Int32.Parse(Adults.Text) + Int32.Parse(Children.Text);
                if (people > 4)
                {
                    throw new Exception("Максимальное количество людей не должно превышать 4.");
                }
                else if(TypeOfFood.SelectedItem == null)
                {
                    throw new Exception("Выберите тип питания.");
                }
                else if (DateTime.Parse(arrival.Text) == DateTime.Parse(departure.Text) ||
                    DateTime.Parse(arrival.Text) > DateTime.Parse(departure.Text))
                {
                    throw new Exception("Одна из дат введена неверно.");
                }
                else
                {
                    IEnumerable<Model.Search_Result> search = Manipulation.db.Search(DateTime.Parse(arrival.Text),
                        DateTime.Parse(departure.Text), people).ToList<Model.Search_Result>();
                    List<Classes.ListItem> list = new List<Classes.ListItem>();
                    list = GetList(search);
                    Rooms.ItemsSource = list;
                }
            }
            catch (NullReferenceException ex) { MessageBox.Show("Зaполните все поля!", "Упс.."); }
            catch (FormatException ex) { MessageBox.Show("Зaполните все поля правильно!", "Упс.."); }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex) { MessageBox.Show("Зполните все данные!", "Упс.."); }
            catch (Exception exp) { MessageBox.Show(exp.Message, "Упс.."); }
        }

        private void Book_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                string number = (string)button.Tag;

                Random rand = new Random();
                int discount = GetDiscount();
                Model.Pay pay = new Model.Pay();
                Model.Room room = new Model.Room();

                room = Manipulation.db.Rooms.Find(number);
                room.Booking = "Да";

                Model.Booking booking = new Model.Booking
                {
                    Booking_number = Randomizer(rand, 1000, 100000),
                    Client_code = client1.Client_code,
                    Client = client1,
                    Arrivall = DateTime.Parse(arrival.Text),
                    DateOfBooking = DateTime.Today,
                    Departuree = DateTime.Parse(departure.Text),
                    TypeOfFood = (string)TypeOfFood.SelectedItem,
                    TypeOfFood1 = Manipulation.db.TypeOfFoods.Find((string)TypeOfFood.SelectedItem),
                    Payment_number = Randomizer(rand, 1000, 100000),
                    Room = room,
                    Room_code = room.Room_code
                };
                booking.Pay = new Model.Pay()
                {
                    Payment_number = booking.Payment_number,
                    DateOfPay = DateTime.Today,
                    Client = client1,
                    Payer = client1.Client_code,
                    Card_number = Randomizer(rand, 10000, 100000).ToString(),
                    Summ = booking.TypeOfFood1.Summa + room.CategoryOfRoom.Cost * Nights  - discount * 10 / 100
                };

                Manipulation.db.Bookings.Add(booking);

                MessageBox.Show(String.Concat("Вы зарегестрировали бронь!\n", "Ваша скидка - ", discount, " %"), "Поздравляем!");
                Manipulation.db.SaveChanges();
                ClearItems();
            }
            catch(NullReferenceException ex){ MessageBox.Show("Заполните все данные!"); }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex) { MessageBox.Show("Заполните все данные!"); }
            catch (Exception exp) { MessageBox.Show(exp.Message, "Упс.."); }
        }

        private void MyBooking_Click(object sender, RoutedEventArgs e)
        {
            High2.Visibility = Visibility.Collapsed;
            MyBooking.Visibility = Visibility.Visible;
            ShowBooking.Visibility = Visibility.Collapsed;
            Out.Visibility = Visibility.Visible;
        }
        private void Outer_Click(object sender, RoutedEventArgs e)
        {
            High2.Visibility = Visibility.Visible;
            MyBooking.Visibility = Visibility.Collapsed;
            ShowBooking.Visibility = Visibility.Visible;
            Out.Visibility = Visibility.Collapsed;
        }
        private List<Classes.ListItem> GetList(IEnumerable<Model.Search_Result> search)
        {
            string str = (string)CountOfNights.Content;
            string[] split = str.Split(' ');
            Nights = Int32.Parse(split[0]);
            List<Classes.ListItem> list = new List<Classes.ListItem>();
            foreach (Model.Search_Result s in search)
            {
                Classes.ListItem listItem = new Classes.ListItem();
                if (s.Category_name.Contains("Одноместный номер"))
                {
                    listItem.Path = "../Image/1чел1кров.jpg";
                    listItem.Room_code = s.Room_code;
                    listItem.Category_name = s.Category_name;
                    listItem.Cost = s.Cost;
                    listItem.NumberOfSeats = s.NumberOfSeats;
                    listItem.Servicess = GetServices(s.Servicess);
                    listItem.AndCost = s.Cost * Nights;
                }
                else if (s.Category_name.Contains("Двухместный номер с двумя кроватями"))
                {
                    listItem.Path = "../Image/2чел2кров.jpg";
                    listItem.Category_name = s.Category_name;
                    listItem.Room_code = s.Room_code;
                    listItem.Cost = s.Cost;
                    listItem.NumberOfSeats = s.NumberOfSeats;
                    listItem.Servicess = GetServices(s.Servicess);
                    listItem.AndCost = s.Cost * Nights;

                }
                else if (s.Category_name.Contains("Двухместный номер с одной большой кроватью"))
                {
                    listItem.Path = "../Image/21кровать.jpg";
                    listItem.Category_name = s.Category_name;
                    listItem.Room_code = s.Room_code;
                    listItem.Cost = s.Cost;
                    listItem.NumberOfSeats = s.NumberOfSeats;
                    listItem.Servicess = GetServices(s.Servicess);
                    listItem.AndCost = s.Cost * Nights;

                }
                else if (s.Category_name.Contains("Двухместный номер люкс с одной большой кроватью"))
                {
                    listItem.Path = "../Image/люкс2чел1кр.jpg";
                    listItem.Category_name = s.Category_name;
                    listItem.Room_code = s.Room_code;
                    listItem.Cost = s.Cost;
                    listItem.NumberOfSeats = s.NumberOfSeats;
                    listItem.Servicess = GetServices(s.Servicess);
                    listItem.AndCost = s.Cost * Nights;

                }
                else if (s.Category_name.Contains("Трехместный номер"))
                {
                    listItem.Path = "../Image/3чел3кро.jpg";
                    listItem.Category_name = s.Category_name;
                    listItem.Room_code = s.Room_code;
                    listItem.Cost = s.Cost;
                    listItem.NumberOfSeats = s.NumberOfSeats;
                    listItem.Servicess = GetServices(s.Servicess);
                    listItem.AndCost = s.Cost * Nights;

                }
                else if (s.Category_name.Contains("Четырехместный номер"))
                {
                    listItem.Path = "../Image/4чел3кроват.jpg";
                    listItem.Category_name = s.Category_name;
                    listItem.Room_code = s.Room_code;
                    listItem.Cost = s.Cost;
                    listItem.NumberOfSeats = s.NumberOfSeats;
                    listItem.Servicess = GetServices(s.Servicess);
                    listItem.AndCost = s.Cost * Nights;

                }
                list.Add(listItem);
            }
            return list;
        }
        public int Randomizer(Random rand, int min, int max)
        {
            int number = rand.Next(min, max);
            return number;
        }
        public string GetServices(string str)
        {
            string And = "";
            string[] split = str.Split(',');
            foreach (string s in split)
            {
                And += String.Concat("\u058E " + s + "\n");
            }
            return And;
        }
        public void ClearItems()
        {

            Rooms.ItemsSource = null;
        }
        private int GetDiscount()
        {
            int Discount;
            string pattern = client1.Full_name;
            List<Model.SelectBooking> selectBooking = new List<Model.SelectBooking>();
            IEnumerable<Model.SelectBooking> select = Manipulation.db.SelectBookings().ToList<Model.SelectBooking>();

            if (select.Count() > 0)
            {
                int caf = select.Count();
                if (caf > 2 && caf < 5)
                {
                    caf += 3;
                }
                else if(caf > 5 && caf < 10)
                {
                    caf += 5;
                }
                
                return Discount = caf * 110 / 100;
            }
            else
            {
                return 0;
            }
        }
    }
}
