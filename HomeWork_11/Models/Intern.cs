using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_11.Models
{
    class Intern : Employee
    {

        private DateTime endOfInternature; //Дата окончания интернатуры

        /// <summary>
        /// Конструктор класса интерн
        /// </summary>
        /// <param name="fname">Имя</param>
        /// <param name="lname">Фамилия</param>
        /// <param name="age">Возраст</param>
        /// <param name="emplDate">Дата приема на стажировку</param>
        /// <param name="endofInter">Дата окончания стажировки</param>
        public Intern(string fname, 
            string lname, 
            byte age,
            DateTime emplDate,DateTime endofInter) : base(fname, lname, "СТАЖЕР", age, emplDate)
        {
            EndOfInternature = endofInter;
            Salary = CalcSalary();
        }

        public Intern() :base()
        {
            EndOfInternature = Convert.ToDateTime("2100.1.1");
            Salary = CalcSalary();
        }

        /// <summary>
        /// Автосвойство для даты окончания интернатуры
        /// </summary>
        public DateTime EndOfInternature { get => endOfInternature; set => endOfInternature = value; }

        /// <summary>
        /// Перезагруженное свойство подсчета зарплаты
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public override uint CalcSalary(Department dep = null)
        {
            return 500;
        }
    }

}
