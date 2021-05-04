using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace BudgetWithGitGUI
{
    public class DataPresenter
    {
        public IDataView IView;
        public HomeBudget homeBudget;
        public MainWindow main;

        

        public DataPresenter(MainWindow mainWindow)
        {
            this.main = mainWindow;
            //this.IView = IView;
            //this.main = IView as MainWindow;
            homeBudget = new HomeBudget("F:/Winter_2021/App_dev/Assignments/DemoForClass.db");
            //homeBudget = new HomeBudget(main.FileNamePresenter);
        }

        public HomeBudget HomeBudget
        {
            get
            {
                return this.homeBudget;
            }
            set
            {
                homeBudget = value;
            }
        }
        public List<Object> DataSource
        {
            get
            {
                return IView.DataSource;
            }
            set
            {
                IView.DataSource = value;
            }
        }
        //loop over the list of categories and pass it in

        public void FiltersHaveChanged(DateTime? startDate, DateTime? endDate, bool filterFlag, int id, bool isMonthChecked,bool isCategoryChecked)
        {

            
            
            IView.CreateDefaultDataGrid();
            List<BudgetItem> budgetItems = HomeBudget.GetBudgetItems(startDate, endDate, filterFlag, id);
            DataSource = budgetItems.Cast<object>().ToList();           

            if (isMonthChecked && isCategoryChecked)
            {
                IView.CreateSummaryByCategoryAndMonthGrid();
                var bi = HomeBudget.GetBudgetDictionaryByCategoryAndMonth(startDate, endDate, filterFlag, id);
                DataSource = bi.Cast<object>().ToList();
            }
            else if (isMonthChecked && !isCategoryChecked)
            {
                IView.CreateSummaryByMonthGrid();
                List<BudgetItemsByMonth> bi = HomeBudget.GetBudgetItemsByMonth(startDate, endDate, filterFlag, id);
                DataSource = bi.Cast<object>().ToList();

            }
            else if(isCategoryChecked && !isMonthChecked)
            {
                IView.CreateSummaryByCategoryGrid();
                List<BudgetItemsByCategory> bi = HomeBudget.GetBudgetItemsByCategory(startDate, endDate, filterFlag, id);
                DataSource = bi.Cast<object>().ToList();
            }
            
            
            

        }


    }
}
