﻿using MyFeedbackHub.Domain.Organization;

namespace MyFeedbackHub.Application.Users;

public interface IUserService
{
    Task<IEnumerable<Guid>> GetProjectIdsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<UserDomain?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
