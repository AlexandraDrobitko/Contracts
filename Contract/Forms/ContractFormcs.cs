﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Contract.Forms
{
    public delegate void getIDFromForm(int ID, Tables type);
    public partial class ContractForm : DevExpress.XtraEditors.XtraForm
    {
        private DB.DBModel dbContext;
        private int returnID;
        private int contractID;
        private bool isLoaded = false;
        
        private DB.Contract currContract;

        private UpdateContracts uc;
        
        public ContractForm(int contractID, UpdateContracts updateContracts)
        {
            InitializeComponent();
            dbContext = new DB.DBModel();
            
            this.contractID = contractID;
            this.uc = updateContracts;
            fillControls();

            currContract = new DB.Contract();
            if (contractID != 0)
                fillExistingContract(contractID);
        }
        private void fillExistingContract(int id)
        {
            currContract = dbContext.Contract.Where(x => x.ID == id).ToList()[0];

            Text += currContract.Number + " от " + currContract.Date.ToString();

            teContractNumber.Text = currContract.Number;
            teContractNote.Text = currContract.Note;
            teSumm.Text = currContract.Summ.ToString();
            teContractTheme.Text = currContract.Theme;
            teAuthor.Text = currContract.Users.Surname;

            if (currContract.Date != null)
                deDate.DateTime = (DateTime)currContract.Date;
            if (currContract.EndDate != null)
                deContractDateEnd.DateTime = (DateTime)currContract.EndDate;
            if (currContract.StartDate != null)
                deContractDateStart.DateTime = (DateTime)currContract.StartDate;

            lueDepartment.EditValue = currContract.DepartmentID;
            lueContractual.EditValue = currContract.ContractualID;
            lueExtensions.EditValue = currContract.ContractExtensionID;
            lueContractCategory.EditValue = currContract.CategoryID;
            lueContractors.EditValue = currContract.ContractorID;

            if(currContract.OnAgreement == true)
                FillAgreemenst();

            isLoaded = true;
        }

        private void fillControls()
        {
            lueDepartment.Properties.DisplayMember = "Text";
            lueDepartment.Properties.ValueMember = "Value";
            lueDepartment.Properties.DataSource = dbContext.Departments.Select(x => new { Value = x.ID, Text = x.Name }).ToList();

            lueContractual.Properties.DisplayMember = "Text";
            lueContractual.Properties.ValueMember = "Value";
            lueContractual.Properties.DataSource = dbContext.Users.Select(x => new { Value = x.ID, Text = x.Surname + " " + x.FirstName.Substring(0, 1) + "." + x.SecondName.Substring(0, 1) + "." }).ToList();

            lueExtensions.Properties.DisplayMember = "Text";
            lueExtensions.Properties.ValueMember = "Value";
            lueExtensions.Properties.DataSource = dbContext.ContractExtension.Select(x => new { Value = x.ID, Text = x.Name }).ToList();

            lueContractCategory.Properties.DisplayMember = "Text";
            lueContractCategory.Properties.ValueMember = "Value";
            lueContractCategory.Properties.DataSource = dbContext.ContractCategory.Select(x => new { Value = x.ID, Text = x.Name }).ToList();

            lueContractors.Properties.DisplayMember = "Text";
            lueContractors.Properties.ValueMember = "Value";
            lueContractors.Properties.DataSource = dbContext.Contractors.Select(x => new { Value = x.ID, Text = x.Name }).ToList();
        }

        #region  LookUpEdits
        private void getIDSelectedItemID(int ID, Tables type)
        {
            switch (type)
            {
                case Tables.Category: lueContractCategory.EditValue = ID; break;
                case Tables.Users: lueContractual.EditValue = ID; break;
                case Tables.Departments: lueDepartment.EditValue = ID; break;
                case Tables.Contractors: lueContractors.EditValue = ID; break;
            }
        }
        private void lueContractCategory_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                Forms.SelectInfoForm tmpForm = new SelectInfoForm(Tables.Category, dbContext, getIDSelectedItemID);

                tmpForm.ShowDialog();
            }
        }
        private void lueDepartment_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                Forms.SelectInfoForm tmpForm = new SelectInfoForm(Tables.Departments, dbContext, getIDSelectedItemID);

                tmpForm.ShowDialog();
            }

        }
        private void lueContractual_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                Forms.SelectInfoForm tmpForm = new SelectInfoForm(Tables.Users, dbContext, getIDSelectedItemID);

                tmpForm.ShowDialog();
            }
        }
        private void lueContractors_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                Forms.SelectInfoForm tmpForm = new SelectInfoForm(Tables.Contractors, dbContext, getIDSelectedItemID);

                tmpForm.ShowDialog();
            }
        }
        private void lueActivityKind_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                Forms.SelectInfoForm tmpForm = new SelectInfoForm(Tables.ActivityKinds, dbContext, getIDSelectedItemID);

                tmpForm.ShowDialog();
            }
        }
        #endregion

        #region SaveChanges
        private void SaveContracChanges()
        {
            if (lueContractCategory.Text != "" && lueContractCategory.EditValue!= null && currContract.CategoryID != (int)lueContractCategory.EditValue)
                currContract.CategoryID = (int)lueContractCategory.EditValue;
            if (lueDepartment.Text != "" && lueDepartment.EditValue != null && currContract.DepartmentID != (int)lueDepartment.EditValue)
                currContract.DepartmentID = (int)lueDepartment.EditValue;
            if (lueContractual.Text != "" && lueContractual.EditValue != null && currContract.ContractualID != (int)lueContractual.EditValue)
                currContract.ContractualID = (int)lueContractual.EditValue;
            if (lueExtensions.Text != "" && lueExtensions.EditValue != null && currContract.ContractExtensionID != (int)lueExtensions.EditValue)
                currContract.ContractExtensionID = (int)lueExtensions.EditValue;
            if (lueContractors.Text != "" && lueContractors.EditValue != null && currContract.ContractorID != (int)lueContractors.EditValue)
                currContract.ContractorID = (int)lueContractors.EditValue;
            if (teSumm.Text != "" && currContract.Summ != Convert.ToDouble(teSumm.Text))
                currContract.Summ = Convert.ToDouble(teSumm.Text);
            if (teContractNote.Text != "" && currContract.Note != teContractNote.Text)
                currContract.Note = teContractNote.Text;
            if (teContractTheme.Text != "" && currContract.Theme != teContractTheme.Text)
                currContract.Theme = teContractTheme.Text;

            if (deDate.Text == "")
                currContract.Date = null;
            else
                if (currContract.Date != Convert.ToDateTime(deDate.Text))
                currContract.Date = Convert.ToDateTime(deDate.Text);

            if (deContractDateEnd.Text == "")
                currContract.EndDate = null;
            else
                if (currContract.EndDate != Convert.ToDateTime(deContractDateEnd.Text))
                currContract.EndDate = Convert.ToDateTime(deContractDateEnd.Text);

            if (deContractDateStart.Text == "")
                currContract.StartDate = null;
            else
                if (currContract.StartDate != Convert.ToDateTime(deContractDateStart.Text))
                currContract.StartDate = Convert.ToDateTime(deContractDateStart.Text);
            currContract.AuthorID = Properties.Settings.CurrentUserID;

            if(this.contractID == 0)
                dbContext.Contract.Add(currContract);
                
            dbContext.SaveChanges();
            contractID = currContract.ID;
            uc();
        }

        private void sbSaveChanges_Click(object sender, EventArgs e)
        {
            SaveContracChanges();
            sbSaveChanges.Enabled = false;
            simpleButton1.Enabled = true;
        }

        #endregion

        #region Отправить на согласование

        private void SendContractToSigning()
        {
            foreach (var user in dbContext.AgreementSignList.Select(x => x.ID).ToList())
            {
                DB.Signing tmpSigning = new DB.Signing();
                tmpSigning.UserID = user;
                tmpSigning.ContractID = currContract.ID;
                tmpSigning.DeadlineTime = DateTime.Now.AddDays(5);
                tmpSigning.IsAgreed = false;
                dbContext.Signing.Add(tmpSigning);     
            }
            dbContext.SaveChanges();
        }

        private void FillAgreemenst()
        {
            gcAgreements.DataSource = dbContext.Signing.Where(x => x.ContractID == currContract.ID).ToList();
            gvAgreements.Columns["ID"].Visible = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SaveContracChanges();

            currContract.OnAgreement = true;
            SendContractToSigning();

            FillAgreemenst();
        }
        #endregion

        private void control_EditValueChanged(object sender, EventArgs e)
        {
            if (isLoaded)
                if (!DataChanged())
                    sbSaveChanges.Enabled = true;             
                else
                    sbSaveChanges.Enabled = false;
        }

        //Придумать оптимизацию на проверку
        // когда-нибудь
        private bool DataChanged()
        {
            if (currContract.CategoryID != (int)lueContractCategory.EditValue) return false;
            if (currContract.DepartmentID != (int)lueDepartment.EditValue) return false;
            if (currContract.ContractualID != (int)lueContractual.EditValue) return false;
            if (currContract.ContractExtensionID != (int)lueExtensions.EditValue) return false;
            if (currContract.ContractorID != (int)lueContractors.EditValue) return false;
            if (currContract.Summ != Convert.ToDouble(teSumm.Text)) return false;
            if (currContract.Note != teContractNote.Text) return false;
            if (currContract.Theme != teContractTheme.Text) return false;
            if (currContract.Date != Convert.ToDateTime(deDate.Text)) return false;
            if (currContract.StartDate != Convert.ToDateTime(deContractDateStart.Text)) return false;
            if (currContract.EndDate != Convert.ToDateTime(deContractDateEnd.Text)) return false;

            return true;
        }
    }
}
