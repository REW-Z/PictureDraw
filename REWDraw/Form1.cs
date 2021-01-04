using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REWDraw
{
    public partial class Form1 : Form
    {

        private List<ToolStripButton> tools = new List<ToolStripButton>();

        public Brush brush;

        private bool isDrawing = false;

        public Form1()
        {
            InitializeComponent();
        }

        ///重置颜色
        private void ResetButtonColors()
        {
            foreach (var item in tools)
            {
                item.BackColor = Color.Transparent;
            }
        }

        ///Draw函数
        private void Draw(int posX, int posY)
        {
            Bitmap img = new Bitmap(panelMain.BackgroundImage);

            for (int x = -brush.size; x < brush.size; x++)
            {
                for (int y = -brush.size; y < brush.size; y++)
                {
                    if(posX + x < img.Width && posX + x >= 0 && posY + y < img.Height && posY + y >= 0)
                    {
                        float colorScale = ((float)(brush.size * brush.size) - (float)(x * x + y * y))
                        .Clamp(0f, (float)(brush.size * brush.size));

                        colorScale /= (float)(brush.size * brush.size);

                        colorScale = (float)Math.Pow(colorScale, (1f / brush.capacity));

                        Color colorScaled = Color.FromArgb(
                            ((int)Math.Round(brush.color.A * colorScale)).Clamp(0, 255),
                            ((int)Math.Round(brush.color.R * colorScale)).Clamp(0, 255),
                            ((int)Math.Round(brush.color.G * colorScale)).Clamp(0, 255),
                            ((int)Math.Round(brush.color.B * colorScale)).Clamp(0, 255)
                            );

                        Color colorSource = img.GetPixel(posX + x, posY + y);
                        Color colorMix = Color.FromArgb(
                            (colorSource.A + colorScaled.A).Clamp(0, 255),
                            (colorSource.R + colorScaled.R).Clamp(0, 255),
                            (colorSource.G + colorScaled.G).Clamp(0, 255),
                            (colorSource.B + colorScaled.B).Clamp(0, 255)
                            );

                        img.SetPixel(posX + x, posY + y, colorMix);

                    }

                }
            }
            panelMain.BackgroundImage = img;
        }

        /// <summary>
        /// 载入窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //背景
            panelMain.BackgroundImage = new Bitmap(panelMain.Width, panelMain.Height);

            //工具栏
            tools.Add(toolStripButtonPencil);
            tools.Add(toolStripButtonBrush);
            ResetButtonColors();

            //笔刷初始化
            brush = new Brush() { color = Color.Black, capacity = 1f, size = 10 };
        }


        /// <summary>
        /// 铅笔工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonPencil_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            brush = new Brush() { color = Color.Black, size = 1, capacity = 999f};
            toolStripButtonPencil.BackColor = Color.Black;
        }

        /// <summary>
        /// 刷子工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonBrush_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            brush = new Brush() { color = Color.Black, size = 10, capacity = 1f };
            toolStripButtonBrush.BackColor = Color.Black;
        }


        /// <summary>
        /// 绘制事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelMain_MouseDown(object sender, MouseEventArgs e)
        {
            Draw(e.X, e.Y);
            isDrawing = true;
        }

        private void panelMain_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }

        private void panelMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                Draw(e.X, e.Y);
            }
        }
    }
}
