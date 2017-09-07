// 2012302590125Doc.cpp : implementation of the CMy2012302590125Doc class
//

#include "stdafx.h"
#include "2012302590125.h"

#include "2012302590125Doc.h"
#include "math.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


#define XMIN 100//����Ĵ��ڲ������ţ�����ֻҪ�ڱ�ʹ��ǰ���弴�� 
#define XMAX 400 
#define YMIN 100 
#define YMAX 300 
#define LEFT 1//����ļ����������� 
#define RIGHT 2 
#define BOTTOM 4 
#define TOP 8 






/////////////////////////////////////////////////////////////////////////////
// CMy2012302590125Doc

IMPLEMENT_DYNCREATE(CMy2012302590125Doc, CDocument)

BEGIN_MESSAGE_MAP(CMy2012302590125Doc, CDocument)
//{{AFX_MSG_MAP(CMy2012302590125Doc)
ON_COMMAND(ID_GRAPH_COLOR, OnGraphColor)
//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CMy2012302590125Doc construction/destruction

CMy2012302590125Doc::CMy2012302590125Doc()
{
	// TODO: add one-time construction code here
	
	m_crColor=RGB(0, 0, 0);
	
}

CMy2012302590125Doc::~CMy2012302590125Doc()
{
}

BOOL CMy2012302590125Doc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;
	
	// TODO: add reinitialization code here
	// (SDI documents will reuse this document)
	
	return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CMy2012302590125Doc serialization

void CMy2012302590125Doc::Serialize(CArchive& ar)
{
	if (ar.IsStoring())
	{
		// TODO: add storing code here
	}
	else
	{
		// TODO: add loading code here
	}
}

/////////////////////////////////////////////////////////////////////////////
// CMy2012302590125Doc diagnostics

#ifdef _DEBUG
void CMy2012302590125Doc::AssertValid() const
{
	CDocument::AssertValid();
}

void CMy2012302590125Doc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CMy2012302590125Doc commands


void CMy2012302590125Doc::DDALine(CClientDC *DCPoint) 
{ 
	int x,x0,y0,x1,y1,flag; 
	float m,y; 
	DCPoint->SetROP2(R2_COPYPEN); //��ͼ����Ϊֱ�ӻ� 
	//ֱ�߶˵������ȷ�������� group[0]��group[1] 
	x0=group[0].x;y0=group[0].y; 
	x1=group[1].x;y1=group[1].y; 
	if(x0==x1&&y0==y1)return; 
	if(x0==x1)//��ֱ�� 
	{ 
		if(y0>y1) 
		{ 
			x=y0;y0=y1;y1=x;
		}
		for(x=y0;x<=y1;x++) 
		{ 
			DCPoint->SetPixel(x0, x,m_crColor);
		}
		return; 
		
		
	} 
	if(y0==y1)//ˮƽ�� 
	{ 
		if(x0>x1) 
		{ 
			x=x0;x0=x1;x1=x;
		}
		for(x=x0;x<=x1;x++) 
		{ 
			DCPoint->SetPixel(x, y0,m_crColor);
		}
		return;
	}
	if(x0>x1) 
	{ 
		x=x0;x0=x1;x1=x;//������ʼ����ֹ�� 
		x=y0;y0=y1;y1=x; 
	} 
	flag=0; 
	if(x1-x0>y1-y0&&y1-y0>0)flag=1; 
	if(x1-x0>y0-y1&&y0-y1>0) 
	{ 
		flag=2;y0=-y0;y1=-y1;
	}
	if(y1-y0>x1-x0) 
	{ 
		flag=3;x=x0;x0=y0;y0=x;x=x1;x1=y1;y1=x;
	}
	if(y0-y1>x1-x0) 
	{ 
		flag=4;x=x0;x0=-y0;y0=x;x=x1;x1=-y1;y1=x;
	}
	m=(float)(y1-y0)/(float)(x1-x0); 
	for(x=x0,y=(float)y0;x<=x1;x++,y=y+m) 
	{ 
		if(flag==1)DCPoint->SetPixel(x,int(y),m_crColor); 
		if(flag==2)DCPoint->SetPixel(x,-int(y),m_crColor); 
		if(flag==3)DCPoint->SetPixel(int(y),x,m_crColor); 
		if(flag==4)DCPoint->SetPixel(int(y),-x,m_crColor); 
	} 
	
} 





void CMy2012302590125Doc::BCircle(CClientDC *DCPoint,CPoint p1,CPoint p2) 
{ 
	CRect rc;
	bool a=DCPoint->GetBoundsRect(&rc,0);
	BCircle(DCPoint,&rc,p1,p2);

}

void CMy2012302590125Doc::CircleCut(CClientDC *DCPoint,CPoint p1,CPoint p2)
{
	CRect rc(XMIN,YMIN,XMAX,YMAX);
	BCircle(DCPoint, &rc,p1,p2);
}



void CMy2012302590125Doc::BCircle(CClientDC * DCPoint, 
								 CRect* rc ,
								 CPoint p1,CPoint p2)
{
	int r,d,x,y,x0,y0; 
	DCPoint->SetROP2(R2_COPYPEN); //��ͼ����Ϊֱ�ӻ� 
	r=(int)sqrt((p1.x-p2.x)*(p1.x-p2.x)+(p1.y-p2.y)*(p1.y-p2.y)); 
	x=0;y=r;d=3-2*r;x0=p1.x;y0=p1.y; 
	while(x<y||x==y) 
	{ 
		if(rc->PtInRect(CPoint(x+x0,y+y0)))	//�жϵ��Ƿ��ھ��ο���
			DCPoint->SetPixel(x+x0,y+y0, m_crColor); 
		if(rc->PtInRect(CPoint(-x+x0,y+y0)))	
			DCPoint->SetPixel(-x+x0,y+y0, m_crColor); 
		if(rc->PtInRect(CPoint(x+x0,-y+y0)))
			DCPoint->SetPixel(x+x0,-y+y0, m_crColor); 
		if(rc->PtInRect(CPoint(-x+x0,-y+y0)))
			DCPoint->SetPixel(-x+x0,-y+y0, m_crColor); 
		if(rc->PtInRect(CPoint(y+x0,x+y0)))		
			DCPoint->SetPixel(y+x0,x+y0, m_crColor); 
		if(rc->PtInRect(CPoint(-y+x0,x+y0)))	
			DCPoint->SetPixel(-y+x0,x+y0, m_crColor); 
		if(rc->PtInRect(CPoint(y+x0,-x+y0)))
			DCPoint->SetPixel(y+x0,-x+y0, m_crColor); 
		if(rc->PtInRect(CPoint(-y+x0,-x+y0)))
			DCPoint->SetPixel(-y+x0,-x+y0, m_crColor); 
		
		x=x+1; 
		if(d<0||d==0) 
		{ 
			d=d+4*x+6;
		}
		else 
		{ 
			y=y-1;d=d+4*(x-y)+10;
		}
	};
}



void CMy2012302590125Doc::PNCircle(CClientDC *DCPoint,CPoint p1,CPoint p2) 
{ 
	
	
	
	
	int r,d,x,y,x0,y0; 
	DCPoint->SetROP2(R2_COPYPEN); //��ͼ����Ϊֱ�ӻ� 
	r=(int)sqrt((p1.x-p2.x)*(p1.x-p2.x)+(p1.y-p2.y)*(p1.y-p2.y)); 
	d=0;x0=p1.x;y0=p1.y;x=x0;y=y0+r; 
	while(y>y0) 
	{ 
		DCPoint->SetPixel(x,y,m_crColor); 
		DCPoint->SetPixel(-x+2*x0,y,m_crColor); 
		DCPoint->SetPixel(x,-y+2*y0,m_crColor); 
		DCPoint->SetPixel(-x+2*x0,-y+2*y0,m_crColor); 
		if(d<0) 
		{ 
			x++;d=d+2*(x-x0)+1;
		}
		else 
		{ 
			y--;d=d-2*(y-y0)+1;
		}
	};
}




void CMy2012302590125Doc::Bezier(CClientDC *pDC,int mode) 
{ 
	CPoint p[1000];//����һ������洢������ Bezier ���߿��Ƶ� 
	int i,j; 
	i=0;j=0; 
	p[i++]=group[j++];//�Ƚ��� 1��2 �ŵ�������� 
	p[i++]=group[j++]; 
	while(j<=PointNum-2)//�����桢ż�ŵ㣬���ɲ��������� 
	{ 
		p[i++]=group[j++]; 
		p[i].x=(group[j].x+group[j-1].x)/2; 
		p[i++].y=(group[j].y+group[j-1].y)/2; p[i++]=group[j++]; 
	}; 
	for(j=0;j<i-3;j+=3)//���Ƶ���飬�ֱ����ɸ������� 
	{ 
		Bezier_4(pDC,mode,p[j],p[j+1],p[j+2],p[j+3]);
	}
	
} 


void CMy2012302590125Doc::Bezier_4(CClientDC *pDC,int mode,CPoint p1,CPoint p2,CPoint p3,CPoint p4) 
{ 
	int i,n; 
	CPoint p; 
	double t1,t2,t3,t4,dt; 
	CPen pen; 
	n=10; 
	if(mode)//mode=1ʱ�������ʽ���ɲ����ĺ�ɫ���ߣ����ڵ�����״
	{
		pDC->SetROP2(R2_NOT); 
		pen.CreatePen(PS_SOLID,1,m_crColor);
	}
	else//mode=0 ʱ������ɫ����ʽ���� 
	{ 
		pDC->SetROP2(R2_COPYPEN); 
		pen.CreatePen(PS_SOLID,1,m_crColor);
	}
	CPen *pOldPen=pDC->SelectObject(&pen); 
	dt=1.0/n;//���� t �ļ������ 10 �Σ����� 10 ��ֱ�߱�ʾһ������ 
	pDC->MoveTo(p1);//�Ƶ���� 
	for(i=1;i<=n;i++)//�� Bezier �������̼��������ϵȼ���� 10 ���� 
	{ 
		t1=(1.0-i*dt)*(1.0-i*dt)*(1.0-i*dt);	//����(1-t)3
		t2=i*dt*(1.0-i*dt)*(1.0-i*dt);	//���� t (1-t) 2
		t3=i*dt*i*dt*(1.0-i*dt);	//���� t2 (1-t)
		t4=i*dt*i*dt*i*dt;	//���� t3
		
		
		p.x=(int)(t1*p1.x+3*t2*p2.x+3*t3*p3.x+t4*p4.x); 
		p.y=(int)(t1*p1.y+3*t2*p2.y+3*t3*p3.y+t4*p4.y); pDC->LineTo(p); 
	} 
	pDC->SelectObject(pOldPen);
}


void CMy2012302590125Doc::OnGraphColor() 
{
	// TODO: Add your command handler code here
	
	CColorDialog dlg(m_crColor);//�����Ի�����󣬲�����Ĭ����ɫΪ
	//��һ��ѡ�����ɫ
	if(dlg.DoModal()==IDOK)      
		m_crColor =dlg.GetColor();
	
}


void    CMy2012302590125Doc::GenerateGraph(CClientDC *pDC) 
{ 
	group[0].x=100;group[0].y=100;//ͼ������׼�� 
	group[1].x=200;group[1].y=100; 
	group[2].x=200;group[2].y=200; 
	group[3].x=100;group[3].y=200; 
	group[4].x=100;group[4].y=100; 
	PointNum=5; 
	DrawGraph(pDC);//��ͼ��
}
void    CMy2012302590125Doc::DrawGraph(CClientDC *pDC) 
{ 
	int i; 
	CPen pen,*pOldPen; 
	
	
	pDC->SetROP2(R2_COPYPEN); 
	pen.CreatePen(PS_SOLID,1,RGB(255,0,255)); pOldPen=pDC->SelectObject(&pen); 
	pDC->MoveTo(group[0]); 
	for(i=1;i<PointNum;i++) 
		pDC->LineTo(group[i]); 
	pDC->SelectObject(pOldPen); 
} 


void  CMy2012302590125Doc::MidLine(CClientDC *DCPoint)
{
	int x,x0,y0,x1,y1,flag; 
	float y; 
	DCPoint->SetROP2(R2_COPYPEN); //��ͼ����Ϊֱ�ӻ� 
	//ֱ�߶˵������ȷ�������� group[0]��group[1] 
	x0=group[0].x;y0=group[0].y; 
	x1=group[1].x;y1=group[1].y; 
	if(x0==x1&&y0==y1)return; 
	if(x0==x1)//��ֱ��
	{
		if(y0>y1)
		{
			x=y0;y0=y1;y1=x;
		}
		for(x=y0;x<=y1;x++)
		{
			DCPoint->SetPixel(x,x0,m_crColor);
		}
		return;
	}
	if(y0==y1)//ˮƽ��
	{
		if(x0>x1)
		{
			x=x0;x0=x1;x1=x;
		}
		for(x=x0;x<=x1;x++)
		{
			DCPoint->SetPixel(x,x0,m_crColor);
		}
		return;
	}
	if(x0>x1)//�����㷨�����(x0,y0)����˵㣬��������㣬�ͽ�(x0,y0)��(x1,y1)����
	{        
		x=x0;x0=x1;x1=x;
		x=y0;y0=y1;y1=x;
	}
	flag=0;//ֱ�������
	if(x1-x0>y1-y0&&y1-y0>0)flag=1;
	if(x1-x0>y0-y1&&y0-y1>0)//�ڶ���ֱ��ת��Ϊ��һ��ֱ��
	{
		flag=2;y0=-y0;y1=-y1;
	}
	if(y1-y0>x1-x0)//������ֱ��ת��Ϊ��һ��ֱ��
	{
		flag=3;x=x0;x0=y0;y0=x;x=x1;x1=y1;y1=x;
	}
	if(y0-y1>x1-x0)//������ֱ��ת��Ϊ��һ��ֱ��
	{
		flag=4;x=x0;x0=-y0;y0=x;x=x1;x1=-y1;y1=x;
	}
	
	x=x0; y=y0; int d=(x1-x0)-2*(y1-y0);
	
	while(x<x1+1)
	{
		if(flag==1)DCPoint->SetPixel(x,int(y),m_crColor); 
		if(flag==2)DCPoint->SetPixel(x,-int(y),m_crColor); 
		if(flag==3)DCPoint->SetPixel(int(y),x,m_crColor); 
		if(flag==4)DCPoint->SetPixel(int(y),-x,m_crColor); 
		x++;
		if (d > 0)
		{
			d = d - 2 * (y1 - y0);
		}
		else
		{
			y++; d = d - 2 * ((y1 - y0) - (x1 - x0));
		}
	}
}


void CMy2012302590125Doc::Symmetry(CPoint p1,CPoint p2) 
{ 
	float a[3][3],b[3][3],c[3][3]; 
	float sa,ca,x,y; 
	int i; 
	ca=(p2.x-p1.x)/sqrt((p2.x-p1.x)*(p2.x-p1.x)+(p2.y-p1.y)*(p2.y-p1.y));//cos�� 
	sa=(p2.y-p1.y)/sqrt((p2.x-p1.x)*(p2.x-p1.x)+(p2.y-p1.y)*(p2.y-p1.y)); //sin�� 
	c[0][0]=1;c[0][1]=0;c[0][2]=-p1.x;//���� 1 
	c[1][0]=0;c[1][1]=1;c[1][2]=-p1.y; 
	c[2][0]=0;c[2][1]=0;c[2][2]=1; 
	b[0][0]=ca;b[0][1]=sa;b[0][2]=0; //���� 2 
	b[1][0]=-sa;b[1][1]=ca;b[1][2]=0; 
	b[2][0]=0;b[2][1]=0;b[2][2]=1; 
	a[0][0]=b[0][0]*c[0][0]+b[0][1]*c[1][0]+b[0][2]*c[2][0]; //���� 1��2 �ϲ� 
	a[0][1]=b[0][0]*c[0][1]+b[0][1]*c[1][1]+b[0][2]*c[2][1]; 
	a[0][2]=b[0][0]*c[0][2]+b[0][1]*c[1][2]+b[0][2]*c[2][2]; 
	a[1][0]=b[1][0]*c[0][0]+b[1][1]*c[1][0]+b[1][2]*c[2][0]; 
	a[1][1]=b[1][0]*c[0][1]+b[1][1]*c[1][1]+b[1][2]*c[2][1]; 
	a[1][2]=b[1][0]*c[0][2]+b[1][1]*c[1][2]+b[1][2]*c[2][2]; 
	a[2][0]=b[2][0]*c[0][0]+b[2][1]*c[1][0]+b[2][2]*c[2][0]; 
	a[2][1]=b[2][0]*c[0][1]+b[2][1]*c[1][1]+b[2][2]*c[2][1]; 
	a[2][2]=b[2][0]*c[0][2]+b[2][1]*c[1][2]+b[2][2]*c[2][2]; 
	b[0][0]=1;b[0][1]=0;b[0][2]=0; //���� 3 
	b[1][0]=0;b[1][1]=-1;b[1][2]=0; 
	b[2][0]=0;b[2][1]=0;b[2][2]=1; 
	c[0][0]=b[0][0]*a[0][0]+b[0][1]*a[1][0]+b[0][2]*a[2][0]; //���� 1��2��3 	�ϲ� 
	c[0][1]=b[0][0]*a[0][1]+b[0][1]*a[1][1]+b[0][2]*a[2][1]; 
	c[0][2]=b[0][0]*a[0][2]+b[0][1]*a[1][2]+b[0][2]*a[2][2]; 
	c[1][0]=b[1][0]*a[0][0]+b[1][1]*a[1][0]+b[1][2]*a[2][0]; 
	c[1][1]=b[1][0]*a[0][1]+b[1][1]*a[1][1]+b[1][2]*a[2][1]; 
	
	
	c[1][2]=b[1][0]*a[0][2]+b[1][1]*a[1][2]+b[1][2]*a[2][2]; 
	c[2][0]=b[2][0]*a[0][0]+b[2][1]*a[1][0]+b[2][2]*a[2][0]; 
	c[2][1]=b[2][0]*a[0][1]+b[2][1]*a[1][1]+b[2][2]*a[2][1]; 
	c[2][2]=b[2][0]*a[0][2]+b[2][1]*a[1][2]+b[2][2]*a[2][2]; 
	b[0][0]=ca;b[0][1]=-sa;b[0][2]=0; //���� 4 
	b[1][0]=sa;b[1][1]=ca;b[1][2]=0; 
	b[2][0]=0;b[2][1]=0;b[2][2]=1; 
	a[0][0]=b[0][0]*c[0][0]+b[0][1]*c[1][0]+b[0][2]*c[2][0]; //���� 1��2��3��4�ϲ� 
	a[0][1]=b[0][0]*c[0][1]+b[0][1]*c[1][1]+b[0][2]*c[2][1]; 
	a[0][2]=b[0][0]*c[0][2]+b[0][1]*c[1][2]+b[0][2]*c[2][2]; 
	a[1][0]=b[1][0]*c[0][0]+b[1][1]*c[1][0]+b[1][2]*c[2][0]; 
	a[1][1]=b[1][0]*c[0][1]+b[1][1]*c[1][1]+b[1][2]*c[2][1]; 
	a[1][2]=b[1][0]*c[0][2]+b[1][1]*c[1][2]+b[1][2]*c[2][2]; 
	a[2][0]=b[2][0]*c[0][0]+b[2][1]*c[1][0]+b[2][2]*c[2][0]; 
	a[2][1]=b[2][0]*c[0][1]+b[2][1]*c[1][1]+b[2][2]*c[2][1]; 
	a[2][2]=b[2][0]*c[0][2]+b[2][1]*c[1][2]+b[2][2]*c[2][2]; 
	b[0][0]=1;b[0][1]=0;b[0][2]=p1.x; //���� 5 
	b[1][0]=0;b[1][1]=1;b[1][2]=p1.y; 
	b[2][0]=0;b[2][1]=0;b[2][2]=1; 
	c[0][0]=b[0][0]*a[0][0]+b[0][1]*a[1][0]+b[0][2]*a[2][0];//���о���ϲ� 
	c[0][1]=b[0][0]*a[0][1]+b[0][1]*a[1][1]+b[0][2]*a[2][1]; 
	c[0][2]=b[0][0]*a[0][2]+b[0][1]*a[1][2]+b[0][2]*a[2][2]; 
	c[1][0]=b[1][0]*a[0][0]+b[1][1]*a[1][0]+b[1][2]*a[2][0]; 
	c[1][1]=b[1][0]*a[0][1]+b[1][1]*a[1][1]+b[1][2]*a[2][1]; 
	c[1][2]=b[1][0]*a[0][2]+b[1][1]*a[1][2]+b[1][2]*a[2][2]; 
	c[2][0]=b[2][0]*a[0][0]+b[2][1]*a[1][0]+b[2][2]*a[2][0]; 
	c[2][1]=b[2][0]*a[0][1]+b[2][1]*a[1][1]+b[2][2]*a[2][1]; 
	c[2][2]=b[2][0]*a[0][2]+b[2][1]*a[1][2]+b[2][2]*a[2][2]; 
	for(i=0;i<PointNum;i++) //���ø��Ͼ��������ͼ�ε�������б任 
	{ 
		
		
		x=c[0][0]*group[i].x+c[0][1]*group[i].y+c[0][2]; y=c[1][0]*group[i].x+c[1][1]*group[i].y+c[1][2]; 
		group[i].x=x; group[i].y=y; 
	} 
} 



void CMy2012302590125Doc::SeedFill(CClientDC *pDC,CPoint seedpoint) 
{ 
	int savex,xleft,xright,pflag,x,y,num; CPoint stack_ptr[200];//��ջ 
	pDC->SetROP2(R2_COPYPEN); //��ͼ����Ϊֱ�ӻ� 
	num=0; //num Ϊ��ջ�е������� 
	stack_ptr[num++]=seedpoint; 
	while(num>0) 
	{ 
		x=stack_ptr[--num].x;y=stack_ptr[num].y; 
		pDC->SetPixel(x,y,m_crColor);
		savex=x;     x++; 
		while(pDC->GetPixel(x,y)!=m_crColor)//������䣬ֱ���߽�
		{ 
			pDC->SetPixel(x++,y,m_crColor);
		};
		xright=x-1;   x=savex-1; 
		while(pDC->GetPixel(x,y)!=m_crColor)//������䣬ֱ���߽�
		{ 
			pDC->SetPixel(x--,y,m_crColor);
		};
		xleft=x+1;    x=xleft; y++;//��ɨ������һ������δ�������
		pflag=1; 
		while(x<xright) 
		{ 
			if(pDC->GetPixel(x,y)!=m_crColor&&pflag==1) 
			{//�߽��ĵ�һ��δ����������� 
				stack_ptr[num].x=x;stack_ptr[num++].y=y; x++;
			}
			if(pDC->GetPixel(x,y)==m_crColor) 
				pflag=1;//pflag=1 ��ʾ��������߽� 
			else 
				pflag=0;//pflag=0 ��ʾδ������� 
			x++; 
		}
		x=xleft; y-=2;	pflag=1;//��ɨ������һ������δ�������
		while(x<xright)
		{ 
			if(pDC->GetPixel(x,y)!=m_crColor&&pflag==1)
			{ 
				stack_ptr[num].x=x;stack_ptr[num++].y=y; x++;
			}
			if(pDC->GetPixel(x,y)==m_crColor) 
				pflag=1; 
			else 
				pflag=0; 
			x++; 
		} 
	}
}

void CMy2012302590125Doc::EdgeFill(CClientDC *pDC) 
{ 
	int i,xr,x1,y1,x2,y2,y;
	float m,x; 
	CPen pen; 
	pen.CreatePen(PS_SOLID,1,RGB(255-GetRValue(m_crColor),255-GetGValue(m_crColor),255-GetBValue(m_crColor)));//ȷ�������ɫ���ɸ���ɫ�뱳�� 
	//ɫ����϶��� 
	pDC->SetROP2(R2_XORPEN); //��ͼ����Ϊ��� 
	CPen *pOldPen=pDC->SelectObject(&pen); 
	xr=0; 	
	for(i=0;i<PointNum;i++)//�ҳ��߽���ұ߽����
	{ 
		if(xr<group[i].x)xr=group[i].x;
	}
	for(i=0;i<PointNum-1;i++) 
	{ 
		x1=group[i].x;x2=group[i+1].x;//ȡһ����
		y1=group[i].y;y2=group[i+1].y; 
		if(y1!=y2) 
		{ 
			if(y1>y2)//ȷ����x1,y1��Ϊ�¶˵�
			{ 
				y=y1;y1=y2;y2=y;
				y=x1;x1=x2;x2=y; 
			} 
			m=(float)(x2-x1)/(float)(y2-y1); x=x1;//mΪ����ɨ����֮��ߵ� x ���� 
			for(y=y1+1;y<=y2;y++) 
			{ 
				x+=m;//ȷ����Ե�� 
				pDC->MoveTo((int)x,y);//�ӱ�Ե��һֱ�����߽���Ҷ� 
				pDC->LineTo(xr,y); 
			} 	
		}
	}
	pDC->SelectObject(pOldPen);
}


void CMy2012302590125Doc::DrawWindow(CClientDC *pDC) 
{ 
	CPen pen; 
	pen.CreatePen(PS_SOLID,2,m_crColor); 
	CPen *pOldPen=pDC->SelectObject(&pen); pDC->SetROP2(R2_COPYPEN); 
	pDC->MoveTo(XMIN,YMIN); 
	pDC->LineTo(XMAX,YMIN); 
	pDC->LineTo(XMAX,YMAX); 
	pDC->LineTo(XMIN,YMAX); 
	pDC->LineTo(XMIN,YMIN); 
	pDC->SelectObject(pOldPen); 
} 

void CMy2012302590125Doc::CohenSutherland(CClientDC *pDC,CPoint p1,CPoint p2) 
{ 
	int code1,code2,code,x,y,x1,y1,x2,y2;
	pDC->SetROP2(R2_COPYPEN); 
	CPen Pen; 
	Pen.CreatePen(PS_SOLID,2,m_crColor); 
	CPen *OldPen=pDC->SelectObject(&Pen); 
	x1=p1.x; y1=p1.y; 
	x2=p2.x; y2=p2.y; 
	code1=encode(x1,y1);//�Զ˵���� 
	code2=encode(x2,y2); 
	while(code1!=0||code2!=0) 
	{ 
		if((code1&code2)!=0)return;//��ȫ���ɼ� 
		code=code1; 
		if(code1==0)code=code2; 
		if((LEFT&code)!=0)//���߶��봰����ߵĽ���
		{ 
			x=XMIN; 
			y=y1+(y2-y1)*(x-x1)/(x2-x1);
		}
		else if((RIGHT&code)!=0) //���߶��봰���ұߵĽ���
		{ 
			x=XMAX;
			y=y1+(y2-y1)*(x-x1)/(x2-x1);
		}
		else if((BOTTOM&code)!=0) //���߶��봰�ڵױߵĽ���
		{ 
			y=YMIN; 
			x=x1+(x2-x1)*(y-y1)/(y2-y1);
		}
		else if((TOP&code)!=0) //���߶��봰�ڶ��ߵĽ��� 
		{ 
			y=YMAX; 
			x=x1+(x2-x1)*(y-y1)/(y2-y1);
		}
		if(code==code1) 
		{ 
			x1=x;y1=y;code1=encode(x,y);
		}
		else 
		{ 
			x2=x;y2=y;code2=encode(x,y);
		}
	} 
	pDC->MoveTo(x1,y1); 
	pDC->LineTo(x2,y2); 
	pDC->SelectObject(OldPen);
}
///////////////////�������㷨
void CMy2012302590125Doc::CutLiang(CClientDC *pDC,CPoint p1,CPoint p2) 
{                      //�涨��x1,y1��Ϊ���
	int x1,y1,x2,y2;
	pDC->SetROP2(R2_COPYPEN); 
	CPen Pen; 
	Pen.CreatePen(PS_SOLID,2,m_crColor); 
	CPen *OldPen=pDC->SelectObject(&Pen); 
	x1=p1.x; y1=p1.y; 
	x2=p2.x; y2=p2.y;

	float tsx, tsy, tex, tey;//��������ʼ�ߡ������ձ߶�ӦT����
	if (x1 == x2)  //����
	{
		tsx = 0; tex = 1;
	}
	else if (x1 < x2)
	{   // �������㣬X�����ʼ�ߡ��ձ��漴ȷ������ֱ�Ӽ����Ӧ����
		tsx = (float)(XMIN - x1) / (float)(x2 - x1);
		tex = (float)(XMAX - x1) / (float)(x2 - x1);
	}
	else
	{
		tsx = (float)(XMAX - x1) / (float)(x2 - x1);
		tex = (float)(XMIN - x1) / (float)(x2 - x1);
	}
	if (y1 == y2)  //ˮƽ��
	{
		tsy = 0; tey = 1;
	}
	else if (y1 < y2)
	{   // �������㣬Y�����ʼ�ߡ��ձ��漴ȷ������ֱ�Ӽ����Ӧ����
		tsy = (float)(YMIN - y1) / (float)(y2 - y1);
		tey = (float)(YMAX - y1) / (float)(y2 - y1);
	}
	else
	{
		tsy = (float)(YMAX - y1) / (float)(y2 - y1);
		tey = (float)(YMIN - y1) / (float)(y2 - y1);
	}
	tsx = max(tsx, tsy);   //ϵͳ�ṩ�ĺ���ֻ�ܱȽ�������
	tsx = max(tsx, 0);     //�����Σ���3������ѡ������
	tex = min(tex, tey);
	tex = min(tex, 1);
	if (tsx < tex)     //���������㣬���ǿɼ���
	{
		int xx1, yy1, xx2, yy2;
		xx1=(int)(x1 + (x2 - x1) * tsx);
		yy1=(int)(y1 + (y2 - y1) * tsx);
		xx2=(int)(x1 + (x2 - x1) * tex);
		yy2=(int)(y1 + (y2 - y1) * tex);
		pDC->MoveTo(xx1,yy1);  //���ü�����߶�
    	pDC->LineTo(xx2,yy2);
    	pDC->SelectObject(OldPen);
	}
}
void CMy2012302590125Doc::CutMiddle(CClientDC *pDC,CPoint p1,CPoint p2){ //�е�ü� 
	int x1,y1,x2,y2;
	pDC->SetROP2(R2_COPYPEN); 
	CPen Pen; 
	Pen.CreatePen(PS_SOLID,2,m_crColor); 
	CPen *OldPen=pDC->SelectObject(&Pen); 
	x1=p1.x; y1=p1.y; 
	x2=p2.x; y2=p2.y;

	if (LineIsOutOfWindow(x1, y1, x2, y2))//������ھͿ���ȷ���߶���ȫ���ɼ���������
		return;
	p1 = FindNearestPoint(x1, y1, x2, y2);//�ӣ�X1��Y1��������Ѱ������ɼ���
	if (PointIsOutOfWindow(p1.x,p1.y))    //�ҵ���"�ɼ���"���ɼ���������
		return;
	p2 = FindNearestPoint(x2, y2, x1, y1);//����
	

	pDC->MoveTo(p1.x, p1.y);  //���ü�����߶�
	pDC->LineTo(p2.x,p2.y);
	pDC->SelectObject(OldPen);


}



int CMy2012302590125Doc::encode(int x,int y) 
{ 
	int c; 
	c=0; 
	if(x<XMIN)c=c+LEFT; 
	else if(x>XMAX)c=c+RIGHT; 
	if(y<YMIN)c=c+BOTTOM; 
	else if(y>YMAX)c=c+TOP; 
	return c; 
} 


void CMy2012302590125Doc::PolygonCut(CClientDC *pDC) 
{ 
	
	CPen pen; 
	pen.CreatePen(0,2,RGB(255,0,0)); 
	CPen *OldPen=pDC->SelectObject(&pen); pDC->SetROP2(R2_COPYPEN); 
	EdgeClipping(0);	//�õ�һ�����ڱ߽��вü�
	EdgeClipping(1);	//�õڶ������ڱ߽��вü�
	EdgeClipping(2);	//�õ��������ڱ߽��вü�
	EdgeClipping(3);	//�õ��������ڱ߽��вü�
	
	pDC->MoveTo(group[0]); //    ���Ʋü������ 
	for(int i=1;i<=PointNum;i++) 
	pDC->LineTo(group[i]); 
	pDC->SelectObject(OldPen); 
} 
void CMy2012302590125Doc::EdgeClipping(int linecode) 
{ 
	float x,y; 
	int n,i,number1; 
	CPoint q[200]; 
	number1=0; 
	if(linecode==0)// x=XMIN 
	{ 
		for(n=0;n<PointNum;n++) 
		{ 
			if(group[n].x<XMIN&&group[n+1].x<XMIN)//���⣬����� 
			{ 
			} 
			if(group[n].x>=XMIN&&group[n+1].x>=XMIN)//��������� 
			{ 
				q[number1++]=group[n+1];
			}
			if(group[n].x>=XMIN&&group[n+1].x<XMIN)//���⣬������� 
			{ 
				y=group[n].y+(float)(group[n+1].y-group[n].y)/ 
					(float)(group[n+1].x-group[n].x)*(float)(XMIN-group[n].x); q[number1].x=XMIN; 
				q[number1++].y=(int)y;
			}
			if(group[n].x<XMIN&&group[n+1].x>=XMIN)//���������㡢��� 
			{
				y=group[n].y+(float)(group[n+1].y-group[n].y)/ 
					(float)(group[n+1].x-group[n].x)*(float)(XMIN-group[n].x); q[number1].x=XMIN; 
				q[number1++].y=(int)y; 
				q[number1++]=group[n+1];
			} 
		} 
		
		for(i=0;i<number1;i++) 
		{ 
			group[i]=q[i];
		}
		group[number1]=q[0];
		PointNum=number1;number1=0;
	}
	if(linecode==1)//y=YMAX 
	{ 
		for(n=0;n<PointNum;n++) 
		{ 
			if(group[n].y>=YMAX&&group[n+1].y>=YMAX)//���⣬�����
			{ 
			} 
			if(group[n].y<YMAX&&group[n+1].y<YMAX)//���������
			{ 
				q[number1++]=group[n+1];
			}
			if(group[n].y<YMAX&&group[n+1].y>=YMAX)//���⣬�������
			{ 
				x=group[n].x+(float)(group[n+1].x-group[n].x)/ 
					(float)(group[n+1].y-group[n].y)*(float)(YMAX-group[n].y); 
				q[number1].x=(int)x; 
				q[number1++].y=YMAX;
			}
			if(group[n].y>=YMAX&&group[n+1].y<YMAX)//���������㡢���
			{ 
				x=group[n].x+(float)(group[n+1].x-group[n].x)/ 
					(float)(group[n+1].y-group[n].y)*(float)(YMAX-group[n].y); q[number1].x=(int)x; 
				q[number1++].y=YMAX; 
				q[number1++]=group[n+1];
			} 
		} 
		for(i=0;i<number1;i++) 
		{ 
			group[i]=q[i];
		}
		group[number1]=q[0];
		PointNum=number1;number1=0;
	}
	if(linecode==2)//x=XMAX 
	{ 
		for(n=0;n<PointNum;n++) 
		{ 
			if(group[n].x>=XMAX&&group[n+1].x>=XMAX)//���⣬����� 
			{ 
			} 
			if(group[n].x<XMAX&&group[n+1].x<XMAX)//��������� 
			{ 
				q[number1++]=group[n+1]; 
			}
			if(group[n].x<XMAX&&group[n+1].x>=XMAX)//���⣬������� 
			{ 
				y=group[n].y+(float)(group[n+1].y-group[n].y)/ 
					(float)(group[n+1].x-group[n].x)*(float)(XMAX-group[n].x); q[number1].x=XMAX; 
				q[number1++].y=(int)y;
			}
			if(group[n].x>=XMAX&&group[n+1].x<XMAX)//���������㡢��� 
			{ 
				y=group[n].y+(float)(group[n+1].y-group[n].y)/ 
					(float)(group[n+1].x-group[n].x)*(float)(XMAX-group[n].x); q[number1].x=XMAX; 
				q[number1++].y=(int)y; 
				q[number1++]=group[n+1]; 
			} 
		} 
		for(i=0;i<number1;i++) 
		{ 
			group[i]=q[i];
		}
		group[number1]=q[0];
		PointNum=number1;number1=0;
	}
	if(linecode==3)// y=YMIN 
	{ 
		for(int n=0;n<PointNum;n++) 
		{ 
			if(group[n].y<YMIN&&group[n+1].y<YMIN)//���⣬����� 
			{ 
			}
			if(group[n].y>=YMIN&&group[n+1].y>=YMIN)//��������� 
			{ 
				q[number1++]=group[n+1];
			}
			if(group[n].y>=YMIN&&group[n+1].y<YMIN)//���⣬������� 
			{ 
				x=group[n].x+(float)(group[n+1].x-group[n].x)/ 
					(float)(group[n+1].y-group[n].y)*(float)(YMIN-group[n].y); q[number1].x=(int)x; 
				q[number1++].y=YMIN;
			}
			if(group[n].y<YMIN&&group[n+1].y>=YMIN)//���������㡢��� 
			{ 
				x=group[n].x+(float)(group[n+1].x-group[n].x)/ 
					(float)(group[n+1].y-group[n].y)*(float)(YMIN-group[n].y); q[number1].x=(int)x; 
				q[number1++].y=YMIN; 
				q[number1++]=group[n+1]; 
			} 
		} 
		for(i=0;i<number1;i++) 
		{ 
			group[i]=q[i];
		}
		group[number1]=q[0];
		PointNum=number1;number1=0;
	}
} 




























//////////////////////////////////////////////////////////////////////////�е�ü� ����
bool CMy2012302590125Doc::LineIsOutOfWindow(int x1,int y1,int x2,int y2)
{
	if (x1 < XMIN && x2 < XMIN)
		return true;
	else if(x1 > XMAX && x2 > XMAX)
		return true;
	else if(y1 > YMAX && y2 > YMAX)
		return true;
	else if(y1 < YMIN && y2 < YMIN)
		return true;
	else
		return false;
}
bool CMy2012302590125Doc::PointIsOutOfWindow(int x,int y)
{
	if (x < XMIN)
		return true;
	else if(x > XMAX)
		return true;
	else if(y > YMAX)
		return true;
	else if(y < YMIN)
		return true;
	else
		return false;
}

POINT CMy2012302590125Doc::FindNearestPoint(int x1, int y1, int x2, int y2)
{                            //(x1,y1)����ʼ�˵㣬(x2,y2)���յ�
	int x=0, y=0;
	POINT p;
	if(!PointIsOutOfWindow(x1,y1))//������ɼ���ֱ�ӷ������
	{
		p.x=x1;
		p.y=y1;
		return p;
	}

	while (!(abs(x1 - x2) <=1 && abs(y1 - y2) <= 1))
	{       //�ж��Ƿ����յ��㹻����
		x = (x1 + x2) / 2; y = (y1 + y2) / 2;
		if (LineIsOutOfWindow(x1, y1, x, y))
		{
			x1 = x; y1 = y;//���⣬��ʼ���Ƶ��е�
		}
		else
		{
			x2 = x; y2 = y;//�����⣬�յ��Ƶ��е�
		}
	}


	if (PointIsOutOfWindow(x1, y1))
	{
		p.x = x2; p.y = y2;//��ʼ�����⣬�����յ�
	}
	else
	{
		p.x = x1; p.y = y1;//���򣬷�����ʼ��
	}
	return p;
}


