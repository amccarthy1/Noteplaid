// Noteplaid: Notepad for Hipsters.
// <copyright file="MainWindow.xaml.cs" company="corb.co and Adam McCarthy">
//     corb.co and Adam McCarthy. All rights reserved.
// </copyright>
// <author>Corban Mailloux</author>
namespace Noteplaid
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for NOTEPLAID!
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// Creates the main window, with plaid, of course.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Open a new text file into Noteplaid.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void OpenCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            // Open a FileSelectDialog.
            //new FileSelectDialog();
        }
        
        /// <summary>
        /// Check if the Open command can execute.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void OpenCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            // Open can always execute, for now.
            e.CanExecute = true;
        }
    }
}
