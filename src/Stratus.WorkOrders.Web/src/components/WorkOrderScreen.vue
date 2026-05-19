<template>
  <section class="screen">
    <header class="header">
      <div>
        <h1>{{ workOrder.number }}</h1>
        <p>{{ workOrder.title }}</p>
      </div>
      <button class="primary" @click="generateScopeBrief" :disabled="loading">
        {{ loading ? 'Generating…' : 'Generate scope brief' }}
      </button>
    </header>

    <div class="grid">
      <article>
        <h2>Recent notes</h2>
        <ul>
          <li v-for="note in workOrder.jobNotes" :key="note.createdAt + note.author">
            <strong>{{ note.author }}</strong>: {{ note.body }}
          </li>
        </ul>
      </article>

      <article>
        <h2>Proposed scope brief</h2>
        <div v-if="proposal">
          <p><strong>{{ proposal.brief.summary }}</strong></p>
          <section v-for="section in proposal.brief.sections" :key="section.heading" class="brief-section">
            <h3>{{ section.heading }}</h3>
            <pre>{{ section.content }}</pre>
          </section>
          <section class="brief-section">
            <h3>Risks</h3>
            <ul><li v-for="risk in proposal.brief.risks" :key="risk">{{ risk }}</li></ul>
          </section>
          <section class="brief-section">
            <h3>Open questions</h3>
            <ul><li v-for="q in proposal.brief.openQuestions" :key="q">{{ q }}</li></ul>
          </section>
          <div class="actions">
            <button @click="approveScopeBrief" :disabled="approving">{{ approving ? 'Approving…' : 'Approve' }}</button>
            <button class="secondary" @click="discardProposal">Discard</button>
          </div>
        </div>
        <p v-else class="muted">No scope brief generated yet.</p>
      </article>
    </div>
  </section>
</template>

<script setup>
import { ref } from 'vue'

const workOrder = ref({
  id: 'wo-1001',
  number: 'WO-1001',
  title: 'Install hydronic tie-in at Level 3',
  jobNotes: [
    { createdAt: '2026-05-18T10:00:00Z', author: 'Sam', body: 'Field crew confirmed valve access is blocked until adjacent tenant work is complete.' },
    { createdAt: '2026-05-18T13:00:00Z', author: 'Priya', body: 'Need recheck hanger layout before releasing spool fabrication.' }
  ]
})

const loading = ref(false)
const approving = ref(false)
const proposal = ref(null)

async function generateScopeBrief() {
  loading.value = true
  try {
    proposal.value = {
      workOrderId: workOrder.value.id,
      workOrderNumber: workOrder.value.number,
      brief: {
        summary: `Proposed field brief for ${workOrder.value.number}`,
        sections: []
      }
    }
  } finally {
    loading.value = false
  }
}

async function approveScopeBrief() {
  approving.value = true
  try {
    if (proposal.value) {
      proposal.value.brief.status = 'Approved'
    }
  } finally {
    approving.value = false
  }
}

function discardProposal() {
  proposal.value = null
}
</script>

<style scoped>
.screen { display: grid; gap: 16px; }
.header { display: flex; justify-content: space-between; align-items: center; gap: 16px; }
.grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; }
article { background: white; border-radius: 12px; padding: 16px; box-shadow: 0 1px 3px rgba(0,0,0,.08); }
.primary { background: #2563eb; color: white; border: none; padding: 10px 14px; border-radius: 8px; }
.secondary { background: #e5e7eb; border: none; padding: 10px 14px; border-radius: 8px; }
.brief-section { margin-top: 12px; }
pre { white-space: pre-wrap; font-family: inherit; background: #f9fafb; padding: 12px; border-radius: 8px; }
.muted { color: #6b7280; }
.actions { display: flex; gap: 8px; margin-top: 12px; }
</style>
