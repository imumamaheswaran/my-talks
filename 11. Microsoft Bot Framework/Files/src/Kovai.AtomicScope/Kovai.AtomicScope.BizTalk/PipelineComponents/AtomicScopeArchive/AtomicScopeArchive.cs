namespace Kovai.AtomicScope.PipelineComponent
{
    using System;
    using System.IO;
    using System.Text;
    using System.Reflection;
    using System.ComponentModel;
    using Microsoft.BizTalk.Component.Interop;
    using Microsoft.BizTalk.Streaming;
    using Kovai.AtomicScope.Helper;


    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("c2b72b41-64ef-40f6-baad-4a407afd670a")]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    public class AtomicScopeArchive : Microsoft.BizTalk.Component.Interop.IComponent, IBaseComponent, IPersistPropertyBag, IComponentUI
    {

        private System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("Kovai.AtomicScope.PipelineComponent.AtomicScopeArchive", Assembly.GetExecutingAssembly());

        private bool _Activate;
        public bool Activate
        {
            get
            {
                return _Activate;
            }
            set
            {
                _Activate = value;
            }
        }

        private string _ConfigFilePath;

        public string ConfigFilePath
        {
            get
            {
                return _ConfigFilePath;
            }
            set
            {
                _ConfigFilePath = value;
            }
        }

        #region IBaseComponent members
        /// <summary>
        /// Name of the component
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get
            {
                return resourceManager.GetString("COMPONENTNAME", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Version of the component
        /// </summary>
        [Browsable(false)]
        public string Version
        {
            get
            {
                return resourceManager.GetString("COMPONENTVERSION", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Description of the component
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get
            {
                return resourceManager.GetString("COMPONENTDESCRIPTION", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        #endregion

        #region IPersistPropertyBag members
        /// <summary>
        /// Gets class ID of component for usage from unmanaged code.
        /// </summary>
        /// <param name="classid">
        /// Class ID of the component
        /// </param>
        public void GetClassID(out System.Guid classid)
        {
            classid = new System.Guid("c2b72b41-64ef-40f6-baad-4a407afd670a");
        }

        /// <summary>
        /// not implemented
        /// </summary>
        public void InitNew()
        {
        }

        /// <summary>
        /// Loads configuration properties for the component
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="errlog">Error status</param>
        public virtual void Load(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, int errlog)
        {
            object val = null;
            val = this.ReadPropertyBag(pb, "Activate");
            if ((val != null))
            {
                this._Activate = ((bool)(val));
            }
            val = this.ReadPropertyBag(pb, "ConfigFilePath");
            if ((val != null))
            {
                this._ConfigFilePath = ((string)(val));
            }
        }

        /// <summary>
        /// Saves the current component configuration into the property bag
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="fClearDirty">not used</param>
        /// <param name="fSaveAllProperties">not used</param>
        public virtual void Save(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, bool fClearDirty, bool fSaveAllProperties)
        {
            this.WritePropertyBag(pb, "Activate", this.Activate);
            this.WritePropertyBag(pb, "ConfigFilePath", this.ConfigFilePath);
        }

        #region utility functionality
        /// <summary>
        /// Reads property value from property bag
        /// </summary>
        /// <param name="pb">Property bag</param>
        /// <param name="propName">Name of property</param>
        /// <returns>Value of the property</returns>
        private object ReadPropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName)
        {
            object val = null;
            try
            {
                pb.Read(propName, out val, 0);
            }
            catch (System.ArgumentException)
            {
                return val;
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException(e.Message);
            }
            return val;
        }

        /// <summary>
        /// Writes property values into a property bag.
        /// </summary>
        /// <param name="pb">Property bag.</param>
        /// <param name="propName">Name of property.</param>
        /// <param name="val">Value of property.</param>
        private void WritePropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName, object val)
        {
            try
            {
                pb.Write(propName, ref val);
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException(e.Message);
            }
        }
        #endregion
        #endregion

        #region IComponentUI members
        /// <summary>
        /// Component icon to use in BizTalk Editor
        /// </summary>
        [Browsable(false)]
        public IntPtr Icon
        {
            get
            {
                return ((System.Drawing.Bitmap)(this.resourceManager.GetObject("COMPONENTICON", System.Globalization.CultureInfo.InvariantCulture))).GetHicon();
            }
        }

        /// <summary>
        /// The Validate method is called by the BizTalk Editor during the build 
        /// of a BizTalk project.
        /// </summary>
        /// <param name="obj">An Object containing the configuration properties.</param>
        /// <returns>The IEnumerator enables the caller to enumerate through a collection of strings containing error messages. These error messages appear as compiler error messages. To report successful property validation, the method should return an empty enumerator.</returns>
        public System.Collections.IEnumerator Validate(object obj)
        {
            // example implementation:
            // ArrayList errorList = new ArrayList();
            // errorList.Add("This is a compiler error");
            // return errorList.GetEnumerator();
            return null;
        }
        #endregion

        #region IComponent members
        /// <summary>
        /// Implements IComponent.Execute method.
        /// </summary>
        /// <param name="pc">Pipeline context</param>
        /// <param name="inmsg">Input message</param>
        /// <returns>Original input message</returns>
        /// <remarks>
        /// IComponent.Execute method is used to initiate
        /// the processing of the message in this pipeline component.
        /// </remarks>
        public Microsoft.BizTalk.Message.Interop.IBaseMessage Execute(Microsoft.BizTalk.Component.Interop.IPipelineContext pc, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg)
        {
            string ActivityID = Guid.NewGuid().ToString();
            string ShareLocation = "D:\\Share\\TrackingArchive\\"; // Should be an UNC Path with desired access granted to the User Account
            string FileExtension = ".txt";
            string FullFilePath = ShareLocation + ActivityID + FileExtension;
            StringBuilder SBContext = new StringBuilder();
            ReadOnlySeekableStream stream = new ReadOnlySeekableStream(inmsg.BodyPart.GetOriginalDataStream());
            Stream sourceStream = inmsg.BodyPart.GetOriginalDataStream();

            if (!sourceStream.CanSeek)
            {
                ReadOnlySeekableStream seekableStream = new ReadOnlySeekableStream(sourceStream);

                inmsg.BodyPart.Data = seekableStream;

                sourceStream = inmsg.BodyPart.Data;
            }

            if (inmsg.BodyPart != null)
            {
                VirtualStream virtualStream = new VirtualStream(sourceStream);

                PipelineHelper.ArchiveToFileLocation(virtualStream, FullFilePath);
                PipelineHelper.ArchivetoStorage(pc, inmsg, FullFilePath, true);

                sourceStream.Position = 0;
                inmsg.BodyPart.Data = sourceStream;
                inmsg.BodyPart.Data.Position = 0;
            }

            return inmsg;
            #endregion

        }
        
    }
}
