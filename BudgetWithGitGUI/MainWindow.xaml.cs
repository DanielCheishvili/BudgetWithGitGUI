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
        private string locationOfPreviousSave;
        private bool isNewFile = true;
        HomeBudget homeBudget;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "DB Files |*.db";
            if(isNewFile)
            {
                if (saveFile.ShowDialog() == true)
                {
                    locationOfPreviousSave = saveFile.FileName;
                    isNewFile = false;
                }
                else
                {
                    return;
                }
            }
            
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "DB Files|*.db";
            if(openFile.ShowDialog() == true)
            {
                homeBudget = new HomeBudget(locationOfPreviousSave, false);
            }
            else
            {
                return;
            }
        }

        private void addExpenseBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addCategoryBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
