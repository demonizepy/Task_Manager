namespace Task_Manager.Forms
{
    partial class MainForm
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
            this.groupNew = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.numId = new System.Windows.Forms.NumericUpDown();
            this.lblId = new System.Windows.Forms.Label();
            this.listTasks = new System.Windows.Forms.ListView();
            this.colId = new System.Windows.Forms.ColumnHeader();
            this.colDescription = new System.Windows.Forms.ColumnHeader();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numId)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupNew
            // 
            this.groupNew.Controls.Add(this.btnAdd);
            this.groupNew.Controls.Add(this.txtDescription);
            this.groupNew.Controls.Add(this.lblDescription);
            this.groupNew.Controls.Add(this.numId);
            this.groupNew.Controls.Add(this.lblId);
            this.groupNew.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupNew.Location = new System.Drawing.Point(0, 0);
            this.groupNew.Name = "groupNew";
            this.groupNew.Size = new System.Drawing.Size(720, 72);
            this.groupNew.TabIndex = 0;
            this.groupNew.TabStop = false;
            this.groupNew.Text = "Новая задача";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(606, 28);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 27);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(203, 29);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(395, 23);
            this.txtDescription.TabIndex = 3;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(132, 33);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(65, 15);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Описание:";
            // 
            // numId
            // 
            this.numId.Location = new System.Drawing.Point(42, 29);
            this.numId.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numId.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numId.Name = "numId";
            this.numId.Size = new System.Drawing.Size(75, 23);
            this.numId.TabIndex = 1;
            this.numId.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(14, 33);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(23, 15);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "ID:";
            // 
            // listTasks
            // 
            this.listTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colDescription});
            this.listTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTasks.FullRowSelect = true;
            this.listTasks.GridLines = true;
            this.listTasks.HideSelection = false;
            this.listTasks.Location = new System.Drawing.Point(0, 72);
            this.listTasks.Name = "listTasks";
            this.listTasks.Size = new System.Drawing.Size(720, 334);
            this.listTasks.TabIndex = 1;
            this.listTasks.UseCompatibleStateImageBehavior = false;
            this.listTasks.View = System.Windows.Forms.View.Details;
            // 
            // colId
            // 
            this.colId.Text = "ID";
            this.colId.Width = 80;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Описание";
            this.colDescription.Width = 600;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.btnClear);
            this.panelBottom.Controls.Add(this.btnDeleteSelected);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 406);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(720, 52);
            this.panelBottom.TabIndex = 2;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(172, 11);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 30);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Очистить всё";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Location = new System.Drawing.Point(14, 11);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(150, 30);
            this.btnDeleteSelected.TabIndex = 0;
            this.btnDeleteSelected.Text = "Удалить выбранные";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCount});
            this.statusStrip.Location = new System.Drawing.Point(0, 458);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(720, 22);
            this.statusStrip.TabIndex = 3;
            // 
            // lblCount
            // 
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(56, 17);
            this.lblCount.Text = "Задач: 0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 480);
            this.Controls.Add(this.listTasks);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupNew);
            this.MinimumSize = new System.Drawing.Size(560, 360);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Менеджер задач";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupNew.ResumeLayout(false);
            this.groupNew.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numId)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupNew;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.NumericUpDown numId;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.ListView listTasks;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblCount;
    }
}
