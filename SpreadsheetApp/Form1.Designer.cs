
namespace SpreadsheetApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.LoadButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.loadExplain = new System.Windows.Forms.Label();
            this.curCellVal = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.Label();
            this.setCellBtn = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.getCellLbl1 = new System.Windows.Forms.Label();
            this.setCellTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadButton
            // 
            this.LoadButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LoadButton.Location = new System.Drawing.Point(95, 142);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(129, 40);
            this.LoadButton.TabIndex = 0;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.Load_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.Color.SeaGreen;
            this.dataGridView1.Location = new System.Drawing.Point(99, 229);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(770, 362);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // loadExplain
            // 
            this.loadExplain.AutoSize = true;
            this.loadExplain.BackColor = System.Drawing.Color.Transparent;
            this.loadExplain.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.loadExplain.Location = new System.Drawing.Point(99, 107);
            this.loadExplain.Name = "loadExplain";
            this.loadExplain.Size = new System.Drawing.Size(181, 22);
            this.loadExplain.TabIndex = 5;
            this.loadExplain.Text = "Choose .dat file to load";
            this.loadExplain.Click += new System.EventHandler(this.label1_Click);
            // 
            // curCellVal
            // 
            this.curCellVal.AutoSize = true;
            this.curCellVal.BackColor = System.Drawing.Color.Transparent;
            this.curCellVal.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.curCellVal.Location = new System.Drawing.Point(195, 190);
            this.curCellVal.Name = "curCellVal";
            this.curCellVal.Size = new System.Drawing.Size(0, 29);
            this.curCellVal.TabIndex = 6;
            this.curCellVal.Click += new System.EventHandler(this.curCellVal_Click);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.Font = new System.Drawing.Font("Calibri", 35F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Title.Location = new System.Drawing.Point(222, 15);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(539, 86);
            this.Title.TabIndex = 7;
            this.Title.Text = "SpreadSheet App";
            this.Title.Click += new System.EventHandler(this.label3_Click);
            // 
            // setCellBtn
            // 
            this.setCellBtn.Enabled = false;
            this.setCellBtn.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.setCellBtn.Location = new System.Drawing.Point(740, 141);
            this.setCellBtn.Name = "setCellBtn";
            this.setCellBtn.Size = new System.Drawing.Size(129, 40);
            this.setCellBtn.TabIndex = 8;
            this.setCellBtn.Text = "Set Cell";
            this.setCellBtn.UseVisualStyleBackColor = true;
            this.setCellBtn.Click += new System.EventHandler(this.setCellBtn_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.Enabled = false;
            this.saveBtn.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.saveBtn.Location = new System.Drawing.Point(230, 142);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(129, 40);
            this.saveBtn.TabIndex = 9;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // getCellLbl1
            // 
            this.getCellLbl1.AutoSize = true;
            this.getCellLbl1.BackColor = System.Drawing.Color.Transparent;
            this.getCellLbl1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.getCellLbl1.Location = new System.Drawing.Point(99, 192);
            this.getCellLbl1.Name = "getCellLbl1";
            this.getCellLbl1.Size = new System.Drawing.Size(97, 29);
            this.getCellLbl1.TabIndex = 10;
            this.getCellLbl1.Text = "Get Cell:";
            // 
            // setCellTextBox
            // 
            this.setCellTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.setCellTextBox.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.setCellTextBox.Location = new System.Drawing.Point(605, 141);
            this.setCellTextBox.Name = "setCellTextBox";
            this.setCellTextBox.Size = new System.Drawing.Size(129, 37);
            this.setCellTextBox.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.setCellTextBox);
            this.Controls.Add(this.getCellLbl1);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.setCellBtn);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.curCellVal);
            this.Controls.Add(this.loadExplain);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.LoadButton);
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label loadExplain;
        private System.Windows.Forms.Label curCellVal;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button setCellBtn;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label getCellLbl1;
        private System.Windows.Forms.TextBox setCellTextBox;
    }
}

