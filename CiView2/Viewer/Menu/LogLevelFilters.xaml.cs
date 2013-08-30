﻿using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CK.Core;
using Viewer.Model;

namespace Viewer
{
    /// <summary>
    /// Logique d'interaction pour LogLevelFilters.xaml
    /// </summary>
    public partial class LogLevelFilters : UserControl
    {
        public LogLevelFilters()
        {
            InitializeComponent();
            _listBoxOfCheckBoxCounter.Sort = false;
            _listBoxOfCheckBoxCounter.ForceDelete = false;
            _listBoxOfCheckBoxCounter.Add("Trace");
            _listBoxOfCheckBoxCounter.Add("Info");
            _listBoxOfCheckBoxCounter.Add("Warn").Text = "Warning";
            _listBoxOfCheckBoxCounter.Add("Error");
            _listBoxOfCheckBoxCounter.Add("Fatal");
            _listBoxOfCheckBoxCounter.CheckBoxClick += EventManager.Instance.OnCheckBoxFilterLogLevelClick;

            EventManager.Instance.InsertChild += InsertChild;
            EventManager.Instance.RemoveChild += RemoveChild;
       }

        
    

        private void InsertChild(ILineItem itemImpl, LogLineItem item)
        {
            _listBoxOfCheckBoxCounter.Increase(item.LogLevel.ToString());
        }
        private void RemoveChild(ILineItem itemImpl, LogLineItem item)
        {
            _listBoxOfCheckBoxCounter.Decrease(item.LogLevel.ToString());
        }
    }
}
