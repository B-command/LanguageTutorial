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
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace LanguageTutorial
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow// : Window
    {
        //получаем ID языка, который хотим учить(тестировать)
        int LanguageID;
        public TestWindow(int language)
        {
            LanguageID = language;
            InitializeComponent();

            Uri uri = new Uri("pack://siteoforigin:,,,/Resources/Без имени-3.png");
            BitmapImage bitmap = new BitmapImage(uri);
            img.Source = bitmap;

            uri = new Uri("pack://siteoforigin:,,,/Resources/untitled_1.png");
            bitmap = new BitmapImage(uri);
            beep.Source = bitmap;
        }     
        //Это все для смены раскладки через WinIP
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(
            [In] IntPtr hWnd,
            [Out, Optional] IntPtr lpdwProcessId
            );
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        static extern ushort GetKeyboardLayout(
            [In] int idThread
            );

        /// <summary>
        /// Вернёт Id раскладки.
        /// </summary>
        ushort GetKeyboardLayout()
        {
            return GetKeyboardLayout(GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero));
        }

        //Кол-во слов за сессию
        static public int countWordOfS;
        //Кол-во угаданных слов
        public int countRightWord = 0;
        //Кол-во балов за сессию
        public int result = 0;
        //Кол-во выученных слов
        int isLearned = 0;

        //текущий словарь изучаемых слов
        List<WordDictionary> currentWord = new List<WordDictionary>();
        //текущее значение флага
        //toRussian показывает, какой перевод нам нужно получить:
        //если true, тогда слово нужно перевести с иностранного на русский
        //(переводимое слово на иностранном, перевод на русском);
        //если false, тогда слово нужно перевести с русского на иностранный
        //(переводимое слово на русском, перевод на иностранном)
        bool toRussian = true;//перевод на русский
        string[] translatingWord = new string[2];//берем строку из словаря и делим на два слова
        int LetterFalse = 0;//количество букв, которые пользователь ввел неправильно
        // для ID использованного слова, отгаданного с первого раза
        WordDictionary useWord = new WordDictionary();

        /// <summary>
        /// при загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // часть программы, которая загружает список изучаемых слов в dict
            using (var db = new LanguageTutorialContext())
            {
                var result = db.WordQueue.Where(wq => wq.UserId == App.oActiveUser.Id && wq.IsLearned == false);
                if (result != null)
                {
                    foreach (var wd in result)
                    {
                        var pair = db.WordDictionary.Find(wd.WordDictionaryId);
                        if (pair.LanguageId == LanguageID)
                        {
                            // Сохраняешь в список
                            currentWord.Add(pair);
                        }
                    }
                }
            }
            //если учим английский, то берем счетчик сессии
            if (LanguageID == 1)
            {
                countWordOfS = App.oCourseEnglish.WordsPerSession;
            }
            //если учим французский, то берем счетчик сессии
            if (LanguageID == 2)
            {
                countWordOfS = App.oCourseFrançais.WordsPerSession;
            }
            //вытаскиваем словарь, который учит пользователь
            //перевод с иностранного на русский
            toRussian = true;
            //выбор случайным образом как переводить(с русского на ин.яз или наоборот)
            translatingWord = NextWordChosing();
            //берем слово
            SequenceWords();
        }
      
        /// <summary>
        /// Функция выбора следующего слова из текущего списка изучаемых слов
        /// </summary>
        private string[] NextWordChosing()
        {
            toRussian = true;
            Random rnd = new Random();
            // определение индекса очередного слова из текущего списка изучаемых слов currentWord
            int translatingWordIndex = rnd.Next(0, currentWord.Count - 1);// dict.Count - 1);
            //translatingWord содержит слово и его перевод
            //пример: используется строка "cat,кошка"
            //полученный массив: translatingWord, который содержит:
            //translatingWord[0] = "cat"
            //translatingWord[1] = "кошка"
            translatingWord[0] = currentWord.ElementAt(translatingWordIndex).Word;
            translatingWord[1] = currentWord.ElementAt(translatingWordIndex).Translate;
            useWord = currentWord.ElementAt(translatingWordIndex);
            //условие, по которому определяем, переводим ли мы с ин.языка на русский
            //или с русского на иностранный
            if (rnd.Next(0, 19) > 9)
            {
                //если условие выполнилось, то меняем местами в "cat" и "кошка"
                //в translatingWord
                string tempStr = translatingWord[1];
                translatingWord[1] = translatingWord[0];
                translatingWord[0] = tempStr;
                //флаг toRussian = false, значит перевод у нас с русского на иностранный
                toRussian = false;
            }
            // если условие не выполнилось, тогда мы переводим с иностранного на русский
            //флаг toRussian установлен true
            //удаляем слово, которое мы только что использовали, из списка изучаемых слов,
            //чтобы еще раз на него не наткнуться
            currentWord.Remove(currentWord.ElementAt(translatingWordIndex));
            // возвращаем массив translatingWord, который содержит слово и перевод
            return translatingWord;
        }

        int schet = 1;//счетчик слов
        Label[] words;//массив Label для слов
        int length;//Длина угадываемого слова

        /// <summary>
        ///Последовательный прогон слов за сессию 
        /// </summary>
        void SequenceWords()
        {
            flag = false;
            enter = false;
            //Если счетчик равен списку слов, то 
            if (schet > countWordOfS)
            {
                enter = true;
                //конец тестирования
                SkipWord.Content = "ЗАВЕРШИТЬ ТЕСТИРОВАНИЕ";
                lblWord.Content = "";
                SkipWord.Click += new RoutedEventHandler(OnTestEnd);
            }
            else
            {
                SkipWord.Content = "ПРОПУСТИТЬ СЛОВО";
                //проверяем французский ли это язык
                if (LanguageID == 2 && toRussian == false)
                {
                    FranceWord();
                }
                lblSchetchik.Content = schet + "/" + countWordOfS;
                lblResult.Content = "Твой текущий результат " + result + WriteBall(result.ToString());
                //выводим слово с заглавной буквы
                lblWord.Content = translatingWord[0].Substring(0, 1).ToUpper() + translatingWord[0].Substring(1, translatingWord[0].Length - 1);
                //создаем массив Label для слова-перевода
                CreateLabel(translatingWord[1]);
            }
        }

        /// <summary>
        /// Функция преобразует французские символы в латиницу
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        void FranceWord()
        {
            string s = translatingWord[1];
            string franE = "ÉéÈèÊêËë";
            string franA = "ÀàÂâ";
            string franU = "ÙùÛûÜü";
            string franO = "Ôô";
            string franI = "ÎîÏï";
            string franY = "Ÿÿ";
            string franC = "Çç";
            string franAE = "Ææ";
            string franOE = "Œœ";
            for (int i = 0; i < s.Length; i++)
            {
                if (ToLatino(s[i], franE))
                {
                    s = s.Remove(i, 1).Insert(i, "e");
                }
                if (ToLatino(s[i], franA))
                {
                    s = s.Remove(i, 1).Insert(i, "a");
                }
                if (ToLatino(s[i], franU))
                {
                    s = s.Replace(s[i], 'u');
                }
                if (ToLatino(s[i], franO))
                {
                    s = s.Replace(s[i], 'o');
                }
                if (ToLatino(s[i], franI))
                {
                    s = s.Replace(s[i], 'i');
                }
                if (ToLatino(s[i], franY))
                {
                    s = s.Replace(s[i], 'y');
                }
                if (ToLatino(s[i], franC))
                {
                    s = s.Replace(s[i], 'c');
                }
                if (ToLatino(s[i], franAE))
                {
                    s = s.Remove(i, 1).Insert(i, "ae");
                }
                if (ToLatino(s[i], franOE))
                {
                    s = s.Remove(i, 1).Insert(i, "oe");
                }
            }
            translatingWord[1] = s;
        }

           //   lblResult.Co
        /// <summary>
        /// Проверка на наличие символа
        /// </summary>
        /// <param name="sym"></param>
        /// <param name="fran"></param>
        /// <returns></returns>
        public bool ToLatino(char sym, string fran)
        {
            if (fran.IndexOf(sym) == -1)
            {
                return false;
            }
            else return true;
        }

        /// <summary>
        /// Динамическое создание Label под слова
        /// </summary>
        void CreateLabel(string word)
        {
            length = word.Length;
            words = new Label[length];
            //Проверка пробела, чтоб отобразить в Label
            string s = " -'";
            int[] index = Words(word, s);
            for (int i = 0; i < length; i++)
            {
                words[i] = new Label();
                words[i].Width = 50;
                words[i].Height = 50;
                words[i].FontSize = 24;
                words[i].FontFamily = new FontFamily("Microsoft Sans Serif");
                words[i].FontSize = 24;
                if (i == index[0])
                {
                    words[i].Content = s[0];
                }
                else if (i == index[1])
                {
                    words[i].Content = s[1];
                }
                else if (i == index[2])
                {
                    words[i].Content = s[2];
                }
                else
                {
                    words[i].Content = "*";
                }
                ///добавляем на панель Label горизонтальной ориентации
                PanelLetters.Orientation = Orientation.Horizontal;
                PanelLetters.Children.Add(words[i]);
            }
        }

         /// <summary>
        /// Переделывает окончания слова балл
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
         string WriteBall(string result)
        {
            string w = "";
            char last = result[result.Length - 1];
            if (result.Length > 1 && result[result.Length - 2] == '1')
            {
                w = " баллов";
            }
            else
            {
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
            }
            return w;
        }

        /// <summary>
        /// Проверка слова на - или пробел
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        int[] Words(string word, string s)
        {
            int[] index = new int[3];
            index[0] = word.IndexOf(s[0]);
            index[1] = word.IndexOf(s[1]);
            index[2] = word.IndexOf(s[2]);
            return index;
        }

        /// <summary>
        /// Удаляет Label
        /// </summary>
        void DeleteLabel()
        {
            for (int i = 0; i < length; i++)
            {
                words[i].Content = "";
            }
            words = null;
            PanelLetters.Children.RemoveRange(0, length);
        }

        private string RusKey = "Ё!\"№;%:?*()_+ЙЦУКЕНГШЩЗХЪ/ФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,ё1234567890-=йцукенгшщзхъ\\фывапролджэячсмитьбю. ";
        private string EngKey = "~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./ ";

        /// <summary>
        /// Проверяем нажатие клавиш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
                if ((int)e.Key >= 44 && (int)e.Key <= 69 || (int)e.Key == 149 || (int)e.Key == 140 || (int)e.Key == 142 ||
                   (int)e.Key == 144 || (int)e.Key == 152 || (int)e.Key == 151)
                {
                    e.Handled = false;
                    string a;
                    switch ((int)e.Key)
                    {
                        case 140:
                            a = ":";
                            break;
                        case 142:
                            a = "<";
                            break;
                        case 144:
                            a = ">";
                            break;
                        case 149:
                            a = "{";
                            break;
                        case 152:
                            a = Convert.ToString('"');
                            break;
                        case 151:
                            a = "}";
                            break;
                        default:
                            a = e.Key.ToString();
                            break;
                    }
                    OnKeyDown(a);
                }
                else
                {
                    e.Handled = true;
                }
            if (e.Key==Key.Enter&&enter==false)
            {
                SkipWord_Click(new object(), new RoutedEventArgs());
            }
            }

        //записываем из Label букву
        string ss = "";
        /// <summary>
        /// Проверяем ввод букв
        /// </summary>
        /// <param name="a"></param>
        void OnKeyDown(string a)
        {
                //смотрим раскладку
                string raskladka = GetKeyboardLayout().ToString();
                string s = "";
                //С русского на иностранный (toRussian=false)
                if (toRussian == true && raskladka == "1033")//EN
                {
                    s = RusKey.Substring(EngKey.IndexOf(a), 1);
                }
                else if (toRussian == false && raskladka == "1033")
                {
                    s = a;
                }
                if (toRussian == false && raskladka == "1049")//RUS
                {
                    s = a;
                }

                else if (toRussian == true && raskladka == "1049")
                {
                    s = RusKey.Substring(EngKey.IndexOf(a), 1);
                }
                if (flag == false && translatingWord[1].ToLower() != ss.ToLower())
                {
                    FillTest(s);
                }
        }

        static bool sound = true;
        public static void errorSignal()
        {
                  Task.Factory.StartNew(() =>
            {
                if (sound == true) {
                    Console.Beep(400, 200);
                }
               System.Threading.Thread.Sleep(200);
            });
        }

        string lieLetters="";
        /// <summary>
        /// Проверяет символ и добавлет его в Label
        /// </summary>
        /// <param name="s"></param>
        void FillTest(string s)
        {
             ss = "";
            //переписываем в массив символов наше отгадываемое слово
            char[] wor = translatingWord[1].ToUpper().ToCharArray();
            //сравниваем есть ли буква в слове
            bool have = false;
            for (int i = 0; i < wor.Length; i++)
            {
                if (wor[i] == s[0])
                {
                    //Добавляем в Label букву
                    words[i].Content = s[0];
                    have = true;
                }
            }
            if (!have)
            {
                if (lieLetters.IndexOf(s[0]) == -1)
                {
                    lieLetters += " "+s[0];
                    lblLie.Content = lieLetters;
                    result -= 1;
                    LetterFalse += 1;
                    lblResult.Content = "Твой текущий результат " + result + WriteBall(result.ToString());
                }
                //звук об ошибке
                errorSignal();
                LetterFalse += 1;
                lblResult.Content = "Твой текущий результат " + result + WriteBall(result.ToString());
            }
            for (int i = 0; i < words.Length; i++)
            {
                ss += words[i].Content;
            }
            //если слово отгадано, даем другое слово
            if (translatingWord[1].ToLower() == ss.ToLower())
            {
                if (schet == countWordOfS)
                {
                    enter = true;
                    SkipWord.Content = "ЗАВЕРШИТЬ ТЕСТИРОВАНИЕ";
                    SkipWord.Click += new RoutedEventHandler(OnTestEnd);
                }
                else
                {
                        if (toRussian)
                        {
                            result += 2 * translatingWord[1].Length;
                            lblResult.Content = "Твой текущий результат " + result + WriteBall(result.ToString());
                        }
                        else
                        {
                            result += 3 * translatingWord[1].Length;
                            lblResult.Content = "Твой текущий результат " + result + WriteBall(result.ToString());
                        }
                        SkipWord.Content = "СЛЕДУЮЩЕЕ СЛОВО";
                        if (LetterFalse == 0)
                        {
                            countRightWord += 1;
                            //Если угадано слово с первого раза, то заносим в БД TrueAnswers+=1
                            // MessageBox.Show("С первого раза" + LetterFalse);
                            using (var db = new LanguageTutorialContext())
                            {
                                var result = db.WordQueue.FirstOrDefault(wq => wq.UserId == App.oActiveUser.Id && wq.IsLearned == false
                                && wq.WordDictionaryId == useWord.Id);
                                if (result != null)
                                {
                                    result.TrueAnswers++;
                                    if (LanguageID == 1)
                                    {
                                        if (App.oCourseEnglish.TrueAnswers == result.TrueAnswers)
                                        {
                                            result.IsLearned = true;
                                            isLearned++;
                                        }
                                    }
                                    if (LanguageID == 2)
                                    {
                                        if (App.oCourseFrançais.TrueAnswers == result.TrueAnswers)
                                        {
                                            result.IsLearned = true;
                                            isLearned++;
                                        }
                                    }
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    LetterFalse = 0;
            }
            
        }
        bool enter = false;
        bool flag = false;
        private void SkipWord_Click(object sender, RoutedEventArgs e) 
        {
            flag = true;
            if (schet > countWordOfS)
            {
                enter = true;
                SkipWord.Content = "ЗАВЕРШИТЬ ТЕСТИРОВАНИЕ";
                SkipWord.Click += new RoutedEventHandler(OnTestEnd);
                lblWord.Content = "";
            }
            else
            {
                if (SkipWord.Content == "СЛЕДУЮЩЕЕ СЛОВО")
                {
                    DeleteLabel();
                    schet++;
                    toRussian = true;
                    translatingWord = NextWordChosing();
                    SequenceWords();
                }
                else
                {
                    //вычитаем балы за пропуск
                if (toRussian) 
                {
                    result -= 3 * translatingWord[1].Length;
                    lblResult.Content = "Твой текущий результат " + result + WriteBall(result.ToString());
                }
                else
                {
                    result -= 2 * translatingWord[1].Length;
                    lblResult.Content = "Твой текущий результат " + result + WriteBall(result.ToString());
                }
                    //следующее слово
                    if(schet>countWordOfS)
                    {
                        SkipWord.Content = "Завершить тестирование";
                        SkipWord.Click += new RoutedEventHandler(OnTestEnd);
                    } 
                   else
                    {
                        DeleteLabel();
                        schet++;
                        toRussian = true;
                        translatingWord = NextWordChosing();
                        SequenceWords();
                    }
                }
            }  
        }

        bool timer = false;
        void OnTestEnd(object sender, RoutedEventArgs e)
        {
           ResultWindow resultWindow = new ResultWindow(LanguageID, result, countRightWord, isLearned);
            resultWindow.ShowDialog();
            timer = true;
            Close();
        }
       
        private void MetroWindow_Closed(object sender, EventArgs e) {
            if(timer == false) {
                if (App.EngSession < TimerMet.numberSessionsLanguageEng() || App.FranSession < TimerMet.numberSessionsLanguageFran()) {//переместить код в тест
                    App.aTimer.Start();
                }
            }
        }

        private void beep_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            sound = !sound;
            if (sound == true) {
                Uri uri = new Uri("pack://siteoforigin:,,,/Resources/untitled_1.png");
                BitmapImage bitmap = new BitmapImage(uri);
                beep.Source = bitmap;
                beep.ToolTip = "Отключить звук";
            } else {
                Uri uri = new Uri("pack://siteoforigin:,,,/Resources/volume-off_318-25206.jpg");
                BitmapImage bitmap = new BitmapImage(uri);
                beep.Source = bitmap;
                beep.ToolTip = "Включить звук";
            }
        }
    }
}
