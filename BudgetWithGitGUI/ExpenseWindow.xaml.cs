using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BudgetWithGitGUI
{
    /// <summary>
    /// Interaction logic for ExpenseWindow.xaml
    /// </summary>
    public partial class ExpenseWindow : Window
    {
        public ExpenseWindow(ref Budget.HomeBudget homeBudget)
        {
            InitializeComponent();
            datePicker1.SelectedDate = DateTime.Today;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Are you sure you want to cancel ?", "Home Budget", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);

        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            //Add Receipt , summarize form
            if (descriptionText.Text == "" )
            {
                MessageBox.Show("Please fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (amountText.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (categoryText.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if(datePicker1.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                
                MessageBox.Show("Would you like to add this data", "Home Budget", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            }
            
        }

        private void amountText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                amountText.Text = double.Parse(amountText.Text).ToString("c");
            }
        }
    }
}
