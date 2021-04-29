using Budget;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;

namespace BudgetWithGitGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window, IDataView
    {
        private HomeBudget _homeBudget;
        private int currentIndex;
        private DataPresenter presenter;
        public MainWindow()
        {
            InitializeComponent();

            addCategory.IsEnabled = false;
            addExpense.IsEnabled = false;
            filterGB.IsEnabled = false;
            summaryGB.IsEnabled = false;
            fileName.Visibility = Visibility.Hidden;
            contextMenu.IsEnabled = false;
            searchBox.IsEnabled = false;
            searchBtn.IsEnabled = false;
            presenter = new DataPresenter(this);


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

        public DataPresenter presnter
        {
            get
            {
                return presenter;
            }
            set
            {
                presenter = value;
            }
        }
        public List<Object> DataSource 
        {
            get
            {

                return dataGrid.ItemsSource.Cast<object>().ToList();
            }
            set
            {
                dataGrid.ItemsSource = value;
            }
        }
        #region DataGrid
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
                searchBox.IsEnabled = true;
                searchBtn.IsEnabled = true;
            }
            if (byCategoryCB.IsChecked == true || byMonthCB.IsChecked == true)
            {
                modifySelect.IsEnabled = false;
                DeleteSelect.IsEnabled = false;
                searchBox.IsEnabled = false;
                searchBtn.IsEnabled = false;


            }
            else
            {
                modifySelect.IsEnabled = true;
                DeleteSelect.IsEnabled = true;
                searchBox.IsEnabled = true;
                searchBtn.IsEnabled = true;
            }
            if (startDatePicker.SelectedDate > endDatePicker.SelectedDate)
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
        public void CreateDefaultDataGrid()
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
            Style style = new Style();
            style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            column.CellStyle = style;
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Balance";
            column.Binding = new Binding("Balance");
            column.Binding.StringFormat = "F2";
            column.CellStyle = style;
            dataGrid.Columns.Add(column);

        }
        public void CreateSummaryByMonthGrid()
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
            Style style = new Style();
            style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            column.CellStyle = style;
            dataGrid.Columns.Add(column);
        }
        public void CreateSummaryByCategoryGrid()
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
            Style style = new Style();
            style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            column.CellStyle = style;
            dataGrid.Columns.Add(column);
        }
        public void CreateSummaryByCategoryAndMonthGrid()
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
                Style style = new Style();
                style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
                column.CellStyle = style;
                dataGrid.Columns.Add(column);
            }
            column = new DataGridTextColumn();
            column.Header = "Total";
            column.Binding = new Binding("[Total]");
            column.Binding.StringFormat = "F2";
            Style style2 = new Style();
            style2.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            column.CellStyle = style2;
            dataGrid.Columns.Add(column);

        }
        #endregion
        #region Events
        private void openBtn_Click(object sender, RoutedEventArgs e)
        {  
            OpenFileDialog openFile = new OpenFileDialog();

            string fileFilter = "DB Files|*.db";

            openFile.Title = "Open/Create Homebudget";

            openFile.CheckFileExists = false;
            openFile.CheckPathExists = false;

            //opens the directory where the file was last used.
            openFile.RestoreDirectory = true;

            openFile.Filter = fileFilter;
            

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
            contextMenu.IsEnabled = true;
            
            UpdateDataGrid();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (homeBudget_ != null)
                homeBudget_.CloseDB();
        }

        private void addExpenseBtn_Click(object sender, RoutedEventArgs e)
        {
            ExpenseWindow newExpWindow = new ExpenseWindow(homeBudget_, false, -1);
            newExpWindow.ShowDialog();
            UpdateDataGrid();
            ResetFocus(dataGrid.Items.Count - 1);
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
           
            presenter.FiltersHaveChanged(startDatePicker.SelectedDate, endDatePicker.SelectedDate,filterByCategoryCB.IsChecked == true, dataGrid.SelectedIndex, byMonthCB.IsChecked == true );
        }

        private void byCategoryCB_Checked(object sender, RoutedEventArgs e)
        {

            UpdateDataGrid();
        }


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
            if (filterByCategoryCB.IsChecked == true)
            {
                UpdateDataGrid();
            }
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (byCategoryCB.IsChecked == true || byMonthCB.IsChecked == true)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                ModifyExpenseForm();
            }


        }

        //modifying an expense on right click
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ModifyExpenseForm();
        }

        //delete an expense on right click
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            BudgetItem item = dataGrid.SelectedItem as BudgetItem;
            int temp = dataGrid.SelectedIndex;
            ExpenseWindow modifyWin = new ExpenseWindow(homeBudget_, false, item.ExpenseID);
            homeBudget_.expenses.Delete(item.ExpenseID);    
            UpdateDataGrid();
            ResetFocus(temp);



        }
        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchInDataGrid();
        }
        private void searchBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(dataGrid.SelectedItem != null)
                currentIndex = dataGrid.SelectedIndex;
            else
                currentIndex = 0;
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentIndex = dataGrid.SelectedIndex;
        }
        #endregion

        #region Helper Functions
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
        public void DataClear()
        {
        }
        public void SearchInDataGrid()
        {
            if (searchBox.Text.Equals(""))
                return;
            
            int count = 0;   
            
            //iterates through all the data grid items
            for (int i = currentIndex; i < dataGrid.Items.Count; i++)
            {

                BudgetItem item = dataGrid.Items.GetItemAt(i) as BudgetItem;
                dataGrid.SelectedItem = item;

                //if the string matches the description or the amount of current budget item then it
                //highlights it and scrolls into view
                if (item.ShortDescription.ToLower().Contains(searchBox.Text.ToLower()) || item.Amount.ToString("F").Contains(searchBox.Text))
                {
                    dataGrid.Focus();
                    dataGrid.ScrollIntoView(dataGrid.SelectedItem);

                    //assigns the last value so next time the method is called
                    //the loop starts on the index after the previous found one.
                    currentIndex = ++i;

                    //if the next index is out of bounds resets to 0
                    if(currentIndex == dataGrid.Items.Count)
                    {
                        currentIndex = 0;
                    }
                    return;
                }
                //if the loop iterates through the entire datagrid and doesnt find a match
                else if (count == dataGrid.Items.Count)
                {
                    MessageBox.Show("Match Results came back negative", "Search not found", MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }
                count++;

                //if it reaches the end the it resets the index to -1 because in the for loop
                //it does the i++ after therefore if its = to 0 then it would be actually = to 1
                if (i + 1 == dataGrid.Items.Count)
                {
                    i = -1;
                }


            }

        }

        //sandys code
        public void ResetFocus(int index)
        {
            if (index >= dataGrid.Items.Count || index < 0)
            {
                index = dataGrid.Items.Count - 1;
            }
            dataGrid.SelectedIndex = index;
            dataGrid.Focus();

            if (index != -1)
            {
                dataGrid.CurrentCell = new DataGridCellInfo(dataGrid.Items[index], dataGrid.Columns[0]);
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
            }
        }
        #endregion

    }
}
