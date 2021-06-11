<template>
    <div class="view" v-loading="loading">
      <div class="view-top">
        <el-input v-model="SearchParams.Querytxt" placeholder="菜单名称" size="mini" clearable></el-input>
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
    </div>
  </template>
  <script>

    export default {
      name:"MenuList",
      components:{
        
      },
      data(){
        return{
          multipleSelection:[],
          tableData: [],
          loading:false,
          SearchParams: {
            PageIndex: 1,
            PageSize: 50,
            PageSizes: [50, 100, 200, 300, 400, 500],
            CurrentPage: 1,
            Total: 0,
            Querytxt:null
          },
          Params:{
            Visible:false
          }
        };
      },
      methods: {
        Del(){
          
        },
        Edit(innerCode){
         
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
        New(){

        },
        HandleSelectChange(val){
          this.multipleSelection = val;
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