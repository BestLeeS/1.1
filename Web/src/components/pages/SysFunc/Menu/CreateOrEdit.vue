<template>
  <el-dialog :title="Title" :visible.sync="Visible" :close-on-click-modal="false" :before-close="CloseModal" width="670px" :append-to-body="true">
    <el-form ref="form" :model="Form" :rules="rules" label-width="80px" size="mini" v-loading="loading">
      <el-row>
        <el-col :span="12">
          <el-form-item style="width: 250px;" label="菜单索引" prop="MenuName">
            <el-input v-model="Form.MenuName"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item style="width: 250px;" label="功能标题" prop="Title">
            <el-input v-model="Form.Title"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item style="width: 250px;" label="文件路径" prop="ComponentPath">
            <el-input v-model="Form.ComponentPath"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item style="width: 250px;" label="重定向" prop="Redirect">
            <el-input v-model="Form.Redirect"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item style="width: 250px;" label="图标" prop="Icon">
            <el-input v-model="Form.Icon"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item style="width: 250px;" label="上级菜单" prop="ParentID">
            <el-select v-model="Form.ParentID" placeholder="请选择">
              <el-option
                v-for="item in Params.MenuList"
                :key="item.id"
                :label="item.title"
                :value="item.id">
              </el-option>
            </el-select>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item style="width: 250px;" label="排序" prop="OrderNum">
            <el-input-number v-model="Form.OrderNum" :min="0" :max="10" :step="1" :controls="false" :step-strictly="true"></el-input-number>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item style="width: 250px;" label="启用" prop="Enable">
            <el-switch v-model="Form.Enable" active-color="#13ce66" inactive-color="#ff4949">
            </el-switch>
          </el-form-item>
        </el-col>
      </el-row>      
    </el-form>
    <span slot="footer" class="dialog-footer">
      <el-button size="mini" @click="CloseModal">取 消</el-button>
      <el-button size="mini" type="primary" @click="Save">确 定</el-button>
    </span>
  </el-dialog>
</template>
<script>
  export default {
    props: {
      Visible: {
        type: Boolean,
        default: false
      },
      Params:{
        MenuList:[],
        InnerCode:null
      }
    },
    data() {
      return {
        loading:false,
        Title: "",
        Form:{
          ID:null,
          MenuName:null,
          Title:null,
          Path:null,
          ComponentPath:null,
          Redirect:null,
          Icon:null,
          ParentID:null,
          OrderNum:0,
          Enable:true
        },
        rules:{
          MenuName:[
            { required: true, message: '请输入菜单索引', trigger: 'blur' }
          ],
          Title:[
            { required: true, message: '请输入功能标题', trigger: 'blur' }
          ],
          Path:[
            { required: true, message: '请输入跳转路径', trigger: 'blur' }
          ]
        }
      }
    },
    methods: {
      Init(){
        if(this.Params.InnerCode && this.Params.InnerCode != this.$EmptyGuid){
          this.Title = "编辑菜单";
          this.loading = true;
          this.$axios
          .post('api/SysFunc/GetMenuByInnerCode',{InnerCode:this.Params.InnerCode})
          .then(res=>{
            if(res.data.Status == 1){
              let entity = res.data.Entity;
              this.Form.ID = entity.id;
              this.Form.MenuName = entity.name;
              this.Form.Title = entity.title;
              this.Form.Path = entity.componentpath;
              this.Form.ComponentPath = entity.componentpath;
              this.Form.Redirect = entity.redirect;
              this.Form.Icon = entity.icon;
              this.Form.ParentID = entity.parentID == this.$EmptyGuid ? null : entity.parentID;
              this.Form.OrderNum = entity.orderNum;
              this.Form.Enable = entity.Enable;
            }
            else
              this.$message.error(res.data.Message);
            this.loading = false;
          })
          .catch(err=>{
            this.loading = false;
            this.$message.error(err.toString());
          })
        }
        else{
          this.Title = "新建菜单";
          this.Form.ID = this.$EmptyGuid;
        }
      },
      Save(){
        this.$refs.form.validate(valid => {
          if(valid){
            this.loading = true;
            this.$axios
              .post('api/SysFunc/InsertOrUpdateMenu', this.Form)
              .then(res => {
                if (res.data.Status == 1) {
                  this.$message.success("保存成功,重新登录生效!");
                  this.$emit("GetData");
                  this.CloseModal();
                }
                else
                  this.$message.error(res.data.Message);
                this.loading = false;
              })
              .catch(err => {
                this.loading = false;
                this.$message.error(err.toString());
              })
          } 
        });
      },
      CloseModal() {
        this.$emit("update:Visible", false);
      },
    },
    created() {
      this.Init();
    },
    computed: {

    }
  }
</script>