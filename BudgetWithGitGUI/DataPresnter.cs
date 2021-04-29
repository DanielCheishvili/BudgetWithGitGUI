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

        public DataPresenter(IDataView IView)
        {
            this.IView = IView;
            homeBudget = new HomeBudget("F:/Winter_2021/App_dev/Assignments/DemoForClass.db");
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
                return DataSource;
            }
            set
            {
                DataSource = value;
            }
        }

        public void FiltersHaveChanged(DateTime? startDate, DateTime? endDate, bool filterFlag, int id, bool isMonthChecked)
        {
           
            if (isMonthChecked)
            {
                IView.CreateSummaryByMonthGrid();
                List<BudgetItemsByMonth> bi = HomeBudget.GetBudgetItemsByMonth(startDate, endDate, filterFlag, id);
                DataSource = bi.Cast<object>().ToList();

            }

        }


    }
}
