using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Toolbox;
using System.Windows.Forms;
using Toolbox.Library;
using Toolbox.Library.Forms;
using Toolbox.Library.IO;
using Syroot.BinaryData;
using Toolbox.Library.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using FirstPlugin.GUI.RSizeTable;

namespace FirstPlugin.FileFormats
{
    public class RESTBL : IEditor<UserControl>, IFileFormat
    {
        public FileType FileType { get; set; } = FileType.Parameter;
        public bool CanSave { get; set; }
        public string[] Description { get; set; } = new string[] { "RESTBL" };
        public string[] Extension { get; set; } = new string[] { "*.rsizetable" };
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public IFileInfo IFileInfo { get; set; }

        public Type[] Types
        {
            get
            {
                List<Type> types = new List<Type>();
                //types.Add(typeof(MenuExt));
                return types.ToArray();
            }
        }  

        public void FillEditor(UserControl Editor)
        {
            throw new NotImplementedException();
        }

        public bool Identify(System.IO.Stream stream)
        {
            using (var reader = new Toolbox.Library.IO.FileReader(stream, true))
            {
                if (stream.Length <= 22)
                    return false;
                if (reader.CheckSignature(6, "RESTBL"))
                {
                    Debug.WriteLine("RESTBL Found!");
                    return true;
                }
                return false;
            }
        }

        public void Load(Stream stream)
        {
            CanSave = false;
        }

        public UserControl OpenForm()
        {
            Debug.WriteLine("RESTBL OpenForm() called!");
            RestBlEditor editor = new RestBlEditor();
            editor.FileFormat = this;
            editor.Text = FileName;
            editor.Dock = DockStyle.Fill;
            return editor;
        }

        public void Save(Stream stream)
        {

        }

        public void Unload()
        {

        }
    }
}
