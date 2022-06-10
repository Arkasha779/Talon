using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using Talon.Connection;

namespace Talon
{
    /// <summary>
    /// Логика взаимодействия для Rec.xaml
    /// </summary>
    public partial class Rec : Window
    {
        public Rec()
        {
            InitializeComponent();
            CbFill();
        }

        public void CbFill()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConn.myConn))
            {
                try
                {
                    connection.Open();
                    string query1 = $@"SELECT * FROM FIO";
                    string query2 = $@"SELECT * FROM Time";
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                    SQLiteDataAdapter SDA2 = new SQLiteDataAdapter(cmd2);
                    DataTable dt1 = new DataTable("FIO");
                    DataTable dt2 = new DataTable("Time");
                    SDA1.Fill(dt1);
                    SDA2.Fill(dt2);
                    CbDoc.ItemsSource = dt1.DefaultView;
                    CbDoc.DisplayMemberPath = "FIO";
                    CbDoc.SelectedValuePath = "ID";
                    CbTime.ItemsSource = dt2.DefaultView;
                    CbTime.DisplayMemberPath = "Time";
                    CbTime.SelectedValuePath = "ID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Btnback_Click(object sender, RoutedEventArgs e)
        {
            Reg win1 = new Reg();
            win1.Show();
            Close();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConn.myConn))
            {
                connection.Open();
                if (CbDoc.SelectedIndex == -1 || CbTime.SelectedIndex == -1 || String.IsNullOrEmpty(DpDate.Text))
                {
                    MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    int id, id2;
                    bool resultKab = int.TryParse(CbDoc.SelectedValue.ToString(), out id);
                    bool resultCon = int.TryParse(CbTime.SelectedValue.ToString(), out id2);
                    //var UserAdd = Saver.Login;
                    var startWork = DpDate.Text;
                    string query = $@"SELECT  COUNT(1) FROM Rec WHERE FIO=@FIO AND Data=@Data AND Time=@Time";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@FIO", id);
                    cmd.Parameters.AddWithValue("@Data", DpDate.Text);
                    cmd.Parameters.AddWithValue("@Time", id2);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.ExecuteNonQuery();
                    int pr = 0;
                    if (count == 1)
                    {
                        MessageBox.Show("Данное время  в этот день занято", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        pr = 1;
                    }
                    if (pr == 0)
                    {
                        //string query2 = $@"INSERT INTO Doctors(IDSpecialist,IDMedic,IDTime,IDDay,Login) values ('{id}','{id2}','{startWork}','{UserAdd}');";
                        string query2 = $@"INSERT INTO Doctors(IDSpecialist,IDMedic,IDTime,IDDay,Login) values ('{id}','{id2}','{startWork}');";
                        SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                        try
                        {
                            MessageBox.Show("Вы записались к врачу");
                            cmd2.ExecuteNonQuery();

                        }

                        catch (SQLiteException)
                        {

                        }
                    }
                    Print win3 = new Print();
                    win3.Show();
                    Close();
                }
            }
        }
    }
}
