// PDFConverterMonitorWrapper.cpp: CPDFConverterMonitorWrapper class implementation
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "PDFConverterMonitorWrapper.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////

CPDFConverterMonitorWrapper::CPDFConverterMonitorWrapper()
: MyPDFConverterMonitorEvents()
{
}

//////////////////////////////////////////////////////////////////////

CPDFConverterMonitorWrapper::~CPDFConverterMonitorWrapper()
{
	_Release();
}

//////////////////////////////////////////////////////////////////////

void CPDFConverterMonitorWrapper::SetPDFConverterMonitor(EasyPDFConverter::IPDFConverterMonitorPtr pPDFConverterMonitor)
{
	HRESULT hr = NULL;

	_Release();

	if(pPDFConverterMonitor != NULL)
	{
		hr = MyPDFConverterMonitorEvents::DispEventAdvise(pPDFConverterMonitor, &__uuidof(EasyPDFConverter::_IPDFConverterMonitorEvents));
		if(FAILED(hr))
		{
			_com_raise_error(hr);
		}

		m_pPDFConverterMonitor = pPDFConverterMonitor;
	}
}

//////////////////////////////////////////////////////////////////////

EasyPDFConverter::IPDFConverterMonitorPtr CPDFConverterMonitorWrapper::GetPDFConverterMonitor()
{
	return m_pPDFConverterMonitor;
}

//////////////////////////////////////////////////////////////////////

void CPDFConverterMonitorWrapper::_Release()
{
	if(m_pPDFConverterMonitor != NULL)
	{
		MyPDFConverterMonitorEvents::DispEventUnadvise(m_pPDFConverterMonitor);
		m_pPDFConverterMonitor = NULL;
	}
}

//////////////////////////////////////////////////////////////////////

long __stdcall CPDFConverterMonitorWrapper::__OnInit()
{
	return OnInit();
}

//////////////////////////////////////////////////////////////////////

long __stdcall CPDFConverterMonitorWrapper::__OnPageStart(long PageNumber, long PageCount, BSTR FileName)
{
	return OnPageStart(PageNumber, PageCount, FileName);
}

//////////////////////////////////////////////////////////////////////

long __stdcall CPDFConverterMonitorWrapper::__OnEnd(long Result)
{
	return OnEnd(static_cast<EasyPDFConverter::cnvResult>(Result));
}

//////////////////////////////////////////////////////////////////////

long CPDFConverterMonitorWrapper::OnInit()
{
	return EasyPDFConverter::CNV_MON_CONTINUE_CONVERSION;
}

/////////////////////////////////////////////////////////////////////////////

long CPDFConverterMonitorWrapper::OnPageStart(long PageNumber, long PageCount, _bstr_t FileName)
{
	return EasyPDFConverter::CNV_MON_CONTINUE_CONVERSION;
}

/////////////////////////////////////////////////////////////////////////////

long CPDFConverterMonitorWrapper::OnEnd(EasyPDFConverter::cnvResult Result)
{
	return EasyPDFConverter::CNV_MON_CONTINUE_CONVERSION;
}

/////////////////////////////////////////////////////////////////////////////
