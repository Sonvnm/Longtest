using Microsoft.EntityFrameworkCore;
using Reposiotry;
using Reposiotry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Longtest
{
    public partial class Management : Form
    {
        public UserServices _userServices { get; set; }
        public BankAccountServices _bankAccount { get; set; }
        public AccountTypeServices _accountType { get; set; }

        public Management(User user)
        {
            _userServices = new UserServices();
            _bankAccount = new BankAccountServices();
            _accountType = new AccountTypeServices();
            if (user == null)
            {
                this.Hide();
                var loginform = new Form1();
                loginform.ShowDialog();
            }
            InitializeComponent();
            GetListdgv();
        }

        private void Management_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var SearchKey = txtSearch.Text;
            var listAccount = _bankAccount.GetAll()
                                .Where(p => p.BranchName.Contains(SearchKey))
                                .Include(p => p.Type).ToList();
            dgvListAccount.DataSource = listAccount.Select(p => new {
                p.AccountId,
                p.AccountName,
                p.OpenDate,
                p.BranchName,
                p.Type.TypeName
            }).ToList();

        }

        private void dgvListAccount_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var AccountId = dgvListAccount.Rows[e.RowIndex].Cells[0].Value;
            if (AccountId != null)
            {
                var obj = _bankAccount.GetAll().ToList().Where(p => p.AccountId.Equals(AccountId)).FirstOrDefault();
                if (obj != null)
                {
                    txtAccountId.Text = obj.AccountId;
                    txtAccountName.Text = obj.AccountName;
                    txtBranchName.Text = obj.BranchName;
                    dtpOpenDate.Value = obj.OpenDate.Value;

                    var listType = _accountType.GetAll().ToList();
                    cbType.DataSource = listType;
                    cbType.DisplayMember = "TypeName";
                    cbType.ValueMember = "TypeId";
                    cbType.SelectedValue = obj.TypeId;

                }

            }

        }

        public void GetListdgv()
        {
            //show lai cai list
            var listBankAccount = _bankAccount.GetAll().Include(p=>p.Type).ToList();
            dgvListAccount.DataSource = listBankAccount.Select(p=>new {p.AccountId,p.AccountName,
                                            p.OpenDate,p.BranchName,p.Type.TypeName}).ToList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var AccountOld = _bankAccount.GetAll()
                .Where(p => p.AccountId.Equals(txtAccountId.Text)).FirstOrDefault();

            if (AccountOld==null)
            {
                MessageBox.Show("ID not existed, please try again Bros", "Error", MessageBoxButtons.OK);
                return;
            }

            AccountOld.BranchName= txtBranchName.Text;
            AccountOld.AccountName= txtAccountName.Text;
            AccountOld.OpenDate = dtpOpenDate.Value;
            AccountOld.TypeId = cbType.SelectedValue.ToString();

            _bankAccount.Update(AccountOld);

            //show lai cai list
            GetListdgv();


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var AccountOld = _bankAccount.GetAll()
                .Where(p => p.AccountId.Equals(txtAccountId.Text)).FirstOrDefault();
            _bankAccount.Delete(AccountOld);

            //show lai cai list
            GetListdgv();

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var AccountOld = _bankAccount.GetAll()
                .Where(p => p.AccountId.Equals(txtAccountId.Text)).FirstOrDefault();

            if (AccountOld!=null)
            {
                MessageBox.Show("ID existed, please try again Bros", "Error", MessageBoxButtons.OK);
                return;
            }

            var AccountNew = new BankAccount();
            AccountNew.AccountId = txtAccountId.Text;
            AccountNew.BranchName = txtBranchName.Text;
            AccountNew.AccountName = txtAccountName.Text;
            AccountNew.OpenDate = dtpOpenDate.Value;
            AccountNew.TypeId = cbType.SelectedValue.ToString();

            _bankAccount.Create(AccountNew);

            //show lai cai list
            GetListdgv();
        }
    }
}
