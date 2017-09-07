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
	
	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ�� 

	int r;
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDC.DPtoLP(&point); 
	pDC.SetROP2(R2_NOT); // �������ʽ
	
	
	if(MenuID==1||MenuID ==12) { // DDA ֱ��
		if(PressNum==0){	//��һ�ΰ�������һ�㱣�����ĵ���������
			pDoc->group[PressNum]=point;
			PressNum++;
			mPointOrign=point; 
			mPointOld=point; //��¼��һ��
			SetCapture(); 
		} 
		else if(PressNum==1){	//�ڶ��ΰ��������ڶ��㣬���ĵ��໭��
			
			pDC.MoveTo(mPointOrign);   
			pDC.LineTo(mPointOld);//������ 
			
			pDoc->group[PressNum]=point;
			PressNum=0;//����ͼ 
			if(MenuID == 1)
			pDoc->DDALine(&pDC); 

			else if(MenuID==12)
				pDoc->MidLine(&pDC);
			
			ReleaseCapture(); 
		} 
	} 
	
	
	if(MenuID==3|| MenuID==4||MenuID==33) { // Bresenham Բ 
		if(PressNum==0){//��һ�ΰ�������һ�㱣���� mPointOrign 
			pDoc->group[PressNum]=point; 
			PressNum++; 
			mPointOrign=point; 
			mPointOld=point; //��¼��һ�� 
			SetCapture(); 
		} 
		else if(PressNum==1&&MenuID==3){//�ڶ��ΰ��������ĵ��໭Բ����ͼ 
			
			pDC.SelectStockObject(NULL_BRUSH);//������Բ
			r=(int)sqrt((mPointOrign.x-mPointOld.x)*(mPointOrign.x-mPointOld.x) 
				+(mPointOrign.y-mPointOld.y)*(mPointOrign.y-mPointOld.y)); 
			pDC.Ellipse(mPointOrign.x-r,mPointOrign.y-r,mPointOrign.x+r, 
				mPointOrign.y+r);//����Բ 
			
			PressNum=0; 
			pDoc->BCircle(&pDC,mPointOrign,point);ReleaseCapture();
		}
		else if(PressNum==1&&MenuID==4){//�ڶ��ΰ������û�Բ����ͼ
			
			pDC.SelectStockObject(NULL_BRUSH);//������Բ
			r=(int)sqrt((mPointOrign.x-mPointOld.x)*(mPointOrign.x-mPointOld.x) 
				+(mPointOrign.y-mPointOld.y)*(mPointOrign.y-mPointOld.y)); 
			pDC.Ellipse(mPointOrign.x-r,mPointOrign.y-r,mPointOrign.x+r, 
				mPointOrign.y+r);//����Բ 
			
			PressNum=0;
			pDoc->PNCircle(&pDC,mPointOrign,point);ReleaseCapture();
		}
		else if(PressNum==1&&MenuID==33) 					//Բ�ü�
		{
			pDC.SelectStockObject(NULL_BRUSH);//������Բ
			r=(int)sqrt((mPointOrign.x-mPointOld.x)*(mPointOrign.x-mPointOld.x) 
				+(mPointOrign.y-mPointOld.y)*(mPointOrign.y-mPointOld.y)); 
			pDC.Ellipse(mPointOrign.x-r,mPointOrign.y-r,mPointOrign.x+r, 
				mPointOrign.y+r);//����Բ

			pDoc->group[PressNum]=point;
			PressNum=0;
			pDoc->CircleCut(&pDC,mPointOrign,point);
			ReleaseCapture();
		}

	} 
	
	if(MenuID==5) { // Bezier ����ѡ�㲢��ʮ�ֱ�־ 
		pDoc->group[pDoc->PointNum++]=point; 
		pDC.MoveTo(point.x-5,point.y); 
		pDC.LineTo(point.x+5,point.y); 
		pDC.MoveTo(point.x,point.y-5); 
		pDC.LineTo(point.x,point.y+5); 
		SetCapture();PressNum=1; 
	} 
	if(MenuID==6&&PressNum==0){//  �ڿ��Ƶ������У����Ѱ��
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

	if(MenuID==11) { //  ƽ�� 
		if(PressNum==0){ 
			PressNum++; 
			mPointOrign=point; 
			mPointOld=point; //��¼��һ�� 
			SetCapture(); 
		} 
		else if(PressNum==1){	//�������������ƽ����
			for(int i=0;i<pDoc->PointNum;i++)//����ƽ����������ͼ������
			{
				pDoc->group[i].x+=point.x-mPointOrign.x; 
				pDoc->group[i].y+=point.y-mPointOrign.y; 
			} 
					pDC.MoveTo(mPointOrign);//������Ƥ��
				    pDC.LineTo(point); 
			
			pDoc->DrawGraph(&pDC);//������ͼ�� 
			ReleaseCapture(); 
			PressNum=0; 
		} 
	} 
	if (MenuID==45){    //   ��ת 
		if(PressNum==0){ 
			PressNum++; 
			mPointOrign=point; 
			mPointOld=point; //��¼��һ�� 
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
// 				//������ת����
// 				a = atan((double)(e.Y - FirstY) / (double)(e.X - FirstX));
// 			//����ת��Ϊ�Ƕ�
// 			a=a/3.1415926*180.0;



		}
	}

	if(MenuID==15) { // �ԳƱ任 
		if(PressNum==0){ 
			PressNum++; 
			mPointOrign=point; 
			mPointOld=point; //��¼��һ�� 
			SetCapture(); 
		} 
		else if(PressNum==1){ 
			pDoc->Symmetry(mPointOrign,point);//���жԳƱ任
			pDoc->DrawGraph(&pDC);//������ͼ�� 
			ReleaseCapture(); 
			PressNum=0; 
		} 
	} 

	if(MenuID==20) { // �������:���߽�
		if(PressNum==0){ 
			mPointOrign=point; 
			mPointOld=point; 
			mPointOld1=point; //��¼��һ�� 
			PressNum++; 
			
			
			SetCapture();
		}
		else{ 
			pDC.MoveTo(mPointOrign);//������Ƥ�� 
			pDC.LineTo(point); 
			pDoc->group[0]=mPointOrign;//���� DDA ֱ�ߺ������߽�
			pDoc->group[1]=point; 
			pDoc->DDALine(&pDC); 
			mPointOrign=point; 
			mPointOld=point; 
			PressNum++; 
		} 
	} 

	if(MenuID==21) { // ȷ�����ӵ㣬��� 
		pDoc->SeedFill(&pDC,point); 
		PressNum=0;MenuID=20;//���þ������������ʽ
	}

	if(MenuID==22||MenuID==23||MenuID==25) { // ��Ե���ѡ���� 
		pDoc->group[PressNum++]=point; pDoc->PointNum++; 
		mPointOrign=point; 
		mPointOld=point; 
		SetCapture(); 
	} 

	if(MenuID==24||MenuID==43||MenuID==44) { // Cohen-sutherland �ü��㷨 
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

	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ��
	
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDC.DPtoLP(&point); 
	pDC.SetROP2(R2_NOT); // �������ʽ
	
	
	xx=point.x;	//ȡ��������Ϣ
	yy=point.y; 
	sprintf(p1,"%4d",xx);	//ת��Ϊ�ַ���
	m_wndStatusBar.SetPaneText(2,p1,TRUE); //�ڵ� 2 ��������ʾ x ���� 
	sprintf(p1,"%4d",yy); 
    m_wndStatusBar.SetPaneText(3,p1,TRUE); //�ڵ� 3 ��������ʾ y ���� 
	
	if((MenuID==1||MenuID==11||MenuID==12||MenuID==15||MenuID==20||MenuID==22||MenuID==23||MenuID==24||MenuID==25||MenuID==43||MenuID==44)&&PressNum>0){ 
		if(mPointOld!=point){ 
			pDC.MoveTo(mPointOrign);   
			pDC.LineTo(mPointOld);//������ 
			
			pDC.MoveTo(mPointOrign);
			pDC.LineTo(point);//������ 
			mPointOld=point; 
		} 
	} 
	
	if((MenuID==3||MenuID==4||MenuID==33)&&PressNum==1){ 
		pDC.SelectStockObject(NULL_BRUSH);//������Բ 
		if(mPointOld!=point){ 
			r=(int)sqrt((mPointOrign.x-mPointOld.x)*(mPointOrign.x-
				mPointOld.x)+(mPointOrign.y-mPointOld.y)*(mPointOrign.y -mPointOld.y)); 
			pDC.Ellipse(mPointOrign.x-r,mPointOrign.y-r,mPointOrign.x+r, mPointOrign.y+r);//����Բ 
			r=(int)sqrt((mPointOrign.x-point.x)*(mPointOrign.x-point.x) 
				+(mPointOrign.y-point.y)*(mPointOrign.y-point.y)); 
			pDC.Ellipse(mPointOrign.x-r,mPointOrign.y-r,mPointOrign.x+r, mPointOrign.y+r);//����Բ 
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
			pDoc->Bezier(&pDC,1);//����ʮ�ֱ�־�;��� 
			pDC.MoveTo(point.x-5,point.y); 
			pDC.LineTo(point.x+5,point.y); 
			pDC.MoveTo(point.x,point.y-5); 
			pDC.LineTo(point.x,point.y+5); 
			pDoc->group[SaveNumber]=point;//��¼�¿��Ƶ� 
			pDoc->Bezier(&pDC,1);//��ʮ�ֱ�־�������� 
		} 
	} 

	
	CView::OnMouseMove(nFlags, point);
}

void CMy2012302590125View::OnRButtonDown(UINT nFlags, CPoint point) 
{
	// TODO: Add your message handler code here and/or call default

	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ�� 
	CClientDC ht(this);	//���嵱ǰ��ͼ�豸
	OnPrepareDC(&ht); 
	ht.DPtoLP(&point);


	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDC.DPtoLP(&point); 
	pDC.SetROP2(R2_NOT); // �������ʽ
	if(MenuID==5&&pDoc->PointNum>3){ 
		pDoc->Bezier(&ht,1);//���� Bezier ���� 
		MenuID=6;	//������Ĳ�����Ϊ�޸Ŀ��Ƶ�λ��
		PressNum=0;
	}
	if(MenuID==6&&PressNum==1){ 
		PressNum=0; 
	} 

	if(MenuID==20&&PressNum>0) { //  ������� 
		pDC.MoveTo(mPointOrign);//������Ƥ��
		pDC.LineTo(point); 
		pDoc->group[0]=mPointOld1;//��ն����
		pDoc->group[1]=mPointOrign; 
		pDoc->DDALine(&ht); 
		PressNum=0;MenuID=21;//�ı������ʽΪ���ӵ�ѡȡ
		ReleaseCapture(); 
	} 

	if(MenuID==22) { // ��Ե���ѡ����� 
		pDoc->group[PressNum]=pDoc->group[0]; pDoc->PointNum++; 
		ht.MoveTo(pDoc->group[PressNum-1]);
		ht.LineTo(pDoc->group[0]); 
		for(int i=0;i<PressNum;i++) 
			ht.LineTo(pDoc->group[i+1]);
		pDC.MoveTo(mPointOrign);//������Ƥ��
		pDC.LineTo(point); 
		pDoc->EdgeFill(&ht); 
		PressNum=0;pDoc->PointNum=0; ReleaseCapture(); 
	} 

	if(MenuID==23){ 
		pDoc->group[PressNum]=pDoc->group[0]; //��ն����
		ht.MoveTo(pDoc->group[PressNum-1]); //����
		ht.LineTo(pDoc->group[0]); 
		for(int i=0;i<PressNum;i++) //���� 
			ht.LineTo(pDoc->group[i+1]); 
		CPen pen(PS_SOLID,1,pDoc->m_crColor);//���ö���α߽���ɫ�������ʣ� 
		CPen *pOldPen=ht.SelectObject(&pen); 
		CBrush brush(pDoc->m_crColor);	//���ö���������ɫ������ˢ��
		CBrush *pOldBrush=ht.SelectObject(&brush);
		ht.SetROP2(R2_COPYPEN);	//����ֱ�ӻ���ʽ

		pDC.MoveTo(mPointOrign);//������Ƥ��
		pDC.LineTo(point);

		ht.Polygon(pDoc->group,PressNum);//���ö����ɨ������亯�� 

	

		ht.SelectObject(pOldPen);//�ָ�ϵͳ�Ļ��ʡ���ˢ��ɫ���� 
		ht.SelectObject(pOldBrush); 
		PressNum=0;pDoc->PointNum=0;//��ʼ��������Ϊ��һ�β�����׼�� 
		ReleaseCapture(); 
	} 

	if(MenuID==25) { // ����βü� 
		pDoc->group[PressNum]=pDoc->group[0];//����һ��������Ϊ���һ������ 
		pDoc->PointNum=PressNum;	//��¼��������
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
	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ�� 
	pDoc->PointNum=0; //��ʼ��
	
	PressNum=0; MenuID=5; 
}

void CMy2012302590125View::OnLButtonDblClk(UINT nFlags, CPoint point) 
{
	// TODO: Add your message handler code here and/or call default
	
	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ�� 
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDC.DPtoLP(&point); 
	pDC.SetROP2(R2_NOT); // �������ʽ 
	if(MenuID==6){ 
		for(int i=0;i<pDoc->PointNum;i++){//�������й�� 
			
			
			pDC.MoveTo(pDoc->group[i].x-5,pDoc->group[i].y); 
			pDC.LineTo(pDoc->group[i].x+5,pDoc->group[i].y); 
			pDC.MoveTo(pDoc->group[i].x,pDoc->group[i].y-5); 
			pDC.LineTo(pDoc->group[i].x,pDoc->group[i].y+5); 
		} 
		pDoc->Bezier(&pDC,0);//���� Bezier ���� 
		
		MenuID=5;	//������Ĳ����Ļ� Bezier ���߷�ʽ
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
		CFont *pfntOld=pDC->SelectObject(&dlg.m_fnt);//���������
		pDC->SetTextColor(dlg.m_clrText);                    //������ɫ
		pDC->TextOut(dlg.m_nX,dlg.m_nY,dlg.m_strString);//������Ļ��
		pDC->SelectObject(pfntOld);                              //��ԭ������
	}
	ReleaseDC(pDC);

}

void CMy2012302590125View::OnTransMove() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *PDoc=GetDocument(); //����ĵ���ָ��
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	PDoc->GenerateGraph(&pDC);//�����ĵ��ຯ������Ļ������ͼ�� 
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
	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ��
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
 	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ�� 
 	pDoc->PointNum=0;
	PressNum=0; MenuID=22; 
}

void CMy2012302590125View::OnFillScanline() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ�� 
	pDoc->PointNum=0;//ʵ���ϲ���Ҫ�ñ�������Ϊ�˽����Ե���Ĳ��ֹ� 
	//�ܣ����Ե��䱣��һ�� 
	PressNum=0;MenuID=23; 

}

void CMy2012302590125View::OnCutCs() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ��
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDoc->DrawWindow(&pDC); 
	PressNum=0;MenuID=24; 

}

void CMy2012302590125View::OnCutPolygon() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ��
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDoc->DrawWindow(&pDC); 
	PressNum=0;MenuID=25; 

}

void CMy2012302590125View::OnOnCutCircle() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc  *pDoc=GetDocument(); //����ĵ���ָ��
	CClientDC pDC(this);
	OnPrepareDC(&pDC);
	pDoc->DrawWindow(&pDC);
	PressNum=0;MenuID=33;

}

void CMy2012302590125View::OnCutMiddle() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ��
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDoc->DrawWindow(&pDC); 
	PressNum=0;MenuID=43; 
}

void CMy2012302590125View::OnCutLiang() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *pDoc=GetDocument(); //����ĵ���ָ��
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	pDoc->DrawWindow(&pDC); 
	PressNum=0;MenuID=44; 
}

void CMy2012302590125View::OnTransRotate() 
{
	// TODO: Add your command handler code here
	CMy2012302590125Doc *PDoc=GetDocument(); //����ĵ���ָ��
	CClientDC pDC(this); 
	OnPrepareDC(&pDC); 
	PDoc->GenerateGraph(&pDC);//�����ĵ��ຯ������Ļ������ͼ�� 
	PressNum=0; MenuID=45; 
}
