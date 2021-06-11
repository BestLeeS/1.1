<template>
  <div id="tags-view-container" class="tags-view-container">
    <!-- <i class="el-icon-d-arrow-left" style="color:#409EFF;margin-top:5px;" @click="Scroll_left($event)"></i> -->
    <scroll-pane ref="scrollPane" class="tags-view-wrapper">
        <router-link
          v-for="tag in visitedViews"
          ref="tag"
          :key="tag.path"
          :class="isActive(tag)?'active':''"
          :to="{ path: tag.path, query: tag.query, fullPath: tag.path }"
          tag="span"
          class="tags-view-item"
          @click.middle.native="closeSelectedTag(tag)"
          @contextmenu.prevent.native="openMenu(tag,$event)"
        >
          <span @click="SetActive(tag.path)"><i :class="tag.meta.icon"></i>{{tag.meta.title}}</span>
          <span
            v-show="isClose"
            class="el-icon-close"
            style="font-size:16px;"
            @click.prevent.stop="closeSelectedTag(tag)"
          />
        </router-link>

    </scroll-pane>
    <!-- <i class="el-icon-d-arrow-right" style="color:#409EFF;margin-top:5px;" @click="Scroll_right()"></i> -->
    <ul v-show="visible" :style="{left:left+'px',top:top+'px'}" class="contextmenu">
      <li @click="closeSelectedTag(selectedTag)">关闭当前</li>
      <li @click="closeOthersTags">关闭其他</li>
      <li @click="closeAllTags(selectedTag)">关闭所有</li>
    </ul>
  </div>
</template>

<script>
import ScrollPane from "./ScrollPane";

export default {
  components: { ScrollPane },
  data() {
    return {
      visible: false,
      isClose: true,
      top: 0,
      left: 0,
      selectedTag: {},
      affixTags: [],
      right: 0,
    };
  },
  computed: {
    visitedViews() {
      // this.$store.commit("changeTagItem", []);
      // 添加页面 获取路由和name

      if (this.$store.state.TabKeepList.length == 1) {
        this.isClose = false;
      } 
      else {
        this.isClose = true;
      }
      return this.$store.state.TabKeepList;
    },
    routes() {
      return this.$store.state.permission.routes;
    },
  },
  watch: {

  },
  methods: {
    SetActive(path){
      this.$store.state.ActiveIndex = path.replace("/","");
    },
    isActive(route) {
      return route.path === this.$route.path;
    },
    Scroll_left() {
      this.right = 10;
      document.getElementsByClassName("el-scrollbar__wrap")[0].scrollLeft = this.right;
    },
    Scroll_right() {
      this.right += 50;
      document.getElementsByClassName("el-scrollbar__wrap")[0].scrollLeft = this.right;
    },
    closeSelectedTag(view) {
      let index = this.$store.state.TabKeepList.findIndex(z=>z.name == view.name);
      this.$store.state.TabKeepList.splice(index, 1);
      if(this.$route.path == view.path){
        this.$router.replace(this.$store.state.TabKeepList[index - 1].path)
      }
      this.visible = false;
    },
    closeOthersTags() {
      this.$router.push(this.selectedTag.path);
      this.$store.state.TabKeepList = this.$store.state.TabKeepList.filter(z=>z.path == this.selectedTag.path);
      this.visible = false;
    },
    closeAllTags(view) {
      this.$store.state.TabKeepList = [this.$store.state.TabKeepList[0]];
      this.$router.push(this.$store.state.TabKeepList[0].path);
      this.visible = false;
    },
    openMenu(tag, e) {
      const menuMinWidth = 105;
      const offsetLeft = this.$el.getBoundingClientRect().left; // container margin left
      const offsetWidth = this.$el.offsetWidth; // container width
      const maxLeft = offsetWidth - menuMinWidth; // left boundary
      const left = e.clientX - offsetLeft + 15; // 15: margin right

      if (left > maxLeft) {
        this.left = maxLeft;
      } else {
        this.left = left;
      }

      this.top = e.clientY;
      this.visible = true;
      this.selectedTag = tag;
    },
  },
}; 
</script>

<style lang="scss" scoped>
.tags-view-container {
  display: flex;
  height: 27px;
  width: 100%;
  background: #fff;
  border-bottom: 1px solid #DCDFE6;
  box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.12), 0 0 3px 0 rgba(0, 0, 0, 0.04);
  // margin-left: 35px;

  .tags-view-wrapper {
    .tags-view-item {
      display: inline-block;
      position: relative;
      cursor: pointer;
      height: 27px;
      line-height: 27px;
      border-left: 1px solid #d8dce5;
      color: #495060;
      background: #f9f9f9;
      padding: 0 8px;
      font-size: 12px;
      margin-right: 1px;
      border-radius: 4px;
      &:first-of-type {
        margin-left: 15px;
      }
      &:last-of-type {
        // margin-right: 15px;
      }
      &.active {
        background-color: #8CC5FF;
        color: #000000;
        border-color: #eeeeee;
      }
    }
  }
  .tags-view-item:last-child {
    border-right: 1px solid #d8dce5;
  }
  .contextmenu {
    margin: 0;
    background: #fff;
    z-index: 3000;
    position: absolute;
    list-style-type: none;
    padding: 5px 0;
    border-radius: 4px;
    font-size: 12px;
    font-weight: 400;
    color: #333;
    box-shadow: 2px 2px 3px 0 rgba(0, 0, 0, 0.3);
    li {
      margin: 0;
      padding: 7px 16px;
      cursor: pointer;
      &:hover {
        background: #eee;
      }
    }
  }
}
.tags-view-container >>> .el-scrollbar__wrap {
  margin-bottom: 0 !important;
  overflow: hidden;
}
</style>

<style lang="scss">
.tags-view-wrapper {
  .tags-view-item {
    .el-icon-close {
      vertical-align: 2px;
      border-radius: 50%;
      text-align: center;
      transition: all 0.3s cubic-bezier(0.645, 0.045, 0.355, 1);
      transform-origin: 100% 50%;
      &:before {
        transform: scale(0.6);
        display: inline-block;
        vertical-align: -3px;
      }
      &:hover {
        background-color: #b4bccc;
        color: #fff;
      }
    }
  }
}
</style>
