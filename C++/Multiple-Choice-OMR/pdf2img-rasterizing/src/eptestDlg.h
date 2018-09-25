// eptestDlg.h :
//

#if !defined(AFX_EPTESTDLG_H__4300D2B9_6127_4B7D_A15C_C05641F30432__INCLUDED_)
#define AFX_EPTESTDLG_H__4300D2B9_6127_4B7D_A15C_C05641F30432__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "PDFConverterMonitorWrapper.h"

/////////////////////////////////////////////////////////////////////////////
// CEptestDlg dialog

class CEptestDlg : public CDialog, CPDFConverterMonitorWrapper
{
public:
	CEptestDlg(CWnd* pParent = NULL);

	//{{AFX_DATA(CEptestDlg)
	enum { IDD = IDD_EPTEST_DIALOG };
	int		m_nImageFormat;
	//}}AFX_DATA

	//{{AFX_VIRTUAL(CEptestDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	//}}AFX_VIRTUAL

protected:
	HICON m_hIcon;

	//{{AFX_MSG(CEptestDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnButtonConvert();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

public:
	// overrides
	virtual long OnInit();
	virtual long OnPageStart(long PageNumber, long PageCount, _bstr_t FileName);
	virtual long OnEnd(EasyPDFConverter::cnvResult Result);

private:
	BOOL _GetFileNames(CString &sInFileName, CString &sOutFileName);
};

//{{AFX_INSERT_LOCATION}}

#endif // !defined(AFX_EPTESTDLG_H__4300D2B9_6127_4B7D_A15C_C05641F30432__INCLUDED_)
