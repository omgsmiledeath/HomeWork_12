using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HomeWork_11.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace HomeWork_11
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Department organization; // Экземпляр всей организации
        private OrganizationBase repo; // экземпляр базы для сохранения и загрузок
       
        private Employee transferEmployee ;
        private Department transferDepartment;


        #region Конструктор
        /// <summary>
        /// При запуске приложения
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            BaseCheck();
        }
        #endregion

        /// <summary>
        /// Первый запуск окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartTransferButton.Visibility = Visibility.Hidden;

            mainorg_expanded();
        }
        /// <summary>
        /// Метод для проверки наличая базы при старте программы
        /// </summary>
        private void BaseCheck()
        {
            if(File.Exists("base.json"))
            {
                repo = new OrganizationBase();
                //repo.RandomBaseGenerator();
                
                organization = repo.GetOrganization();
               //CalcSalary();
            }
            else
            {
                MessageBox.Show("База в месте по умолчанию не обнаруженна,введите название для организации!");
                repo = new OrganizationBase("base.json");
                organization = repo.GetOrganization();
                AddDepartment adddep = new AddDepartment(organization);
   
                if (adddep.ShowDialog() == true)
                {
                    mainorg_expanded();
                }
                
            }
            
        }
        /// <summary>
        /// Метод заполнения и установок для TreeView главного элемента
        /// </summary>
        public void mainorg_expanded()
        {
            var mainItem = new TreeViewItem
            {
                Header = organization.DepartmentName,
                DataContext = organization
            };
            mainItem.Selected += OrganizationTree_Selected;
            mainItem.Expanded += org_expanded;
            mainItem.Collapsed += org_colapsed;

            OrganizationTree.Items.Clear();
            OrganizationTree.Items.Add(mainItem);

        

            foreach (var item in organization.GetDepartmens())
            {
                var trItem = new TreeViewItem
                {
                    Header = item.DepartmentName,
                    DataContext = item
                };
                if (item.Departments.Count > 0)
                {
                    trItem.Items.Add(null);
                }
                trItem.Expanded += org_expanded;
                trItem.Selected += OrganizationTree_Selected;
                trItem.Collapsed += org_colapsed;
                mainItem.Items.Add(trItem);
            }
        }

        /// <summary>
        /// Метод отвечающий за обработку сворачивания ветвей TreeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void org_colapsed(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.Source;
            //if (item.Items.Count == 0)
            item.Items.Refresh();
            item.Items.Clear();
            item.Items.Add(null);
        }
        /// <summary>
        /// Метод отвечающий за обработку разворачиваний ветвей TreeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void org_expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = new TreeViewItem();
        
            item = (TreeViewItem)e.Source;
            
            if(item.Items.Count!=1 || item.Items[0] != null )
            {
                return;
            }
            Department dep = (Department)item.DataContext;
            
            item.Items.Clear();

                if(dep.Departments.Count>0)
                foreach (var subdep in dep.GetDepartmens())
                {
                        var newItem = new TreeViewItem()
                        {
                            Header = subdep.DepartmentName,
                            DataContext = subdep
                        };

                    newItem.Items.Add(null);
                    newItem.Expanded += org_expanded;
                    newItem.Selected += OrganizationTree_Selected;
                    newItem.Collapsed += org_colapsed;
                    item.Items.Add(newItem);
                }   
        }

        
        /// <summary>
        /// Метод который при выборе ветви , получает текущий департамент
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrganizationTree_Selected(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)e.OriginalSource;
            var dep = (Department)item.DataContext;
            ListView1.ItemsSource = dep.Employees;
            txt2.DataContext = item;
            
            txt1.DataContext = dep;

        }
        /// <summary>
        /// Метод обрабатывающий нажатие по кнопке и вызов окна добавления работника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            Department dep = new Department(null);
            if((Department) txt1.DataContext is Department)
            {
                dep = (Department)txt1.DataContext;
            }

            AddEmployee AddPageEmpl = new AddEmployee(dep)
            {
                Owner = this
            };
            AddPageEmpl.Show();
            repo.IsSaved = false;
        }
        /// <summary>
        /// Метод обрабатывающий нажатие по кнопке и добавление департамента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDepartment_Click(object sender, RoutedEventArgs e)
        {

            if(NameDepBox.Text == String.Empty)
            {
                MessageBox.Show("Укажите название для департамента");
            }
            else { 
            Department dep = new Department(null);
            if ((Department)txt1.DataContext is Department)
            {
                dep = (Department)txt1.DataContext;
            }
            dep.AddSubDepartment(new Department(NameDepBox.Text));
            var CurrentTree = (TreeViewItem)txt2.DataContext;
                if (CurrentTree is null) MessageBox.Show("Не выбран управляющий департамент");
                else { 
            if(CurrentTree.Items.Count==0)
            CurrentTree.Items.Add(null);

                    CurrentTree.IsExpanded = true;
                    CurrentTree.IsExpanded = false;
                    CurrentTree.IsExpanded = true;
                }
            }
            repo.IsSaved = false;
        }
        /// <summary>
        /// Удаление работника из отдела
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelWorkerButton_Click(object sender, RoutedEventArgs e)
        {

            var choise = MessageBox.Show("Уверены что хотите удалить данного сотрудника?", "Внимание", MessageBoxButton.YesNo);
            if (choise == MessageBoxResult.Yes)
            {
                var selectEmployee = (Employee)ListView1.SelectedItem;
                var dep = (Department)txt1.DataContext;

                dep.Employees.Remove(selectEmployee);
            }
            repo.IsSaved = false;
        }
        /// <summary>
        /// Удаление департамента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelDepartment_Click(object sender, RoutedEventArgs e)
        {

            var choise = MessageBox.Show("Уверены что хотите удалить данный департамент?", "Внимание", MessageBoxButton.YesNo);
            if (choise == MessageBoxResult.Yes)
            {
                var selectedDep = (TreeViewItem)OrganizationTree.SelectedItem;
                if (selectedDep.Parent is TreeViewItem)
                {
                    var parent = (TreeViewItem)selectedDep.Parent;
                    Department parentDep = (Department)parent.DataContext;
                    parentDep.Departments.Remove((Department)txt1.DataContext);
                    parent.IsExpanded = false;
                    parent.IsExpanded = true;
                }
                else
                {
                    choise = MessageBox.Show("Вы собираетесь удалить всю организацию.","Внимание",MessageBoxButton.YesNo);
                    if(choise == MessageBoxResult.Yes)
                    {
                        organization.Departments.Clear();
                        organization.DepartmentName = "Новый";
                        AddDepartment adddep = new AddDepartment(organization);
                        adddep.Owner = this;
                        if(adddep.ShowDialog() ==true)
                        {
                            mainorg_expanded();
                        }
                        
                        
                    }
                }
            }
            repo.IsSaved = false;


        }
        /// <summary>
        /// Метод обработки нажатия по клавище подсчета ЗП у начальников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalcSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            CalcSalary();
           // foreach (var item in organization.Departments) firstHightManager(item);
            repo.IsSaved = false;
        }
        /// <summary>
        /// Подсчет зп у начальников организации
        /// </summary>
        private void CalcSalary()
        {
            foreach (var el in organization.Employees)
            {
                if (el is HighManager)
                {
                    (el as HighManager).CalcSalary(organization);
                    // MessageBox.Show(Convert.ToString(el.Salary));
                    return;
                }

            }
        }

        private void firstHightManager(Department dep)
        {
            foreach (var item in dep.Departments)
            {
                foreach (var el in item.Employees)
                {
                    if (el is HighManager)
                    {
                        el.Salary = el.CalcSalary(organization);
                       // MessageBox.Show(Convert.ToString(el.Salary));
                        return;
                    }

                }
                firstHightManager(item);
            }
        }


        private void startFileDialogToLoad()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            Nullable<bool> result = ofd.ShowDialog();
            string path = ofd.FileName;
            if(path != string.Empty) repo.Load(path);
            organization = repo.GetOrganization();
            mainorg_expanded();
            repo.IsSaved = false;
        }

        private void LoadBaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (repo.IsSaved == false)
            {
                var choise = MessageBox.Show("Хотите сохранить изменения в текущей базе?", "Внимание", MessageBoxButton.YesNo);
                if (choise == MessageBoxResult.Yes)
                {
                    repo.Save();
                    startFileDialogToLoad();
                }
                else if(choise == MessageBoxResult.No)
                {
                    startFileDialogToLoad();
                }
            }
            else
            {
                startFileDialogToLoad();
            }

            }

        private void SaveBaseButton_Click(object sender, RoutedEventArgs e)
        {
            repo.Save();
        }

        private void SaveAsBaseButton_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog sfd = new SaveFileDialog();
            Nullable<bool> result = sfd.ShowDialog();
            string path = sfd.FileName;
            if (path != string.Empty) repo.Save(path);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (repo.IsSaved == false)
            {
                var choise = MessageBox.Show("Хотите сохранить изменения в текущей базе?", "Внимание", MessageBoxButton.YesNo);
                if (choise == MessageBoxResult.Yes)
                {
                    repo.Save();
                }
            }
                    this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            
            var header = sender as GridViewColumnHeader;
            var thisHeader = header.Content;

            var dep = txt1.DataContext as Department;
            if(dep!=null)
            switch ((thisHeader as TextBlock).Text)
            {
                case "Фамилия":
                    dep.Sort(Department.SortCriterion.LastName);
                    break;
                case "Имя":
                    dep.Sort(Department.SortCriterion.FristName);
                    break;
                case "Должность":
                    dep.Sort(Department.SortCriterion.Type);
                    break;
                case "Возраст(Лет)":
                    dep.Sort(Department.SortCriterion.Age);
                    break;
                case "Зарплата($)":
                    dep.Sort(Department.SortCriterion.Salary);
                    break;
                default:
                    dep.Sort(Department.SortCriterion.DateEmployee);
                    break;
            }
        }

        private void SelectWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            
            var choise = MessageBox.Show("Уверены c выбором сотрудника ?", "Внимание", MessageBoxButton.YesNo);
            if (choise == MessageBoxResult.Yes)
            {
                transferEmployee = (Employee)ListView1.SelectedItem;
                transferDepartment = (Department)txt1.DataContext;

                StartTransferButton.Visibility = Visibility.Visible;
                SelectWorkerButton.Visibility = Visibility.Hidden;
            }
            
        }

        private void StartTransferButton_Click(object sender, RoutedEventArgs e)
        {
            var currDep = (Department)txt1.DataContext;
            if (transferDepartment == currDep) MessageBox.Show("Выберите другой департамент для перевода сотрудника (нельзя переводить в тот же самый департамент)");
            else
            {
                currDep.AddWorker(transferEmployee);
                transferDepartment.Employees.Remove(transferEmployee);
                transferEmployee = null;
                transferDepartment = null;
                StartTransferButton.Visibility = Visibility.Hidden;
                SelectWorkerButton.Visibility = Visibility.Visible;
            }
            repo.IsSaved = false;
        }

        private void EditDepNameButton_Click(object sender, RoutedEventArgs e)
        {
            AddDepartment adddep = new AddDepartment((Department)txt1.DataContext);

            if (adddep.ShowDialog() == true)
            {
                mainorg_expanded();
                repo.IsSaved = false;
            }

        }
    }
}
