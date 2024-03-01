using FirstPlugin.FileFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toolbox.Library.Forms;
using Toolbox.Library;
using ByamlExt.Byaml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FirstPlugin.GUI.RSizeTable
{
    public partial class RestBlEditor : UserControl, IFIleEditor
    {
        public RESTBL FileFormat;

        public List<IFileFormat> GetFileFormats()
        {
            return new List<IFileFormat>() { FileFormat };
        }

        public RestBlEditor()
        {
            InitializeComponent();
            //Reload();
        }
    }
}