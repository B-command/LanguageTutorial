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
    /// Логика взаимодействия для WindowLanguage.xaml
    /// </summary>
    public partial class WindowLanguage {
        public WindowLanguage() {
            InitializeComponent();

            Uri uri = new Uri("pack://siteoforigin:,,,/Resources/catsdogs1.png");
            BitmapImage bitmap = new BitmapImage(uri);
            img.Source = bitmap;

            if (cb_language.SelectedIndex == -1) {
                bt_continue.IsEnabled = false;
            }
        }

        private void bt_continue_Click(object sender, RoutedEventArgs e) {
            Close();
            ///!!!!!!!!если английский, то передать параметр 1 в конструктор, если нет(французский) -2
            TestWindow test;
            if (cb_language.SelectedIndex == 1) {
                test = new TestWindow(1);
                test.ShowDialog();
            }
            if (cb_language.SelectedIndex == 2) { 
                test = new TestWindow(2);
                test.ShowDialog();
            }
            

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {
            cb_language.Items.Add("English");
            cb_language.Items.Add("Français");
        }

        private void cb_language_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cb_language.SelectedIndex == -1) {
                bt_continue.IsEnabled = false;
            } else {
                bt_continue.IsEnabled = true;
            }
        }

        private void MetroWindow_Closed(object sender, EventArgs e) {
            App.aTimer.Start();
        }

    }
}
