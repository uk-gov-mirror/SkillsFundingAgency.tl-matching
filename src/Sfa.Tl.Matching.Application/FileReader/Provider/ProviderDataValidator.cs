﻿using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.Provider
{
    public class ProviderDataValidator : AbstractValidator<string[]>
    {
        private const int NumberOfColumns = 12;

        public ProviderDataValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .Must(x => x.Length == NumberOfColumns)
                .WithErrorCode(ValidationErrorCode.WrongNumberOfColumns.ToString())
                .WithMessage(ValidationErrorCode.MissingMandatoryData.Humanize());

            RuleFor(x => x[(int)ProviderColumnIndex.UkPrn])
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(ValidationErrorCode.MissingMandatoryData.Humanize())
                .Matches(ValidationConstants.UkprnRegex)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(ProviderColumnIndex.UkPrn)}' {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(x => x[(int)ProviderColumnIndex.Name])
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(ValidationErrorCode.MissingMandatoryData.Humanize());

            RuleFor(x => x[(int)ProviderColumnIndex.OfstedRating])
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(ValidationErrorCode.MissingMandatoryData.Humanize())
                .Must(x => x.IsOfsteadRating())
                    .WithErrorCode(ValidationErrorCode.WrongDataType.ToString())
                    .WithMessage($"'{nameof(ProviderColumnIndex.OfstedRating)}' {ValidationErrorCode.WrongDataType.Humanize()}");

            RuleFor(x => x[(int)ProviderColumnIndex.Active])
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(ValidationErrorCode.MissingMandatoryData.Humanize())
                .Must(x => x.IsYesNo())
                    .WithErrorCode(ValidationErrorCode.WrongDataType.ToString())
                    .WithMessage($"'{nameof(ProviderColumnIndex.Active)}' {ValidationErrorCode.WrongDataType.Humanize()}");

            RuleFor(x => x[(int) ProviderColumnIndex.PrimaryContact])
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(ValidationErrorCode.MissingMandatoryData.Humanize())
                .Matches(ValidationConstants.PhoneNumberRegex)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(ProviderColumnIndex.PrimaryContact)}' {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(x => x[(int)ProviderColumnIndex.PrimaryContactEmail])
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(ValidationErrorCode.MissingMandatoryData.Humanize())
                .EmailAddress();

            RuleFor(x => x[(int)ProviderColumnIndex.SecondaryContact])
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(ValidationErrorCode.MissingMandatoryData.Humanize())
                .Matches(ValidationConstants.PhoneNumberRegex)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(ProviderColumnIndex.PrimaryContact)}' {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(x => x[(int)ProviderColumnIndex.SecondaryContactEmail])
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(ValidationErrorCode.MissingMandatoryData.Humanize())
                .EmailAddress();

            RuleFor(x => x[(int)ProviderColumnIndex.Source])
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage(ValidationErrorCode.MissingMandatoryData.Humanize())
                .Must(x => x.IsOfsteadRating())
                    .WithErrorCode(ValidationErrorCode.WrongDataType.ToString())
                    .WithMessage($"'{nameof(ProviderColumnIndex.Source)}' {ValidationErrorCode.WrongDataType.Humanize()}");
        }
    }
}