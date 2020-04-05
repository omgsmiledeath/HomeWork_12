using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_11.Models
{
    class HighManager : Employee
    {

        public HighManager(string fname,
            string lname,
            string post,
            byte age,
            DateTime emplDate,
            Department dep
            ) :base (fname,lname,post,age,emplDate)
        {
            CalcSalary(dep);
        }

        public HighManager() :base ()
        {

        }

        public override uint CalcSalary(Department dep)
        {
            uint result = 0;

            if (dep.Departments.Count != 0)
            {
                foreach (var item in dep.Departments)
                {
                    foreach (var worker in item.Employees)
                    {
                        if (worker is HighManager)
                        {
                            result += (worker as HighManager).CalcSalary(item);
                        }
                    }
                }
            }
            if (dep.Employees.Count > 1)
                foreach (var item in dep.Employees)
                {
                    if (!(item is HighManager) || (item is Manager) || (item is Intern))
                    {
                        result += item.Salary;
                    }
                    
                }
            if (result*15/100 < 1300) Salary = 1300;
            else  Salary = result*15/100;
            return result + this.Salary;
        }
    }
}
