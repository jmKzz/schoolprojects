using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace koosteteht
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class OlympicRing : Window
    {
        public OlympicRing()
        {
            InitializeComponent();

            //Canvas.SetLeft(ellipse1, 0);
            //Ellipse e1 = (Ellipse)canvas.Children[0];
            //canvas.Children.Clear();
            //Ellipse e1 = new Ellipse();
            //e1.Width = 200; 
            //e1.Height = 200;
            //e1.Stroke = Brushes.Red;
            //e1.StrokeThickness = 5;
            //e1.Fill = Brushes.Red;
            //canvas.Children.Add(e1);

            Canvas.SetZIndex(e1, 2);
            Canvas.SetZIndex(e2, 1);
            //Canvas.SetZIndex(e3, 2);
            Canvas.SetZIndex(e4, 1);
            Canvas.SetZIndex(e5, 2);

            omaCanvas.width = e1.Width;
            omaCanvas.height = e1.Height;
            omaCanvas.InvalidateVisual(); // pakottaa piirron

            ellipse1animation();

        }

        void ellipse1animation()
        {
            DoubleAnimation a = new DoubleAnimation(); // Double-tyyppiselle arvolle
            DoubleAnimation b = new DoubleAnimation(); // Double-tyyppiselle arvolle
            DoubleAnimation c = new DoubleAnimation(); // Double-tyyppiselle arvolle
            DoubleAnimation d = new DoubleAnimation(); // Double-tyyppiselle arvolle
            DoubleAnimation e = new DoubleAnimation(); // Double-tyyppiselle arvolle
            DoubleAnimation f = new DoubleAnimation(); // Double-tyyppiselle arvolle
            DoubleAnimation g = new DoubleAnimation(); // Double-tyyppiselle arvolle
            DoubleAnimation h = new DoubleAnimation(); // Double-tyyppiselle arvolle

            a.From = 196;
            a.To = -200;
            a.Duration = new Duration(TimeSpan.Parse("0:0:5"));

            b.From = 513;
            b.To = 1000;
            b.Duration = a.Duration;

            c.From = 358;
            c.To = 1000;
            c.Duration = a.Duration;

            d.From = c.From;
            d.To = c.To;
            d.Duration = a.Duration;

            e.From = 287;                       // VASEN YLÄ
            e.To = -200;
            e.Duration = a.Duration;

            f.From = 287;                       // OIKEA YLÄ
            f.To = -200;
            f.Duration = a.Duration;

            g.From = 271;                       // VASEN ALA
            g.To = -200;
            g.Duration = a.Duration;

            h.From = 445;                       // OIKEA ALA
            h.To = 1000;
            h.Duration = a.Duration;


            Storyboard animateStoryboard = new Storyboard();

            Storyboard.SetTargetName(a, e1.Name);
            Storyboard.SetTargetProperty(a, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTargetName(e, e1.Name);
            Storyboard.SetTargetProperty(e, new PropertyPath(Canvas.TopProperty));

            Storyboard.SetTargetName(b, e5.Name);
            Storyboard.SetTargetProperty(b, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTargetName(f, e5.Name);
            Storyboard.SetTargetProperty(f, new PropertyPath(Canvas.TopProperty));

            Storyboard.SetTargetName(c, e2.Name);
            Storyboard.SetTargetProperty(c, new PropertyPath(Canvas.TopProperty));
            Storyboard.SetTargetName(g, e2.Name);
            Storyboard.SetTargetProperty(g, new PropertyPath(Canvas.LeftProperty));


            Storyboard.SetTargetName(d, e4.Name);
            Storyboard.SetTargetProperty(d, new PropertyPath(Canvas.TopProperty));
            Storyboard.SetTargetName(h, e4.Name);
            Storyboard.SetTargetProperty(h, new PropertyPath(Canvas.LeftProperty));

            animateStoryboard.Children.Add(a);
            animateStoryboard.Children.Add(b);
            animateStoryboard.Children.Add(c);
            animateStoryboard.Children.Add(d);
            animateStoryboard.Children.Add(e);
            animateStoryboard.Children.Add(f);
            animateStoryboard.Children.Add(g);
            animateStoryboard.Children.Add(h);

            animateButton.Click += delegate (object sender, RoutedEventArgs args)
            {
                animateStoryboard.Begin(e1, true);
                animateStoryboard.Begin(e5, true);
                animateStoryboard.Begin(e2, true);
                animateStoryboard.Begin(e4, true);
            };

            resetButton.Click += delegate (object sender, RoutedEventArgs args)
            {
                animateStoryboard.Stop(e1);
                animateStoryboard.Stop(e5);
                animateStoryboard.Stop(e2);
                animateStoryboard.Stop(e4);

                animateStoryboard.Remove(e1);
                animateStoryboard.Remove(e5);
                animateStoryboard.Remove(e2);
                animateStoryboard.Remove(e4);


            };
        }
    }
}
