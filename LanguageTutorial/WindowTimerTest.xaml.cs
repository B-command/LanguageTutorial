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

namespace LanguageTutorial {
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class WindowTimerTest  {
        public WindowTimerTest() {
            InitializeComponent();
            Uri uri = new Uri("pack://siteoforigin:,,,/Resources/Без имени-5.png");
            BitmapImage bitmap = new BitmapImage(uri);
            img.Source = bitmap;

            if (cb_language.SelectedIndex == -1) {
                button_pass_test.IsEnabled = false;
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {
            if (App.EngSession < TimerMet.numberSessionsLanguageEng()) {
                cb_language.Items.Add("English");
            }
            if (App.FranSession < TimerMet.numberSessionsLanguageFran()) {
                cb_language.Items.Add("Français");
            }
        }

        private void MetroWindow_Closed(object sender, EventArgs e) {
            if (timer == false) {
                App.aTimer.Start();
            }
        }

        bool timer = false;
        private void button_remember_later_Click(object sender, RoutedEventArgs e) {
            timer = false;
            Close();
        }

        private void button_pass_test_Click(object sender, RoutedEventArgs e) {
            timer = true;
            Close();
            App.test = true;
            TestWindow test;
            if (cb_language.SelectedIndex == 0) {
                test = new TestWindow(1);
                test.ShowDialog();
                App.test = false;
            }
            if (cb_language.SelectedIndex == 1) {
                test = new TestWindow(2);
                test.ShowDialog();
                App.test = false;
            }
        }

        private void cb_language_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cb_language.SelectedIndex == -1) {
                button_pass_test.IsEnabled = false;
            } else {
                button_pass_test.IsEnabled = true;
            }
        }


    }
}
