using System;
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

            Assert.IsTrue(list.Count>0);

            Console.WriteLine("Records: {0}", list.Count);
            Console.WriteLine("Takens: {0}", watch.Elapsed);
        }

        //[TestMethod]
        //public void TestQuery()
        //{
        //    int count = 100000;

        //    string sql = string.Format("select account_no,affundcode,afaveragecost,lastupdate from tb_acctfundmaster where rownum<={0}", count);

        //    var watch = new Stopwatch();

        //    watch.Start();

        //    var list = Database.Instance.Query<Model>(sql);

        //    watch.Stop();

        //    Assert.AreEqual(count, list.Count);

        //    Console.WriteLine("Execute: {0}", sql);
        //    Console.WriteLine("Records: {0}", list.Count);
        //    Console.WriteLine("Takens: {0}", watch.Elapsed);
        //}
    }
}
