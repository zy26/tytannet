using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace Pretorianie.Tytan.Core.BaseGenerators
{
    /// <summary>
    /// This attribute describes a custom code generator.
    /// It is responsible for creation proper registry entries for it.
    /// 
    /// [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\8.0\Generators\
    ///  or
    /// [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\9.0\Generators\
    /// ...
    ///		{fae04ec1-301f-11d3-bf4b-00c04f79efbc}\CustomGenerator]
    ///			"CLSID"="{AAAABBBB-CCCC-DDDD-EEEE-FFFFAAAABBBB}"
    ///         "GeneratesDesignTimeSource" = '1'
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class CodeGeneratorRegistrationAttribute : Attribute
    {
        private readonly string generatorName;
        private readonly Type generatorType;

        private readonly string generatorRegistryName;
        private readonly Guid generatorGuid;

        private string description;

        private bool generatesDesignTimeSource = false;
        private bool generatesSharedDesignTimeSource = false;
        private bool generatesCSharpSourceCode = false;
        private bool generatesVBasicSourceCode = false;

        /// <summary>
        /// Defines new description for custom code generator.
        /// </summary>
        /// <param name="generatorName">The generator name.</param>
        public CodeGeneratorRegistrationAttribute(Type generatorType, string generatorName)
        {
            if (generatorType == null)
                throw new ArgumentNullException("generatorType");
            if (string.IsNullOrEmpty(generatorName))
                throw new ArgumentNullException("generatorName");

            this.generatorName = (string.IsNullOrEmpty(generatorName) ? generatorType.Name : generatorName);
            this.generatorType = generatorType;

            generatorRegistryName = this.generatorName;
            generatorGuid = generatorType.GUID;

            if(generatorGuid == Guid.Empty)
                throw new InvalidDataException("Generator type is not a COM-visible object.");
        }

        /// <summary>
        /// Get the generator Type
        /// </summary>
        public Type GeneratorType
        {
            get { return generatorType; }
        }

        /// <summary>
        /// Gets or sets if the generator should be used during design time inside Visual Studio.
        /// </summary>
        public bool GeneratesDesignTimeSource
        {
            get { return generatesDesignTimeSource; }
            set { generatesDesignTimeSource = value; }
        }

        /// <summary>
        /// Gets or sets the GeneratesSharedDesignTimeSource value
        /// </summary>
        public bool GeneratesSharedDesignTimeSource
        {
            get { return generatesSharedDesignTimeSource; }
            set { generatesSharedDesignTimeSource = value; }
        }

        /// <summary>
        /// Gets or sets the indication for what language register that code generator.
        /// </summary>
        public bool GeneratesCSharpSourceCode
        {
            get { return generatesCSharpSourceCode; }
            set { generatesCSharpSourceCode = value; }
        }

        /// <summary>
        /// Gets or sets the indication for what language register that code generator.
        /// </summary>
        public bool GeneratesVBasicSouceCode
        {
            get { return generatesVBasicSourceCode; }
            set { generatesVBasicSourceCode = value; }
        }

        /// <summary>
        /// Gets the name of the generator.
        /// </summary>
        public string GeneratorName
        {
            get { return generatorName; }
        }

        /// <summary>
        /// Gets or sets the description of the generator.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets the Generator reg key name under 
        /// </summary>
        private IList<string> GetGeneratorRegistryKeys(string vsVersion)
        {
            if (generatesCSharpSourceCode || generatesVBasicSourceCode)
            {
                int i = 0;
                string[] result =
                    new string[(generatesCSharpSourceCode ? 1 : 0) + (generatesVBasicSourceCode ? 1 : 0)];

                if (generatesCSharpSourceCode)
                    result[i++] = GetGeneratorRegistryKeyName(vsVersion, VsGuid.CSharpLanguage);

                if (generatesVBasicSourceCode)
                    result[i] = GetGeneratorRegistryKeyName(vsVersion, VsGuid.VBasicLanguage);

                return result;
            }

            return null;
        }

        /// <summary>
        /// Gets the base registry key name for current generator and given language.
        /// </summary>
        private string GetGeneratorRegistryKeyName(string vsVersion, string languageGuid)
        {
            return
                string.Format(CultureInfo.InvariantCulture, @"SOFTWARE\Microsoft\VisualStudio\{0}\Generators\{{{1}}}\{2}",
                              vsVersion, languageGuid, generatorRegistryName);
        }
        /// <summary>
        /// Registers current code generator inside system registry.
        /// </summary>
        public void Register(bool forCurrentUserOnly, string vsVersion)
        {
            IList<string> keyNames = GetGeneratorRegistryKeys(vsVersion);
            RegistryKey masterKey = (forCurrentUserOnly ? Registry.CurrentUser : Registry.LocalMachine);

            if (keyNames != null)
            {
                foreach (string keyName in keyNames)
                {
                    try
                    {
                        using (RegistryKey childKey = masterKey.CreateSubKey(keyName))
                        {
                            if (!string.IsNullOrEmpty(description))
                                childKey.SetValue(string.Empty, description);
                            childKey.SetValue("CLSID", generatorGuid.ToString("B").ToUpper());

                            if (generatesDesignTimeSource)
                                childKey.SetValue("GeneratesDesignTimeSource", 1);

                            if (generatesSharedDesignTimeSource)
                                childKey.SetValue("GeneratesSharedDesignTimeSource", 1);

                            childKey.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.Write(
                            string.Format("Unregistering {0}{1}{2}{3}{4}", generatorName, Environment.NewLine,
                                          ex.Message,
                                          Environment.NewLine, ex.StackTrace));
                    }
                }
            }
        }

        /// <summary>
        /// Removes support inside Visual Studio for current code generator.
        /// </summary>
        public void Unregister(bool forCurrentUserOnly, string vsVersion)
        {
            IList<string> keyNames = GetGeneratorRegistryKeys(vsVersion);
            RegistryKey masterKey = (forCurrentUserOnly ? Registry.CurrentUser : Registry.LocalMachine);

            if (keyNames != null)
            {
                foreach (string keyName in keyNames)
                {
                    try
                    {
                        masterKey.DeleteSubKeyTree(keyName);
                    }
                    catch(Exception ex)
                    {
                        Trace.Write(
                            string.Format("Unregistering {0}{1}{2}{3}{4}", generatorName, Environment.NewLine, ex.Message,
                                          Environment.NewLine, ex.StackTrace));
                    }
                }
            }
        }
    }
}
