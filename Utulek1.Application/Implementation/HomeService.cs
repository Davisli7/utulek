using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utulek1.Application.Abstraction;
using Utulek1.Application.ViewModels;

namespace Utulek1.Application.Implementation
{

    public class HomeService : IHomeService
    {
        IAnimalAppService _productAppService;
        ICarouselAppService _carouselAppService;

        public HomeService(IAnimalAppService productAppService,
                           ICarouselAppService carouselAppService)
        {
            _productAppService = productAppService;
            _carouselAppService = carouselAppService;
        }

        public CarouselAnimalViewModel GetIndexViewModel()
        {
            CarouselAnimalViewModel viewModel = new CarouselAnimalViewModel();
            viewModel.Products = _productAppService.Select();
            viewModel.Carousels = _carouselAppService.Select();
            return viewModel;
        }
    }
}
