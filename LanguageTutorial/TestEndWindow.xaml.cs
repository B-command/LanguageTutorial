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

namespace LanguageTutorial
{
    /// <summary>
    /// Interaction logic for TestEndWindow.xaml
    /// </summary>
    public partial class TestEndWindow 
    {
        int LanguageID;
        public TestEndWindow(int language)
        {
            LanguageID = language;

            InitializeComponent();
        }
        public TestEndWindow()
        {
            InitializeComponent();
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //переключится 
            ReturnKursWindow returnKurs = new ReturnKursWindow(LanguageID);
            returnKurs.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblLanguage.TextWrapping =TextWrapping.Wrap;
            if (LanguageID == 1)
            { lblLanguage.Text = "Ты прошел весь курс английского языка"; }
            if (LanguageID == 2)
            { lblLanguage.Text = "Ты прошел весь курс французского языка"; }
        }
    }
}
