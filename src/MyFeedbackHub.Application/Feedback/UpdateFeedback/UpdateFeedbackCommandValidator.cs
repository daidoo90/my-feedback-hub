﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Organization;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users;
using MyFeedbackHub.SharedKernel.Results;

namespace MyFeedbackHub.Application.Feedback;

public sealed class UpdateFeedbackCommandValidator : AbstractValidator<UpdateFeedbackCommand>
{
    private readonly IOrganizationService _organizationService;
    private readonly IUserService _userService;
    private readonly IFeedbackHubDbContextFactory _dbContextFactory;
    private readonly IUserContext _currentUser;

    public UpdateFeedbackCommandValidator(
        IOrganizationService organizationService,
        IUserService userService,
        IFeedbackHubDbContextFactory dbContextFactory,
        IUserContext currentUser)

    {
        _organizationService = organizationService;
        _userService = userService;
        _dbContextFactory = dbContextFactory;
        _currentUser = currentUser;
        ValidateTitle();
        ValidateFeedback();
    }

    private void ValidateTitle()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Feedback.TitleInvalid)
            .NotNull()
            .WithErrorCode(ErrorCodes.Feedback.TitleInvalid);
    }

    private void ValidateFeedback()
    {
        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
            {
                var dbContext = _dbContextFactory.Create();
                var feedback = await dbContext.Feedbacks
                    .SingleOrDefaultAsync(f => f.FeedbackId == command.FeedbackId
                                                && !f.IsDeleted,
                                                cancellationToken);

                if (feedback == null
                    || feedback.IsDeleted
                    || feedback.CreatedBy != _currentUser.UserId)
                {
                    return false;
                }

                return true;
            })
            .WithErrorCode(ErrorCodes.Feedback.FeedbackInvalid);
    }
}
