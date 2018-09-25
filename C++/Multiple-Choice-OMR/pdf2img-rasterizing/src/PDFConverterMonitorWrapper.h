// PDFConverterMonitorWrapper.h: CPDFConverterMonitorWrapper class interface
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_PDFCONVERTERMONITORWRAPPER_H__AD2C7D33_D77F_4A68_AA42_2E7BF173CB09__INCLUDED_)
#define AFX_PDFCONVERTERMONITORWRAPPER_H__AD2C7D33_D77F_4A68_AA42_2E7BF173CB09__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#import "C:\Program Files\Common Files\BCL Technologies\easyPDF 6\bepconv.dll" named_guids

class CPDFConverterMonitorWrapper;

const int ID_PDFCONVERTERMONITOR = 1;
const int SDK_VERSION_MAJOR = 6;
const int SDK_VERSION_MINOR = 0;

typedef IDispEventImpl<ID_PDFCONVERTERMONITOR,
					   CPDFConverterMonitorWrapper,
					   &__uuidof(EasyPDFConverter::_IPDFConverterMonitorEvents),
					   &EasyPDFConverter::LIBID_EasyPDFConverter,
					   SDK_VERSION_MAJOR,
					   SDK_VERSION_MINOR> MyPDFConverterMonitorEvents;

class CPDFConverterMonitorWrapper : public MyPDFConverterMonitorEvents
{
public:
	// constructor/destructor
	CPDFConverterMonitorWrapper();
	virtual ~CPDFConverterMonitorWrapper();

	// set IPDFConverterMonitor
	void SetPDFConverterMonitor(EasyPDFConverter::IPDFConverterMonitorPtr pPDFConverterMonitor);
	EasyPDFConverter::IPDFConverterMonitorPtr GetPDFConverterMonitor();

public:
	//////////////////////////////////////////////
	// Virtual functions. Override the ones you
	// want to receive the events from

	virtual long OnInit();
	virtual long OnPageStart(long PageNumber, long PageCount, _bstr_t FileName);
	virtual long OnEnd(EasyPDFConverter::cnvResult Result);

	//////////////////////////////////////////////
	// Sink interface declaration

	BEGIN_SINK_MAP(CPDFConverterMonitorWrapper)
		SINK_ENTRY_EX(ID_PDFCONVERTERMONITOR, __uuidof(EasyPDFConverter::_IPDFConverterMonitorEvents), 1, __OnInit)
		SINK_ENTRY_EX(ID_PDFCONVERTERMONITOR, __uuidof(EasyPDFConverter::_IPDFConverterMonitorEvents), 2, __OnPageStart)
		SINK_ENTRY_EX(ID_PDFCONVERTERMONITOR, __uuidof(EasyPDFConverter::_IPDFConverterMonitorEvents), 3, __OnEnd)
	END_SINK_MAP()

	//////////////////////////////////////////////
	// COM event hookup
public:
	STDMETHOD_(long, __OnInit)();
	STDMETHOD_(long, __OnPageStart)(long PageNumber, long PageCount, BSTR FileName);
	STDMETHOD_(long, __OnEnd)(long Result);

private:
	void _Release();

private:
	EasyPDFConverter::IPDFConverterMonitorPtr m_pPDFConverterMonitor;
};

#endif // !defined(AFX_PDFCONVERTERMONITORWRAPPER_H__AD2C7D33_D77F_4A68_AA42_2E7BF173CB09__INCLUDED_)
