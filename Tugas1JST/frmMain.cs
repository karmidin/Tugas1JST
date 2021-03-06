﻿using JST;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tugas1JST
{
    public partial class frmMain : Form
    {
        Perceptron pct;
        Label[] lblGambarPattern;
        bool print = true;
        System.Diagnostics.Process proc;

        public frmMain()
        {
            pct = new Perceptron();
            InitializeComponent();
            modeInputPattern();
            addTargetLabel();

            proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = false;

            saveFileDialog1.Filter = "csv files (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 1;
           
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (openFileDialog1.SafeFileName == "pattern1.txt" || openFileDialog1.SafeFileName == "pattern4.txt")
                    {
                        pct.ListPattern.Add(new Pattern(1, openFileDialog1.FileName));
                    }
                    else
                    {
                        pct.ListPattern.Add(new Pattern(-1, openFileDialog1.FileName));
                    }

                    inputPattern(i);
                }
            }
            modeTestTrain();
        }

        private void btnTraining_Click(object sender, EventArgs e)
        {
            rtbTraining.Text = rtbTraining.Text + pct.Training();
            btnTraining.Enabled = false;
            btnTesting.Enabled = true;
        }

        private void btnTesting_Click(object sender, EventArgs e)
        {
            rtbTesting.Text = rtbTesting.Text + pct.Testing();
            btnTesting.Enabled = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            pct.Reset();
            rtbTesting.Clear();
            rtbTraining.Clear();
            for (int i = 0; i < 6; i++) {
                lblGambarPattern[i].Text = "";
            }
            modeInputPattern();
            print = true;
        }

       

        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (rtbTesting.Text != "" || rtbTraining.Text != "")
            {
               
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    PrintToExcel(saveFileDialog1.FileName);
                    proc.StartInfo.FileName = saveFileDialog1.FileName;
                    proc.Start();
                }
            }
            else
            {
                MessageBox.Show("There are no results to be printed", "Alert",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void addTargetLabel()
        {
            int x = new int();
            x = 60;
            lblGambarPattern = new Label[6];

                for (int i = 0; i < 6; i++)
                {
                    lblGambarPattern[i] = new Label();
                    lblGambarPattern[i].Location = new Point(x, 20);
                    lblGambarPattern[i].AutoSize = true;
                    groupBox1.Controls.Add(lblGambarPattern[i]);
                    x += 90;
                }
        }

        private void inputPattern(int i) {
            string input;

            for (int j = 0; j < 63; j++)
            {
                if (pct.ListPattern[i].x[j].ToString() == "1")
                {
                    input = "#";
                }
                else {
                    input = "_";
                }
                if ((j + 1) % 7 == 0)
                {
                    lblGambarPattern[i].Text = lblGambarPattern[i].Text + input + "\n";
                }
                else
                {
                    lblGambarPattern[i].Text = lblGambarPattern[i].Text + input;
                }
            }
        }

        private void PrintToExcel(string name)
        {
            FileStream fs1 = new FileStream(name, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fs1);
            writer.Write(pct.getPrintToExcel(print));
            writer.Close();
            print=false;
        }

        private void modeInputPattern()
        {
            btnTesting.Enabled = false;
            btnTraining.Enabled = false;
            btnPrint.Enabled = false;
            btnReset.Enabled = false;
            btnBrowse.Enabled = true;
        }

        private void modeTestTrain()
        {
            btnTesting.Enabled = false;
            btnTraining.Enabled = true;
            btnPrint.Enabled = true;
            btnReset.Enabled = true;
            btnBrowse.Enabled = false;
        }

    }
}
