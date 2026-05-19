# Stratus Work Order AI Scope Brief

## What you're building

You’re extending an existing work-order screen with an AI-assisted scope brief generator.

The new flow should read a work order’s recent job notes, fabrication constraints, and linked drawing metadata, then propose a structured task brief for the field team. The brief should be reviewable and editable by a person before it is applied to the work order.

This slice is intentionally small, but it should feel like real brownfield work: there is existing API shape, existing UI, and a service boundary where you need to wire retrieval, structured output, and a human approval step into the workflow.

## Time-box

60 minutes.

## Getting started

Install dependencies:

```bash
# backend
cd src/Stratus.WorkOrders.Api
dotnet restore

# frontend
cd ../Stratus.WorkOrders.Web
npm install
```

You are expected to write and run your own tests as part of the exercise.

## Acceptance criteria

A completed slice should:

- Add an AI scope brief action to the work-order screen.
- Gather the relevant work-order context needed for the brief.
- Produce a structured scope brief with clear sections for the field team.
- Keep the generated brief in a pending/review state until a human approves it.
- Allow the user to review and accept or discard the proposed brief.
- Preserve the existing work-order screen behavior outside of this flow.
- Include tests that cover the service/orchestrator behavior you changed.

## Process

Follow the workflow described in `INTERVIEW_RULES.md`.
