using Microsoft.AspNetCore.Mvc;
using Stratus.WorkOrders.Api.Domain;
using Stratus.WorkOrders.Api.Services;

namespace Stratus.WorkOrders.Api.Controllers;

[ApiController]
[Route("api/work-orders")]
public sealed class WorkOrdersController : ControllerBase
{
    private readonly IWorkOrderRepository _repository;
    private readonly IWorkOrderScopeBriefOrchestrator _orchestrator;

    public WorkOrdersController(IWorkOrderRepository repository, IWorkOrderScopeBriefOrchestrator orchestrator)
    {
        _repository = repository;
        _orchestrator = orchestrator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkOrder>> GetById(string id, CancellationToken cancellationToken)
    {
        var workOrder = await _repository.GetByIdAsync(id, cancellationToken);
        return workOrder is null ? NotFound() : Ok(workOrder);
    }

    [HttpPost("{id}/scope-brief/propose")]
    public async Task<ActionResult<ScopeBriefProposal>> ProposeScopeBrief(string id, CancellationToken cancellationToken)
    {
        var proposal = await _orchestrator.CreateProposalAsync(id, cancellationToken);
        return proposal is null ? NotFound() : Ok(proposal);
    }

    [HttpPost("{id}/scope-brief/approve")]
    public async Task<ActionResult<ScopeBriefApprovalResult>> ApproveScopeBrief(string id, CancellationToken cancellationToken)
    {
        var result = await _orchestrator.ApproveAsync(id, cancellationToken);
        return result is null ? BadRequest("No pending scope brief exists.") : Ok(result);
    }
}
