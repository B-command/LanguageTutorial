using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LanguageTutorial.DataModel;
using System.Data.Entity;

namespace LanguageTutorial
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow
    {
        int LanguageID;
        int Result;
        int CountRightWords;
        int isLearned;
        public ResultWindow(int language, int result, int countWordS, int learned)
        {
            isLearned = learned;
            CountRightWords = countWordS;
            Result = result;
            LanguageID = language;
            InitializeComponent();
            lblBall.Content = "Ваш результат за текущую сессию " + Result + WriteBall(Result.ToString());
            lblRight.Content = "Вы отгадали " + CountRightWords + WriteWord(CountRightWords.ToString());
            lblRight.Content += " с первого раза";

            Uri uri = new Uri("pack://siteoforigin:,,,/Resources/caty.png");
            BitmapImage bitmap = new BitmapImage(uri);
            img.Source = bitmap;
        }
        public string WriteWord(string result)
        {
            string w = "";
            char last = result[result.Length-1];
            if (result.Length > 1 && result[result.Length - 2] == '1') {
                w = " слов";
            } else {
                switch (last) {
                    case '0':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9': {
                            w = " слов";
                            break;
                        }
                    case '1': {
                            w = " слово";
                            break;
                        }
                    case '2':
                    case '3':
                    case '4': {
                            w = " слова";
                            break;
                        }
                }
            }
            return w;
        }


        public string WriteBall(string result)
        {
            string w = "";
            char last = result[result.Length - 1];
            if (result.Length > 1 && result[result.Length - 2] == '1') {
                w = " баллов";
            } else {
                switch (last) {
                    case '0':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9': {
                            w = " баллов";
                            break;
                        }
                    case '1': {
                            w = " балл";
                            break;
                        }
                    case '2':
                    case '3':
                    case '4': {
                            w = " балла";
                            break;
                        }
                }
            }
            return w;
        }

        private void Window_Closed(object sender, EventArgs e) {
            if (LanguageID == 1) {
                App.EngSession++;
            }
            if (LanguageID == 2) {
                App.FranSession++;
            }
            if (App.EngSession < TimerMet.numberSessionsLanguageEng() || App.FranSession < TimerMet.numberSessionsLanguageFran()) {//переместить код в тест
                App.aTimer.Start();
            }
        }

        private void bt_ok_Click(object sender, RoutedEventArgs e) {
            Close();
        }


        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
           using (var db = new LanguageTutorialContext())
            {
                if (LanguageID == 1)
                {
                    //var result = db.Course.Where(wq => wq.UserId == App.oActiveUser.Id && wq.LanguageId == LanguageID).FirstOrDefault().Id;
                   // if (result != null)
                    //{
                            var session = new Session();
                            session.CourseId = App.oCourseEnglish.Id;
                            session.Words = CountRightWords;
                            session.Points = Result;
                            session.Datetime = DateTime.Now;
                            session.FinishedWords = isLearned;
                            db.Session.Add(session);
                        //}
                }
                        if (LanguageID == 2)
                        {
                           // var result = db.Course.Where(wq => wq.UserId == App.oActiveUser.Id && wq.LanguageId == LanguageID);
                    //if (result != null)
                    //{
                            var session = new Session();
                            session.CourseId = App.oCourseFrançais.Id;
                            session.Words = CountRightWords;
                            session.Points = Result;
                            session.Datetime = DateTime.Now;
                            session.FinishedWords = isLearned;
                            db.Session.Add(session);
                        //}
                        }
                db.SaveChanges();
                    }
                }
    }
}
