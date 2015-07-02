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
        public SettingsWindow(string Lang)
        {
            InitializeComponent();

            label_Settings.Content = Lang;
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
                grid.DataContext = App.oSettingsEnglish;
            }
            else
            {
                grid.DataContext = App.oSettingsFrançais;
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
                    App.oSettingsEnglish.NumberOfWordsPerSeans = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oSettingsEnglish.NumberOfWordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oSettingsEnglish.NumberOfSeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oSettingsEnglish.NumberOfTrueAnswers = (int)num_Number_of_True_Answer.Value;
                }
                else
                {
                    App.oSettingsFrançais.NumberOfWordsPerSeans = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oSettingsFrançais.NumberOfWordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oSettingsFrançais.NumberOfSeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oSettingsFrançais.NumberOfTrueAnswers = (int)num_Number_of_True_Answer.Value;
                }
            }
            else
            {// Если пользователь меняет настройки, то изменяем их и в БД

                if (label_Settings.Content == "English")
                {
                    App.oSettingsRepository.lSettings.Remove(App.oSettingsEnglish);

                    App.oSettingsEnglish.NumberOfWordsPerSeans = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oSettingsEnglish.NumberOfWordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oSettingsEnglish.NumberOfSeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oSettingsEnglish.NumberOfTrueAnswers = (int)num_Number_of_True_Answer.Value;

                    App.oSettingsRepository.lSettings.Add(App.oSettingsEnglish);
                }
                else
                {
                    App.oSettingsRepository.lSettings.Remove(App.oSettingsFrançais);

                    App.oSettingsFrançais.NumberOfWordsPerSeans = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oSettingsFrançais.NumberOfWordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oSettingsFrançais.NumberOfSeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oSettingsFrançais.NumberOfTrueAnswers = (int)num_Number_of_True_Answer.Value;

                    App.oSettingsRepository.lSettings.Add(App.oSettingsFrançais);
                }
            }


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
    }
}
