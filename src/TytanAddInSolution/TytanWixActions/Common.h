#pragma once

#include <windows.h>
#include <msi.h>
#include <msiquery.h>
#include <tchar.h>


#ifdef __cplusplus
#define MSI_EXTERN_C extern "C"
#define MSI_STD_CALL __stdcall
#else
#define MSI_EXTERN_C
#define MSI_STD_CALL __stdcall
#endif


#pragma comment(lib, "msi.lib")
