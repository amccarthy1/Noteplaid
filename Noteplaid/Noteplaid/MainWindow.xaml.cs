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
    using System.Windows.Documents;

    /// <summary>
    /// Interaction logic for NOTEPLAID!
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The filename and path of the currently open file, or null if no file is open.
        /// </summary>
        private string currOpenFile = null;
        private bool isRichTextEnabled;
        public bool RichTextEnabled
        {
            get
            {
                return isRichTextEnabled;
            }

            set
            {
                isRichTextEnabled = value;
                this.RichTextMenuItem.IsChecked = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// Creates the main window, with plaid, of course.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.UpdateTitle(null);
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                this.OpenFile(this.MainTextBox, args[1]);
            }
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
            openFileDialog.Filter = "Text Files (*.txt, *.rtf)|*.txt;*.rtf|All Files|*.*";

            // Call the ShowDialog method to show the dialog box.
            if (openFileDialog.ShowDialog() == true)
            {
                this.OpenFile(this.MainTextBox, openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Open the given file into Noteplaid.
        /// </summary>
        /// <param name="textField">The textbox to populate.</param>
        /// <param name="filename">The filename to open</param>
        private void OpenFile(RichTextBox textField, string filename)
        {
            // Determine file type
            string format;
            if (Path.GetExtension(filename).ToLower() == ".rtf")
            {
                format = DataFormats.Rtf;
                this.RichTextEnabled = true;
            }
            else
            {
                format = DataFormats.Text;
                this.RichTextEnabled = false;
            }

            // Try to open the file
            try
            {
                TextRange t = new TextRange(
                    textField.Document.ContentStart,
                    textField.Document.ContentEnd);
                FileStream stream = new FileStream(filename, FileMode.Open);
                t.Load(stream, format);
                stream.Close();

                this.currOpenFile = filename;
                this.UpdateTitle(Path.GetFileName(filename));
            }
            catch (Exception)
            {
                // Error opening file. Open a blank window.
                MessageBox.Show("Error opening file.");
                this.currOpenFile = null;
                this.UpdateTitle(null);
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
            // TODO: Clear text MainTextBox.Clear();
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
                if (!this.SaveFile(this.MainTextBox, this.currOpenFile, RichTextMenuItem.IsChecked))
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
            saveFileDialog.Filter = "Plain Text File|*.txt|Rich Text File|*.rtf|All Files|*.*";
            saveFileDialog.FileName = "Untitled";
            if (RichTextMenuItem.IsChecked)
            {
                saveFileDialog.DefaultExt = ".rtf";
            }
            else
            {
                saveFileDialog.DefaultExt = ".txt";
            }
            
            // Call the ShowDialog method to show the dialog box.
            if (saveFileDialog.ShowDialog() == true)
            {
                if (this.SaveFile(this.MainTextBox, saveFileDialog.FileName, RichTextMenuItem.IsChecked))
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
        /// <param name="textField">The text field to save from.</param>
        /// <param name="file">The filename</param>
        /// <param name="isRichText">Indicates is RTF mode is active</param>
        /// <returns>true for success; false for failure</returns>
        private bool SaveFile(RichTextBox textField, string file, bool isRichText)
        {
            // Set the format for saving
            string format;
            if (isRichText)
            {
                format = DataFormats.Rtf;
            }
            else
            {
                format = DataFormats.Text;
            }

            // Try to save the file
            try
            {
                TextRange t = new TextRange(
                    textField.Document.ContentStart,
                    textField.Document.ContentEnd);
                FileStream stream = new FileStream(file, FileMode.Create);
                t.Save(stream, format);
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

        /// <summary>
        /// Enable spell check when the menu item is checked.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void SpellCheckMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            this.MainTextBox.SpellCheck.IsEnabled = true;
        }

        /// <summary>
        /// Disable spell check when the menu item is unchecked.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void SpellCheckMenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            this.MainTextBox.SpellCheck.IsEnabled = false;
        }

        /// <summary>
        /// Additional logic for when RTF is enabled.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void RichTextMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            // Maybe this isn't the best way to handle this, but it solves an 
            // issue where saving after switching RTF will mess up the output.
            this.OnChangeType();
            this.RichTextEnabled = true;
        }

        /// <summary>
        /// Additional logic for when RTF is disabled.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments passed</param>
        private void RichTextMenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            // Maybe this isn't the best way to handle this, but it solves an 
            // issue where saving after switching RTF will mess up the output.
            this.OnChangeType();
            this.RichTextEnabled = false;
        }

        /// <summary>
        /// Called to do additional logic when RTF mode is toggled.
        /// </summary>
        private void OnChangeType()
        {
            this.currOpenFile = null;
            this.UpdateTitle(null);
        }

        /// <summary>
        /// Check if rich text is enabled. Used by command bindings in the XAML file to enable/disable
        /// rich text functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isRichText(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.RichTextEnabled;
            e.Handled = true;
        }
    }
}
