using HomeWork_11.Models;
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

namespace HomeWork_11
{

    public partial class AddDepartment : Window
    {
        Department dep; //хранение переменной в кторою добавляется департамент

        public AddDepartment()
        {
            InitializeComponent();
        }
        //конструктор 
        public AddDepartment(Department dep)
        {
            InitializeComponent();
            this.dep = dep;
            DepNameBox.DataContext = dep.DepartmentName;
        }

        private void AddDepButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepNameBox.Text != String.Empty)
                dep.DepartmentName = DepNameBox.Text;
            else MessageBox.Show("Введите название");
            this.DialogResult = true;
            this.Close();
            
        }
    }
}
