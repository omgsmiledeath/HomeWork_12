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
        private Employee empl;
        bool edit;
        /// <summary>
        /// Конструктор окна добавления работника
        /// </summary>
        /// <param name="dep"></param>
        public AddEmployee(Department dep)
        {
            InitializeComponent();
            this.dep = dep;
            edit = false;
        }

        public AddEmployee(Department dep,Employee empl)
        {
            InitializeComponent();
            this.dep = dep;
            this.empl = empl;
            EditMode();
        }

        private void EditMode()
        {
            edit = true;
            AddEmployeeButton.Content = "Редактировать";
            EmplTypes.Visibility = Visibility.Hidden;

            if(empl is Intern)
            {
                 LnameBox.Text = empl.First_Name;
                 FnameBox.Text = empl.Last_Name;
                
                 AgeBox.Text = $"{empl.Age}";
                 EmplDateBox.Text = $"{empl.EmploymentDate}";
                 EndOfInternDate.Text = $"{(empl as Intern).EndOfInternature}";
                 EmplTypes.Text ="Интерн";
                InternChoise();
            }

            if(empl is Manager)
            {
                var e = (Manager) empl;
                LnameBox.Text = e.First_Name;
                FnameBox.Text = e.Last_Name;
                AgeBox.Text = $"{e.Age}";
                PostBox.Text = empl.Post;
                EmplDateBox.Text = $"{e.EmploymentDate}";
                WorkHBox.Text = $"{e.WorkHour}";
                PaymentBox.Text = $"{e.PaymentForHour}";
                EmplTypes.Text = "Менеджер";
                ManagerChoise();
            }

            if(empl is HighManager)
            {
                var e = (HighManager)empl;
                LnameBox.Text = e.First_Name;
                FnameBox.Text = e.Last_Name;
                AgeBox.Text = $"{e.Age}";
                PostBox.Text = empl.Post;
                EmplDateBox.Text = $"{e.EmploymentDate}";
                EmplTypes.Text = "Высший менеджер";
                HighManagerChoise();

            }

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
                        
                        break;
                    case "Высший менеджер":
                        result = getHighManager();
                       
                        break;
                    default:
                        result = getInternt();
                        
                        break;

                }
                if (edit) dep.EditWorker(empl);
                else dep.AddWorker(result);
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
            if (edit)
            {
                empl.First_Name = LnameBox.Text;
                empl.Last_Name = FnameBox.Text;
                empl.Post = PostBox.Text;
                empl.Age = Convert.ToByte(AgeBox.Text);
                (empl as Manager).WorkHour = Convert.ToUInt16(WorkHBox.Text);
                (empl as Manager).PaymentForHour = Convert.ToUInt16(PaymentBox.Text);
                empl.EmploymentDate = Convert.ToDateTime(EmplDateBox.Text);
                return empl;
            }
            else {
                string fname = LnameBox.Text;
                string lname = FnameBox.Text;
                string post = PostBox.Text;
                byte age = Convert.ToByte(AgeBox.Text);
                ushort workHour = Convert.ToUInt16(WorkHBox.Text);
                ushort payment = Convert.ToUInt16(PaymentBox.Text);
                DateTime empldate = Convert.ToDateTime(EmplDateBox.Text);
                return new Manager(fname, lname, post, age, empldate, workHour, payment); 
            }
        }
        /// <summary>
        /// Метод для получения данных и формирования экземпляра класса Интерн
        /// </summary>
        /// <returns></returns>
        private Employee getInternt()
        {


            if (edit)
            {
                empl.First_Name = LnameBox.Text;
                empl.Last_Name = FnameBox.Text;
                empl.Age = Convert.ToByte(AgeBox.Text);
                empl.EmploymentDate = Convert.ToDateTime(EmplDateBox.Text);
                (empl as Intern).EndOfInternature = Convert.ToDateTime(EndOfInternDate.Text);
                return empl;
            }
            else
            {
                string fname = LnameBox.Text;
                string lname = FnameBox.Text;
                byte age = Convert.ToByte(AgeBox.Text);
                DateTime empldate = Convert.ToDateTime(EmplDateBox.Text);
                DateTime endofinter = Convert.ToDateTime(EndOfInternDate.Text);
                return new Intern(fname, lname, age, empldate, endofinter);
            } 
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
            if (edit)
            {
                empl.First_Name = LnameBox.Text;
                empl.Last_Name = FnameBox.Text;
                empl.Post = PostBox.Text;
                empl.Age = Convert.ToByte(AgeBox.Text);
                empl.EmploymentDate = Convert.ToDateTime(EmplDateBox.Text);
                return empl;
            }
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
                InternChoise();
                
            }
            if (EmplTypes.Text == "Высший менеджер")
            {
                HighManagerChoise();
            }

            if (EmplTypes.Text == "Менеджер")
            {
                ManagerChoise();
            }
        }

        private void InternChoise()
        {
            PostBox.Visibility = Visibility.Hidden;
            EndOfInternDate.Visibility = Visibility.Visible;
            WorkHBox.Visibility = Visibility.Hidden;
            PaymentBox.Visibility = Visibility.Hidden;
           
        }

        private void ManagerChoise()
        {
            PostBox.Visibility = Visibility.Visible;
            EndOfInternDate.Visibility = Visibility.Hidden;
            WorkHBox.Visibility = Visibility.Visible;
            PaymentBox.Visibility = Visibility.Visible;
        }

        private void HighManagerChoise()
        {
            PostBox.Visibility = Visibility.Visible;
            EndOfInternDate.Visibility = Visibility.Hidden;
            WorkHBox.Visibility = Visibility.Hidden;
            PaymentBox.Visibility = Visibility.Hidden;
        }


    }
}
