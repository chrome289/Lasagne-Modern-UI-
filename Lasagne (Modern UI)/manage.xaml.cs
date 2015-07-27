using FirstFloor.ModernUI.Windows.Controls;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace Lasagne__Modern_UI_ {
    public partial class manage : UserControl {
        public static string word = edit.word;
        public manage() {
            InitializeComponent();
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            m_dbConnection.Open();

            //retrieve the task table
            string sql = "select * from sync";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                datagrid1.Items.Add(new { Col1 = reader.GetInt16(0), Col2 = reader.GetString(1), Col3 = reader.GetString(2), Col4 = reader.GetString(3), Col5 = reader.GetString(4) });
            }
            m_dbConnection.Close();
        }

        private void bt1_Click(object sender, RoutedEventArgs e) {
            //opening the edit window
            word = datagrid1.SelectedItem.ToString();
            edit.word = word;
            var wnd = new ModernWindow {
                Style = (Style)App.Current.Resources["BlankWindow"],
                Content = new edit(),
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "Edit Task",
                ResizeMode = ResizeMode.NoResize
            };
            wnd.Show();
        }

        private void bt2_Click(object sender, RoutedEventArgs e) {
            //parsing task name
            word = datagrid1.SelectedItem.ToString();
            string[] split = word.Split(",".ToCharArray(), 5);
            string name = split[1].Substring(8);

            SQLiteConnection dbConnection;
            dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            dbConnection.Open();

            //getting its serial no
            int num = 0;
            string sql = "select no from sync where name=\"" + name + "\"";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                num = reader.GetInt16(0);
                //MessageBox.Show(reader.GetInt16(0).ToString());
            }

            //deleting the task
            sql = "delete from sync where name=\"" + name + "\"";
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();


            //renumbering remaining tasks
            sql = "update sync set no=no-1 where no>" + num.ToString();
            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            datagrid1.Items.Clear();

            //retrieve the task table
            sql = "select * from sync";
            command = new SQLiteCommand(sql, dbConnection);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                datagrid1.Items.Add(new { Col1 = reader.GetInt16(0), Col2 = reader.GetString(1), Col3 = reader.GetString(2), Col4 = reader.GetString(3), Col5 = reader.GetString(4) });
            }

            dbConnection.Close();
        }
    }
}
