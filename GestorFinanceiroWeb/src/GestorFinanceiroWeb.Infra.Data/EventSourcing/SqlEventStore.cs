using GestorFinanceiroWeb.Domain.Core.Events;
using GestorFinanceiroWeb.Domain.Interfaces;
using GestorFinanceiroWeb.Infra.Data.Repository.EventSourcing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorFinanceiroWeb.Infra.Data.EventSourcing
{
    public class SqlEventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IUser _user;

        public SqlEventStore(IEventStoreRepository eventStoreRepository, IUser user)
        {
            _eventStoreRepository = eventStoreRepository;
            _user = user;
        }

        public void SalvarEvento<T>(T evento) where T : Event
        {
            var serializedData = JsonConvert.SerializeObject(evento);

            var storedEvent = new StoredEvent(
                evento,
                serializedData,
                _user.GetUserId().ToString());

            _eventStoreRepository.Store(storedEvent);
        }
    }
}
