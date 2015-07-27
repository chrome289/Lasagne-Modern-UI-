using FirstFloor.ModernUI.Windows.Navigation;
using System;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Lasagne__Modern_UI_ {
    public interface IContent {
        void OnFragmentNavigation(FragmentNavigationEventArgs e);
        void OnNavigatedFrom(NavigationEventArgs e);
        void OnNavigatedTo(NavigationEventArgs e);
        void OnNavigatingFrom(NavigatingCancelEventArgs e);
    }
    public partial class run : IContent {
        public static string sdir = "", ddir = "", boo = "";
        public static bool is_completed = false,its_on = MainWindow.its_on, scouting = false,wasTerminated=false;
        public static int fileCount = 0;
        public static double size = 0.0;
        public run() {
            InitializeComponent();
            pb1.IsEnabled = false;

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
            try {
                //parsing info
                is_completed = true;
                string f = datagrid1.SelectedItem.ToString();
                string[] split = f.Split(",".ToCharArray(), 5);
                string name, first_folder, second_folder;
                name = split[1].Substring(8);
                first_folder = split[2].Substring(8);
                second_folder = split[3].Substring(8);
                boo = split[4].Substring(8);
                boo = boo.TrimEnd(" }".ToCharArray());
                second_folder = second_folder.TrimEnd(" }".ToCharArray());
                sdir = first_folder;
                ddir = second_folder;
                sync();
            }
            catch (NullReferenceException e1) {
                String sMessageBoxText = "Select a Sync Task or Add a new one";
                string sCaption = "Folder Sync";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
        }
        public void sync() {
            //handles the background thread
            pb1.Visibility = Visibility.Visible;
            datagrid1.IsEnabled = false;
            bt1.IsEnabled = false;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerAsync();
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            //restoring gui
            pb1.Visibility = Visibility.Hidden;
            datagrid1.IsEnabled = true;
            bt1.IsEnabled = true;
            if (is_completed == true && !wasTerminated) {
                String sMessageBoxText = "Sync task completed";
                string sCaption = "Folder Sync";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Information;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e) {
            string temp;
            //scouting
            scouting = true;fileCount = 0;size = 0.0;
            ProcessDirectory(sdir); 
            if (boo == "True") {
                temp = sdir;
                sdir = ddir;
                ddir = temp;
                ProcessDirectory(sdir);
            }
            temp = sdir;
            sdir = ddir;
            ddir = temp;
            scouting = false;
            String sMessageBoxText = fileCount + " File/s totaling to " + Math.Round(size/1024/1024,3) + " MB to be synced \r\nContinue ?";
            string sCaption = "Folder Sync";
            MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;
            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            if (rsltMessageBox == MessageBoxResult.OK) {
                its_on = true;
            }
            else {
                its_on = false;
                wasTerminated = true;
            }

            //doing work
            if (its_on) {
                ProcessDirectory(sdir);
                if (boo == "True") {
                    temp = sdir;
                    sdir = ddir;
                    ddir = temp;
                    ProcessDirectory(sdir);
                }
            }
            its_on = false;
        }
        public void ProcessDirectory(string targetDirectory) {
            try {
                // Process the list of files found in the directory. 
                string[] fileEntries = Directory.GetFiles(targetDirectory);
                foreach (string fileName in fileEntries)
                    ProcessFile(fileName);

                // Recurse into subdirectories of this directory. 
                string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory);
            }
            catch (DirectoryNotFoundException e) {
                is_completed = false;
                String sMessageBoxText = "Source Folder not present";
                string sCaption = "Folder Sync";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
            catch(Exception e) {
                String sMessageBoxText = e.InnerException.ToString();
                string sCaption = "Folder Sync";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
        }

        public void ProcessFile(string path) {
            //parsing final file path
            int sdir_len = sdir.Length, path_len = path.Length;
            string cut_sdir_path = path.Substring(sdir_len, (path_len - sdir_len));
            string final_path = ddir + cut_sdir_path;
            try {
                //actual work
                if (File.Exists(final_path) == true) {
                    FileInfo f1 = new FileInfo(final_path);
                    FileInfo f2 = new FileInfo(path);
                    if (f1.Length != f2.Length) {
                        if (!scouting) {
                            Directory.CreateDirectory(Path.GetDirectoryName(final_path));
                            File.Copy(path, final_path, true);
                        }
                        else {
                            fileCount++;
                            size += (f1.Length);
                        }
                    }
                }
                else {
                    if (!scouting) {
                        Directory.CreateDirectory(Path.GetDirectoryName(final_path));
                        File.Copy(path, final_path, true);
                    }
                    else {
                        FileInfo f1 = new FileInfo(path);
                        fileCount++;
                        size += (f1.Length);
                    }
                }
            }
            catch (UnauthorizedAccessException un) {
                String sMessageBoxText = "The Application does not have required permissions to write to this drive.\nRestart as administrator or choose another task";
                string sCaption = "Folder Sync";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
            catch(Exception e) {
                String sMessageBoxText = e.InnerException.ToString();
                string sCaption = "Folder Sync";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
        }
        public void OnFragmentNavigation(FragmentNavigationEventArgs e) {
            MessageBox.Show("hjkhkh");
        }
        public void OnNavigatedFrom(NavigationEventArgs e) {
            MessageBox.Show("hjkhkh");
        }
        public void OnNavigatedTo(NavigationEventArgs e) {
            MessageBox.Show("hjkhkh");
        }
        public void OnNavigatingFrom(NavigatingCancelEventArgs e) {
            // ask user if navigating away is ok
            e.Cancel = true;
            MessageBox.Show("hjkhkh");
        }

        private void ModernWindow_Closing(object sender, CancelEventArgs e) {

        }
    }
}
