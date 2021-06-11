import moment from 'moment';

export default {
	// 格式化时间
	formatDate: (val, pattern = 'YYYY-MM-DD HH:mm:ss') => {
		if (!val) {
			return '';
		}
		return moment(val).format(pattern);
	},
	//根据code获取医生名字
	getDoctorName: (val, data) => {
		for (let i = 0; i < data.length; i++) {
			if (data[i].operatorCode == val) {
				return data[i].userName;
			}
		}
	},
	//根据日期获取星期
	getWeek: (val) => {
		if (!val) {
			return '';
		}
		return moment(val).locale('zh-cn').format('dddd');
	},
	//根据日期获取是否为节假日
	filterHoliday: (val, data) => {
		if (!data.length) return '工作日'
		if (data.indexOf(val) === -1) {
			return '工作日'
		} else {
			return '节假日'
		}
	},
	// 保留2为小数,key为false(默认)表示不四舍五入，为true表示四舍五入
	numFilter: (val, key = false) => {
		if (!parseFloat(val)) return 0;
		if (key) {
			return parseFloat(val).toFixed(2);
		} else {
			val = parseFloat(val).toFixed(3);
			return val.substring(0, val.length - 1);
		}
	},
	// 交易类型
	tradetype: val => {
		switch (val) {
			case 1:
				return '现金';
			case 2:
				return 'POS';
			case 3:
				return '支票';
			case 4:
				return '汇票';
			case 5:
				return '转账';
			case 6:
				return '支付宝';
			case 7:
				return '微信';
			default:
				return '其他'
		}
	},
	boolFilter: val => {
		return val ? "是" : "否";
	},
	// 发票状态
	invoiceState: val => {
		switch (val) {
			case 1:
				return '初始化';
			case 2:
				return '已使用';
			case 3:
				return '退费回收';
			case 4:
				return '废弃回收'
			default:
				return '其他'
		}
	}
}
