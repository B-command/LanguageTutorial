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
            if (label_Settings.Content == "English")
            {
                if ( App.oCourseEnglish == null )
                {
                    App.oCourseEnglish = new Course() { LanguageId = 1, WordsPerSession = 20, WordsToStudy = 50, SeansPerDay = 5, TrueAnswers = 3 };
                }

                Course tempCourseEnglish = new Course();

                tempCourseEnglish.WordsPerSession = App.oCourseEnglish.WordsPerSession;
                tempCourseEnglish.WordsToStudy = App.oCourseEnglish.WordsToStudy;
                tempCourseEnglish.SeansPerDay = App.oCourseEnglish.SeansPerDay;
                tempCourseEnglish.TrueAnswers = App.oCourseEnglish.TrueAnswers;

                grid.DataContext = tempCourseEnglish;
            }
            else
            {
                if ( App.oCourseFrançais == null)
                {
                    App.oCourseFrançais = new Course() { LanguageId = 2, WordsPerSession = 20, WordsToStudy = 50, SeansPerDay = 5, TrueAnswers = 3 };
                }

                Course tempCourseFrançais = new Course();

                tempCourseFrançais.WordsPerSession = App.oCourseFrançais.WordsPerSession;
                tempCourseFrançais.WordsToStudy = App.oCourseFrançais.WordsToStudy;
                tempCourseFrançais.SeansPerDay = App.oCourseFrançais.SeansPerDay;
                tempCourseFrançais.TrueAnswers = App.oCourseFrançais.TrueAnswers;

                grid.DataContext = tempCourseFrançais;
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

        /// <summary>
        /// Запрет на ввод в Numeric
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void num_Number_of_Words_Per_Seans_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void num_Number_of_Words_To_Study_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void num_Number_of_Seans_Per_Day_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void num_Number_of_True_Answer_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
