using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Talon.Connection;

namespace Talon
{
    /// <summary>
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Window
    {
        public Reg()
        {
            InitializeComponent();
            DisplayData();
        }

        public void DisplayData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConn.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT Doctor.ID, Doctor.FIO, Doctor.Spec, Doctor.Cabin, Doctor.Graf
                                        FROM Doctor ";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    DataTable DT = new DataTable("Doctor");
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    SDA.Fill(DT);
                    DGAllEmp.ItemsSource = DT.DefaultView;

                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Rec win2 = new Rec();
            win2.Show();
            Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win0 = new MainWindow();
            win0.Show();
            Close();
        }

        private void DGAllEmp_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedColumn = DGAllEmp.CurrentCell.Column.DisplayIndex;
            var selectedCell = DGAllEmp.SelectedCells[selectedColumn];
            var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
            if (cellContent is TextBlock)
            {
                MessageBox.Show((cellContent as TextBlock).Text, "Подсказка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
