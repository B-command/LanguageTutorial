using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LanguageTutorial;
using LanguageTutorial.DataModel;

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

        [TestMethod]
        public void TestnumberSessionsLanguage()
        {
           Course someCourse = new LanguageTutorial.DataModel.Course() { Active = true, SeansPerDay = 5 };
            int res = 5;
            int real = TimerMet.numberSessionsLanguage(someCourse);

            Assert.AreEqual(real, res);
        }
    }
}
