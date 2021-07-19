<template>
    <el-popover
    placement="bottom-end"
    width="440"
    trigger="click">
      <div class="view" v-loading="loading">
        <div class="view-main">
          <div class="onlineUsers">

          </div>
          <div class="msgOp">
            <div class="msgRead">

            </div>
            <div class="msgSend">

            </div>
            <div class="msgOpBtn">
              <el-button type="primary" size="mini"><i class="iconfont icon-kaishi2"></i>发送</el-button>
              <el-button type="primary" size="mini"><i class="iconfont icon-kaishi2"></i>清空</el-button>
            </div>
          </div>
        </div>
      </div>
      <el-badge :value="notReadMsgCount" style="margin-right: 10px;" slot="reference" >
        <i class="el-icon-message-solid" style="font-size: 22px;color: white;"></i>
      </el-badge>
    </el-popover>
</template>
<script>
import WebSocket from '../../utils/WebSocket';
import _ from 'lodash';

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
    GetOnlineUsers(){
      this.msgMainLoading = true;
      this.$axios
      .get('/api/Demo/GetOnlineUsers')
      .then(res=>{
        debugger
      })
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
        //心跳检测重置
        if(content.NotReadMsgCount == -1 && content.Flag == "heartCheck"){
          WebSocket.heartCheck.reset();
        }
      }  
    });
    this.GetOnlineUsers();
  }
};
</script>
<style scoped>
.el-badge >>> .el-badge__content{
    margin-top: 20px;
}
.view-main{
  display: flex;
}
.onlineUsers{
  height: 400px;
  width: 150px;
  border: 1px solid black;
}
.msgOp{
  width: 280px;
  border: 1px solid red;
  padding-left: 3px;
}
.msgRead{
  height: 45%;
  width: 99%;
  border: 1px solid black;
}
.msgSend{
  height: 45%;
  width: 99%;
  border: 1px solid black;
}
.msgOpBtn{
  padding-top: 3px;
  text-align: right;
}
</style>