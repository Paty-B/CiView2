using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CK.Core;
using CiView.Recorder.Writer;
using System.IO;
using CiView.Recorder;

namespace GenerateBinaryLogFileApp
{
    public partial class Form1 : Form
    {
        LogWriter _logWriter;

        public Form1()
        {
            InitializeComponent();
            comboBoxLogLevel.SelectedIndex = 0;
            comboBoxLogType.SelectedIndex = 0;
            buttonAdd.Enabled = false;
            buttonClose.Enabled = false;
        }

        private void buttonAddTag_Click(object sender, EventArgs e)
        {
            string tag = textBoxTag.Text;
            if (!string.IsNullOrEmpty(tag))
            {
                foreach (object o in listBoxTags.Items)
                    if (o.ToString() == tag)
                        return;
                listBoxTags.Items.Add(tag);
            }
        }

        private void buttonDeleteTag_Click(object sender, EventArgs e)
        {
            List<object> objs = new List<object>();
            foreach (object o in listBoxTags.SelectedItems)
                objs.Add(o);
            foreach (object o in objs)
                listBoxTags.Items.Remove(o);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                _logWriter = LogWriter.Create(saveFileDialog.FileName, null);
                buttonAdd.Enabled = true;
                buttonClose.Enabled = true;
                buttonStart.Enabled = false;
            }
            catch
            {
                MessageBox.Show("Sorry there was a problem when writing the file"
                    , "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            _logWriter.Close();
            buttonAdd.Enabled = false;
            buttonClose.Enabled = false;
            buttonStart.Enabled = true;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if(_logWriter == null)
                return;
            string tags = "";
            foreach (object o in listBoxTags.Items)
                tags += o.ToString() + "|";
            tags = tags.Substring(0, tags.Length - 1);
            switch ((LogType)comboBoxLogType.SelectedIndex)
            {
                case LogType.Log :
                    _logWriter.OnUnfilteredLog(ActivityLogger.RegisteredTags.FindOrCreate(tags), (LogLevel)comboBoxLogLevel.SelectedIndex, textBoxText.Text, DateTime.UtcNow);
                    MessageBox.Show("log is add in binary file"
                        , "Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                break;
                case LogType.OpenGroup :
                    FakeLogGroup fakeLogGroup = new FakeLogGroup()
                    {
                        GroupLevel = (LogLevel)comboBoxLogLevel.SelectedIndex,
                        GroupText = textBoxText.Text,
                        GroupTags = ActivityLogger.RegisteredTags.FindOrCreate(tags),
                        LogTimeUtc = DateTime.UtcNow,
                    };
                    _logWriter.OnOpenGroup(fakeLogGroup);
                    MessageBox.Show("log is add in binary file"
                        , "Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                break;
                default:
                    MessageBox.Show("log Type not supported"
                       , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                break;

            }
        }
    }
}
