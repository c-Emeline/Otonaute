using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OTONOTE
{
    /// <summary>
    /// Logique d'interaction pour NotePage.xaml
    /// </summary>
    public partial class NotePage : Page
    {

        private string yearDirectory;
        private string monthDirectory;
        private string fullPath;

        private DateTime noteDay;
        public DateTime NoteDay { get; set; }

        public NotePage(DateTime noteDay)
        {
            InitializeComponent();
            this.noteDay = noteDay;


            //constuct path
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string noteDirectory = System.IO.Path.Combine(projectDirectory, "notes");

            string fileName = this.noteDay.ToString("ddMMyyyy") + ".txt";

            this.yearDirectory = System.IO.Path.Combine(noteDirectory, this.noteDay.Year.ToString());
            this.monthDirectory = System.IO.Path.Combine(yearDirectory, this.noteDay.Month.ToString());

            this.fullPath = System.IO.Path.Combine(monthDirectory, fileName);


            if (File.Exists(fullPath))
            {
                string noteContent = File.ReadAllText(fullPath);
                noteTxtBox.Text = noteContent;

                Console.WriteLine("[GOOOOD] Note file " + fileName + " content retrieved");
            }
        }

        public void saveOnClick(object sender, RoutedEventArgs e)
        {
            //TODO see if it is usefull to encode it
            //byte[] noteInput = new UTF8Encoding(true).GetBytes("Mahesh Chand");
            string noteInput = noteTxtBox.Text;

            try
            {
                // Creates file and directories if doesn't exist
                Directory.CreateDirectory(yearDirectory);
                Directory.CreateDirectory(monthDirectory);
                var noteFile = File.Create(fullPath);
                noteFile.Close();
                File.SetAttributes(fullPath, FileAttributes.Normal);

                // Write the note in file
                using (StreamWriter outputFile = new StreamWriter(fullPath))
                {
                    outputFile.Write(noteInput, 0, noteInput.Length);
                    outputFile.Close();
                }
                Console.WriteLine("[GOOOOD] Note file " + fullPath.Substring(fullPath.Length - 12) + " updated");
            }
            catch (Exception Ex)
            {
                Console.WriteLine("[ERROR] " + Ex.ToString());
            }
        }

        public void goBackOnClick(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                Console.WriteLine("No entries in back navigation history.");
            }
        }

    }
}
