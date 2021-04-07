using Budget;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BudgetWithGitGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private string locationOfPreviousSave;
        //private bool isNewFile = true;
        private HomeBudget _homeBudget;
        public MainWindow()
        {
            InitializeComponent();
            FilterDataGrid();

            //Upon starting the application all the buttons except the open file one are invisible until you open the file.
            addCategory.IsEnabled = false;
            addExpense.IsEnabled = false;
            filterGB.IsEnabled = false;
            summaryGB.IsEnabled = false;
            fileName.Visibility = Visibility.Hidden;

        }
        public HomeBudget homeBudget_
        {
            get
            {
                return _homeBudget;
            }
            set
            {
                _homeBudget = value;
            }
        }

        private void FilterDataGrid()
        {
            CreateDataGrid();
            bool filterFlag = filterByCategoryCB.IsChecked == true;
            int id = 0;
            if (byCategoryCB.IsChecked == false && byMonthCB.IsChecked == false)
            {
                dataGrid.ItemsSource = null;
                ResetExpenseList();
                dataGrid.ItemsSource = homeBudget_.expenses.List();
            }
            if (byCategoryCB.IsChecked == false && byMonthCB.IsChecked == true)
            {
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = homeBudget_.GetBudgetItemsByMonth(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterFlag, id);
            }
            if (byCategoryCB.IsChecked == true && byMonthCB.IsChecked == false)
            {
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = homeBudget_.GetBudgetItemsByCategory(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterFlag, id);
            }
            if (byCategoryCB.IsChecked == true && byMonthCB.IsChecked == true)
            {
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = homeBudget_.GetBudgetDictionaryByCategoryAndMonth(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterFlag, id);
            }
        }
        private void CreateDataGrid()
        {
            //https://stackoverflow.com/questions/704724/programmatically-add-column-rows-to-wpf-datagrid

            dataGrid.Columns.Clear();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Date";
            column.Binding = new Binding("Date");
            column.Binding.StringFormat = "yyyy-MM-dd";
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Category";
            column.Binding = new Binding("Category");       
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Description";
            column.Binding = new Binding("Description");
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Amount";
            column.Binding = new Binding("Amount");
            column.Binding.StringFormat = "F2";
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Balance";
            column.Binding = new Binding("Balance");
            column.Binding.StringFormat = "F2";
            dataGrid.Columns.Add(column);

        }
            
        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            
            openFile.CheckFileExists = false;
            openFile.CheckPathExists = false;

            //opens the directory where the file was last used.
            openFile.RestoreDirectory = true;

            openFile.Filter = "DB Files|*.db";
            if (openFile.ShowDialog() == true)
            {       
                //opens the database file.
                _homeBudget = new HomeBudget(openFile.FileName, false);
                fileName.Text = "Using File: " + openFile.SafeFileName;
                
                //adds the categories to the drop down menu.
                categoryDropDownList.ItemsSource = _homeBudget.categories.List();
                ResetExpenseList();

            }
            else
            {
                return;
            }
            addCategory.IsEnabled = true;
            addExpense.IsEnabled = true;
            filterGB.IsEnabled = true;
            summaryGB.IsEnabled = true;
            //expenseDropDownList.Visibility = Visibility.Visible;
            fileName.Visibility = Visibility.Visible;
            openBtn.Visibility = Visibility.Hidden;
        }

        private void addExpenseBtn_Click(object sender, RoutedEventArgs e)
        {
            ExpenseWindow newExpWindow = new ExpenseWindow();
            newExpWindow.ShowDialog();
            //expenseDropDownList.ItemsSource = null;
            ResetExpenseList();

        }

        private void addCategoryBtn_Click(object sender, RoutedEventArgs e)
        {

            CategoryWindow newCatWindow = new CategoryWindow();
            newCatWindow.ShowDialog();
            categoryDropDownList.ItemsSource = null;
            categoryDropDownList.ItemsSource = _homeBudget.categories.List();
        }
        private void ResetExpenseList()
        {
            //adds only the description of the expense instead of the object itself.
            //clears the list so other stuff can be added.
            //expenseDropDownList.Items.Clear();
            foreach (Expense expense in _homeBudget.expenses.List())
            {
                //dataGrid.Items.Add(expense);
            }
        }

        private void expenseDropDownList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //checks for the selected item and once double click shows the full info of the expense
            ComboBox cmb = sender as ComboBox;
            if (cmb.SelectedItem != null)
            {
                foreach (Expense expense in _homeBudget.expenses.List())
                {

                    if (cmb.SelectedItem.ToString() == expense.Description)
                    {

                        MessageBox.Show($"Expense Description: {expense.Description}\n" +
                                           $"Amount: {expense.Amount}\n" +
                                           $"Category Type: {homeBudget_.categories.GetCategoryFromId(expense.Category)}\n" +
                                           $"Date: {expense.Date.ToString("yyyy-MM-dd")}");
                    }



                }
            }
            else
            {
                MessageBox.Show("Please select an item from the drop down list before double clicking", "Expense Drop down list", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            

        }

        private void categoryDropDownList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //checks for the selected item and once double click shows the full info of the category
            ComboBox cmb = sender as ComboBox;
            if (cmb.SelectedItem != null)
            {
                foreach (Category category in _homeBudget.categories.List())
                {
                    if (cmb.SelectedItem.ToString() == category.Description)
                    {
                        MessageBox.Show($"Category Description: {category.Description}\n" +
                                           $"Type: {category.Type}");
                    }


                }
            }
            else
            {
                MessageBox.Show("Please select an item from the drop down list before double clicking", "Category Drop down list", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }
    }
}
