#include "Common.h"

///////////////////////////////////////////////////////////////////////////////
#define TRANSFORM_MAX			2 * 1024

#pragma comment(linker, "/EXPORT:CreateAddInFile=_CreateAddInFile@4")

///////////////////////////////////////////////////////////////////////////////

static TCHAR g_lpFileContent_1[] = _T("<?xml version=\"1.0\" encoding=\"UTF-16\" standalone=\"no\"?>\r\n\
<Extensibility xmlns=\"http://schemas.microsoft.com/AutomationExtensibility\">\r\n\
	<HostApplication>\r\n\
		<Name>Microsoft Visual Studio</Name>\r\n\
		<Version>");

static TCHAR g_lpFileContent_2[] = _T("</Version>\r\n\
	</HostApplication>\r\n\
	<Addin>\r\n\
		<FriendlyName>@AddIn_FriendlyName</FriendlyName>\r\n\
		<Description>@AddIn_Description</Description>\r\n\
		<AboutBoxDetails>@AddIn_AboutBox</AboutBoxDetails>\r\n\
		<AboutIconData>@AddIn_DefaultIcon</AboutIconData>\r\n\
		<Assembly>");

static TCHAR g_lpFileContent_3[] = _T("</Assembly>\r\n\
		<FullClassName>Pretorianie.Tytan.Connect</FullClassName>\r\n\
		<LoadBehavior>1</LoadBehavior>\r\n\
		<CommandPreload>1</CommandPreload>\r\n\
		<CommandLineSafe>0</CommandLineSafe>\r\n\
	</Addin>\r\n\
</Extensibility>\r\n");

static TCHAR g_lpTransform[TRANSFORM_MAX] = _T("");

///////////////////////////////////////////////////////////////////////////////

UINT MSI_STD_CALL WriteAddInFile ( LPCTSTR lpszFileName, LPCTSTR lpszAddInDllLocation, LPCTSTR lpszVisualStudioVersion )
{
	HANDLE hFile;
	DWORD dwWritten;
	BYTE aBOM[2] = { 0xFF, 0xFE };
	
	hFile = CreateFile ( lpszFileName, GENERIC_WRITE, FILE_SHARE_WRITE, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL );

	// write file content:
	if ( hFile != INVALID_HANDLE_VALUE && hFile != NULL )
	{
		_tcscat_s ( g_lpTransform,TRANSFORM_MAX, g_lpFileContent_1 );
		_tcscat_s ( g_lpTransform, TRANSFORM_MAX, lpszVisualStudioVersion );
		_tcscat_s ( g_lpTransform, TRANSFORM_MAX, g_lpFileContent_2 );
		_tcscat_s ( g_lpTransform, TRANSFORM_MAX, lpszAddInDllLocation );
		_tcscat_s ( g_lpTransform, TRANSFORM_MAX, g_lpFileContent_3 );

		// write UTF-8 BOM:
		WriteFile ( hFile, &aBOM[0], 2, &dwWritten, NULL );
		WriteFile ( hFile, g_lpTransform, (DWORD)(sizeof(TCHAR) * _tcslen (g_lpTransform)), &dwWritten, NULL );
		CloseHandle ( hFile );
		return ERROR_SUCCESS;
	}

	return ERROR_FILE_INVALID;
}

/*
 * Creates *.AddIn file for TytanNET with proper checks if given feature was selected to install.
 */
MSI_EXTERN_C UINT MSI_STD_CALL CreateAddInFile ( MSIHANDLE hInstaller )
{
	TCHAR lpAddInPath[MAX_PATH];
	TCHAR lpAddInFileName[32];
	TCHAR lpAssemblyPath[MAX_PATH];
	TCHAR lpAssemblyFileName[32];
	DWORD dwDataLength;
	INSTALLSTATE  featureInstalled = INSTALLSTATE_NOTUSED;
	INSTALLSTATE  featureAction = INSTALLSTATE_NOTUSED;

	// check if given feature is enabled:
	if ( MsiGetFeatureState ( hInstaller, _T("CoreIntegrationFeature"), &featureInstalled, &featureAction ) != ERROR_SUCCESS )
		return ERROR_NOT_READY;

	if ( featureAction == INSTALLSTATE_LOCAL )
	{
		// get paths:
		dwDataLength = MAX_PATH;
		if ( MsiGetProperty ( hInstaller, _T("AddInsInstallLocation"), &lpAddInPath[0], &dwDataLength ) != ERROR_SUCCESS )
			return ERROR_INVALID_NAME;
		dwDataLength = 32;
		if ( MsiGetProperty ( hInstaller, _T("AddInFileName"), &lpAddInFileName[0], &dwDataLength ) != ERROR_SUCCESS )
			return ERROR_INVALID_NAME;
		dwDataLength = MAX_PATH;
		if ( MsiGetProperty ( hInstaller, _T("INSTALLLOCATION"), &lpAssemblyPath[0], &dwDataLength ) != ERROR_SUCCESS )
			return ERROR_INVALID_NAME;
		dwDataLength = 32;
		if ( MsiGetProperty ( hInstaller, _T("AddInAssemblyName"), &lpAssemblyFileName[0], &dwDataLength ) != ERROR_SUCCESS )
			return ERROR_INVALID_NAME;

		_tcscat_s ( lpAddInPath, MAX_PATH, lpAddInFileName );
		_tcscat_s ( lpAssemblyPath, MAX_PATH, lpAssemblyFileName );

		return WriteAddInFile ( lpAddInPath, lpAssemblyPath, _T("8.0") );
	}

	return ERROR_SUCCESS;
}

