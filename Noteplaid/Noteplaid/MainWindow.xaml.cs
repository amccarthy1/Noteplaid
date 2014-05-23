// Noteplaid: Notepad for Hipsters.
// <copyright file="MainWindow.xaml.cs" company="corb.co and Adam McCarthy">
//     corb.co and Adam McCarthy. All rights reserved.
// </copyright>
// <author>Corban Mailloux</author>
namespace Noteplaid
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;    

    /// <summary>
    /// Interaction logic for NOTEPLAID!
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The filename and path of the currently open file, or null if no file is open.
        /// </summary>
        private string currOpenFile = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// Creates the main window, with plaid, of course.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.UpdateTitle(null);
        }

        /// <summary>
        /// Updates the title of the window to include or exclude 
        /// </summary>
        /// <param name="file">File to put in the title (no path)</param>
        private void UpdateTitle(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                this.Title = "Untitled - Noteplaid";
            }
            else
            {
                this.Title = file + " - Noteplaid";
            }
        }

        /// <summary>
        /// Open a text file into Noteplaid.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void OpenCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Set filter
            openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";

            // Call the ShowDialog method to show the dialog box.
            if (openFileDialog.ShowDialog() == true)
            {
                // Read the file as one string into the MainTextBox.
                using (StreamReader file = new StreamReader(openFileDialog.FileName)) 
                {
                    MainTextBox.Text = file.ReadToEnd();
                }

                this.currOpenFile = openFileDialog.FileName;
                this.UpdateTitle(openFileDialog.SafeFileName);
            }
        }
        
        /// <summary>
        /// Check if the Open command can execute.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void OpenCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            // Open can always execute, for now.
            e.CanExecute = true;
        }

        /// <summary>
        /// Open a new text file in Noteplaid.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void NewCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            // Should check for saving...
            MainTextBox.Clear();
            this.currOpenFile = null;
            this.UpdateTitle(null);
        }

        /// <summary>
        /// Check if the New command can execute.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void NewCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            // New can always execute.
            e.CanExecute = true;
        }

        /// <summary>
        /// Save the current text to an open file, or call SaveAs if no file is loaded.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void SaveCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.currOpenFile))
            {
                // Call SaveAs
                this.SaveAsCommand_Executed(sender, e);
            }
            else
            {
                if (!this.SaveTextToFile(this.MainTextBox.Text, this.currOpenFile))
                {
                    MessageBox.Show("Error saving.");
                }
            }
        }

        /// <summary>
        /// Check if the Save command can execute.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void SaveCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            // Save can always execute.
            e.CanExecute = true;
        }

        /// <summary>
        /// Prompt the user for a save location, and then save the current text
        /// to the file.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void SaveAsCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            // Create an instance of the save file dialog box.
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();

            // Set dialog settings
            saveFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            saveFileDialog.FileName = "Untitled";
            saveFileDialog.DefaultExt = ".txt";

            // Call the ShowDialog method to show the dialog box.
            if (saveFileDialog.ShowDialog() == true)
            {
                if (this.SaveTextToFile(this.MainTextBox.Text, saveFileDialog.FileName))
                {
                    this.currOpenFile = saveFileDialog.FileName;
                    this.UpdateTitle(saveFileDialog.SafeFileName);
                }
                else
                {
                    MessageBox.Show("Error saving.");
                }
            }
        }

        /// <summary>
        /// Check if the SaveAs command can execute.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void SaveAsCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            // SaveAs can always execute.
            e.CanExecute = true;
        }

        /// <summary>
        /// Save the text to the file.
        /// </summary>
        /// <param name="text">The text to save</param>
        /// <param name="file">The filename</param>
        /// <returns>true for success; false for failure</returns>
        private bool SaveTextToFile(string text, string file)
        {
            try
            {
                File.WriteAllText(file, text);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Exit the application.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Close
            Application.Current.Shutdown(0);
        }
    }
}
