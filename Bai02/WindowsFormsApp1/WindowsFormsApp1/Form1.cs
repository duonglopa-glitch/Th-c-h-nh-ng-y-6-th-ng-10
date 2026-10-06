using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormServiceBilling : Form
    {
        private List<ServiceItem> _allServices;
        public FormServiceBilling()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FormServiceBilling_Load(object sender, EventArgs e)
        {
            InitData();
            InitCategoryComboBox();
            CalculateTotal();
        }
        private void InitData()
        {
            _allServices = new List<ServiceItem>
            {
                new ServiceItem("Khám tổng quát", 150000m, "Khám bệnh"),
                new ServiceItem("Khám chuyên khoa", 250000m, "Khám bệnh"),
                new ServiceItem("Khám VIP theo yêu cầu", 500000m, "Khám bệnh"),

                new ServiceItem("Xét nghiệm công thức máu", 200000m, "Xét nghiệm"),
                new ServiceItem("Xét nghiệm nước tiểu", 100000m, "Xét nghiệm"),
                new ServiceItem("Xét nghiệm sinh hóa máu", 350000m, "Xét nghiệm"),

                new ServiceItem("Chụp X-Quang Phổi", 180000m, "Chụp X-Quang"),
                new ServiceItem("Chụp X-Quang Cột sống", 220000m, "Chụp X-Quang"),
                new ServiceItem("Chụp CT Scanner", 1500000m, "Chụp X-Quang"),

                new ServiceItem("Vắc-xin Cúm mùa", 350000m, "Vắc-xin"),
                new ServiceItem("Vắc-xin Viêm gan B", 250000m, "Vắc-xin"),
                new ServiceItem("Vắc-xin 6 trong 1", 1000000m, "Vắc-xin")
            };
        }

        private void InitCategoryComboBox()
        {
            cboCategory.Items.Clear();
            cboCategory.Items.Add("Khám bệnh");
            cboCategory.Items.Add("Xét nghiệm");
            cboCategory.Items.Add("Chụp X-Quang");
            cboCategory.Items.Add("Vắc-xin");

            cboCategory.SelectedIndex = 0;
        }

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCategory.SelectedItem == null) return;

            string selectedCategory = cboCategory.SelectedItem.ToString();

            var filteredServices = _allServices
                .Where(s => s.Category == selectedCategory)
                .ToList();

            lstAvailableServices.DataSource = null;
            lstAvailableServices.DataSource = filteredServices;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            AddSelectedService();
        }

        private void lstAvailableServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddSelectedService();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedService();
        }

        private void lstSelectedServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveSelectedService();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (lstSelectedServices.Items.Count > 0)
            {
                lstSelectedServices.Items.Clear();
                CalculateTotal();
            }
        }
        private void AddSelectedService()
        {
            if (lstAvailableServices.SelectedItem == null) return;

            ServiceItem selectedItem = (ServiceItem)lstAvailableServices.SelectedItem;

            if (lstSelectedServices.Items.Contains(selectedItem))
            {
                MessageBox.Show("Dịch vụ này đã được chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            lstSelectedServices.Items.Add(selectedItem);
            CalculateTotal();
        }
        private void RemoveSelectedService()
        {
            if (lstSelectedServices.SelectedItem == null) return;

            lstSelectedServices.Items.Remove(lstSelectedServices.SelectedItem);
            CalculateTotal();
        }

        private void nudDiscount_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }
        private void CalculateTotal()
        {
            decimal subTotal = 0m;

            foreach (ServiceItem item in lstSelectedServices.Items)
            {
                subTotal += item.Price;
            }

            decimal discountPercent = nudDiscount.Value;
            decimal discountAmount = subTotal * (discountPercent / 100m);
            decimal totalPay = subTotal - discountAmount;

            txtSubTotal.Text = string.Format("{0:N0} VNĐ", subTotal);
            txtTotal.Text = string.Format("{0:N0} VNĐ", totalPay);
        }
    }
}
