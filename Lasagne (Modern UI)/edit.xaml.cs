using Ookii.Dialogs.Wpf;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace Lasagne__Modern_UI_ {
    public partial class edit : UserControl {
        public static int no = manage.no;
        public static string name, first_folder, second_folder, boo;
        public static SQLiteConnection dbConnection;
        public edit() {
            InitializeComponent();

            //parsing info
            dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            dbConnection.Open(); no++;
            string sql = "select * from sync where no = " + no;MessageBox.Show(sql);
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                tb1.Text = reader.GetString(1); tb2.Text = reader.GetString(2); tb3.Text = reader.GetString(3);
                boo = reader.GetString(4); comboBox.SelectedIndex = reader.GetInt16(5); comboBox2.SelectedIndex= reader.GetInt16(6);
            }
            dbConnection.Close();
            if (boo == "True")
                checkBox1.IsChecked = true;
        }

        private void bt3_Click(object sender, RoutedEventArgs e) {
            try {
                name = tb1.Text;
                first_folder = tb2.Text;
                second_folder = tb3.Text;

                dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
                dbConnection.Open();

                //updating new info
                string sql = "update sync set First_Folder=\"" + first_folder + "\",Second_Folder=\"" + second_folder + "\",is_two_way=\"" + boo + "\",overwrite2 = "+comboBox.SelectedIndex+" ,overwrite1 = "+comboBox2.SelectedIndex+" where no=\"" + no + "\"";
                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();

                dbConnection.Close();

                String sMessageBoxText = "Sync task updated";
                string sCaption = "Folder Sync";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Information;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
            catch (Exception e1) {
                String sMessageBoxText = "Error " + e1.Message;
                string sCaption = "Folder Sync";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
        }

        private void bt2_Click(object sender, RoutedEventArgs e) {
            VistaFolderBrowserDialog vd = new VistaFolderBrowserDialog();
            vd.RootFolder = System.Environment.SpecialFolder.Desktop;
            vd.ShowDialog();
            tb3.Text = vd.SelectedPath;
        }

        private void bt1_Click(object sender, RoutedEventArgs e) {
            VistaFolderBrowserDialog vd = new VistaFolderBrowserDialog();
            vd.RootFolder = System.Environment.SpecialFolder.Desktop;
            vd.ShowDialog();
            tb2.Text = vd.SelectedPath;
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e) {
            boo = "True";
            comboBox2.IsEnabled = true;
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e) {
            boo = "False";
            comboBox2.IsEnabled = false;
        }
    }
}
