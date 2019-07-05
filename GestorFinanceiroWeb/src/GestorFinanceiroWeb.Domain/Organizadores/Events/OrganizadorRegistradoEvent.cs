using GestorFinanceiroWeb.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorFinanceiroWeb.Domain.Organizadores.Events
{
    public class OrganizadorRegistradoEvent : Event
    {

        public OrganizadorRegistradoEvent(Guid id, string nome, string cpf, string email)
        {
            Id = id;
            Nome = nome;
            CPF = cpf;
            Email = email;
        }

        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string Email { get; private set; }
    }
}
