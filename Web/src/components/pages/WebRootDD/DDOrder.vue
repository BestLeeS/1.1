<template>
  <div class="view-main">
    <div style="width: 100%;height: 100%;">
      AAAA
    </div>
  </div>
</template>
<script>
  export default {
  methods: {
    AA(){
      this.$router.push("dashboard");
      alert(this.$route.path)
    },
    BB(KeyCode){
      if(KeyCode == 116){
        this.AA();
      }
    }
  },
  async created() {
    // document.addEventListener("keydown", this.BB(KeyCode));
  }
};
</script>
</script>