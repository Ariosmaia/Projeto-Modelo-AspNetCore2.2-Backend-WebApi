using System;
using FluentValidation;
using GestorFinanceiroWeb.Domain.Core.Models;


namespace GestorFinanceiroWeb.Domain.Eventos
{
    public class Endereco : Entity<Endereco>
    {
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string CEP { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public Guid? EventoId { get; private set; }

        //EF propriedade de Navegação
        public virtual Evento Evento { get; private set; }

        public Endereco(Guid id,
                        string logradouro,
                        string numero,
                        string complemento,
                        string bairro,
                        string cEP,
                        string cidade,
                        string estado,
                        Guid? eventoId)
        {
            Id = id;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            CEP = cEP;
            Cidade = cidade;
            Estado = estado;
            EventoId = eventoId;
        }

        //Construir para o EF
        protected Endereco() { }


        public override bool EhValido()
        {
            RuleFor(c => c.Logradouro)
                .NotEmpty().WithMessage("O Logradouro precisa ser fornecido")
                .Length(2, 150).WithMessage("O Logradouro precisa ter entre 2 e 150 caracteres");

            RuleFor(c => c.Bairro)
                .NotEmpty().WithMessage("O Bairro precisa ser fornecido")
                .Length(2, 150).WithMessage("O Bairro precisa ter entre 2 e 150 caracteres");

            RuleFor(c => c.CEP)
                .NotEmpty().WithMessage("O CEP precisa ser fornecido")
                .Length(8).WithMessage("O CEP precisa ter 8 caracteres");

            RuleFor(c => c.Cidade)
                .NotEmpty().WithMessage("A Cidade precisa ser fornecido")
                .Length(2, 150).WithMessage("A Cidade precisa ter entre 2 e 150 caracteres");

            RuleFor(c => c.Estado)
                .NotEmpty().WithMessage("A Estado precisa ser fornecido")
                .Length(2, 150).WithMessage("A Estado precisa ter entre 2 e 150 caracteres");

            RuleFor(c => c.Numero)
                .NotEmpty().WithMessage("O Numero precisa ser fornecido")
                .Length(1, 10).WithMessage("O Numero precisa ter entre 1 e 10 caracteres");

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }

    }
}