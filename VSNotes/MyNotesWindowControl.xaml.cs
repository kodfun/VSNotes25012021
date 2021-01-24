using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace VSNotes
{
    /// <summary>
    /// Interaction logic for MyNotesWindowControl.
    /// </summary>
    public partial class MyNotesWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyNotesWindowControl"/> class.
        /// </summary>
        public MyNotesWindowControl()
        {
            this.InitializeComponent();
            LoadNotes();
        }

        public string NotesPath
        {
            get
            {
                string pathDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                return Path.Combine(pathDocuments, "VSNotes");
            }
        }

        // Reads All Txt Files inside MyDocuments>VSNotes folder 
        private void LoadNotes()
        {
            try
            {
                string[] txtFiles = Directory.GetFiles(NotesPath, "*.txt");

                lstNotes.Items.Clear();
                foreach (string txtFile in txtFiles)
                {
                    var noteFile = new NoteFile()
                    {
                        Title = Path.GetFileNameWithoutExtension(txtFile),
                        FileName = Path.GetFileName(txtFile),
                        FilePath = txtFile
                    };
                    lstNotes.Items.Add(noteFile);
                }

            }
            catch (Exception)
            {

            }
        }

        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
            var title = txtTitle.Text.Trim();

            if (CheckIfExists(title))
            {
                MessageBox.Show("The note \"" + title + "\" already exists!");
                return;
            }
            SaveNewNote(txtTitle.Text);
        }

        private bool CheckIfExists(string title)
        {
            string savePath = Path.Combine(NotesPath, title + ".txt");
            return File.Exists(savePath);
        }

        private void SaveNewNote(string title)
        {
            string savePath = Path.Combine(NotesPath, title + ".txt");

            if (!Directory.Exists(NotesPath))
            {
                Directory.CreateDirectory(NotesPath);
            }

            File.WriteAllText(savePath, "", Encoding.Unicode);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadNotes();
        }

        private async void lstNotes_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            // https://stackoverflow.com/a/65875692/15072794
            if (lstNotes.SelectedIndex == -1)
                return;

            NoteFile noteFile = (NoteFile)lstNotes.SelectedItem;

            // https://lib.yemreak.com/programlama/vsix/kod-ornekleri
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE;
            dte.ItemOperations.OpenFile(noteFile.FilePath);

        }
    }
}