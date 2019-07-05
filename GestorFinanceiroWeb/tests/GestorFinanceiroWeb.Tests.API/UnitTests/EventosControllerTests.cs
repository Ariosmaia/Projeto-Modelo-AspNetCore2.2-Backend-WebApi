using AutoMapper;
using GestorFinanceiroWeb.Application.ViewModels;
using GestorFinanceiroWeb.Domain.Core.Notifications;
using GestorFinanceiroWeb.Domain.Eventos.Commands;
using GestorFinanceiroWeb.Domain.Eventos.Repository;
using GestorFinanceiroWeb.Domain.Interfaces;
using GestorFinanceiroWeb.Services.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GestorFinanceiroWeb.Tests.API.UnitTests
{
    public class EventosControllerTests
    {
        // AAA = Arrange (Objeto que vou tratar/manipular), Act (Parte da execução), Assert (Resultado do teste)

        public EventosController eventosController;
        public Mock<DomainNotificationHandler> mockNotification;
        public Mock<IMapper> mockMapper;
        public Mock<IMediatorHandler> mockMediator;

        public EventosControllerTests()
        {
            mockMapper = new Mock<IMapper>();
            mockMediator = new Mock<IMediatorHandler>();
            mockNotification = new Mock<DomainNotificationHandler>();

            var mockRepository = new Mock<IEventoRepository>();
            var mockUser = new Mock<IUser>();

            eventosController = new EventosController(
                mockNotification.Object,
                mockUser.Object,
                mockRepository.Object,
                mockMapper.Object,
                mockMediator.Object
                );
        }

        [Fact]
        public void EventoController_RegistrarEvento_RetornarComSucesso()
        {
            // Arrange
            var eventoViewModel = new EventoViewModel();
            var eventoCommand = new RegistrarEventoCommand("Teste", "X", "XXX", 
                DateTime.Now, DateTime.Now.AddDays(1), true, 0, true, "", 
                Guid.NewGuid(), Guid.NewGuid(),
                new IncluirEnderecoEventoCommand(Guid.NewGuid(), "", null, "", "", "", "", "", null));

            mockMapper.Setup(m => m.Map<RegistrarEventoCommand>(eventoViewModel)).Returns(eventoCommand);
            mockNotification.Setup(m => m.GetNotifications()).Returns(new List<DomainNotification>());

            // Act
            var result = eventosController.Post(eventoViewModel);

            // Assert
            mockMediator.Verify(m => m.EnviarComando(eventoCommand), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void EventoController_RegistrarEvento_RetornarComErrosDeModelState()
        {
            // Arrange
            var notificationList = new List<DomainNotification> { new DomainNotification("Erro", "ModelError") };

            mockNotification.Setup(m => m.GetNotifications()).Returns(notificationList);
            mockNotification.Setup(c => c.HasNotification()).Returns(true);

            eventosController.ModelState.AddModelError("Erro", "ModelError");

            // Act
            var result = eventosController.Post(new EventoViewModel());

            // Assert
            mockMediator.Verify(m => m.EnviarComando(It.IsAny<RegistrarEventoCommand>()), Times.Never);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void EventoController_RegistrarEvento_RetornarComErrosDeDominio()
        {
            var eventoViewModel = new EventoViewModel();
            var eventoCommand = new RegistrarEventoCommand("Teste", "X", "XXX",
                DateTime.Now, DateTime.Now.AddDays(1), true, 0, true, "",
                Guid.NewGuid(), Guid.NewGuid(),
                new IncluirEnderecoEventoCommand(Guid.NewGuid(), "", null, "", "", "", "", "", null));

            mockMapper.Setup(m => m.Map<RegistrarEventoCommand>(eventoViewModel)).Returns(eventoCommand);

            var notificationList = new List<DomainNotification> { new DomainNotification("Erro", "Erro ao adicionar o evento") };

            mockNotification.Setup(m => m.GetNotifications()).Returns(notificationList);
            mockNotification.Setup(c => c.HasNotification()).Returns(true);

            // Act
            var result = eventosController.Post(new EventoViewModel());

            // Assert
            mockMediator.Verify(m => m.EnviarComando(It.IsAny<RegistrarEventoCommand>()), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}
