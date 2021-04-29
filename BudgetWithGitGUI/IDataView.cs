using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetWithGitGUI
{
    public interface IDataView
    {
        DataPresenter presnter { get; set; }

        List<Object> DataSource { get; set; }
        void ResetFocus(int itemIndex);
        void DataClear();
        void CreateDefaultDataGrid();
        void CreateSummaryByMonthGrid();
        void CreateSummaryByCategoryGrid();
        void CreateSummaryByCategoryAndMonthGrid();
        void SearchInDataGrid();

    }
}
