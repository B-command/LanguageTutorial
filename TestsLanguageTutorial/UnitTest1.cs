using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LanguageTutorial;
using LanguageTutorial.DataModel;

namespace TestsLanguageTutorial {
    [TestClass]
    public class TimerMetTest {
        [TestMethod]
        public void TestnumberSessionsLanguageEng1() {
            App.oCourseEnglish = null;
            int res = 0;
            int real = TimerMet.numberSessionsLanguageEng();
            Assert.AreEqual(res, real, "Нет сеанса");
        }

        [TestMethod]
        public void TestnumberSessionsLanguageEng2() {
            var nCourse = new LanguageTutorial.DataModel.Course();
            nCourse.Active = true;
            nCourse.SeansPerDay = 10;
            App.oCourseEnglish = nCourse;

            int res = 10;
            int real = TimerMet.numberSessionsLanguageEng();
            Assert.AreEqual(res, real, "Сеансы за день");
        }

        [TestMethod]
        public void TestnumberSessionsLanguageEng3() {
            var nCourse = new LanguageTutorial.DataModel.Course();
            nCourse.Active = false;
            nCourse.SeansPerDay = 10;
            
            App.oCourseEnglish = nCourse;

            int res = 0;
            int real = TimerMet.numberSessionsLanguageEng();
            Assert.AreEqual(res, real, "Сеанс не активен");
        }

        [TestMethod]
        public void TestnumberSessionsLanguageFran1() {
            App.oCourseFrançais = null;
            int res = 0;
            int real = TimerMet.numberSessionsLanguageFran();
            Assert.AreEqual(res, real, "Нет сеанса");
        }

        [TestMethod]
        public void TestnumberSessionsLanguageFran2() {
            var nCourse = new LanguageTutorial.DataModel.Course();
            nCourse.Active = true;
            nCourse.SeansPerDay = 10;
            App.oCourseFrançais = nCourse;

            int res = 10;
            int real = TimerMet.numberSessionsLanguageFran();
            Assert.AreEqual(res, real, "Сеансы за день");
        }

        [TestMethod]
        public void TestnumberSessionsLanguageFran3() {
            var nCourse = new LanguageTutorial.DataModel.Course();
            nCourse.Active = false;
            nCourse.SeansPerDay = 10;

            App.oCourseFrançais = nCourse;

            int res = 0;
            int real = TimerMet.numberSessionsLanguageFran();
            Assert.AreEqual(res, real, "Сеанс не активен");
        }

       /* [TestMethod]
        public void TestnumberSessionsLanguage()
        {
           Course someCourse = new LanguageTutorial.DataModel.Course() { Active = true, SeansPerDay = 5 };
            int res = 5;
            int real = TimerMet.numberSessionsLanguage(someCourse);

            Assert.AreEqual(real, res);
        }

        [TestMethod]
        public void TestnumberSessionsLanguage_Null()
        {
            //Тестируемая функция должна вернуть 0, т.к. курс не активен
            Course someCourse = new Course() { Active = false, SeansPerDay = 5 };
            int res = 0;
            int real = TimerMet.numberSessionsLanguage(someCourse);

            Assert.AreEqual(real, res);
        } */
    }
}
