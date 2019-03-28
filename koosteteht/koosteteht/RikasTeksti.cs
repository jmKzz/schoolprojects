using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace koosteteht
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RikasTeksti : Window
    {
        public RikasTeksti()
        {
            InitializeComponent();
        }

        private void btnGetText_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            MessageBox.Show(textRange.Text);
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextRange tempRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Selection.Start);
            txtStatus.Text = "Selection starts at character #" + tempRange.Text.Length + Environment.NewLine;
            txtStatus.Text += "Selection is " + rtbEditor.Selection.Text.Length + " character(s) long" + Environment.NewLine;
            txtStatus.Text += "Selected text: '" + rtbEditor.Selection.Text + "'";
        }

        private void btnSetText_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            int index = textRange.Text.IndexOf("is");
            TextPointer myTextPointer1 = kpl.ContentStart.GetPositionAtOffset(index+2);
            TextPointer myTextPointer2 = kpl.ContentStart.GetPositionAtOffset(index+4);
            rtbEditor.Selection.Select(myTextPointer1, myTextPointer2);
            rtbEditor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);

        }
    }
}
