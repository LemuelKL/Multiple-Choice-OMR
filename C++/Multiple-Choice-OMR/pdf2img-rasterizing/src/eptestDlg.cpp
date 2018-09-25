// eptestDlg.cpp : implementation file
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

#define MY_PROG_ID		_T("easyPDF.PDFConverter.6")

/////////////////////////////////////////////////////////////////////////////
// CEptestDlg dialog

CEptestDlg::CEptestDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CEptestDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CEptestDlg)
	m_nImageFormat = 0;
	//}}AFX_DATA_INIT
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

/////////////////////////////////////////////////////////////////////////////

void CEptestDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CEptestDlg)
	DDX_CBIndex(pDX, IDC_COMBO_IMAGE_FORMAT, m_nImageFormat);
	//}}AFX_DATA_MAP
}

/////////////////////////////////////////////////////////////////////////////

BEGIN_MESSAGE_MAP(CEptestDlg, CDialog)
	//{{AFX_MSG_MAP(CEptestDlg)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_CONVERT, OnButtonConvert)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CEptestDlg message handler

BOOL CEptestDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	SetIcon(m_hIcon, TRUE);
	SetIcon(m_hIcon, FALSE);

	EasyPDFConverter::IPDFConverterPtr pConverter = NULL;

	try
	{
		pConverter.CreateInstance(MY_PROG_ID);

		long nVerMajor = pConverter->GetLibraryVersionMajor();
		long nVerMinor = pConverter->GetLibraryVersionMinor();

		CString sVersion;
		sVersion.Format(_T("%d.%d"), nVerMajor, nVerMinor);

		GetDlgItem(IDC_STATIC_LIBRARY_VERSION)->SetWindowText(sVersion);
	}
	catch(_com_error &err)
	{
		GetDlgItem(IDC_STATIC_LIBRARY_VERSION)->SetWindowText((LPCTSTR)err.Description());
	}

	return TRUE;
}

/////////////////////////////////////////////////////////////////////////////

void CEptestDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this);

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

/////////////////////////////////////////////////////////////////////////////

HCURSOR CEptestDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

/////////////////////////////////////////////////////////////////////////////

void CEptestDlg::OnButtonConvert()
{
	if(!UpdateData(TRUE))
		return;

	HRESULT hr = NULL;

	CString sInFileName;
	CString sOutFileName;

	if(_GetFileNames(sInFileName, sOutFileName))
	{
		EasyPDFConverter::IPDFConverterPtr pConverter;
		EasyPDFConverter::IPDF2ImagePtr pPDF2Image;

		GetDlgItem(IDC_STATIC_STATUS)->SetWindowText(_T("Preparing..."));
		GetDlgItem(IDC_STATIC_FILENAME)->SetWindowText(_T("n/a"));

		try
		{
			hr = pConverter.CreateInstance(MY_PROG_ID);
			if(FAILED(hr))
			{
				AfxMessageBox(_T("CreateInstance failed!"), MB_OK | MB_ICONINFORMATION);
			}
			else
			{
				pPDF2Image = pConverter->GetPDF2Image();
				SetPDFConverterMonitor(pConverter->GetPDFConverterMonitor());

				switch(m_nImageFormat)
				{
				case 0:
				default:
					pPDF2Image->PutImageFormat(EasyPDFConverter::CNV_IMAGE_FMT_BMP);
					break;
				case 1:
					pPDF2Image->PutImageFormat(EasyPDFConverter::CNV_IMAGE_FMT_JPEG);
					break;
				case 2:
					pPDF2Image->PutImageFormat(EasyPDFConverter::CNV_IMAGE_FMT_PNG);
					break;
				case 3:
					pPDF2Image->PutImageFormat(EasyPDFConverter::CNV_IMAGE_FMT_GIF);
					break;
				case 4:
					pPDF2Image->PutImageFormat(EasyPDFConverter::CNV_IMAGE_FMT_TIFF);
					pPDF2Image->PutTiffMultiPage(VARIANT_TRUE);
					pPDF2Image->PutTiffCompression(EasyPDFConverter::CNV_TIFF_COMPRESSION_PACKBITS);
					break;
				}

				pPDF2Image->PutImageQuality(85);
				pPDF2Image->PutImageResolution(100);
				pPDF2Image->PutImageColor(EasyPDFConverter::CNV_IMAGE_CLR_24BIT);
				pPDF2Image->PutPageNumberSeparator(_T("_"));

				pPDF2Image->Convert((LPCTSTR)sInFileName, (LPCTSTR)sOutFileName);

				AfxMessageBox(_T("Success!"), MB_OK | MB_ICONINFORMATION);
			}
		}
		catch(_com_error &err)
		{
			// Handle error
			AfxMessageBox(err.Description(), MB_OK | MB_ICONINFORMATION);
		}

		SetPDFConverterMonitor(NULL);
	}
}

/////////////////////////////////////////////////////////////////////////////

long CEptestDlg::OnInit()
{
	CString msg;

	msg = _T("Converter initialized");
	GetDlgItem(IDC_STATIC_STATUS)->SetWindowText(msg);

	return EasyPDFConverter::CNV_MON_CONTINUE_CONVERSION;
}

/////////////////////////////////////////////////////////////////////////////

long CEptestDlg::OnPageStart(long PageNumber, long PageCount, _bstr_t FileName)
{
	CString msg;

	msg.Format(_T("Converting page %u of %u"), (PageNumber + 1), PageCount);
	GetDlgItem(IDC_STATIC_STATUS)->SetWindowText(msg);

	msg = (LPCTSTR)FileName;
	GetDlgItem(IDC_STATIC_FILENAME)->SetWindowText(msg);

	return EasyPDFConverter::CNV_MON_CONTINUE_CONVERSION;
}

/////////////////////////////////////////////////////////////////////////////

long CEptestDlg::OnEnd(EasyPDFConverter::cnvResult Result)
{
	CString msg;

	msg.Format(_T("Converter Ended (%u)"), Result);
	GetDlgItem(IDC_STATIC_STATUS)->SetWindowText(msg);

	return EasyPDFConverter::CNV_MON_CONTINUE_CONVERSION;
}

/////////////////////////////////////////////////////////////////////////////

BOOL CEptestDlg::_GetFileNames(CString &sInFileName, CString &sOutFileName)
{
	CString sFilter;
	CString sExt;

	CFileDialog dlgOpen(TRUE, _T(".pdf"), NULL,
						OFN_HIDEREADONLY | OFN_PATHMUSTEXIST,
						_T("PDF Files (*.pdf)|*.pdf||"), AfxGetMainWnd());

	if(dlgOpen.DoModal() != IDOK)
		return FALSE;

	sInFileName = dlgOpen.GetPathName();

	switch(m_nImageFormat)
	{
	case 0:
	default:
		sFilter = _T("BMP Files (*.bmp)|*.bmp||");
		sExt = _T(".bmp");
		break;
	case 1:
		sFilter = _T("JPEG Files (*.jpg)|*.jpg||");
		sExt = _T(".jpg");
		break;
	case 2:
		sFilter = _T("PNG Files (*.png)|*.png||");
		sExt = _T(".png");
		break;
	case 3:
		sFilter = _T("GIF Files (*.gif)|*.gif||");
		sExt = _T(".gif");
		break;
	case 4:
		sFilter = _T("TIFF Files (*.tif)|*.tif||");
		sExt = _T(".tif");
		break;
	}

	CFileDialog dlgSave(TRUE, sExt, (dlgOpen.GetFileTitle() + sExt),
						OFN_HIDEREADONLY | OFN_PATHMUSTEXIST | OFN_OVERWRITEPROMPT,
						sFilter, AfxGetMainWnd());

	if(dlgSave.DoModal() != IDOK)
		return FALSE;

	sOutFileName = dlgSave.GetPathName();

	return TRUE;
}

//////////////////////////////////////////////////////////////////////
