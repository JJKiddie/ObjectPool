namespace JJKid.Pool
{
    public class TypeObjectPool<T1, T2> : AbstractObjectPool<T1> where T1 : class, ITypeObjectOfPool<T2>
                                                                 where T2 : System.Enum
    {
        public delegate T1 dele_newObjectWithType(T2 type);




        public T1 addNewOne(T2 type, dele_newObjectWithType cbf_newObject)
        {
            T1 obj = null;

            if(this.recycledPool.Count > 0)
            {
                for(int i = 0; i < this.recycledPool.Count; i++)
                {
                    if(this.recycledPool[i].Type.Equals(type))
                    {
                        obj = this.recycledPool[i];
                        this.recycledPool.Remove(obj);
                        break;
                    }
                }
            }

            if(obj == null)
                obj = cbf_newObject(type);

            this.activePool.Add(obj);

            return obj;
        }
    }
}
