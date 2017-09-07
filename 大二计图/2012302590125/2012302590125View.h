// 2012302590125View.h : interface of the CMy2012302590125View class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_2012302590125VIEW_H__BEFEE46E_B5CB_4894_99B9_65B2248B498D__INCLUDED_)
#define AFX_2012302590125VIEW_H__BEFEE46E_B5CB_4894_99B9_65B2248B498D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CMy2012302590125View : public CView
{
protected: // create from serialization only

	int MenuID,PressNum,SaveNumber;
	CPoint mPointOrign, mPointOld,mPointOld1; 

	CMy2012302590125View();
	DECLARE_DYNCREATE(CMy2012302590125View)

// Attributes
public:
	CMy2012302590125Doc* GetDocument();

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMy2012302590125View)
	public:
	virtual void OnDraw(CDC* pDC);  // overridden to draw this view
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	protected:
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CMy2012302590125View();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CMy2012302590125View)
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnDrawDdaline();
	afx_msg void OnDrawBcircle();
	afx_msg void OnDrawPNcircle();
	afx_msg void OnCurveBezier();
	afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
	afx_msg void OnDrawChar();
	afx_msg void OnTransMove();
	afx_msg void OnDrawMidline();
	afx_msg void OnTransSymmetry();
	afx_msg void OnFillSeed();
	afx_msg void OnEdgeFill();
	afx_msg void OnFillScanline();
	afx_msg void OnCutCs();
	afx_msg void OnCutPolygon();
	afx_msg void OnOnCutCircle();
	afx_msg void OnCutMiddle();
	afx_msg void OnCutLiang();
	afx_msg void OnTransRotate();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in 2012302590125View.cpp
inline CMy2012302590125Doc* CMy2012302590125View::GetDocument()
   { return (CMy2012302590125Doc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_2012302590125VIEW_H__BEFEE46E_B5CB_4894_99B9_65B2248B498D__INCLUDED_)
