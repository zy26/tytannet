using System;

namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Enum type that describes options for property generations.
    /// </summary>
    [Flags]
    public enum PropertyGeneratorOptions
    {
        /// <summary>
        /// No options specified. Use default settings.
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// Generate only getter for property.
        /// </summary>
        Getter = 0x01,
        /// <summary>
        /// Generate only setter for property.
        /// </summary>
        Setter = 0x02,
        /// <summary>
        /// Generate both (getter and setter) for property.
        /// </summary>
        GetterAndSetter = Getter | Setter,
        /// <summary>
        /// Don't generate comment for property.
        /// </summary>
        SuppressComment = 0x10,
        /// <summary>
        /// Don't encapsulate properties inside the region.
        /// </summary>
        SuppressRegion = 0x20,

        /// <summary>
        /// Don't change the variable's access level.
        /// </summary>
        ForceVarDontChange = 0x1000,
        /// <summary>
        /// Force variable's 'public' access level.
        /// </summary>
        ForceVarPublic = 0x2000,
        /// <summary>
        /// Force variable's 'internal' access level.
        /// </summary>
        ForceVarInternal = 0x3000,
        /// <summary>
        /// Force variable's 'protected' access level.
        /// </summary>
        ForceVarProtected = 0x4000,
        /// <summary>
        /// Force variable's 'protected internal' access level.
        /// </summary>
        ForceVarProtectedInternal = 0x5000,
        /// <summary>
        /// Force variable's 'private' access level.
        /// </summary>
        ForceVarPrivate = 0x6000,
        /// <summary>
        /// Mask for detecting variable's access right change.
        /// </summary>
        ForceVarMask = 0xF000,

        /// <summary>
        /// Keep the same access level as variable.
        /// </summary>
        ForcePropertyAsVar = 0x0100,
        /// <summary>
        /// Force property's 'public' access level.
        /// </summary>
        ForcePropertyPublic = 0x0200,
        /// <summary>
        /// Force property's 'internal' access level.
        /// </summary>
        ForcePropertyInternal = 0x0300,
        /// <summary>
        /// Force property's 'protected' access level.
        /// </summary>
        ForcePropertyProtected = 0x0400,
        /// <summary>
        /// Force property's 'protected internal' access level.
        /// </summary>
        ForcePropertyProtectedInternal = 0x0500,
        /// <summary>
        /// Force property's 'private' access level.
        /// </summary>
        ForcePropertyPrivate = 0x0600,
        /// <summary>
        /// Mask for detecting property's access right change.
        /// </summary>
        ForcePropertyMask = 0x0F00
    }
}
