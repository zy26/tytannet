namespace Pretorianie.Tytan.Parsers.Model
{
    /// <summary>
    /// Interface implemented by each <c>BinarySection</c> to apply additional info of given element.
    /// </summary>
    public interface IBinaryAppender<T, Z> where T : struct
    {
        /// <summary>
        /// Setup internal data based on given input read from native image.
        /// </summary>
        bool Attach(ref T s, uint size, Z arg);
    }
}