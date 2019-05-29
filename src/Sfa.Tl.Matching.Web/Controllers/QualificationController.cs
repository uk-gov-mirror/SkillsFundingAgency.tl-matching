﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class QualificationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProviderVenueService _providerVenueService;
        private readonly IQualificationService _qualificationService;
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IRoutePathService _routePathService;

        public QualificationController(
            IMapper mapper,
            IProviderVenueService providerVenueService,
            IQualificationService qualificationService,
            IProviderQualificationService providerQualificationService,
            IRoutePathService routePathService)
        {
            _mapper = mapper;
            _providerVenueService = providerVenueService;
            _qualificationService = qualificationService;
            _providerQualificationService = providerQualificationService;
            _routePathService = routePathService;
        }

        [Route("add-qualification/{providerVenueId}", Name = "AddQualification")]
        public async Task<IActionResult> AddQualification(int providerVenueId)
        {
            var postcode = await _providerVenueService.GetVenuePostcodeAsync(providerVenueId);

            return View(new AddQualificationViewModel
            {
                ProviderVenueId = providerVenueId,
                Postcode = postcode
            });
        }

        [HttpPost]
        [Route("add-qualification/{providerId}", Name = "CreateQualification")]
        public async Task<IActionResult> AddQualification(AddQualificationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var isValid = await _qualificationService.IsValidLarIdAsync(viewModel.LarId);

            if (string.IsNullOrWhiteSpace(viewModel.LarId) || !isValid)
            {
                ModelState.AddModelError("LarsId", "Enter a learning aim reference (LAR) that has 8 characters");
                return View(viewModel);
            }

            var qualification = await _qualificationService.GetQualificationAsync(viewModel.LarId);

            if (qualification == null)
            {
                return RedirectToRoute("MissingQualification",
                    new
                    {
                        providerVenueId = viewModel.ProviderVenueId,
                        larId = viewModel.LarId
                    });
            }

            viewModel.QualificationId = qualification.Id;
            await _providerQualificationService.CreateProviderQualificationAsync(viewModel);

            return RedirectToRoute("GetProviderVenueDetail", 
                new { providerVenueId = viewModel.ProviderVenueId });
        }

        [Route("missing-qualification/{providerVenueId}/{larId}", Name = "MissingQualification")]
        public IActionResult MissingQualification(int providerVenueId, string larId)
        {
            return View(new MissingQualificationViewModel
            {
                ProviderVenueId = providerVenueId,
                LarId = larId,
                QualificationId = 1,
                Title = "TODO: Lookup title",
                Routes = GetRoutes()
            });
        }

        [HttpPost]
        [Route("missing-qualification/{providerVenueId}/{larId}", Name = "SaveMissingQualification")]
        public IActionResult MissingQualification(MissingQualificationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Routes = GetRoutes(viewModel);
                return View(viewModel);
            }

            if (string.IsNullOrWhiteSpace(viewModel.ShortTitle) || viewModel.ShortTitle.Length > 100)
            {
                ModelState.AddModelError("ShortTitle", "You must enter a short title that is 100 characters or fewer");
                viewModel.Routes = GetRoutes(viewModel);
                return View(viewModel);
            }

            if (!viewModel.Routes.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Routes", "You must choose a skill area for this qualification");
                viewModel.Routes = GetRoutes(viewModel);
                return View(viewModel);
            }

            //var qualificationId = qualification?.Id 
            //                      ?? await _qualificationService.CreateQualificationAsync(viewModel);

            return RedirectToRoute("GetProviderVenueDetail",
                new { providerVenueId = viewModel.ProviderVenueId });
        }

        private IList<RouteViewModel> GetRoutes(MissingQualificationViewModel viewModel = null)
        {
            var routes = _routePathService.GetRoutes().OrderBy(r => r.Name).ToList();
            var paths = _routePathService.GetPaths().ToList();

            var routesList = _mapper.Map<RouteViewModel[]>(routes);

            foreach (var route in routesList)
            {
                route.PathNames = paths
                    .Where(p => p.RouteId == route.Id)
                    .OrderBy(p => p.Name)
                    .Select(p => p.Name).ToList();

                if (viewModel?.Routes.Any(r => r.Id == route.Id && r.IsSelected) == true)
                {
                    route.IsSelected = true;
                }

            }

            return routesList;
        }
    }
}