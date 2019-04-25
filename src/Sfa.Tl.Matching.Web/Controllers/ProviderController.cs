﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class ProviderController : Controller
    {
        private readonly IProviderService _providerService;
        
        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet]
        [Route("search-ukprn", Name = "SearchProvider")]
        public async Task<IActionResult> SearchProvider()
        {
            return View(await GetProviderSearchResults(new ProviderSearchParametersViewModel()));
        }

        [HttpPost]
        [Route("search-ukprn", Name = "SearchProviderByUkPrn")]
        public async Task<IActionResult> SearchProvider(ProviderSearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(SearchProvider), new ProviderSearchViewModel(viewModel)
                {
                    IsAuthorisedUser = GetIsAuthorisedUser()
                });
            }

            var searchResult = viewModel.UkPrn.HasValue
                ? await _providerService.SearchAsync(viewModel.UkPrn.Value)
                : null;

            if (searchResult == null || searchResult.Id == 0)
            {
                ModelState.AddModelError(nameof(ProviderSearchParametersViewModel.UkPrn), "You must enter a real UKPRN");
                return View(nameof(SearchProvider), new ProviderSearchViewModel(viewModel));
            }

            var resultsViewModel = await GetProviderSearchResults(viewModel);

            return View(resultsViewModel);
        }

        [HttpPost]
        [Route("save-provider-feedback", Name = "SaveProviderFeedback")]
        public async Task<IActionResult> SaveProviderFeedback(SaveProviderFeedbackViewModel viewModel)
        {
            return null;
        }

        [HttpGet]
        [Route("provider-overview/{providerId}", Name = "GetProviderDetail")]
        public async Task<IActionResult> ProviderDetail(int providerId)
        {
            var viewModel = new ProviderDetailViewModel();

            if (providerId > 0)
            {
                viewModel = await _providerService.GetProviderDetailByIdAsync(providerId, true);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("provider-overview", Name = "SaveProviderDetail")]
        public async Task<IActionResult> SaveProviderDetail(ProviderDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(ProviderDetail), viewModel);
            }

            if (viewModel.Id > 0)
            {
                await _providerService.UpdateProviderDetail(viewModel);
            }
            else
            {
                viewModel.Id = await _providerService.CreateProvider(viewModel);
            }

            return View(nameof(SearchProvider));
        }

        [HttpGet]
        [Route("hide-unhide-provider/{providerId}", Name = "GetConfirmProviderChange")]
        public async Task<IActionResult> ConfirmProviderChange(int providerId)
        {
            var viewModel = await _providerService.GetHideProviderViewModelAsync(providerId);
            return View(viewModel);
        }

        [HttpPost]
        [Route("hide-unhide-provider/{providerId}", Name = "ConfirmProviderChange")]
        public async Task<IActionResult> ConfirmProviderChange(HideProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmProviderChange", viewModel);
            }

            await _providerService.SetIsProviderEnabledForSearchAsync(viewModel.ProviderId, !viewModel.IsEnabledForSearch);

            return RedirectToRoute("GetProviderDetail", new { providerId = viewModel.ProviderId });
        }

        private async Task<ProviderSearchViewModel> GetProviderSearchResults(ProviderSearchParametersViewModel viewModel)
        {
            var resultsViewModel = new ProviderSearchViewModel(viewModel)
            {
                SearchResults = new ProviderSearchResultsViewModel
                {
                    Results = await _providerService.SearchProvidersWithFundingAsync(viewModel)
                },
                IsAuthorisedUser = GetIsAuthorisedUser()
            };

            return resultsViewModel;
        }

        private bool GetIsAuthorisedUser()
        {
            return true;
        }
    }
}