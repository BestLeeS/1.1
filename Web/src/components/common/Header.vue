<template>
  <div class="WebSiteHeader">
    <div style="width: 50%;"></div>
    <div style="width: 50%;text-align: right;">
      <el-button size="mini" style="background: none;color: #fff;">配置打印机</el-button>
      <SysMsg></SysMsg>
    </div>
  </div>
</template>
<script>
import { mapState, mapMutations, mapActions } from "vuex";

// 引入进度查询界面

import SysMsg from './SysMsg'
import ws from './../../utils/WebSocket'

export default {
  components: {SysMsg },
  data() {
    return {
      isCollapse: false
    };
  },
  created() {

  },
  methods: {
    handleSelect(key) {
      if (key == this.activeIndex) {
        return;
			}
    }
  },
  watch: {
    // 侧边栏折叠
    isCollapse(val, oldVal) {
      if (val != oldVal) {
        this.$store.commit("changeCollapse", val);
      }
    },
    collapse(val, oldVal) {
      if (val != oldVal) {
        this.isCollapse = this.collapse;
      }
		},
		activeIndex:function(newVal,oldVal) {
			window.activeIndex = oldVal;
		}
  },
  computed: {
    ...mapState(["collapse", "Menus", "activeIndex", "isGoto", "assist","isjump"])
  },
  created() {
    
  }
};
</script>
<style scoped>
.el-badge{
  cursor:pointer
}
.el-badge >>> .el-badge__content{
  top: 20px;
}
.WebSiteHeader{
  display: flex;
  position: relative;
	width: 100%;
	padding: 0 16px;
	background-color: #2F3A4A;
	height: 40px;
	line-height: 40px;
	overflow: hidden;
	box-sizing: border-box;
}

</style>
