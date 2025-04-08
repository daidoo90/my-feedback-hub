using Microsoft.EntityFrameworkCore;
using MyFeedbackHub.Domain;

namespace MyFeedbackHub.Application.Shared.Abstractions;

public interface IFeedbackHubDbContext
{
    DbSet<UserDomain> Users { get; }

    DbSet<BusinessDomain> Businesses { get; }

    DbSet<BoardDomain> Boards { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
