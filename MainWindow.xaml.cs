using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using Talon.Connection;

namespace Talon
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void UpdateDB()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConn.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT Recipient.ID, Recipient.FIO, Recipient.Num_med
                                        FROM Recipient ";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    DataTable DT = new DataTable("Recipient");
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    SDA.Fill(DT);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConn.myConn))
            {

                if (String.IsNullOrEmpty(TbFIO.Text) || String.IsNullOrEmpty(TbNMP.Text))
                {
                    MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (TbNMP.Text.Length != 16)
                {
                    MessageBox.Show("Номер медицинского полиса должен состоять из 16 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    connection.Open();
                    string query = $@"SELECT  COUNT(1) FROM  RECIPIENT WHERE FIO=@FIO";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FIO", TbFIO.Text.ToLower());

                    string query2 = $@"SELECT  COUNT(1) RECIPIENT WHERE NUM_MED=@NM";
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    cmd2.Parameters.AddWithValue("@NM", TbNMP.Text.ToLower());

                    string query3 = $@"INSERT INTO Recipient ('FIO','Num_med') VALUES (@FIO,@NM)";
                    SQLiteCommand cmd3 = new SQLiteCommand(query3, connection);
                    try
                    {
                        cmd3.Parameters.AddWithValue("@FIO", TbFIO.Text.ToLower());
                        cmd3.Parameters.AddWithValue("@NM", TbNMP.Text.ToLower());
                        cmd3.ExecuteNonQuery();
                        MessageBox.Show("Вход произведён успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Reg win1 = new Reg();
                        win1.Show();
                        Close();
                    }

                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Ошибка" + ex);
                    }
                    UpdateDB();
                }
            }
        }
    }
}


