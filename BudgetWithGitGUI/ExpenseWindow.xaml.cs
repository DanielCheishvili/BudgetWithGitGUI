using System;
using System.Windows;
using System.Windows.Input;
using Budget;

namespace BudgetWithGitGUI
{
    /// <summary>
    /// Interaction logic for ExpenseWindow.xaml
    /// </summary>
    public partial class ExpenseWindow : Window
    {
        private MainWindow parent;
        public ExpenseWindow(HomeBudget budget, bool isModified, int expenseId )
        {
            InitializeComponent();
            datePicker1.SelectedDate = DateTime.Today;
            descriptionText.Text = "";
            amountText.Text = "";
            buttonUpdate.Visibility = Visibility.Hidden;
            buttonDelet.Visibility = Visibility.Hidden;

            if(isModified == true && expenseId !=-1)
            {

                Title = "ModifyExpense";
                buttonUpdate.Visibility = Visibility.Visible;
                buttonDelet.Visibility = Visibility.Visible;
                buttonSave.Visibility = Visibility.Hidden;
                titleOfWindow.Text = "Modify Expense";
                budget.expenses.UpdateProperties(expenseId, Convert.ToDateTime(datePicker1.SelectedDate), categoryList.SelectedIndex + 1, -Convert.ToDouble(amountText.Text), descriptionText.Text);


            }

            //Gets the categories from the main window list, and adds it as an option to add to the expense.
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    parent = window as MainWindow;
                    categoryList.ItemsSource = parent.homeBudget_.categories.List();
                }
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult results = MessageBox.Show("Are you sure you want to cancel ?", "Home Budget", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if(MessageBoxResult.Yes == results)
            {
                Close();
            }
            else
            {
                return;
            }

        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {

            InputFields();

        }
        public void InputFields()
        {
            //Add Receipt , summarize form
            if (descriptionText.Text == "")
            {
                MessageBox.Show("The Description Text is missing ", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (amountText.Text == "" || !double.TryParse(amountText.Text, out double amount))
            {
                MessageBox.Show("The amount is missing or it is not a number", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (categoryList.Text == "")
            {
                MessageBox.Show("The category type is missing", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (datePicker1.SelectedDate == null)
            {
                MessageBox.Show("The date is missing", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Category category = parent.homeBudget_.categories.GetCategoryFromId(categoryList.SelectedIndex + 1);

                DateTime datetime = datePicker1.SelectedDate ?? DateTime.Now;
                MessageBoxResult result = MessageBox.Show($@"You are adding the following Expense:
                                Description: {descriptionText.Text}
                                Amount: {amountText.Text}
                                Category: {categoryList.SelectedItem}
                                Date: {datetime.ToString("yyyy-MM-dd")}",
                    "Home Budget", MessageBoxButton.YesNo, MessageBoxImage.Information);


                if (result == MessageBoxResult.Yes)
                {
                    if ((int)category.Type == 2 || (int)category.Type == 4)
                    {
                        parent.homeBudget_.expenses.Add(Convert.ToDateTime(datePicker1.SelectedDate), categoryList.SelectedIndex + 1, -Convert.ToDouble(amountText.Text), descriptionText.Text);
                    }
                    if ((int)category.Type == 1 || (int)category.Type == 3)
                    {
                        parent.homeBudget_.expenses.Add(Convert.ToDateTime(datePicker1.SelectedDate), categoryList.SelectedIndex + 1, Convert.ToDouble(amountText.Text), descriptionText.Text);

                    }

                    descriptionText.Text = "";
                    amountText.Text = "";

                }
                else
                {
                    return;
                }
            }
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonDelet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
