// 2012302590125View.cpp : implementation of the CMy2012302590125View class
//

#include "stdafx.h"
#include "2012302590125.h"

#include "2012302590125Doc.h"
#include "2012302590125View.h"
#include "DrawCharDIG1.h"



#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

extern CStatusBar    m_wndStatusBar;
/////////////////////////////////////////////////////////////////////////////
// CMy2012302590125View



IMPLEMENT_DYNCREATE(CMy2012302590125View, CView)

BEGIN_MESSAGE_MAP(CMy2012302590125View, CView)
//{{AFX_MSG_MAP(CMy2012302590125View)
ON_WM_LBUTTONDOWN()
ON_WM_MOUSEMOVE()
ON_WM_RBUTTONDOWN()
ON_COMMAND(ID_DRAW_DDALINE, OnDrawDdaline)
ON_COMMAND(ID_DRAW_BCIRCLE, OnDrawBcircle)
ON_COMMAND(ID_DRAW_PNCIRCLE, OnDrawPNcircle)
ON_COMMAND(ID_CURVE_BEZIER, OnCurveBezier)
	ON_WM_LBUTTONDBLCLK()
	ON_COMMAND(ID_DRAW_CHAR, OnDrawChar)
	ON_COMMAND(ID_TRANS_MOVE, OnTransMove)
	ON_COMMAND(ID_DRAW_MIDLINE, OnDrawMidline)
	ON_COMMAND(ID_TRANS_SYMMETRY, OnTransSymmetry)
	ON_COMMAND(ID_FILL_SEED, OnFillSeed)
	ON_COMMAND(ID_EDGE_FILL, OnEdgeFill)
	ON_COMMAND(ID_FILL_SCANLINE, OnFillScanline)
	ON_COMMAND(ID_CUT_CS, OnCutCs)
	ON_COMMAND(ID_CUT_POLYGON, OnCutPolygon)
	ON_COMMAND(ID_ON_CUT_CIRCLE, OnOnCutCircle)
	ON_COMMAND(ID_CUT_MIDDLE, OnCutMiddle)
	ON_COMMAND(ID_CUT_LIANG, OnCutLiang)
	ON_COMMAND(ID_TRANS_ROTATE, OnTransRotate)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CMy2012302590125View construction/destruction

CMy2012302590125View::CMy2012302590125View()
{
	// TODO: add construction code here
	
}

CMy2012302590125View::~CMy2012302590125View()
{
}

BOOL CMy2012302590125View::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs
	
	return CView::PreCreateWindow(cs);
}

/////////////////////////////////////////////////////////////////////////////
// CMy2012302590125View drawing

void CMy2012302590125View::OnDraw(CDC* pDC)
{
	CMy2012302590125Doc* pDoc = GetDocument();
	ASSERT_VALID(pDoc);
	// TODO: add draw code for native data here
}

/////////////////////////////////////////////////////////////////////////////
// CMy2012302590125View diagnostics

#ifdef _DEBUG
void CMy2012302590125View::AssertValid() const
{
	CView::AssertValid();
}

void CMy2012302590125View::Dump(CDumpContext& dc) const
{
	CView::Dump(dc);
}

CMy2012302590125Doc* CMy2012302590125View::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CMy2012302590125Doc)));
	return (CMy2012302590125Doc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CMy2012302590125View message handlers

void CMy2012302590125View::OnLButtonDown(UINT nFlags, CPoint point) 
{
	// TODO: Add your message handler code here and/or call default
	
	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针 

	int r;
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDC.DPtoLP(&point); 
	pDC.SetROP2(R2_NOT); // 设置异或方式
	
	
	if(MenuID==1||MenuID ==12) { // DDA 直线
		if(PressNum==0){	//第一次按键将第一点保留在文档类数组中
			pDoc->group[PressNum]=point;
			PressNum++;
			mPointOrign=point; 
			mPointOld=point; //记录第一点
			SetCapture(); 
		} 
		else if(PressNum==1){	//第二次按键保留第二点，用文档类画线
			
			pDC.MoveTo(mPointOrign);   
			pDC.LineTo(mPointOld);//擦旧线 
			
			pDoc->group[PressNum]=point;
			PressNum=0;//程序画图 
			if(MenuID == 1)
			pDoc->DDALine(&pDC); 

			else if(MenuID==12)
				pDoc->MidLine(&pDC);
			
			ReleaseCapture(); 
		} 
	} 
	
	
	if(MenuID==3|| MenuID==4||MenuID==33) { // Bresenham 圆 
		if(PressNum==0){//第一次按键将第一点保留在 mPointOrign 
			pDoc->group[PressNum]=point; 
			PressNum++; 
			mPointOrign=point; 
			mPointOld=point; //记录第一点 
			SetCapture(); 
		} 
		else if(PressNum==1&&MenuID==3){//第二次按键调用文档类画圆程序画图 
			
			pDC.SelectStockObject(NULL_BRUSH);//画空心圆
			r=(int)sqrt((mPointOrign.x-mPointOld.x)*(mPointOrign.x-mPointOld.x) 
				+(mPointOrign.y-mPointOld.y)*(mPointOrign.y-mPointOld.y)); 
			pDC.Ellipse(mPointOrign.x-r,mPointOrign.y-r,mPointOrign.x+r, 
				mPointOrign.y+r);//擦旧圆 
			
			PressNum=0; 
			pDoc->BCircle(&pDC,mPointOrign,point);ReleaseCapture();
		}
		else if(PressNum==1&&MenuID==4){//第二次按键调用画圆程序画图
			
			pDC.SelectStockObject(NULL_BRUSH);//画空心圆
			r=(int)sqrt((mPointOrign.x-mPointOld.x)*(mPointOrign.x-mPointOld.x) 
				+(mPointOrign.y-mPointOld.y)*(mPointOrign.y-mPointOld.y)); 
			pDC.Ellipse(mPointOrign.x-r,mPointOrign.y-r,mPointOrign.x+r, 
				mPointOrign.y+r);//擦旧圆 
			
			PressNum=0;
			pDoc->PNCircle(&pDC,mPointOrign,point);ReleaseCapture();
		}
		else if(PressNum==1&&MenuID==33) 					//圆裁剪
		{
			pDC.SelectStockObject(NULL_BRUSH);//画空心圆
			r=(int)sqrt((mPointOrign.x-mPointOld.x)*(mPointOrign.x-mPointOld.x) 
				+(mPointOrign.y-mPointOld.y)*(mPointOrign.y-mPointOld.y)); 
			pDC.Ellipse(mPointOrign.x-r,mPointOrign.y-r,mPointOrign.x+r, 
				mPointOrign.y+r);//擦旧圆

			pDoc->group[PressNum]=point;
			PressNum=0;
			pDoc->CircleCut(&pDC,mPointOrign,point);
			ReleaseCapture();
		}

	} 
	
	if(MenuID==5) { // Bezier 曲线选点并做十字标志 
		pDoc->group[pDoc->PointNum++]=point; 
		pDC.MoveTo(point.x-5,point.y); 
		pDC.LineTo(point.x+5,point.y); 
		pDC.MoveTo(point.x,point.y-5); 
		pDC.LineTo(point.x,point.y+5); 
		SetCapture();PressNum=1; 
	} 
	if(MenuID==6&&PressNum==0){//  在控制点数组中，逐个寻找
		for(int i=0;i<pDoc->PointNum;i++) 
		{ 
			if((point.x>=pDoc->group[i].x-5)&&(point.x<=pDoc->group[i].x+5) 
				&&(point.y>=pDoc->group[i].y-5)&&(point.y<=pDoc->group[i].y+5)) 
			{ 
				
				SaveNumber=i;
				PressNum=1;
			}
		}
	}

	if(MenuID==11) { //  平移 
		if(PressNum==0){ 
			PressNum++; 
			mPointOrign=point; 
			mPointOld=point; //记录第一点 
			SetCapture(); 
		} 
		else if(PressNum==1){	//根据两点间距计算平移量
			for(int i=0;i<pDoc->PointNum;i++)//根据平移量计算新图形坐标
			{
				pDoc->group[i].x+=point.x-mPointOrign.x; 
				pDoc->group[i].y+=point.y-mPointOrign.y; 
			} 
					pDC.MoveTo(mPointOrign);//擦除橡皮筋
				    pDC.LineTo(point); 
			
			pDoc->DrawGraph(&pDC);//生成新图形 
			ReleaseCapture(); 
			PressNum=0; 
		} 
	} 
	if (MenuID==45){    //   旋转 
		if(PressNum==0){ 
			PressNum++; 
			mPointOrign=point; 
			mPointOld=point; //记录第一点 
			SetCapture(); 
		} 
		else if (PressNum==1)
		{
// 			double a=0;
// 			if (point.x==mPointOrign.x&&point.y==mPointOrign.y)
// 				return;
// 			if (point.x==mPointOrign.x&&point.y>mPointOrign.y)
// 				a = 3.1415926/2.0;
// 			else if(point.x==mPointOrign.x&&point.y<mPointOrign.y)
// 				a=3.1415926/2.0*3.0;
// 			else
// 				//计算旋转弧度
// 				a = atan((double)(e.Y - FirstY) / (double)(e.X - FirstX));
// 			//弧度转化为角度
// 			a=a/3.1415926*180.0;



		}
	}

	if(MenuID==15) { // 对称变换 
		if(PressNum==0){ 
			PressNum++; 
			mPointOrign=point; 
			mPointOld=point; //记录第一点 
			SetCapture(); 
		} 
		else if(PressNum==1){ 
			pDoc->Symmetry(mPointOrign,point);//进行对称变换
			pDoc->DrawGraph(&pDC);//生成新图形 
			ReleaseCapture(); 
			PressNum=0; 
		} 
	} 

	if(MenuID==20) { // 种子填充:画边界
		if(PressNum==0){ 
			mPointOrign=point; 
			mPointOld=point; 
			mPointOld1=point; //记录第一点 
			PressNum++; 
			
			
			SetCapture();
		}
		else{ 
			pDC.MoveTo(mPointOrign);//擦除橡皮筋 
			pDC.LineTo(point); 
			pDoc->group[0]=mPointOrign;//借助 DDA 直线函数画边界
			pDoc->group[1]=point; 
			pDoc->DDALine(&pDC); 
			mPointOrign=point; 
			mPointOld=point; 
			PressNum++; 
		} 
	} 

	if(MenuID==21) { // 确定种子点，填充 
		pDoc->SeedFill(&pDC,point); 
		PressNum=0;MenuID=20;//设置决定顶点操作方式
	}

	if(MenuID==22||MenuID==23||MenuID==25) { // 边缘填充选顶点 
		pDoc->group[PressNum++]=point; pDoc->PointNum++; 
		mPointOrign=point; 
		mPointOld=point; 
		SetCapture(); 
	} 

	if(MenuID==24||MenuID==43||MenuID==44) { // Cohen-sutherland 裁剪算法 
		if(PressNum==0){ 
			mPointOrign=point; 
			mPointOld=point; 
			PressNum++; 
			SetCapture(); 
		} 
		else
		{ 
			if (MenuID==24)
			{
				pDoc->CohenSutherland(&pDC,mPointOrign,point); 
			}
		else if (MenuID==43)
		{
			pDoc->CutMiddle(&pDC,mPointOrign,point);
		}
		else if (MenuID==44)
		{
			pDoc->CutLiang(&pDC,mPointOrign,point);
		}
			ReleaseCapture(); 
			PressNum=0; 
		} 
	} 

	
	CView::OnLButtonDown(nFlags, point);
}

void CMy2012302590125View::OnMouseMove(UINT nFlags, CPoint point) 
{
	// TODO: Add your message handler code here and/or call default
	
	int xx,yy,r; 
	char p1[20];

	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针
	
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDC.DPtoLP(&point); 
	pDC.SetROP2(R2_NOT); // 设置异或方式
	
	
	xx=point.x;	//取出坐标信息
	yy=point.y; 
	sprintf(p1,"%4d",xx);	//转化为字符串
	m_wndStatusBar.SetPaneText(2,p1,TRUE); //在第 2 个区域显示 x 坐标 
	sprintf(p1,"%4d",yy); 
    m_wndStatusBar.SetPaneText(3,p1,TRUE); //在第 3 个区域显示 y 坐标 
	
	if((MenuID==1||MenuID==11||MenuID==12||MenuID==15||MenuID==20||MenuID==22||MenuID==23||MenuID==24||MenuID==25||MenuID==43||MenuID==44)&&PressNum>0){ 
		if(mPointOld!=point){ 
			pDC.MoveTo(mPointOrign);   
			pDC.LineTo(mPointOld);//擦旧线 
			
			pDC.MoveTo(mPointOrign);
			pDC.LineTo(point);//画新线 
			mPointOld=point; 
		} 
	} 
	
	if((MenuID==3||MenuID==4||MenuID==33)&&PressNum==1){ 
		pDC.SelectStockObject(NULL_BRUSH);//画空心圆 
		if(mPointOld!=point){ 
			r=(int)sqrt((mPointOrign.x-mPointOld.x)*(mPointOrign.x-
				mPointOld.x)+(mPointOrign.y-mPointOld.y)*(mPointOrign.y -mPointOld.y)); 
			pDC.Ellipse(mPointOrign.x-r,mPointOrign.y-r,mPointOrign.x+r, mPointOrign.y+r);//擦旧圆 
			r=(int)sqrt((mPointOrign.x-point.x)*(mPointOrign.x-point.x) 
				+(mPointOrign.y-point.y)*(mPointOrign.y-point.y)); 
			pDC.Ellipse(mPointOrign.x-r,mPointOrign.y-r,mPointOrign.x+r, mPointOrign.y+r);//画新圆 
			mPointOld=point; 
		} 
	} 
	
	if(MenuID==6&&PressNum>0){ 
		if(pDoc->group[SaveNumber]!=point) 
		{ 
			pDC.MoveTo(pDoc->group[SaveNumber].x-5,pDoc->group[SaveNumber].y); 
			pDC.LineTo(pDoc->group[SaveNumber].x+5,pDoc->group[SaveNumber].y); 
			pDC.MoveTo(pDoc->group[SaveNumber].x,pDoc->group[SaveNumber].y-5); 
			pDC.LineTo(pDoc->group[SaveNumber].x,pDoc->group[SaveNumber].y+5); 
			pDoc->Bezier(&pDC,1);//擦除十字标志和旧线 
			pDC.MoveTo(point.x-5,point.y); 
			pDC.LineTo(point.x+5,point.y); 
			pDC.MoveTo(point.x,point.y-5); 
			pDC.LineTo(point.x,point.y+5); 
			pDoc->group[SaveNumber]=point;//记录新控制点 
			pDoc->Bezier(&pDC,1);//画十字标志和新曲线 
		} 
	} 

	
	CView::OnMouseMove(nFlags, point);
}

void CMy2012302590125View::OnRButtonDown(UINT nFlags, CPoint point) 
{
	// TODO: Add your message handler code here and/or call default

	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针 
	CClientDC ht(this);	//定义当前绘图设备
	OnPrepareDC(&ht); 
	ht.DPtoLP(&point);


	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDC.DPtoLP(&point); 
	pDC.SetROP2(R2_NOT); // 设置异或方式
	if(MenuID==5&&pDoc->PointNum>3){ 
		pDoc->Bezier(&ht,1);//绘制 Bezier 函数 
		MenuID=6;	//将下面的操作改为修改控制点位置
		PressNum=0;
	}
	if(MenuID==6&&PressNum==1){ 
		PressNum=0; 
	} 

	if(MenuID==20&&PressNum>0) { //  种子填充 
		pDC.MoveTo(mPointOrign);//擦除橡皮筋
		pDC.LineTo(point); 
		pDoc->group[0]=mPointOld1;//封闭多边形
		pDoc->group[1]=mPointOrign; 
		pDoc->DDALine(&ht); 
		PressNum=0;MenuID=21;//改变操作方式为种子点选取
		ReleaseCapture(); 
	} 

	if(MenuID==22) { // 边缘填充选点结束 
		pDoc->group[PressNum]=pDoc->group[0]; pDoc->PointNum++; 
		ht.MoveTo(pDoc->group[PressNum-1]);
		ht.LineTo(pDoc->group[0]); 
		for(int i=0;i<PressNum;i++) 
			ht.LineTo(pDoc->group[i+1]);
		pDC.MoveTo(mPointOrign);//擦除橡皮筋
		pDC.LineTo(point); 
		pDoc->EdgeFill(&ht); 
		PressNum=0;pDoc->PointNum=0; ReleaseCapture(); 
	} 

	if(MenuID==23){ 
		pDoc->group[PressNum]=pDoc->group[0]; //封闭多边形
		ht.MoveTo(pDoc->group[PressNum-1]); //擦除
		ht.LineTo(pDoc->group[0]); 
		for(int i=0;i<PressNum;i++) //擦除 
			ht.LineTo(pDoc->group[i+1]); 
		CPen pen(PS_SOLID,1,pDoc->m_crColor);//设置多边形边界颜色（即画笔） 
		CPen *pOldPen=ht.SelectObject(&pen); 
		CBrush brush(pDoc->m_crColor);	//设置多边形填充颜色（即画刷）
		CBrush *pOldBrush=ht.SelectObject(&brush);
		ht.SetROP2(R2_COPYPEN);	//设置直接画方式

		pDC.MoveTo(mPointOrign);//擦除橡皮筋
		pDC.LineTo(point);

		ht.Polygon(pDoc->group,PressNum);//调用多边形扫描线填充函数 

	

		ht.SelectObject(pOldPen);//恢复系统的画笔、画刷颜色设置 
		ht.SelectObject(pOldBrush); 
		PressNum=0;pDoc->PointNum=0;//初始化参数，为下一次操作做准备 
		ReleaseCapture(); 
	} 

	if(MenuID==25) { // 多边形裁剪 
		pDoc->group[PressNum]=pDoc->group[0];//将第一个顶点作为最后一个顶点 
		pDoc->PointNum=PressNum;	//记录顶点数量
		ht.MoveTo(pDoc->group[PressNum-1]); ht.LineTo(pDoc->group[0]); 
		pDoc->PolygonCut(&ht); 
		PressNum=0;pDoc->PointNum=0; 
		ReleaseCapture(); 
	} 

	CView::OnRButtonDown(nFlags, point);
}

void CMy2012302590125View::OnDrawDdaline() 
{
	// TODO: Add your command handler code here
	PressNum=0; MenuID=1; 
}

void CMy2012302590125View::OnDrawBcircle() 
{
	// TODO: Add your command handler code here
	PressNum=0; MenuID=3; 
}

void CMy2012302590125View::OnDrawPNcircle() 
{
	// TODO: Add your command handler code here
	PressNum=0; MenuID=4; 
}

void CMy2012302590125View::OnCurveBezier() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针 
	pDoc->PointNum=0; //初始化
	
	PressNum=0; MenuID=5; 
}

void CMy2012302590125View::OnLButtonDblClk(UINT nFlags, CPoint point) 
{
	// TODO: Add your message handler code here and/or call default
	
	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针 
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDC.DPtoLP(&point); 
	pDC.SetROP2(R2_NOT); // 设置异或方式 
	if(MenuID==6){ 
		for(int i=0;i<pDoc->PointNum;i++){//消除所有光标 
			
			
			pDC.MoveTo(pDoc->group[i].x-5,pDoc->group[i].y); 
			pDC.LineTo(pDoc->group[i].x+5,pDoc->group[i].y); 
			pDC.MoveTo(pDoc->group[i].x,pDoc->group[i].y-5); 
			pDC.LineTo(pDoc->group[i].x,pDoc->group[i].y+5); 
		} 
		pDoc->Bezier(&pDC,0);//绘制 Bezier 函数 
		
		MenuID=5;	//将下面的操作改回 Bezier 曲线方式
		PressNum=0; 
		pDoc->PointNum=0; 
		ReleaseCapture(); 
	} 

	CView::OnLButtonDblClk(nFlags, point);
}

void CMy2012302590125View::OnDrawChar() 
{
	// TODO: Add your command handler code here
	CDC *pDC=GetDC();
	CDrawCharDIG dlg;
	if(dlg.DoModal()==IDOK)
	{
		CFont *pfntOld=pDC->SelectObject(&dlg.m_fnt);//保存旧字体
		pDC->SetTextColor(dlg.m_clrText);                    //设置颜色
		pDC->TextOut(dlg.m_nX,dlg.m_nY,dlg.m_strString);//画到屏幕上
		pDC->SelectObject(pfntOld);                              //还原旧字体
	}
	ReleaseDC(pDC);

}

void CMy2012302590125View::OnTransMove() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *PDoc=GetDocument(); //获得文档类指针
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	PDoc->GenerateGraph(&pDC);//调用文档类函数在屏幕上生成图形 
	PressNum=0; MenuID=11; 

}

void CMy2012302590125View::OnDrawMidline() 
{
	// TODO: Add your command handler code here
	PressNum=0; MenuID=12; 
}

void CMy2012302590125View::OnTransSymmetry() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针
	CClientDC pDC(this); 	
	OnPrepareDC(&pDC); 
	pDoc->GenerateGraph(&pDC); 
	PressNum=0; MenuID=15; 

}

void CMy2012302590125View::OnFillSeed() 
{
	// TODO: Add your command handler code here
	PressNum=0; MenuID=20; 
}

void CMy2012302590125View::OnEdgeFill() 
{
	// TODO: Add your command handler code here
 	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针 
 	pDoc->PointNum=0;
	PressNum=0; MenuID=22; 
}

void CMy2012302590125View::OnFillScanline() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针 
	pDoc->PointNum=0;//实际上不需要该变量，但为了借鉴边缘填充的部分功 
	//能，与边缘填充保持一致 
	PressNum=0;MenuID=23; 

}

void CMy2012302590125View::OnCutCs() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDoc->DrawWindow(&pDC); 
	PressNum=0;MenuID=24; 

}

void CMy2012302590125View::OnCutPolygon() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDoc->DrawWindow(&pDC); 
	PressNum=0;MenuID=25; 

}

void CMy2012302590125View::OnOnCutCircle() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc  *pDoc=GetDocument(); //获得文档类指针
	CClientDC pDC(this);
	OnPrepareDC(&pDC);
	pDoc->DrawWindow(&pDC);
	PressNum=0;MenuID=33;

}

void CMy2012302590125View::OnCutMiddle() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDoc->DrawWindow(&pDC); 
	PressNum=0;MenuID=43; 
}

void CMy2012302590125View::OnCutLiang() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //获得文档类指针
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDoc->DrawWindow(&pDC); 
	PressNum=0;MenuID=44; 
}

void CMy2012302590125View::OnTransRotate() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *PDoc=GetDocument(); //获得文档类指针
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	PDoc->GenerateGraph(&pDC);//调用文档类函数在屏幕上生成图形 
	PressNum=0; MenuID=45; 
}
