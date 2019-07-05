using GestorFinanceiroWeb.Domain.Core.Models;
using GestorFinanceiroWeb.Domain.Eventos;
using System;
using System.Collections.Generic;

namespace GestorFinanceiroWeb.Domain.Organizadores
{
    public class Organizador : Entity<Organizador>
    {
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string Email { get; private set; }



        public Organizador(Guid id, string nome, string cPF, string email)
        {
            Id = id;
            Nome = nome;
            CPF = cPF;
            Email = email;
        }

        //EF Costrutor
        public Organizador() { }

        //EF Propriedade de Navegação
        public virtual ICollection<Evento> Eventos { get; set; }


        public override bool EhValido()
        {
            return true;
        }
    }
}