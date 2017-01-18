using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 지뢰찾기
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int cellWH = 20;

        int colCnt = 10;
        int rowCnt = 10;

        Color defColor = Color.Empty;
        Color safeColor = Color.LightGray;

        private void Form1_Load(object sender, EventArgs e)
        {
            
            dataGridView1.ColumnCount = colCnt;
            dataGridView1.RowCount = rowCnt+1; //지운 헤더가 한줄로 쳐져서 빼야됨
            for (int i = 0; i < colCnt; i++) {
                dataGridView1.Columns[i].Width = cellWH;
            }
            for (int i = 0; i < rowCnt+1; i++)
            {
                dataGridView1.Rows[i].Height= cellWH;
            }

            //헤더제거
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;
            //사용자 변경 제거
            dataGridView1.AllowDrop = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Blue;
            paint(e.ColumnIndex, e.RowIndex);
            MessageBox.Show(i+"");
        }
        int i = 0;
        private void paint(int col, int row)
        {

            dataGridView1[col, row].Style.BackColor = safeColor;
            i++;
            //왼
            if (col != 0 && dataGridView1[col - 1, row].Style.BackColor == defColor)
            {
                paint(col - 1, row);
            }
            //위
            if (row != 0 && dataGridView1[col, row - 1].Style.BackColor == defColor)
            {
                paint(col, row - 1);
            }
            //오
            if (col != colCnt - 1 && dataGridView1[col + 1, row].Style.BackColor == defColor)
            {
                paint(col + 1, row);
            }
            //아
            if (row != rowCnt - 1 && dataGridView1[col, row + 1].Style.BackColor == defColor)
            {
                paint(col, row + 1);
            }

            //개수 검사
            int war = 0;
            if (col != 0 && row != 0 && dataGridView1[col - 1, row - 1].Style.BackColor == Color.LawnGreen) war++;
            if (col != 0 && row != rowCnt - 1 && dataGridView1[col - 1, row + 1].Style.BackColor == Color.LawnGreen) war++;
            if (col != colCnt - 1 && row != 0 && dataGridView1[col + 1, row - 1].Style.BackColor == Color.LawnGreen) war++;
            if (col != colCnt - 1 && row != rowCnt - 1 && dataGridView1[col + 1, row + 1].Style.BackColor == Color.LawnGreen) war++;

            if (col != 0 && dataGridView1[col - 1, row].Style.BackColor == Color.LawnGreen) war++;
            if (row != 0 && dataGridView1[col, row - 1].Style.BackColor == Color.LawnGreen) war++;
            if (col != colCnt - 1 && dataGridView1[col + 1, row].Style.BackColor == Color.LawnGreen) war++;
            if (row != rowCnt - 1 && dataGridView1[col, row + 1].Style.BackColor == Color.LawnGreen) war++;

            dataGridView1[col, row].Value = war;

        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.LawnGreen;
            }
        }
    }
}
