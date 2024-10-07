using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.OleDb;

namespace 센서값따로받기
{
    public partial class 도깨비고비 : Form
    {

        SerialPort sPort;
        private static double xCount = 24;
        List<sensorData> myData = new List<sensorData>();   
        public 도깨비고비()
        {
            InitializeComponent();
            chartsetting();
        }

        private void chartsetting()
        {
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add("draw");
            chart1.ChartAreas["draw"].AxisX.Minimum = 0;
            chart1.ChartAreas["draw"].AxisX.Maximum = xCount;
            chart1.ChartAreas["draw"].AxisX.Interval = 2;
            chart1.ChartAreas["draw"].AxisX.MajorGrid.LineColor = Color.White;
            chart1.ChartAreas["draw"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas["draw"].AxisY.Minimum = 0;
            chart1.ChartAreas["draw"].AxisY.Maximum = 1023;
            chart1.ChartAreas["draw"].AxisY.Interval = 200;
            chart1.ChartAreas["draw"].AxisY.MajorGrid.LineColor = Color.White;  
            chart1.ChartAreas["draw"].AxisY.MajorGrid.LineDashStyle= ChartDashStyle.Dash;

            chart1.ChartAreas["draw"].BackColor = Color.Navy;

            chart1.ChartAreas["draw"].CursorX.AutoScroll = true;
            chart1.ChartAreas["draw"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart1.ChartAreas["draw"].AxisX.ScrollBar.ButtonColor = Color.LightSteelBlue;

            chart1.Series.Clear();
            chart1.Series.Add("s[0]");
            chart1.Series["s[0]"].ChartType = SeriesChartType.Line;
            chart1.Series["s[0]"].Color = Color.Yellow;
            chart1.Series["s[0]"].BorderWidth = 3;
            if (chart1.Legends.Count > 0)
                chart1.Legends.RemoveAt(0);
        }

        private void button1_Click(object sender, EventArgs e)
        { //나가기 
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            sPort = new SerialPort(cb.SelectedItem.ToString());
            sPort.Open();
            sPort.DataReceived += sPort_DataReceived;
            //serialPort1.PortName = "COM3";
            //serialPort1.DataReceived += sPort_DataReceived;
            
        }

        private void sPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sPort.IsOpen)
            {
                string d = sPort.ReadLine();
                string d1 = sPort.ReadLine();
                //listBox1.Invoke(new MethodInvoker(delegate { listBox1.Items.Add(d); }));
                int.TryParse(d, out int s);
                int.TryParse(d1, out int s1);
                //stBox1.Invoke(new MethodInvoker(delegate { listBox1.Items.Add(s); }));
                /* //byte[] buffer8 = new byte[8];
                //int totalRead = 0;
                int readCnt = 0;
                while (totalRead < 8)
                {
                    readCnt = sPort.Read(buffer8, totalRead, 8 - totalRead);
                    totalRead += readCnt;
                }
                Int32[] soil = new int[2];
                soil[0] = BitConverter.ToInt32(buffer8, 0);
                soil[1] = BitConverter.ToInt32(buffer8, 4);
                string s = sPort.ReadLine();
               */
                this.BeginInvoke((new Action(delegate { showValue(s); })));
                
                //this.BeginInvoke((new Action(delegate { showValue(soil); })));
                
            }
        }

       

        private void showValue(int s)
        {
            
            string moisture;
            double sv1 = 100 - ((s - 300) / 723) * 100;

            if(sv1 < 0 || sv1 > 100)
            {
                moisture = "error!";
            }
            else
            {
                moisture = sv1.ToString("0.0") + "%";
            }


            sensorData data = new sensorData(
                DateTime.Now.ToShortDateString(),
                DateTime.Now.ToString("HH:mm:ss"), Convert.ToInt32(moisture)); 

                myData.Add(data); 
            string item = DateTime.Now.ToString() + '\t' + s + '\t' + moisture ;
            listBox1.Items.Add(item);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;


            chart1.Series["s[0]"].Points.Add(s);
            chart1.ChartAreas["draw"].AxisX.Minimum = 0;
            chart1.ChartAreas["draw"].AxisX.Maximum = (myData.Count >= xCount) ? myData.Count : xCount;
        }

        private void 도깨비고비_Load(object sender, EventArgs e)
        {
            //InitializeComponent();
            //chartsetting();
            comboBox1.Items.Add("COM3");
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        { // 연결 버튼 
            //sPort.PortName = "COM3";
            sPort = new SerialPort("COM3");
            if (sPort.IsOpen)
            {

                //이미 오픈이 되어있으면...
                //아무것도 안함

            }
            else
            {
                //연결이 안되어있으면 연결한다.
                sPort.Open();
                sPort.DataReceived += sPort_DataReceived;
            }
            //serialPort1.Open();
        }

        private void button3_Click(object sender, EventArgs e)
        { //정지 
            if (sPort.IsOpen)
            {
                //이미 오픈이 되어있으면...
                //종료함
                sPort.Close();
            }
        }
    }
}
