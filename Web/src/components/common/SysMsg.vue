<template>
    <el-popover
        placement="bottom-end"
        width="400"
        trigger="click">
        <div class="msg-main" v-loading="msgMainLoading">
          <div class="msg-func">
            <span style="font-size:20px;">消息通知</span> &nbsp;&nbsp;
            <el-popover
              
              placement="left"
              width="120"
              trigger="click">
              <div>消息设置</div>
              <div class="msgset-main" v-loading="msgSetLoading">
                <el-switch
                  v-for="item in MsgTypeData"
                  :key = "item.value"
                  v-model="item.allow"
                  :inactive-text="item.name"
                  @change="SetChange"
                  style="margin-top:5px;"
                  active-color="#13ce66"
                  inactive-color="#ff4949">
                </el-switch>
              </div>
              <el-link slot="reference" type="primary" class="msg-set" @click="MsgSetVisible = !MsgSetVisible">设置</el-link>
            </el-popover>
            
            <el-tooltip class="item" effect="dark" content="全部标记为已读" placement="bottom">
              <i class="el-icon-finished" style="font-size: 22px;float: right;" @click="NoteAllRead"></i>
            </el-tooltip>
          </div>
          <div class="msg-body">
            <div v-for="msg in MsgPool" :key="msg.InnerCode" class="msg-body-body" v-loading="msg.msgLoading" >
              <div class="msg-title"><h2 class="el-notification__title" style="width:70%;">{{msg.Title}}</h2> <el-link type="primary" @click="NoteRead(msg)"  class="msg-isread">{{msg.IsRead?'已读':'标记为已读'}}</el-link></div>
              <div v-html="msg.SMSContent" class="msg-content"></div>
            </div>
          </div>
          <div class="msg-more">
            <el-link type="primary" @click="GetMoreMsg(true)">加载更多</el-link>
          </div>
        </div>
        <el-badge :value="notReadMsgCount" style="margin-right: 10px;" slot="reference" >
          <i class="el-icon-message-solid" style="font-size: 22px;color: white;"></i>
        </el-badge>
    </el-popover>
</template>
<script>
import WebSocket from '../../utils/WebSocket';
import { Message,Notification } from 'element-ui';
import Vue from 'vue/dist/vue.esm.js';
import _ from 'lodash';
import localforage from 'localforage';
import moment from 'moment';

export default {
  data() {
    return {
      value:false,
      MsgSetVisible:false,
      WebSocket:null,
      MsgPool:[],
      MsgDivKey:0,
      msgMainLoading:false,
      notReadMsgCount:0,
      notifyPromise:Promise.resolve(),//解决弹窗重叠问题
      MsgTypeData:[],
      MsgTypeAllowData:[],
      msgSetLoading:false,
      heartCheckStart:null
    };
  },
  created() {
    
  },
  methods: {
    CloseSingleMsg(InnerCode){
      let singleMsg = this.MsgPool.find(x=>x.InnerCode == InnerCode);
      if(singleMsg && singleMsg.notify)
        singleMsg.notify.then(res=>{
          this.NoteRead(singleMsg);
          res.close();
        })
    },
    async NoteRead(Msg){
      Msg.msgLoading = true;
      if(!Msg.IsRead){
       await WebSocket.SetMsgRead(Msg.InnerCode)
        .then(res=>{
          if(res){
            Msg.IsRead = true;
            this.CloseSingleMsg(Msg.InnerCode);
            if(this.notReadMsgCount > 0)
              this.notReadMsgCount = this.notReadMsgCount - 1; 
          }
        });
      }
      Msg.msgLoading = false;  
    },
    async NoteAllRead(){
      this.msgMainLoading = true;
      let InnerCodeArry = [];
      this.MsgPool.forEach(x=>{
        if(!x.isRead)
          InnerCodeArry.push(x.InnerCode);
      })
       await WebSocket.SetMsgRead(InnerCodeArry)
        .then(res=>{
          this.MsgPool.forEach(x=>{
            x.IsRead = true;
          })
          this.notReadMsgCount = 0;
        });
      this.msgMainLoading = false;
    },
    MsgShow(msg,isGetMore){
      const h = this.$createElement;
      const MsgDiv = h(
        "div",
        {
          style: {
            'width': "270px",
            "min-height": "50px",
            "max-heght": "180px",
            "overflow-x": "hidden",
            "overflow-y": "auto",
            "margin-top": "-15px",
            "margin-bottom": "-10px"
          }
        },
        [
          h(
            "h2",
            {
              style: {
                width: "90%"
              },
              class: {
                "el-notification__title": true
              }
            },
            msg.Title
          ),
          h("div", {
            style: {
              top: "7px"
            },
            class: {
              "el-notification__closeBtn": true,
              "el-icon-close": true
            },
            on: {
              click: (()=>{
                this.CloseSingleMsg(msg.InnerCode);
              })
            }
          }),
          h("div", {
            domProps: {
              innerHTML: msg.SMSContent
            }
          })
        ]
      );
      if(isGetMore || msg.IsRead == true || this.MsgTypeAllowData.findIndex(x=>x == msg.Type) == -1 )
        this.MsgPool.push({
          InnerCode:msg.InnerCode,
          Title:msg.Title,
          IsRead:msg.IsRead,
          Type:msg.Type,
          CreateTime:msg.CreateTime,
          SMSContent:msg.SMSContent,
          msgLoading:false,
          notify:null
        })
      else
        this.MsgPool.unshift({
          InnerCode:msg.InnerCode,
          Title:msg.Title,
          IsRead:msg.IsRead,
          Type:msg.Type,
          CreateTime:msg.CreateTime,
          SMSContent:msg.SMSContent,
          msgLoading:false,
          notify:this.notifyPromise = this.notifyPromise.then(()=>{
            return Notification({
            showClose:false,
            dangerouslyUseHTMLString: true,
            position: 'top-right',
            type: 'warning',
            duration:0,//不可自动关闭
            offset: 100,
            message: MsgDiv,
            })
          })
        })  
        this.MsgPool = _.reverse(_.sortBy(_.clone(this.MsgPool),function(item) {return moment(item.CreateTime);}));
    },
    GetMoreMsg(flag){
      this.msgMainLoading = true;
      let time = null;
      if(this.MsgPool.length > 0)
        time = _.minBy(this.MsgPool,'CreateTime').CreateTime
      WebSocket.GetNotReadMsgs(time)
      .then(res=>{
        this.notReadMsgCount = res.NotReadMsg;
        if(res.XtMsg.length > 0){
          let msgArry = _.sortBy(res.XtMsg,function(item) {return new Date(item.CreateTime);});
          msgArry.forEach(x=>{
            let index = this.MsgPool.findIndex(y=>y.InnerCode == x.InnerCode);
            if(index == -1)
              this.MsgShow(x,flag);
          })
        }else
          this.$notify({
            title: '提示',
            message: '没有更多未读消息',
            type:'warning',
            position: 'top-right'
          });
        this.msgMainLoading = false;
      });
    },
    async InitAlertRecvTypes(){
      this.msgSetLoading = true;
      if(window.USERINFO.alertRecvTypes){
        this.MsgTypeAllowData = JSON.parse(window.USERINFO.alertRecvTypes);
      }
      if(this.MsgTypeAllowData.indexOf(4) == -1)
        this.MsgTypeAllowData.push(4);
      await this.$getType('MsgType')
      .then(res => {
        if(res.data.status == 1){
          if(this.MsgTypeAllowData.length > 0)
            res.data.entity.forEach(x=>{
              let index = this.MsgTypeAllowData.findIndex(y=>y == x.value);
              if(index > -1)
                x.allow = true;
              else
                x.allow = false;
            })
          }
        this.MsgTypeData = res.data.entity;
      });
      this.msgSetLoading = false;
    },
    async SetChange(){
      this.msgSetLoading = true;
      let MsgTypeAllowArry = [];
       this.MsgTypeData.forEach(x=>{
         if(x.allow)
          MsgTypeAllowArry.push(x.value);
       })
       await this.$axios
       .post('/api/XT/SetMsgRecType',{operatorCode:window.USERINFO.operatorCode,recTypes:MsgTypeAllowArry})
       .then(res=>{
         if(res.data.status != 1)
          this.$message.error("设置失败!");
         else{
           this.MsgTypeAllowData = MsgTypeAllowArry;
           this.MsgTypeData.forEach(x=>{
              let index = this.MsgTypeAllowData.findIndex(y=>y == x.value);
              if(index > -1)
                x.allow = true;
              else
                x.allow = false;
           })
         } 
       })
       .catch(err=>{
         this.msgSetLoading = false;
         this.$message.error("设置失败!");
       })
       this.msgSetLoading = false;
    }
  },
  watch: {

  },
  computed: {
  
  },
  created() {
    let webSocket = WebSocket.WebSocketConnect(ev=>{
      let content = JSON.parse(ev.data);
      //推送消息处理
      if(content.NotReadMsgCount >= 0){
        this.notReadMsgCount = content.NotReadMsgCount;
        let msg = JSON.parse(content.MsgsString);
        msg = _.sortBy(msg,function(item) {return new Date(item.CreateTime);})
        msg.forEach(x=>{
          let index = this.MsgPool.findIndex(y=>y.InnerCode == x.InnerCode);
          if(index == -1){
              this.MsgShow(x);
          }
        })
      }
      else{
        //处理其他事物
        if(content.NotReadMsgCount == -1 && content.Flag == "GrpCusImport"){
          this.notifyPromise = this.notifyPromise.then(()=>{
            return Notification({
            dangerouslyUseHTMLString: true,
            position: 'top-right',
            type: 'success',
            duration:0,//不可自动关闭
            offset: 100,
            title:'提示',
            message: JSON.parse(content.MsgsString)[0].SMSContent,
            })
          })
        }
        if(content.NotReadMsgCount == -1 && content.Flag == "heartCheck"){
          WebSocket.heartCheck.reset();
        }
      }  
    });
    this.GetMoreMsg(false);
    this.InitAlertRecvTypes();
  }
};
</script>
<style scoped>

.msg-main{
  height: 420px;
  width:380px;
  overflow: hidden;
}
.msg-func{
  width:370px;
  height:40px;
  border-bottom:1px solid black;
}
.msg-set{
  margin-left:25px;
}
.msg-body{
  width:380px;
  height: 350px;
  overflow-x: hidden;
  overflow-y: auto;
}
.msg-body-body{
  width:360px;
  max-height:100px;
  overflow-y: auto;
  overflow-x: hidden;
  margin-top:10px;
  border-bottom:1px dashed black;
}
.msg-title{
  width:350px;
  display:flex;
}
.msg-isread{
  float: right;
}
.msg-content{
  width:350px;
}
.msg-more{
  width:370px;
  height: 28px;
  border-top: 1px solid black;
  padding-top: 10px;
  text-align: center;
}
.el-badge >>> .el-badge__content{
    margin-top: 20px;
}
.msgset-main{
  width: 100%;
  min-height: 100px;
  text-align: right;
  padding-top: 10px;
  padding-bottom: 10px;
}
</style>