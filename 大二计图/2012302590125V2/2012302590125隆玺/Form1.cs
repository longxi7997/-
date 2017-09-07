using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;

namespace _2012302590125隆玺
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       //..............................
        public int MenuID, PressNum, FirstX, FirstY,R,XL,XR,YU,YD;
        static double A,B,C;
        Point[] pointsgroup = new Point[4];//创建一个有4个点的点数组
        Point[] group = new Point[100];//创建一个能放100个点的点数组
        Point3D[] modegroup = new Point3D[32];//创建一个有8个点的三维点数组

        public struct EdgeInfo
        {
            int ymax, ymin;//Y的上下端点
            float k, xmin;//斜率倒数和X的下端点
            //为四个内部变量设置的公共变量，方便外界存取数据
            public int YMax { get { return ymax; } set { ymax = value; } }
            public int YMin { get { return ymin; } set { ymin = value; } }
            public float XMin { get { return xmin; } set { xmin = value; } }
            public float K { get { return k; } set { k = value; } }
            //构造函数，这里用来初始化结构变量
            public EdgeInfo(int x1, int y1, int x2, int y2)//(x1,y1):下端点；(x2,y2):上端点
            {
                ymax = y2; ymin = y1; xmin = (float)x1; k = (float)(x1 - x2) / (float)(y1 - y2); //p = -1;
            }
        }

      //.....................
        private void Exit_Click_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DDALine_Click(object sender, EventArgs e)
        {
            MenuID = 1; PressNum = 0;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (MenuID==1||MenuID==2||MenuID==3)
            {
                if (PressNum==0)
                {
                    FirstX = e.X;
                    FirstY = e.Y;
                }
                else
                {
                    if(MenuID==1)
                    DDALine1(FirstX, FirstY, e.X, e.Y);
                    if (MenuID==2)
                    {
                        MidLine1(FirstX, FirstY, e.X, e.Y);
                    }
                    if (MenuID==3)
                    {
                        BresenhamLine1(FirstX, FirstY, e.X, e.Y);
                    }
                    
                }
                PressNum++;
                if (PressNum>=2)
                {
                    PressNum = 0;
                }
            }
            if (MenuID==5)
            {
                if (PressNum==0)
                {
                    FirstX = e.X; FirstY = e.Y;
                }
                else
                {
                    if (FirstX==e.X&&FirstY ==e.Y)
                    {
                        return;
                    }
                    if (MenuID==5)
                    {
                        BresenhamCircle1(FirstX, FirstY, e.X, e.Y);
                    }
                    
                }
                PressNum++;
                if (PressNum >= 2) PressNum = 0;
               
            }
            if (MenuID==4)
            {
                if (PressNum == 0)
                {
                    FirstX = e.X; FirstY = e.Y;
                }
                else
                {
                    if (FirstX == e.X && FirstY == e.Y)
                    {
                        return;
                    }
                    if (MenuID == 4)
                    {
                        MidCircle1(FirstX, FirstY, e.X, e.Y);
                    }

                }
                PressNum++;
                if (PressNum >= 2) PressNum = 0;
            }
            if (MenuID == 24||MenuID == 31)//扫描线填充
            {
                Graphics g = CreateGraphics();//创建图形设备
                if (e.Button == MouseButtons.Left)//如果按左键，存顶点
                {
                    group[PressNum].X = e.X;
                    group[PressNum].Y = e.Y;
                    if (PressNum > 0)//依次画多边形边
                    {
                        g.DrawLine(Pens.Red, group[PressNum - 1], group[PressNum]);
                    }
                    PressNum++;//这里，PressNum记录了多边形顶点数
                }
                if (e.Button == MouseButtons.Right)//如果按右键，结束顶点采集，开始填充
                {
                    g.DrawLine(Pens.Red, group[PressNum - 1], group[0]);//最后一条边
                    if (MenuID == 31)   //扫描线填充
                        ScanLineFill1();
                    if (MenuID == 24)   //窗口裁剪
                        WindowCut1();
                }
            }
            if (MenuID == 21 || MenuID == 22 || MenuID == 23)
            {
                if (PressNum == 0)//保留第一点
                {
                   FirstX = e.X; FirstY = e.Y; PressNum++;
                }
                else//第二点，调用裁剪算法
                {
                    if (MenuID == 21)      //CohenCut裁剪
                        CohenCut1(FirstX, FirstY, e.X, e.Y);
                    if (MenuID == 22)      //中点裁剪
                        MidCut1(FirstX, FirstY, e.X, e.Y);
                    if (MenuID == 23)      //梁友栋裁剪                        
                        LiangCut1(FirstX, FirstY, e.X, e.Y);

                    PressNum = 0;  //清零，为下一次裁剪做准备
                }
            }

            if (MenuID == 11)//平移
            {
                if (PressNum == 0)//保留
                {
                    FirstX = e.X; FirstY = e.Y;
                }
                else//第二点，确定平移量，改变图形参数
                {
                    for (int i = 0; i < 4; i++)
                    {
                        pointsgroup[i].X += e.X - FirstX;
                        pointsgroup[i].Y += e.Y - FirstY;
                    }
                    Graphics g = CreateGraphics();//创建图形设备
                    g.DrawPolygon(Pens.Blue, pointsgroup);
                }
                PressNum++;
                if (PressNum >= 2) PressNum = 0;//完毕，清零，为下一次做准备
            }

            if (MenuID == 12)//旋转
            {
                if (PressNum == 0)//保留第一点
                {
                    FirstX = e.X; FirstY = e.Y;
                }
                else//第二点，确定旋转角度，改变图形参数
                {
                    double a;
                    //排除两点重合的异常
                    if (e.X == FirstX && e.Y == FirstY)
                        return;
                    //排除分母为零的异常
                    if (e.X == FirstX && e.Y > FirstY)
                        a = 3.1415926 / 2.0;
                    else if (e.X == FirstX && e.Y < FirstY)
                        a = 3.1415926 / 2.0 * 3.0;
                    else
                        //计算旋转弧度
                        a = Math.Atan((double)(e.Y - FirstY) / (double)(e.X - FirstX));
                    //弧度转化为角度
                    a = a / 3.1415926 * 180.0;
                    int x0 = 150, y0 = 150;//指定旋转中心
                    Matrix myMatrix = new Matrix();
                    //创建平移量为(-x0, -y0)的平移矩阵，
                    myMatrix.Translate(-x0, -y0);
                    //右乘一个旋转量为角度a的旋转矩阵
                    myMatrix.Rotate((float)a, MatrixOrder.Append);
                    //右乘一个平移量为(x0, y0)的平移矩阵
                    myMatrix.Translate(x0, y0, MatrixOrder.Append);

                    Graphics g = CreateGraphics();//创建图形设备
                    g.Transform = myMatrix;//用复合矩阵变换图形设备
                    g.DrawPolygon(Pens.Blue, pointsgroup);//显示旋转结果
                }
                PressNum++;
                if (PressNum >= 2) PressNum = 0;//完毕，清零，为下一次做准备
            }

            if (MenuID == 14 || MenuID == 15)//对称变换
            {
                if (PressNum == 0)//保留
                {
                    FirstX = e.X; FirstY = e.Y;
                }
                else//第二点
                {
                    Graphics g = CreateGraphics();
                    g.DrawLine(Pens.CadetBlue, FirstX, FirstY, e.X, e.Y);//画对称变换基线
                    if (MenuID == 14)
                        TransSymmetry1(FirstX, FirstY, e.X, e.Y);
                    if (MenuID == 15)
                        TransShear1(FirstX, FirstY, e.X, e.Y);

                }
                PressNum++;
                if (PressNum >= 2) PressNum = 0;//完毕，清零，为下一次做准备
            }

            
        }
        private void DDALine1(int x0, int y0, int x1, int y1)
        {
            int x, flag;
            float m, y;
            Graphics g = CreateGraphics();
            if (x0==1&&y0==y1)
            if(x0==x1)
            {
                if (y0>y1)
                {
                    x = y0; y0 = y1; y1 = x;
                }
                for (x = y0; x <= y1;x++ )
                {
                    g.DrawRectangle(Pens.Red, x1, x, 1, 1);
                }
                return;
            }
            if (y0==y1)
            {
                if (x0>x1)
                {
                    x = x0; x0 = x1; x1 = x;
                }
                for (x = x0; x <= x1;x++ )
                {
                    g.DrawRectangle(Pens.Red, x, y0, 1, 1);
                }
                return;
            }
            if (x0>x1)
            {
                x = x0; x0 = x1; x1 = x;
                x = y0; y0 = y1; y1 = x;
            }
            flag = 0;
            if (x1 - x0 > y1 - y0 && y1 - y0 > 0)
            {
                flag = 1;
            }
            if (x1 - x0 > y0 - y1 && y0 - y1 > 0)
            {
                flag = 2; y0 = -y0; y1 = -y1;
            }
            if (y1 - y0 > x1 - x0)
            {
                flag = 3; x = x0; x0 = y0; y0 = x; x = x1; x1 = y1; y1 = x;
            }
            if (y0 - y1 > x1 - x0)
            {
                flag = 4; x = x0; x0 = -y0; y0 = x; x = x1; x1 = -y1; y1 = x;
            }

            m = (float)(y1 - y0) / (float)(x1 - x0);
            for (x = x0, y = (float)y0; x <= x1; x++, y = y + m)
            {
                if (flag == 1) g.DrawRectangle(Pens.Red, x, (int)(y + 0.5), 1, 1);
                if (flag == 2) g.DrawRectangle(Pens.Red, x, -(int)(y + 0.5), 1, 1);
                if (flag == 3) g.DrawRectangle(Pens.Red, (int)(y + 0.5), x, 1, 1);
                if (flag == 4) g.DrawRectangle(Pens.Red, (int)(y + 0.5), -x, 1, 1);

            }
        }

        private void MidLine_Click(object sender, EventArgs e)
        {
            MenuID = 2; PressNum = 0;
        }

        private void MidLine1(int x0, int y0, int x1, int y1)
        {
            int x, y, d, flag;
            Graphics g = CreateGraphics();
            if (x0 == x1 && y0 == y1) return;
            if(x0==x1)
            {
                if (y0>y1)
                {
                    x = y0; y0 = y1; y1 = x;
                }
                for (x = y0; x <= y1;x++ )
                {
                    g.DrawRectangle(Pens.Red, x1, x, 1, 1);
                }
                return;
            }
            if (y0==y1)
            {
                if (x0>x1)
                {
                    x = x0; x0 = x1; x1 = x;
                }
                for (x = x0; x <= x1;x++ )
                {
                    g.DrawRectangle(Pens.Red, x, y0, 1, 1);
                }
                return;
            }
            if (x0>x1)
            {
                x = x0; x0 = x1; x1 = x;
                x = y0; y0 = y1; y1 = x;
            }
            flag = 0;
            if (x1 - x0 > y1 - y0 && y1 - y0 > 0) flag = 1;
            if(x1-x0>y0-y1&&y0-y1>0)
            {
                flag = 2;
                y0 = -y0; y1 = -y1;
            }
            if (y1 - y0 > x1 - x0)
            {
                flag = 3; 
                x = x0; x0 = y0; y0 = x; x = x1; x1 = y1; y1 = x;
            }
            if (y0 - y1 > x1 - x0)
            {
                flag = 4;
                x = x0; x0 = -y0; y0 = x; x = x1; x1 = -y1; y1 = x;
            }
            x = x0; y = y0; d = (x1 - x0) - 2 * (y1 - y0);
            while (x<x1+1)
            {
                if (flag == 1) g.DrawRectangle(Pens.Red, x, y, 1, 1);
                if (flag == 2) g.DrawRectangle(Pens.Red, x, -y, 1, 1);
                if (flag == 3) g.DrawRectangle(Pens.Red, y, x, 1, 1);
                if (flag == 4) g.DrawRectangle(Pens.Red, y, -x, 1, 1);
                x++;
                if (d>0)
                {
                    d = d - 2 * (y1 - y0);
                }
                else
                {
                    y++; d = d - 2 * ((y1 - y0) - (x1 - x0));
                }

            }
        }

        private void BresenhamLine1(int x0, int y0, int x1, int y1)
        {
            int x, y,d,flag;
            
            Graphics g = CreateGraphics();
            if (x0 == x1 && y0 == y1) return;
            if (x0 == x1)
            {
                if (y0 > y1)
                {
                    x = y0; y0 = y1; y1 = x;
                }
                for (x = y0; x <= y1; x++)
                {
                    g.DrawRectangle(Pens.Red, x1, x, 1, 1);
                }
                return;
            }
            if (y0 == y1)
            {
                if (x0 > x1)
                {
                    x = x0; x0 = x1; x1 = x;
                }
                for (x = x0; x <= x1; x++)
                {
                    g.DrawRectangle(Pens.Red, x, y0, 1, 1);
                }
                return;
            }
            if (x0 > x1)
            {
                x = x0; x0 = x1; x1 = x;
                x = y0; y0 = y1; y1 = x;
            }
            flag = 0;
            if (x1 - x0 > y1 - y0 && y1 - y0 > 0) flag = 1;
            if (x1 - x0 > y0 - y1 && y0 - y1 > 0)
            {
                flag = 2;
                y0 = -y0; y1 = -y1;
            }
            if (y1 - y0 > x1 - x0)
            {
                flag = 3;
                x = x0; x0 = y0; y0 = x; x = x1; x1 = y1; y1 = x;
            }
            if (y0 - y1 > x1 - x0)
            {
                flag = 4;
                x = x0; x0 = -y0; y0 = x; x = x1; x1 = -y1; y1 = x;
            }
            x = x0; y = y0; d=x0-x1;
            while (x < x1 + 1)
            {
                if (flag == 1) g.DrawRectangle(Pens.Red, x, y, 1, 1);
                if (flag == 2) g.DrawRectangle(Pens.Red, x, -y, 1, 1);
                if (flag == 3) g.DrawRectangle(Pens.Red, y, x, 1, 1);
                if (flag == 4) g.DrawRectangle(Pens.Red, y, -x, 1, 1);
                x++;
                d = d + 2 * (y1 - y0);
                if (d >=0)
                {
                    y++;
                    d=d-2*(x1-x0);
                }
               

            }
        }

        private void BresenhamCircle_Click(object sender, EventArgs e)
        {
            MenuID = 5; PressNum = 0;
        }

        private void BresenhamCircle1(int x0, int y0, int x1, int y1)
        {
            int r, d, x, y;
            Graphics g = CreateGraphics();//创建图形设备
            r = (int)(Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0)) + 0.5);
            x = 0; y = r; d = 3 - 2 * r;
            while (x < y || x == y)
            {
                g.DrawRectangle(Pens.Blue, x + x0, y + y0, 1, 1);
                g.DrawRectangle(Pens.Red, -x + x0, y + y0, 1, 1);
                g.DrawRectangle(Pens.Green, x + x0, -y + y0, 1, 1);
                g.DrawRectangle(Pens.Yellow, -x + x0, -y + y0, 1, 1);
                g.DrawRectangle(Pens.Black, y + x0, x + y0, 1, 1);
                g.DrawRectangle(Pens.Red, -y + x0, x + y0, 1, 1);
                g.DrawRectangle(Pens.Red, y + x0, -x + y0, 1, 1);
                g.DrawRectangle(Pens.Red, -y + x0, -x + y0, 1, 1);
                x = x + 1;
                if (d < 0 || d == 0)
                {
                    d = d + 4 * x + 6;
                }
                else
                {
                    y = y - 1; d = d + 4 * (x - y) + 10;
                }
            };

        }
        private void MidCircle1(int x0, int y0, int x1, int y1)
        {
            int r, d, x, y;
            Graphics g = CreateGraphics();//创建图形设备
            r = (int)(Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0)) + 0.5);
            x = 0; y = r; d = 5 / 4 - r;
            while (x < y || x == y)
            {
                g.DrawRectangle(Pens.Blue, x + x0, y + y0, 1, 1);
                g.DrawRectangle(Pens.Red, -x + x0, y + y0, 1, 1);
                g.DrawRectangle(Pens.Green, x + x0, -y + y0, 1, 1);
                g.DrawRectangle(Pens.Yellow, -x + x0, -y + y0, 1, 1);
                g.DrawRectangle(Pens.Black, y + x0, x + y0, 1, 1);
                g.DrawRectangle(Pens.Red, -y + x0, x + y0, 1, 1);
                g.DrawRectangle(Pens.Red, y + x0, -x + y0, 1, 1);
                g.DrawRectangle(Pens.Red, -y + x0, -x + y0, 1, 1);
                x = x + 1;
                if (d < 0 || d == 0)
                {
                    d = d + 2 * x + 3;
                }
                else
                {
                    y=y-1; d = d + 2 * (x - y) + 5;
                }
            };

        }

        private void ScanLineFill1()
        {
            EdgeInfo[] edgelist = new EdgeInfo[100];//建立边结构数组
            int j = 0, yu = 0, yd = 1024;//活化边的扫描范围从yd到yu
            group[PressNum] = group[0];//将第一点复制为数组最后一点
            for (int i = 0; i < PressNum; i++)//建立每一条边的边结构
            {
                if (group[i].Y > yu) yu = group[i].Y;//找出图形最高点
                if (group[i].Y < yd) yd = group[i].Y; //找出图形最低点

                if (group[i].Y != group[i + 1].Y)//只处理非水平边
                {
                    if (group[i].Y > group[i + 1].Y)//下端点在前，上端点在后
                    {
                        edgelist[j++] = new EdgeInfo(group[i + 1].X, group[i + 1].Y, group[i].X, group[i].Y);
                    }
                    else
                    {
                        edgelist[j++] = new EdgeInfo(group[i].X, group[i].Y, group[i + 1].X, group[i + 1].Y);
                    }
                }
            }
            Graphics g = CreateGraphics();//创建图形设备
            for (int y = yd; y < yu; y++)
            {
                //AEL表操作
                var sorted =               //定义存放选择结果的集合
                    from item in edgelist //从edgelist中选
                    where y < item.YMax && y >= item.YMin //选择条件
                    orderby item.XMin, item.K  //集合元素排序条件
                    select item;              //开始选
                int flag = 0;
                foreach (var item in sorted)//两两配对，画线
                {
                    if (flag == 0)//第一点，不画
                    {
                        FirstX = (int)(item.XMin + 0.5); flag++;
                    }
                    else//第二点，画
                    {
                        g.DrawLine(Pens.Blue, (int)(item.XMin + 0.5), y, FirstX - 1, y);
                        flag = 0;
                    }
                }
                for (int i = 0; i < j; i++)//将dx加到x上
                {
                    if (y < edgelist[i].YMax - 1 && y > edgelist[i].YMin)
                    {
                        edgelist[i].XMin += edgelist[i].K;
                    }
                }


            }

            PressNum = 0;
        }

        private void CohenCut1(int x1, int y1, int x2, int y2)
        {
            int code1 = 0, code2 = 0, code, x = 0, y = 0;
            Graphics g = CreateGraphics();//创建图形设备
            g.DrawLine(Pens.Red, x1, y1, x2, y2);//画原始线段
            code1 = encode(x1, y1);//对线段端点编码
            code2 = encode(x2, y2);
            while (code1 != 0 || code2 != 0)
            {
                if ((code1 & code2) != 0) return;//完全不可见，
                code = code1;
                if (code1 == 0) code = code2;
                if ((1 & code) != 0)//求线段与窗口左边的交点:0001=1
                {
                    x = XL;
                    y = y1 + (y2 - y1) * (x - x1) / (x2 - x1);
                }
                else if ((2 & code) != 0) //求线段与窗口右边的交点:0010=2
                {
                    x = XR;
                    y = y1 + (y2 - y1) * (x - x1) / (x2 - x1);
                }
                else if ((4 & code) != 0) //求线段与窗口底边的交点:0100=4
                {
                    y = YD;
                    x = x1 + (x2 - x1) * (y - y1) / (y2 - y1);
                }
                else if ((8 & code) != 0) //求线段与窗口顶边的交点:01000=8
                {
                    y = YU;
                    x = x1 + (x2 - x1) * (y - y1) / (y2 - y1);
                }
                if (code == code1)
                {
                    x1 = x; y1 = y; code1 = encode(x, y);
                }
                else
                {
                    x2 = x; y2 = y; code2 = encode(x, y);
                }
            }
            Pen MyPen = new Pen(Color.Yellow, 3);//创建一支粗笔
            g.DrawLine(MyPen, x1, y1, x2, y2);//画裁剪线段
        }
        private void MidCut1(int x1, int y1, int x2, int y2)
        {
            Graphics g = CreateGraphics();//创建图形设备
            g.DrawLine(Pens.Red, x1, y1, x2, y2);//画要裁剪的线段
            Point p1, p2;

            if (LineIsOutOfWindow(x1, y1, x2, y2))//如果现在就可以确定线段完全不可见，结束。
                return;
            p1 = FindNearestPoint(x1, y1, x2, y2);//从（X1，Y1）出发，寻找最近可见点
            if (PointIsOutOfWindow(p1.X, p1.Y))    //找到的“可见点”不可见，结束。
                return;
            p2 = FindNearestPoint(x2, y2, x1, y1);//交换

            Pen MyPen = new Pen(Color.Yellow, 3);
            g.DrawLine(MyPen, p1, p2);  //画裁剪后的线段
        }
        private void LiangCut1(int x1, int y1, int x2, int y2)
        {                      //规定（x1,y1）为起点
            Graphics g = CreateGraphics();//创建图形设备
            g.DrawLine(Pens.Red, x1, y1, x2, y2);

            float tsx, tsy, tex, tey;//设置两个始边、两个终边对应T参数
            if (x1 == x2)  //垂线
            {
                tsx = 0; tex = 1;
            }
            else if (x1 < x2)
            {   // 条件满足，X方向的始边、终边随即确立，可直接计算对应参数
                tsx = (float)(XL - x1) / (float)(x2 - x1);
                tex = (float)(XR - x1) / (float)(x2 - x1);
            }
            else
            {
                tsx = (float)(XR - x1) / (float)(x2 - x1);
                tex = (float)(XL - x1) / (float)(x2 - x1);
            }
            if (y1 == y2)  //水平线
            {
                tsy = 0; tey = 1;
            }
            else if (y1 < y2)
            {   // 条件满足，Y方向的始边、终边随即确立，可直接计算对应参数
                tsy = (float)(YD - y1) / (float)(y2 - y1);
                tey = (float)(YU - y1) / (float)(y2 - y1);
            }
            else
            {
                tsy = (float)(YU - y1) / (float)(y2 - y1);
                tey = (float)(YD - y1) / (float)(y2 - y1);
            }
            tsx = Math.Max(tsx, tsy);   //系统提供的函数只能比较两个数
            tsx = Math.Max(tsx, 0);     //用两次，从3个数中选出最大的
            tex = Math.Min(tex, tey);
            tex = Math.Min(tex, 1);
            if (tsx < tex)     //该条件满足，才是可见的
            {
                int xx1, yy1, xx2, yy2;
                xx1 = (int)(x1 + (x2 - x1) * tsx);
                yy1 = (int)(y1 + (y2 - y1) * tsx);
                xx2 = (int)(x1 + (x2 - x1) * tex);
                yy2 = (int)(y1 + (y2 - y1) * tex);
                Pen MyPen = new Pen(Color.Yellow, 3);
                g.DrawLine(MyPen, xx1, yy1, xx2, yy2);
            }
        }
        private void WindowCut1()//多边形和裁剪结果都存放在group数组中
        {
            group[PressNum] = group[0];//将第一点复制为数组最后一点
            EdgeClipping(0);		//用第一条窗口边进行裁剪
            EdgeClipping(1);		//用第二条窗口边进行裁剪
            EdgeClipping(2);		//用第三条窗口边进行裁剪
            EdgeClipping(3);		//用第四条窗口边进行裁剪

            Graphics g = CreateGraphics();//创建图形设备
            Pen MyPen = new Pen(Color.Yellow, 3);
            for (int i = 0; i < PressNum; i++)//  绘制裁剪多边形
                g.DrawLine(MyPen, group[i], group[i + 1]);
            PressNum = 0;

        }





        private void EdgeClipping(int linecode)
        {
            float x, y;
            int n, i, number1;
            Point[] q = new Point[200];//创建点数组存放裁剪结果

            number1 = 0;
            if (linecode == 0)// x=XL 用窗口左边来裁剪多边形
            {
                for (n = 0; n < PressNum; n++)
                {
                    if (group[n].X < XL && group[n + 1].X < XL)//外外，不输出
                    {
                    }
                    if (group[n].X >= XL && group[n + 1].X >= XL)//内内，输出后点
                    {
                        q[number1++] = group[n + 1];
                    }
                    if (group[n].X >= XL && group[n + 1].X < XL)//内外，输出交点
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XL - group[n].X);
                        q[number1].X = XL;
                        q[number1++].Y = (int)y;
                    }
                    if (group[n].X < XL && group[n + 1].X >= XL)//外内，输出交点、后点
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XL - group[n].X);
                        q[number1].X = XL;
                        q[number1++].Y = (int)y;
                        q[number1++] = group[n + 1];
                    }
                }

                for (i = 0; i < number1; i++)//裁剪结果存入group数组
                {
                    group[i] = q[i];
                }
                group[number1] = q[0];
                PressNum = number1;
            }
            if (linecode == 1)//y=YU   用窗口顶边来裁剪多边形
            {
                for (n = 0; n < PressNum; n++)
                {
                    if (group[n].Y >= YU && group[n + 1].Y >= YU)//外外，不输出
                    {
                    }
                    if (group[n].Y < YU && group[n + 1].Y < YU)//内内，输出后点
                    {
                        q[number1++] = group[n + 1];
                    }
                    if (group[n].Y < YU && group[n + 1].Y >= YU)//内外，输出交点
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YU - group[n].Y);
                        q[number1].X = (int)x;
                        q[number1++].Y = YU;
                    }
                    if (group[n].Y >= YU && group[n + 1].Y < YU)//外内，输出交点、后点
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YU - group[n].Y);
                        q[number1].X = (int)x;
                        q[number1++].Y = YU;
                        q[number1++] = group[n + 1];
                    }
                }
                for (i = 0; i < number1; i++)
                {
                    group[i] = q[i];
                }
                group[number1] = q[0];
                PressNum = number1;
            }
            if (linecode == 2)//x=XR  用窗口右边来裁剪多边形
            {
                for (n = 0; n < PressNum; n++)
                {
                    if (group[n].X >= XR && group[n + 1].X >= XR)//外外，不输出
                    {
                    }
                    if (group[n].X < XR && group[n + 1].X < XR)//内内，输出后点
                    {
                        q[number1++] = group[n + 1];
                    }
                    if (group[n].X < XR && group[n + 1].X >= XR)//内外，输出交点
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XR - group[n].X);
                        q[number1].X = XR;
                        q[number1++].Y = (int)y;
                    }
                    if (group[n].X >= XR && group[n + 1].X < XR)//外内，输出交点、后点
                    {
                        y = group[n].Y + (float)(group[n + 1].Y - group[n].Y) / (float)(group[n + 1].X - group[n].X) * (float)(XR - group[n].X);
                        q[number1].X = XR;
                        q[number1++].Y = (int)y;
                        q[number1++] = group[n + 1];
                    }
                }
                for (i = 0; i < number1; i++)
                {
                    group[i] = q[i];
                }
                group[number1] = q[0];
                PressNum = number1;
            }
            if (linecode == 3)// y=YD   用窗口底边来裁剪多边形
            {
                for (n = 0; n < PressNum; n++)
                {
                    if (group[n].Y < YD && group[n + 1].Y < YD)//外外，不输出
                    {
                    }
                    if (group[n].Y >= YD && group[n + 1].Y >= YD)//内内，输出后点
                    {
                        q[number1++] = group[n + 1];
                    }
                    if (group[n].Y >= YD && group[n + 1].Y < YD)//内外，输出交点
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YD - group[n].Y);
                        q[number1].X = (int)x;
                        q[number1++].Y = YD;
                    }
                    if (group[n].Y < YD && group[n + 1].Y >= YD)//外内，输出交点、后点
                    {
                        x = group[n].X + (float)(group[n + 1].X - group[n].X) / (float)(group[n + 1].Y - group[n].Y) * (float)(YD - group[n].Y);
                        q[number1].X = (int)x;
                        q[number1++].Y = YD;
                        q[number1++] = group[n + 1];
                    }
                }
                for (i = 0; i < number1; i++)
                {
                    group[i] = q[i];
                }
                group[number1] = q[0];
                PressNum = number1;
            }
        }


        private Point FindNearestPoint(int x1, int y1, int x2, int y2)
        {                            //(x1,y1)是起始端点，(x2,y2)是终点
            int x = 0, y = 0;
            Point p = new Point(0, 0);
            if (!PointIsOutOfWindow(x1, y1))//如果起点可见，直接返回起点
            {
                p.X = x1;
                p.Y = y1;
                return p;
            }
            while (!(Math.Abs(x1 - x2) <= 1 && Math.Abs(y1 - y2) <= 1))
            {       //判断是否起、终点足够靠近
                x = (x1 + x2) / 2; y = (y1 + y2) / 2;
                if (LineIsOutOfWindow(x1, y1, x, y))
                {
                    x1 = x; y1 = y;//在外，起始点移到中点
                }
                else
                {
                    x2 = x; y2 = y;//不在外，终点移到中点
                }
            }
            if (PointIsOutOfWindow(x1, y1))
            {
                p.X = x2; p.Y = y2;//起始点在外，返回终点
            }
            else
            {
                p.X = x1; p.Y = y1;//否则，返回起始点
            }
            return p;
        }

        private bool LineIsOutOfWindow(int x1, int y1, int x2, int y2)
        {
            if (x1 < XL && x2 < XL)
                return true;
            else if (x1 > XR && x2 > XR)
                return true;
            else if (y1 > YU && y2 > YU)
                return true;
            else if (y1 < YD && y2 < YD)
                return true;
            else
                return false;
        }
        private bool PointIsOutOfWindow(int x, int y)
        {
            if (x < XL)
                return true;
            else if (x > XR)
                return true;
            else if (y > YU)
                return true;
            else if (y < YD)
                return true;
            else
                return false;
        }

        private int encode(int x, int y)
        {
            int code = 0;//编码位规定：YU-YD-XR-XL
            if (x >= XL && x <= XR && y >= YD && y <= YU) code = 0;//窗口区域：0000;
            if (x < XL && y >= YD && y <= YU) code = 1;    // 窗口左区域：0001;
            if (x > XR && y >= YD && y <= YU) code = 2;   // 窗口右区域：0010;
            if (x >= XL && x <= XR && y > YU) code = 8;   // 窗口上区域：1000;
            if (x >= XL && x <= XR && y < YD) code = 4;  // 窗口下区域：0100;
            if (x <= XL && y > YU) code = 9;              // 窗口左上区域：1001;
            if (x >= XR && y > YU) code = 10;            // 窗口右上区域：1010;
            if (x <= XL && y < YD) code = 5;             // 窗口左下区域： 0101;
            if (x >= XR && y < YD) code = 6;            // 窗口右下区域： 0110;
            return code;
        }

//.....................图形变换

        private void TransSymmetry1(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2 && y1 == y2)//排除两点重合的情况
                return;
            double angle;
            if (x1 == x2 && y1 < y2)//特殊角
                angle = 3.1415926 / 2.0;
            else if (x1 == x2 && y1 > y2)//特殊角
                angle = 3.1415926 / 2.0 * 3.0;
            else
                angle = Math.Atan((double)(y2 - y1) / (double)(x2 - x1));
            angle = angle * 180.0 / 3.1415926; //将弧度转化为角度

            Matrix myMatrix = new Matrix();//建立矩阵变量，为复合矩阵计算做准备
            myMatrix.Translate(-x1, -y1);//根据缩放中心，建立平移矩阵
            myMatrix.Rotate(-(float)angle, MatrixOrder.Append);//右乘缩放矩阵
            Matrix MyM1 = new Matrix(1, 0, 0, -1, 0, 0);//创ä建对称变换矩阵
            myMatrix.Multiply(MyM1, MatrixOrder.Append);//右乘对称变换矩阵
            myMatrix.Rotate((float)angle, MatrixOrder.Append);//右乘缩放矩阵
            myMatrix.Translate(x1, y1, MatrixOrder.Append);//右乘平移矩阵

            Graphics g = CreateGraphics();//创建图形设备
            g.Transform = myMatrix;//用得到的复合矩阵对图形进行变换?
            g.DrawPolygon(Pens.Blue, pointsgroup);//画变换后的图形?
        }

        private void TransShear1(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2 && y1 == y2)//排除两点重合的情况
                return;
            double angle;
            if (x1 == x2 && y1 < y2)//特殊角
                angle = 3.1415926 / 2.0;
            else if (x1 == x2 && y1 > y2)//特殊角
                angle = 3.1415926 / 2.0 * 3.0;
            else
                angle = Math.Atan((double)(y2 - y1) / (double)(x2 - x1));
            angle = angle * 180.0 / 3.1415926; //将弧度转化为角度

            Matrix myMatrix = new Matrix();//建立矩阵变量，为复合矩阵计算做准备
            myMatrix.Translate(-x1, -y1);//根据第一点，建立平移矩阵
            myMatrix.Rotate(-(float)angle, MatrixOrder.Append);//右乘缩放矩阵
            myMatrix.Shear((float)1.0, 0, MatrixOrder.Append);//右乘错切变换矩阵
            myMatrix.Rotate((float)angle, MatrixOrder.Append);//右乘缩放矩阵
            myMatrix.Translate(x1, y1, MatrixOrder.Append);//右乘平移矩阵

            Graphics g = CreateGraphics();//创建图形设备
            g.Transform = myMatrix;//用得到的复合矩阵对图形进行变换?
            g.DrawPolygon(Pens.Blue, pointsgroup);//画变换后的图形?
        }
//..........................投影
        //。。。。。1

        private void Projection1()
        {   //模型数据存入数组
            modegroup[0] = new Point3D(100, 100, 100);//A
            modegroup[1] = new Point3D(200, 100, 100);//B
            modegroup[2] = new Point3D(200, 200, 100);//C   
            modegroup[3] = new Point3D(100, 200, 100);//D
            modegroup[4] = new Point3D(100, 100, 200);//A'
            modegroup[5] = new Point3D(200, 100, 200);//B'
            modegroup[6] = new Point3D(200, 200, 200);//C'
            modegroup[7] = new Point3D(100, 200, 200);//D'

            Point3D vp = new Point3D(50, 30, 500);
            if (MenuID == 42||MenuID == 43)
                vp = InputProjectionDirection();//设置投影方向

            for (int i = 0; i < 8; i++)//每个空间点逐一投影
            {
                if (MenuID == 41)
                    group[i] = SimpleP(modegroup[i]);
                if (MenuID == 42)
                {
                    group[i] = ParallelP(vp, modegroup[i]);
                }
                if (MenuID == 43)
                {
                    group[i] = PerspectiveP(vp, modegroup[i]);
                }


            }
            //按照模型拓扑关系联接投影点
            Graphics g = CreateGraphics();
            g.DrawLine(Pens.Red, group[0], group[1]);
            g.DrawLine(Pens.Red, group[1], group[2]);
            g.DrawLine(Pens.Red, group[2], group[3]);
            g.DrawLine(Pens.Red, group[3], group[0]);
            g.DrawLine(Pens.Red, group[4], group[5]);
            g.DrawLine(Pens.Red, group[5], group[6]);
            g.DrawLine(Pens.Red, group[6], group[7]);
            g.DrawLine(Pens.Red, group[7], group[4]);
            g.DrawLine(Pens.Red, group[0], group[4]);
            g.DrawLine(Pens.Red, group[1], group[5]);
            g.DrawLine(Pens.Red, group[2], group[6]);
            g.DrawLine(Pens.Red, group[3], group[7]);
        }


        private Point SimpleP(Point3D p3d)//投影函数
        {
            Point p;
            float kx, ky;
            kx = (float)0.4; ky = (float)0.3;
            p = new Point((int)(p3d.X - kx * p3d.Z + 0.5), (int)(p3d.Y - ky * p3d.Z + 0.5));
            return p;
        }

        private Point3D InputProjectionDirection()
        {
            Form MyInputForm = new Form();//创建窗口
            Label label1 = new Label();//创建窗口控件
            Label label2 = new Label();
            Label label3 = new Label();
            label1.Text = "X:";//设置控件属性
            label2.Text = "Y:";
            label3.Text = "Z:";
            label1.Location = new Point(10, 10);
            label2.Location = new Point(10, 40);
            label3.Location = new Point(10, 70);

            NumericUpDown numeric1 = new NumericUpDown();//创建窗口控件
            NumericUpDown numeric2 = new NumericUpDown();
            NumericUpDown numeric3 = new NumericUpDown();
            numeric1.Minimum = 0; numeric1.Maximum = 500;// 设置控件属性
            numeric1.Increment = 10; numeric1.Value = 150;
            numeric2.Minimum = 0; numeric2.Maximum = 500;
            numeric2.Increment = 10; numeric2.Value = 150;
            if (MenuID == 42)
            {
                numeric3.Minimum = -800; numeric3.Maximum = -300;
                numeric3.Increment = 10; numeric3.Value = -500;
            }
            else
            {    //为了保证视线方向向下，透视投影中Z值必须为高于立方体的正数
                numeric3.Minimum = 300; numeric3.Maximum = 800;
                numeric3.Increment = 10; numeric3.Value = 500;
            }

            
            numeric1.Location = new Point(30, 10);
            numeric2.Location = new Point(30, 40);
            numeric3.Location = new Point(30, 70);

            Button button1 = new Button();//创建窗口控件
            Button button2 = new Button();
            button1.Text = "确定";//设置控件属性
            button1.DialogResult = DialogResult.OK;
            button1.Location = new Point(10, 100);
            button2.Text = "取消";
            button2.DialogResult = DialogResult.Cancel;
            button2.Location = new Point(button1.Width + 20, 100);


            if (MenuID == 42)
            {
                MyInputForm.Text = "请输入透视投影方向向量";//设置窗口属性
            }
            else
                MyInputForm.Text = "请输入透视投影中心位置";


            MyInputForm.HelpButton = true;

            MyInputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            MyInputForm.MaximizeBox = false;
            MyInputForm.MinimizeBox = false;
            MyInputForm.AcceptButton = button1;
            MyInputForm.CancelButton = button2;
            MyInputForm.StartPosition = FormStartPosition.CenterScreen;

            MyInputForm.Controls.Add(numeric1);//向窗口添加控件
            MyInputForm.Controls.Add(numeric2);
            MyInputForm.Controls.Add(numeric3);
            MyInputForm.Controls.Add(label1);
            MyInputForm.Controls.Add(label2);
            MyInputForm.Controls.Add(label3);
            MyInputForm.Controls.Add(button1);
            MyInputForm.Controls.Add(button2);

            Point3D vp = new Point3D(150, 150, -500);//设置变量，用于接收窗口输入值
            if (MyInputForm.ShowDialog() == DialogResult.Cancel)//打开窗口
            {                     //如果选择的是“取消”，则关闭对话框，退出
                MyInputForm.Close();
                return vp;
            }
            vp = new Point3D((float)numeric1.Value, (float)numeric2.Value, (float)numeric3.Value);
            MyInputForm.Close();
            return vp;
        }


        private Point ParallelP(Point3D ViewP, Point3D ModeP)
        {
            Point p;
            int x, y;
            x = (int)(ModeP.X - ViewP.X / ViewP.Z * ModeP.Z + 0.5);
            y = (int)(ModeP.Y - ViewP.Y / ViewP.Z * ModeP.Z + 0.5);
            p = new Point(x, y);
            return p;
        }

        private Point PerspectiveP(Point3D ViewP, Point3D ModeP)
        {
            Point p;
            int x, y;
            x = (int)(ViewP.X + (ModeP.X - ViewP.X) * ViewP.Z / (ViewP.Z - ModeP.Z) + 0.5);
            y = (int)(ViewP.Y + (ModeP.Y - ViewP.Y) * ViewP.Z / (ViewP.Z - ModeP.Z) + 0.5);
            p = new Point(x, y);
            return p;
        }

        //.....................222
        private void Projection2()
        {   //模型数据存入数组
            modegroup[0] = new Point3D(100, 100, 0);//A
            modegroup[1] = new Point3D(200, 100, 0);//B
            modegroup[2] = new Point3D(200, 200, 0);//C
            modegroup[3] = new Point3D(100, 200, 0);//D
            modegroup[4] = new Point3D(100, 100, 200);//A'
            modegroup[5] = new Point3D(200, 100, 200);//B'
            modegroup[6] = new Point3D(200, 200, 200);//C'
            modegroup[7] = new Point3D(100, 200, 200);//D'

            modegroup[8] = new Point3D(300, 100, 0);//A
            modegroup[9] = new Point3D(400, 100, 0);//B
            modegroup[10] = new Point3D(400, 200, 0);//C
            modegroup[11] = new Point3D(300, 200, 0);//D
            modegroup[12] = new Point3D(300, 100, 200);//A'
            modegroup[13] = new Point3D(400, 100, 200);//B'
            modegroup[14] = new Point3D(400, 200, 200);//C'
            modegroup[15] = new Point3D(300, 200, 200);//D'

            modegroup[16] = new Point3D(100, 300, 0);//A
            modegroup[17] = new Point3D(200, 300, 0);//B
            modegroup[18] = new Point3D(200, 400, 0);//C
            modegroup[19] = new Point3D(100, 400, 0);//D
            modegroup[20] = new Point3D(100, 300, 200);//A'
            modegroup[21] = new Point3D(200, 300, 200);//B'
            modegroup[22] = new Point3D(200, 400, 200);//C'
            modegroup[23] = new Point3D(100, 400, 200);//D'

            modegroup[24] = new Point3D(300, 300, 0);//A
            modegroup[25] = new Point3D(400, 300, 0);//B
            modegroup[26] = new Point3D(400, 400, 0);//C
            modegroup[27] = new Point3D(300, 400, 0);//D
            modegroup[28] = new Point3D(300, 300, 200);//A'
            modegroup[29] = new Point3D(400, 300, 200);//B'
            modegroup[30] = new Point3D(400, 400, 200);//C'
            modegroup[31] = new Point3D(300, 400, 200);//D'

            Point3D vp1 = new Point3D(800, 250, 0);//初始漫游起点
            Point3D vp2 = new Point3D(400, 250, 0);//初始漫游终点
            Point3D vp3 = new Point3D(-800, 0, 20);//初始观测方向
            Point3D vp = new Point3D(800, 250, 0);//初始当前观测点?
            vp1 = InputProjectionDirection2(1);//设置漫游起点
            vp2 = InputProjectionDirection2(2);//设置漫游终点
            vp3 = InputProjectionDirection2(3);//设置观测方向
            for (double j = 0; j < 10; j++)
            {
                vp.X = (int)((1.0 - j / 10.0) * vp1.X + j / 10.0 * vp2.X);
                vp.Y = (int)((1.0 - j / 10.0) * vp1.Y + j / 10.0 * vp2.Y);
                vp.Z = (int)((1.0 - j / 10.0) * vp1.Z + j / 10.0 * vp2.Z);
                for (int i = 0; i < 32; i++)//每个空间点逐一投影
                {
                    group[i] = ScenceP(vp, vp3, modegroup[i]);
                }
                //按照模型拓扑关系连接投影点
                Graphics g = CreateGraphics();//创建图形设备
                g.Clear(Color.LightGray);
                g.DrawLine(Pens.Red, group[0], group[1]);
                g.DrawLine(Pens.Red, group[1], group[2]);
                g.DrawLine(Pens.Red, group[2], group[3]);
                g.DrawLine(Pens.Red, group[3], group[0]);
                g.DrawLine(Pens.Red, group[4], group[5]);
                g.DrawLine(Pens.Red, group[5], group[6]);
                g.DrawLine(Pens.Red, group[6], group[7]);
                g.DrawLine(Pens.Red, group[7], group[4]);
                g.DrawLine(Pens.Red, group[0], group[4]);
                g.DrawLine(Pens.Red, group[1], group[5]);
                g.DrawLine(Pens.Red, group[2], group[6]);
                g.DrawLine(Pens.Red, group[3], group[7]);

                g.DrawLine(Pens.Green, group[8], group[9]);
                g.DrawLine(Pens.Green, group[9], group[10]);
                g.DrawLine(Pens.Green, group[10], group[11]);
                g.DrawLine(Pens.Green, group[11], group[8]);
                g.DrawLine(Pens.Green, group[12], group[13]);
                g.DrawLine(Pens.Green, group[13], group[14]);
                g.DrawLine(Pens.Green, group[14], group[15]);
                g.DrawLine(Pens.Green, group[15], group[12]);
                g.DrawLine(Pens.Green, group[8], group[12]);
                g.DrawLine(Pens.Green, group[9], group[13]);
                g.DrawLine(Pens.Green, group[10], group[14]);
                g.DrawLine(Pens.Green, group[11], group[15]);

                g.DrawLine(Pens.Blue, group[16], group[17]);
                g.DrawLine(Pens.Blue, group[17], group[18]);
                g.DrawLine(Pens.Blue, group[18], group[19]);
                g.DrawLine(Pens.Blue, group[19], group[16]);
                g.DrawLine(Pens.Blue, group[20], group[21]);
                g.DrawLine(Pens.Blue, group[21], group[22]);
                g.DrawLine(Pens.Blue, group[22], group[23]);
                g.DrawLine(Pens.Blue, group[23], group[20]);
                g.DrawLine(Pens.Blue, group[16], group[20]);
                g.DrawLine(Pens.Blue, group[17], group[21]);
                g.DrawLine(Pens.Blue, group[18], group[22]);
                g.DrawLine(Pens.Blue, group[19], group[23]);

                g.DrawLine(Pens.Black, group[24], group[25]);
                g.DrawLine(Pens.Black, group[25], group[26]);
                g.DrawLine(Pens.Black, group[26], group[27]);
                g.DrawLine(Pens.Black, group[27], group[24]);
                g.DrawLine(Pens.Black, group[28], group[29]);
                g.DrawLine(Pens.Black, group[29], group[30]);
                g.DrawLine(Pens.Black, group[30], group[31]);
                g.DrawLine(Pens.Black, group[31], group[28]);
                g.DrawLine(Pens.Black, group[24], group[28]);
                g.DrawLine(Pens.Black, group[25], group[29]);
                g.DrawLine(Pens.Black, group[26], group[30]);
                g.DrawLine(Pens.Black, group[27], group[31]);

                System.Threading.Thread.Sleep(3000);// 暂停，观看投影结果
            }
        }

        private Point3D InputProjectionDirection2(int mode)
        {
            Form MyInputForm = new Form();//创建窗口
            Label label1 = new Label();//创建窗口控件
            Label label2 = new Label();
            Label label3 = new Label();
            label1.Text = "X:";//设置控件属性
            label2.Text = "Y:";
            label3.Text = "Z:";
            label1.Location = new Point(10, 10);
            label2.Location = new Point(10, 40);
            label3.Location = new Point(10, 70);

            NumericUpDown numeric1 = new NumericUpDown();//创建窗口控件
            NumericUpDown numeric2 = new NumericUpDown();
            NumericUpDown numeric3 = new NumericUpDown();
            Point3D vp = new Point3D(150, 150, -500);//设置变量，用于接收窗口输入值
            if (mode == 1)//“漫游起点”限制参数输入
            {
                numeric1.Minimum = 600; numeric1.Maximum = 800;
                numeric1.Increment = 10; numeric1.Value = 800;
                numeric2.Minimum = 210; numeric2.Maximum = 290;
                numeric2.Increment = 5; numeric2.Value = 250;
                numeric3.Minimum = 0; numeric3.Maximum = 500;
                numeric3.Increment = 10; numeric3.Value = 0;
            }
            if (mode == 2)//“漫游终点”限制参数输入
            {
                numeric1.Minimum = 400; numeric1.Maximum = 500;
                numeric1.Increment = 10; numeric1.Value = 400;
                numeric2.Minimum = 210; numeric2.Maximum = 290;
                numeric2.Increment = 5; numeric2.Value = 250;
                numeric3.Minimum = 0; numeric3.Maximum = 500;
                numeric3.Increment = 10; numeric3.Value = 0;
            }
            if (mode == 3)//“观测方向”限制参数输入
            {
                numeric1.Minimum = -900; numeric1.Maximum = -800;
                numeric1.Increment = 5; numeric1.Value = -800;
                numeric2.Minimum = -50; numeric2.Maximum = 50;
                numeric2.Increment = 10; numeric2.Value = 0;
                numeric3.Minimum = -100; numeric3.Maximum = 100;
                numeric3.Increment = 10; numeric3.Value = 20;
            }
            numeric1.Location = new Point(30, 10);
            numeric2.Location = new Point(30, 40);
            numeric3.Location = new Point(30, 70);

            Button button1 = new Button();//创建窗口控件
            Button button2 = new Button();
            button1.Text = "确定";//设置控件属性
            button1.DialogResult = DialogResult.OK;
            button1.Location = new Point(10, 100);
            button2.Text = "取消";
            button2.DialogResult = DialogResult.Cancel;
            button2.Location = new Point(button1.Width + 20, 100);

            if (mode == 1)
                MyInputForm.Text = "请输入漫游起点";//设置窗口属性
            if (mode == 2)
                MyInputForm.Text = "请输入漫游终点";//设置窗口属性
            if (mode == 3)
                MyInputForm.Text = "请输入观测方向";//设置窗口属性
            MyInputForm.HelpButton = true;
            MyInputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            MyInputForm.MaximizeBox = false;
            MyInputForm.MinimizeBox = false;
            MyInputForm.AcceptButton = button1;
            MyInputForm.CancelButton = button2;
            MyInputForm.StartPosition = FormStartPosition.CenterScreen;

            MyInputForm.Controls.Add(numeric1);//向窗口添加控件t
            MyInputForm.Controls.Add(numeric2);
            MyInputForm.Controls.Add(numeric3);
            MyInputForm.Controls.Add(label1);
            MyInputForm.Controls.Add(label2);
            MyInputForm.Controls.Add(label3);
            MyInputForm.Controls.Add(button1);
            MyInputForm.Controls.Add(button2);
            vp = new Point3D((float)numeric1.Value, (float)numeric2.Value, (float)numeric3.Value);

            if (MyInputForm.ShowDialog() == DialogResult.Cancel)//打开窗口
            {                     //如果选择的是“取消”则关闭对话框，退出
                MyInputForm.Close();
                return vp;
            }
            vp = new Point3D((float)numeric1.Value, (float)numeric2.Value, (float)numeric3.Value);
            MyInputForm.Close();
            return vp;
        }


        private Point ScenceP(Point3D ViewP, Point3D DirP, Point3D ModeP)
        {
            double a11, a12, a13, a21, a22, a23, a31, a32, a33;
            a11 = Math.Sqrt(DirP.X * DirP.X + DirP.Y * DirP.Y + DirP.Z * DirP.Z);
            a31 = DirP.X / a11; a32 = DirP.Y / a11; a33 = DirP.Z / a11;//确定Z'坐标轴
            a12 = Math.Sqrt(a31 * a31 / (a31 * a31 + a32 * a32));//确定X'坐标轴
            a11 = -Math.Sqrt(a32 * a32 / (a31 * a31 + a32 * a32));
            a13 = 0;
            a23 = a11 * a32 - a12 * a31;
            if (a23 < 0)//根据a23,确定a12,a11的符号
            {
                a23 = -a23; a12 = -a12; a11 = -a11;
            }
            a21 = a12 * a33;//确定Y'坐标轴
            a22 = -a11 * a33;
            Point p;
            double z, x, y;
            z = a31 * (ModeP.X - ViewP.X) + a32 * (ModeP.Y - ViewP.Y) + a33 * (ModeP.Z - ViewP.Z);
            //显示位置有所调整，以便于观察
            x = (100.0 * (a11 * (ModeP.X - ViewP.X) + a12 * (ModeP.Y - ViewP.Y)) / z) + 300;
            y = -(100.0 * (a21 * (ModeP.X - ViewP.X) + a22 * (ModeP.Y - ViewP.Y) + a23 * (ModeP.Z - ViewP.Z)) / z) + 500;//倒置Y'坐标。因为显示坐标原点在左上角，致使场景倒置
            p = new Point((int)x, (int)y);
            return p;
        }





//.........................
        private void BresenhamLine_Click(object sender, EventArgs e)
        {
            MenuID = 3; PressNum = 0;
        }

        private void MidCircle_Click(object sender, EventArgs e)
        {
            MenuID = 4; PressNum = 0;
        }

        private void ScanLineFill_Click(object sender, EventArgs e)
        {
            MenuID = 31; PressNum = 0;
        }

        private void CohenCut_Click(object sender, EventArgs e)
        {
            MenuID = 21; PressNum = 0;
            Graphics g = CreateGraphics();//创建裁剪窗口
            XL = 50; XR = 250; YD = 50; YU = 250;//窗口参数
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            g.DrawPolygon(Pens.Blue, pointsgroup);

        }


        private void MidCut_Click(object sender, EventArgs e)
        {
            MenuID = 22; PressNum = 0;
            Graphics g = CreateGraphics();//创建裁剪窗口
            XL = 100; XR = 300; YD = 100; YU = 300;
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            g.DrawPolygon(Pens.Blue, pointsgroup);

        }

        private void LiangCut_Click(object sender, EventArgs e)
        {
            MenuID = 23; PressNum = 0;
            Graphics g = CreateGraphics();//创建裁剪窗口
            XL = 150; XR = 350; YD = 150; YU = 350;
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            g.DrawPolygon(Pens.Blue, pointsgroup);

        }

        private void WindowsCut_Click(object sender, EventArgs e)
        {
            MenuID = 24; PressNum = 0;
            Graphics g = CreateGraphics();//创建裁剪窗口
            XL = 200; XR = 400; YD = 200; YU = 400;
            pointsgroup[0] = new Point(XL, YD);
            pointsgroup[1] = new Point(XR, YD);
            pointsgroup[2] = new Point(XR, YU);
            pointsgroup[3] = new Point(XL, YU);
            g.DrawPolygon(Pens.Blue, pointsgroup);


        }

        private void TransMove_Click(object sender, EventArgs e)
        {
            MenuID = 11; PressNum = 0;
            Graphics g = CreateGraphics();//创建图形设备
            pointsgroup[0] = new Point(100, 100);//设置变换图形
            pointsgroup[1] = new Point(200, 100);
            pointsgroup[2] = new Point(200, 200);
            pointsgroup[3] = new Point(100, 200);
            g.DrawPolygon(Pens.Red, pointsgroup);//显示图形

        }

        private void TransRotate_Click(object sender, EventArgs e)
        {
            MenuID = 12; PressNum = 0;
            Graphics g = CreateGraphics();//创建图形设备
            pointsgroup[0] = new Point(100, 100);
            pointsgroup[1] = new Point(200, 100);
            pointsgroup[2] = new Point(200, 200);
            pointsgroup[3] = new Point(100, 200);
            g.DrawPolygon(Pens.Red, pointsgroup);

        }

        private void TransScale_Click(object sender, EventArgs e)
        {
            MenuID = 13;
            float xs, ys;
            MyForm myf = new MyForm();  //打开建立的对话框，接收变换系数
            if (myf.ShowDialog() == DialogResult.Cancel)
            {
                myf.Close();  //如果选择的是“取消”，则关闭对话框，退出
                return;
            }
            xs = myf.Xscale;
            ys = myf.Yscale;
            myf.Close();

            Graphics g = CreateGraphics();//创建图形设备
            pointsgroup[0] = new Point(100, 100);//画原图形
            pointsgroup[1] = new Point(200, 100);
            pointsgroup[2] = new Point(200, 200);
            pointsgroup[3] = new Point(100, 200);
            g.DrawPolygon(Pens.Red, pointsgroup);//原图形存在于图形设备g中

            Matrix myMatrix = new Matrix();//建立矩阵变量，为计算复合矩阵做准备
            myMatrix.Translate(-100, -100);//根据缩放中心，建立平移矩阵
            myMatrix.Scale(xs, ys, MatrixOrder.Append);//右乘缩放矩阵
            myMatrix.Translate(100, 100, MatrixOrder.Append);//右乘平移矩阵
            g.Transform = myMatrix;//用得到的复合矩阵对图形进行变换
            g.DrawPolygon(Pens.Blue, pointsgroup);//画变换后的图形

        }

        private void TransSymmertry_Click(object sender, EventArgs e)
        {
            MenuID = 14; PressNum = 0;
            Graphics g = CreateGraphics();//创建图形设备
            pointsgroup[0] = new Point(100, 100);
            pointsgroup[1] = new Point(200, 100);
            pointsgroup[2] = new Point(200, 200);
            pointsgroup[3] = new Point(100, 200);
            g.DrawPolygon(Pens.Red, pointsgroup);

        }

        private void TransShear_Click(object sender, EventArgs e)
        {
            MenuID = 15; PressNum = 0;
            Graphics g = CreateGraphics();//创建图形设备
            pointsgroup[0] = new Point(100, 100);
            pointsgroup[1] = new Point(200, 100);
            pointsgroup[2] = new Point(200, 200);
            pointsgroup[3] = new Point(100, 200);
            g.DrawPolygon(Pens.Red, pointsgroup);

        }

        private void SimpleProjection_Click(object sender, EventArgs e)
        {
            MenuID = 41;
            Projection1();
        }

        private void ParrallelProjection_Click(object sender, EventArgs e)
        {
            MenuID = 42;
            Projection1();

        }

        private void PerspectiveProjection_Click(object sender, EventArgs e)
        {
            MenuID = 43;
            Projection1();

        }

        private void SceneProjection_Click(object sender, EventArgs e)
        {
            MenuID = 44;
            Projection2();
        }

        private void Terrain1_Click(object sender, EventArgs e)
        {
            MenuID = 51;
            Terrain11(); 

        }
        private void Terrain11()
        {
            int[,] DEM = new int[200, 200];//建立二维数组存放DEM数据
            DEM = ReadDEM();//读入高程数据
            int size = 3;//柱状体的底面积设置为size*size
            double ky = 0.4, kz = 0.3;//深度值对投影位置的影响比例系数
            Graphics g = CreateGraphics();//创建图形设备
            g.Clear(Color.LightGray);//清空绘图区
            int dy = (int)(ky * size + 0.5);//深度值对投影位置的影响值
            int dz = (int)(kz * size + 0.5);
            for (int i = 0; i < 200; i++)
                for (int j = 0; j < 200; j++)
                {
                    int y = (int)(j * size - i * size * ky);//Ky=0.4,Kz=0.3
                    int z = (int)(-i * size * kz);//柱状体基点为空间点（i,j,0）的投影点
                    DrawPixel(g, dy, dz, size, y, z, DEM[i, j]);//画高程值DEM[i,j]对应的柱状体
                }
        }

        private int[,] ReadDEM()
        {
            int[,] D = new int[200, 200];//建立数组存放DEM数据
            FileStream fs = new FileStream("DEM.dat", FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            for (int i = 0; i < 200; i++)
                for (int j = 0; j < 200; j++)
                    D[i, j] = r.ReadByte();
            return D;
        }
        private void DrawPixel(Graphics g, int dx, int dy, int size, int x, int y, int z)
        {
            x = x + 400;//X、Y方向适当偏移，以调整场景显示位置
            y = -y + 400;//y方向需要颠倒
            Point[] pts = new Point[4];
            pts[0].X = x - dx;
            pts[0].Y = y + dy;//y方向增量也需要颠倒，即y-dy变成y+dy
            pts[1].X = x - dx;
            pts[1].Y = y + dy - z;
            pts[2].X = x - dx + size;
            pts[2].Y = y + dy - z;
            pts[3].X = x - dx + size;
            pts[3].Y = y + dy;
            g.FillPolygon(Brushes.White, pts);
            g.DrawPolygon(Pens.Black, pts);
            pts[0].X = x;
            pts[0].Y = y - z;
            pts[1].X = x - dx;
            pts[1].Y = y + dy - z;
            pts[2].X = x - dx + size;
            pts[2].Y = y + dy - z;
            pts[3].X = x + size;
            pts[3].Y = y - z;
            g.FillPolygon(Brushes.White, pts);
            g.DrawPolygon(Pens.Black, pts);
            pts[0].X = x + size;
            pts[0].Y = y;
            pts[1].X = x;
            pts[1].Y = y - z;
            pts[2].X = x - dx + size;
            pts[2].Y = y + dy - z;
            pts[3].X = x - dx + size;
            pts[3].Y = y + dy;
            g.FillPolygon(Brushes.White, pts);
            g.DrawPolygon(Pens.Black, pts);
        }
       /*..............................................
        .................................................*/

        private void Terrain2_Click(object sender, EventArgs e)
        {
            MenuID = 52;
            Terrain21(); 

        }

        private void Terrain21()
        {
            int[,] DEM = new int[200, 200];//建立二维数组存放DEM数据
            DEM = ReadDEM();//读入高程数据
            int size = 3;//相邻两像素点的距离
            double[] A = new double[1280];//存储最大值，假设显示器水平分辨率不超过1280
            for (int i = 0; i < 1280; i++)
                A[i] = 0.0;//用最小值初始化
            double ky = 0.6, kz = 0.6;
            Graphics g = CreateGraphics();//创建图形设备
            g.Clear(Color.LightGray);
            int dy = 400, dz = 500;//调整图形显示位置
            for (int i = 199; i >= 0; i--)
            {
                for (int j = 0; j < 199; j++)//处理一行
                {
                    double y1 = j * size - i * size * ky;//（i,j,DEM[i,j]）的投影点
                    double z1 = DEM[i, j] - i * size * kz;
                    double y2 = (j + 1) * size - i * size * ky;//（i,j+1,DEM[i,j+1]）的投影点
                    double z2 = DEM[i, j + 1] - i * size * kz;
                    Dealwith(g, y1 + dy, z1 + dz, y2 + dy, z2 + dz, A);
                }
                for (int j = 198; j >= 0; j--)//处理一列
                {
                    double y1 = i * size - j * size * ky;//（j,i,DEM[j,i]）的投影点
                    double z1 = DEM[j, i] - j * size * kz;
                    double y2 = i * size - (j + 1) * size * ky;//（j+1,i,DEM[j+1,i]）的投影点
                    double z2 = DEM[j + 1, i] - (j + 1) * size * kz;
                    Dealwith(g, y1 + dy, z1 + dz, y2 + dy, z2 + dz, A);
                }
            }
            for (int i = 0; i < 200; i++)//绘制边框
            {
                double y1 = i * size - 199 * size * ky + dy;//行边框
                double z1 = DEM[199, i] - 199 * size * kz + dz;
                double z2 = -199 * size * kz + dz;
                z1 = 900 - z1; z2 = 900 - z2;
                g.DrawLine(Pens.Black, (int)y1, (int)z1, (int)y1, (int)z2);
                y1 = 199 * size - i * size * ky + dy;//列边框
                z1 = DEM[i, 199] - i * size * kz + dz;
                z2 = -i * size * kz + dz;
                z1 = 900 - z1; z2 = 900 - z2;
                g.DrawLine(Pens.Black, (int)y1, (int)z1, (int)y1, (int)z2);
            }
        }
        private void Dealwith(Graphics g, double x1, double y1, double x2, double y2, double[] A)
        {
            int flag = 0;//标识项，1：超出，0：不超出?
            int xsave1 = 0, xsave2, x;
            double ysave1 = 0, ysave2, y;
            if (x1 > x2)//确保x1<x2
            {
                double xx = x1; x1 = x2; x2 = xx;
                double yy = y1; y1 = y2; y2 = yy;
            }
            for (x = (int)x1 + 1; x < x2; x++)
            {
                y = (y2 - y1) / (x2 - x1) * ((double)x - x1) + y1;
                if (y > A[x])
                {
                    A[x] = y;
                    if (flag == 0)//线段超出起始点
                    {
                        xsave1 = x; ysave1 = y;
                        flag = 1;
                    }
                }
                else
                {
                    if (flag == 1)//线段超出部分结束点
                    {
                        xsave2 = x - 1; //计算超出部分结束点
                        ysave2 = (y2 - y1) / (x2 - x1) * ((double)(x - 1) - x1) + y1;
                        flag = 0;
                        ysave1 = 900 - ysave1; //向下的设备坐标系Y坐标反向
                        ysave2 = 900 - ysave2;//画出超出部分
                        g.DrawLine(Pens.Black, xsave1, (int)ysave1, xsave2, (int)ysave2);
                    }

                }
            }
            if (flag == 1)//直到本线段结束也未 被遮挡的处理方法
            {
                y = (y2 - y1) / (x2 - x1) * (x - x1) + y1;
                ysave1 = 900 - ysave1; y = 900 - y;
                g.DrawLine(Pens.Black, xsave1, (int)ysave1, x, (int)y);
            }
        }

        private void ZBuffer_Click(object sender, EventArgs e)
        {
            MenuID = 53;
            ReadPolygon();
            ZBufferDraw(); 
        }
        public struct My3DPolygon
        {
            public Color pcolor;
            public int number;
            public Point3D[] Points;
        }
        My3DPolygon[] PGroup = new My3DPolygon[6];
private void ReadPolygon()
        {
            PGroup[0].pcolor = Color.Red;
            PGroup[0].number = 4;
            PGroup[0].Points = new Point3D[4];
            PGroup[0].Points[0] = new Point3D(150, 130, 310);
            PGroup[0].Points[1] = new Point3D(340, 150, 540);
            PGroup[0].Points[2] = new Point3D(380, 410, 1100);
            PGroup[0].Points[3] = new Point3D(160, 380, 820);

            PGroup[1].pcolor = Color.Green;
            PGroup[1].number = 5;
            PGroup[1].Points = new Point3D[5];
            PGroup[1].Points[0] = new Point3D(160, 120, 440);
            PGroup[1].Points[1] = new Point3D(410, 110, 930);
            PGroup[1].Points[2] = new Point3D(520, 550, 1570);
            PGroup[1].Points[3] = new Point3D(350, 430, 1130);
            PGroup[1].Points[4] = new Point3D(170, 220, 560);
 
            PGroup[2].pcolor = Color.Blue;
            PGroup[2].number = 6;
            PGroup[2].Points = new Point3D[6];
            PGroup[2].Points[0] = new Point3D(600, 600, 1300);
            PGroup[2].Points[1] = new Point3D(770, 500, 1370);
            PGroup[2].Points[2] = new Point3D(960, 420, 1480);
            PGroup[2].Points[3] = new Point3D(830, 300, 1230);
            PGroup[2].Points[4] = new Point3D(720, 230, 1050);
            PGroup[2].Points[5] = new Point3D(650, 210, 960);
 
            PGroup[3].pcolor = Color.Yellow;
            PGroup[3].number = 6;
            PGroup[3].Points = new Point3D[6];
            PGroup[3].Points[0] = new Point3D(220, 200, 710);
            PGroup[3].Points[1] = new Point3D(350, 340, 1055);
            PGroup[3].Points[2] = new Point3D(590, 660, 1815);
            PGroup[3].Points[3] = new Point3D(300, 600, 1550);
            PGroup[3].Points[4] = new Point3D(180, 320, 930);
            PGroup[3].Points[5] = new Point3D(150, 220, 715);

            PGroup[4].pcolor = Color.SkyBlue;
            PGroup[4].number = 6;
            PGroup[4].Points = new Point3D[6];
            PGroup[4].Points[0] = new Point3D(600, 680, 2260);
            PGroup[4].Points[1] = new Point3D(300, 670, 1840);
            PGroup[4].Points[2] = new Point3D(200, 520, 1440);
            PGroup[4].Points[3] = new Point3D(190, 430, 1250);
            PGroup[4].Points[4] = new Point3D(380, 220, 1020);
            PGroup[4].Points[5] = new Point3D(540, 300, 1340);

            PGroup[5].pcolor = Color.Brown;
            PGroup[5].number = 6;
            PGroup[5].Points = new Point3D[6];
            PGroup[5].Points[0] = new Point3D(400, 670, 1470);
            PGroup[5].Points[1] = new Point3D(680, 610, 1690);
            PGroup[5].Points[2] = new Point3D(690, 340, 1430);
            PGroup[5].Points[3] = new Point3D(500, 210, 1110);
            PGroup[5].Points[4] = new Point3D(200, 180, 780);
            PGroup[5].Points[5] = new Point3D(400, 320, 1120);
        }
private void ZBufferDraw()
        {
            double[,] Z=new double[1000,800];
            for (int i = 0; i < 1000; i++)//初始化Z缓冲区
                for (int j = 0; j < 800; j++)
                    Z[i, j] = 0.0;
            for (int i = 0; i < 6; i++)
            {
                ZBufferPolygon(PGroup[i], Z);//画一个投影多边形
            }
        }
private void ZBufferPolygon(My3DPolygon PG, double[,] Z)
{
    if (!GetPolygen(PG))
    {
        return;
    }
    Point[] g1 = new Point[100];//用于存放投影后的二维多边形
    EdgeInfo[] edgelist = new EdgeInfo[100];
    int j = 0, yu = 0, yd = 1024;//活化边的扫描范围，从yd到yu
    for (int i = 0; i < PG.number; i++)
    {
        g1[i].X = (int)(PG.Points[i].X + 0.5);
        g1[i].Y = (int)(PG.Points[i].Y + 0.5);
    }
    g1[PG.number].X = (int)(PG.Points[0].X + 0.5);//将第一点复制为数组最后一点
    g1[PG.number].Y = (int)(PG.Points[0].Y + 0.5);

    for (int i = 0; i < PG.number; i++)//建立每一条边的边结构
    {
        if (g1[i].Y > yu) yu = g1[i].Y;//活化边的扫描范围从yd到yu
        if (g1[i].Y < yd) yd = g1[i].Y;
        if (g1[i].Y != g1[i + 1].Y)//只处理非水平边
        {
            if (g1[i].Y > g1[i + 1].Y)
            {
                edgelist[j++] = new EdgeInfo(g1[i + 1].X, g1[i + 1].Y, g1[i].X, g1[i].Y);
            }
            else
            {
                edgelist[j++] = new EdgeInfo(g1[i].X, g1[i].Y, g1[i + 1].X, g1[i + 1].Y);
            }
        }
    }
    Graphics g = CreateGraphics();//创建图形设备
    for (int y = yd; y < yu; y++)
    {
        var sorted =               //选出与当前扫描线相交的边结构¡排序
            from item in edgelist
            where y < item.YMax && y >= item.YMin
            orderby item.XMin, item.K
            select item;
        int flag = 0;
        foreach (var item in sorted)//两两配对，画线
        {
            if (flag == 0)
            {
                FirstX = (int)(item.XMin + 0.5); flag++;
            }
            else
            {
                Pen p = new Pen(PG.pcolor);//根据空间多边形属性设置填充颜色
                for (int x = FirstX; x < (int)(item.XMin + 0.5); x++)
                {
                    double z = A * x + B * y + C;//计算高程值
                    if (z > Z[x, y])
                    {
                        Z[x, y] = z; //高于，才画
                        g.DrawRectangle(p, x, y, 1, 1);
                    }
                }
                flag = 0;
            }
        }
        for (int i = 0; i < j; i++)//将dx加到x
        {
            if (y < edgelist[i].YMax - 1 && y > edgelist[i].YMin)
            {
                edgelist[i].XMin += edgelist[i].K;
            }
        }
    }
}

private bool GetPolygen(My3DPolygon PG)
{
    int d;
    int i = 0;
    do
    {
        d = (int)((PG.Points[i].X - PG.Points[i + 2].X) * (PG.Points[i].Y - PG.Points[i + 1].Y)
            - (PG.Points[i].X - PG.Points[i + 1].X) * (PG.Points[i].Y - PG.Points[i + 2].Y));
        i++;
    } while (d == 0 || i + 2 < PG.number);
    if (d != 0)
    {
        i--;
        A = ((PG.Points[i].Z - PG.Points[i + 2].Z) * (PG.Points[i].Y - PG.Points[i + 1].Y)
            - (PG.Points[i].Z - PG.Points[i + 1].Z) * (PG.Points[i].Y - PG.Points[i + 2].Y)) / d;
        B = ((PG.Points[i].Z - PG.Points[i + 1].Z) * (PG.Points[i].X - PG.Points[i + 2].X)
            - (PG.Points[i].Z - PG.Points[i + 2].Z) * (PG.Points[i].X - PG.Points[i + 1].X)) / d;
        C = PG.Points[i].Z - A * PG.Points[i].X - B * PG.Points[i].Y;
        return true;
    }
    else
        return false;
}


        }


}
