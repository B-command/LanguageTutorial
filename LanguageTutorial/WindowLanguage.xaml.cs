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
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Close();
            MessageBox.Show("Начать Тестирование - Всплывающее окно (заглушка)");
            if (App.EngSession < Querry.numberSessionsLanguage("English") || App.FranSession < Querry.numberSessionsLanguage("Français")) {//переместить код в тест
                App.aTimer.Start();
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {
            cb_language.Items.Add("English");
            cb_language.Items.Add("Français");
        }

    }
}
