<template>
  <div class="d-flex align-items-center flex-wrap gap-2">
    <!-- Data od -->
    <div>
      <label class="form-label mb-1 d-block">Od</label>
      <input
        type="date"
        class="form-control"
        :value="from"
        @input="onInput('from', $event)"
        placeholder="rrrr-mm-dd"
      />
    </div>

    <span class="fw-bold mx-2">–</span>

    <!-- Data do -->
    <div>
      <label class="form-label mb-1 d-block">Do</label>
      <input
        type="date"
        class="form-control"
        :value="to"
        @input="onInput('to', $event)"
        placeholder="rrrr-mm-dd"
      />
    </div>
  </div>
</template>

<script setup>
const props = defineProps({
  from: { type: String, default: '' },
  to: { type: String, default: '' }
})

const emit = defineEmits(['update:from', 'update:to', 'change'])

function onInput(field, e) {
  let value = e.target.value.trim()
  // prosta walidacja: dozwolone "-", "" albo data (format YYYY-MM-DD)
  if (value !== '-' && value !== '' && !/^\d{4}-\d{2}-\d{2}$/.test(value)) return

  if (field === 'from') emit('update:from', value)
  else emit('update:to', value)

  emit('change', { from: field === 'from' ? value : props.from, to: field === 'to' ? value : props.to })
}
</script>

<style scoped>
.form-control {
  width: 160px;
}

input[type="date"].form-control {
  background-color: #2b2b2b;      
  color: #fff;                    
  border: 1px solid #444;         
  border-radius: 6px;
  padding: 0.5rem 0.75rem;
  font-size: 1rem;

  /* żeby placeholder (dd.mm.rrrr) był też jasny */
  &::placeholder {
    color: #bbb;
  }

  &:focus {
    outline: none;
    border-color: #777;
    box-shadow: 0 0 0 2px rgba(255,255,255,0.2);
  }
}
</style>