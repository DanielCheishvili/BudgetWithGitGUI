using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetWithGitGUI
{
    public interface IDataView
    {
        int CurrentIndex { get; set; }
        int SelectedIndex { get; set; }
        object SelectedItem { get; set; }
        void ResetFocus(int index);
        bool ContextMenuEnabled { get; set; }
        DataPresenter presenter { get; set; }
        List<object> DataSource { get; set; }
        void ResetFocusAfterUpdate(int itemIndex);
        void DataClear();
        void InitializeStandardDisplay();
        void InitializeByMonthDisplay();
        void InitializeByCategoryDisplay();
        void InitializeByCategoryAndMonthDisplay(List<String> usedCategoryList);

    }
}
