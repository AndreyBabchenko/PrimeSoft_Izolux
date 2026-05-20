#pragma once

#define GLASS_EXPORT __declspec(dllimport)

#include "..\..\Version\Defines_WINVER.h"

#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions

#ifndef _AFX_NO_OLE_SUPPORT
#include <afxole.h>         // MFC OLE classes
#include <afxodlgs.h>       // MFC OLE dialog classes
#include <afxdisp.h>        // MFC Automation classes
#endif // _AFX_NO_OLE_SUPPORT

#ifndef _AFX_NO_DB_SUPPORT
#include <afxdb.h>                      // MFC ODBC database classes
#endif

#ifndef _AFX_NO_DAO_SUPPORT
#include <afxdao.h>                     // MFC DAO database classes
#endif

#ifndef _AFX_NO_OLE_SUPPORT
#include <afxdtctl.h>           // MFC support for Internet Explorer 4 Common Controls
#endif

#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>                     // MFC support for Windows Common Controls
#endif
/*
#ifdef _DEBUG
#import "..\..\Bin\Debug\ABMfc.tlb" \
  rename ("EOF", "IsEOF")
#import "..\..\Bin\Debug\Glass.tlb" \
  rename ("EOF",          "IsEOF") \
  rename ("MessageBoxEx", "MessageBoxEx_")
#else
#import "..\..\Bin\Release\ABMfc.tlb" \
  rename ("EOF", "IsEOF")
#import "..\..\Bin\Release\Glass.tlb" \
  rename ("EOF",          "IsEOF") \
  rename ("MessageBoxEx", "MessageBoxEx_")
#endif
*/
#if _MSC_VER < 1920    // https://docs.microsoft.com/en-us/cpp/preprocessor/predefined-macros?view=vs-2019
                       // Версия Visual Studio менее 2019 16.0
#  ifdef _DEBUG
#    define PATH_ABMFC_TLB "..\..\Bin\Debug\ABMfc.tlb"
#    define PATH_GLASS_TLB "..\..\Bin\Debug\Glass.tlb"
#  else
#    define PATH_ABMFC_TLB "..\..\Bin\Release\ABMfc.tlb"
#    define PATH_GLASS_TLB "..\..\Bin\Release\Glass.tlb"
#  endif

#else
#  ifdef _DEBUG
#    ifdef _WIN64
#      define PATH_ABMFC_TLB  "..\..\Bin\Debug_x64_16\ABMfc.tlb"
#      define PATH_GLASS_TLB  "..\..\Bin\Debug_x64_16\Glass.tlb"
#    else   
#      define PATH_ABMFC_TLB  "..\..\Bin\Debug_16\DBOper.tlb"
#      define PATH_GLASS_TLB  "..\..\Bin\Debug_16\Glass.tlb"
#    endif
#  else
#    define PATH_ABMFC_TLB "..\..\Bin\Release_x64_16\ABMfc.tlb"
#    define PATH_GLASS_TLB "..\..\Bin\Release_x64_16\Glass.tlb"
#  endif
#endif

//#import "..\..\Design\Model\Debug\Model.tlb"

#import PATH_ABMFC_TLB \
                       rename ("EOF",          "IsEOF")
#import PATH_GLASS_TLB \
                       rename ("EOF",          "IsEOF") \
                       rename ("MessageBox",   "MessageBox_"  ) \
                       rename ("MessageBoxEx", "MessageBoxEx_")
