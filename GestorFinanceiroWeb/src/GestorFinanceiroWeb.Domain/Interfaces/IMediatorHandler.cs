using GestorFinanceiroWeb.Domain.Core.Commands;
using GestorFinanceiroWeb.Domain.Core.Events;
using System.Threading.Tasks;

namespace GestorFinanceiroWeb.Domain.Interfaces
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task EnviarComando<T>(T comando) where T : Command;
    }
}
