using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Windows;
using System.Windows.Navigation;
using FirstFloor.ModernUI.Windows;

namespace Lasagne__Modern_UI_
{
    public partial class MainWindow : ModernWindow
    {
        public static bool its_on = run.its_on;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ModernWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            its_on = run.its_on;
            if (its_on == true)
            {
                String sMessageBoxText = "Folders are being synced and you dare ABORT ?";
                string sCaption = "Folder Sync";
                MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;
                MessageBoxImage icnMessageBox = MessageBoxImage.Stop;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                if (rsltMessageBox == MessageBoxResult.OK)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
