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
        private ILineItem currentLineItem;

        public EnvironmentCreator(ILineItemHost host)
        {
            currentLineItem = host.Root;
        }   

        public void OnGroupClosed(IActivityLogGroup group, ICKReadOnlyList<ActivityLogGroupConclusion> conclusions)
        {
            currentLineItem = currentLineItem.Parent;
        }

        
        public void OnOpenGroup(IActivityLogGroup group)
        {
            if (group.Exception != null)
            {
                currentLineItem.InsertChild(LineItem.CreateExceptionLineItem(group.GroupText, group.GroupLevel, group.GroupTags, group.LogTimeUtc, group.Exception));
            }
            else
            {
                currentLineItem.InsertChild(LineItem.CreateLogLineItem(group.GroupText, group.GroupLevel, group.GroupTags, group.LogTimeUtc, true));
            }
            currentLineItem = currentLineItem.LastChild;
        }

        public void OnUnfilteredLog(CKTrait tags, LogLevel level, string text, DateTime logTimeUtc)
        {
            currentLineItem.InsertChild(LineItem.CreateLogLineItem(text,level,tags,logTimeUtc, false));
        }

        #region useless function

        public void OnGroupClosing(IActivityLogGroup group, ref List<ActivityLogGroupConclusion> conclusions)
        {
            //useless
        }

        public void OnFilterChanged(LogLevelFilter current, LogLevelFilter newValue)
        {
            //useless
        }

        #endregion
    }
}
