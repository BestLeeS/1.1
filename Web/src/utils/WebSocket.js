import {
	Message,
	Notification
} from 'element-ui';
import axios from 'axios';
import {
	async
} from 'q';

let Ws = null;
let isConnect = false;
let rec = null;
let errCount = 0;
let WebSocketConnect = ((onmessageCallBack) => {
	let baseURL = '127.0.0.1:8883';
	if (process.env.NODE_ENV === 'production') {
		baseURL = window.g.ProDuctWSURL;
	}
	Ws = new WebSocket(`ws://${baseURL}?UserID=${window.UserInfo.UserID}&UserName=${window.UserInfo.UserName}`);
	Ws.onopen = function () {
		console.log('与消息中心建立连接!');
		//连接建立后修改标识
		isConnect = true;
		//错误计数清空
		errCount = 0;
		// 建立连接后开始心跳
		heartCheck.start();
	};
	Ws.onmessage = onmessageCallBack;
	Ws.onerror = function (e) {
		console.error(e.data);
		isConnect = false;
		if (errCount <= 20)
			reConnect();
	};
	Ws.onclose = function () {
		console.error("WebSocket连接关闭");
		isConnect = false;
	}
	return Ws;
});

//心跳设置
let heartCheck = {
	timeout: 20000,
	timeoutObj: null,
	//一段时间后发送心跳包
	start: function () {
		this.timeoutObj = setTimeout(function () {
			if (isConnect) Ws.send(JSON.stringify({
				SenderId: window.UserInfo.UserID,
				ReceiverId: window.UserInfo.UserID,
				msgContentObjs: [{
					type: -1,
					needRead:false
				}]
			}));
		}, this.timeout);
	},
	//    接收到服务器的信息之后要重置心跳发送的方法
	reset: function () {
		clearTimeout(this.timeoutObj);
		this.start();
	},
};

let reConnect = () => {
	console.log(`第 ${errCount}/20 次,尝试重新连接`);
	//如果已经连上就不在重连了
	// 延迟10秒重连  避免过多次过频繁请求重连
	//最多重连20次
	if (isConnect) {
		clearTimeout(rec);
		return;
	} else {
		rec = setTimeout(function () {
			WebSocketConnect();
		}, 10000);
	}
	errCount++;
};

export default {
	Ws: Ws,
	//WebSocket 注册
	WebSocketConnect: WebSocketConnect,
	heartCheck: heartCheck,
	// Msg:要发送的消息 string
	WebSocketSendMsg: ((MsgObj) => {
		try 
		{
			if (Ws.readyState != 1) 
				throw new Error();
			Ws.send(JSON.stringify(MsgObj));
		} 
		catch (e) {
			Message.error("与消息中心失去连接,正在重连···");
			WebSocketConnect();
		}
	}),
	SetMsgRead: function (msgIDArry) {
		return new Promise((resolve, reject) => {
			axios
				.get('/api/XT/SetXtMsgRead', {
					params: {
						UserID: window.UserInfo.UserID,
						MsgID: msgIDArry
					}
				})
				.then(res => {
					if (res.data.status == 1)
						resolve(true);
					else
						resolve(false);
				})
				.catch(err => {
					console.error(err)
					reject(false);
				})
		})

	},
	GetNotReadMsgs: function (lastTime) {
		return new Promise((resolve, reject) => {
			axios
				.get('/api/XT/GetXtMsg', {
					params: {
						UserID: window.UserInfo.UserID,
						lastTime: lastTime
					}
				})
				.then(res => {
					if (res.data.status == 1)
						resolve(JSON.parse(res.data.entity));
					else
						resolve([]);
				})
				.catch(err => {
					console.error(err)
					reject(err);
				})
		})
	}
}
