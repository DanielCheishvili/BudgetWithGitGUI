using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BudgetWithGitGUI;
using System.Collections.Generic;
using System.Data.SQLite;
using Budget;

namespace HomeBudgetTesting 
{
    [TestClass]
    public class UnitTest1 : IDataView
    {

        private List<Object> dataSource;
        bool called_CreateDefaultDataGrid = false;
        bool called_CreateSummaryByCategoryAndMonthGrid = false;
        bool called_CreateSummaryByCategory = false;
        bool called_CreateSummaryByMonth = false;
        public List<object> DataSource 
        { 
           get
            {
                return dataSource;
            }
            set
            {
                dataSource = value;
            }
        }
        public DataPresenter presenter 
        { 
            get => presenter; 
            set => presenter = value; 
        }

        public void InitializeStandardDisplay()
        {
            called_CreateDefaultDataGrid = true;
        }

        public void InitializeByCategoryAndMonthDisplay(List<string> usedList)
        {
            called_CreateSummaryByCategoryAndMonthGrid = true;
        }

        public void InitializeByCategoryDisplay()
        {
            called_CreateSummaryByCategory = true;
        }

        public void InitializeByMonthDisplay()
        {
            called_CreateSummaryByMonth = true;
        }

        public void DataClear()
        {
            throw new NotImplementedException();
        }
       [TestInitialize]
        public void Reset()
        {
            called_CreateDefaultDataGrid = false;
            called_CreateSummaryByCategoryAndMonthGrid = false;
            called_CreateSummaryByCategory = false;
            called_CreateSummaryByMonth = false;
        }

        public void ResetFocusAfterUpdate(int itemIndex)
        {
            throw new NotImplementedException();
        }

        public void SearchInDataGrid()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ChecksIfTheDefaultDisplayHasBeenCalled()
        {
            
            HomeBudget home = new HomeBudget("./DemoForClass.db");
            UnitTest1 testDataView = new UnitTest1();
            
            var preseter = new DataPresenter(home, testDataView);
            preseter.FiltersHaveChanged(null, null, false, 1, false, false);

            Assert.IsTrue(testDataView.called_CreateDefaultDataGrid);
            Assert.IsFalse(testDataView.called_CreateSummaryByCategoryAndMonthGrid);
            Assert.IsFalse(testDataView.called_CreateSummaryByCategory);
            Assert.IsFalse(testDataView.called_CreateSummaryByMonth);
        }
        [TestMethod]
        public void ChecksIfTheByMonthAndCategoryDisplayHasBeenCalled()
        {
            
            HomeBudget home = new HomeBudget("./DemoForClass.db");
            UnitTest1 testDataView = new UnitTest1();

            var preseter = new DataPresenter(home, testDataView);
            preseter.FiltersHaveChanged(null, null, false, 1, true, true);

            Assert.IsFalse(testDataView.called_CreateDefaultDataGrid);
            Assert.IsTrue(testDataView.called_CreateSummaryByCategoryAndMonthGrid);
            Assert.IsFalse(testDataView.called_CreateSummaryByCategory);
            Assert.IsFalse(testDataView.called_CreateSummaryByMonth);
        }
        [TestMethod]
        public void ChecksIfTheByCategoryDisplayHasBeenCalled()
        {
            
            HomeBudget home = new HomeBudget("./DemoForClass.db");
            UnitTest1 testDataView = new UnitTest1();

            var preseter = new DataPresenter(home, testDataView);
            preseter.FiltersHaveChanged(null, null, false, 1, false, true);

            Assert.IsFalse(testDataView.called_CreateDefaultDataGrid);
            Assert.IsFalse(testDataView.called_CreateSummaryByCategoryAndMonthGrid);
            Assert.IsTrue(testDataView.called_CreateSummaryByCategory);
            Assert.IsFalse(testDataView.called_CreateSummaryByMonth);
        }
        [TestMethod]
        public void ChecksIfTheByMonthDisplayHasBeenCalled()
        {
            
            HomeBudget home = new HomeBudget("./DemoForClass.db");
            UnitTest1 testDataView = new UnitTest1();

            var preseter = new DataPresenter(home, testDataView);
            preseter.FiltersHaveChanged(null, null, false, 1, true, false);

            Assert.IsFalse(testDataView.called_CreateDefaultDataGrid);
            Assert.IsFalse(testDataView.called_CreateSummaryByCategoryAndMonthGrid);
            Assert.IsFalse(testDataView.called_CreateSummaryByCategory);
            Assert.IsTrue(testDataView.called_CreateSummaryByMonth);
        }

    }
}
