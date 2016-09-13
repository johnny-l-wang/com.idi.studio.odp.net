using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IDI.Studio.ODP.UnitTests
{
    public class Model
    {
        public string fundcode { get; set; }
        public decimal start_amt { get; set; }
        public decimal fel { get; set; }
        public DateTime reclastupdate { get; set; }
    }

    [TestClass]
    public class DatabaseUnitTest
    {
        [TestMethod]
        public void TestQuery()
        {
            int count = 1000;

            string sql = string.Format("select fundcode,start_amt,fel,reclastupdate from if_fel where rownum<={0}", count);

            var list = Database.Instance.Query<Model>(sql);

            Assert.AreEqual(count, list.Count);
        }
    }
}
