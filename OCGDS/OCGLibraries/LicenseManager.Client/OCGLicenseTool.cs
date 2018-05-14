using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

using LicenseManager.Utils;

namespace LicenseManager.Client
{
    public partial class OCGLicenseTool : Form
    {

        public OCGLicenseTool()
        {
            try
            {
                InitializeComponent();

                string appNames = ConfigurationManager.AppSettings["ApplicationName"];
                string licCount = ConfigurationManager.AppSettings["LicenseCount"];
                string[] appNamesList = appNames.Split('|');
                string[] licCountList = licCount.Split('|');

                cmbApplicationName.Items.Clear();
                foreach(string entry in appNamesList)
                {
                    cmbApplicationName.Items.Add(entry);
                }

                cbLicenseCount.Items.Clear();
                foreach (string entry in licCountList)
                {
                    cbLicenseCount.Items.Add(entry);
                }
            }
            catch (Exception)
            {
                throw new Exception("Unable to read configuration parameters from app.config");
            }

        }

        private void cbLicenseCount_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
        }

        private void cbLicenseCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLicenseCount.Items.Count > 0)
            {
                int selIndex = cbLicenseCount.SelectedIndex;
                for (int i = 0; i < cbLicenseCount.Items.Count; i++)
                {
                    if (i != selIndex)
                    {
                        cbLicenseCount.SetItemCheckState(i, CheckState.Unchecked);
                    }
                    else
                    {
                        cbLicenseCount.SetItemCheckState(i, CheckState.Checked);
                    }
                }
            }
        }

        private void btnAddComputer_Click(object sender, EventArgs e)
        {
            ListViewItem lvItem = new ListViewItem("New Item");
            lvComputername.Items.Add(lvItem);
            lvItem.BeginEdit();
        }

        private void btnDeleteComputer_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvItem in lvComputername.SelectedItems)
            {
                lvItem.Remove();
            }
        }

        private void btnCreateLicense_Click(object sender, EventArgs e)
        {
            if (txtClientName.Text.Trim().Equals(string.Empty))
            {
                MessageBox.Show("Client Name must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ((cmbApplicationName.SelectedItem==null)||(cmbApplicationName.SelectedItem.ToString().Trim().Equals(string.Empty)))
            {
                MessageBox.Show("Application Name must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (lvComputername.Items.Count == 0)
            {
                MessageBox.Show("Licensed Computers must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dtExpireDate.Value < DateTime.Now)
            {
                MessageBox.Show("License expire date is in the past", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] servers = new string[lvComputername.Items.Count];
            for (int i=0;i<lvComputername.Items.Count;i++)
            {
                servers[i] = lvComputername.Items[i].Text;
            }

            string key = string.Empty;
            try
            {
                key = LicenseGenerator.GenerateLicenseKeyV1(txtClientName.Text, cmbApplicationName.SelectedItem.ToString(), servers, Int64.Parse(cbLicenseCount.SelectedItem.ToString()), dtExpireDate.Value, chkServer.Checked, new string[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while creating license key\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                LicenseHandler lh = new LicenseHandler(key);
            }
            catch
            {
                MessageBox.Show("Key generation failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string filename = string.Empty;

            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Filter = "OCG License File|*.lic";
            try
            {
                sDialog.FileName = string.Format("{0}.{1}", txtClientName.Text, cmbApplicationName.SelectedItem.ToString());
            }
            catch
            { }

            if (sDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter licWriter = new StreamWriter(sDialog.FileName, false);
                    licWriter.WriteLine(key);
                    licWriter.Flush();
                    licWriter.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while creating keyfile\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
    }
}