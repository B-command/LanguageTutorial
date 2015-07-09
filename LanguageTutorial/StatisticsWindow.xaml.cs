using System;
using System.Collections;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow //: Window
    {
        //класс, объект которого хранит значения одной сессии
        private class StatisticsRow
        {
            //поле, хранящее дату сессии
            public DateTime Date { get; set; }

            //поле, хранящее количество баллов за сессию
            public int PointsQuantity { get; set; }

            //поле, хранящее количество отгаданных слов за сессию
            public int WordsQuantity { get; set; }
        }

        // массив, хранящий индексы активных курсов пользователя
        private int[] ActiveCourseID = new int[2];

        // коллекция, хранящая строки в DataGridStatistics
        ObservableCollection<StatisticsRow> collection;

        // поле, хранящее последний день отображаемой недели        
        DateTime WeekTopBoundary = DateTime.Now;

        // поле, хранящее первый день отображаемой недели
        DateTime WeekBottomBoundary = DateTime.Now.AddDays(-6);

        public StatisticsWindow()
        {
            InitializeComponent();
            Uri uri = new Uri("pack://siteoforigin:,,,/Resources/Без имени-2.png");
            BitmapImage bitmap = new BitmapImage(uri);
            img.Source = bitmap;
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            using (var db = new LanguageTutorialContext())
            {
                var activeLanguages = db.Course.Where(c => c.Active && c.UserId == App.oActiveUser.Id);
                foreach (var l in activeLanguages)
                {
                    if (l.LanguageId == 1)
                    {
                        ComboBoxLanguage.Items.Add("English");
                        ActiveCourseID[0] = l.Id;
                    }
                    else
                    {
                        ComboBoxLanguage.Items.Add("Français");
                        ActiveCourseID[1] = l.Id;
                    }
                }
            }
            ComboBoxLanguage.SelectedIndex = 0;
        }

        //функция записи в DataGridStatistics сессии за неделю
        private void WeekStatisticsGenegation(int CourseID, DateTime TopBoundary, DateTime BottomBounadry)
        {
            //показ в лэйбле последнего дня отображаемой недели
            LabelTopWeekBoundary.Content = TopBoundary.ToLongDateString();

            //показ в лэйбле первого дня отображаемой недели
            LabelBottomWeekBoundary.Content = BottomBounadry.ToLongDateString();
            if (collection == null)
            {
                collection = new ObservableCollection<StatisticsRow>();
                DataGridStatistics.ItemsSource = collection;
            }
            //очистка коллекции, хранящей строки для отображения статистики
            collection.Clear();
            
            //скрыть надпись, информирующую о том, что на отображаемой
            //неделе не было сессий
            LabelEmptyWeek.Visibility = System.Windows.Visibility.Hidden;
            
            //переменные, хранящие кол-во баллов за неделю и 
            //кол-во отгаданных слов за неделю
            //(слово считается, если было отгаданно без ошибок)
            int PointsForWeek = 0, WordsForWeek = 0;

            using (var db = new LanguageTutorialContext())
            {
                //выбор всех сессий для текущей недели и данного курса
                var sessionsForWeek = db.Session.Where(s => s.Datetime <= TopBoundary
                    && s.Datetime >= BottomBounadry && s.CourseId == CourseID).ToList();
                if (sessionsForWeek != null)
                {
                    foreach (var s in sessionsForWeek)
                    {
                        // Заносим в DataGridStatistics новую строку,
                        // содержащую информацию о дате сессии, 
                        //кол-ве баллов за сессию и кол-ве отгаданных слов за сессию
                        //(слово считается, если было отгаданно без ошибок)
                        collection.Add(new StatisticsRow() {Date = s.Datetime, PointsQuantity = s.Points, WordsQuantity = s.Words});
                        
                        //изменяем общее кол-во баллов за неделю с учетом сессии
                        PointsForWeek += s.Points;

                        //изменяем общее кол-во отгаданных слов 
                        //за неделю с учетом сессии
                        WordsForWeek += s.Words;
                    }
                }

                //если не было сессий на недели
                if (collection.Count == 0)
                {
                    //вывести надпись, информирующую о том, что на отображаемой
                    //неделе не было сессий
                    LabelEmptyWeek.Visibility = System.Windows.Visibility.Visible;
                    LabelFullPointsQuantityResult.Content = "";
                    LabelFullWordsQuantityResult.Content = "";
                }
                else
                {
                    // вывод кол-ва баллов за неделю
                    LabelFullPointsQuantityResult.Content = Convert.ToString(PointsForWeek);
                    
                    // вывод кол-ва отгаданных слов за неделю 
                    LabelFullWordsQuantityResult.Content = Convert.ToString(WordsForWeek);
                }
            }
        }

        private void ComboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // генерация для одного языка статистики за неделю
            WeekStatisticsGenegation(CourseIdFinding(ComboBoxLanguage, ActiveCourseID), WeekTopBoundary, WeekBottomBoundary);
            
            // проверка на существование сессий в последующих неделях
            PreviousWeekExisting(CourseIdFinding(ComboBoxLanguage, ActiveCourseID), WeekBottomBoundary);

            // проверка на существование сессий в предыдущих неделях
            NextWeekExisting(CourseIdFinding(ComboBoxLanguage, ActiveCourseID), WeekTopBoundary);
        }

        //Нахождение границ для следующей недели
        private void NextWeekBoundaryFinding(DateTime BottomBoundary, DateTime TopBoundary)
        {
            TopBoundary = TopBoundary.AddDays(7);

            BottomBoundary = BottomBoundary.AddDays(7);
        }

        //Нахождение границ для предыдущей недели
        private void PreviousWeekBoundaryFinding(DateTime BottomBoundary, DateTime TopBoundary)
        {
            TopBoundary = TopBoundary.AddDays(-7);

            BottomBoundary = BottomBoundary.AddDays(-7);
        }

        //Функция, определяющая есть ли сессии после текущей недели
        private void NextWeekExisting(int CourseID, DateTime TopBoundary)
        {
            List<StatisticsRow> coll = new List<StatisticsRow>();
            using (var db = new LanguageTutorialContext())
            {
                var nextAllSessions = db.Session.Where(s => s.Datetime >= TopBoundary && s.CourseId == CourseID).ToList();
                foreach (var s in nextAllSessions)
                {
                    // Заносим в DataGridStatistics новую строку
                    coll.Add(new StatisticsRow() { Date = s.Datetime.Date, PointsQuantity = s.Points, WordsQuantity = s.Words });
                }
                if (coll.Count == 0)
                {
                    ButtonNextWeek.IsEnabled = false;
                }
                else
                {
                    ButtonNextWeek.IsEnabled = true;
                }
            }
        }

        //Функция, определяющая есть ли сессии до текущей недели
        private void PreviousWeekExisting(int CourseID, DateTime BottomBoundary)
        {
            List<StatisticsRow> coll = new List<StatisticsRow>();
            using (var db = new LanguageTutorialContext())
            {
                var nextAllSessions = db.Session.Where(s => s.Datetime <= BottomBoundary && s.CourseId == CourseID).ToList();
                foreach (var s in nextAllSessions)
                {
                    // Заносим в DataGridStatistics новую строку
                    coll.Add(new StatisticsRow() { Date = s.Datetime.Date, PointsQuantity = s.Points, WordsQuantity = s.Words });
                }
                if (coll.Count == 0)
                {
                    ButtonPreviousWeek.IsEnabled = false;
                }
                else
                {
                    ButtonPreviousWeek.IsEnabled = true;
                }
            }
        }
        
        private int CourseIdFinding( ComboBox ChosenLanguage, int[] ActiveCourseID)
        {
            if (ChosenLanguage.SelectedItem == "English")
            {
                return ActiveCourseID[0];
            }
            else
            {
                return ActiveCourseID[1];
            }
        }

        private void ButtonPreviousWeek_Click(object sender, RoutedEventArgs e)
        {
            WeekTopBoundary = WeekTopBoundary.AddDays(-7);
            WeekBottomBoundary = WeekBottomBoundary.AddDays(-7);
            ButtonNextWeek.IsEnabled = true;
            WeekStatisticsGenegation(CourseIdFinding(ComboBoxLanguage, ActiveCourseID), WeekTopBoundary, WeekBottomBoundary);
            PreviousWeekExisting(CourseIdFinding(ComboBoxLanguage, ActiveCourseID), WeekBottomBoundary);
        }

        private void ButtonNextWeek_Click(object sender, RoutedEventArgs e)
        {
            WeekTopBoundary = WeekTopBoundary.AddDays(7);
            WeekBottomBoundary = WeekBottomBoundary.AddDays(7);
            ButtonPreviousWeek.IsEnabled = true;
            WeekStatisticsGenegation(CourseIdFinding(ComboBoxLanguage, ActiveCourseID), WeekTopBoundary, WeekBottomBoundary);
            NextWeekExisting(CourseIdFinding(ComboBoxLanguage, ActiveCourseID), WeekTopBoundary);
        }
    }
}
