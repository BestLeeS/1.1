<template>
    <div class="view" v-loading="loading">
      <div class="view-top">
        <el-input v-model="SearchParams.Querytxt" placeholder="菜单名称" size="mini" clearable></el-input>
        <el-button type="primary" size="mini" @click="GetData">查询</el-button>
        <el-button type="primary" size="mini" @click="">新建</el-button>
        <el-button type="primary" size="mini" @click="">删除</el-button>
      </div>
      <div class="view-main">
        <el-table :data="tableData" height="100%" stripe @selection-change="HandleSelectChange">
          <el-table-column type="selection" width="55">
          </el-table-column>
          <el-table-column prop="orderNum" label="排序" width="50">
          </el-table-column>
          <el-table-column prop="UserName" label="用户名">
          </el-table-column>
          <el-table-column prop="LoginName" label="登录名">
          </el-table-column>
          <el-table-column prop="IDCardNum" label="身份证号">
          </el-table-column>
          <el-table-column prop="PhoneNum" label="手机号">
          </el-table-column>
          <el-table-column prop="Photo" label="照片">
          </el-table-column>
          <el-table-column prop="Signature" label="签名">
          </el-table-column>
          <el-table-column prop="Remark" label="备注">
          </el-table-column>
          <el-table-column prop="orderNum" label="操作">
            <template slot-scope="scope">
              <el-button type="text" size="mini" @click="Edit(scope.row.UserID)">编辑</el-button>
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
        Edit(UserID){
          this.Params.InnerCode = UserID;
          this.Params.Visible = true;
        },
        GetData(){
          this.loading = true;
          this.$axios
          .post('api/SysUser/GetSysUserList',this.SearchParams)
          .then(res=>{
            if(res.data.Status == 1){
              this.tableData = res.data.Entity.ResultData;
              this.SearchParams.Total = res.data.Entity.TotalCount;
            }
            else 
              this.$message.error(res.data.Entity.Message);
            this.loading = false;
          })
          .catch(err=>{
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