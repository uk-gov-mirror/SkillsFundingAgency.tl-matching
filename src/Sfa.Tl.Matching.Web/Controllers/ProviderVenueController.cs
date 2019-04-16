﻿using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class ProviderVenueController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProviderService _providerService;
        private readonly IProviderVenueService _providerVenueService;

        public ProviderVenueController(IMapper mapper,
            IProviderService providerService,
            IProviderVenueService providerVenueService)
        {
            _mapper = mapper;
            _providerService = providerService;
            _providerVenueService = providerVenueService;
        }

        [Route("add-venue/{ukPrn}", Name = "AddVenue")]
        public async Task<IActionResult> ProviderVenueAdd(long ukPrn)
        {
            var provider = await _providerService.GetProviderByUkPrnAsync(ukPrn);
            return View(new ProviderVenueAddViewModel
            {
                ProviderId = provider.Id,
                UkPrn = ukPrn
            });
        }

        [HttpPost]
        [Route("add-venue/{ukPrn}", Name = "CreateVenue")]
        public async Task<IActionResult> ProviderVenueAdd(ProviderVenueAddViewModel viewModel)
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

            var isUniqueVenue = await _providerVenueService.HaveUniqueVenueAsync(viewModel.UkPrn, viewModel.Postcode);
            if (!isUniqueVenue)
            {
                ModelState.AddModelError("Postcode", "Venue already exists");
                return View(viewModel);
            }

            var dto = _mapper.Map<ProviderVenueDto>(viewModel);

            await _providerVenueService.CreateVenueAsync(dto);

            return RedirectToRoute("GetProviderVenueDetail", new
            {
                ukPrn = viewModel.UkPrn,
                postcode = viewModel.Postcode
            });
        }
        
        [Route("venue-overview/{ukPrn}/{postcode}", Name = "GetProviderVenueDetail")]
        public async Task<IActionResult> ProviderVenueDetail(long ukPrn, string postcode)
        {
            var viewModel = await Populate(ukPrn, postcode);

            return View(viewModel);
        }

        [HttpPost]
        [Route("venue-overview/{ukPrn}/{postcode}", Name = "UpdateVenue")]
        public async Task<IActionResult> SaveVenue(ProviderVenueDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await Populate(viewModel.UkPrn, viewModel.Postcode);
                return View("ProviderVenueDetail", viewModel);
            }

            var dto = _mapper.Map<UpdateProviderVenueDto>(viewModel);
            await _providerVenueService.UpdateVenueAsync(dto);
            viewModel = await Populate(viewModel.UkPrn, viewModel.Postcode);

            return View("ProviderVenueDetail", viewModel);
        }

        [HttpPost]
        [Route("venue-overvieww/{ukPrn}/{postcode}", Name = "SaveAcademicYears")]
        public async Task<IActionResult> ProviderVenueDetail(ProviderVenueDetailViewModel viewModel)
        {
            // TODO Update Academic Years
            if (viewModel.Qualifications == null || viewModel.Qualifications.Count == 0)
            {
                ModelState.AddModelError("Qualifications", "You must add a qualification for this venue");
                viewModel = await Populate(viewModel.UkPrn, viewModel.Postcode);
                return View(viewModel);
            }

            return RedirectToRoute("GetProviderDetail", new { ukPrn = viewModel.UkPrn });
        }

        [HttpGet]
        [Route("hide-unhide/{ukPrn}/{postcode}", Name = "GetConfirmProviderVenueChange")]
        public async Task<IActionResult> ConfirmProviderVenueChange(long ukPrn, string postcode)
        {
            var viewModel = await Populate(ukPrn, postcode);

            return View(viewModel);
        }

        [HttpPost]
        [Route("hide-unhide/{ukPrn}/{postcode}", Name = "ConfirmProviderVenueChange")]
        public async Task<IActionResult> ConfirmProviderVenueChange(ProviderVenueDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmProviderVenueChange", viewModel);
            }

            await _providerVenueService.SetIsProviderVenueEnabledForSearchAsync(viewModel.Id, !viewModel.IsEnabledForSearch);

            return RedirectToRoute("GetProviderVenueDetail",
                new
                {
                    ukPrn = viewModel.UkPrn,
                    postcode = viewModel.Postcode
                });
        }

        private async Task<ProviderVenueDetailViewModel> Populate(long ukPrn, string postcode)
        {
            var viewModel = await _providerVenueService.GetVenueWithQualificationsAsync(ukPrn, postcode);

            return viewModel ?? new ProviderVenueDetailViewModel();
        }
    }
}