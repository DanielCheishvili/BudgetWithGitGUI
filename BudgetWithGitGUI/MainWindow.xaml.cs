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
        #region creating grids
        private void FilterDataGrid()
        {
            dataGrid.Columns.Clear();
            
            bool filterFlag = false;
            int id = 0;
            if(categoryDropDownList.SelectedIndex > -1)
            {
                id = categoryDropDownList.SelectedIndex;
            }          
            if (filterByCategoryCB.IsChecked == true)
            {
                filterFlag = true;
            }

            
            if (byCategoryCB.IsChecked == false && byMonthCB.IsChecked == false)
            {
                CreateDefaultDataGrid();
                dataGrid.ItemsSource = null;
                
                /*ResetExpenseList();
                dataGrid.ItemsSource = homeBudget_.expenses.List();*/
            }
            if (byCategoryCB.IsChecked == false && byMonthCB.IsChecked == true)
            {
                CreateSummaryByMonthGrid();
                dataGrid.ItemsSource = null;
                //dataGrid.ItemsSource = homeBudget_.GetBudgetItemsByMonth(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterFlag, id);
            }
            if (byCategoryCB.IsChecked == true && byMonthCB.IsChecked == false)
            {
                CreateSummaryByCategoryGrid();
                dataGrid.ItemsSource = null;
                /*dataGrid.ItemsSource = homeBudget_.GetBudgetItemsByCategory(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterFlag, id);*/
            }
            if (byCategoryCB.IsChecked == true && byMonthCB.IsChecked == true)
            {
                CreateSummaryByCategoryAndMonthGrid();
                dataGrid.ItemsSource = null;
                /*dataGrid.ItemsSource = homeBudget_.GetBudgetDictionaryByCategoryAndMonth(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterFlag, id);*/
            }
        }
        
        //https://stackoverflow.com/questions/704724/programmatically-add-column-rows-to-wpf-datagrid
        private void CreateDefaultDataGrid()
        {
            
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
            column.Binding.StringFormat = "C2";
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Balance";
            column.Binding = new Binding("Balance");
            column.Binding.StringFormat = "C2";
            dataGrid.Columns.Add(column);

        }
        private void CreateSummaryByMonthGrid()
        {
            dataGrid.Columns.Clear();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Month";
            column.Binding = new Binding("Month");
            column.Binding.StringFormat = "yyyy-MM";
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Total";
            column.Binding = new Binding("Total");
            column.Binding.StringFormat = "C2";
            dataGrid.Columns.Add(column);
        }
        private void CreateSummaryByCategoryGrid()
        {
            dataGrid.Columns.Clear();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Category";
            column.Binding = new Binding("Category");           
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Total";
            column.Binding = new Binding("Total");
            column.Binding.StringFormat = "C2";
            dataGrid.Columns.Add(column);
        }
        private void CreateSummaryByCategoryAndMonthGrid()
        {
            dataGrid.Columns.Clear();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Month";
            column.Binding = new Binding("Month");
            column.Binding.StringFormat = "yyyy-MM";
            dataGrid.Columns.Add(column);

            foreach(Category category in homeBudget_.categories.List())
            {
                column = new DataGridTextColumn();
                column.Header = category.Description;
                column.Binding = new Binding(category.Description);
                dataGrid.Columns.Add(column);
            }
            column = new DataGridTextColumn();
            column.Header = "Total";
            column.Binding = new Binding("Total");
            column.Binding.StringFormat = "C2";
            dataGrid.Columns.Add(column);
            
        }
        #endregion

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
            FilterDataGrid();
        }
        #region buttons&checkboxes
        private void addExpenseBtn_Click(object sender, RoutedEventArgs e)
        {
            ExpenseWindow newExpWindow = new ExpenseWindow();
            newExpWindow.ShowDialog();
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

        private void byMonthCB_Checked(object sender, RoutedEventArgs e)
        {
           
            FilterDataGrid();
        }

        private void byCategoryCB_Checked(object sender, RoutedEventArgs e)
        {
            
            FilterDataGrid();
        }
        #endregion
    }
}
