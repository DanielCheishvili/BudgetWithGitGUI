using Budget;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BudgetWithGitGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private HomeBudget _homeBudget;
        private int currentIndex;
        private DataPresenter presenter;
        public string fileNamePresenter;
        public DataView datagridView;

        //make a getter for filename. public, call it datapresenter
        public MainWindow()
        {
            InitializeComponent();

            addCategory.IsEnabled = false;
            addExpense.IsEnabled = false;
            filterGB.IsEnabled = false;
            summaryGB.IsEnabled = false;
            fileName.Visibility = Visibility.Hidden;
            dataGridMainWindow.contextMenu.IsEnabled = false;
            searchBox.IsEnabled = false;
            searchBtn.IsEnabled = false;
           


        }
        public string FileNamePresenter
        {
            get
            {
                return fileNamePresenter;
            }
            set
            {
                fileNamePresenter = value;
            }
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
                
                fileName.Text = openFile.SafeFileName;
                FileNamePresenter = fileName.Text;
                
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
            dataGridMainWindow.contextMenu.IsEnabled = true;
            searchBox.IsEnabled = true;
            searchBtn.IsEnabled = true;

            datagridView = dataGridMainWindow;
            presenter = new DataPresenter(_homeBudget, datagridView);
            datagridView.presnter = presnter;
            presenter.main = this;

            presenter.FiltersHaveChanged(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterByCategoryCB.IsChecked == true, -1, byMonthCB.IsChecked == true, byCategoryCB.IsChecked == true);

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
            presenter.FiltersHaveChanged(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterByCategoryCB.IsChecked == true, -1, byMonthCB.IsChecked == true, byCategoryCB.IsChecked == true);
            ResetFocus(dataGridMainWindow.dataGrid.Items.Count - 1);
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

        private void FilterAndSummaryCheck_Checked(object sender, RoutedEventArgs e)
        {
            bool filterFlag = false;

            if (filterByCategoryCB.IsChecked == true)
            {
                filterFlag = true;
                searchBox.IsEnabled = true;
                searchBtn.IsEnabled = true;
            }
            if (byCategoryCB.IsChecked == true || byMonthCB.IsChecked == true)
            {
                dataGridMainWindow.modifySelect.IsEnabled = false;
                dataGridMainWindow.DeleteSelect.IsEnabled = false;
                searchBox.IsEnabled = false;
                searchBtn.IsEnabled = false;


            }
            else
            {
                dataGridMainWindow.modifySelect.IsEnabled = true;
                dataGridMainWindow.DeleteSelect.IsEnabled = true;
                searchBox.IsEnabled = true;
                searchBtn.IsEnabled = true;
            }
            if (startDatePicker.SelectedDate > endDatePicker.SelectedDate)
            {
                endDatePicker.SelectedDate = null;
                MessageBox.Show("The end date cannot be earlier than the start date", "Date Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            presenter.FiltersHaveChanged(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterFlag, -1, byMonthCB.IsChecked == true, byCategoryCB.IsChecked == true);
        }


        private void startDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            presenter.FiltersHaveChanged(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterByCategoryCB.IsChecked == true, -1, byMonthCB.IsChecked == true, byCategoryCB.IsChecked == true);

        }

        private void endDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            presenter.FiltersHaveChanged(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterByCategoryCB.IsChecked == true, -1, byMonthCB.IsChecked == true, byCategoryCB.IsChecked == true);

        }

        //BUG: after re checking the 
        private void categoryDropDownList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = -1;
            if (categoryDropDownList.SelectedIndex > -1)
            {
                id = ((Category)categoryDropDownList.SelectedItem).Id;
            }
            presenter.FiltersHaveChanged(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterByCategoryCB.IsChecked == true, id, byMonthCB.IsChecked == true, byCategoryCB.IsChecked == true);


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
        public void DeleteItem()
        {
            BudgetItem item = dataGridMainWindow.dataGrid.SelectedItem as BudgetItem;
            int temp = dataGridMainWindow.dataGrid.SelectedIndex;
            ExpenseWindow modifyWin = new ExpenseWindow(homeBudget_, false, item.ExpenseID);
            homeBudget_.expenses.Delete(item.ExpenseID);
            presenter.FiltersHaveChanged(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterByCategoryCB.IsChecked == true, dataGridMainWindow.dataGrid.SelectedIndex, byMonthCB.IsChecked == true, byCategoryCB.IsChecked == true);
            ResetFocus(temp);
        }
        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchInDataGrid();
        }
        private void searchBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (dataGridMainWindow.dataGrid.SelectedItem != null)
                currentIndex = dataGridMainWindow.dataGrid.SelectedIndex;
            else
                currentIndex = 0;
        }
        //BUG HERE
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentIndex = dataGridMainWindow.dataGrid.SelectedIndex;
        }
        #endregion

        #region Helper Functions
        public void ModifyExpenseForm()
        {

            BudgetItem item = dataGridMainWindow.dataGrid.SelectedItem as BudgetItem;

            if (item != null)
            {
                ExpenseWindow modifyWin = new ExpenseWindow(homeBudget_, true, item.ExpenseID);
                modifyWin.datePicker1.SelectedDate = item.Date;
                modifyWin.categoryList.SelectedIndex = item.CategoryID - 1;
                modifyWin.amountText.Text = item.Amount.ToString();
                modifyWin.descriptionText.Text = item.ShortDescription;
                modifyWin.ShowDialog();
                presenter.FiltersHaveChanged(startDatePicker.SelectedDate, endDatePicker.SelectedDate, filterByCategoryCB.IsChecked == true, dataGridMainWindow.dataGrid.SelectedIndex, byMonthCB.IsChecked == true, byCategoryCB.IsChecked == true);

            }
            else
                return;

        }
        public void SearchInDataGrid()
        {
            if (searchBox.Text.Equals(""))
                return;

            int count = 0;

            //iterates through all the data grid items
            for (int i = currentIndex; i < dataGridMainWindow.dataGrid.Items.Count; i++)
            {

                BudgetItem item = dataGridMainWindow.dataGrid.Items.GetItemAt(i) as BudgetItem;
                dataGridMainWindow.dataGrid.SelectedItem = item;

                //if the string matches the description or the amount of current budget item then it
                //highlights it and scrolls into view
                if (item.ShortDescription.ToLower().Contains(searchBox.Text.ToLower()) || item.Amount.ToString("F").Contains(searchBox.Text))
                {
                    dataGridMainWindow.dataGrid.Focus();
                    dataGridMainWindow.dataGrid.ScrollIntoView(dataGridMainWindow.dataGrid.SelectedItem);

                    //assigns the last value so next time the method is called
                    //the loop starts on the index after the previous found one.
                    currentIndex = ++i;

                    //if the next index is out of bounds resets to 0
                    if (currentIndex == dataGridMainWindow.dataGrid.Items.Count)
                    {
                        currentIndex = 0;
                    }
                    return;
                }
                //if the loop iterates through the entire datagrid and doesnt find a match
                else if (count == dataGridMainWindow.dataGrid.Items.Count)
                {
                    MessageBox.Show("The Item you are looking for cannot be found", "Search not found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                count++;

                //if it reaches the end the it resets the index to -1 because in the for loop
                //it does the i++ after therefore if its = to 0 then it would be actually = to 1
                if (i + 1 == dataGridMainWindow.dataGrid.Items.Count)
                {
                    i = -1;
                }


            }

        }

        //sandys code
        public void ResetFocus(int index)
        {
            if (index >= dataGridMainWindow.dataGrid.Items.Count || index < 0)
            {
                index = dataGridMainWindow.dataGrid.Items.Count - 1;
            }
            dataGridMainWindow.dataGrid.SelectedIndex = index;
            dataGridMainWindow.dataGrid.Focus();

            if (index != -1)
            {
                dataGridMainWindow.dataGrid.CurrentCell = new DataGridCellInfo(dataGridMainWindow.dataGrid.Items[index], dataGridMainWindow.dataGrid.Columns[0]);
                dataGridMainWindow.dataGrid.ScrollIntoView(dataGridMainWindow.dataGrid.SelectedItem);
            }
        }
        #endregion
    }
}
