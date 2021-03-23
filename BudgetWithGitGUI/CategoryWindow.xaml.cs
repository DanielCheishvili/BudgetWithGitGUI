using Budget;
using System;
using System.Windows;

namespace BudgetWithGitGUI
{
    /// <summary>
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {

        public CategoryWindow()
        {
            InitializeComponent();
            TypeBox.ItemsSource = Enum.GetValues(typeof(Category.CategoryType));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (DescriptionBox.Text == "" || TypeBox.SelectedIndex == -1)
            {
                MessageBox.Show("One or more fields are empty");
            }
            else
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        (window as MainWindow).homeBudget_.categories.Add(DescriptionBox.Text, (Category.CategoryType)TypeBox.SelectedIndex);
                        MessageBox.Show($"Description: {DescriptionBox.Text}, Type: {(Category.CategoryType)TypeBox.SelectedItem}");
                        Close();
                    }
                }
                //(Window as MainWindow).homeBudget.categories.Add(DescriptionBox.Text, (Category.CategoryType)TypeBox.SelectedIndex);
                //MessageBox.Show($"Description: {DescriptionBox.Text}, Type: {(Category.CategoryType)TypeBox.SelectedItem}");
                //Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
