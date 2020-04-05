using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_11.Models
{
    class Manager : Employee
    {
        private ushort workHour; //количество рабочих часов в месяц
        private ushort paymentForHour; //оплата за 1 рабочий час



        public Manager(string fname, 
            string lname, 
            string post,
            byte age, 
            DateTime emplDate,
            ushort workHour,
            ushort paymentForHour) : base(fname, lname, post, age, emplDate)
        {
            WorkHour = workHour;
            PaymentForHour = paymentForHour;
            Salary = CalcSalary();
        }

        public Manager() :base()
        {
            WorkHour = 50;
            paymentForHour = 15;
            Salary = CalcSalary();
        }


        public ushort WorkHour { get => workHour; set 
            { workHour = value;
                Salary = CalcSalary();
                OnPropertyChanged();
            } }
        public ushort PaymentForHour { get => paymentForHour; set 
            {
                paymentForHour = value;
                Salary = CalcSalary();
                OnPropertyChanged();
            } }

        public override uint CalcSalary(Department dep = null)
        {

            return (uint) workHour * paymentForHour;

        }




        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
