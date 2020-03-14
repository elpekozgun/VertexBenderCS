using Engine.Core;
using Engine.Processing;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VertexBenderCS.Forms
{
    public partial class frmShortestPath : Form
    {
        private int _sourceIndex;
        private int _targetIndex;
        private eShortestPathMethod _method;
        private Mesh _mesh;
        public Mesh Mesh 
        { 
            get 
            { 
                return _mesh; 
            } 
            set 
            {
                _mesh = value;
                numericSource.Maximum = _mesh.Vertices.Count - 1;
                numericTarget.Maximum = _mesh.Vertices.Count - 1;
                labelMaxVertex.Text = (_mesh.Vertices.Count - 1).ToString();
            }
        }

        public Action<List<ShortestPathOutput>> OnInputSelected;

        public frmShortestPath()
        {
            InitializeComponent();
            PrepareUI();
            SubscribeEvents();
        }

        private void PrepareUI()
        {
            numericSource.Maximum = 0;
            numericTarget.Maximum = 0;
            labelMaxVertex.Text = 0.ToString();
            cmbMethod.DataSource = Enum.GetValues(typeof(eShortestPathMethod));
        }

        private void CmbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            _method = (eShortestPathMethod)cmbMethod.SelectedItem;
        }

        private void SubscribeEvents()
        {
            numericSource.ValueChanged += NumericSource_ValueChanged;
            numericTarget.ValueChanged += NumericTarget_ValueChanged;
            btnStart.Click += BtnStart_Click;
        }

        void necati(string asdasd) { }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (Mesh != null)
            {
                var output = Algorithm.ShortestPath(Mesh, _sourceIndex, _targetIndex, _method, necati);
                OnInputSelected?.Invoke(new List<ShortestPathOutput>() { output });
            }
        }

        private void NumericTarget_ValueChanged(object sender, EventArgs e)
        {
            _targetIndex = (int)numericTarget.Value;
        }

        private void NumericSource_ValueChanged(object sender, EventArgs e)
        {
            _sourceIndex = (int)numericSource.Value;
        }
    }
}
