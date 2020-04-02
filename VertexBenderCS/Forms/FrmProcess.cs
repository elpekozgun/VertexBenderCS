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
        Descriptor,
        Parametrization
    }

    public partial class FrmProcess : Form
    {
        private eShortestPathMethod _shortestPathmethod;
        private eParameterizationMethod _parametrizationMethod;
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
            cmbShortestPathMethod.DataSource = Enum.GetValues(typeof(eShortestPathMethod));
            cmbDisk.DataSource = Enum.GetValues(typeof(eParameterizationMethod));

            if (type == eProcessCoreType.ShortestPath)
            {
                tabProcess.TabPages.Remove(tabDescriptor);
                tabProcess.TabPages.Remove(tabParametrization);
            }
            else if (type == eProcessCoreType.Descriptor)
            {
                tabProcess.TabPages.Remove(tabSPH);
                tabProcess.TabPages.Remove(tabGm);
                tabProcess.TabPages.Remove(tabParametrization);
            }
            else if (type == eProcessCoreType.Parametrization)
            {
                tabProcess.TabPages.Remove(tabSPH);
                tabProcess.TabPages.Remove(tabGm);
                tabProcess.TabPages.Remove(tabDescriptor);
            }

            numericSource.Controls[0].Visible = false;
            numericTarget.Controls[0].Visible = false;
            numericStartIndex.Controls[0].Visible = false;
            numericSampleCount.Controls[0].Visible  = false;
            numericWeight.Controls[0].Visible = false;

        }

        private void SubscribeEvents()
        {
            btnStart.Click += BtnStart_Click;
            cmbShortestPathMethod.SelectedIndexChanged += CmbShortestPathMethod_SelectedIndexChanged;
            cmbDisk.SelectedIndexChanged += CmbDisk_SelectedIndexChanged;

            radioDisk.CheckedChanged += RadioDisk_CheckedChanged;
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
                e.Result = Algorithm.ShortestPath(_mesh, (int)(numericSource.Value), (int)numericTarget.Value, _shortestPathmethod, true);
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
            else if (tab == tabParametrization)
            {
                if (radioDisk.Checked)
                {
                    e.Result = Algorithm.ParameterizeMeshToDisc
                    (
                        _mesh,
                        _parametrizationMethod,
                        UpdateProgress,
                        (float)numericWeight.Value,
                        checkFixate.Checked,
                        checkUniformBoundary.Checked
                    );
                }
                else if (radioSphere.Checked)
                {
                    e.Result = Algorithm.ParameterizeMeshToSphere
                    (
                        _mesh,
                        (int)numericIteration.Value,
                        UpdateProgress,
                        checkUseCenter.Checked
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

        private void CmbShortestPathMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            _shortestPathmethod = (eShortestPathMethod)cmbShortestPathMethod.SelectedItem;
        }

        private void CmbDisk_SelectedIndexChanged(object sender, EventArgs e)
        {
            _parametrizationMethod = (eParameterizationMethod)cmbDisk.SelectedItem;
            if (_parametrizationMethod != eParameterizationMethod.Uniform || !radioDisk.Checked)
            {
                labelDiskWeight.Enabled = false;
                numericWeight.Enabled = false;
                return;
            }
            labelDiskWeight.Enabled = true;
            numericWeight.Enabled = true;
        }

        private void RadioDisk_CheckedChanged(object sender, EventArgs e)
        {
            cmbDisk.Enabled = radioDisk.Checked;
            labelDiskWeight.Enabled = radioDisk.Checked;
            numericWeight.Enabled = radioDisk.Checked;
            labelDiskMethod.Enabled = radioDisk.Checked;
            CmbDisk_SelectedIndexChanged(sender, e);
        }

    }
}
