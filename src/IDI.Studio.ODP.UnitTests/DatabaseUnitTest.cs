using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IDI.Studio.ODP.UnitTests
{
    public class Model
    {
        public string account_no { get; set; }
        public string affundcode { get; set; }
        public decimal? afaveragecost { get; set; }
        public string lastupdate { get; set; }
    }

    [TestClass]
    public class DatabaseUnitTest
    {
        [TestMethod]
        public void TestQuery()
        {
            var watch = new Stopwatch();

            watch.Start();

            var list = Database.Instance.Query<Model>("demo");

            watch.Stop();

            Assert.IsTrue(list.Count > 0);

            Console.WriteLine("Records: {0}", list.Count);
            Console.WriteLine("Takens: {0}", watch.Elapsed);
        }

        [TestMethod]
        public void TestBuildScript()
        {
            string sql = Database.Instance.BuildScript("select * from t", new List<string> { "t.date >= :p_from_date", "t.date <= :p_from_date" });

            Console.WriteLine(sql);

            Assert.IsTrue(string.Equals(sql,"select * from t where t.date >= :p_from_date and t.date <= :p_from_date", StringComparison.CurrentCultureIgnoreCase));
        }

        [TestMethod]
        public void TestBuildScriptWithExistingConditions()
        {
            string sql = Database.Instance.BuildScript("select * from t where rownum<=100", new List<string> { "t.date >= :p_from_date", "t.date <= :p_from_date" });

            Console.WriteLine(sql);

            Assert.IsTrue(string.Equals(sql, "select * from t where rownum<=100 and t.date >= :p_from_date and t.date <= :p_from_date", StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
