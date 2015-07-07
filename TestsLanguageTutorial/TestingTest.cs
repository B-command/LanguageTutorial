using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LanguageTutorial;

namespace TestsLanguageTutorial {
    [TestClass]
    public class TestingTest {
        #region WriteBall
        [TestMethod]
        public void WriteBallTest1() {
            TestWindow resWin = new TestWindow(1);

            Assert.AreEqual(" баллов", resWin.WriteBall("10"));
        }

        [TestMethod]
        public void WriteBallTest2() {
            TestWindow resWin = new TestWindow(1);

            Assert.AreEqual(" балл", resWin.WriteBall("101"));
        }

        [TestMethod]
        public void WriteBallTest3() {
            TestWindow resWin = new TestWindow(1);

            Assert.AreEqual(" балла", resWin.WriteBall("102"));
        }

        [TestMethod]
        public void WriteBallTest4() {
            TestWindow resWin = new TestWindow(1);

            Assert.AreEqual(" балла", resWin.WriteBall("103"));
        }

        [TestMethod]
        public void WriteBallTest5() {
            TestWindow resWin = new TestWindow(1);

            Assert.AreEqual(" балла", resWin.WriteBall("104"));
        }

        [TestMethod]
        public void WriteBallTest6() {
            TestWindow resWin = new TestWindow(1);

            Assert.AreEqual(" баллов", resWin.WriteBall("105"));
        }

        [TestMethod]
        public void WriteBallTest7() {
            TestWindow resWin = new TestWindow(1);

            Assert.AreEqual(" баллов", resWin.WriteBall("106"));
        }

        [TestMethod]
        public void WriteBallTest8() {
            TestWindow resWin = new TestWindow(1);

            Assert.AreEqual(" баллов", resWin.WriteBall("107"));
        }

        [TestMethod]
        public void WriteBallTest9() {
            TestWindow resWin = new TestWindow(1);

            Assert.AreEqual(" баллов", resWin.WriteBall("108"));
        }

        [TestMethod]
        public void WriteBallTest10() {
            TestWindow resWin = new TestWindow(1);

            Assert.AreEqual(" баллов", resWin.WriteBall("109"));
        }

        [TestMethod]
        public void WriteBallTest11() {
            TestWindow resWin = new TestWindow(1);

            Assert.AreEqual(" баллов", resWin.WriteBall("111"));
        }
        #endregion

        [TestMethod]
        public void ToLatinoTest1() {
            TestWindow testWin = new TestWindow(1);
            Assert.AreEqual(true, testWin.ToLatino('É', "ÉéÈè"));
        }

        [TestMethod]
        public void ToLatinoTest2() {
            TestWindow testWin = new TestWindow(1);
            Assert.AreEqual(false, testWin.ToLatino('É', "mama"));
        }
    }
}
