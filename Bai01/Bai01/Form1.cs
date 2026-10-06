using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai01
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FormRegister_Load(object sender, EventArgs e)
        {
            dtpBirthDate.Value = DateTime.Now;
            rdoMale.Checked = true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            epCheck.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                epCheck.SetError(txtUsername, "Tên đăng nhập không được để trống!");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                epCheck.SetError(txtPassword, "Mật khẩu không được để trống!");
                isValid = false;
            }

            
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                epCheck.SetError(txtConfirmPassword, "Vui lòng nhập lại mật khẩu!");
                isValid = false;
            }
            else if (txtConfirmPassword.Text != txtPassword.Text)
            {
                epCheck.SetError(txtConfirmPassword, "Mật khẩu xác nhận không khớp!");
                isValid = false;
            }

            DateTime birthDate = dtpBirthDate.Value.Date;
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            if (age < 18)
            {
                epCheck.SetError(dtpBirthDate, "Bạn phải đủ 18 tuổi trở lên mới được đăng ký!");
                isValid = false;
            }

            if (!chkTerms.Checked)
            {
                epCheck.SetError(chkTerms, "Bạn phải tích chọn đồng ý với Điều khoản dịch vụ!");
                isValid = false;
            }

            if (isValid)
            {
                string gender = rdoMale.Checked ? "Nam" : "Nữ";
                string message = string.Format(
                    "Đăng ký tài khoản thành công!\n\n" +
                    "- Tên đăng nhập: {0}\n" +
                    "- Ngày sinh: {1}\n" +
                    "- Giới tính: {2}",
                    txtUsername.Text.Trim(),
                    birthDate.ToString("dd/MM/yyyy"),
                    gender
                );

                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            dtpBirthDate.Value = DateTime.Now;
            rdoMale.Checked = true;
            rdoFemale.Checked = false;
            chkTerms.Checked = false;

            epCheck.Clear();

            txtUsername.Focus();
        }
    }
}
