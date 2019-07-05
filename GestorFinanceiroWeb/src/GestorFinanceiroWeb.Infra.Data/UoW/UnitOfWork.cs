using GestorFinanceiroWeb.Domain.Core.Commands;
using GestorFinanceiroWeb.Domain.Interfaces;
using GestorFinanceiroWeb.Infra.Data.Context;

namespace GestorFinanceiroWeb.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventosContext _context;

        public UnitOfWork(EventosContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
