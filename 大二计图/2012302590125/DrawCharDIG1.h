#if !defined(AFX_DRAWCHARDIG1_H__1A4B7AE7_9216_44A6_AA5C_67126DA08C10__INCLUDED_)
#define AFX_DRAWCHARDIG1_H__1A4B7AE7_9216_44A6_AA5C_67126DA08C10__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// DrawCharDIG1.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CDrawCharDIG dialog

class CDrawCharDIG : public CDialog
{
// Construction
public:
	CDrawCharDIG(CWnd* pParent = NULL);   // standard constructor

	CFont m_fnt;//保存字体
	COLORREF m_clrText;//保存颜色


// Dialog Data
	//{{AFX_DATA(CDrawCharDIG)
	enum { IDD = IDD__CHAR_DIALOG };
	CString	m_strString;
	int		m_nX;
	int		m_nY;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDrawCharDIG)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CDrawCharDIG)
	afx_msg void OnButtonFont();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DRAWCHARDIG1_H__1A4B7AE7_9216_44A6_AA5C_67126DA08C10__INCLUDED_)
