#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"

class CPlugin_Izolux : public CWinApp
{
public:
	CPlugin_Izolux();

public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
