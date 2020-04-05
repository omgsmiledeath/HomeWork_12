using System;
using System.Windows;
using HomeWork_11.Models;
namespace HomeWork_11
{
    /// <summary>
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    partial class AddEmployee : Window
    {

        private Department dep; //хранит департамент в котором добавляется сотрудник

        /// <summary>
        /// Конструктор окна добавления работника
        /// </summary>
        /// <param name="dep"></param>
        public AddEmployee(Department dep)
        {
            InitializeComponent();
            this.dep = dep;

        }

        /// <summary>
        /// Метод нажатия по кнопке добавления работника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            Employee result = null;
            if (CheckBoxes())
            {
                switch (EmplTypes.Text)
                {
                    case "Менеджер":

                        result = getManager();
                        dep.AddWorker(result);
                        break;
                    case "Высший менеджер":
                        result = getHighManager();
                        dep.AddWorker(result);
                        break;
                    default:
                        result = getInternt();
                        dep.AddWorker(result);
                        break;

                }
                this.Close();
            }
            
            
        }

        /// <summary>
        /// Метод проверки текстовых полей на заполненность
        /// </summary>
        /// <returns></returns>
        private bool CheckBoxes()
        {
            if(LnameBox.Text==String.Empty|| FnameBox.Text == String.Empty || AgeBox.Text ==String.Empty ||
                EmplDateBox.Text == string.Empty)
            {
                MessageBox.Show("Заполните все поля");
                return false;
            }
            
            switch (EmplTypes.Text)
            {
                case "Менеджер":
                    if (PostBox.Text==string.Empty || WorkHBox.Text==string.Empty|| PaymentBox.Text==string.Empty )
                    {
                        MessageBox.Show("Заполните все поля");
                        return false;
                    }
                        break;
                case "Высший менеджер":
                    if (PostBox.Text == string.Empty)
                    {
                        MessageBox.Show("Заполните все поля");
                        return false;
                    }
                    break;
                default:
                    if ( EndOfInternDate.Text == string.Empty)
                    {
                        MessageBox.Show("Заполните все поля");
                        return false;
                    }
                    break;
                    
            }
            return true;

        }

        /// <summary>
        /// Метод для получения данных и формирования экземпляра класса Менеджер
        /// </summary>
        /// <returns></returns>
        private Employee getManager()
        {
            string fname = LnameBox.Text;
            string lname = FnameBox.Text;
            string post = PostBox.Text;
            byte age = Convert.ToByte(AgeBox.Text);
            ushort workHour = Convert.ToUInt16(WorkHBox.Text);
            ushort payment = Convert.ToUInt16(PaymentBox.Text);
            DateTime empldate = Convert.ToDateTime(EmplDateBox.Text);

            return new Manager(fname, lname, post, age, empldate, workHour, payment);
        }
        /// <summary>
        /// Метод для получения данных и формирования экземпляра класса Интерн
        /// </summary>
        /// <returns></returns>
        private Employee getInternt()
        {
            string fname = LnameBox.Text;
            string lname = FnameBox.Text;
            byte age = Convert.ToByte(AgeBox.Text);
            DateTime empldate = Convert.ToDateTime(EmplDateBox.Text);
            DateTime endofinter = Convert.ToDateTime(EndOfInternDate.Text);
            return new Intern(fname,lname,age,empldate,endofinter);
        }
        /// <summary>
        /// Метод для получения данных и формирования экземпляра класса Топ Менеджер
        /// </summary>
        /// <returns></returns>
        private Employee getHighManager()
        {
            string fname = LnameBox.Text;
            string lname = FnameBox.Text;
            string post = PostBox.Text;
            byte age = Convert.ToByte(AgeBox.Text);
            DateTime empldate = Convert.ToDateTime(EmplDateBox.Text);
            return new HighManager(fname,lname,post,age,empldate,dep);
        }

        /// <summary>
        /// Отображение нужных полей по выбору типа сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmplTypes_DropDownClosed(object sender, EventArgs e)
        {
            if (EmplTypes.Text == "Интерн")
            {
                PostBox.Visibility = Visibility.Hidden;
                EndOfInternDate.Visibility = Visibility.Visible;
                WorkHBox.Visibility = Visibility.Hidden;
                PaymentBox.Visibility = Visibility.Hidden;
                
            }
            if (EmplTypes.Text == "Высший менеджер")
            {
                PostBox.Visibility = Visibility.Visible;
                EndOfInternDate.Visibility = Visibility.Hidden;
                WorkHBox.Visibility = Visibility.Hidden;
                PaymentBox.Visibility = Visibility.Hidden;
            }

            if (EmplTypes.Text == "Менеджер")
            {
                PostBox.Visibility = Visibility.Visible;
                EndOfInternDate.Visibility = Visibility.Hidden;
                WorkHBox.Visibility = Visibility.Visible;
                PaymentBox.Visibility = Visibility.Visible;
            }
        }


    }
}
