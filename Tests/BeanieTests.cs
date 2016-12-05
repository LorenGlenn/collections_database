using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Inventory.Objects;

namespace InventoryTest
{
  public class BeanieTest : IDisposable
  {
    public BeanieTest()
    {
      Inventory.DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Beanie.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      Beanie beanieOne = new Beanie("Ryan", "High", 100, 1);
      Beanie beanieTwo = new Beanie("Ryan", "Low", 5, 2);

      Assert.Equal(beanieOne, beanieTwo);
    }

    [Fact]
    public void Test_SavesToDatabase()
    {
      Beanie testBeanie = new Beanie("Ryan", "High", 100, 1);
      testBeanie.Save();
      List<Beanie> result = Beanie.GetAll();
      List<Beanie> testList = new List<Beanie>{testBeanie};

      Assert.Equal(testList, result);
    }

    public void Dispose()
    {
      Beanie.DeleteAll();
    }
  }
}
