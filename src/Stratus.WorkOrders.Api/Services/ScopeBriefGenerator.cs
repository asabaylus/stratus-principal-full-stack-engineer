using Stratus.WorkOrders.Api.Domain;

namespace Stratus.WorkOrders.Api.Services;

public interface IScopeBriefGenerator
{
    Task<ScopeBriefProposal> GenerateAsync(WorkOrderScopeContext context, CancellationToken cancellationToken);
}

public sealed class ScopeBriefGenerator : IScopeBriefGenerator
{
    public Task<ScopeBriefProposal> GenerateAsync(WorkOrderScopeContext context, CancellationToken cancellationToken)
    {
        var risks = new List<string>();
        if (context.FabricationConstraints.Any(c => c.Severity.Equals("High", StringComparison.OrdinalIgnoreCase)))
        {
            risks.Add("High-severity fabrication constraints must be verified before field release.");
        }

        if (context.RecentJobNotes.Count == 0)
        {
            risks.Add("No recent job notes were available for context.");
        }

        var sections = new List<ScopeBriefSection>
        {
            new("Work to complete", $"Review tie-in location, confirm access, and coordinate field install for {context.Title}."),
            new("Context from recent notes", string.Join("\n", context.RecentJobNotes.Select(n => $"- {n.Author}: {n.Body}"))),
            new("Fabrication constraints", string.Join("\n", context.FabricationConstraints.Select(c => $"- {c.Code} ({c.Severity}): {c.Description}"))),
            new("Linked drawings", string.Join("\n", context.LinkedDrawings.Select(d => $"- {d.DrawingId} / {d.Sheet} rev {d.Revision} ({d.Discipline}): {d.Summary}")))
        };

        var brief = new ScopeBrief(
            Summary: $"Proposed field brief for {context.WorkOrderNumber}",
            Sections: sections,
            Risks: risks,
            OpenQuestions: ["Confirm access window with adjacent tenant work.", "Validate hanger layout before fabrication release."],
            Status: ScopeBriefStatus.PendingApproval,
            CreatedAt: DateTimeOffset.UtcNow,
            CreatedBy: "ai-assistant");

        return Task.FromResult(new ScopeBriefProposal(context.WorkOrderId, context.WorkOrderNumber, brief, "Generated from notes, constraints, and drawing metadata."));
    }
}
