using FirstFloor.ModernUI.Windows.Controls;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace Lasagne__Modern_UI_
{
    /// <summary>
    /// Interaction logic for manage.xaml
    /// </summary>
    public partial class manage : UserControl
    {
        public static string word = edit.word;
        public manage()
        {
            InitializeComponent();
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "select * from sync";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                datagrid1.Items.Add(new { Col1 = reader.GetInt16(0), Col2 = reader.GetString(1), Col3 = reader.GetString(2), Col4 = reader.GetString(3), Col5 = reader.GetString(4) });
            }
            m_dbConnection.Close();
        }

        private void bt1_Click(object sender, RoutedEventArgs e)
        {
            word = datagrid1.SelectedItem.ToString();
            edit.word = word;
            var wnd = new ModernWindow
            {
                Style = (Style)App.Current.Resources["BlankWindow"],
                Content = new edit(),
                SizeToContent = SizeToContent.WidthAndHeight
            };
            wnd.Show();
        }

        private void bt2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
