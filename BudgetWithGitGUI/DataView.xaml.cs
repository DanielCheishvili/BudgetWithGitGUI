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
            InitializeStandardDisplay();
        }
        public DataPresenter presenter;
        public List<Object> dataSource;
        


        public HomeBudget homeBudget_;
        public DataPresenter presnter 
        { 
            get
            {
                return presenter;
            }
            set
            {
                presenter = value;
            }
        }
        public List<object> DataSource
        {
            get
            {
                return dataGrid.ItemsSource.Cast<object>().ToList();
            }
            set
            {

                dataGrid.ItemsSource = value;
            }
        }

        DataPresenter IDataView.presenter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void InitializeStandardDisplay()
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
        public void InitializeByMonthDisplay()
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
        public void InitializeByCategoryDisplay()
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
        public void InitializeByCategoryAndMonthDisplay(List<string> usedList)
        {
            dataGrid.Columns.Clear();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = "Month";
            column.Binding = new Binding("[Month]");
            column.Binding.StringFormat = "yyyy-MM";
            dataGrid.Columns.Add(column);

            foreach (String category in usedList)
            {
                column = new DataGridTextColumn();
                column.Header = category;
                column.Binding = new Binding("[" + category + "]");
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

        //ask
        public void DataClear()
        {
            throw new NotImplementedException();
        }

        public void ResetFocusAfterUpdate(int itemIndex)
        {
            throw new NotImplementedException();
        }

        public void SearchInDataGrid()
        {
            presenter.SearchInDataGrid();
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            presenter.Modify();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void modifySelect_Click(object sender, RoutedEventArgs e)
        {
            presenter.Modify();
        }

        private void DeleteSelect_Click(object sender, RoutedEventArgs e)
        {
            presenter.Delete();
        }
    }
}
