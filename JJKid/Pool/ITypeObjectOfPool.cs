namespace JJKid.Pool
{
    public interface ITypeObjectOfPool<T> where T : System.Enum
    {
        T Type { get; }
    }
}
