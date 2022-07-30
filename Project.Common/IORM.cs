using System.Collections.Generic;

namespace Project.Common
{
    public interface IORM<T>
    {
        Result<List<T>> Select();
        Result<bool> Insert(T entity);
        Result<bool> Update(T entity);
        Result<bool> Delete(T entity);
    }
}
