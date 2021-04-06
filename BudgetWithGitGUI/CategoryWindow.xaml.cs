﻿using Budget;
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
                //checks if a category with the same description exists.
                foreach (Category cat in parent.homeBudget_.categories.List())
                {
                    if (DescriptionBox.Text == cat.Description)
                    {
                        MessageBox.Show("The category you are trying to add already exists.");
                        return;
                    }
                   
                }
                
                MessageBoxResult results = MessageBox.Show($"You are adding the following category:\n" +
                    $"Description: {DescriptionBox.Text}\n" +
                    $"Type: {(Category.CategoryType)TypeBox.SelectedItem}\n" +
                    $"do you wish the proceed?", "Category", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if(MessageBoxResult.Yes == results)
                {
                   
                    parent.homeBudget_.categories.Add(DescriptionBox.Text, (Category.CategoryType)TypeBox.SelectedIndex + 1);
                }
                else
                {
                    return;
                }

            }
            DescriptionBox.Text = "";
            TypeBox.SelectedIndex = -1;
           
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult results = MessageBox.Show("Do you wish to cancel this operation?", "Category", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(results == MessageBoxResult.Yes)
            {
                Close();
            }
            else
            {
                return;
            }

        }

    }
}
