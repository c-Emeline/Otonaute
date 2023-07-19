using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.IO;
using System.Windows.Navigation;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace OTONOTE
{
    /// <summary>
    /// Logique d'interaction pour EveryNotesOverviewPage.xaml
    /// </summary>
    public partial class EveryNotesOverviewPage : Page
    {

        private string noteDirectory;
        private const int COLUMNNBNOTES = 5;
        private const int COLUMNNBDATE = 6;
        private const int NOTEICONHEIGTH_PX = 60;

        public EveryNotesOverviewPage()
        {
            InitializeComponent();
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            this.noteDirectory = System.IO.Path.Combine(projectDirectory, "notes");

            displayYearsButtons();

            fillGrid(this.noteDirectory);
        }

        public void fillGrid(string directoryPath)
        {
            notesGrid.Children.Clear();
            notesGrid.RowDefinitions.Clear();
            notesGrid.ColumnDefinitions.Clear();

            //construct columns
            for (int i = 0; i<COLUMNNBNOTES; i++) notesGrid.ColumnDefinitions.Add(new ColumnDefinition());

            //create every note display and add it to the grid
            try
            {
                var noteFiles = Directory.EnumerateFiles(directoryPath, "*.txt", SearchOption.AllDirectories);

                int i = 0;
                foreach (string currentNoteFile in noteFiles)
                {
                    string noteName = "Note of " + System.IO.Path.GetFileName(currentNoteFile).Substring(0, 8);

                    //create the stack panel to display notes
                    TextBlock noteNameTextBlock = new TextBlock();
                    Image noteIcon = new Image();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(@"C:/Users/ecreu/Documents/p_info/OTONOTE/OTONOTE/src/img/noteIcon.png");
                    bitmap.EndInit();
                    noteIcon.Source = bitmap;
                    noteIcon.Height= NOTEICONHEIGTH_PX;

                    StackPanel noteStackPanel = new StackPanel
                    {
                        Name = "noteStackPanel",
                        Orientation = Orientation.Vertical
                    };
                    
                    noteNameTextBlock.Text = noteName;
                    noteStackPanel.Children.Add(noteIcon);
                    noteStackPanel.Children.Add(noteNameTextBlock);

                    noteStackPanel.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(goToNotePage);


                    if (i%COLUMNNBNOTES == 0)
                    {
                        notesGrid.RowDefinitions.Add(new RowDefinition());
                    }

                    Grid.SetRow(noteStackPanel, (int) i/COLUMNNBNOTES);
                    Grid.SetColumn(noteStackPanel, i%COLUMNNBNOTES);

                    notesGrid.Children.Add(noteStackPanel);
                    i++;

                    //STYLE
                    noteIcon.Margin= new Thickness(4);
                    noteStackPanel.Margin = new Thickness(20);
                    noteNameTextBlock.Foreground = Brushes.WhiteSmoke;

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

        }

        public void goToNotePage(object sender, RoutedEventArgs e )
        {
            string noteName = null;
            var noteStackPanel = sender as StackPanel;

            if ( noteStackPanel == null ) 
            {
                Console.WriteLine("Error : No stack panel for click event !");
                return;
            }

            Console.WriteLine("Event sent on click on " + noteStackPanel.Name);

            foreach (UIElement element in noteStackPanel.Children)
            {
                if (element is TextBlock)
                {
                    TextBlock textBlock = (TextBlock) element;
                    noteName = textBlock.Text;
                    break;
                }
            }

            if (noteName == null)
            {
                Console.WriteLine("Error : Name of note file not found !");
                return;
            }

            DateTime noteDate = DateTime.Parse(noteName.Substring(noteName.Length - 8, 2) + "/" + noteName.Substring(noteName.Length - 6, 2) + "/" + noteName.Substring(noteName.Length - 4, 4));

            NotePage notePage = new NotePage(noteDate);
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(notePage);
        }

        public void goBackOnClick(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                //TODO : see if usefull, Erase grids content before leaving
                yearSelectionGrid.Children.Clear();
                yearSelectionGrid.RowDefinitions.Clear();

                this.NavigationService.GoBack();
            }
            else
            {
                Console.WriteLine("No entries in back navigation history.");
            }
        }

        private void displayYearsButtons()
        {
            //Erase previous content
            yearSelectionGrid.Children.Clear();
            yearSelectionGrid.RowDefinitions.Clear();
            yearSelectionGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < COLUMNNBDATE; i++) yearSelectionGrid.ColumnDefinitions.Add(new ColumnDefinition());

            try
            {
                var yearDirectories = Directory.EnumerateDirectories(this.noteDirectory);
                yearDirectories.Reverse();

                Console.WriteLine("Print year directories: " + yearDirectories.ToString());

                int i = 0;
                foreach (string currentDirectory in yearDirectories)
                {
                    Button yearButton = new Button();
                    yearButton.Content = new DirectoryInfo(currentDirectory).Name;
                    yearButton.HorizontalAlignment = HorizontalAlignment.Left;
                    yearButton.Tag = currentDirectory;
                    yearButton.Click+= yearButton_Click;
                    yearButton.MouseEnter += yearButton_MouseEnter;
                    yearButton.MouseLeave += yearButton_MouseLeave;
                    if (i % COLUMNNBNOTES == 0)
                    {
                        yearSelectionGrid.RowDefinitions.Add(new RowDefinition());
                    }

                    Grid.SetRow(yearButton, (int)i / COLUMNNBNOTES);
                    Grid.SetColumn(yearButton, i % COLUMNNBNOTES);

                    yearSelectionGrid.Children.Add(yearButton);
                    i++;

                    //STYLE
                    yearButton.Margin = new Thickness(4);
                    yearButton.Padding = new Thickness(8);
                    yearButton.Background = Brushes.WhiteSmoke;
                    yearButton.BorderBrush = Brushes.Transparent; //"#00707070"
                }


            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void yearButton_Click(object sender, RoutedEventArgs e)
        {
            Button yearButton = (Button)sender;
            yearButton.MouseEnter-= yearButton_MouseEnter;
            yearButton.MouseLeave-= yearButton_MouseLeave;
            var yearDirectory = (String)((Button)sender).Tag;

            displayMonthButtons(yearDirectory);
        }

        private void monthButton_Click(object sender, RoutedEventArgs e)
        {
            var monthDirectory = (String)((Button)sender).Tag;

            fillGrid(monthDirectory);
        }

        private void yearButton_MouseEnter(object sender, RoutedEventArgs e)
        {
            var yearDirectory = (String)((Button)sender).Tag;
            displayMonthButtons(yearDirectory);
        }

        private void yearButton_MouseLeave(object sender, RoutedEventArgs e)
        {
            monthSelectionGrid.Children.Clear();
            monthSelectionGrid.RowDefinitions.Clear();
            monthSelectionGrid.ColumnDefinitions.Clear();
        }

        private void displayMonthButtons(string yearDirectoryPath)
        {
            //Erase previous content
            monthSelectionGrid.Children.Clear();
            monthSelectionGrid.RowDefinitions.Clear();
            monthSelectionGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < COLUMNNBDATE; i++) monthSelectionGrid.ColumnDefinitions.Add(new ColumnDefinition());

            try
            {
                System.Globalization.DateTimeFormatInfo mfi = System.Globalization.CultureInfo.GetCultureInfo("en-US").DateTimeFormat;

                var monthDirectories = Directory.EnumerateDirectories(yearDirectoryPath);

                fillGrid(yearDirectoryPath);

                int i = 0;
                foreach (string currentDirectory in monthDirectories)
                {
                    Button monthButton = new Button();
                    if (!Int32.TryParse(new DirectoryInfo(currentDirectory).Name, out int monthNumber)) Console.WriteLine("Error: couldn't parse month directory name to int");
                    monthButton.Content = mfi.GetMonthName(monthNumber).ToString();
                    monthButton.HorizontalAlignment = HorizontalAlignment.Left;
                    monthButton.Tag = currentDirectory;
                    monthButton.Click += monthButton_Click;
                    if (i % COLUMNNBNOTES == 0)
                    {
                        monthSelectionGrid.RowDefinitions.Add(new RowDefinition());
                    }

                    Grid.SetRow(monthButton, (int)i / COLUMNNBNOTES);
                    Grid.SetColumn(monthButton, i % COLUMNNBNOTES);

                    monthSelectionGrid.Children.Add(monthButton);
                    i++;

                    //STYLE
                    monthButton.Margin = new Thickness(4);
                    monthButton.Padding = new Thickness(8);
                    monthButton.Background = Brushes.LightGray;
                    monthButton.BorderBrush = Brushes.Transparent; //"#00707070"
                }


            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
