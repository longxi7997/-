// DrawCharDIG1.cpp : implementation file
//

#include "stdafx.h"
#include "2012302590125.h"
#include "DrawCharDIG1.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDrawCharDIG dialog


CDrawCharDIG::CDrawCharDIG(CWnd* pParent /*=NULL*/)
	: CDialog(CDrawCharDIG::IDD, pParent)
{
	//{{AFX_DATA_INIT(CDrawCharDIG)
	m_strString = _T("");
	m_nX = 0;
	m_nY = 0;
	m_clrText=RGB(0,0,0);
	//}}AFX_DATA_INIT
}


void CDrawCharDIG::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDrawCharDIG)
	DDX_Text(pDX, IDC_EDIT_STRING, m_strString);
	DDX_Text(pDX, IDC_EDIT_X, m_nX);
	DDX_Text(pDX, IDC_EDIT_Y, m_nY);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CDrawCharDIG, CDialog)
	//{{AFX_MSG_MAP(CDrawCharDIG)
	ON_BN_CLICKED(IDC_BUTTON_FONT, OnButtonFont)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDrawCharDIG message handlers

void CDrawCharDIG::OnButtonFont() 
{
	// TODO: Add your control notification handler code here
	CFontDialog dlg;
	if(dlg.DoModal()==IDOK)
	{
		m_fnt.DeleteObject();
		LOGFONT LogFnt;
		dlg.GetCurrentFont(&LogFnt);//保存所选字体
		m_fnt.CreateFontIndirect(&LogFnt);//创建所选字体
		m_clrText=dlg.GetColor();//获得所选颜色
	}

}
