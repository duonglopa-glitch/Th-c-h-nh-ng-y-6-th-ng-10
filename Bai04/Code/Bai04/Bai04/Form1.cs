using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai04
{
    public partial class FormDepartmentManager : Form
    {
        private List<Employee> _employeeList;
        public FormDepartmentManager()
        {
            InitializeComponent();
        }

        private void FormDepartmentManager_Load(object sender, EventArgs e)
        {
            InitViewModeComboBox();
            InitListViewColumns();
            InitTreeViewData();
            InitEmployeeData();

            if (tvDepartments.Nodes.Count > 0)
            {
                tvDepartments.SelectedNode = tvDepartments.Nodes[0];
                tvDepartments.Nodes[0].Expand();
            }
        }
            private void InitViewModeComboBox()
        {
            cboViewMode.Items.Clear();
            cboViewMode.Items.Add(View.Details);
            cboViewMode.Items.Add(View.LargeIcon);
            cboViewMode.Items.Add(View.SmallIcon);
            cboViewMode.Items.Add(View.List);
            cboViewMode.Items.Add(View.Tile);

            cboViewMode.SelectedIndex = 0; // Mặc định là Details
        }

        private void InitListViewColumns()
        {
            lsvEmployees.Columns.Clear();
            lsvEmployees.Columns.Add("Mã NV", 90, HorizontalAlignment.Left);
            lsvEmployees.Columns.Add("Họ Tên", 180, HorizontalAlignment.Left);
            lsvEmployees.Columns.Add("Chức Vụ", 140, HorizontalAlignment.Left);
            lsvEmployees.Columns.Add("Ngày Vào Làm", 120, HorizontalAlignment.Center);
        }

        private void InitTreeViewData()
        {
            tvDepartments.Nodes.Clear();

            TreeNode rootNode = new TreeNode("Tập đoàn TechMart")
            {
                Tag = "COMPANY",
                ImageIndex = 0,
                SelectedImageIndex = 0
            };

            TreeNode hrNode = new TreeNode("Phòng Nhân Sự") { Tag = "HR", ImageIndex = 1, SelectedImageIndex = 1 };
            TreeNode techNode = new TreeNode("Phòng Kỹ Thuật") { Tag = "TECH", ImageIndex = 1, SelectedImageIndex = 1 };
            TreeNode salesNode = new TreeNode("Phòng Kinh Doanh") { Tag = "SALES", ImageIndex = 1, SelectedImageIndex = 1 };

            hrNode.Nodes.Add(new TreeNode("Nhóm Tuyển Dụng") { Tag = "HR_REC", ImageIndex = 2, SelectedImageIndex = 2 });
            hrNode.Nodes.Add(new TreeNode("Nhóm C&B") { Tag = "HR_CB", ImageIndex = 2, SelectedImageIndex = 2 });

            techNode.Nodes.Add(new TreeNode("Nhóm Dev") { Tag = "TECH_DEV", ImageIndex = 2, SelectedImageIndex = 2 });
            techNode.Nodes.Add(new TreeNode("Nhóm QA/QC") { Tag = "TECH_QA", ImageIndex = 2, SelectedImageIndex = 2 });

            salesNode.Nodes.Add(new TreeNode("Nhóm Bán Hàng") { Tag = "SALES_DIRECT", ImageIndex = 2, SelectedImageIndex = 2 });

            rootNode.Nodes.Add(hrNode);
            rootNode.Nodes.Add(techNode);
            rootNode.Nodes.Add(salesNode);

            tvDepartments.Nodes.Add(rootNode);
            tvDepartments.ExpandAll();
        }

        private void InitEmployeeData()
        {
            _employeeList = new List<Employee>
            {
                new Employee("NV001", "Nguyễn Văn An", "Trưởng Phòng HR", new DateTime(2020, 3, 15), "HR", 0),
                new Employee("NV002", "Trần Thị Bích", "Chuyên Viên Tuyển Dụng", new DateTime(2021, 6, 1), "HR_REC", 1),
                new Employee("NV003", "Lê Hoàng Cường", "Chuyên Viên C&B", new DateTime(2022, 1, 10), "HR_CB", 1),

                new Employee("NV004", "Phạm Minh Đức", "Giám Đốc Kỹ Thuật", new DateTime(2019, 1, 5), "TECH", 0),
                new Employee("NV005", "Vũ Thị Em", "Senior Developer", new DateTime(2021, 8, 20), "TECH_DEV", 2),
                new Employee("NV006", "Đặng Văn Giang", "Junior Developer", new DateTime(2023, 2, 1), "TECH_DEV", 2),
                new Employee("NV007", "Hoàng Thị Hương", "Leader QA", new DateTime(2020, 11, 12), "TECH_QA", 3),

                new Employee("NV008", "Nông Văn Hùng", "Trưởng Phòng Sales", new DateTime(2018, 5, 10), "SALES", 0),
                new Employee("NV009", "Đỗ Kim Yến", "Nhân Viên Kinh Doanh", new DateTime(2022, 9, 15), "SALES_DIRECT", 4)
            };
        }

        private void tvDepartments_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null) return;

            string selectedTag = e.Node.Tag.ToString();
            LoadEmployeesToListView(selectedTag);
        }
        private void LoadEmployeesToListView(string deptCode)
        {
            lsvEmployees.Items.Clear();

            List<Employee> filteredEmployees;

            if (deptCode == "COMPANY")
            {
                filteredEmployees = _employeeList;
            }
            else
            { 
                filteredEmployees = _employeeList
                    .Where(emp => emp.DepartmentCode == deptCode || emp.DepartmentCode.StartsWith(deptCode + "_"))
                    .ToList();
            }

            foreach (var emp in filteredEmployees)
            {
                ListViewItem item = new ListViewItem(emp.EmployeeId, emp.ImageIndex);

                item.SubItems.Add(emp.FullName);
                item.SubItems.Add(emp.Position);
                item.SubItems.Add(emp.JoinDate.ToString("dd/MM/yyyy"));

                item.Tag = emp;

                lsvEmployees.Items.Add(item);
            }
        }

        private void cboViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboViewMode.SelectedItem != null)
            {
                lsvEmployees.View = (View)cboViewMode.SelectedItem;
            }

        }
    }
    
    
}
