using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace CsvtoDataGridView
{
    public partial class Change : Form
    {
        string fname;                           //ファイル名を取得
        public DataTable dt = new DataTable();         //表示用データテーブルの作成

        public Change()
        {
            InitializeComponent();
        }

        private void ReadCsv_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "ファイル読込み";
            ofd.Filter = "csvファイル|*.csv";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                fname = ofd.FileName;
                textBox1.Text = fname;

                if(textBox1.Text != null)
                {
                    
                    //CSVファイルを読み込むときに使うEncoding
                    System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");

                    //パスが存在しないとき処理を終了
                    if (!System.IO.File.Exists(textBox1.Text)) return;

                    //CSVを読み込むために必要なクラス
                    System.IO.StreamReader sr = new System.IO.StreamReader(textBox1.Text, enc);

                    string line = null;//1行読込用変数
                    string[] cells = null;//1行を1マス毎に分割した配列

                    //1行目の処理（列数をカウント）
                    //CSVの読み込み部分
                    line = sr.ReadLine();//1行読込
                    cells = line.Split(',');//読み込んだ行をカンマで分割して1セル毎の値を取得
                                            //DataGridViewに列を追加
                    for (int i = 0; i < cells.Length; i++)
                    {
                        dt.Columns.Add("列" + (i).ToString());
                    }
                    //DataTableの1行を作成
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < cells.Length; i++)
                    {
                        row[i] = cells[i];
                    }
                    //DataTableに作成した行を追加
                    dt.Rows.Add(row);

                    while (!sr.EndOfStream)//最後の行にくるまで処理を継続
                    {
                        //CSVの読み込み部分
                        line = sr.ReadLine();//1行読込
                        cells = line.Split(',');//読み込んだ行をカンマで分割して1セル毎の値を取得

                        //DataGridViewへの表示部分  
                        //DataTableの1行を作成
                        row = dt.NewRow();
                        //行の中のセルにCSVから読み込んだデータを入力
                        for (int i = 0; i < cells.Length; i++)
                        {
                            row[i] = cells[i];
                        }
                        //DataTableに作成した行を追加
                        dt.Rows.Add(row);
                    }
                    //DataGridViewにDataTableを入力
                    dataGridView1.DataSource = dt;

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DataGrifViewの最終行を取得
            //int cells = dataGridView1.BindingContext[dataGridView1.DataSource, dataGridView1.DataMember].Count;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if((string)(dataGridView1.Rows[i].Cells[0].Value) == "2007")
                {
                    dataGridView1.Rows[i].Cells[0].Value = "3007";
                }
           
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 dlg = new Form2(dataGridView1);
            dlg.Owner = this;
            dlg.ShowDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            //ChartにChartAreaを追加
            string chart_area1 = "Area1";
            chart1.ChartAreas.Add(chart_area1);
            //X軸の設定
            chart1.ChartAreas["Area1"].AxisX.IsMarginVisible = true;
            chart1.ChartAreas["Area1"].AxisX.Interval = 1;
            chart1.ChartAreas["Area1"].AxisX.Maximum = 2011;
            chart1.ChartAreas["Area1"].AxisX.Minimum =2007;

            //Y軸の設定
            chart1.ChartAreas["Area1"].AxisY.IsMarginVisible = true;
            chart1.ChartAreas["Area1"].AxisY.Interval = 50;
            chart1.ChartAreas["Area1"].AxisY.Maximum = 1850;
            chart1.ChartAreas["Area1"].AxisY.Minimum = 1250;

            //ChartにSeriesを追加します
            string legend1 = "Graph1";
            chart1.Series.Add(legend1);

            //プロットのサイズ指定
            chart1.Series[legend1].MarkerSize = 8;

            //プロットの色
            chart1.Series[legend1].Color = Color.Red;

            //プロットの形状
            chart1.Series[legend1].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            //グラフの種別を指定
            chart1.Series[legend1].ChartType = SeriesChartType.Point;


            for (var i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                double x1 = Convert.ToDouble(dataGridView1.Rows[i+1].Cells[0].Value);
                double y1 = Convert.ToDouble(dataGridView1.Rows[i+1].Cells[1].Value);

                DataPoint db = new DataPoint(x1, y1);

                chart1.Series[legend1].Points.Add(db);
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            //datagridview1と同じ行列数を追加
            dataGridView2.ColumnCount = dataGridView1.Columns.Count;
            dataGridView2.RowCount = dataGridView1.Rows.Count;


            dataGridView2.Rows[0].Cells[0].Value = dataGridView1.Rows[0].Cells[0].Value;
        }
    }
}
