// 2012302590125Doc.h : interface of the CMy2012302590125Doc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_2012302590125DOC_H__AFBF8CC4_A558_4DF3_945B_D3373D5BFBC8__INCLUDED_)
#define AFX_2012302590125DOC_H__AFBF8CC4_A558_4DF3_945B_D3373D5BFBC8__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CMy2012302590125Doc : public CDocument
{
protected: // create from serialization only
	CMy2012302590125Doc();
	DECLARE_DYNCREATE(CMy2012302590125Doc)

// Attributes
public:
    CPoint group[100];	//定义数组，
	int PointNum; 
	COLORREF m_crColor;//保存图形颜色


// Operations
public:
   void DDALine(CClientDC *DCPoint); //定义函数 
   void MidLine(CClientDC *DCPoint);

   void BCircle(CClientDC *DCPoint,CPoint p1,CPoint p2); 
   void BCircle(CClientDC *DCPoint, CRect* rc, CPoint p1,CPoint p2); 
   
   void PNCircle(CClientDC *DCPoint,CPoint p1,CPoint p2); 
   void Bezier(CClientDC *DCPoint,int mode); 
   void Bezier_4(CClientDC *DCPoint,int mode,CPoint p1,CPoint p2,CPoint p3,CPoint p4); 

   void GenerateGraph(CClientDC *DCPoint); 
   void DrawGraph(CClientDC *DCPoint); 


   void Symmetry(CPoint p1,CPoint p2); 
   void SeedFill(CClientDC *DCPoint,CPoint p);

   void EdgeFill(CClientDC *DCPoint); 
   void DrawWindow(CClientDC *DCPoint); 

   void CohenSutherland(CClientDC *DCPoint,CPoint p1,CPoint p2); 
   void CutMiddle(CClientDC *DCPoint,CPoint p1,CPoint p2); 
   void CutLiang(CClientDC *DCPoint,CPoint p1,CPoint p2); 

   int encode(int x,int y); 
   void PolygonCut(CClientDC *DCPoint); void EdgeClipping(int linecode); 



   void CircleCut(CClientDC *DCPoint,CPoint p1,CPoint p2);



/////////.............中点裁剪函数
private: bool LineIsOutOfWindow(int x1,int y1,int x2,int y2);
 
       bool PointIsOutOfWindow(int x,int y);
	   POINT FindNearestPoint(int x1, int y1, int x2, int y2);


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMy2012302590125Doc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CMy2012302590125Doc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CMy2012302590125Doc)
	afx_msg void OnGraphColor();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_2012302590125DOC_H__AFBF8CC4_A558_4DF3_945B_D3373D5BFBC8__INCLUDED_)
