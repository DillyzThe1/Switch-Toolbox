using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toolbox.Library.Forms {
    public partial class TeamColorSelector : Form {
        //public static bool rainbowColor/*Inkantation*/ = false;
        public static Color teamColor = Color.FromArgb(230, 255, 80);

        public TeamColorSelector() {
            InitializeComponent();
        }

        private void TeamColorSelector_Load(object sender, EventArgs e) {
           // rainbowColorCheck.Checked = rainbowColor;
            teamColorPicker.Color = teamColor;
        }

        private void rainbowColorCheck_CheckedChanged(object sender, EventArgs e) {
            //rainbowColor = rainbowColorCheck.Checked;
        }

        private void teamColorPicker_Paint(object sender, PaintEventArgs e) {
            teamColor = teamColorPicker.Color;
            Console.WriteLine("CHANGE TEAM COLOR: " + teamColor);
        }
    }
}
