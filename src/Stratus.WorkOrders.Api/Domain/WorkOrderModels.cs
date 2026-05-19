namespace Stratus.WorkOrders.Api.Domain;

public enum ScopeBriefStatus
{
    Draft,
    PendingApproval,
    Approved,
    Rejected
}

public sealed record WorkOrder(
    string Id,
    string Number,
    string Title,
    IReadOnlyList<JobNote> JobNotes,
    IReadOnlyList<FabricationConstraint> FabricationConstraints,
    IReadOnlyList<DrawingMetadata> LinkedDrawings,
    ScopeBrief? CurrentScopeBrief);

public sealed record JobNote(DateTimeOffset CreatedAt, string Author, string Body);

public sealed record FabricationConstraint(string Code, string Description, string Severity);

public sealed record DrawingMetadata(string DrawingId, string Sheet, string Revision, string Discipline, string Summary);

public sealed record WorkOrderScopeContext(
    string WorkOrderId,
    string WorkOrderNumber,
    string Title,
    IReadOnlyList<JobNote> RecentJobNotes,
    IReadOnlyList<FabricationConstraint> FabricationConstraints,
    IReadOnlyList<DrawingMetadata> LinkedDrawings);

public sealed record ScopeBriefSection(string Heading, string Content);

public sealed record ScopeBrief(
    string Summary,
    IReadOnlyList<ScopeBriefSection> Sections,
    IReadOnlyList<string> Risks,
    IReadOnlyList<string> OpenQuestions,
    ScopeBriefStatus Status,
    DateTimeOffset CreatedAt,
    string CreatedBy);

public sealed record ScopeBriefProposal(
    string WorkOrderId,
    string WorkOrderNumber,
    ScopeBrief Brief,
    string ModelNotes);

public sealed record ScopeBriefApprovalResult(
    string WorkOrderId,
    ScopeBrief ApprovedBrief);
