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

        // коллекция, хранящая строки в DataGridStatistics
        ObservableCollection<StatisticsRow> collection;

        DateTime WeekTopBoundary = DateTime.Now;

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
            LabelEmptyWeek.Visibility = System.Windows.Visibility.Hidden;
            using (var db = new LanguageTutorialContext())
            {
                var activeLanguages = db.Course.Where(c => c.Active);
                foreach (var l in activeLanguages)
                {
                    if (l.LanguageId == 1)
                    {
                        ComboBoxLanguage.Items.Add("English");
                    }
                    else
                    {
                        ComboBoxLanguage.Items.Add("Français");
                    }
                }
            }
            ComboBoxLanguage.SelectedIndex = 0;
        }

        //функция записи в DataGridStatistics сессии за неделю
        private void WeekStatisticsGenegation(int CourseID, DateTime TopBoundary, DateTime BottomBounadry)
        {
            LabelTopWeekBoundary.Content = TopBoundary.ToLongDateString();
            LabelBottomWeekBoundary.Content = BottomBounadry.ToLongDateString();
            // проверка: была ли объявлена коллекция collection раньше
            if (collection == null)
            {
                collection = new ObservableCollection<StatisticsRow>();
                DataGridStatistics.ItemsSource = collection;
            }

            collection.Clear();
            LabelEmptyWeek.Visibility = System.Windows.Visibility.Hidden;
            int PointsForWeek = 0, WordsForWeek = 0;

            using (var db = new LanguageTutorialContext())
            {
                var sessionsForWeek = db.Session.Where(s => s.Datetime <= TopBoundary && s.Datetime >= BottomBounadry && s.CourseId == CourseID).ToList();
                if (sessionsForWeek != null)
                {
                    foreach (var s in sessionsForWeek)
                    {
                        // Заносим в DataGridStatistics новую строку
                        collection.Add(new StatisticsRow() { Date = s.Datetime, PointsQuantity = s.Points, WordsQuantity = s.Words});
                        PointsForWeek += s.Points;
                        WordsForWeek += s.Words;
                    }
                }

                if (collection.Count == 0)
                {
                    LabelEmptyWeek.Visibility = System.Windows.Visibility.Visible;
                    LabelFullPointsQuantityResult.Content = "";
                    LabelFullWordsQuantityResult.Content = "";
                }
                else
                {
                    LabelFullPointsQuantityResult.Content = Convert.ToString(PointsForWeek);
                    LabelFullWordsQuantityResult.Content = Convert.ToString(WordsForWeek);
                }
                
            }
        }

        private void ComboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // генерация для одного языка статистики за неделю
            WeekStatisticsGenegation(ComboBoxLanguage.SelectedIndex + 1, WeekTopBoundary, WeekBottomBoundary);
            
            // проверка на существование сессий в последующих неделях
            PreviousWeekExisting(ComboBoxLanguage.SelectedIndex + 1, WeekBottomBoundary);

            // проверка на существование сессий в предыдущих неделях
            NextWeekExisting(ComboBoxLanguage.SelectedIndex + 1, WeekTopBoundary);
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

        private void ButtonPreviousWeek_Click(object sender, RoutedEventArgs e)
        {
            WeekTopBoundary = WeekTopBoundary.AddDays(-7);
            WeekBottomBoundary = WeekBottomBoundary.AddDays(-7);
            ButtonNextWeek.IsEnabled = true;
            WeekStatisticsGenegation(ComboBoxLanguage.SelectedIndex + 1, WeekTopBoundary, WeekBottomBoundary);
            PreviousWeekExisting(ComboBoxLanguage.SelectedIndex + 1, WeekBottomBoundary);
        }

        private void ButtonNextWeek_Click(object sender, RoutedEventArgs e)
        {
            WeekTopBoundary = WeekTopBoundary.AddDays(7);
            WeekBottomBoundary = WeekBottomBoundary.AddDays(7);
            ButtonPreviousWeek.IsEnabled = true;
            WeekStatisticsGenegation(ComboBoxLanguage.SelectedIndex + 1, WeekTopBoundary, WeekBottomBoundary);
            NextWeekExisting(ComboBoxLanguage.SelectedIndex + 1, WeekTopBoundary);
        }
    }
}
