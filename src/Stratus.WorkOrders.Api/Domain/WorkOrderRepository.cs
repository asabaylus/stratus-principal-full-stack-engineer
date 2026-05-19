namespace Stratus.WorkOrders.Api.Domain;

public interface IWorkOrderRepository
{
    Task<WorkOrder?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task SaveScopeBriefAsync(string workOrderId, ScopeBrief brief, CancellationToken cancellationToken);
}

public sealed class InMemoryWorkOrderRepository : IWorkOrderRepository
{
    private readonly Dictionary<string, WorkOrder> _workOrders = new(StringComparer.OrdinalIgnoreCase)
    {
        ["wo-1001"] = new WorkOrder(
            "wo-1001",
            "WO-1001",
            "Install hydronic tie-in at Level 3",
            [
                new JobNote(DateTimeOffset.UtcNow.AddHours(-5), "Sam", "Field crew confirmed valve access is blocked until adjacent tenant work is complete."),
                new JobNote(DateTimeOffset.UtcNow.AddHours(-2), "Priya", "Need recheck hanger layout before releasing spool fabrication."),
            ],
            [
                new FabricationConstraint("FC-12", "No hot work in occupied zones", "High"),
                new FabricationConstraint("FC-19", "Prefabricate as modular assemblies where possible", "Medium"),
            ],
            [
                new DrawingMetadata("D-310", "M3.2", "R4", "Mechanical", "Tie-in detail and ceiling elevation"),
                new DrawingMetadata("D-311", "P1.4", "R2", "Plumbing", "Nearby service corridor routing"),
            ],
            null)
    };

    public Task<WorkOrder?> GetByIdAsync(string id, CancellationToken cancellationToken)
        => Task.FromResult(_workOrders.TryGetValue(id, out var workOrder) ? workOrder : null);

    public Task SaveScopeBriefAsync(string workOrderId, ScopeBrief brief, CancellationToken cancellationToken)
    {
        if (!_workOrders.TryGetValue(workOrderId, out var existing))
        {
            return Task.CompletedTask;
        }

        _workOrders[workOrderId] = existing with { CurrentScopeBrief = brief };
        return Task.CompletedTask;
    }
}
