<template>
    <el-popover
      placement="bottom-end"
      width="450"
      trigger="click">
      <div class="view" v-loading="loading" style="height:460px">
        <div class="view-main">
          <div class="onlineUsers">
            <!-- <el-radio-group v-model="selectedUserID" style="width:145px;text-align:left;" :border="false">
              <el-radio-button class="DivBreathe" style="width:145px;text-align:left;" v-for="user in onlineUsers" :key="user.UserID" :label="user.UserID">{{user.UserName}}</el-radio-button>
            </el-radio-group> -->
            <div class="DivBreathe" style="width:145px;text-align:center;margin-top:3px;" 
              v-for="user in onlineUsers" 
              :key="user.UserID" 
              :label="user.UserID">
              <span :class="'iconfont ' + (user.IsOnline ? 'icon-zaixian' : 'icon-lixian-xian') "></span>{{user.UserName}}
            </div>
          </div>
          <div class="msgOp">
            <div class="msgRead" v-html="msgLogtxt"></div>
            <div class="msgSend">
              <el-input type="textarea" style="width:100%;height:100%;" :rows="9" placeholder="请输入发送的消息" v-model="txtMsg"></el-input>
            </div>
          </div>
        </div>
        <div class="view-bottom">
          <div class="msgOpBtn">
            <el-button type="primary" @click="GetOnlineUsers" size="mini"><i class="iconfont icon-fasong" ></i>AA</el-button>
            <el-button type="primary" size="mini"><i class="iconfont icon-qingkongbeifen"></i>清空</el-button>
            <el-button type="primary" @click="SendMsg" size="mini"><i class="iconfont icon-fasong"></i>发送</el-button>
          </div>
        </div>
      </div>
      <el-badge :value="notReadMsgCount" style="margin-right:30px;margin-left:30px;width:50px;" slot="reference" :class="">
        <i class="el-icon-message-solid" style="font-size: 22px;color: white;"></i>
      </el-badge>
    </el-popover>
</template>
<script>
import WebSocket from '../../utils/WebSocket';
import _ from 'lodash';
import moment from 'moment';

export default {
  data() {
    return {
      onLineClass:"",
      loading:false,
      WebSocket:null,
      MsgPool:[],
      msgMainLoading:false,
      notReadMsgCount:0,
      onlineUsers:[],
      selectedUserID:null,
      txtMsg:"",
      msgLogtxt:"",
      BreatheClassName:""
    };
  },
  created() {
    
  },
  methods: {
    SendMsg(){
      if(!this.selectedUserID)
        return this.$message.error("请选择要发送消息的人!");
      let msg = {
        senderId: window.UserInfo.UserID,
        receiverId: this.selectedUserID,
        msgContentObjs: [{
          msgID: this.$uuid.v1(),
          senderName:window.UserInfo.UserName,
          needRead: true,
          Type: 1,
          title: "",
          createTime: moment().format('YYYY-MM-DD hh:mm:ss'),
          content: this.txtMsg
        }]
      }
      if(msg.msgContentObjs.length > 0){
        msg.msgContentObjs.forEach(x=>{
          this.msgLogtxt = _.cloneDeep(this.msgLogtxt.concat(`
          <br /><span style='color:DarkGray;float:right'>${x.senderName} ${x.createTime.replace("T","")}</span>
          <br /><span style='color:DeepSkyBlue;float:right'>${x.content}</span>
          `))
        });
      }
      WebSocket.WebSocketSendMsg(msg);
      this.txtMsg = "";
    },
    CloseSingleMsg(InnerCode){
      let singleMsg = this.MsgPool.find(x=>x.InnerCode == InnerCode);
      if(singleMsg && singleMsg.notify)
        singleMsg.notify.then(res=>{
          this.NoteRead(singleMsg);
          res.close();
        })
    },
    async GetOnlineUsers(){
        this.msgMainLoading = true;
        await this.$axios
        .get('/api/Demo/GetOnlineUsers')
        .then(res=>{
          if(res.data.Status === 1)
            this.onlineUsers = res.data.Entity.map(x=>{
              x.UserID = x.UserID.toUpperCase()
              return x;
            });
          this.msgMainLoading = false;
        })
        .catch(err=>{
          this.$message.error(err.toString());
          this.msgMainLoading = false;
        })
    },
    InitWs(){
      let _this = this;
      WebSocket.WebSocketConnect(ev=>{
        let msgRecvCount = 0;
        let msgObjs = JSON.parse(ev.data);
        //根据类型处理消息 
        //-1 心跳重置,0 全体广播消息,1 定向发送消息,2 上线通知消息,3 下线通知消息
        if(msgObjs && msgObjs.length > 0){
          if(msgRecvCount > 300)
            _this.msgLogtxt = "";
          let msgType = msgObjs[0].type;
          switch(msgType) {
              case -1:
                WebSocket.heartCheck.reset();
                break;
              case 0:
                  
                break;
              case 1:
                
                let appiontMsgs = msgObjs.filter(x=>x.type == 1);
                if(appiontMsgs.length > 0){
                  _this.BreatheClassName = "DivBreathe";
                  setTimeout(()=>{
                    _this.BreatheClassName = "";
                  },3000)
                  appiontMsgs.forEach(x=>{
                    _this.msgLogtxt = _.cloneDeep(_this.msgLogtxt.concat(`
                    <br /><span style='color:DarkGray;float:left'>${x.senderName} ${x.createTime.replace("T","")}</span>
                    <br /><span style='color:DarkOrange;float:left'>${x.content}</span>
                    `))
                  });
                }

                break;
              case 2:
                debugger
                let tmpUsers = _this.onlineUsers.find(x=>x.UserID == msgObjs[0].senderID);
                if(tmpUsers)
                  tmpUsers.IsOnline = true;
                else
                  _this.onlineUsers.push({
                    UserID:msgObjs[0].senderID,
                    UserName:msgObjs[0].senderName,
                    IsOnline:true
                  });

                break;
              case 3:
                let tmpUsers3 = _this.onlineUsers.find(x=>x.UserID == msgObjs[0].senderID);
                if(tmpUsers3)
                  tmpUsers3.IsOnline = false;
                break;
              default:

                  break;
          } 
        }
      });
    }
  },
  watch: {

  },
  async mounted(){
    await this.GetOnlineUsers()
    this.InitWs();
  },
  created() {
    
  }
};
</script>
<style scoped>
.el-badge >>> .el-badge__content{
    margin-top: 10px;
}
.view-main{
  display: flex;
}
.onlineUsers{
  height: 383px;
  width: 145px;
  border: 1px solid #DCDFE6;
  border-radius: 7px;
}
.onlineUsers >>> .el-radio-button__inner{
  width: 100%;
  text-align: left;
}
.msgOp{
  width: 280px;
  padding-left: 3px;
}
.msgRead{
  height: 167px;
  width: 265px;
  border: 1px solid #DCDFE6;
  border-radius: 7px;
  overflow-y: auto;
  overflow-x: hidden;
  padding: 5px;
  margin-left: 5px;
}
.msgSend{
  height: 167px;
  width: 99%;
  padding: 5px;
}
.msgOpBtn{
  padding-top: 3px;
  text-align: right;
}
.el-badge >>> sup{
  margin-right: 10px;
  margin-bottom: 10px;
}
.el-badge >>> .el-icon-message-solid{
  margin-right: 10px;
}

</style>