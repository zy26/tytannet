namespace Pretorianie.Tytan.Core.DbgView
{
    /// <summary>
    /// Style of jumping inside the code.
    /// </summary>
    public enum DbgViewCodeJumpStyle
    {
        /// <summary>
        /// Each time perform full scan of the source code and use the best matching option.
        /// </summary>
        Automatic,
        /// <summary>
        /// Detect the message style and jump method and switch to use that in future to speed detecting code place.
        /// </summary>
        Autodetect,
        /// <summary>
        /// Class is given as a 1st parameter of the message.
        /// </summary>
        Class_1,
        /// <summary>
        /// Class and function name are given as 1st and 2nd parameter of the message.
        /// </summary>
        Class_12,
        /// <summary>
        /// Class is given as a 2nd parameter of the message.
        /// </summary>
        Class_2,
        /// <summary>
        /// Class and function name are given as 2nd and 3rd parameter of the message.
        /// </summary>
        Class_23,
        /// <summary>
        /// Class is given as a 3rd parameter of the message.
        /// </summary>
        Class_3,
        /// <summary>
        /// Class and function name are given as 3rd and 4th parameter of the message.
        /// </summary>
        Class_34,
        /// <summary>
        /// Class is given as a 4th parameter of the message.
        /// </summary>
        Class_4,
        /// <summary>
        /// Class and function name are given as 4th and 5th parameter of the message.
        /// </summary>
        Class_45,
        /// <summary>
        /// Class is given as a 5th parameter of the message.
        /// </summary>
        Class_5,
        /// <summary>
        /// Class and function name are given as 5th and 6th parameter of the message.
        /// </summary>
        Class_56
    }
}
