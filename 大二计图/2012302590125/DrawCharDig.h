#if !defined(AFX_DRAWCHARDIG_H__07D903A1_E1C0_4BF4_A3BA_EDDBC6C878E0__INCLUDED_)
#define AFX_DRAWCHARDIG_H__07D903A1_E1C0_4BF4_A3BA_EDDBC6C878E0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

//

/////////////////////////////////////////////////////////////////////////////
// CDrawCharDig dialog

class CDrawCharDig : public CDialog
{
// Construction
public:
   CDrawCharDlg(CWnd* pParent = NULL);   // standard constructor

public:
	CFont m_fnt;//保存字体
	COLORREF m_clrText;//保存颜色

	

// Dialog Data
	//{{AFX_DATA(CDrawCharDig)
	enum { IDD = IDD_DRW_CHAR };
	CString	m_strString;
	int		m_nX;
	int		m_nY;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDrawCharDig)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CDrawCharDig)
	afx_msg void OnButtonFont();
	afx_msg void OnDrawChar();
	//}}AFX_MSG

public:


	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DRAWCHARDIG_H__07D903A1_E1C0_4BF4_A3BA_EDDBC6C878E0__INCLUDED_)
