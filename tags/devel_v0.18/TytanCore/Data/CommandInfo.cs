using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualBasic.Compatibility.VB6;
using Microsoft.VisualStudio.CommandBars;

namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Class that exposes info about Command interface in read-only mode.
    /// </summary>
    [Designer(typeof(Window))]
    public class CommandInfo
    {
        private Command command;
        private string shortName;
        private string[] bindings;
        private readonly Image image;
        private readonly int faceID;
        private readonly PictureBox picture;

        /// <summary>
        /// Default constructor of CommandInfo.
        /// </summary>
        public CommandInfo()
        {
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public CommandInfo(Command c)
        {
            Init(c);
        }

        protected void Init(Command c)
        {
            command = c;

            // generate short-name:
            string[] names = command.Name.Split(NamedItemTreeCollection<Command>.SplitChars);

            if (names.Length == 1)
                shortName = names[0];
            shortName = names[names.Length - 1];

            // genarate keyboard bindings:
            object[] kb = command.Bindings as object[];
            if (kb != null && kb.Length == 0)
            {
                bindings = new string[kb.Length];
                for (int i = 0; i < kb.Length; i++)
                    bindings[i] = kb[i].ToString();
            }
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public CommandInfo(DTE2 appObject, CommandBarButton c)
        {
            try
            {
                string guid;
                int id;

                // check if this contains the image:
                appObject.Commands.CommandInfo(c, out guid, out id);
                
                Init(appObject.Commands.Item(guid, id));
                faceID = c.FaceId;
                image = Support.IPictureToImage(c.Picture);
                picture = new PictureBox();
                picture.Image = image;
            }
            catch
            {
            }
        }

        #region Properties

        /// <summary>
        /// Gets the short name of the command.
        /// </summary>
        public string NameShort
        {
            get { return shortName; }
        }

        /// <summary>
        /// Gets the name of the command;
        /// </summary>
        public string Name
        {
            get
            {
                if(command == null)
                    return null;

                return command.Name;
            }
        }

        /// <summary>
        /// Gets the localized name of the command.
        /// </summary>
        public string NameLocalized
        {
            get
            {
                if(command == null)
                    return null;

                return command.LocalizedName;
            }
        }

        /// <summary>
        /// Gets the Guid of the command.
        /// </summary>
        public Guid CommandGuid
        {
            get
            {
                if(command == null)
                    return Guid.Empty;

                return new Guid(command.Guid);
            }
        }

        /// <summary>
        /// Gets the ID of the command.
        /// </summary>
        public uint CommandID
        {
            get
            {
                if (command == null)
                    return 0;

                return (uint)command.ID;
            }
        }

        /// <summary>
        /// Gets the full-command ID.
        /// </summary>
        public string CommandFullID
        {
            get
            {
                if (command == null)
                    return null;

                return string.Format("{0}:{1}", command.Guid, command.ID);
            }
        }

        /// <summary>
        /// Gets the keyboard binding of the command.
        /// </summary>
        public string[] Bindings
        {
            get { return bindings; }
        }

        /// <summary>
        /// Gets the first keyboard binding.
        /// </summary>
        public string BindingFirst
        {
            get
            {
                if (bindings != null && bindings.Length > 0)
                    return bindings[0];

                return null;
            }
        }

        /// <summary>
        /// Gets the image of the button associated with the command.
        /// </summary>
        public Image Image
        {
            get
            {
                if (picture != null)
                    return picture.Image;

                return null;
            }
        }

        /// <summary>
        /// Gets the FaceID of the image.
        /// </summary>
        public int FaceID
        {
            get { return faceID; }
        }

        #endregion
    }
}
