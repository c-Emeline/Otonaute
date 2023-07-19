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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OTONOTE
{
    /// <summary>
    /// Logique d'interaction pour TodayNotePage.xaml
    /// </summary>
    public partial class TodayNotePage : Page
    {

        private string fullPath;
        public string fileName;


        public TodayNotePage()
        {
            InitializeComponent();
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string noteDirectory = System.IO.Path.Combine(projectDirectory, "notes");
            this.fileName = DateTime.Now.ToString("yyyyMMdd") + "_note.txt";
            this.fullPath = System.IO.Path.Combine(noteDirectory, fileName);

            if (File.Exists(fullPath))
            {
                string noteContent = File.ReadAllText(fullPath);
                noteTxtBox.Text = noteContent;
            }
        }

        public void saveOnClick(object sender, RoutedEventArgs e)
        {
            //TODO see if it is usefull to encode it
            //byte[] noteInput = new UTF8Encoding(true).GetBytes("Mahesh Chand");
            string noteInput = noteTxtBox.Text;

            try
            {
                // Creates file if doesn't exist     
                if (!File.Exists(fullPath)) File.Create(fullPath).Close();

                // Write the note in file
                using (StreamWriter outputFile = new StreamWriter(fullPath))
                {
                    outputFile.Write(noteInput, 0, noteInput.Length);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }
}
