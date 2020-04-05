using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HomeWork_11.Models
{
    public class Department : INotifyPropertyChanged
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
    }
}
