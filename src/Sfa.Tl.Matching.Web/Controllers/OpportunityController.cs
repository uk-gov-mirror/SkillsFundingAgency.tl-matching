﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser + "," + RolesExtensions.StandardUser)]
    public class OpportunityController : Controller
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IReferralService _referralService;
        private readonly IMapper _mapper;

        public OpportunityController(IOpportunityService opportunityService, IReferralService referralService, IMapper mapper)
        {
            _opportunityService = opportunityService;
            _referralService = referralService;
            _mapper = mapper;
        }

        [Route("{SearchResultProviderCount}-provisiongap-opportunities-within-{SearchRadius}-miles-of-{Postcode}-for-route-{SelectedRouteId}", Name = "SaveProvisionGap")]
        public async Task<IActionResult> SaveProvisionGap(SaveProvisionGapViewModel viewModel)
        {
            var dto = _mapper.Map<OpportunityDto>(viewModel);

            if (await _opportunityService.IsNewProvisionGap(viewModel.OpportunityId))
                return await CreateOpportunity(dto);

            var providerSearchDto = new ProviderSearchDto
            {
                OpportunityId = dto.Id,
                SearchRadius = viewModel.SearchRadius,
                Postcode = viewModel.Postcode,
                RouteId = viewModel.SelectedRouteId ?? 0
            };
            await _opportunityService.UpdateOpportunity(providerSearchDto);

            return RedirectToRoute("PlacementInformationSave_Get", new { id = dto.Id });
        }

        [Route("referral-create", Name = "SaveReferral")]
        public async Task<IActionResult> SaveReferral(string viewModel)
        {
            var saveReferralViewModel = JsonConvert.DeserializeObject<SaveReferralViewModel>(viewModel);

            var dto = _mapper.Map<OpportunityDto>(saveReferralViewModel);
            if (await _opportunityService.IsNewReferral(saveReferralViewModel.OpportunityId))
                return await CreateOpportunity(dto);

            var providerSearchDto = new ProviderSearchDto
            {
                OpportunityId = saveReferralViewModel.OpportunityId,
                SearchRadius = saveReferralViewModel.SearchRadius,
                Postcode = saveReferralViewModel.Postcode,
                RouteId = saveReferralViewModel.SelectedRouteId ?? 0
            };
            await _opportunityService.UpdateOpportunity(providerSearchDto);
            await _opportunityService.UpdateReferrals(dto);

            return RedirectToRoute("PlacementInformationSave_Get", new { id = saveReferralViewModel.OpportunityId });
        }

        [HttpGet]
        [Route("placement-information/{id?}", Name = "PlacementInformationSave_Get")]
        public async Task<IActionResult> PlacementInformationSave(int id)
        {
            var dto = await _opportunityService.GetPlacementInformationSave(id);

            var viewModel = _mapper.Map<PlacementInformationSaveViewModel>(dto);

            return View(viewModel);
        }

        [HttpPost]
        [Route("placement-information/{id?}", Name = "PlacementInformationSave_Post")]
        public async Task<IActionResult> PlacementInformationSave(PlacementInformationSaveViewModel viewModel)
        {
            await Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = _mapper.Map<PlacementInformationSaveDto>(viewModel);
            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToRoute("LoadWhoIsEmployer", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers/{id?}", Name = "GetCheckAnswersReferrals")]
        public async Task<IActionResult> CheckAnswersReferrals(int id)
        {
            var viewModel = await GetCheckAnswersReferralViewModel(id);

            return View(viewModel);
        }

        [HttpPost]
        [Route("check-answers/{id?}", Name = "SaveCheckAnswersReferrals")]
        public async Task<IActionResult> CheckAnswersReferrals(CheckAnswersReferralViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(await GetCheckAnswersReferralViewModel(viewModel.OpportunityId));

            var dto = _mapper.Map<CheckAnswersDto>(viewModel);
            await _opportunityService.UpdateOpportunity(dto);

            await _referralService.SendEmployerReferralEmail(dto.OpportunityId);
            await _referralService.SendProviderReferralEmail(dto.OpportunityId);

            return RedirectToRoute("EmailSentReferrals_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers-gap/{id?}", Name = "GetCheckAnswersProvisionGap")]
        public async Task<IActionResult> CheckAnswersProvisionGap(int id)
        {
            var dto = await _opportunityService.GetCheckAnswers(id);

            var viewModel = _mapper.Map<CheckAnswersProvisionGapViewModel>(dto);

            return View(viewModel);
        }

        [HttpPost]
        [Route("check-answers-gap/{id?}", Name = "SaveCheckAnswersProvisionGap")]
        public async Task<IActionResult> CheckAnswersProvisionGap(CheckAnswersProvisionGapViewModel viewModel)
        {
            var dto = _mapper.Map<CheckAnswersDto>(viewModel);

            await _opportunityService.UpdateOpportunity(dto);
            await _opportunityService.CreateProvisionGap(viewModel);

            return RedirectToRoute("ProvisionGapSent_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("placement-gap/{id?}", Name = "ProvisionGapSent_Get")]
        public async Task<IActionResult> ProvisionGapSent(int id)
        {
            var opportunity = await _opportunityService.GetOpportunity(id);

            return View(new ProvisionGapSentViewModel
            {
                EmployerCrmRecord = opportunity.EmployerCrmId.ToString(),
                EmployerContactName = opportunity.EmployerContact,
                Postcode = opportunity.Postcode,
                RouteName = opportunity.RouteName
            });
        }

        [HttpGet]
        [Route("emails-sent/{id?}", Name = "EmailSentReferrals_Get")]
        public async Task<IActionResult> ReferralEmailSent(int id)
        {
            var opportunity = await _opportunityService.GetOpportunity(id);

            return View(new EmailsSentViewModel
            {
                EmployerCrmRecord = opportunity.EmployerCrmId.ToString(),
                EmployerContactName = opportunity.EmployerContact,
                EmployerBusinessName = opportunity.EmployerName
            });
        }

        private async Task<IActionResult> CreateOpportunity(OpportunityDto dto)
        {
            var id = await _opportunityService.CreateOpportunity(dto);
            return RedirectToRoute("PlacementInformationSave_Get", new { id });
        }

        private async Task Validate(PlacementInformationSaveViewModel viewModel)
        {
            var opportunity = await _opportunityService.GetOpportunity(viewModel.OpportunityId);
            if (opportunity != null)
            {
                viewModel.Postcode = opportunity.Postcode;
                viewModel.SearchRadius = opportunity.SearchRadius;
                viewModel.RouteId = opportunity.RouteId;
            }

            if (!viewModel.PlacementsKnown.HasValue || !viewModel.PlacementsKnown.Value) return;
            if (!viewModel.Placements.HasValue)
                ModelState.AddModelError(nameof(viewModel.Placements), "You must estimate how many placements the employer wants at this location");
            else if (viewModel.Placements < 1)
                ModelState.AddModelError(nameof(viewModel.Placements), "The number of placements must be 1 or more");
            else if (viewModel.Placements > 999)
                ModelState.AddModelError(nameof(viewModel.Placements), "The number of placements must be 999 or less");
        }

        private async Task<CheckAnswersReferralViewModel> GetCheckAnswersReferralViewModel(int id)
        {
            var dto = await _opportunityService.GetCheckAnswers(id);
            var providersForReferral = _opportunityService.GetReferrals(id);

            var viewModel = _mapper.Map<CheckAnswersReferralViewModel>(dto);
            viewModel.Providers = _mapper.Map<List<ReferralsViewModel>>(providersForReferral);

            return viewModel;
        }
    }
}