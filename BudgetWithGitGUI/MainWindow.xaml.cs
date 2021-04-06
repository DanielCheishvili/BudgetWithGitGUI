using Budget;
using Microsoft.Win32;
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
            //expenseDropDownList.Visibility = Visibility.Hidden;
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
                //expenseDropDownList.Items.Add(expense.Description);
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
