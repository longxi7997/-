namespace _2012302590125隆玺
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.基本图形生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DDALine = new System.Windows.Forms.ToolStripMenuItem();
            this.MidLine = new System.Windows.Forms.ToolStripMenuItem();
            this.BresenhamLine = new System.Windows.Forms.ToolStripMenuItem();
            this.MidCircle = new System.Windows.Forms.ToolStripMenuItem();
            this.BresenhamCircle = new System.Windows.Forms.ToolStripMenuItem();
            this.PlusOrMinusCircle = new System.Windows.Forms.ToolStripMenuItem();
            this.BezierCurve = new System.Windows.Forms.ToolStripMenuItem();
            this.BSplineCurve = new System.Windows.Forms.ToolStripMenuItem();
            this.HermitCurve = new System.Windows.Forms.ToolStripMenuItem();
            this.二维图形变换ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TransMove = new System.Windows.Forms.ToolStripMenuItem();
            this.TransRotate = new System.Windows.Forms.ToolStripMenuItem();
            this.TransScale = new System.Windows.Forms.ToolStripMenuItem();
            this.TransSymmertry = new System.Windows.Forms.ToolStripMenuItem();
            this.TransShear = new System.Windows.Forms.ToolStripMenuItem();
            this.二维图形剪裁ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CohenCut = new System.Windows.Forms.ToolStripMenuItem();
            this.MidCut = new System.Windows.Forms.ToolStripMenuItem();
            this.LiangCut = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowsCut = new System.Windows.Forms.ToolStripMenuItem();
            this.图形填充算法ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScanLineFill = new System.Windows.Forms.ToolStripMenuItem();
            this.投影ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SimpleProjection = new System.Windows.Forms.ToolStripMenuItem();
            this.ParrallelProjection = new System.Windows.Forms.ToolStripMenuItem();
            this.PerspectiveProjection = new System.Windows.Forms.ToolStripMenuItem();
            this.SceneProjection = new System.Windows.Forms.ToolStripMenuItem();
            this.消隐 = new System.Windows.Forms.ToolStripMenuItem();
            this.Terrain1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Terrain2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ZBuffer = new System.Windows.Forms.ToolStripMenuItem();
            this.Exit_Click = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.基本图形生成ToolStripMenuItem,
            this.二维图形变换ToolStripMenuItem,
            this.二维图形剪裁ToolStripMenuItem,
            this.图形填充算法ToolStripMenuItem,
            this.投影ToolStripMenuItem,
            this.消隐,
            this.Exit_Click});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(714, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 基本图形生成ToolStripMenuItem
            // 
            this.基本图形生成ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DDALine,
            this.MidLine,
            this.BresenhamLine,
            this.MidCircle,
            this.BresenhamCircle,
            this.PlusOrMinusCircle,
            this.BezierCurve,
            this.BSplineCurve,
            this.HermitCurve});
            this.基本图形生成ToolStripMenuItem.Name = "基本图形生成ToolStripMenuItem";
            this.基本图形生成ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.基本图形生成ToolStripMenuItem.Text = "基本图形生成";
            // 
            // DDALine
            // 
            this.DDALine.Name = "DDALine";
            this.DDALine.Size = new System.Drawing.Size(165, 22);
            this.DDALine.Text = "DDA直线";
            this.DDALine.Click += new System.EventHandler(this.DDALine_Click);
            // 
            // MidLine
            // 
            this.MidLine.Name = "MidLine";
            this.MidLine.Size = new System.Drawing.Size(165, 22);
            this.MidLine.Text = "中点直线";
            this.MidLine.Click += new System.EventHandler(this.MidLine_Click);
            // 
            // BresenhamLine
            // 
            this.BresenhamLine.Name = "BresenhamLine";
            this.BresenhamLine.Size = new System.Drawing.Size(165, 22);
            this.BresenhamLine.Text = "Bresenham直线";
            this.BresenhamLine.Click += new System.EventHandler(this.BresenhamLine_Click);
            // 
            // MidCircle
            // 
            this.MidCircle.Name = "MidCircle";
            this.MidCircle.Size = new System.Drawing.Size(165, 22);
            this.MidCircle.Text = "中点圆";
            this.MidCircle.Click += new System.EventHandler(this.MidCircle_Click);
            // 
            // BresenhamCircle
            // 
            this.BresenhamCircle.Name = "BresenhamCircle";
            this.BresenhamCircle.Size = new System.Drawing.Size(165, 22);
            this.BresenhamCircle.Text = "Bresenham圆";
            this.BresenhamCircle.Click += new System.EventHandler(this.BresenhamCircle_Click);
            // 
            // PlusOrMinusCircle
            // 
            this.PlusOrMinusCircle.Name = "PlusOrMinusCircle";
            this.PlusOrMinusCircle.Size = new System.Drawing.Size(165, 22);
            this.PlusOrMinusCircle.Text = "正负圆";
            // 
            // BezierCurve
            // 
            this.BezierCurve.Name = "BezierCurve";
            this.BezierCurve.Size = new System.Drawing.Size(165, 22);
            this.BezierCurve.Text = "Bezier曲线";
            // 
            // BSplineCurve
            // 
            this.BSplineCurve.Name = "BSplineCurve";
            this.BSplineCurve.Size = new System.Drawing.Size(165, 22);
            this.BSplineCurve.Text = "B样条曲线";
            // 
            // HermitCurve
            // 
            this.HermitCurve.Name = "HermitCurve";
            this.HermitCurve.Size = new System.Drawing.Size(165, 22);
            this.HermitCurve.Text = "Hermitt曲线";
            // 
            // 二维图形变换ToolStripMenuItem
            // 
            this.二维图形变换ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TransMove,
            this.TransRotate,
            this.TransScale,
            this.TransSymmertry,
            this.TransShear});
            this.二维图形变换ToolStripMenuItem.Name = "二维图形变换ToolStripMenuItem";
            this.二维图形变换ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.二维图形变换ToolStripMenuItem.Text = "二维图形变换";
            // 
            // TransMove
            // 
            this.TransMove.Name = "TransMove";
            this.TransMove.Size = new System.Drawing.Size(124, 22);
            this.TransMove.Text = "图形平移";
            this.TransMove.Click += new System.EventHandler(this.TransMove_Click);
            // 
            // TransRotate
            // 
            this.TransRotate.Name = "TransRotate";
            this.TransRotate.Size = new System.Drawing.Size(124, 22);
            this.TransRotate.Text = "图形旋转";
            this.TransRotate.Click += new System.EventHandler(this.TransRotate_Click);
            // 
            // TransScale
            // 
            this.TransScale.Name = "TransScale";
            this.TransScale.Size = new System.Drawing.Size(124, 22);
            this.TransScale.Text = "图形缩放";
            this.TransScale.Click += new System.EventHandler(this.TransScale_Click);
            // 
            // TransSymmertry
            // 
            this.TransSymmertry.Name = "TransSymmertry";
            this.TransSymmertry.Size = new System.Drawing.Size(124, 22);
            this.TransSymmertry.Text = "对称变换";
            this.TransSymmertry.Click += new System.EventHandler(this.TransSymmertry_Click);
            // 
            // TransShear
            // 
            this.TransShear.Name = "TransShear";
            this.TransShear.Size = new System.Drawing.Size(124, 22);
            this.TransShear.Text = "错切变换";
            this.TransShear.Click += new System.EventHandler(this.TransShear_Click);
            // 
            // 二维图形剪裁ToolStripMenuItem
            // 
            this.二维图形剪裁ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CohenCut,
            this.MidCut,
            this.LiangCut,
            this.WindowsCut});
            this.二维图形剪裁ToolStripMenuItem.Name = "二维图形剪裁ToolStripMenuItem";
            this.二维图形剪裁ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.二维图形剪裁ToolStripMenuItem.Text = "二维图形剪裁";
            // 
            // CohenCut
            // 
            this.CohenCut.Name = "CohenCut";
            this.CohenCut.Size = new System.Drawing.Size(184, 22);
            this.CohenCut.Text = "Cohen算法";
            this.CohenCut.Click += new System.EventHandler(this.CohenCut_Click);
            // 
            // MidCut
            // 
            this.MidCut.Name = "MidCut";
            this.MidCut.Size = new System.Drawing.Size(184, 22);
            this.MidCut.Text = "中点分割算法";
            this.MidCut.Click += new System.EventHandler(this.MidCut_Click);
            // 
            // LiangCut
            // 
            this.LiangCut.Name = "LiangCut";
            this.LiangCut.Size = new System.Drawing.Size(184, 22);
            this.LiangCut.Text = "梁友栋算法";
            this.LiangCut.Click += new System.EventHandler(this.LiangCut_Click);
            // 
            // WindowsCut
            // 
            this.WindowsCut.Name = "WindowsCut";
            this.WindowsCut.Size = new System.Drawing.Size(184, 22);
            this.WindowsCut.Text = "窗口对多边形的裁剪";
            this.WindowsCut.Click += new System.EventHandler(this.WindowsCut_Click);
            // 
            // 图形填充算法ToolStripMenuItem
            // 
            this.图形填充算法ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScanLineFill});
            this.图形填充算法ToolStripMenuItem.Name = "图形填充算法ToolStripMenuItem";
            this.图形填充算法ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.图形填充算法ToolStripMenuItem.Text = "图形填充算法";
            // 
            // ScanLineFill
            // 
            this.ScanLineFill.Name = "ScanLineFill";
            this.ScanLineFill.Size = new System.Drawing.Size(160, 22);
            this.ScanLineFill.Text = "扫描线填充算法";
            this.ScanLineFill.Click += new System.EventHandler(this.ScanLineFill_Click);
            // 
            // 投影ToolStripMenuItem
            // 
            this.投影ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SimpleProjection,
            this.ParrallelProjection,
            this.PerspectiveProjection,
            this.SceneProjection});
            this.投影ToolStripMenuItem.Name = "投影ToolStripMenuItem";
            this.投影ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.投影ToolStripMenuItem.Text = "投影";
            // 
            // SimpleProjection
            // 
            this.SimpleProjection.Name = "SimpleProjection";
            this.SimpleProjection.Size = new System.Drawing.Size(124, 22);
            this.SimpleProjection.Text = "简单投影";
            this.SimpleProjection.Click += new System.EventHandler(this.SimpleProjection_Click);
            // 
            // ParrallelProjection
            // 
            this.ParrallelProjection.Name = "ParrallelProjection";
            this.ParrallelProjection.Size = new System.Drawing.Size(124, 22);
            this.ParrallelProjection.Text = "平行投影";
            this.ParrallelProjection.Click += new System.EventHandler(this.ParrallelProjection_Click);
            // 
            // PerspectiveProjection
            // 
            this.PerspectiveProjection.Name = "PerspectiveProjection";
            this.PerspectiveProjection.Size = new System.Drawing.Size(124, 22);
            this.PerspectiveProjection.Text = "透视投影";
            this.PerspectiveProjection.Click += new System.EventHandler(this.PerspectiveProjection_Click);
            // 
            // SceneProjection
            // 
            this.SceneProjection.Name = "SceneProjection";
            this.SceneProjection.Size = new System.Drawing.Size(124, 22);
            this.SceneProjection.Text = "场景漫游";
            this.SceneProjection.Click += new System.EventHandler(this.SceneProjection_Click);
            // 
            // 消隐
            // 
            this.消隐.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Terrain1,
            this.Terrain2,
            this.ZBuffer});
            this.消隐.Name = "消隐";
            this.消隐.Size = new System.Drawing.Size(44, 21);
            this.消隐.Text = "消隐";
            // 
            // Terrain1
            // 
            this.Terrain1.Name = "Terrain1";
            this.Terrain1.Size = new System.Drawing.Size(152, 22);
            this.Terrain1.Text = "地图显示1";
            this.Terrain1.Click += new System.EventHandler(this.Terrain1_Click);
            // 
            // Terrain2
            // 
            this.Terrain2.Name = "Terrain2";
            this.Terrain2.Size = new System.Drawing.Size(152, 22);
            this.Terrain2.Text = "地图显示2";
            this.Terrain2.Click += new System.EventHandler(this.Terrain2_Click);
            // 
            // ZBuffer
            // 
            this.ZBuffer.Name = "ZBuffer";
            this.ZBuffer.Size = new System.Drawing.Size(152, 22);
            this.ZBuffer.Text = "Z缓冲区算法";
            this.ZBuffer.Click += new System.EventHandler(this.ZBuffer_Click);
            // 
            // Exit_Click
            // 
            this.Exit_Click.Name = "Exit_Click";
            this.Exit_Click.Size = new System.Drawing.Size(44, 21);
            this.Exit_Click.Text = "退出";
            this.Exit_Click.Click += new System.EventHandler(this.Exit_Click_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 238);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "计算机图形学练习平台";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 基本图形生成ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DDALine;
        private System.Windows.Forms.ToolStripMenuItem MidLine;
        private System.Windows.Forms.ToolStripMenuItem BresenhamLine;
        private System.Windows.Forms.ToolStripMenuItem MidCircle;
        private System.Windows.Forms.ToolStripMenuItem BresenhamCircle;
        private System.Windows.Forms.ToolStripMenuItem 二维图形变换ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PlusOrMinusCircle;
        private System.Windows.Forms.ToolStripMenuItem BezierCurve;
        private System.Windows.Forms.ToolStripMenuItem BSplineCurve;
        private System.Windows.Forms.ToolStripMenuItem HermitCurve;
        private System.Windows.Forms.ToolStripMenuItem 二维图形剪裁ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图形填充算法ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 投影ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 消隐;
        private System.Windows.Forms.ToolStripMenuItem Exit_Click;
        private System.Windows.Forms.ToolStripMenuItem ScanLineFill;
        private System.Windows.Forms.ToolStripMenuItem CohenCut;
        private System.Windows.Forms.ToolStripMenuItem MidCut;
        private System.Windows.Forms.ToolStripMenuItem LiangCut;
        private System.Windows.Forms.ToolStripMenuItem WindowsCut;
        private System.Windows.Forms.ToolStripMenuItem TransMove;
        private System.Windows.Forms.ToolStripMenuItem TransRotate;
        private System.Windows.Forms.ToolStripMenuItem TransScale;
        private System.Windows.Forms.ToolStripMenuItem TransSymmertry;
        private System.Windows.Forms.ToolStripMenuItem TransShear;
        private System.Windows.Forms.ToolStripMenuItem SimpleProjection;
        private System.Windows.Forms.ToolStripMenuItem ParrallelProjection;
        private System.Windows.Forms.ToolStripMenuItem PerspectiveProjection;
        private System.Windows.Forms.ToolStripMenuItem SceneProjection;
        private System.Windows.Forms.ToolStripMenuItem Terrain1;
        private System.Windows.Forms.ToolStripMenuItem Terrain2;
        private System.Windows.Forms.ToolStripMenuItem ZBuffer;
    }
}

