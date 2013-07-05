using CiView.Recorder;
using CiView.Recorder.Writer;
using CK.Core;
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
using System.Windows.Shapes;

namespace Viewer
{
    /// <summary>
    /// Logique d'interaction pour WriterEditor.xaml
    /// </summary>
    public partial class WriterEditor : Window
    {
        public WriterEditor()
        {
            InitializeComponent();
        }

        private void MenuHelper_Click(object sender, RoutedEventArgs e)
        {
            var menu = sender as MenuItem;
            textBoxEditor.SelectedText = menu.Header.ToString().Substring(1).ToUpper() + " ";
            textBoxEditor.SelectionStart = textBoxEditor.SelectionStart + textBoxEditor.SelectedText.Length;
            textBoxEditor.SelectionLength = 0;
        }

        private void MenuHelperDateTimeNow_Click(object sender, RoutedEventArgs e)
        {
            textBoxEditor.SelectedText = DateTime.UtcNow.ToString() + " ";
            textBoxEditor.SelectionStart = textBoxEditor.SelectionStart + textBoxEditor.SelectedText.Length;
            textBoxEditor.SelectionLength = 0;
        }

        private void MenuHelperLog_Click(object sender, RoutedEventArgs e)
        {
            var menu = sender as MenuItem;
            textBoxEditor.SelectedText = DateTime.UtcNow.ToString() + " LOG " + menu.Header.ToString().Substring(1).ToUpper() + " none" + Environment.NewLine;
            textBoxEditor.SelectionStart = textBoxEditor.SelectionStart + textBoxEditor.SelectedText.Length;
            textBoxEditor.SelectionLength = 0;
        }

        private void MenuHelperOpen_Click(object sender, RoutedEventArgs e)
        {
            var menu = sender as MenuItem;
            textBoxEditor.SelectedText = DateTime.UtcNow.ToString() + " OPEN " + menu.Header.ToString().Substring(1).ToUpper() + " none" + Environment.NewLine;
            textBoxEditor.SelectionStart = textBoxEditor.SelectionStart + textBoxEditor.SelectedText.Length;
            textBoxEditor.SelectionLength = 0;
        }

        private void MenuHelperClose_Click(object sender, RoutedEventArgs e)
        {
            textBoxEditor.SelectedText = DateTime.UtcNow.ToString() + " CLOSE" + Environment.NewLine;
            textBoxEditor.SelectionStart = textBoxEditor.SelectionStart + textBoxEditor.SelectedText.Length;
            textBoxEditor.SelectionLength = 0;
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();

            sfd.DefaultExt = ".log";
            sfd.Filter = "log documents (.log)|*.log";

            Nullable<bool> result = sfd.ShowDialog();

            if (result == true)
            {
                Stream s = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                Save(s);
            }
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog sfd = new Microsoft.Win32.OpenFileDialog();

            sfd.DefaultExt = ".log";
            sfd.Filter = "log documents (.log)|*.log";

            Nullable<bool> result = sfd.ShowDialog();

            if (result != true)
                return;

            textBoxEditor.Text = "";
            using (LogReader logReader = LogReader.Open(sfd.FileName, 4))
            {
                while (logReader.MoveNext())
                {
                    ILogEntry logEntry = logReader.Current;
                    textBoxEditor.Text += logEntry.LogTimeUtc.ToString() + " ";
                    switch (logEntry.LogType)
                    {
                        case LogType.OpenGroup:
                            textBoxEditor.Text += "OPEN ";
                            break;
                        case LogType.CloseGroup:
                            textBoxEditor.Text += "CLOSE" + Environment.NewLine;
                            continue;
                            break;
                        case LogType.Log:
                            textBoxEditor.Text += "LOG ";
                            break;
                        default: break;
                    }
                    //ActivityLogger.EmptyTag 
                    //textBoxEditor.Text += logEntry.LogLevel.ToString().ToUpper() + " " + logEntry.Tags.ToString();
                    textBoxEditor.Text += logEntry.LogLevel.ToString().ToUpper() + " none";
                    textBoxEditor.Text += Environment.NewLine + logEntry.Text + Environment.NewLine;
                }
            }
        }

        private void Save(Stream stream)
        {
            int errNumber = 0;
            ActivityLogger logger = new ActivityLogger();
            LogWriter logWriter = new LogWriter(stream, true, true);

            for (int i = 0; i < textBoxEditor.LineCount; i++)
            {
                logger.Output.RegisterClient(logWriter);
                string line = textBoxEditor.GetLineText(i);
                string[] words = line.Split(' ');
                if (words.Length < 3)
                {
                    errNumber++;
                    continue;
                }
                DateTime dateTime;
                if (!DateTime.TryParse(words[0] + " " + words[1], out dateTime))
                {
                    errNumber++;
                    continue;
                }
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                try
                {
                    switch (words[2].ToLower().Trim())
                    {
                        case "log":
                            //logger.UnfilteredLog(Str2Tags(words[4]), Str2LogLevel(words[3]), textBoxEditor.GetLineText(++i).Trim(), dateTime);
                            logger.UnfilteredLog(ActivityLogger.EmptyTag, Str2LogLevel(words[3]), textBoxEditor.GetLineText(++i).Trim(), dateTime);
                            break;
                        case "open":
                            //logger.OpenGroup(Str2Tags(words[4]), Str2LogLevel(words[3]), null, textBoxEditor.GetLineText(++i).Trim(), dateTime);
                            logger.OpenGroup(ActivityLogger.EmptyTag, Str2LogLevel(words[3]), null, textBoxEditor.GetLineText(++i).Trim(), dateTime);
                            break;
                        case "close":
                            logger.CloseGroup(dateTime);
                            break;
                        default:
                            errNumber++;
                            break;
                    }
                }
                catch(Exception e)
                {
                    errNumber++;
                }
            }
            logWriter.Close(true);
            MessageBoxResult result = MessageBox.Show("there are "+errNumber+" log that have not been saved" ); 
        }

        private CKTrait Str2Tags(string tags)
        {
            return ActivityLogger.RegisteredTags.FindOrCreate(tags);
        }

        private LogLevel Str2LogLevel(string logLevel)
        {
            switch (logLevel.ToLower())
            {
                case "trace":
                    return LogLevel.Trace;
                case "info":
                    return LogLevel.Info;
                case "warn":
                    return LogLevel.Warn;
                case "error":
                    return LogLevel.Error;
                case "fatal":
                    return LogLevel.Fatal;
                default:
                    throw new KeyNotFoundException(logLevel);
            }
        }
    }
}
