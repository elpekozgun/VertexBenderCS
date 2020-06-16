using Engine.Core;
using Engine.GLApi;
using OpenTK;
using System;
using System.Windows.Forms;

namespace VertexBenderCS.Forms
{
    public partial class FrmFinalize : Form
    {
        public Action<Mesh, Vector4> BtnProcedeClicked;

        private VolumeRenderer _volRenderer;

        public FrmFinalize(VolumeRenderer volRenderer)
        {
            InitializeComponent();
            SubscribeEvents();

            _volRenderer = volRenderer;

            numericSmoothen.Value = 3;
            numericFill.Value = 3;
        }

        private void SubscribeEvents()
        {
            btnCancel.Click += BtnCancel_Click;
            btnProcede.Click += BtnProcede_Click;
            this.KeyDown += FinalizeForm_KeyDown;
        }

        private void FinalizeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Cancel)
            {
                this.Close();
            }
        }

        private void BtnProcede_Click(object sender, EventArgs e)
        {
            var mesh = _volRenderer.FinalizeMesh(chkSmoothen.Checked, chkIsland.Checked, chkFill.Checked, (int)numericSmoothen.Value, (int)numericFill.Value);
            mesh.Name = _volRenderer.Name + "- Finalized";
            BtnProcedeClicked?.Invoke(mesh, _volRenderer.Color);
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
