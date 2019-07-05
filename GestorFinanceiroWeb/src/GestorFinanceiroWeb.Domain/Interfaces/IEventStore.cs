using GestorFinanceiroWeb.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorFinanceiroWeb.Domain.Interfaces
{
    public interface IEventStore
    {
        void SalvarEvento<T>(T evento) where T : Event;
    }
}
