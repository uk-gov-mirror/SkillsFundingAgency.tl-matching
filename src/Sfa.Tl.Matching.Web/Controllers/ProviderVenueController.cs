﻿using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class ProviderVenueController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProviderVenueService _providerVenueService;

        public ProviderVenueController(IMapper mapper, IProviderVenueService providerVenueService)
        {
            _mapper = mapper;
            _providerVenueService = providerVenueService;
        }

        [Route("add-venue/{providerId}", Name = "AddVenue")]
        public IActionResult AddProviderVenue(int providerId)
        {
            return View(new AddProviderVenueViewModel { ProviderId = providerId });
        }

        [HttpPost]
        [Route("add-venue/{providerId}", Name = "CreateVenue")]
        public async Task<IActionResult> AddProviderVenue(AddProviderVenueViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var (isValid, formatedPostCode) = await _providerVenueService.IsValidPostCodeAsync(viewModel.Postcode);
            viewModel.Postcode = formatedPostCode;

            if (string.IsNullOrWhiteSpace(viewModel.Postcode) || !isValid)
            {
                ModelState.AddModelError("Postcode", "You must enter a real postcode");
                return View(viewModel);
            }

            var venue = await _providerVenueService.GetVenue(viewModel.ProviderId, viewModel.Postcode);

            int venueId;
            if (venue != null)
                venueId = venue.Id;
            else
                venueId = await _providerVenueService.CreateVenueAsync(viewModel);

            return RedirectToRoute("GetProviderVenueDetail", new { providerVenueId = venueId });
        }
        
        [Route("venue-overview/{providerVenueId}", Name = "GetProviderVenueDetail")]
        public async Task<IActionResult> ProviderVenueDetail(int providerVenueId)
        {
            var viewModel = await Populate(providerVenueId);

            return View(viewModel);
        }

        [HttpPost]
        [Route("venue-overview/{providerVenueId}", Name = "UpdateVenue")]
        public async Task<IActionResult> SaveVenue(ProviderVenueDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await Populate(viewModel.Id);
                return View("ProviderVenueDetail", viewModel);
            }

            await _providerVenueService.UpdateVenueAsync(viewModel);
            viewModel = await Populate(viewModel.Id);

            return View("ProviderVenueDetail", viewModel);
        }

        [HttpPost]
        [Route("venue-overview", Name = "SaveProviderVenueDetail")]
        public async Task<IActionResult> ProviderVenueDetail(ProviderVenueDetailViewModel viewModel)
        {
            // TODO Update Academic Years
            if (viewModel.Qualifications == null || viewModel.Qualifications.Count == 0)
            {
                ModelState.AddModelError("Qualifications", "You must add a qualification for this venue");
                viewModel = await Populate(viewModel.Id);
                return View(viewModel);
            }

            return RedirectToRoute("GetProviderDetail", new { providerId = viewModel.ProviderId });
        }

        [HttpGet]
        [Route("hide-unhide-venue/{providerVenueId}", Name = "GetConfirmProviderVenueChange")]
        public async Task<IActionResult> ConfirmProviderVenueChange(int providerVenueId)
        {
            var viewModel = await _providerVenueService.GetHideProviderVenueViewModelAsync(providerVenueId);

            return View(viewModel);
        }

        [HttpPost]
        [Route("hide-unhide-venue/{providerVenueId}", Name = "ConfirmProviderVenueChange")]
        public async Task<IActionResult> ConfirmProviderVenueChange(HideProviderVenueViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmProviderVenueChange", viewModel);
            }

            await _providerVenueService.SetIsProviderVenueEnabledForSearchAsync(viewModel.ProviderVenueId, !viewModel.IsEnabledForSearch);

            return RedirectToRoute("GetProviderVenueDetail", new { providerVenueId = viewModel.ProviderVenueId });
        }

        private async Task<ProviderVenueDetailViewModel> Populate(int providerVenueId)
        {
            var viewModel = await _providerVenueService.GetVenueWithQualificationsAsync(providerVenueId);

            return viewModel ?? new ProviderVenueDetailViewModel();
        }
    }
}