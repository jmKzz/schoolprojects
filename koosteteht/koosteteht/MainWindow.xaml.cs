using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;
using MonitoredUndo;

namespace koosteteht
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DrawingAttributes inkDA;
        //DrawingAttributes highlighterDA;
        Image i;
        Brush selectedcolor;
        List<StrokeCollection> testiadded;
        List<StrokeCollection> testiremoved;

        System.Windows.Ink.StrokeCollection _added;
        System.Windows.Ink.StrokeCollection _removed;
        private bool handle = true;
        private int x = 0;

        public MainWindow()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture =
            new System.Globalization.CultureInfo("fi-FI");

            InitializeComponent();
            inkCanvas1.Strokes.StrokesChanged += Strokes_StrokesChanged;
            kumiAlkuValinta();
        }

        public void kumiAlkuValinta()
        {
            eraser1.IsChecked = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            testiadded = new List<StrokeCollection>();
            testiremoved = new List<StrokeCollection>();
            selectedcolor = buttonBlack.Background;
            inkCanvas1.Background = Brushes.White;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Black;

            inkDA = new DrawingAttributes();
            inkDA.Height = 5;
            inkDA.Width = 5;
            inkDA.FitToCurve = false;

            /*highlighterDA = new DrawingAttributes();
            highlighterDA.Color = Colors.Orchid;
            highlighterDA.IsHighlighter = true;
            highlighterDA.IgnorePressure = true;
            highlighterDA.StylusTip = StylusTip.Rectangle;
            highlighterDA.Height = 30;
            highlighterDA.Width = 10;*/


            inkCanvas1.DefaultDrawingAttributes = inkDA;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "FileNameHere"; // Default file name
            dlg.DefaultExt = ".jpg"; // Default file extension
            dlg.Filter = "(.jpg)|*.jpg|(.gif)|*.gif|(.png)|*.png|(.bmp)|*.bmp"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;

                //get the dimensions of the ink control
                int margin = (int)this.inkCanvas1.Margin.Left;
                int width = (int)this.inkCanvas1.ActualWidth - margin;
                int height = (int)this.inkCanvas1.ActualHeight - margin;

                //render ink to bitmap
                RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
                rtb.Render(inkCanvas1);

                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));
                    encoder.Save(fs);
                }
            }
        }

        private void MenuPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
            inkCanvas1.Measure(pageSize);
            inkCanvas1.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));

            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(inkCanvas1, "Printing Canvas");
            }

            ///printDialog.PrintVisual(inkCanvas1, );
        }

        private void MenuManual_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Program functionality: Sliders change values for the stylus and the eraser. " +
                "You can also choose which kind of eraser you want to use. " +
                "The program also has save and print buttons under navigationmenu's 'File'. " +
                "Toolbar also has a lot of colors to choose from. Have fun!");
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This excellent program was made by a student who put his heart and soul into it," +
                " also alot of sweat and tears were shed.");
        }

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {

            string messageBoxText = "New project.Are you sure? All progress will be lost.";
            string caption = "New project";

            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process message box results
            switch (result)
            {
                case MessageBoxResult.OK:
                    // User pressed Yes button
                    // ...
                    this.inkCanvas1.Children.Clear();
                    this.inkCanvas1.Strokes.Clear();
                    tyhjennys();
                    break;
                case MessageBoxResult.Cancel:
                    // User pressed Cancel button
                    // ...
                    break;
            }
        }

        #region Eraser

        private void kumiButton_Click(object sender, RoutedEventArgs e)
        {

            selectButton.IsChecked = false;
            inkCanvas1.EditingMode = InkCanvasEditingMode.None;

            if (kumiButton.IsChecked == false)
            {
                inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            }

            else
            {
                inkCanvas1.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }

        }

        private void eraser1_Checked(object sender, RoutedEventArgs e)
        {
            eraserSize(eraserSizeSlider.Value);
        }

        private void eraser2_Checked(object sender, RoutedEventArgs e)
        {
            eraserSize(eraserSizeSlider.Value);
        }

        private void eraserSize(double erasersize)
        {
            try
            {
                int koko = Convert.ToInt32(Math.Round(erasersize));


                if (eraser1.IsChecked == true)
                {
                    inkCanvas1.EraserShape = new RectangleStylusShape(koko, koko);
                    var editMode = inkCanvas1.EditingMode;
                    inkCanvas1.EditingMode = InkCanvasEditingMode.None;
                    inkCanvas1.EditingMode = editMode;
                }

                else if (eraser2.IsChecked == true)
                {
                    inkCanvas1.EraserShape = new EllipseStylusShape(koko, koko);
                    var editMode = inkCanvas1.EditingMode;
                    inkCanvas1.EditingMode = InkCanvasEditingMode.None;
                    inkCanvas1.EditingMode = editMode;
                }
            }

            catch (Exception ex) { };
        }

        #endregion


        #region ColorButtons

        private void eraserSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            eraserSize(eraserSizeSlider.Value);
        }

        private void buttonYellow_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonYellow.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Yellow;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonGreen_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonGreen.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Green;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonBlue_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonBlue.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Blue;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonRed_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonRed.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Red;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonViolet_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonViolet.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Violet;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonBrown_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonBrown.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Brown;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonTurqoise_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonTurqoise.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Turquoise;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonOrange_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonOrange.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Orange;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonLightBlue_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonLightBlue.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.LightBlue;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonPink_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonPink.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Pink;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonGold_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonGold.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Gold;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonMintCream_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonMintCream.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.MintCream;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonPurple_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonPurple.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Purple;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonDarkRed_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonDarkRed.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.DarkRed;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonTeal_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonTeal.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Teal;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonGray_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonGray.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Gray;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonBlack_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonBlack.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.Black;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        private void buttonWhite_Click(object sender, RoutedEventArgs e)
        {
            selectedcolor = buttonWhite.Background;
            inkCanvas1.DefaultDrawingAttributes.Color = Colors.White;
            inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            choosecolor();
            tyhjennys();
        }

        #endregion


        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = ""; // Default file name
            ofd.DefaultExt = ".jpg"; // Default file extension
            ofd.Filter = "(.jpg)|*.jpg|(.gif)|*.gif|(.png)|*.png|(.bmp)|*.bmp"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = ofd.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                Window1 newproject = new Window1(); // tässä ikkunaluokan nimi=Window1
                                                    //mywindow.Show(); // avaisi hakutyyppisen (modaalisen) ikkunan
                                                    // mieluummin käytä
                                                    //newproject.ShowDialog();

                if (newproject.ShowDialog() == true)
                {
                    int width, height = 0;

                    if (int.TryParse(newproject.openWidth.Text, out width) && int.TryParse(newproject.openHeight.Text, out height))
                    {
                        int.TryParse(newproject.openWidth.Text, out width);
                        int.TryParse(newproject.openHeight.Text, out height);

                        //Math.Round(width);
                        //Math.Round(height);

                        string strfilename = ofd.InitialDirectory + ofd.FileName;
                        inkCanvas1.EditingMode = InkCanvasEditingMode.Select;
                        i = new Image();
                        BitmapImage src = new BitmapImage();
                        src.BeginInit();
                        src.UriSource = new Uri(strfilename, UriKind.Relative);
                        src.CacheOption = BitmapCacheOption.OnLoad;
                        src.EndInit();
                        i.Source = src;
                        i.Stretch = Stretch.Fill;
                        i.Width = width;
                        i.Height = height;
                        //i.Height = 200;
                        //i.Width = 200;
                        this.inkCanvas1.Children.Add(i);
                        //inkCanvas1.Select(inkCanvas1.Strokes, new UIElement[] { i });         // tekee kuvan osaksi canvaksen piirtopohjaa, otettu pois käyttäjäystävällisyyden takia

                        selectButton.IsChecked = true;
                    }

                    else
                    {
                        string messageBoxText = "Invalid picture size";
                        string caption = "Load Error";

                        MessageBoxButton openPictureButton = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Error;

                        MessageBoxResult result2 = MessageBox.Show(messageBoxText, caption, openPictureButton, icon);

                        // Process message box results
                        switch (result2)
                        {
                            case MessageBoxResult.OK:
                                // User pressed Yes button
                                // ...

                                break;
                        }
                    }
                }
            }

        }

        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            kumiButton.IsChecked = false;

            if (selectButton.IsChecked == false)
            {
                inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
            }

            else
            {
                inkCanvas1.EditingMode = InkCanvasEditingMode.Select;
            }
        }

        private void choosecolor()
        {
            selectButton.IsChecked = false;
            kumiButton.IsChecked = false;
        }

        private void ellipseButton_Click(object sender, RoutedEventArgs e)
        {
            int width, height, thickness = 0;

            if (int.TryParse(shapeWidth.Text, out width) && int.TryParse(shapeHeight.Text, out height) && int.TryParse(shapeThickness.Text, out thickness))
            {
                //Canvas.SetLeft(ellipse1, 0);
                Ellipse e1 = new Ellipse();
                e1.Stroke = selectedcolor;

                // Set the width and height of the Ellipse.
                e1.Width = width;
                e1.Height = height;
                e1.StrokeThickness = thickness;
                //inkCanvas1.Children.Clear();
                inkCanvas1.Children.Add(e1);
            }

            else
            {
                shapeError();
            }
        }

        private void rectangleButton_Click(object sender, RoutedEventArgs e)
        {
            int width, height, thickness = 0;

            if (int.TryParse(shapeWidth.Text, out width) && int.TryParse(shapeHeight.Text, out height) && int.TryParse(shapeThickness.Text, out thickness))
            {
                //Canvas.SetLeft(ellipse1, 0);
                Rectangle r1 = new Rectangle();
                r1.Stroke = selectedcolor;

                // Set the width and height of the Ellipse.
                r1.Width = width;
                r1.Height = height;
                r1.StrokeThickness = thickness;
                //inkCanvas1.Children.Clear();
                inkCanvas1.Children.Add(r1);
            }

            else
            {
                shapeError();
            }
        }

        private void shapeError()
        {
            string messageBoxText = "Invalid shape parameters";
            string caption = "Shape Error";

            MessageBoxButton openPictureButton = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBoxResult result4 = MessageBox.Show(messageBoxText, caption, openPictureButton, icon);

            // Process message box results
            switch (result4)
            {
                case MessageBoxResult.OK:
                    // User pressed Yes button
                    // ...

                    break;
            }
        }

        private void MenuOlympic_Click(object sender, RoutedEventArgs e)
        {
            OlympicRing olympicrings = new OlympicRing();

            if (olympicrings.ShowDialog() == true)
            {

            }
        }

        private void MenuRich_Click(object sender, RoutedEventArgs e)
        {
            RikasTeksti richtext = new RikasTeksti();

            if (richtext.ShowDialog() == true)
            {

            }
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            handle = false;
            try
            {
                testiremoved.Add(testiadded[testiadded.Count - 1]);
                inkCanvas1.Strokes.RemoveAt(inkCanvas1.Strokes.Count - 1);
                x++;

            }
            catch (Exception ex)
            {
                MessageBox.Show("No previous strokes to remove");
            }

            handle = true;
        }

        private void Strokes_StrokesChanged(object sender, System.Windows.Ink.StrokeCollectionChangedEventArgs e)
        {
            if (handle)
            {
                _added = e.Added;
                _removed = e.Removed;
           
                testiadded.Add(_added);
            }
        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {
            handle = false;
            try
            {
                inkCanvas1.Strokes.Add(testiadded[testiadded.Count - x]);
                
                x--;
            }

            
            catch(Exception ex)
            {
                MessageBox.Show("No previous strokes to add back");
            }

            handle = true;
        }

        private void tyhjennys()
        {
            testiadded.Clear();
            testiremoved.Clear();
            x = 0;
        }
    }
}

