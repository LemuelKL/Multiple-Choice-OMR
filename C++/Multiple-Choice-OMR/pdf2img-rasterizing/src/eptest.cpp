// eptest.cpp :
//

#include "stdafx.h"
#include "eptest.h"
#include "eptestDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CEptestApp

BEGIN_MESSAGE_MAP(CEptestApp, CWinApp)
	//{{AFX_MSG_MAP(CEptestApp)
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////

CEptestApp::CEptestApp()
{
}

/////////////////////////////////////////////////////////////////////////////

CEptestApp theApp;

CComModule _Module;

/////////////////////////////////////////////////////////////////////////////

BOOL CEptestApp::InitInstance()
{
	_Module.Init(NULL, AfxGetInstanceHandle());

	// Initialize COM subsystem
	AfxOleInit();

	AfxEnableControlContainer();

	CCommandLineInfo cmdInfo;
	ParseCommandLine(cmdInfo);

	if (cmdInfo.m_bRunEmbedded || cmdInfo.m_bRunAutomated)
	{
		return TRUE;
	}

#ifdef _AFXDLL
	Enable3dControls();
#else
	Enable3dControlsStatic();
#endif

	CEptestDlg dlg;
	m_pMainWnd = &dlg;
	int nResponse = dlg.DoModal();
	if (nResponse == IDOK)
	{
	}
	else if (nResponse == IDCANCEL)
	{
	}

	return FALSE;
}

int CEptestApp::ExitInstance()
{
	_Module.Term();

	return CWinApp::ExitInstance();
}
