using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LanguageTutorial;

namespace TestsLanguageTutorial {
    [TestClass]
    public class ResultTest {

        #region WriteWord
        [TestMethod]
        public void WriteWordTest1() {
            ResultWindow resWin = new ResultWindow(1, 10);
            
            Assert.AreEqual(" слов", resWin.WriteWord("10"));
        }

        [TestMethod]
        public void WriteWordTest2() {
            ResultWindow resWin = new ResultWindow(1, 101);

            Assert.AreEqual(" слово", resWin.WriteWord("101"));
        }

        [TestMethod]
        public void WriteWordTest3() {
            ResultWindow resWin = new ResultWindow(1, 102);

            Assert.AreEqual(" слова", resWin.WriteWord("102"));
        }

        [TestMethod]
        public void WriteWordTest4() {
            ResultWindow resWin = new ResultWindow(1, 103);

            Assert.AreEqual(" слова", resWin.WriteWord("103"));
        }

        [TestMethod]
        public void WriteWordTest5() {
            ResultWindow resWin = new ResultWindow(1, 104);

            Assert.AreEqual(" слова", resWin.WriteWord("104"));
        }

        [TestMethod]
        public void WriteWordTest6() {
            ResultWindow resWin = new ResultWindow(1, 105);

            Assert.AreEqual(" слов", resWin.WriteWord("105"));
        }

        [TestMethod]
        public void WriteWordTest7() {
            ResultWindow resWin = new ResultWindow(1, 106);

            Assert.AreEqual(" слов", resWin.WriteWord("106"));
        }

        [TestMethod]
        public void WriteWordTest8() {
            ResultWindow resWin = new ResultWindow(1, 107);

            Assert.AreEqual(" слов", resWin.WriteWord("107"));
        }

        [TestMethod]
        public void WriteWordTest9() {
            ResultWindow resWin = new ResultWindow(1, 108);

            Assert.AreEqual(" слов", resWin.WriteWord("108"));
        }

        [TestMethod]
        public void WriteWordTest10() {
            ResultWindow resWin = new ResultWindow(1, 109);

            Assert.AreEqual(" слов", resWin.WriteWord("109"));
        }

        [TestMethod]
        public void WriteWordTest11() {
            ResultWindow resWin = new ResultWindow(1, 111);

            Assert.AreEqual(" слов", resWin.WriteWord("111"));
        }
        #endregion

        #region WriteBall
        [TestMethod]
        public void WriteBallTest1() {
            ResultWindow resWin = new ResultWindow(1, 10);

            Assert.AreEqual(" баллов", resWin.WriteBall("10"));
        }

        [TestMethod]
        public void WriteBallTest2() {
            ResultWindow resWin = new ResultWindow(1, 101);

            Assert.AreEqual(" балл", resWin.WriteBall("101"));
        }

        [TestMethod]
        public void WriteBallTest3() {
            ResultWindow resWin = new ResultWindow(1, 102);

            Assert.AreEqual(" балла", resWin.WriteBall("102"));
        }

        [TestMethod]
        public void WriteBallTest4() {
            ResultWindow resWin = new ResultWindow(1, 103);

            Assert.AreEqual(" балла", resWin.WriteBall("103"));
        }

        [TestMethod]
        public void WriteBallTest5() {
            ResultWindow resWin = new ResultWindow(1, 104);

            Assert.AreEqual(" балла", resWin.WriteBall("104"));
        }

        [TestMethod]
        public void WriteBallTest6() {
            ResultWindow resWin = new ResultWindow(1, 105);

            Assert.AreEqual(" баллов", resWin.WriteBall("105"));
        }

        [TestMethod]
        public void WriteBallTest7() {
            ResultWindow resWin = new ResultWindow(1, 106);

            Assert.AreEqual(" баллов", resWin.WriteBall("106"));
        }

        [TestMethod]
        public void WriteBallTest8() {
            ResultWindow resWin = new ResultWindow(1, 107);

            Assert.AreEqual(" баллов", resWin.WriteBall("107"));
        }

        [TestMethod]
        public void WriteBallTest9() {
            ResultWindow resWin = new ResultWindow(1, 108);

            Assert.AreEqual(" баллов", resWin.WriteBall("108"));
        }

        [TestMethod]
        public void WriteBallTest10() {
            ResultWindow resWin = new ResultWindow(1, 109);

            Assert.AreEqual(" баллов", resWin.WriteBall("109"));
        }

        [TestMethod]
        public void WriteBallTest11() {
            ResultWindow resWin = new ResultWindow(1, 111);

            Assert.AreEqual(" баллов", resWin.WriteBall("111"));
        }
        #endregion
    }
}
