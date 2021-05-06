using Budget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetWithGitGUI
{
    public class DataPresenter
    {
        public IDataView IView;
        public HomeBudget homeBudget;
        public MainWindow main;

        public DataPresenter(HomeBudget homeBudget, IDataView dataView)
        {
            this.IView = dataView;
            this.homeBudget = homeBudget;
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

        public void FiltersHaveChanged(DateTime? startDate, DateTime? endDate, bool filterFlag, int id, bool isMonthChecked, bool isCategoryChecked)
        {
            if (!isCategoryChecked && !isMonthChecked)
            {
                IView.InitializeStandardDisplay();
                List<BudgetItem> budgetItems = HomeBudget.GetBudgetItems(startDate, endDate, filterFlag, id);
                DataSource = budgetItems.Cast<object>().ToList();
            }

            else if (isMonthChecked && isCategoryChecked)
            {
                List<string> usedList = new List<string>();

                foreach (Category category in homeBudget.categories.List())
                {
                    usedList.Add(category.Description);
                }
                IView.InitializeByCategoryAndMonthDisplay(usedList);
                var bi = HomeBudget.GetBudgetDictionaryByCategoryAndMonth(startDate, endDate, filterFlag, id);
                DataSource = bi.Cast<object>().ToList();
            }
            else if (isMonthChecked && !isCategoryChecked)
            {
                IView.InitializeByMonthDisplay();
                List<BudgetItemsByMonth> bi = HomeBudget.GetBudgetItemsByMonth(startDate, endDate, filterFlag, id);
                DataSource = bi.Cast<object>().ToList();

            }
            else if (isCategoryChecked && !isMonthChecked)
            {
                IView.InitializeByCategoryDisplay();
                List<BudgetItemsByCategory> bi = HomeBudget.GetBudgetItemsByCategory(startDate, endDate, filterFlag, id);
                DataSource = bi.Cast<object>().ToList();
            }

        }

        //not testable!
        public void Modify()
        {
            main.ModifyExpenseForm();
        }
        public void Delete()
        {
            main.DeleteItem();
        }
        public void SearchInDataGrid()
        {
            main.SearchInDataGrid();
        }
    }
}
