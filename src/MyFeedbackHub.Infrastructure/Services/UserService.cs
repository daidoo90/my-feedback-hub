﻿using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Application.Shared.Abstractions;
using MyFeedbackHub.Application.Users;
using MyFeedbackHub.Domain.Organization;

namespace MyFeedbackHub.Infrastructure.Services;

public sealed class UserService(
    IFeedbackHubDbContextFactory dbContextFactory) : IUserService
{
    public async Task<UserDomain?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var dbContext = dbContextFactory.Create();
        var user = await dbContext
        .Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

        return user;
    }

    public async Task<IEnumerable<Guid>> GetProjectIdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var dbContext = dbContextFactory.Create();
        return await dbContext.ProjectAccess
            .Where(pa => pa.UserId == userId)
            .Select(pa => pa.ProjectId)
            .ToListAsync(cancellationToken);
    }
}
