using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BudgetWithGitGUI;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HomeBudgetTesting 
{
    [TestClass]
    public class UnitTest1 : IDataView
    {
        public MainWindow main;
        public DataView dataView;
        bool called_CreateDefaultDataGrid = false;
        bool called_CreateSummaryByCategoryAndMonthGrid = false;

        public DataPresenter presnter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<object> DataSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataPresenter presenter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            throw new NotImplementedException();
        }

        public void InitializeByMonthDisplay()
        {
            throw new NotImplementedException();
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
        public void TestMethod1()
        {
            var preseter = new DataPresenter(main, dataView);
            Assert.IsTrue(called_CreateDefaultDataGrid, "Default Category has been been displayed");
            Assert.IsTrue(called_CreateSummaryByCategoryAndMonthGrid, "Displayed by categroy and month");
        }
        

    }
}
