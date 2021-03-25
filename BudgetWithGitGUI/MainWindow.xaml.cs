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
            addCategory.Visibility = Visibility.Hidden;
            addExpense.Visibility = Visibility.Hidden;
            categoryDropDownList.Visibility = Visibility.Hidden;
            expenseDropDownList.Visibility = Visibility.Hidden;
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
            openFile.RestoreDirectory = true;
            openFile.Filter = "DB Files|*.db";
            if (openFile.ShowDialog() == true)
            {                
                _homeBudget = new HomeBudget(openFile.FileName, false);
                fileName.Text = "Using File: " + openFile.SafeFileName;
                categoryDropDownList.ItemsSource = _homeBudget.categories.List();
                ResetExpenseList();

            }
            else
            {
                return;
            }
            addCategory.Visibility = Visibility.Visible;
            addExpense.Visibility = Visibility.Visible;
            categoryDropDownList.Visibility = Visibility.Visible;
            expenseDropDownList.Visibility = Visibility.Visible;
            fileName.Visibility = Visibility.Visible;
            openBtn.Visibility = Visibility.Hidden;

        }

        private void addExpenseBtn_Click(object sender, RoutedEventArgs e)
        {
            ExpenseWindow newExpWindow = new ExpenseWindow();
            newExpWindow.ShowDialog();
            expenseDropDownList.ItemsSource = null;
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
            expenseDropDownList.Items.Clear();
            foreach (Expense expense in _homeBudget.expenses.List())
            {
                expenseDropDownList.Items.Add(expense.Description);
            }
        }

        private void expenseDropDownList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

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
