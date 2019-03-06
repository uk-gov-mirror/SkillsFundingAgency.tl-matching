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
    public class DataImportController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDataBlobUploadService _dataBlobUploadService;

        public DataImportController(IMapper mapper, IDataBlobUploadService dataBlobUploadService)
        {
            _mapper = mapper;
            _dataBlobUploadService = dataBlobUploadService;
        }

        public IActionResult Index()
        {
            return View(new DataImportParametersViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(DataImportParametersViewModel viewModel)
        {
            if (viewModel.File == null)
                ModelState.AddModelError("file", "You must select a file");

            if (viewModel.File != null && viewModel.File.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                ModelState.AddModelError("file", "You must upload an Excel file with the XLSX file extension");

            if (ModelState.IsValid)
            {
                var dto = _mapper.Map<DataUploadDto>(viewModel);
                dto.UserName = HttpContext.User.GetUserName();

                await _dataBlobUploadService.Upload(dto);

                viewModel.IsImportSuccessful = true;
            }

            return View(viewModel);
        }
    }
}