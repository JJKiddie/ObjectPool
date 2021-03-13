namespace JJKid.Pool
{
    public class ObjectPool<T> : AbstractObjectPool<T> where T : class
    {
        public delegate T dele_newObject();
        



        public T addNewOne(dele_newObject cbf_newObject)
        {
            T obj;

            if(this.recycledPool.Count > 0)
            {
                obj = this.recycledPool[0];
                this.recycledPool.Remove(obj);
            }
            else
                obj = cbf_newObject();

            this.activePool.Add(obj);

            return obj;
        }
    }
}
