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

        public class ModelExpectation {
            public string charName, objName;
            public Color teamColor;
            public List<string> hiddenObjects;
        }
        public static List<ModelExpectation> modelExpectations = new List<ModelExpectation> {
            new ModelExpectation {
                charName = "Callie",
                objName = "Npc_IdolA",
                teamColor = Color.FromArgb(255, 225, 90, 180),
                hiddenObjects = new List<string> {
                    "Eyaball10__M_Eyelids",
                    "Face_Remake_polySurface98_1__M_NpcIdolA_Face",
                    "Face_Remake_polySurface99Mouse02_KimeModel_Root__M_NpcIdolA_Face",
                    "Face_Remake_polySurface98_2__M_NpcIdolA_Face",
                    "Face_Remake_polySurface98_3__M_NpcIdolA_Face",
                    "Face_Remake_polySurface98_4__M_NpcIdolA_Face",
                    "Face_Remake_polySurface98_5__M_NpcIdolA_Face"
                }
            },
            new ModelExpectation {
                charName = "Marie",
                objName = "Npc_IdolB",
                teamColor = Color.FromArgb(255, 220, 255, 80),
                hiddenObjects = new List<string> {
                    "Eyelids1__M_Eyelids",
                    "UnitedFaceMesh2__M_NpcIdolB_Face",
                    "UnitedFaceMesh2_1__M_NpcIdolB_Face",
                    "UnitedFaceMesh2_3__M_NpcIdolB_Face",
                    "Mesh_1__M_NpcIdolB_Face",
                    "UnitedFaceMesh2_4__M_NpcIdolB_Face",
                    "UnitedFaceMesh2_5__M_NpcIdolB_Face"
                }
            },
        };

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
