using Ookii.Dialogs.Wpf;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace Lasagne__Modern_UI_
{
    public partial class edit : UserControl
    {
        public static string word = manage.word;
        public static string name, first_folder, second_folder ,boo;
        public edit()
        {
            InitializeComponent();

            //parsing selected task information
            string[] split = word.Split(",".ToCharArray(), 5);
            name = split[1].Substring(8);
            first_folder = split[2].Substring(8);
            second_folder = split[3].Substring(8);
            boo = split[4].Substring(8);
            boo = boo.TrimEnd(" }".ToCharArray());
            second_folder = second_folder.TrimEnd(" }".ToCharArray());
            tb1.Text = name;
            tb2.Text = first_folder;
            tb3.Text = second_folder;
            if (boo == "True")
                checkBox1.IsChecked = true;
        }

        private void bt3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                name = tb1.Text;
                first_folder = tb2.Text;
                second_folder = tb3.Text;

                SQLiteConnection dbConnection;
                dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
                dbConnection.Open();

                //updating new info
                string sql = "update sync set First_Folder=\"" + first_folder + "\",Second_Folder=\"" + second_folder + "\",is_two_way=\"" + boo + "\" where name=\"" + name + "\"";
                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();

                dbConnection.Close();

                String sMessageBoxText = "Sync task updated";
                string sCaption = "Folder Sync";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Information;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
            catch(Exception e1)
            {
                String sMessageBoxText = "Error "+e1.Message;
                string sCaption = "Folder Sync";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
        }

        private void bt2_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog vd = new VistaFolderBrowserDialog();
            vd.RootFolder = System.Environment.SpecialFolder.Desktop;
            vd.ShowDialog();
            tb3.Text = vd.SelectedPath;
        }

        private void bt1_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog vd = new VistaFolderBrowserDialog();
            vd.RootFolder = System.Environment.SpecialFolder.Desktop;
            vd.ShowDialog();
            tb2.Text = vd.SelectedPath;
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
           boo = "True";
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            boo = "False";
        }
    }
}
