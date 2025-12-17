using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq; 
using System.Collections.Generic;
using Utulek1.Application.Abstraction;
using Utulek1.Application.ViewModels;
using Utulek1.Controllers;
using Utulek1.Domain.Entities;
using Xunit; 

namespace Utulek1.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WithCorrectViewModel()
        {

            var mockLogger = new Mock<ILogger<HomeController>>();

            var mockService = new Mock<IHomeService>();

            var expectedViewModel = new CarouselAnimalViewModel
            {
                Carousels = new List<Carousel>
                    {
                        new Carousel { ImageSrc = "test.jpg" }
                    }

            };

            mockService.Setup(service => service.GetIndexViewModel())
                       .Returns(expectedViewModel);

            var controller = new HomeController(mockLogger.Object, mockService.Object);


            var result = controller.Index();


            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CarouselAnimalViewModel>(viewResult.Model);

            Assert.Same(expectedViewModel, model);
            Assert.Single(model.Carousels);


        }
    }
}