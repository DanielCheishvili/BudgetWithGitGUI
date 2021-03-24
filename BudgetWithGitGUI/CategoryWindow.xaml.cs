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
        private MainWindow parent;
        public CategoryWindow()
        {
            InitializeComponent();
            //this.homeBudget = homeBudget;
            TypeBox.ItemsSource = Enum.GetValues(typeof(Category.CategoryType));
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    parent = window as MainWindow;

                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (DescriptionBox.Text == "" || TypeBox.SelectedIndex == -1)
            {
                MessageBox.Show("One or more fields are empty");
            }        
            else
            {
                foreach (Category cat in parent.homeBudget_.categories.List())
                {
                    if (DescriptionBox.Text == cat.Description)
                    {
                        MessageBox.Show("The category you are trying to add already exists.");
                        return;
                    }
                   
                }
                parent.homeBudget_.categories.Add(DescriptionBox.Text, (Category.CategoryType)TypeBox.SelectedIndex + 1);
                MessageBox.Show($"Description: {DescriptionBox.Text}, Type: {(Category.CategoryType)TypeBox.SelectedItem}");


            }
           
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
