namespace Pretorianie.Tytan.Parsers.Model
{
    /// <summary>
    /// Interface implemented by each <c>BinarySection</c> to claim the support for specific conversion.
    /// </summary>
    public interface IBinaryConverter<T> where T : struct
    {
        /// <summary>
        /// Setup internal data based on given input read from native image.
        /// </summary>
        bool Convert(ref T s, uint startOffset, uint size);
    }
}