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
using Budget;
using System.Data.SQLite;

namespace BudgetWithGitGUI
{
    /// <summary>
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {
        HomeBudget homeBudget;
        public CategoryWindow(ref HomeBudget homeBudget)
        {
            InitializeComponent();
            //this.homeBudget = homeBudget;
            TypeBox.ItemsSource = Enum.GetValues(typeof (Category.CategoryType));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (DescriptionBox.Text == "" || TypeBox.SelectedIndex == -1)
            {
                MessageBox.Show("One or more fields are empty");
            }
            else
            {
                homeBudget.categories.Add(DescriptionBox.Text, (Category.CategoryType)TypeBox.SelectedIndex);
                MessageBox.Show($"Description: {DescriptionBox.Text}, Type: {(Category.CategoryType)TypeBox.SelectedItem}");
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
