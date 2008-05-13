namespace Bindable.Linq.Samples.WindowsForms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.processWrapperDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._processWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this._filterTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.processWrapperDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._processWrapperBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // processWrapperDataGridView
            // 
            this.processWrapperDataGridView.AllowUserToAddRows = false;
            this.processWrapperDataGridView.AllowUserToDeleteRows = false;
            this.processWrapperDataGridView.AllowUserToResizeColumns = false;
            this.processWrapperDataGridView.AllowUserToResizeRows = false;
            this.processWrapperDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.processWrapperDataGridView.AutoGenerateColumns = false;
            this.processWrapperDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.processWrapperDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.processWrapperDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.processWrapperDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.processWrapperDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.processWrapperDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.processWrapperDataGridView.DataSource = this._processWrapperBindingSource;
            this.processWrapperDataGridView.GridColor = System.Drawing.SystemColors.ControlLight;
            this.processWrapperDataGridView.Location = new System.Drawing.Point(12, 48);
            this.processWrapperDataGridView.Name = "processWrapperDataGridView";
            this.processWrapperDataGridView.RowHeadersVisible = false;
            this.processWrapperDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.processWrapperDataGridView.Size = new System.Drawing.Size(640, 484);
            this.processWrapperDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ProcessName";
            this.dataGridViewTextBoxColumn1.HeaderText = "Process Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ID";
            this.dataGridViewTextBoxColumn2.FillWeight = 20F;
            this.dataGridViewTextBoxColumn2.HeaderText = "ID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Title";
            this.dataGridViewTextBoxColumn3.FillWeight = 175F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Title";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // _processWrapperBindingSource
            // 
            this._processWrapperBindingSource.DataSource = typeof(Bindable.Linq.Samples.WindowsForms.ProcessWrapper);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Find a process:";
            // 
            // _filterTextBox
            // 
            this._filterTextBox.Location = new System.Drawing.Point(104, 10);
            this._filterTextBox.Name = "_filterTextBox";
            this._filterTextBox.Size = new System.Drawing.Size(226, 23);
            this._filterTextBox.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 544);
            this.Controls.Add(this._filterTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.processWrapperDataGridView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "Bindable LINQ Process Explorer";
            ((System.ComponentModel.ISupportInitialize)(this.processWrapperDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._processWrapperBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource _processWrapperBindingSource;
        private System.Windows.Forms.DataGridView processWrapperDataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _filterTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;


    }
}

