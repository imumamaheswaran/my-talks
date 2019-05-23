using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Streaming;
using Microsoft.XLANGs.BaseTypes;
using Kovai.AtomicScope.Common.Helpers;
using Kovai.AtomicScope.Models;
using Kovai.AtomicScope.Common.Models;
using Kovai.AtomicScope.Models.Data;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace Kovai.AtomicScope
{
    [Serializable]
    public  class ActivityLogger1
   {
        
        public ActivityLogger1()
        {

        }
        #region Main activity      

        //public string StartMainActivity(IBaseMessage btsMessage, string interfaceName)
        //{
        //    string mainActivityTrackingId = Guid.NewGuid().ToString();
        //    ActivityHelper activityHelper = new ActivityHelper();
        //    //Remove the context property in biztalk helper.
        //    List<Kovai.AtomicScope.Common.Models.ContextProperty> contextProperties = activityHelper.StartMainActivity(btsMessage, interfaceName);

        //    WriteProperties(btsMessage, contextProperties);

        //    return mainActivityTrackingId;
        //}           //Remove and append the logic to start activity

        //public void UpdateMainActivity(IBaseMessage btsMessage, string interfaceName, string messageType = null, string MessageId = null, string BatchId = null, string SenderId = null, string ReceiverId = null)
        //{
        //    string mainActivityId = string.Empty;
        //    ActivityHelper activityHelper = new ActivityHelper();

        //    //Check if the main activity is already started. If not, start main activity. 

        //    if (btsMessage.Context.Read(ASProperties.MainActivityId, ASProperties.NameSpace) != null)
        //        mainActivityId = (string)btsMessage.Context.Read(ASProperties.MainActivityId, ASProperties.NameSpace);
        //    else
        //    {
        //        mainActivityId = StartMainActivity(btsMessage, interfaceName);
        //    }

        //    List<Kovai.AtomicScope.Common.Models.ContextProperty> contextProperties = activityHelper.UpdateMainActivity(btsMessage, mainActivityId);

        //    WriteProperties(btsMessage, contextProperties);
        //}
        
        
        #endregion

        #region  StartStageActivity

        public string StartActivity( string stageName ,string interfaceName= null , string mainActivityId = null , IBaseMessage btsMessage = null)
        {

            

            ActivityHelper activityHelper = new ActivityHelper();

            string stageActivityId = Guid.NewGuid().ToString();

            //List<Kovai.AtomicScope.Common.Models.ContextProperty> contextProperties = 
            activityHelper.StartStageActivity(stageName, btsMessage, mainActivityId,stageActivityId, interfaceName);

           // WriteProperties(btsMessage, contextProperties);//??

            return stageActivityId; //helps for further archiving 
        }

        public string StartActivity(string stageName, string interfaceName = null, string mainActivityId = null, XLANGMessage unConstructedMessage = null, XLANGMessage constructedMessage = null)
        {
            ActivityHelper activityHelper = new ActivityHelper();

            string stageActivityId = Guid.NewGuid().ToString();

            activityHelper.StartStageActivity(stageName, unConstructedMessage, constructedMessage, mainActivityId, stageActivityId, interfaceName);

            return stageActivityId;
        }

        #endregion

        #region Update Stage Activity
        public void UpdateStageActivity(IBaseMessage  btsMessage, string interfaceName, string stageName)
        {
            ActivityHelper activityHelper = new ActivityHelper();
            activityHelper.UpdateStageActivity(btsMessage, interfaceName, stageName);
           // WriteProperties(btsMessage, contextProperties);
        }

        public void UpdateStageActivity(string stageActivityId, XLANGMessage  xlangMessage, string interfaceName, string stageName)
        {
            ActivityHelper activityHelper = new ActivityHelper();
            //List<Kovai.AtomicScope.Common.Models.ContextProperty> contextProperties = activityHelper.UpdateStageActivity(xlangMessage, stageActivityId, interfaceName, stageName);
            //WriteProperties(xlangMessage, contextProperties);
            activityHelper.UpdateStageActivity(xlangMessage, interfaceName, stageName);
         }

        #endregion

        #region archive
        public void ArchiveMessage(IBaseMessage msg, string ActivityId)
        {
            ActivityHelper activityHelper = new ActivityHelper();
            activityHelper.ArchiveMessage(msg, ActivityId);
        }

        public void ArchiveMessage(XLANGMessage Xmsg, string ActivityId)
        {
            ActivityHelper activityHelper = new ActivityHelper();
            activityHelper.ArchiveMessage(Xmsg, ActivityId);
        }
        #endregion

        #region  Log Exception
        public void logException(string StageActivityId, string ExceptionType, string ExceptionMessage)
        {
            ActivityHelper activityHelper = new ActivityHelper();
            activityHelper.logException(StageActivityId, ExceptionType, ExceptionMessage);
        }
        public void LogSystemException(string stageId, object exceptionDetails)
        {

        }

        public void LogBusinessException(string stageId, object exceptionDetails)
        {

        }

        #endregion

        #region private methods

        private void WriteProperties(IBaseMessage message, List<Kovai.AtomicScope.Common.Models.ContextProperty> properties)
        {
            foreach (Kovai.AtomicScope.Common.Models.ContextProperty prop in properties)
            {
                message.Context.Write(prop.Name, prop.NameSpace, prop.Value);
            }
        }

        private void WriteProperties(XLANGMessage  message, List<Kovai.AtomicScope.Common.Models.ContextProperty> properties)
        {
            foreach (Kovai.AtomicScope.Common.Models.ContextProperty prop in properties)
            {
                //**************************************************************
               //TODO  - Logic to write message context to the xlang message.
               //***************************************************************
            }
        }
        #endregion 
    }
}
