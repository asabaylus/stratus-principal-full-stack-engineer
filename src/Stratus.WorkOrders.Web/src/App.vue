<template>
  <main class="page">
    <WorkOrderScreen />
  </main>
</template>

<script setup>
import WorkOrderScreen from './components/WorkOrderScreen.vue'
</script>

<style>
body {
  margin: 0;
  font-family: Inter, system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
  background: #f6f7fb;
  color: #1f2937;
}

.page {
  padding: 24px;
}
</style>
