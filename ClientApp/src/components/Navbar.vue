<template>
  <nav class="navbar navbar-expand-lg bg-light sticky-top mb-3">
    <div class="container-fluid">
      <RouterLink to="/" class="d-flex align-items-center mb-2 mb-lg-0 text-dark text-decoration-none">
        <span class="fs-4">TaskApp</span>
      </RouterLink>

      <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
        <li><RouterLink to="/" class="nav-link px-2" active-class="link-secondary" exact-active-class="link-secondary">Lista Zadań</RouterLink></li>
        <li><RouterLink to="/upcoming" class="nav-link px-2" active-class="link-secondary">Nadchodzące</RouterLink></li>
        <li><RouterLink to="/history" class="nav-link px-2" active-class="link-secondary">Historia</RouterLink></li>
        <li><RouterLink to="/projects" class="nav-link px-2" active-class="link-secondary">Projekty</RouterLink></li>
        <li><RouterLink to="/stats" class="nav-link px-2" active-class="link-secondary">Statystyki</RouterLink></li>
      </ul>
      <!--
      <div class="text-end">
        <button type="button" class="btn btn-outline-primary me-2">Login</button>
        <button type="button" class="btn btn-primary">Sign-up</button>
      </div>
      -->
    </div>
  </nav>
</template>

<script setup>

</script>

<style scoped>

</style>