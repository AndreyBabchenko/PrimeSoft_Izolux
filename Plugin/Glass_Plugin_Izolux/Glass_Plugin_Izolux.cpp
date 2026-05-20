// Пример разработки плагина для IzoGlass
// https://ru.wikipedia.org/wiki/%D0%9F%D0%BB%D0%B0%D0%B3%D0%B8%D0%BD
//
#include "stdafx.h"
#include <vector>
#include "Glass_Plugin_Izolux.h"

#include "..\..\Design\ABMfc\ABCatch.h"
#include "..\..\Design\ABMfc\ABMfc_Export.h"
#include <Prof-UIS.h>
#include "..\..\Design\Glass\TaskView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

BEGIN_MESSAGE_MAP(CPlugin_Izolux, CWinApp)
END_MESSAGE_MAP()

CPlugin_Izolux::CPlugin_Izolux()
{
}

CPlugin_Izolux theApp;

// 055ABE9D-97EC-4f45-A0BE-75123E8EFBF3
const GUID CDECL BASED_CODE _tlid =
{ 0x55abe9d, 0x97ec, 0x4f45, { 0xa0, 0xbe, 0x75, 0x12, 0x3e, 0x8e, 0xfb, 0xf3 } };

const WORD _wVerMajor = 1;
const WORD _wVerMinor = 0;

BOOL CPlugin_Izolux::InitInstance()
{
  CWinApp::InitInstance();

  // Register all OLE server (factories) as running.  This enables the
  //  OLE libraries to create objects from other applications.
  COleObjectFactory::RegisterAll();

  return TRUE;
}

// Returns class factory
//
STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
  AFX_MANAGE_STATE(AfxGetStaticModuleState());
  return AfxDllGetClassObject(rclsid, riid, ppv);
}

// Allows COM to unload DLL
//
STDAPI DllCanUnloadNow(void)
{
  AFX_MANAGE_STATE(AfxGetStaticModuleState());
  return AfxDllCanUnloadNow();
}

// Adds entries to the system registry
//
STDAPI DllRegisterServer(void)
{
  AFX_MANAGE_STATE(AfxGetStaticModuleState());

  if ( !AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid) )
    return SELFREG_E_TYPELIB;

  if ( !COleObjectFactory::UpdateRegistryAll() )
    return SELFREG_E_CLASS;

  return S_OK;
}

// Removes entries from the system registry
//
STDAPI DllUnregisterServer(void)
{
  AFX_MANAGE_STATE(AfxGetStaticModuleState());

  if ( !AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor) )
    return SELFREG_E_TYPELIB;

  if ( !COleObjectFactory::UpdateRegistryAll(FALSE) )
    return SELFREG_E_CLASS;

  return S_OK;
}

/////////////////////////////////////////////////////////////////////////////

// Новая команда для меню, не должна пересекаться с любой из команд в программе:
#define ID_RECALC_DEPTRANS_DOCDATE 62000

/////////////////////////////////////////////////////////////////////////////
// Грид отображения заказов:

long nCommandIzolux = ID_RECALC_DEPTRANS_DOCDATE;

class CTaskViewGrid_Export : public CTaskViewGrid
{
public:
  CTaskViewGrid_Export(TCHAR* lpsz) : CTaskViewGrid(lpsz)
  {
    Glass::IProgEventPtr pIPE(GetProgEventDispatch());
    if ( pIPE )
      nCommandIzolux = pIPE->GetNextIDPluginCommand();
  }
  DECLARE_MESSAGE_MAP()

public:
  afx_msg void OnPopupRecalcDepTransDocDate()
  {
    try
    {
      _ConnectionPtr  pConn(__uuidof(Connection));
      pConn = m_Recordset->GetActiveConnection();

      if ( !pConn && GetConnectFunc() )
        pConn = GetConnectFunc()();

      CString        sSQL;
      sSQL.Format(_T("exec sp_UpdateDepTransDocDateByTask %lf, %lf"), m_pParent->m_dBegin, m_pParent->m_dEnd);
      RunSQL(sSQL, pConn.GetInterfacePtr());
    }
    CATCH_HIDE(__TFILE__, __LINE__, __TFUNCTION__)
  }
};

BEGIN_MESSAGE_MAP(CTaskViewGrid_Export, CTaskViewGrid)
  ON_COMMAND(nCommandIzolux, OnPopupRecalcDepTransDocDate)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// Добавление новой команды в контекстное меню подсистемы:

class CExtPopupMenuWnd_Export : public CExtPopupMenuWnd
{
public:
  virtual BOOL UpdateFromMenu(HWND hWndCmdRecv, CMenu* pBuildMenu, bool bPopupMenu = true, bool bTopLevel = true, bool bNoRefToCmdMngr = false)
  {
    BOOL  bRes = FALSE;
    if ( pBuildMenu )
    {
      CMenu* pPopup = pBuildMenu->GetSubMenu(0);

      bRes = pPopup->AppendMenu(MF_SEPARATOR);
      bRes = pPopup->AppendMenu(MF_STRING | MF_ENABLED, nCommandIzolux, _T("Рассчитать даты документов"));
    }
    return CExtPopupMenuWnd::UpdateFromMenu(hWndCmdRecv, pBuildMenu, bPopupMenu, bTopLevel, bNoRefToCmdMngr);
  }
};

/////////////////////////////////////////////////////////////////////////////

// Вернёт имена функций в данной *.dll

extern __declspec(dllexport) CObject* PluginCreateObject(const TCHAR* File, const TCHAR* szFunction, CRuntimeClass* pClassFunc, CRuntimeClass* pClassNew)
{
  if ( pClassNew == RUNTIME_CLASS(CExtPopupMenuWnd) && (CString)szFunction == "CTaskViewGrid::OnContextMenu" )
    return new CExtPopupMenuWnd_Export;

  if ( pClassNew == RUNTIME_CLASS(CTaskViewGrid) )
    return new CTaskViewGrid_Export(_T("CTaskView\\Grid"));

  return NULL;
}
