using org.mariuszgromada.math.mxparser;
using System;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            License.iConfirmNonCommercialUse("1");
        }

        public double stringToDouble(string s)
        {
            double result;
            if (!double.TryParse(
                s.Replace(",", "."),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            ))
            {
                throw new Exception();
            }
            return result;
        }

        private double calculate(double x)
        {
            Function formula = new Function("Ex(x,y,z) = 2 * cos(x - pi/6) / (0.5 + sin(y)^2) * (1 + z^2 / (3 - z^2/5))");
            var _x = new Argument($"x", x);
            var _y = new Argument($"y", -1.22);
            var _z = new Argument($"z", 3.5 * Math.Pow(10, -2));
            var expression = new Expression("Ex(x,y,z)", formula, _x, _y, _z);
            double result = expression.calculate();
            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "0.5";
            textBox2.Text = "100";
            chart1.Series[0].ToolTip = "x=#VALX, y=#VALY";
            chart1.ChartAreas[0].AxisX.Title = "x";
            chart1.ChartAreas[0].AxisY.Title = "y(x)";
            chart1.Series[0].LegendText = "y(x)";
        }

        private struct Result
        {
            public Result(double x, double value)
            {
                this.x = x;
                this.value = value;
            }

            public double x;
            public double value;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            double xStep;
            chart1.Series[0].Points.Clear(); // Удаляем старые точки
            try
            {
                xStep = stringToDouble(textBox1.Text.ToString());
            } catch (Exception)
            {
                MessageBox.Show("Неверное значение шага X");
                return;
            }

            int xCount;
            if (!int.TryParse(textBox2.Text.ToString(), out xCount))
            {
                MessageBox.Show("Неверное значение количества точек");
                return;
            }

            double initialX = -20;
            int counter = 0;
            Result[] data = new Result[xCount];
            while (counter < xCount)
            {
                double x = initialX + counter * xStep;
                double value = this.calculate(x);
                data[counter] = new Result(x, value);
                counter++;
            }

            for(int i = 0; i < data.Length; i++) 
            {
                Result point = data[i];
                chart1.Series[0].Points.AddXY(point.x, point.value);
            }

        }
    }
}