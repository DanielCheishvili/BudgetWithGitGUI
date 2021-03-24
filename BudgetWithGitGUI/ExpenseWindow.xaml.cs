using System;
using System.Windows;
using System.Windows.Input;

namespace BudgetWithGitGUI
{
    /// <summary>
    /// Interaction logic for ExpenseWindow.xaml
    /// </summary>
    public partial class ExpenseWindow : Window
    {
        private MainWindow parent;
        public ExpenseWindow()
        {
            InitializeComponent();
            datePicker1.SelectedDate = DateTime.Today;
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
            MessageBox.Show("Are you sure you want to cancel ?", "Home Budget", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            //Add Receipt , summarize form
            if (descriptionText.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (amountText.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (categoryList.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (datePicker1.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {

                MessageBox.Show("Would you like to add this data", "Home Budget", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);


                parent.homeBudget_.expenses.Add(Convert.ToDateTime(datePicker1.SelectedDate), categoryList.SelectedIndex + 1, Convert.ToDouble(amountText.Text), descriptionText.Text);
                Close();

            }

        }

        private void amountText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                amountText.Text = double.Parse(amountText.Text).ToString("c");
            }
        }
    }
}
