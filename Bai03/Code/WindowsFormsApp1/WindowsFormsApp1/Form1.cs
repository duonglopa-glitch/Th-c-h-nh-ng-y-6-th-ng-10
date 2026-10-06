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
    public partial class FormProductManager : Form
    {
        private BindingList<Product> _productList;
        private BindingSource _bindingSource;
        public FormProductManager()
        {
            InitializeComponent();
        }

        private void FormProductManager_Load(object sender, EventArgs e)
        {
            InitCategoryData();
            InitProductData();
            ConfigureDataGridView();
        }
        private void InitCategoryData()
        {
            cboCategory.Items.Clear();
            cboCategory.Items.Add("Điện thoại");
            cboCategory.Items.Add("Laptop");
            cboCategory.Items.Add("Phụ kiện");
            cboCategory.Items.Add("Thiết bị văn phòng");
            cboCategory.SelectedIndex = 0;
        }

        private void InitProductData()
        {
            _productList = new BindingList<Product>
            {
                new Product("SP001", "iPhone 15 Pro", 28000000m, 15, "Điện thoại"),
                new Product("SP002", "Laptop Dell XPS 13", 25000000m, 10, "Laptop"),
                new Product("SP003", "Tai nghe AirPods Pro", 5500000m, 30, "Phụ kiện")
            };

            _bindingSource = new BindingSource();
            _bindingSource.DataSource = _productList;
            dgvProducts.DataSource = _bindingSource;
        }

        private void ConfigureDataGridView()
        {
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.Columns.Clear();

            // Định nghĩa từng cột hiển thị trên DataGridView
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductId",
                HeaderText = "Mã SP",
                Width = 90
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Tên Sản Phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Category",
                HeaderText = "Danh Mục",
                Width = 130
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UnitPrice",
                HeaderText = "Đơn Giá (VNĐ)",
                Width = 130,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "Số Lượng",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string id = txtProductId.Text.Trim();

            if (_productList.Any(p => p.ProductId.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại! Vui lòng nhập mã khác.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductId.Focus();
                return;
            }

            Product newProduct = new Product(
                id,
                txtProductName.Text.Trim(),
                decimal.Parse(txtUnitPrice.Text.Trim()),
                int.Parse(txtQuantity.Text.Trim()),
                cboCategory.SelectedItem.ToString()
            );

            _productList.Add(newProduct);
            ClearInputFields();
            MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProducts.CurrentRow != null && dgvProducts.CurrentRow.DataBoundItem != null)
            {
                Product selectedProduct = (Product)dgvProducts.CurrentRow.DataBoundItem;

                txtProductId.Text = selectedProduct.ProductId;
                txtProductName.Text = selectedProduct.ProductName;
                txtUnitPrice.Text = selectedProduct.UnitPrice.ToString("G0");
                txtQuantity.Text = selectedProduct.Quantity.ToString();
                cboCategory.SelectedItem = selectedProduct.Category;

                txtProductId.ReadOnly = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || dgvProducts.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa từ danh sách!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            Product selectedProduct = (Product)dgvProducts.CurrentRow.DataBoundItem;

            selectedProduct.ProductName = txtProductName.Text.Trim();
            selectedProduct.Category = cboCategory.SelectedItem.ToString();
            selectedProduct.UnitPrice = decimal.Parse(txtUnitPrice.Text.Trim());
            selectedProduct.Quantity = int.Parse(txtQuantity.Text.Trim());

            _bindingSource.ResetBindings(false); 
            MessageBox.Show("Cập nhật thông tin sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || dgvProducts.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Product selectedProduct = (Product)dgvProducts.CurrentRow.DataBoundItem;

            DialogResult confirm = MessageBox.Show(
                string.Format("Bạn có chắc chắn muốn xóa sản phẩm [{0}]?", selectedProduct.ProductName),
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                _productList.Remove(selectedProduct);
                ClearInputFields();
                MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox1.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                _bindingSource.DataSource = _productList;
            }
            else
            {
                var filteredList = _productList
                    .Where(p => p.ProductName.ToLower().Contains(keyword))
                    .ToList();

                _bindingSource.DataSource = filteredList;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtProductId.Text))
            {
                MessageBox.Show("Mã sản phẩm không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProductId.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Tên sản phẩm không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProductName.Focus();
                return false;
            }

            if (!decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal price) || price < 0)
            {
                MessageBox.Show("Đơn giá phải là số dương hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUnitPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int qty) || qty < 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQuantity.Focus();
                return false;
            }

            return true;
        }

        private void ClearInputFields()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            cboCategory.SelectedIndex = 0;

            txtProductId.ReadOnly = false;
            dgvProducts.ClearSelection();
            txtProductId.Focus();
        }
    }
}
