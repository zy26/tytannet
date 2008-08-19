// Guids.cs
// MUST match guids.h
using System;

namespace Pretorianie.Tytan
{
    public static class GuidList
    {
        public const string guidCommandSetString = "6f4de5f2-f501-4d06-9214-700098f08CCC";
        public static readonly Guid guidCmdSet = new Guid(guidCommandSetString);

        public const string guidToolWindow_DebugView = "6f4de5f2-f501-4d06-9214-700098f08C00";
        public const string guidToolWindow_RegistryView = "6f4de5f2-f501-4d06-9214-700098f08C01";
        public const string guidToolWindow_EnvironmentVarView = "6f4de5f2-f501-4d06-9214-700098f08C02";
        public const string guidToolWindow_TypeFinder = "6f4de5f2-f501-4d06-9214-700098f08C03";
        public const string guidToolWindow_NativeImagePreview = "6f4de5f2-f501-4d06-9214-700098f08C04";
    };
}