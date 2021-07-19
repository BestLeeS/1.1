<template>
  <div class="login-wrap">
    <div class="bg bg-blur"></div>
    <div class="ms-login">
      <el-form :model="ruleForm" :rules="rules" ref="ruleForm" label-width="0px" class="ms-content">
        <div v-if="isSuccess" role="alert" class="el-message el-message--success is-center is-closable message"
          :style="msgStyle">
          <i class="el-message__icon el-icon-success"></i>
          <p class="el-message__content">{{ msg }}</p>
        </div>
        <div v-else role="alert" class="el-message el-message--error is-center is-closable message" :style="msgStyle">
          <i class="el-message__icon el-icon-error"></i>
          <p class="el-message__content">{{ msg }}</p>
        </div>
        <span class="ms-title">登录</span>
        <el-form-item prop="LoginId">
          <el-input v-model="ruleForm.LoginId" placeholder="账号" clearable @keyup.enter.native="submitForm('ruleForm')"
            class="username" size="mini">
            <i slot="prefix" class="iconfont">&#xe630;</i>
          </el-input>
        </el-form-item>
        <el-form-item prop="Pwd">
          <el-input type="password" placeholder="登录密码" v-model="ruleForm.Pwd" clearable
            @keyup.enter.native="submitForm()" size="mini">
            <i slot="prefix" class="iconfont">&#xe66e;</i>
          </el-input>
        </el-form-item>
        <!-- <el-checkbox style="padding-bottom: 5px;" v-model="checked">记住密码</el-checkbox> -->
        <div class="login-btn">
          <el-button type="primary" @click="submitForm()" :loading="isLogin">登录</el-button>
        </div>
      </el-form>
      <span class="GroupInfo">Powered By BestLee</span>
    </div>
  </div>
</template>

<script>
  import _ from "lodash";
  import promission from "./../../router/promission.js";
  export default {
    name: "Login",
    data() {
      return {
        checked: true,
        isLogin: false,
        ruleForm: {
          LoginId: "admin",
          Pwd: "1"
        },
        msgStyle: { opacity: 1, display: "none" },
        isSuccess: true,
        msg: "",
        rules: {
          LoginId: [{ required: true, message: "请输入用户名", trigger: "blur" }],
          Pwd: [{ required: true, message: "请输入密码", trigger: "blur" }]
        }
      };
    },
    mounted() {
      this.$message.close();
      this.getCookie();
      document.querySelector(".username input").focus();
    },
    methods: {
      msgShow_Hide() {
        this.msgStyle.display = "inline-flex";
        this.msgStyle.opacity = 1;
        let timeMS = 1500;
        for (let index = 0; index < timeMS; index++) {
          setTimeout(() => {
            this.msgStyle.opacity = 1 - (1 / timeMS) * index;
          }, 1);
        }
      },
      submitForm() {
        this.$refs.ruleForm.validate(validate=>{
          if(validate){
            this.isLogin = true;
            this.$axios
            .post('api/Login/GetToken',{loginName:this.ruleForm.LoginId,passWord:this.ruleForm.Pwd})
            .then(res=>{
              if(res.data.Status == 1){
                window.Token = res.data.Entity.Token;
                window.UserID = res.data.Entity.UserID;
                this.$message.success("登录成功!");
                promission.LoadMenus(res.data.Entity.Menus,true);
                setTimeout(() => {
                  localStorage.setItem("USERINFO",JSON.stringify(res.data.Entity))
                  this.$router.replace("/");
                }, 100);
              }
              else{
                this.$message.error(res.data.Message);
              }
              this.isLogin = false;
            })
            .catch(err=>{
              this.isLogin = false;
              this.$message.error(err.toString());
            })
          }
        });
      },
      //设置cookie
      setCookie(c_name, c_pwd, exdays) { },
      getCookie: function () { },
      clearCookie() {
        this.setCookie("", "", -1);
      }
    },
    created() {
      document.onkeydown = function (e) {
          var ev = window.event || e;
          var code = ev.keyCode || ev.which;
          if (code == 116) {
              if(ev.preventDefault) {
                  ev.preventDefault();
              } else {
                  ev.keyCode = 0;
                  ev.returnValue = false;
              }
          }
      }
    }
  };
</script>

<style scoped>

  .bg {
    background: url(../../assets/img/login-bg.jpg);
    height: calc(100% + 10px);
    width: calc(100% + 18px);
    text-align: center;
    overflow: hidden;
  }

  .bg-blur {
    float: left;
    margin-left: -8px;
    background-repeat: no-repeat;
    background-position: center;
    background-size: cover;
    -webkit-filter: blur(5px);
    -moz-filter: blur(5px);
    -o-filter: blur(5px);
    -ms-filter: blur(5px);
    filter: blur(5px);
  }

  .login-wrap {
    position: absolute;
    width: 100%;
    height: 100%;
    overflow: hidden;
  }

  .ms-title {
    line-height: 50px;
    margin-left: 55px;
    font-size: 20px;
  }

  .ms-login {
    position: absolute;
    left: 50%;
    top: 50%;
    width: 500px;
    height: 330px;
    margin: -190px 0 0 -250px;
    border-radius: 5px;
    background: #f3f6f7;
    overflow: hidden;
  }
.ms-login >>> .el-form-item__error{
  padding-top: 0px;
}
.ms-login >>> .el-form-item__content{
  margin-top: -10px;
}
  .ms-content {
    padding: 15px 30px;
    width: 150px;
    margin: 0 auto;
    margin-top: 10px;
  }

  .login-btn {
    text-align: center;
  }

  .login-btn button {
    width: 100%;
    height: 36px;
    margin-bottom: 10px;
  }

  .login-tips {
    font-size: 12px;
    line-height: 30px;
    color: #fff;
  }

  .GroupInfo {
    color: #c0c4cc;
    font-size: 8px;
    margin-left: 176px;
  }

  .message {
    position: absolute;
    margin-top: -18px;
    text-align: center;
  }
</style>