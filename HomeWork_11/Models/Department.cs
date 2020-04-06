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

        #region Ascending
        private class SortByFirstNameAscending : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                return String.Compare(x.First_Name, y.First_Name);
            }
        }

        private class SortByLastNameAscending : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                return String.Compare(x.Last_Name, y.Last_Name);
            }
        }
        private class SortByAgeAscending : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                if (x.Age == y.Age) return 0;
                else if (x.Age > y.Age) return 1;
                else return -1;
            }
        }
        private class SortBySalaryAscending : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                if (x.Salary == y.Salary) return 0;
                else if (x.Salary > y.Salary) return 1;
                else return -1;
            }
        }

        private class SortByEmployeeDateAscending : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                return DateTime.Compare(x.EmploymentDate, y.EmploymentDate);
            }
        }

        private class SortByTypeAscending : IComparer<Employee>
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
        #endregion

        #region Descending
        private class SortByFirstNameDescending : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                var result = String.Compare(x.First_Name, y.First_Name);
                if (result == 1) return -1;
                if (result == -1) return 1;
                return result;
            }
        }

        private class SortByLastNameDescending : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                var result =  String.Compare(x.Last_Name, y.Last_Name);
                if (result == 1) return -1;
                if (result == -1) return 1;
                return result;
            }
        }
        private class SortByAgeDescending : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                if (x.Age == y.Age) return 0;
                else if (x.Age > y.Age) return -1;
                else return 1;
            }
        }
        private class SortBySalaryDescending : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                if (x.Salary == y.Salary) return 0;
                else if (x.Salary > y.Salary) return -1;
                else return 1;
            }
        }

        private class SortByEmployeeDateDescending : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                var result =  DateTime.Compare(x.EmploymentDate, y.EmploymentDate);
                if (result == 1) return -1;
                if (result == -1) return 1;
                return result;
            }
        }

        private class SortByTypeDescending : IComparer<Employee>
        {
            public int Compare(Employee x, Employee y)
            {
                if ((x is Intern) && ((y is Manager) || (y is HighManager))) return -1;
                else if ((x is Intern) && (y is Intern)) return 0;
                else if ((x is Manager) && (y is HighManager)) return -1;
                else if ((x is Manager) && (y is Manager)) return 0;
                else if ((x is HighManager) && ((y is Manager) || (y is Intern))) return 1;
                else return 1;
            }
        }
        #endregion

        private IComparer<Employee> SortByAscending(SortCriterion criterion)
        {
           
            if (criterion == SortCriterion.FristName) return new SortByFirstNameAscending();
             else if (criterion == SortCriterion.LastName) return new SortByLastNameAscending();
             else if (criterion == SortCriterion.Age) return new SortByAgeAscending();
             else if (criterion == SortCriterion.Type) return new SortByTypeAscending();
             else if (criterion == SortCriterion.Salary) return new SortBySalaryAscending();
             else return new SortByEmployeeDateAscending();
            
        }

        private IComparer<Employee> SortByDescending(SortCriterion criterion)
        {
            
            if (criterion == SortCriterion.FristName) return new SortByFirstNameDescending();
            else if (criterion == SortCriterion.LastName) return new SortByLastNameDescending();
            else if (criterion == SortCriterion.Age) return new SortByAgeDescending();
            else if (criterion == SortCriterion.Type) return new SortByTypeDescending();
            else if (criterion == SortCriterion.Salary) return new SortBySalaryDescending();
            else return new SortByEmployeeDateDescending();

        }

        public void Sort(SortCriterion criterion,bool line)
        {
            var listDep = Employees.ToList<Employee>();
            if (line == true)
            {
                listDep.Sort(SortByAscending(criterion));
            }
            else
            {
                listDep.Sort(SortByDescending(criterion));
            }

            Employees.Clear();

            foreach (var item in listDep)
            {
                AddWorker(item);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Employees)));
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
