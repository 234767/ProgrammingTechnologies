using System.Threading.Tasks;
using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface ILeaseRepository : IRepository<ILease>
{ }

public interface IReturnRepository : IRepository<IReturn> 
{ }