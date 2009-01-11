namespace Pretorianie.Tytan.Parsers.Model
{
    /// <summary>
    /// Definition of a handler called each time specific operation has been performed over a <c>BinarySection</c>.
    /// </summary>
    public delegate void BinarySectionChangedEventHandler(BinaryFile f, BinarySection s);
}