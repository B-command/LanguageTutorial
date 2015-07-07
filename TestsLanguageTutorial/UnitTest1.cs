using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LanguageTutorial;

namespace TestsLanguageTutorial {
    [TestClass]
    public class TimerMetTest {
        [TestMethod]
        public void TestnumberSessionsLanguageEng() {
            App.oCourseEnglish = null;
            int res = 0;
            int real = TimerMet.numberSessionsLanguageEng();
            Assert.AreEqual(res, real, "Нет активного сеанса");
        }
    }
}
