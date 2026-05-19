using Stratus.WorkOrders.Api.Domain;

namespace Stratus.WorkOrders.Api.Services;

public interface IScopeBriefApprovalService
{
    Task<ScopeBrief> ApproveAsync(ScopeBrief brief, CancellationToken cancellationToken);
}

public sealed class ScopeBriefApprovalService : IScopeBriefApprovalService
{
    public Task<ScopeBrief> ApproveAsync(ScopeBrief brief, CancellationToken cancellationToken)
        => Task.FromResult(brief with { Status = ScopeBriefStatus.Approved });
}
