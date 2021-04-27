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
        public DataPresenter(IDataView IView)
        {
            this.IView = IView;
        }
        public void FiltersHaveChanged(DateTime startDate, DateTime endDate, bool filterByCatChecked, int id, bool monthChecked, bool catChecked)
        {

            if (monthChecked)
            {
                IView.CreateSummaryByMonthGrid();
            }

        }


    }
}
