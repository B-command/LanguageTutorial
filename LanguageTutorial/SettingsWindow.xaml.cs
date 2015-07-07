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

namespace LanguageTutorial
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        const int minWordsForSession = 10, maxWordsForSession = 50, defaultWordsForSession = 20,
                  minWordsForStudy = 30, maxWordsForStudy = 100, defaultWordsForStudy = 50,
                  defaultSessionsPerDay = 5,
                  defaultTrueAnsers = 3;

        public SettingsWindow(string Lang)
        {
            InitializeComponent();

            label_Settings.Content = Lang;

            num_Number_of_Words_Per_Seans.Maximum = maxWordsForSession;
            num_Number_of_Words_Per_Seans.Minimum = minWordsForSession;

            num_Number_of_Words_To_Study.Maximum = maxWordsForStudy;
            num_Number_of_Words_To_Study.Minimum = minWordsForStudy;

            Uri uri = new Uri("pack://siteoforigin:,,,/Resources/64180878_1284827358_31.png");
            BitmapImage bitmap = new BitmapImage(uri);
            img.Source = bitmap;
        }

        /// <summary>
        /// Заполняем поля значениями при загрузке окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.oActiveUser != null)
            {
                if (label_Settings.Content == "English")
                {
                    Course tempCourseEnglish = new Course();

                    tempCourseEnglish.WordsPerSession = App.oCourseEnglish.WordsPerSession;
                    tempCourseEnglish.WordsToStudy = App.oCourseEnglish.WordsToStudy;
                    tempCourseEnglish.SeansPerDay = App.oCourseEnglish.SeansPerDay;
                    tempCourseEnglish.TrueAnswers = App.oCourseEnglish.TrueAnswers;

                    grid.DataContext = tempCourseEnglish;
                }
                else
                {
                    Course tempCourseFrançais = new Course();

                    tempCourseFrançais.WordsPerSession = App.oCourseFrançais.WordsPerSession;
                    tempCourseFrançais.WordsToStudy = App.oCourseFrançais.WordsToStudy;
                    tempCourseFrançais.SeansPerDay = App.oCourseFrançais.SeansPerDay;
                    tempCourseFrançais.TrueAnswers = App.oCourseFrançais.TrueAnswers;

                    grid.DataContext = tempCourseFrançais;
                }
            }


        }

        /// <summary>
        /// Применяем выбранные настройки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Accept_Click(object sender, RoutedEventArgs e)
        {
            if (label_Settings.Content == "English")
            {
                try
                {
                    App.oCourseEnglish.WordsPerSession = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oCourseEnglish.WordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oCourseEnglish.SeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oCourseEnglish.TrueAnswers = (int)num_Number_of_True_Answer.Value;
                }
                catch
                {
                    this.Close(); //Если какие-то из значений пустые, то происходит отмена изменений
                }

            }
            else
            {
                try
                {
                    App.oCourseFrançais.WordsPerSession = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oCourseFrançais.WordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oCourseFrançais.SeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oCourseFrançais.TrueAnswers = (int)num_Number_of_True_Answer.Value;
                }
                catch
                {
                    this.Close();  //Если какие-то из значений пустые, то происходит отмена изменений
                }
            }

            this.Close();
        }

        /// <summary>
        /// Отмена изменения настроек и выход из них
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void num_Number_of_Words_Per_Seans_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (num_Number_of_Words_Per_Seans.Value > num_Number_of_Words_To_Study.Value) num_Number_of_Words_Per_Seans.Value = num_Number_of_Words_To_Study.Value;
        }

        private void num_Number_of_Words_To_Study_LostFocus(object sender, RoutedEventArgs e)
        {
            // if (num_Number_of_Words_Per_Seans.Value > num_Number_of_Words_To_Study.Value) num_Number_of_Words_To_Study = num_Number_of_Words_Per_Seans;
        }

        private void num_Number_of_Seans_Per_Day_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (num_Number_of_Seans_Per_Day.Value == null)
            {
                num_Number_of_Seans_Per_Day.Value = defaultSessionsPerDay;
            }
        }

        private void num_Number_of_Words_To_Study_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (num_Number_of_Words_Per_Seans.Value > num_Number_of_Words_To_Study.Value)
            {
                num_Number_of_Words_Per_Seans.Maximum = (double)num_Number_of_Words_To_Study.Value;
            }

            if (num_Number_of_Words_To_Study.Value == null)
            {
                num_Number_of_Words_To_Study.Value = defaultWordsForStudy;
            }
        }

        private void num_Number_of_Words_Per_Seans_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (num_Number_of_Words_Per_Seans.Value > num_Number_of_Words_To_Study.Value)
            {
                num_Number_of_Words_Per_Seans.Maximum = (double)num_Number_of_Words_To_Study.Value;
            }

            if (num_Number_of_Words_Per_Seans.Value == null)
            {
                num_Number_of_Words_Per_Seans.Value = defaultWordsForSession;
            }
        }

        private void num_Number_of_True_Answer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (num_Number_of_True_Answer.Value == null)
            {
                num_Number_of_True_Answer.Value = defaultTrueAnsers;
            }
        }
    }
}
