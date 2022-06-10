using System.Windows;
using System.Windows.Controls;

namespace Talon
{
    /// <summary>
    /// Логика взаимодействия для Print.xaml
    /// </summary>
    public partial class Print : Window
    {
        public Print()
        {
            InitializeComponent();
        }

        private void printBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(card, "");
            }
        }

        private void bb_Click(object sender, RoutedEventArgs e)
        {
            Rec win2 = new Rec();
            win2.Show();
            Close();
        }
    }
}
