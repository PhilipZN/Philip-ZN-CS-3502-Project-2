using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zen.Barcode;
using System.Drawing.Imaging;
using System.IO;

namespace CpuSchedulingWinForms
{
    public partial class CpuScheduler : Form
    {
        // for manual Gantt drawing
        private List<GanttEntry> currentGanttEntries = new List<GanttEntry>();
        private double maxGanttTime = 0;

        public CpuScheduler()
        {
            InitializeComponent();
            // attach paint handler
            this.panelGantt.Paint += panelGantt_Paint;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dashBoardTab.Show();
            this.tabSelection.SelectTab(0);
            this.sidePanel.Height = btnDashBoard.Height;
            this.sidePanel.Top = btnDashBoard.Top;

            //this.btnProductCode.BackColor = Color.Transparent;
            //this.btnCpuScheduler.BackColor = Color.Transparent;
            //this.btnDashBoard.BackColor = Color.DimGray;
        }

        private void btnCpuScheduler_Click(object sender, EventArgs e)
        {
            //this.dashBoardTab.Show();
            this.tabSelection.SelectTab(1);
            this.sidePanel.Height = btnCpuScheduler.Height;
            this.sidePanel.Top = btnCpuScheduler.Top;
            // clear previous metrics
            dgvMetrics.Rows.Clear();

            //this.btnProductCode.BackColor = Color.Transparent;         
            //this.btnDashBoard.BackColor = Color.Transparent;
            //this.btnCpuScheduler.BackColor = Color.DimGray;
        }


        private void btnFCFS_Click(object sender, EventArgs e)
        {
            if (txtProcess.Text != "")
            {
                Algorithms.fcfsAlgorithm(txtProcess.Text);
                int numberOfProcess = Int16.Parse(txtProcess.Text);
                if (numberOfProcess <= 10)
                {
                    this.progressBar1.Increment(4); //cpu progress bar
                    this.progressBar1.SetState(1);
                    this.progressBar2.Increment(13);
                    this.progressBar2.SetState(1);
                }
                else if (numberOfProcess > 10)
                {
                    this.progressBar1.Increment(15);
                    this.progressBar1.SetState(1);
                    this.progressBar2.Increment(38); //memory progress bar
                    this.progressBar2.SetState(3);
                }

                listView1.Clear();
                listView1.View = View.Details;
                
                listView1.Columns.Add("Process ID", 150, HorizontalAlignment.Center);
                listView1.Columns.Add("Quantum Time", 100, HorizontalAlignment.Center);

                for (int i = 0; i < numberOfProcess; i++)
                {
                    //listBoxProcess.Items.Add(" Process " + (i + 1));
                    var item = new ListViewItem();
                    item.Text = "Process " + (i + 1);
                    item.SubItems.Add("-");
                    listView1.Items.Add(item);
                }
                //listBoxProcess.Items.Add("\n");
                //listBoxProcess.Items.Add(" Total number of processes executed: " + numberOfProcess);
                listView1.Items.Add("\n");
                listView1.Items.Add("CPU handles: " + numberOfProcess);
            }
            else
            {
                MessageBox.Show("Enter number of processes", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProcess.Focus();
            }
        }

        private void btnSJF_Click(object sender, EventArgs e)
        {
            if (txtProcess.Text != "")
            {
                Algorithms.sjfAlgorithm(txtProcess.Text);
                int numberOfProcess = Int16.Parse(txtProcess.Text);
                if (numberOfProcess <= 10)
                {
                    this.progressBar1.Increment(4); //cpu progress bar
                    this.progressBar1.SetState(1);
                    this.progressBar2.Increment(13);
                    this.progressBar2.SetState(1);
                }
                else if (numberOfProcess > 10)
                {
                    this.progressBar1.Increment(15);
                    this.progressBar1.SetState(1);
                    this.progressBar2.Increment(38); //memory progress bar
                    this.progressBar2.SetState(3);
                }

                listView1.Clear();
                listView1.View = View.Details;
               
                listView1.Columns.Add("Process ID", 150, HorizontalAlignment.Center);
                listView1.Columns.Add("Quantum Time", 100, HorizontalAlignment.Center);

                for (int i = 0; i < numberOfProcess; i++)
                {
                    var item = new ListViewItem();
                    item.Text = "Process " + (i + 1);
                    item.SubItems.Add("-");
                    listView1.Items.Add(item);
                }
                
                listView1.Items.Add("\n");
                listView1.Items.Add("CPU handles: " + numberOfProcess);
            }
            else
            {
                MessageBox.Show("Enter number of processes", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProcess.Focus();
            }
        }

        private void btnPriority_Click(object sender, EventArgs e)
        {
            if (txtProcess.Text != "")
            {
                Algorithms.priorityAlgorithm(txtProcess.Text);
                int numberOfProcess = Int16.Parse(txtProcess.Text);
                if (numberOfProcess <= 10)
                {
                    this.progressBar1.Increment(4); //cpu progress bar
                    this.progressBar1.SetState(1);  //cpu color progress bar
                    this.progressBar2.Increment(13);
                    this.progressBar2.SetState(1);
                }
                else if (numberOfProcess > 10)
                {
                    this.progressBar1.Increment(15);
                    this.progressBar1.SetState(1);
                    this.progressBar2.Increment(38); //memory progress bar
                    this.progressBar2.SetState(3);   //memory color progress bar
                }
                listView1.Clear();
                listView1.View = View.Details;

                listView1.Columns.Add("Process ID", 150, HorizontalAlignment.Center);
                listView1.Columns.Add("Quantum Time", 100, HorizontalAlignment.Center);

                for (int i = 0; i < numberOfProcess; i++)
                {
                    var item = new ListViewItem();
                    item.Text = "Process " + (i + 1);
                    item.SubItems.Add("-");
                    listView1.Items.Add(item);
                }

                listView1.Items.Add("\n");
                listView1.Items.Add("CPU handles : " + numberOfProcess);
            }
            else
            {
                MessageBox.Show("Enter number of processes", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProcess.Focus();
            }
        }

        private void txtProcess_TextChanged(object sender, EventArgs e)
        {

        }

        private void restartApp_Click(object sender, EventArgs e)
        {
            this.Hide();
            CpuScheduler cpuScheduler = new CpuScheduler();
            cpuScheduler.ShowDialog();
        }

        private void btnBarcode_Click(object sender, EventArgs e)
        {
            if (txtCodeInput.Text != "")
            {
                string barcode = txtCodeInput.Text;
                //Code128BarcodeDraw barcode = BarcodeDrawFactory.Code128WithChecksum;
                //pictureBoxCodeOutput.Image = barcode.Draw(barcodeInput, 30);
                //pictureBoxCodeOutput.Height = barcode.Draw(txtCodeInput.Text, 150).Height;
                //pictureBoxCodeOutput.Width = barcode.Draw(txtCodeInput.Text, 150).Width;

                Bitmap bitmap = new Bitmap(barcode.Length * 36, 109);   //40, 150
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    Font font = new Font("IDAutomationHC39M Free Version", 25);
                    PointF point = new PointF(2f, 2f);
                    SolidBrush black = new SolidBrush(Color.Black);
                    SolidBrush white = new SolidBrush(Color.White);
                    graphics.FillRectangle(white, 0, 0, bitmap.Width, bitmap.Height);
                    graphics.DrawString(barcode, font, black, point);
                }
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, ImageFormat.Png);
                    pictureBoxCodeOutput.Image = bitmap;
                    //pictureBoxCodeOutput.Height = bitmap.Height;
                    //pictureBoxCodeOutput.Width = bitmap.Width;
                }
            }
            else
            {
                MessageBox.Show("No Input", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCodeInput.Focus();
            }
            
        }

        private void btnQrcode_Click(object sender, EventArgs e)
        {
            if (txtCodeInput.Text != "")
            {
                CodeQrBarcodeDraw codeQr = BarcodeDrawFactory.CodeQr;
                pictureBoxCodeOutput.Image = codeQr.Draw(txtCodeInput.Text, 100);
                //string barcode = txtCodeInput.Text;
                //Bitmap bitmap = new Bitmap(barcode.Length * 40, 150);
                //pictureBoxCodeOutput.Image = bitmap; 
            }
            else
            {
                MessageBox.Show("No Input", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCodeInput.Focus();
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pictureBoxCodeOutput.Image == null)
            {
                return;
            }
            else if (pictureBoxCodeOutput.Image != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "PNG|*.png|JPEG|*.jpeg|ICON|*.ico"})
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        pictureBoxCodeOutput.Image.Save(saveFileDialog.FileName);
                    }
                }
            }           
        }

        private void btnProductCode_Click(object sender, EventArgs e)
        {
            //this.dashBoardTab.Show();
            this.tabSelection.SelectTab(2);
            this.sidePanel.Height = btnProductCode.Height;
            this.sidePanel.Top = btnProductCode.Top;
            
            //this.btnCpuScheduler.BackColor = Color.Transparent;
            //this.btnDashBoard.BackColor = Color.Transparent;
            //this.btnProductCode.BackColor = Color.DimGray;
        }

        private void CpuScheduler_Load(object sender, EventArgs e)
        {
            this.sidePanel.Height = btnDashBoard.Height;
            this.sidePanel.Top = btnDashBoard.Top;
            this.progressBar1.Increment(5);
            this.progressBar2.Increment(17);
            listView1.View = View.Details;
            listView1.GridLines = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Application.Exit();
            timer1.Start();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void btnRoundRobin_Click(object sender, EventArgs e)
        {
            if (txtProcess.Text != "")
            {
                Algorithms.roundRobinAlgorithm(txtProcess.Text);
                int numberOfProcess = Int16.Parse(txtProcess.Text);
                if (numberOfProcess <= 10)
                {
                    this.progressBar1.Increment(4); //cpu progress bar
                    this.progressBar1.SetState(1);  //cpu color progress bar
                    this.progressBar2.Increment(13);
                    this.progressBar2.SetState(1);
                }
                else if (numberOfProcess > 10)
                {
                    this.progressBar1.Increment(15);
                    this.progressBar1.SetState(1);
                    this.progressBar2.Increment(38); //memory progress bar
                    this.progressBar2.SetState(3);   //memory color progress bar
                }
                string quantumTime = Helper.QuantumTime;
                listView1.Clear();
                listView1.View = View.Details;

                listView1.Columns.Add("Process ID", 150, HorizontalAlignment.Center);
                listView1.Columns.Add("Quantum Time", 100, HorizontalAlignment.Center);

                for (int i = 0; i < numberOfProcess; i++)
                {
                    var item = new ListViewItem();
                    item.Text = "Process " + (i + 1);
                    item.SubItems.Add(quantumTime);
                    listView1.Items.Add(item);
                }

                listView1.Items.Add("\n");
                listView1.Items.Add("CPU handles: " + numberOfProcess);
            }
            else
            {
                MessageBox.Show("Enter number of processes", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProcess.Focus();
            }
        }
        private void btnSRTF_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtProcess.Text))
                Algorithms.srtfAlgorithm(txtProcess.Text);
        }

        private void btnHRRN_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtProcess.Text))
                Algorithms.hrrnAlgorithm(txtProcess.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(this.Opacity > 0.0)
            {
                this.Opacity -= 0.021;
            } else
            {
                timer1.Stop();
                Application.Exit();
            }
        }

        private void txtCodeInput_Click(object sender, EventArgs e)
        {
            this.txtCodeInput.Clear();
        }

        // Adds a row to the metrics table
        public void AddMetricRow(
            string algorithmName,
            double avgWaiting,
            double avgTurnaround,
            double cpuUtilization,
            double throughput,
            double? avgResponseTime)
        {
            int idx = dgvMetrics.Rows.Add();
            var row = dgvMetrics.Rows[idx];
            row.Cells["AlgorithmName"].Value   = algorithmName;
            row.Cells["AvgWaiting"].Value      = avgWaiting.ToString("F2");
            row.Cells["AvgTurnaround"].Value   = avgTurnaround.ToString("F2");
            row.Cells["CPUUtilization"].Value  = cpuUtilization.ToString("F2") + "%";
            row.Cells["Throughput"].Value      = throughput.ToString("F2");
            row.Cells["AvgResponseTime"].Value = avgResponseTime.HasValue
                ? avgResponseTime.Value.ToString("F2")
                : "-";
            // switch to metrics tab to display results
            this.tabSelection.SelectTab(this.metricsTab);
        }

        // Renders the Gantt chart in the metrics tab
        public void DrawGanttChart(string algorithmName, List<GanttEntry> entries)
        {
            // store entries and compute max time
            currentGanttEntries = entries;
            maxGanttTime = entries.Max(en => en.StartTime + en.Duration);
            // trigger redraw
            panelGantt.Invalidate();
            panelGantt.Refresh();
        }

        // Paint handler for panelGantt: draws Gantt bars
        private void panelGantt_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(panelGantt.BackColor);
            if (currentGanttEntries == null || currentGanttEntries.Count == 0) return;
            int procCount = currentGanttEntries.Max(en => en.ProcessId);
            int rowHeight = panelGantt.Height / procCount;
            float scale = (float)(panelGantt.Width / maxGanttTime);
            var brush = Brushes.SteelBlue;
            var pen = Pens.Black;
            // draw each entry
            foreach (var entry in currentGanttEntries)
            {
                int y = (entry.ProcessId - 1) * rowHeight;
                float x = (float)(entry.StartTime * scale);
                float w = (float)(entry.Duration * scale);
                var rect = new RectangleF(x, y + 2, w, rowHeight - 4);
                g.FillRectangle(brush, rect);
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                // process label
                g.DrawString("P" + entry.ProcessId, Font, Brushes.White, rect.X + 2, rect.Y + 2);
            }
            // draw time scale
            for (int t = 0; t <= maxGanttTime; t++)
            {
                float xt = t * scale;
                g.DrawLine(Pens.Gray, xt, 0, xt, panelGantt.Height);
                g.DrawString(t.ToString(), Font, Brushes.Black, xt, panelGantt.Height - 15);
            }
        }
    }
}