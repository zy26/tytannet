#include "Common.h"


///////////////////////////////////////////////////////////////////////////////

#pragma comment(linker, "/EXPORT:RegisterGeneratorAssembly=_RegisterGeneratorAssembly@4")

#define MAX_COMMAND			1024

///////////////////////////////////////////////////////////////////////////////

static TCHAR g_lpCommand[MAX_COMMAND] = _T("");

///////////////////////////////////////////////////////////////////////////////

UINT MSI_STD_CALL RegisterAssembly ( LPCTSTR lpszRegAsmPath, LPCTSTR lpszAssembly, BOOL bRegister )
{
	STARTUPINFO suInfo;
	PROCESS_INFORMATION procInfo;

	wsprintf ( g_lpCommand, _T("\"%s\" %s \"%s\""), lpszRegAsmPath,
		( bRegister ? _T("/codebase"): _T("/unregister") ),
		lpszAssembly );

	// _tsystem ( g_lpCommand );

	// start process without console window:
	memset ( &suInfo, 0, sizeof(suInfo) );
	suInfo.cb			= sizeof(suInfo);
	suInfo.dwFlags		= STARTF_USESHOWWINDOW;
	suInfo.wShowWindow	= SW_HIDE;

	CreateProcess ( lpszRegAsmPath, g_lpCommand,
		 NULL, NULL, FALSE, NORMAL_PRIORITY_CLASS, NULL, NULL, 
		 &suInfo, &procInfo);

	// wait for external process to finish:
	WaitForSingleObject ( procInfo.hProcess, 10 * 1000 );

	return ERROR_SUCCESS;
}

MSI_EXTERN_C UINT MSI_STD_CALL RegisterGeneratorAssembly ( MSIHANDLE hInstaller )
{
	TCHAR lpRegAsmPath[MAX_PATH];
	TCHAR lpAssemblyPath[MAX_PATH];
	TCHAR lpAssemblyFileName[32];
	DWORD dwDataLength;
	INSTALLSTATE  featureInstalled = INSTALLSTATE_NOTUSED;
	INSTALLSTATE  featureAction = INSTALLSTATE_NOTUSED;

	// check if given feature is enabled:
	if ( MsiGetFeatureState ( hInstaller, _T("CodeGeneratorsFeature"), &featureInstalled, &featureAction ) != ERROR_SUCCESS )
		return ERROR_NOT_READY;

	// get paths:
	dwDataLength = MAX_PATH;
	if ( MsiGetProperty ( hInstaller, _T("REGASMPATH"), &lpRegAsmPath[0], &dwDataLength ) != ERROR_SUCCESS )
		return ERROR_INVALID_NAME;
	dwDataLength = MAX_PATH;
	if ( MsiGetProperty ( hInstaller, _T("INSTALLLOCATION"), &lpAssemblyPath[0], &dwDataLength ) != ERROR_SUCCESS )
		return ERROR_INVALID_NAME;
	dwDataLength = 32;
	if ( MsiGetProperty ( hInstaller, _T("AddInGeneratorsName"), &lpAssemblyFileName[0], &dwDataLength ) != ERROR_SUCCESS )
		return ERROR_INVALID_NAME;

	_tcscat_s ( lpAssemblyPath, MAX_PATH, lpAssemblyFileName );

	if ( featureAction == INSTALLSTATE_LOCAL )
		return RegisterAssembly ( lpRegAsmPath, lpAssemblyPath, TRUE );
	else
		if ( featureAction == INSTALLSTATE_REMOVED || featureAction == INSTALLSTATE_ABSENT )
			return RegisterAssembly ( lpRegAsmPath, lpAssemblyPath, FALSE );

	return ERROR_SUCCESS;
}