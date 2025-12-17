using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq; // Knihovna pro mockování
using System.Collections.Generic;
using Utulek1.Application.Abstraction;
using Utulek1.Application.ViewModels;
using Utulek1.Controllers;
using Utulek1.Domain.Entities;
using Xunit; // Testovací framework

namespace Utulek1.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WithCorrectViewModel()
        {
            // --- 1. ARRANGE (Pøíprava) ---

            // Vytvoøíme mock (falešnou instanci) loggeru
            var mockLogger = new Mock<ILogger<HomeController>>();

            // Vytvoøíme mock služby IHomeService
            var mockService = new Mock<IHomeService>();

            // Pøipravíme si testovací data (co má služba vrátit)
            var expectedViewModel = new CarouselAnimalViewModel
            {
                Carousels = new List<Carousel>
                    {
                        new Carousel { ImageSrc = "test.jpg" }
                    }

            };

            // Nastavíme chování mocku: "Když nìkdo zavolá GetHomeViewModel, vra expectedViewModel"
            mockService.Setup(service => service.GetIndexViewModel())
                       .Returns(expectedViewModel);

            // Vytvoøíme instanci Controlleru a podstrèíme mu naše mocky
            var controller = new HomeController(mockLogger.Object, mockService.Object);

            // --- 2. ACT (Akce) ---

            // Zavoláme metodu, kterou testujeme
            var result = controller.Index();

            // --- 3. ASSERT (Ovìøení) ---

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CarouselAnimalViewModel>(viewResult.Model);

            // Ovìøíme, že pøišly carousely
            Assert.Same(expectedViewModel, model);
            Assert.Single(model.Carousels);


        }
    }
}