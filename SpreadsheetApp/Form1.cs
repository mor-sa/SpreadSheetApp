using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;


namespace SpreadsheetApp
{
    [Serializable]
    public partial class Form1 : Form
    {
        private String loadFileName = "";
        private SharableSpreadSheet spreadSheet;
        private DataGridViewCell selectedCell;

        public Form1()
        {
            InitializeComponent();
        }

        private void Load_Click(object sender, EventArgs e)
        {
            this.spreadSheet = new SharableSpreadSheet(5, 5);
            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            openFileDialog1.ShowDialog();
            this.loadFileName = openFileDialog1.FileName;
            this.spreadSheet.load(this.loadFileName);
            this.showSpreadSheet();
            this.saveBtn.Enabled = true;
            this.setCellBtn.Enabled = true;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            //this.spreadSheet.save(this.fil);
            saveFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            saveFileDialog1.ShowDialog();
            this.loadFileName = saveFileDialog1.FileName;
            if (this.spreadSheet.save(this.loadFileName))
            {
                showDialogSave();
            }
        }

        private void showDialogSave()
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Saved Succesfully",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel1 = new Label() { Left = 20, Top = 20, Width=450, Text = "The file was saved succesfully!" };
            Button confirmation = new Button() { Text = "OK", Left = 200, Width = 100, Height = 50, Top = 120, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textLabel1);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            prompt.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void showSpreadSheet()
        {
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 255, 230);
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 255, 230);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(240, 255, 230);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(91, 255, 69);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.FromArgb(12, 145, 75);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DefaultCellStyle.Font = new Font("Calibri", 14F, GraphicsUnit.Pixel);
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Columns.Clear();

            for (int i = 0; i < this.spreadSheet.cols; i++)
            {
                int colNum = i + 1;
                String colName = "Col" + colNum;
                String colText = "Col" + colNum;
                dataGridView1.Columns.Add(colName, colText);
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

                //dataGridView1.Columns[i].Width = 110;
                for (int j = 0; j < this.spreadSheet.rows; j++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.RowHeadersWidth = 100;

                    int rowNum = j + 1;
                    String rowName = "Row" + rowNum;
                    dataGridView1.Rows[j].HeaderCell.Value = rowName;
                    dataGridView1[i, j].Value = spreadSheet.getCell(j, i);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(!(e.RowIndex < 0 || e.RowIndex > spreadSheet.rows - 1 || e.ColumnIndex < 0 || e.ColumnIndex > spreadSheet.cols - 1))
            {
                this.selectedCell = (DataGridViewCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (this.selectedCell != null)
                {
                    curCellVal.Text = this.spreadSheet.getCell(this.selectedCell.RowIndex, this.selectedCell.ColumnIndex);
                }
                else
                {
                    curCellVal.Text = " ";
                }
                
            }
            else
            {
                curCellVal.Text = " ";
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void curCellVal_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void setCellBtn_Click(object sender, EventArgs e)
        {
            if (this.spreadSheet != null || this.selectedCell != null)
            {
                if (setCellTextBox.Text != null)
                {
                    String stringToSet = setCellTextBox.Text;
                    this.spreadSheet.setCell(this.selectedCell.RowIndex, this.selectedCell.ColumnIndex, stringToSet);
                    this.dataGridView1[this.selectedCell.ColumnIndex, this.selectedCell.RowIndex].Value = this.spreadSheet.getCell(this.selectedCell.RowIndex, this.selectedCell.ColumnIndex);
                }
            }
        }

        // Instead of prompt i used a text box
        private String showDialogSetCell()
        {
        
            Form prompt = new Form()
            {
                Width = 500,
                Height = 500,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Set Cell",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel1 = new Label() { Left = 110, Top = 20, Text = "Input:" };
            TextBox textBox1 = new TextBox() { Left = 110, Top = 70, Width = 250 };
            Button confirmation = new Button() { Text = "Set", Left = 200, Width = 100, Height=50, Top = 320, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close();};
            
            prompt.Controls.Add(textBox1);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel1);
            prompt.AcceptButton = confirmation;

            prompt.ShowDialog();

            return textBox1.Text;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.selectedCell != null)
            {
                DataGridView dgv = (DataGridView)sender;
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < this.spreadSheet.rows - 1 && e.ColumnIndex < this.spreadSheet.cols - 1)
                {
                    String stringToSet = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    this.spreadSheet.setCell(this.selectedCell.RowIndex, this.selectedCell.ColumnIndex, stringToSet);
                    this.dataGridView1[this.selectedCell.ColumnIndex, this.selectedCell.RowIndex].Value = this.spreadSheet.getCell(this.selectedCell.RowIndex, this.selectedCell.ColumnIndex);
                }
            }
            
        }
    }
}
