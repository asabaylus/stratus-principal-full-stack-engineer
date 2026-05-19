using Stratus.WorkOrders.Api.Domain;

namespace Stratus.WorkOrders.Api.Services;

public interface IWorkOrderScopeBriefOrchestrator
{
    Task<ScopeBriefProposal?> CreateProposalAsync(string workOrderId, CancellationToken cancellationToken);
    Task<ScopeBriefApprovalResult?> ApproveAsync(string workOrderId, CancellationToken cancellationToken);
}

public sealed class WorkOrderScopeBriefOrchestrator : IWorkOrderScopeBriefOrchestrator
{
    private readonly IWorkOrderRepository _repository;
    private readonly IScopeBriefGenerator _generator;
    private readonly IScopeBriefApprovalService _approvalService;

    public WorkOrderScopeBriefOrchestrator(
        IWorkOrderRepository repository,
        IScopeBriefGenerator generator,
        IScopeBriefApprovalService approvalService)
    {
        _repository = repository;
        _generator = generator;
        _approvalService = approvalService;
    }

    public async Task<ScopeBriefProposal?> CreateProposalAsync(string workOrderId, CancellationToken cancellationToken)
    {
        var workOrder = await _repository.GetByIdAsync(workOrderId, cancellationToken);
        if (workOrder is null) return null;

        var context = new WorkOrderScopeContext(
            workOrder.Id,
            workOrder.Number,
            workOrder.Title,
            workOrder.JobNotes.OrderByDescending(n => n.CreatedAt).Take(5).ToArray(),
            workOrder.FabricationConstraints,
            workOrder.LinkedDrawings);

        return await _generator.GenerateAsync(context, cancellationToken);
    }

    public async Task<ScopeBriefApprovalResult?> ApproveAsync(string workOrderId, CancellationToken cancellationToken)
    {
        var workOrder = await _repository.GetByIdAsync(workOrderId, cancellationToken);
        if (workOrder?.CurrentScopeBrief is null) return null;

        var approved = await _approvalService.ApproveAsync(workOrder.CurrentScopeBrief, cancellationToken);
        await _repository.SaveScopeBriefAsync(workOrderId, approved, cancellationToken);
        return new ScopeBriefApprovalResult(workOrderId, approved);
    }
}
