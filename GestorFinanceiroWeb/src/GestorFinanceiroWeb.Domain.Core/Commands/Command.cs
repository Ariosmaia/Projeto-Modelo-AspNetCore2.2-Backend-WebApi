using GestorFinanceiroWeb.Domain.Core.Events;
using MediatR;
using System;

namespace GestorFinanceiroWeb.Domain.Core.Commands
{
    public class Command : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        public Command()
        {
            Timestamp = DateTime.Now;
        }

    }
}
