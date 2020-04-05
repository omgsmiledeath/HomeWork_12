using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace HomeWork_11.Models
{
    public class Department : INotifyPropertyChanged , IEquatable<Department>
    {
        #region Статические элементы
        static protected ushort DepCount; //количество департаментов 
        static protected List<string> DepNames; //лист названий департаментов
        /// <summary>
        /// Конструктор для статических переменных
        /// </summary>
        static Department()
        {
            DepCount = 0;
            DepNames = new List<string>();
        }
        #endregion


        private ObservableCollection<Employee> employees; //список сотрудников в департаменте
        private ObservableCollection<Department> departments; //вложенные департаменты
        private string id; //ID департамента
        private string departmentName; //название департамента
        /// <summary>
        /// Конструктор департамента
        /// </summary>
        /// <param name="name">Название</param>
        public Department(string name)
        {
            employees = new ObservableCollection<Employee>();
            departments = new ObservableCollection<Department>();
            DepartmentName = name;
            Id = Guid.NewGuid().ToString().Substring(0, 5) + (++DepCount);
        }
        
        public ObservableCollection<Employee> Employees { get => employees;}
        public ObservableCollection<Department> Departments { get => departments;}

        public string Id { get => id; set => id = value; }

        public string DepartmentName { get => departmentName; set => departmentName = value; }
        

        public void AddSubDepartment(Department dep)
        {
            Departments.Add(dep);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Departments)));
        }

        public void AddWorker(Employee newWorker)
        {
            if(!employees.Contains(newWorker)) Employees.Add(newWorker);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Employees)));
        }

        public ObservableCollection<Department> GetDepartmens() => departments;


        public event PropertyChangedEventHandler PropertyChanged;


       public enum SortCriterion
        {
            FristName,
            LastName,
            Age,
            Type,
            DateEmployee,
            Salary
        }

        private class SortByFirstName : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                return String.Compare(x.First_Name, y.First_Name);
            }
        }

        private class SortByLastName : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                return String.Compare(x.Last_Name, y.Last_Name);
            }
        }
        private class SortByAge : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                if (x.Age == y.Age) return 0;
                else if (x.Age > y.Age) return 0;
                else return -1;
            }
        }
        private class SortBySalary : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                if (x.Salary == y.Salary) return 0;
                else if (x.Salary > y.Salary) return 0;
                else return -1;
            }
        }

        private class SortByEmployeeDate : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                return DateTime.Compare(x.EmploymentDate, y.EmploymentDate);
            }
        }

        private class SortByType : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                if ((x is Intern) && ((y is Manager) || (y is HighManager))) return 1;
                else if ((x is Intern) && (y is Intern)) return 0;
                else if ((x is Manager) && (y is HighManager)) return 1;
                else if ((x is Manager) && (y is Manager)) return 0;
                else if ((x is HighManager) && ((y is Manager) || (y is Intern))) return -1;
                else return -1;
            }
        }

        private IComparer<Employee> SortBy(SortCriterion criterion)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Employees)));
            if (criterion == SortCriterion.FristName) return new SortByFirstName();
             else if (criterion == SortCriterion.LastName) return new SortByLastName();
             else if (criterion == SortCriterion.Age) return new SortByAge();
             else if (criterion == SortCriterion.Type) return new SortByType();
             else if (criterion == SortCriterion.Salary) return new SortBySalary();
             else return new SortByEmployeeDate();
            
        }

        public void Sort(SortCriterion criterion)
        {
            var listDep = Employees.ToList<Employee>();

            if (criterion == SortCriterion.FristName) listDep.Sort(SortBy(Department.SortCriterion.FristName));
            else if (criterion == SortCriterion.LastName) listDep.Sort(SortBy(Department.SortCriterion.LastName));
            else if (criterion == SortCriterion.Age) listDep.Sort(SortBy(Department.SortCriterion.Age));
            else if (criterion == SortCriterion.Type) listDep.Sort(SortBy(Department.SortCriterion.Type));
            else if (criterion == SortCriterion.Salary) listDep.Sort(SortBy(Department.SortCriterion.Salary));
            else listDep.Sort(SortBy(Department.SortCriterion.DateEmployee));

            Employees.Clear();

            foreach (var item in listDep)
            {
                AddWorker(item);
            }
        }

        public bool Equals(Department other) => this.Id == other.Id;

        public void EditWorker(Employee worker)
        {
            Employees[Employees.IndexOf(worker)] = worker;
           // Employees.RemoveAt(Employees.IndexOf(worker));
           // Employees.Add(worker);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Employees)));
        }
    }
}
