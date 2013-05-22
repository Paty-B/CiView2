using CK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.Model;

namespace Viewer
{
    public class EnvironmentCreator : IActivityLoggerClient
    {
        private LineItem currentLineItem;

        public EnvironmentCreator(LineItemHost lineItemHost)
        {
            currentLineItem = null;
            RootLineItem root = new RootLineItem(lineItemHost);
            currentLineItem = root;
        }   

        public void OnGroupClosed(IActivityLogGroup group, ICKReadOnlyList<ActivityLogGroupConclusion> conclusions)
        {
            currentLineItem = currentLineItem.Parent;
        }

        
        public void OnOpenGroup(IActivityLogGroup group)
        {
            LogLineItem logLineItem = new LogLineItem(group.GroupText, group.GroupLevel, Status.Expanded, group.GroupTags, group.Exception);
            currentLineItem.InsertChild(logLineItem);
            currentLineItem = logLineItem;
        }

        public void OnUnfilteredLog(CKTrait tags, LogLevel level, string text, DateTime logTimeUtc)
        {
            LogLineItem logLineItem = new LogLineItem(text, level, Status.Expanded, tags, null);
            currentLineItem.InsertChild(logLineItem);

        }

        #region useless function

        public void OnGroupClosing(IActivityLogGroup group, ref List<ActivityLogGroupConclusion> conclusions)
        {
            throw new NotImplementedException();
            //useless
        }

        public void OnFilterChanged(LogLevelFilter current, LogLevelFilter newValue)
        {
            throw new NotImplementedException();
            //useless
        }

        #endregion
    }
}
