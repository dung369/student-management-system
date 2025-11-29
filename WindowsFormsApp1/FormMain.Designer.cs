namespace WindowsFormsApp1
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.cboKhoa = new System.Windows.Forms.ComboBox();
            this.dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.chkPhai = new System.Windows.Forms.CheckBox();
            this.txtHocBong = new System.Windows.Forms.TextBox();
            this.txtNoiSinh = new System.Windows.Forms.TextBox();
            this.txtTenSV = new System.Windows.Forms.TextBox();
            this.txtHoSV = new System.Windows.Forms.TextBox();
            this.txtMaSV = new System.Windows.Forms.TextBox();
            this.lblKhoa = new System.Windows.Forms.Label();
            this.lblHocBong = new System.Windows.Forms.Label();
            this.lblNoiSinh = new System.Windows.Forms.Label();
            this.lblNgaySinh = new System.Windows.Forms.Label();
            this.lblPhai = new System.Windows.Forms.Label();
            this.lblTenSV = new System.Windows.Forms.Label();
            this.lblHoSV = new System.Windows.Forms.Label();
            this.lblMaSV = new System.Windows.Forms.Label();
            this.groupBoxActions = new System.Windows.Forms.GroupBox();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.groupBoxSearch = new System.Windows.Forms.GroupBox();
            this.btnSearchExecute = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.lblServerInfo = new System.Windows.Forms.Label();
            this.groupBoxInfo.SuspendLayout();
            this.groupBoxActions.SuspendLayout();
            this.groupBoxSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Blue;
            this.lblTitle.Location = new System.Drawing.Point(400, 18);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(541, 36);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ SINH VIÊN - PHÂN MẢNH";
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Controls.Add(this.cboKhoa);
            this.groupBoxInfo.Controls.Add(this.dtpNgaySinh);
            this.groupBoxInfo.Controls.Add(this.chkPhai);
            this.groupBoxInfo.Controls.Add(this.txtHocBong);
            this.groupBoxInfo.Controls.Add(this.txtNoiSinh);
            this.groupBoxInfo.Controls.Add(this.txtTenSV);
            this.groupBoxInfo.Controls.Add(this.txtHoSV);
            this.groupBoxInfo.Controls.Add(this.txtMaSV);
            this.groupBoxInfo.Controls.Add(this.lblKhoa);
            this.groupBoxInfo.Controls.Add(this.lblHocBong);
            this.groupBoxInfo.Controls.Add(this.lblNoiSinh);
            this.groupBoxInfo.Controls.Add(this.lblNgaySinh);
            this.groupBoxInfo.Controls.Add(this.lblPhai);
            this.groupBoxInfo.Controls.Add(this.lblTenSV);
            this.groupBoxInfo.Controls.Add(this.lblHoSV);
            this.groupBoxInfo.Controls.Add(this.lblMaSV);
            this.groupBoxInfo.Location = new System.Drawing.Point(27, 74);
            this.groupBoxInfo.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxInfo.Size = new System.Drawing.Size(1387, 185);
            this.groupBoxInfo.TabIndex = 1;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Thông tin sinh viên";
            // 
            // cboKhoa
            // 
            this.cboKhoa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKhoa.FormattingEnabled = true;
            this.cboKhoa.Location = new System.Drawing.Point(1027, 72);
            this.cboKhoa.Margin = new System.Windows.Forms.Padding(4);
            this.cboKhoa.Name = "cboKhoa";
            this.cboKhoa.Size = new System.Drawing.Size(239, 24);
            this.cboKhoa.TabIndex = 15;
            this.cboKhoa.SelectedIndexChanged += new System.EventHandler(this.cboKhoa_SelectedIndexChanged);
            // 
            // dtpNgaySinh
            // 
            this.dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgaySinh.Location = new System.Drawing.Point(638, 81);
            this.dtpNgaySinh.Margin = new System.Windows.Forms.Padding(4);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.Size = new System.Drawing.Size(239, 22);
            this.dtpNgaySinh.TabIndex = 14;
            // 
            // chkPhai
            // 
            this.chkPhai.AutoSize = true;
            this.chkPhai.Location = new System.Drawing.Point(638, 37);
            this.chkPhai.Margin = new System.Windows.Forms.Padding(4);
            this.chkPhai.Name = "chkPhai";
            this.chkPhai.Size = new System.Drawing.Size(46, 20);
            this.chkPhai.TabIndex = 13;
            this.chkPhai.Text = "Nữ";
            this.chkPhai.UseVisualStyleBackColor = true;
            // 
            // txtHocBong
            // 
            this.txtHocBong.Location = new System.Drawing.Point(1027, 119);
            this.txtHocBong.Margin = new System.Windows.Forms.Padding(4);
            this.txtHocBong.Name = "txtHocBong";
            this.txtHocBong.Size = new System.Drawing.Size(239, 22);
            this.txtHocBong.TabIndex = 12;
            this.txtHocBong.Text = "0";
            // 
            // txtNoiSinh
            // 
            this.txtNoiSinh.Location = new System.Drawing.Point(638, 119);
            this.txtNoiSinh.Margin = new System.Windows.Forms.Padding(4);
            this.txtNoiSinh.Name = "txtNoiSinh";
            this.txtNoiSinh.Size = new System.Drawing.Size(239, 22);
            this.txtNoiSinh.TabIndex = 11;
            // 
            // txtTenSV
            // 
            this.txtTenSV.Location = new System.Drawing.Point(133, 116);
            this.txtTenSV.Margin = new System.Windows.Forms.Padding(4);
            this.txtTenSV.Name = "txtTenSV";
            this.txtTenSV.Size = new System.Drawing.Size(239, 22);
            this.txtTenSV.TabIndex = 10;
            // 
            // txtHoSV
            // 
            this.txtHoSV.Location = new System.Drawing.Point(133, 80);
            this.txtHoSV.Margin = new System.Windows.Forms.Padding(4);
            this.txtHoSV.Name = "txtHoSV";
            this.txtHoSV.Size = new System.Drawing.Size(239, 22);
            this.txtHoSV.TabIndex = 9;
            // 
            // txtMaSV
            // 
            this.txtMaSV.Location = new System.Drawing.Point(133, 37);
            this.txtMaSV.Margin = new System.Windows.Forms.Padding(4);
            this.txtMaSV.MaxLength = 3;
            this.txtMaSV.Name = "txtMaSV";
            this.txtMaSV.Size = new System.Drawing.Size(239, 22);
            this.txtMaSV.TabIndex = 8;
            // 
            // lblKhoa
            // 
            this.lblKhoa.AutoSize = true;
            this.lblKhoa.Location = new System.Drawing.Point(958, 80);
            this.lblKhoa.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKhoa.Name = "lblKhoa";
            this.lblKhoa.Size = new System.Drawing.Size(41, 16);
            this.lblKhoa.TabIndex = 7;
            this.lblKhoa.Text = "Khoa:";
            // 
            // lblHocBong
            // 
            this.lblHocBong.AutoSize = true;
            this.lblHocBong.Location = new System.Drawing.Point(949, 122);
            this.lblHocBong.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHocBong.Name = "lblHocBong";
            this.lblHocBong.Size = new System.Drawing.Size(69, 16);
            this.lblHocBong.TabIndex = 6;
            this.lblHocBong.Text = "Học bổng:";
            // 
            // lblNoiSinh
            // 
            this.lblNoiSinh.AutoSize = true;
            this.lblNoiSinh.Location = new System.Drawing.Point(530, 122);
            this.lblNoiSinh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNoiSinh.Name = "lblNoiSinh";
            this.lblNoiSinh.Size = new System.Drawing.Size(58, 16);
            this.lblNoiSinh.TabIndex = 5;
            this.lblNoiSinh.Text = "Nơi sinh:";
            this.lblNoiSinh.Click += new System.EventHandler(this.lblNoiSinh_Click);
            // 
            // lblNgaySinh
            // 
            this.lblNgaySinh.AutoSize = true;
            this.lblNgaySinh.Location = new System.Drawing.Point(530, 86);
            this.lblNgaySinh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNgaySinh.Name = "lblNgaySinh";
            this.lblNgaySinh.Size = new System.Drawing.Size(70, 16);
            this.lblNgaySinh.TabIndex = 4;
            this.lblNgaySinh.Text = "Ngày sinh:";
            // 
            // lblPhai
            // 
            this.lblPhai.AutoSize = true;
            this.lblPhai.Location = new System.Drawing.Point(530, 43);
            this.lblPhai.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPhai.Name = "lblPhai";
            this.lblPhai.Size = new System.Drawing.Size(57, 16);
            this.lblPhai.TabIndex = 3;
            this.lblPhai.Text = "Giới tính:";
            // 
            // lblTenSV
            // 
            this.lblTenSV.AutoSize = true;
            this.lblTenSV.Location = new System.Drawing.Point(27, 122);
            this.lblTenSV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTenSV.Name = "lblTenSV";
            this.lblTenSV.Size = new System.Drawing.Size(55, 16);
            this.lblTenSV.TabIndex = 2;
            this.lblTenSV.Text = "Tên SV:";
            // 
            // lblHoSV
            // 
            this.lblHoSV.AutoSize = true;
            this.lblHoSV.Location = new System.Drawing.Point(27, 86);
            this.lblHoSV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHoSV.Name = "lblHoSV";
            this.lblHoSV.Size = new System.Drawing.Size(49, 16);
            this.lblHoSV.TabIndex = 1;
            this.lblHoSV.Text = "Họ SV:";
            // 
            // lblMaSV
            // 
            this.lblMaSV.AutoSize = true;
            this.lblMaSV.Location = new System.Drawing.Point(27, 41);
            this.lblMaSV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaSV.Name = "lblMaSV";
            this.lblMaSV.Size = new System.Drawing.Size(50, 16);
            this.lblMaSV.TabIndex = 0;
            this.lblMaSV.Text = "Mã SV:";
            // 
            // groupBoxActions
            // 
            this.groupBoxActions.Controls.Add(this.btnTransfer);
            this.groupBoxActions.Controls.Add(this.btnSearch);
            this.groupBoxActions.Controls.Add(this.btnDelete);
            this.groupBoxActions.Controls.Add(this.btnEdit);
            this.groupBoxActions.Controls.Add(this.btnAdd);
            this.groupBoxActions.Controls.Add(this.btnRefresh);
            this.groupBoxActions.Location = new System.Drawing.Point(27, 271);
            this.groupBoxActions.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxActions.Name = "groupBoxActions";
            this.groupBoxActions.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxActions.Size = new System.Drawing.Size(907, 98);
            this.groupBoxActions.TabIndex = 2;
            this.groupBoxActions.TabStop = false;
            this.groupBoxActions.Text = "Chức năng";
            // 
            // btnTransfer
            // 
            this.btnTransfer.BackColor = System.Drawing.Color.White;
            this.btnTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransfer.Location = new System.Drawing.Point(613, 31);
            this.btnTransfer.Margin = new System.Windows.Forms.Padding(4);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(133, 49);
            this.btnTransfer.TabIndex = 5;
            this.btnTransfer.Text = "CHUYỂN KHOA";
            this.btnTransfer.UseVisualStyleBackColor = false;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(467, 31);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(133, 49);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "TÌM KIẾM";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.White;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(320, 31);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(133, 49);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "XÓA";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.White;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(173, 31);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(133, 49);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "SỬA";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.White;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(27, 31);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(133, 49);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "THÊM";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(760, 31);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(133, 49);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "LÀM MỚI";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // groupBoxSearch
            // 
            this.groupBoxSearch.Controls.Add(this.btnSearchExecute);
            this.groupBoxSearch.Controls.Add(this.txtSearch);
            this.groupBoxSearch.Controls.Add(this.lblSearch);
            this.groupBoxSearch.Location = new System.Drawing.Point(947, 271);
            this.groupBoxSearch.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxSearch.Name = "groupBoxSearch";
            this.groupBoxSearch.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxSearch.Size = new System.Drawing.Size(467, 98);
            this.groupBoxSearch.TabIndex = 3;
            this.groupBoxSearch.TabStop = false;
            this.groupBoxSearch.Text = "Tìm kiếm nhanh";
            // 
            // btnSearchExecute
            // 
            this.btnSearchExecute.Location = new System.Drawing.Point(347, 37);
            this.btnSearchExecute.Margin = new System.Windows.Forms.Padding(4);
            this.btnSearchExecute.Name = "btnSearchExecute";
            this.btnSearchExecute.Size = new System.Drawing.Size(100, 43);
            this.btnSearchExecute.TabIndex = 2;
            this.btnSearchExecute.Text = "Tìm";
            this.btnSearchExecute.UseVisualStyleBackColor = true;
            this.btnSearchExecute.Click += new System.EventHandler(this.btnSearchExecute_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(107, 46);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(225, 22);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(20, 49);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(59, 16);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Từ khóa:";
            // 
            // dgvStudents
            // 
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            this.dgvStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStudents.BackgroundColor = System.Drawing.Color.White;
            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudents.Location = new System.Drawing.Point(27, 382);
            this.dgvStudents.Margin = new System.Windows.Forms.Padding(4);
            this.dgvStudents.MultiSelect = false;
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.RowHeadersWidth = 51;
            this.dgvStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudents.Size = new System.Drawing.Size(1387, 369);
            this.dgvStudents.TabIndex = 4;
            this.dgvStudents.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStudents_CellClick);
            // 
            // lblServerInfo
            // 
            this.lblServerInfo.AutoSize = true;
            this.lblServerInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerInfo.ForeColor = System.Drawing.Color.Green;
            this.lblServerInfo.Location = new System.Drawing.Point(27, 757);
            this.lblServerInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblServerInfo.Name = "lblServerInfo";
            this.lblServerInfo.Size = new System.Drawing.Size(296, 18);
            this.lblServerInfo.TabIndex = 5;
            this.lblServerInfo.Text = "Kết nối: Server Mẹ - SQLEXPRESS03:1433";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1440, 788);
            this.Controls.Add(this.lblServerInfo);
            this.Controls.Add(this.dgvStudents);
            this.Controls.Add(this.groupBoxSearch);
            this.Controls.Add(this.groupBoxActions);
            this.Controls.Add(this.groupBoxInfo);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý sinh viên - Hệ thống phân mảnh";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.groupBoxActions.ResumeLayout(false);
            this.groupBoxSearch.ResumeLayout(false);
            this.groupBoxSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label lblMaSV;
        private System.Windows.Forms.Label lblHoSV;
        private System.Windows.Forms.Label lblTenSV;
        private System.Windows.Forms.Label lblPhai;
        private System.Windows.Forms.Label lblNgaySinh;
        private System.Windows.Forms.Label lblNoiSinh;
        private System.Windows.Forms.Label lblHocBong;
        private System.Windows.Forms.Label lblKhoa;
        private System.Windows.Forms.TextBox txtMaSV;
        private System.Windows.Forms.TextBox txtHoSV;
        private System.Windows.Forms.TextBox txtTenSV;
        private System.Windows.Forms.TextBox txtNoiSinh;
        private System.Windows.Forms.TextBox txtHocBong;
        private System.Windows.Forms.CheckBox chkPhai;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh;
        private System.Windows.Forms.ComboBox cboKhoa;
        private System.Windows.Forms.GroupBox groupBoxActions;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.GroupBox groupBoxSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearchExecute;
        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.Label lblServerInfo;
    }
}
