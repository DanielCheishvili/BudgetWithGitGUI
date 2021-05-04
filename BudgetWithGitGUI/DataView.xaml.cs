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

namespace BudgetWithGitGUI
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataView : UserControl, IDataView
    {
        public DataView()
        {
            InitializeComponent();
        }

        public HomeBudget homeBudget_;
        public DataPresenter presnter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<object> DataSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void CreateDefaultDataGrid()
        {

            dataGrid.Columns.Clear();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Date";
            column.Binding = new Binding("Date");
            column.Binding.StringFormat = "yyyy-MM-dd";
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Category";
            column.Binding = new Binding("Category");
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Description";
            column.Binding = new Binding("ShortDescription");
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Amount";
            column.Binding = new Binding("Amount");
            column.Binding.StringFormat = "F2";
            Style style = new Style();
            style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            column.CellStyle = style;
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Balance";
            column.Binding = new Binding("Balance");
            column.Binding.StringFormat = "F2";
            column.CellStyle = style;
            dataGrid.Columns.Add(column);

        }
        public void CreateSummaryByMonthGrid()
        {
            dataGrid.Columns.Clear();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Month";
            column.Binding = new Binding("Month");
            column.Binding.StringFormat = "yyyy-MM";
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Total";
            column.Binding = new Binding("Total");
            column.Binding.StringFormat = "F2";
            Style style = new Style();
            style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            column.CellStyle = style;
            dataGrid.Columns.Add(column);
        }
        public void CreateSummaryByCategoryGrid()
        {
            dataGrid.Columns.Clear();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Category";
            column.Binding = new Binding("Category");
            dataGrid.Columns.Add(column);

            column = new DataGridTextColumn();
            column.Header = "Total";
            column.Binding = new Binding("Total");
            column.Binding.StringFormat = "F2";
            Style style = new Style();
            style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            column.CellStyle = style;
            dataGrid.Columns.Add(column);
        }
        public void CreateSummaryByCategoryAndMonthGrid()
        {
            dataGrid.Columns.Clear();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Month";
            column.Binding = new Binding("[Month]");
            column.Binding.StringFormat = "yyyy-MM";
            dataGrid.Columns.Add(column);

            foreach (Category category in homeBudget_.categories.List())
            {
                column = new DataGridTextColumn();
                column.Header = category.Description;
                column.Binding = new Binding("[" + category.Description + "]");
                Style style = new Style();
                style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
                column.CellStyle = style;
                dataGrid.Columns.Add(column);
            }
            column = new DataGridTextColumn();
            column.Header = "Total";
            column.Binding = new Binding("[Total]");
            column.Binding.StringFormat = "F2";
            Style style2 = new Style();
            style2.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            column.CellStyle = style2;
            dataGrid.Columns.Add(column);

        }

        public void DataClear()
        {
            throw new NotImplementedException();
        }

        public void ResetFocus(int itemIndex)
        {
            throw new NotImplementedException();
        }

        public void SearchInDataGrid()
        {
            throw new NotImplementedException();
        }

    }
}
