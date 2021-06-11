<template>
  <div class="sidebar">
    <div class="sideInfo" :class="{sideMini: collapse}">
      <i class="toggle-icon" :class="[collapse ? 'el-icon-s-unfold' : 'el-icon-s-fold']" @click="changeCollapse(!collapse)"></i>
    </div>
    <div class="sideBar-con" style="height: calc(100% - 90px) !important;" ref="sideBar">
      <el-menu 
        class="sidebar-el-menu" 
        background-color="#fff" 
        text-color="#606266" 
        active-text-color="#419FFF"
        unique-opened 
        collapse-transition
        :default-active="defaultActive" 
        :default-openeds="openList"
        style="border-right: none;"
        :collapse="collapse"
        router 
        @select="handleSelect"
      >
        <el-submenu :index="submenu.id" :key="submenu.id" v-for="(submenu,index) in sideBarMenus" >
          <template slot="title">
            <i :class="submenu.meta.icon"></i>
            <span slot="title">{{submenu.meta.title}}</span>
          </template>
          <el-menu-item-group>
            <el-menu-item v-for="(child,index) in submenu.children" :key="child.path" :index="child.path"><i :class="child.meta.icon"></i>{{child.meta.title}}</el-menu-item>
          </el-menu-item-group>
        </el-submenu>
      </el-menu>
    </div>
  </div>
</template>

<script>
  import consts from "../../utils/const";
  import router from "../../router";
  import { mapState, mapGetters, mapMutations } from "vuex";
  export default {
    data() {
      return {
        collapse: false,
        errorImg01: '',
        EditVisible: false,
        sideBarMenus: []
      };
    },
    methods: {
      ...mapMutations(["GotoHandle"]),
      changeCollapse(flag) {
        var contentLeft = document.getElementsByClassName('content-box')
        if (this.collapse != flag) {
          this.collapse ? contentLeft[0].style.left = "241px" : contentLeft[0].style.left = "67px"
          this.collapse = flag;
        }
      },
      handleSelect(key) {
        this.$store.state.ActiveIndex = key;
      }
    },
    computed: {
      ...mapState(["openList", "USERINFO","ActiveIndex"]),
      defaultActive(){
        if(this.$store.state.ActiveIndex)
          return this.$store.state.ActiveIndex;
        else
          return this.$router.path;
      }
    },
    mounted() {
      setTimeout(()=>{
        this.changeCollapse(true);
      },400)
    },
    created(){
      let MenusStr = localStorage.getItem("router");
      if (!MenusStr) {
        this.$message.error("登录失效,请重新登录!");
        this.$router.push("/login");
      }
      else {
        this.sideBarMenus = JSON.parse(MenusStr).filter(x=>x.path !== "/login");
        let TWidth = window.innerWidth;
        if (TWidth < 1200) {
          this.changeCollapse(true);
        }
      }
    },
    watch:{
      // defaultActive:function(val){
      //   console.log(val)
      // }
    }
  };
</script>

<style scoped>
  .toggle-icon {
    font-size: 18px;
    padding: 0 4px;
    cursor: pointer;
  }

  .sidebar {
    display: flex;
    flex-direction: column;
    position: absolute;
    left: 0;
    top: 40px;
    bottom: 0;
    overflow-y: scroll;
  }

  .sidebar::-webkit-scrollbar {
    width: 0;
  }

  .sidebar-el-menu:not(.el-menu--collapse) {
    width: 240px;
    overflow-y: unset;
  }

  .sidebar>ul {
    height: 100%;
  }

  .sideBar-con {
    max-width: 240px;
    flex: 1;
    overflow-y: auto;
  }

  .sideInfo {
    flex: 0 0 48px;
  }

  .copyright {
    flex: 0 0 40px;
  }

  .el-tooltip__popper {
    display: inline-block;
    max-width: 300px;
    line-height: 18px;
    cursor: pointer;
  }
</style>
<style>

</style>