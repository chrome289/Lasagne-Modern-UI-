using System;
using System.Windows;
using FirstFloor.ModernUI.Windows.Navigation;

namespace Lasagne__Modern_UI_ {

    public partial class MainWindow : IContent {
        public static bool its_on = run.its_on;
        public MainWindow() {
            InitializeComponent();
        }

        private void ModernWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (Sync_Station.Properties.Settings.Default.minimize) {
                e.Cancel = true;
                this.ShowInTaskbar = false;
            }
            else {
                its_on = run.its_on;
                if (its_on == true) {
                    String sMessageBoxText = "Folders are being synced \r\n Abort ?";
                    string sCaption = "Folder Sync";
                    MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Question;
                    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    if (rsltMessageBox == MessageBoxResult.OK) {
                        e.Cancel = false;
                    }
                    else {
                        e.Cancel = true;
                    }
                }
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
    }
}