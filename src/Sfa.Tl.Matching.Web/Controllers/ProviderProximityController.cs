﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class ProviderProximityController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly IProviderProximityService _providerProximityService;
        private readonly IRoutePathService _routePathService;

        public ProviderProximityController(IRoutePathService routePathService, 
            IProviderProximityService providerProximityService,
            ILocationService locationService)
        {
            _routePathService = routePathService;
            _providerProximityService = providerProximityService;
            _locationService = locationService;
        }

        /* Added to display postcode page, needs removing */
        [Route("postcode", Name = "Postcode")]
        public IActionResult SearchPostcode()
        {
            return View();
        }

        [HttpGet]
        [Route("provider-results-{searchCriteria}", Name = "GetProviderProximityResults", Order = 1)]
        public async Task<IActionResult> GetProviderProximityResults(string searchCriteria)
        {
            var routeNames = _routePathService.GetRoutes().OrderBy(r => r.Name)
                .Select(r => r.Name).ToList();

            var searchParametersViewModel = new ProviderProximitySearchParametersViewModel(searchCriteria, routeNames);
            var viewModel = new ProviderProximitySearchViewModel(searchParametersViewModel);
            viewModel.SearchResults = new ProviderProximitySearchResultsViewModel();
            
            return View("Results", viewModel);
        }

        [HttpPost]
        public IActionResult FilterResultsAsync(ProviderProximitySearchParametersViewModel viewModel)
        {
            if (viewModel.Filters.Count(f => f.IsSelected) == 0)
                return RedirectToRoute("GetProviderProximityResults", new
                {
                    searchCriteria = $"{viewModel.Postcode}"
                });

            var filters = string.Join("-", viewModel.Filters.Where(f => f.IsSelected).Select(f => f.Name));
            
            return RedirectToRoute("GetProviderProximityResults", new
            {
                searchCriteria = $"{viewModel.Postcode}-{filters}"
            });
        }
    }
}