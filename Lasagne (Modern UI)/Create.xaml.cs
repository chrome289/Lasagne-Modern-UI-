using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using Ookii.Dialogs.Wpf;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;


namespace Lasagne__Modern_UI_
{
    public partial class Create : UserControl
    {

        public static bool is_2_way = false;
        public Create()
        {
            InitializeComponent();
            tb1.Text = "";
            tb2.Text = "";
            SQLiteConnection dbConnection;
            dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            dbConnection.Open();

            string sql = "CREATE TABLE if not exists 'sync' (	'S no'	INTEGER PRIMARY KEY AUTOINCREMENT,	'name'	varchar(100) NOT NULL,	'First_Folder'	varchar(500) NOT NULL,	'Second_Folder'	varchar(500) NOT NULL,	'is_two_way'	integer(10) NOT NULL);";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            dbConnection.Close();
        }
        private void bt1_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog vd = new VistaFolderBrowserDialog();
            vd.RootFolder = System.Environment.SpecialFolder.Desktop;
            vd.ShowDialog();
            tb1.Text = vd.SelectedPath;
        }

        private void bt2_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog vd = new VistaFolderBrowserDialog();
            vd.ShowDialog();
            tb2.Text = vd.SelectedPath;
        }

        private void bt3_Click(object sender, RoutedEventArgs e)
        {

            SQLiteConnection dbConnection;
            dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            dbConnection.Open();
            string sql = "insert into sync (Name,First_Folder,Second_Folder,is_two_way) values ('" + tb3.Text + "','" + tb1.Text + "','" + tb2.Text + "','" + is_2_way.ToString() + "')";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            dbConnection.Close();
            tb1.Text = "";
            tb2.Text = "";
            tb3.Text = "";
            String sMessageBoxText = "Sync task saved";
            string sCaption = "Folder Sync";
            MessageBoxButton btnMessageBox = MessageBoxButton.OK;
            MessageBoxImage icnMessageBox = MessageBoxImage.Information;
            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            is_2_way = true;
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            is_2_way = false;
        }
    }
}
