// eptest.h :
//

#if !defined(AFX_EPTEST_H__8F497500_B7B9_4B4B_B4A5_92B229999513__INCLUDED_)
#define AFX_EPTEST_H__8F497500_B7B9_4B4B_B4A5_92B229999513__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"

/////////////////////////////////////////////////////////////////////////////
// CEptestApp:
//

class CEptestApp : public CWinApp
{
public:
	CEptestApp();

	//{{AFX_VIRTUAL(CEptestApp)
	public:
	virtual BOOL InitInstance();
		virtual int ExitInstance();
	//}}AFX_VIRTUAL


	//{{AFX_MSG(CEptestApp)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}

#endif // !defined(AFX_EPTEST_H__8F497500_B7B9_4B4B_B4A5_92B229999513__INCLUDED_)
