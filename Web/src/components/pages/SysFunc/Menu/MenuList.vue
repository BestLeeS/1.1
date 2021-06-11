<template>
  <div class="view" v-loading="loading">
    <div class="view-top">
      <el-input v-model="SearchParams.QueryName" placeholder="菜单名称" size="mini" clearable @change=""></el-input>
      <el-button type="primary" size="mini" @click="GetData">查询</el-button>
      <el-button type="primary" size="mini" @click="NewMenu">新建</el-button>
      <el-button type="primary" size="mini" @click="DelMenu">删除</el-button>
    </div>
    <div class="view-main">
      <el-table :data="tableData" height="100%" stripe @selection-change="HandleSelectChange">
        <el-table-column type="selection" width="55">
        </el-table-column>
        <el-table-column prop="orderNum" label="排序" width="50">
        </el-table-column>
        <el-table-column prop="index" label="索引">
        </el-table-column>
        <el-table-column prop="title" label="功能标题">
        </el-table-column>
        <el-table-column prop="componentpath" label="组件文件路径" >
        </el-table-column>
        <el-table-column prop="redirect" label="重定向跳转" >
        </el-table-column>
        <el-table-column prop="icon" label="图标" >
        </el-table-column>
        <el-table-column prop="parentName" label="父级菜单名称">
        </el-table-column>
        <el-table-column prop="orderNum" label="排序">
        </el-table-column>
        <el-table-column prop="Enable" label="启用">
          <template slot-scope="scope">
            {{scope.row.Enable?"√":"×"}}
          </template>
        </el-table-column>
        <el-table-column prop="orderNum" label="操作">
          <template slot-scope="scope">
            <el-button type="text" size="mini" @click="Edit(scope.row.id)">编辑</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>
    <div class="view-bottom">
      <el-pagination 
        @size-change="HandleSizeChange" 
        @current-change="HandleCurrentChange"
        :current-page="SearchParams.CurrentPage" 
        :page-sizes="SearchParams.PageSizes" 
        :page-size="SearchParams.PageSize"
        layout="total, sizes, prev, pager, next, jumper" 
        :total="SearchParams.Total"
      >
      </el-pagination>
    </div>
    <CreateOrEdit
      v-if="Params.Visible"
      :Visible.sync="Params.Visible"
      @GetData="GetData" 
      :Params="Params"
    ></CreateOrEdit>
  </div>
</template>
<script>
  import CreateOrEdit from './CreateOrEdit.vue';
  export default {
    name:"MenuList",
    components:{
      CreateOrEdit
    },
    data(){
      return{
        MultipleSelection:[],
        FilterTxt:"",
        tableData: [],
        loading:false,
        SearchParams: {
          PageIndex: 1,
          PageSize: 50,
          PageSizes: [50, 100, 200, 300, 400, 500],
          CurrentPage: 1,
          Total: 0,
          QueryName:null
        },
        Params:{
          Visible:false
        }
      };
    },
    methods: {
      DelMenu(){
        if(this.MultipleSelection.length == 0)
          return this.$message.error("请选择要删除的数据!");
        else{
          let innerCodes = this.MultipleSelection.map(x=>x.id);
          this.$axios
            .post('api/SysFunc/DelMenus',innerCodes)
            .then(res=>{
              if(res.data.Status == 1){
                this.$message.success("删除成功!");
                this.GetData();
              }
              else 
                this.$message.error(res.data.Entity.Message);
              this.loading = false;
            })
            .catch(err=>{
              this.$message.error(err.toString());
              this.loading = false;
            })
        }
      },
      Edit(innerCode){
        this.Params.MenuList = this.tableData.filter(x=> x.parentID == this.$EmptyGuid);
        this.Params.InnerCode = innerCode;
        this.Params.Visible = true;
      },
      GetData(){
        this.loading = true;
        this.$axios
        .post('api/SysFunc/GetMenuList',this.SearchParams)
        .then(res=>{
          if(res.data.Status == 1){
            this.tableData = res.data.Entity.ResultData;
            this.SearchParams.Total = res.data.Entity.TotalCount;
          }
          else 
            this.$message.error(res.data.Entity.Message);
          this.loading = false;
        })
        .catch(res=>{
          this.$message.error(err.toString());
          this.loading = false;
        })
      },
      NewMenu(){
        this.Params.MenuList = this.tableData.filter(x=> x.parentID == this.$EmptyGuid);
        this.Params.InnerCode = null;
        this.Params.Visible = true;
      },
      HandleSelectChange(val){
        this.MultipleSelection = val;
      },
      HandleSizeChange(val) {
        this.SearchParams.PageSize = val;
        this.GetData();
      },
      HandleCurrentChange() {
        this.SearchParams.CurrentPage = val;
        this.GetData();
      }
    },
    mounted() {
      
    },
    created() {
      this.GetData();
    }
  };
</script>