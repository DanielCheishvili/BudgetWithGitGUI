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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Budget;
using System.Data.SQLite;
using Microsoft.Win32;

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
            if(openFile.ShowDialog() == true)
            {
                _homeBudget = new HomeBudget(openFile.FileName, false);
            }
            else
            {
                return;
            }
            addCategory.Visibility = Visibility.Visible;
            addExpense.Visibility = Visibility.Visible;
        }

        private void addExpenseBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            CategoryWindow newCatWindow = new CategoryWindow();
            newCatWindow.ShowDialog();
        }

        private void categoryDropDownList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CategoryWindow categoryWindow = categoryDropDownList.SelectedItem as CategoryWindow;

        }

        private void categoryDropDownList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
