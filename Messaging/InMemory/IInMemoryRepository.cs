using System.Collections.Generic;

namespace Messaging.InMemory
{
    public interface IInMemoryRepository<T> where T : class
    {
        public void AddOrUpdate(T entity);

        public IEnumerable<T> Get();
    }
}