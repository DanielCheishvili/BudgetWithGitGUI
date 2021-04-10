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
    
    //This class is in charge of dealing with the datagrid and formatting properly
    public partial class MainWindow : Window
    {
        private void UpdateDataGrid()
        {

            bool filterFlag = false;
            int id = -1;
            if (categoryDropDownList.SelectedIndex > -1)
            {
                id = ((Category)categoryDropDownList.SelectedItem).Id;
            }
            if (filterByCategoryCB.IsChecked == true)
            {
                filterFlag = true;
            }
            if (byCategoryCB.IsChecked == true || byMonthCB.IsChecked == true)
            {
                modifySelect.IsEnabled = false;
                DeleteSelect.IsEnabled = false;


            }
            else
            {
                modifySelect.IsEnabled = true;
                DeleteSelect.IsEnabled = true;
            }
            if(startDatePicker.SelectedDate > endDatePicker.SelectedDate)
            {
                endDatePicker.SelectedDate = null;
                MessageBox.Show("The end date cannot be earlier than the start date", "Date Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }



            if (byCategoryCB.IsChecked == false && byMonthCB.IsChecked == false)
            {

                CreateDefaultDataGrid();
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = homeBudget_.GetBudgetItems(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterFlag, id);
            }

            if (byCategoryCB.IsChecked == false && byMonthCB.IsChecked == true)
            {
                CreateSummaryByMonthGrid();
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = homeBudget_.GetBudgetItemsByMonth(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterFlag, id);
            }
            if (byCategoryCB.IsChecked == true && byMonthCB.IsChecked == false)
            {
                CreateSummaryByCategoryGrid();
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = homeBudget_.GetBudgetItemsByCategory(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterFlag, id);
            }
            if (byCategoryCB.IsChecked == true && byMonthCB.IsChecked == true)
            {

                CreateSummaryByCategoryAndMonthGrid();
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = homeBudget_.GetBudgetDictionaryByCategoryAndMonth(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterFlag, id);

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
            column.Binding = new Binding("ShortDescription");
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
            column.Binding.StringFormat = "F2";
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
            column.Binding.StringFormat = "F2";
            dataGrid.Columns.Add(column);
        }
        private void CreateSummaryByCategoryAndMonthGrid()
        {
            dataGrid.Columns.Clear();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Month";
            column.Binding = new Binding("[Month]");
            column.Binding.StringFormat = "yyyy-MM";
            dataGrid.Columns.Add(column);

            foreach (Category category in homeBudget_.categories.List())
            {
                column = new DataGridTextColumn();
                column.Header = category.Description;
                column.Binding = new Binding("[" + category.Description + "]");
                dataGrid.Columns.Add(column);
            }
            column = new DataGridTextColumn();
            column.Header = "Total";
            column.Binding = new Binding("[Total]");
            column.Binding.StringFormat = "F2";
            dataGrid.Columns.Add(column);

        }
    }
    public partial class MainWindow : Window
    {
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
            contextMenu.IsEnabled = false;

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

        #region buttons and checkboxes
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
            }
            else
            {
                return;
            }
            addCategory.IsEnabled = true;
            addExpense.IsEnabled = true;
            filterGB.IsEnabled = true;
            summaryGB.IsEnabled = true;
            fileName.Visibility = Visibility.Visible;
            openBtn.Visibility = Visibility.Hidden;
            contextMenu.IsEnabled = true;
            UpdateDataGrid();
        }
        #region buttons&checkboxes
        private void addExpenseBtn_Click(object sender, RoutedEventArgs e)
        {
            ExpenseWindow newExpWindow = new ExpenseWindow(homeBudget_,false,-1);
            newExpWindow.ShowDialog();
            UpdateDataGrid();
         
        }

        private void addCategoryBtn_Click(object sender, RoutedEventArgs e)
        {

            CategoryWindow newCatWindow = new CategoryWindow();
            newCatWindow.ShowDialog();
            categoryDropDownList.ItemsSource = null;
            categoryDropDownList.ItemsSource = _homeBudget.categories.List();
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
           
            UpdateDataGrid();
        }

        private void byCategoryCB_Checked(object sender, RoutedEventArgs e)
        {
            
            UpdateDataGrid();
        }
        #endregion

        private void filterByCategoryCB_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void startDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void endDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void categoryDropDownList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(filterByCategoryCB.IsChecked == true)
            {
                UpdateDataGrid();
            }
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(byCategoryCB.IsChecked == true || byMonthCB.IsChecked == true)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                ModifyExpenseForm();
            }
           
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ModifyExpenseForm();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            BudgetItem item = dataGrid.SelectedItem as BudgetItem;
            ExpenseWindow modifyWin = new ExpenseWindow(homeBudget_, false, item.ExpenseID);
            homeBudget_.expenses.Delete(item.ExpenseID);
            UpdateDataGrid();
        }
        #endregion
        private void ModifyExpenseForm()
        {
            
            BudgetItem item = dataGrid.SelectedItem as BudgetItem;

            if (item != null)
            {
                ExpenseWindow modifyWin = new ExpenseWindow(homeBudget_, true, item.ExpenseID);
                modifyWin.datePicker1.SelectedDate = item.Date;
                modifyWin.categoryList.SelectedIndex = item.CategoryID - 1;
                modifyWin.amountText.Text = item.Amount.ToString();
                modifyWin.descriptionText.Text = item.ShortDescription;
                modifyWin.ShowDialog();
                UpdateDataGrid();

            }
            else
                return;
           
        }
    }
}
