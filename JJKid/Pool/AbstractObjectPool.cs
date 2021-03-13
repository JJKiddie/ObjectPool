using System.Collections.Generic;


namespace JJKid.Pool
{
    public abstract class AbstractObjectPool<T> : IObjectIterator<T> where T : class
    {
        public delegate void dele_event(T obj);

        protected List<T> activePool;
        protected List<T> recycledPool;
        protected int iteratorNextIndex = 0;




        public AbstractObjectPool()
        {
            this.activePool = new List<T>();
            this.recycledPool = new List<T>();
        }

        public void destroy(dele_event cbf_destroy)
        {
            for(int i = 0; i < this.activePool.Count; i++)
                cbf_destroy?.Invoke(this.activePool[i]);
            for(int i = 0; i < this.recycledPool.Count; i++)
                cbf_destroy?.Invoke(this.recycledPool[i]);

            this.activePool.Clear();
            this.activePool = null;
            this.recycledPool.Clear();
            this.recycledPool = null;
        }


        //======================================================================
        //  Recycle
        //======================================================================
        public void recycleAll(dele_event cbf_recycle)
        {
            while(this.activePool.Count > 0)
                this.recycle(this.activePool[0], cbf_recycle);
        }

        public void recycle(T obj, dele_event cbf_recycle)
        {
            if(this.activePool.Contains(obj))
            {
                cbf_recycle?.Invoke(obj);

                this.recycledPool.Add(obj);
                this.activePool.Remove(obj);
            }
        }


        //======================================================================
        //  I/F IObjectIterator
        //======================================================================
        public void reset()
        {
            this.iteratorNextIndex = 0;
        }

        public bool hasNext()
        {
            return (this.iteratorNextIndex < this.activePool.Count);
        }

        public T next()
        {
            T obj = this.activePool[this.iteratorNextIndex];

            this.iteratorNextIndex += 1;

            return obj;
        }

        public IObjectIterator<T> getIterator()
        {
            this.reset();

            return this;
        }

        public List<T> getPureList()
        {
            return this.activePool;
        }
    }
}
