﻿using GestorFinanceiroWeb.Domain.Eventos;
using GestorFinanceiroWeb.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorFinanceiroWeb.Infra.Data.Mappings
{
    public class EventoMapping : EntityTypeConfiguration<Evento>
    {
        public override void Map(EntityTypeBuilder<Evento> builder)
        {
            builder.Property(e => e.Nome)
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(e => e.DescricaoCurta)
                .HasColumnType("varchar(150)");


            builder.Property(e => e.DescricaoLonga)
                .HasColumnType("varchar(max)");


            builder.Property(e => e.NomeEmpresa)
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Ignore(e => e.ValidationResult);

            builder.Ignore(e => e.Tags);

            builder.Ignore(e => e.CascadeMode);

            builder.ToTable("Eventos");

            builder.HasOne(e => e.Organizador) //Um evento tem um organizador
                .WithMany(o => o.Eventos) //Organizador possui muitos Eventos
                .HasForeignKey(e => e.OrganizadorId);

            builder.HasOne(e => e.Categoria)
                .WithMany(e => e.Eventos)
                .HasForeignKey(e => e.CategoriaId)
                .IsRequired(false);
        }
    }
}
