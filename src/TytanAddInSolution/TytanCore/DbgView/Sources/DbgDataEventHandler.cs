namespace Pretorianie.Tytan.Core.DbgView.Sources
{
    /// <summary>
    /// Delegate to method processing debug messages from external sources.
    /// </summary>
    public delegate void DbgDataEventHandler (IDbgSource source, uint pid, string message);
}
