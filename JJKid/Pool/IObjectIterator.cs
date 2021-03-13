namespace JJKid.Pool
{
    public interface IObjectIterator<T> where T : class
    {
        void reset();
        bool hasNext();
        T next();
    }
}
