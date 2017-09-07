// 2012302590125.h : main header file for the 2012302590125 application
//

#if !defined(AFX_2012302590125_H__937BCCB2_4D7C_46F6_8A9D_FF1462AC8901__INCLUDED_)
#define AFX_2012302590125_H__937BCCB2_4D7C_46F6_8A9D_FF1462AC8901__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols
#include "math.h" 

/////////////////////////////////////////////////////////////////////////////
// CMy2012302590125App:
// See 2012302590125.cpp for the implementation of this class
//

class CMy2012302590125App : public CWinApp
{
public:
	CMy2012302590125App();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMy2012302590125App)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CMy2012302590125App)
	afx_msg void OnAppAbout();
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_2012302590125_H__937BCCB2_4D7C_46F6_8A9D_FF1462AC8901__INCLUDED_)
