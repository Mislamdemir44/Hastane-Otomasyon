using System;
using System.Threading.Tasks;

namespace HospitalManagement.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
    }
}
