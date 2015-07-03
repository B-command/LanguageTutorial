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
        const int minWordsForSession = 10, maxWordsForSession = 50, minWordsForStudy = 30, maxWordsForStudy = 100;

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
                grid.DataContext = App.oActiveSettingsEnglish;
            }
            else
            {
                grid.DataContext = App.oActiveSettingsFrançais;
            }
        }

        /// <summary>
        /// Применяем выбранные настройки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Accept_Click(object sender, RoutedEventArgs e)
        {
            if (App.oActiveUser == null)
            {// Если пользователь регистрируется, просто меняем настройки

                if (label_Settings.Content == "English")
                {
                    App.oActiveSettingsEnglish.NumberOfWordsPerSeans = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oActiveSettingsEnglish.NumberOfWordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oActiveSettingsEnglish.NumberOfSeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oActiveSettingsEnglish.NumberOfTrueAnswers = (int)num_Number_of_True_Answer.Value;
                }
                else
                {
                    App.oActiveSettingsFrançais.NumberOfWordsPerSeans = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oActiveSettingsFrançais.NumberOfWordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oActiveSettingsFrançais.NumberOfSeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oActiveSettingsFrançais.NumberOfTrueAnswers = (int)num_Number_of_True_Answer.Value;
                }
            }
            else
            {// Если пользователь меняет настройки, то изменяем их и в БД

                if (label_Settings.Content == "English")
                {
                    App.oSettingsRepository.lSettings.Remove(App.oActiveSettingsEnglish);

                    App.oActiveSettingsEnglish.NumberOfWordsPerSeans = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oActiveSettingsEnglish.NumberOfWordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oActiveSettingsEnglish.NumberOfSeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oActiveSettingsEnglish.NumberOfTrueAnswers = (int)num_Number_of_True_Answer.Value;

                    App.oSettingsRepository.lSettings.Add(App.oActiveSettingsEnglish);
                }
                else
                {
                    App.oSettingsRepository.lSettings.Remove(App.oActiveSettingsFrançais);

                    App.oActiveSettingsFrançais.NumberOfWordsPerSeans = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oActiveSettingsFrançais.NumberOfWordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oActiveSettingsFrançais.NumberOfSeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oActiveSettingsFrançais.NumberOfTrueAnswers = (int)num_Number_of_True_Answer.Value;

                    App.oSettingsRepository.lSettings.Add(App.oActiveSettingsFrançais);
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
            if (num_Number_of_Words_Per_Seans.Value > num_Number_of_Words_To_Study.Value) num_Number_of_Words_Per_Seans.Value = num_Number_of_Words_To_Study.Value;
        }

        private void num_Number_of_Words_To_Study_LostFocus(object sender, RoutedEventArgs e)
        {
            if (num_Number_of_Words_Per_Seans.Value > num_Number_of_Words_To_Study.Value) num_Number_of_Words_To_Study = num_Number_of_Words_Per_Seans;
        }
    }
}
