using System.Collections.Generic;


namespace JJKid.Pool
{
    public abstract class AbstractObjectPool<T> : IObjectIterator<T> where T : class
    {
        public delegate void dele_event(T obj);

        protected List<T> activePool;
        protected List<T> recycledPool;
        protected List<T> iteratorContainer = null;
        protected int iteratorNextIndex = 0;




        public AbstractObjectPool()
        {
            this.activePool = new List<T>();
            this.recycledPool = new List<T>();
        }

        public void destroy(dele_event cbf_destroy)
        {
            if(this.iteratorContainer != null)
                this.iteratorContainer.Clear();
            this.iteratorContainer = null;

            for(int i = 0; i < this.activePool.Count; i++)
                if(cbf_destroy != null)
                    cbf_destroy(this.activePool[i]);
            for(int i = 0; i < this.recycledPool.Count; i++)
                if(cbf_destroy != null)
                    cbf_destroy(this.recycledPool[i]);

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
                if(cbf_recycle != null)
                    cbf_recycle(obj);

                this.recycledPool.Add(obj);
                this.activePool.Remove(obj);
            }
        }


        //======================================================================
        //  Getter
        //======================================================================
        public int Count
        {
            get { return this.activePool.Count; }
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
            return (this.iteratorNextIndex < this.iteratorContainer.Count);
        }

        public T next()
        {
            T obj = this.iteratorContainer[this.iteratorNextIndex];

            this.iteratorNextIndex += 1;

            return obj;
        }

        public IObjectIterator<T> getIterator()
        {
            if(this.iteratorContainer == null)
                this.iteratorContainer = new List<T>();
            this.iteratorContainer.Clear();
            for(int i = 0; i < this.activePool.Count; i++)
                this.iteratorContainer.Add(this.activePool[i]);

            this.reset();

            return this;
        }

        public List<T> getPureList()
        {
            return this.activePool;
        }
    }
}
