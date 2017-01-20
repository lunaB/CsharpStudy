using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    2017-01-19
    작성자 : 나영채
*/

namespace 지뢰찾기
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int cellWH = 25;

        int colCnt = 10;
        int rowCnt = 10;

        int mineNum = 1;

        Color defColor = Color.Empty;
        Color safeColor = Color.LightGray;
        Color warColor = Color.White;

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DefaultCellStyle.NullValue = "";
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

            swapMine(); // 지뢰 갯수
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor == warColor)
            {
                for(int i = 0; i < colCnt; i++)
                    for(int j = 0; j < rowCnt; j++)
                        if(dataGridView1[i, j].Style.BackColor == warColor)
                            dataGridView1[i, j].Style.BackColor = Color.Red;
                MessageBox.Show("Gaem Over");
            }
            else {
                paint(e.ColumnIndex, e.RowIndex);
                for(int i = 0; i < colCnt; i++)
                    for(int j = 0; j < rowCnt; j++)
                        if (dataGridView1[i, j].Style.BackColor == defColor)
                            return;


                MessageBox.Show("승리!");
                if (MessageBox.Show("재도전 하시겠습니까?", "지뢰찾기#영채", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    reset();
                }
                else {
                    Application.Exit();
                }
            }
        }

        private void paint(int col, int row)
        {
            //개수 검사
            int war = 0;
            
            //대각
            if (col != 0 && row != 0 && dataGridView1[col - 1, row - 1].Style.BackColor == warColor) war++;
            if (col != 0 && row != rowCnt - 1 && dataGridView1[col - 1, row + 1].Style.BackColor == warColor) war++;
            if (col != colCnt - 1 && row != 0 && dataGridView1[col + 1, row - 1].Style.BackColor == warColor) war++;
            if (col != colCnt - 1 && row != rowCnt - 1 && dataGridView1[col + 1, row + 1].Style.BackColor == warColor) war++;

            //십자
            if (col != 0 && dataGridView1[col - 1, row].Style.BackColor == warColor) war++;
            if (row != 0 && dataGridView1[col, row - 1].Style.BackColor == warColor) war++;
            if (col != colCnt - 1 && dataGridView1[col + 1, row].Style.BackColor == warColor) war++;
            if (row != rowCnt - 1 && dataGridView1[col, row + 1].Style.BackColor == warColor) war++;

            dataGridView1[col, row].Value = war;
            dataGridView1[col, row].Style.BackColor = safeColor;

            if (dataGridView1[col, row].Value.ToString().Equals("0"))
            {
                if (col != 0 && dataGridView1[col - 1, row].Style.BackColor == defColor) paint(col - 1, row);           //left
                if (row != 0 && dataGridView1[col, row - 1].Style.BackColor == defColor) paint(col, row - 1);           //up
                if (col != colCnt - 1 && dataGridView1[col + 1, row].Style.BackColor == defColor) paint(col + 1, row);  //right
                if (row != rowCnt - 1 && dataGridView1[col, row + 1].Style.BackColor == defColor) paint(col, row + 1);  //down
            }
        }

        void swapMine()
        {
            Random rand = new Random(System.DateTime.Now.Ticks.GetHashCode()); //거의 완벽한 난수 발생이 가능한 시드라고 배움
            for(int i = 0; i < mineNum; i++)
            {
                int cTmp = rand.Next(0, colCnt);
                int rTmp = rand.Next(0, rowCnt);
                if (dataGridView1[cTmp, rTmp].Style.BackColor == warColor)
                {
                    i--;
                    continue;
                }
                dataGridView1[cTmp, rTmp].Style.BackColor = warColor;
            }
        }
        
        //c
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView1[e.ColumnIndex, e.RowIndex].Value != null && dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Equals("X"))
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = "";
                }
                else if (dataGridView1[e.ColumnIndex, e.RowIndex].Value == null ||  dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString().Equals(""))
                {
                    dataGridView1[e.ColumnIndex, e.RowIndex].Value = "X";
                }
            }
        }

        //reset
        private void reset()
        {
            for (int i = 0; i < colCnt; i++)
            {
                for (int j = 0; j < rowCnt; j++)
                {
                    dataGridView1[i, j].Style.BackColor = defColor;
                    dataGridView1[i, j].Value = "";
                }
            }
            swapMine();
        }
    }
}
