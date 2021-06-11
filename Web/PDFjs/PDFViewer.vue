
<template>
  <el-dialog
    :title="Prams.CtrlFlag=='Add'?'增加':'编辑'"
    :visible.sync="visible"
    width="330px"
    :before-close="modalClose"
  >
    <el-form
      ref="dataForm"
      :rules="rules"
      :model="FormData"
      label-position="right"
      label-width="100px"
      size="mini"
    >
      <el-form-item label="热区编号:" prop="code">
        <el-input ref="code" v-model="FormData.code" placeholder="请输入" />
      </el-form-item>
      <el-form-item label="热区名称:" prop="name">
        <el-input ref="name" v-model="FormData.name" placeholder="请输入" />
      </el-form-item>
      <el-form-item label="热区类型:" prop="type">
        <el-select v-model="FormData.type" filterable placeholder="请选择" @change="showFlnum">
          <el-option v-for="item in options" :key="item.id" :label="item.name" :value="item.code"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="楼层数:" prop="floorNum" v-if="isShowFlnum">
        <el-input ref="floorNum" v-model="FormData.floorNum" placeholder="请输入" maxlength="2" />
      </el-form-item>
      <el-form-item label="对应房间:" prop="externalCode" v-if="isShowRoomNum">
        <el-input ref="externalCode" v-model="FormData.externalCode" placeholder="请输入" />
      </el-form-item>
      <el-form-item label="热区地址:" prop="addr">
        <el-input ref="addr" v-model="FormData.addr" placeholder="请输入" />
      </el-form-item>
      <el-form-item label="上级热区:" prop="pname">
        <el-input
          ref="pname"
          v-model="FormData.pname"
          placeholder="请输入"
          disabled
          @click.native="TreeVisible=true"
        />
      </el-form-item>
    </el-form>

    <span slot="footer" class="dialog-footer">
      <el-button @click="modalClose" size="mini">取 消</el-button>
      <el-button type="primary" @click="Save" size="mini">保 存</el-button>
    </span>
    <el-dialog
      title="所属热区"
      :visible.sync="TreeVisible"
      width="330px"
      append-to-body
      @close="TreeVisible=false"
    >
      <div class="Tree_Div">
        <el-input placeholder="输入关键字进行过滤" v-model="filterText"></el-input>
        <el-scrollbar style="height:290px;">
          <el-tree
            class="filter-tree"
            :data="TreeData"
            :props="defaultProps"
            default-expand-all
            highlight-current
            @node-click="SaveTreeID"
            :filter-node-method="filterNode"
            ref="tree"
          ></el-tree>
        </el-scrollbar>
      </div>
    </el-dialog>
  </el-dialog>
</template>


<script>
export default {
    name:'PDFViewer'
}
</script>


