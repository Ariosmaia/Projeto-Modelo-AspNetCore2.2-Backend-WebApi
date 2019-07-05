using GestorFinanceiroWeb.Domain.Organizadores;
using GestorFinanceiroWeb.Domain.Organizadores.Repository;
using GestorFinanceiroWeb.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorFinanceiroWeb.Infra.Data.Repository
{
    public class OrganizadorRepository : Repository<Organizador>, IOrganizadorRepository
    {
        public OrganizadorRepository(EventosContext context) : base(context)
        {
        }
    }
}
