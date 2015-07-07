using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LanguageTutorial;

namespace TestsLanguageTutorial {
    [TestClass]
    public class MainMenuTest {
        [TestMethod]
        public void openTestingTest() {
            App.EngSession = 10;
            App.FranSession = 10;
            MainMenuWindow.openTesting(5, 5);
        }
    }
}
