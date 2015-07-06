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

namespace LanguageTutorial
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow
    {
        int LanguageID;
        int Result;
        public ResultWindow(int language,int result)
        {
            Result = result;
            LanguageID=language;
            InitializeComponent();
            lblBall.Content = "Ваш результат за текущую сессию " + Result + WriteBall(Result.ToString());
            lblRight.Content = "Вы отгадали " + TestWindow.countRightWord + WriteWord(TestWindow.countRightWord.ToString());

            Uri uri = new Uri("pack://siteoforigin:,,,/Resources/caty.png");
            BitmapImage bitmap = new BitmapImage(uri);
            img.Source = bitmap;
        }
        string WriteWord(string result)
        {
            string w = "";
            char last = result[result.Length-1];
            switch (last)
            {
                case '0':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    {
                        w = " слов";
                        break;
                    }
                case '1':
                    {
                        w = " слово";
                        break;
                    }
                case '2':
                case '3':
                case '4':
                    {
                        w = " слова";
                        break;
                    }    
            }
            return w;
        }
        string WriteBall(string result)
        {
            string w = "";
            char last = result[result.Length - 1];
            switch (last)
            {
                case '0':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    {
                        w = " баллов";
                        break;
                    }
                case '1':
                    {
                        w = " балл";
                        break;
                    }
                case '2':
                case '3':
                case '4':
                    {
                        w = " балла";
                        break;
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
            if (App.EngSession < Querry.numberSessionsLanguageEng() || App.FranSession < Querry.numberSessionsLanguageFran()) {//переместить код в тест
                App.aTimer.Start();
            }
        }

        private void bt_ok_Click(object sender, RoutedEventArgs e) {
           /* if (LanguageID == 1) {
                App.EngSession++;
            }
            if (LanguageID == 2) {
                App.FranSession++;
            }
            if (App.EngSession < Querry.numberSessionsLanguageEng() || App.FranSession < Querry.numberSessionsLanguageFran()) {//переместить код в тест
                App.aTimer.Start();
            }*/
            Close();
        }

    }
}
