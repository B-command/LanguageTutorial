﻿using System;
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

            Uri uri = new Uri("pack://siteoforigin:,,,/Resources/64180878_1284827358_31.png");
            BitmapImage bitmap = new BitmapImage(uri);
            //Image img = new Image();
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
                grid.DataContext = App.oCourseEnglish;
            }
            else
            {
                grid.DataContext = App.oCourseFrançais;
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
                    App.oCourseEnglish.WordsPerSession = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oCourseEnglish.WordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oCourseEnglish.SeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oCourseEnglish.TrueAnswers = (int)num_Number_of_True_Answer.Value;
                }
                else
                {
                    App.oCourseFrançais.WordsPerSession = (int)num_Number_of_Words_Per_Seans.Value;
                    App.oCourseFrançais.WordsToStudy = (int)num_Number_of_Words_To_Study.Value;
                    App.oCourseFrançais.SeansPerDay = (int)num_Number_of_Seans_Per_Day.Value;
                    App.oCourseFrançais.TrueAnswers = (int)num_Number_of_True_Answer.Value;
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
    }
}
