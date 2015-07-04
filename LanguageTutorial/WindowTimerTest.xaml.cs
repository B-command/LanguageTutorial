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
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {
            if (App.EngSession < 5) { //заменить на значение из базы
                cb_language.Items.Add("English");
            }
            if (App.FranSession < 5) {
                cb_language.Items.Add("Français");
            }

            /*if (App.EngSession == 5 || App.FranSession == 5 || App.FranActive == false || App.EngActive == false) { //заменить на значение из базы
                cb_language.Items.Add("English");
            } else {
            
            }*/
        }

        private void MetroWindow_Closed(object sender, EventArgs e) {
            App.aTimer.Start();
        }

        private void button_remember_later_Click(object sender, RoutedEventArgs e) {
            App.aTimer.Start();
            Close();
        }

        private void button_pass_test_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Начать Тестирование - Всплывающее окно (заглушка)");
            App.EngSession++; //убрать когда появится тест
            if (App.EngSession < 5 || App.FranSession < 5) {//переместить код в тест
                App.aTimer.Start();
            } // если не закончились сессии по английскому и французскому
            //заменить константы на данные из бд
        }


    }
}
