using Engine.Core;
using Engine.GLApi;
using Engine.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VertexBenderCS.Forms
{

    public enum eProcessCoreType
    {
        ShortestPath,
        Descriptor
    }

    public partial class FrmProcess : Form
    {
        private eShortestPathMethod _method;
        private Mesh _mesh;

        private BackgroundWorker _worker;

        public Action<IOutput> OnResultReturned;

        public FrmProcess(Mesh mesh, eProcessCoreType processType)
        {
            InitializeComponent();
            SubscribeEvents();
            InitWorker();
            _mesh = mesh;
            PrepareUI(processType);
        }

        private void InitWorker()
        {
            _worker = new BackgroundWorker();
            _worker.ProgressChanged += _worker_ProgressChanged;
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
        }

        private void PrepareUI(eProcessCoreType type)
        {
            numericSource.Maximum = _mesh.Vertices.Count - 1;
            numericTarget.Maximum = _mesh.Vertices.Count - 1;
            labelMaxVertex.Text = (_mesh.Vertices.Count - 1).ToString();
            cmbMethod.DataSource = Enum.GetValues(typeof(eShortestPathMethod));

            if (type == eProcessCoreType.ShortestPath)
            {
                tabProcess.TabPages.Remove(tabDescriptor);
            }
            else if (type == eProcessCoreType.Descriptor)
            {
                tabProcess.TabPages.Remove(tabSPH);
                tabProcess.TabPages.Remove(tabGm);
            }

            numericSource.Controls[0].Visible = false;
            numericTarget.Controls[0].Visible = false;
            numericStartIndex.Controls[0].Visible = false;
            numericSampleCount.Controls[0].Visible  = false;

        }

        private void SubscribeEvents()
        {
            btnStart.Click += BtnStart_Click;
            cmbMethod.SelectedIndexChanged += CmbMethod_SelectedIndexChanged;
        }

        private void UpdateControls(bool isActive)
        {
            btnStart.Enabled = isActive;
            foreach (var tab in tabProcess.TabPages)
            {
                (tab as TabPage).Enabled = isActive;
            }
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                OnResultReturned?.Invoke
                (
                    new CancelOutput()
                );
            }
            else
            {
                if (e.Result is IOutput)
                {
                    OnResultReturned?.Invoke((IOutput)e.Result);
                }
            }
            UpdateControls(true);
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var tab = (TabPage)e.Argument;
            if (tab == tabSPH)
            {
                e.Result = Algorithm.ShortestPath(_mesh, (int)(numericSource.Value), (int)numericTarget.Value, _method, true);
            }
            else if (tab == tabGm)
            {
                var output = Algorithm.CreateGeodesicDistanceMatrix(_mesh, UpdateProgress, true);
                var bmp = ProcessOutputHandler.CreateBitmapGeodesicDistance(output.Matrix, "output.png");
                ProcessOutputHandler.SaveGeodesicDistanceToFile(output.Matrix, "output.txt");
                e.Result = output;
                imagePanel.BackgroundImage = bmp;
            }
            else if (tab == tabDescriptor)
            {
                if (radioAGD.Checked)
                {
                    e.Result = Algorithm.AverageGeodesicDistance
                    (
                        new Graph(_mesh),
                        (int)numericSampleCount.Value,
                        (int)numericStartIndex.Value,
                        UpdateProgress
                    );
                }
                else if (radioFPS.Checked)
                {
                    e.Result = Algorithm.FarthestPointSampling
                    (
                        new Graph(_mesh),
                        (int)numericSampleCount.Value,
                        (int)numericStartIndex.Value,
                        UpdateProgress
                    );
                }
                else if (radioISO.Checked)
                {
                    e.Result = Algorithm.IsoCurveSignature
                    (
                        _mesh,
                        (int)numericStartIndex.Value,
                        (int)numericSampleCount.Value,
                        UpdateProgress
                    );
                }
            }
        }

        private void UpdateProgress(int percentage)
        {
            _worker.ReportProgress(percentage);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (_mesh != null)
            {
                if (_worker.IsBusy)
                {
                    _worker.CancelAsync();
                }
                else
                {
                    _worker.RunWorkerAsync(tabProcess.SelectedTab);
                }
                UpdateControls(!_worker.IsBusy);

            }
        }

        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = Math.Min(e.ProgressPercentage + 10, 100);
        }

        private void CmbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            _method = (eShortestPathMethod)cmbMethod.SelectedItem;
        }
        
    }
}
