import axios from "./http";

export default {
  install: function (Vue) {
    //验证身份证号是否合法
    (Vue.prototype.$IDCardNumValid = function (code) {
      var city = {
        11: "北京",
        12: "天津",
        13: "河北",
        14: "山西",
        15: "内蒙古",
        21: "辽宁",
        22: "吉林",
        23: "黑龙江 ",
        31: "上海",
        32: "江苏",
        33: "浙江",
        34: "安徽",
        35: "福建",
        36: "江西",
        37: "山东",
        41: "河南",
        42: "湖北 ",
        43: "湖南",
        44: "广东",
        45: "广西",
        46: "海南",
        50: "重庆",
        51: "四川",
        52: "贵州",
        53: "云南",
        54: "西藏 ",
        61: "陕西",
        62: "甘肃",
        63: "青海",
        64: "宁夏",
        65: "新疆",
        71: "台湾",
        81: "香港",
        82: "澳门",
        91: "国外 ",
      };
      var row = true;
      var msg = "验证成功";

      if (
        !code ||
        !/^\d{6}(18|19|20)?\d{2}(0[1-9]|1[012])(0[1-9]|[12]\d|3[01])\d{3}(\d|[xX])$/.test(
          code
        )
      ) {
        (row = false), (msg = "身份证号格式错误");
      } else if (!city[code.substr(0, 2)]) {
        (row = false), (msg = "身份证号地址编码错误");
      } else {
        //18位身份证需要验证最后一位校验位
        if (code.length == 18) {
          code = code.split("");
          //∑(ai×Wi)(mod 11)
          //加权因子
          var factor = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2];
          //校验位
          var parity = [1, 0, "X", 9, 8, 7, 6, 5, 4, 3, 2];
          var sum = 0;
          var ai = 0;
          var wi = 0;
          for (var i = 0; i < 17; i++) {
            ai = code[i];
            wi = factor[i];
            sum += ai * wi;
          }
          if (parity[sum % 11] != code[17].toUpperCase()) {
            (row = false), (msg = "身份证号校验位错误");
          }
        }
      }
      return row;
    }),
    (Vue.prototype.$EmptyGuid = "00000000-0000-0000-0000-000000000000"),
    (Vue.prototype.$GetBirthdayByIDCardNum = function (id) {
      let birthday = "";
      if (id) {
        birthday = id.substr(6, 8).replace(/(.{4})(.{2})/, "$1-$2-");
      }
      return birthday;
    }),
    (Vue.prototype.$axios = axios),
    (Vue.prototype.guid = function () {
      var s = [];
      var hexDigits = "0123456789abcdef";
      for (var i = 0; i < 36; i++) {
        s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
      }
      s[14] = "4"; // bits 12-15 of the time_hi_and_version field to 0010
      s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1); // bits 6-7 of the clock_seq_hi_and_reserved to 01
      s[8] = s[13] = s[18] = s[23] = "-";

      var uuid = s.join("");
      return uuid;
    });
    Vue.prototype.SuitSize = function (mius) {
      let w = window.innerWidth;
      let h = window.innerHeight;
      let suitSize = {
        width: 0,
        height: 0,
        top: "15vh",
      };
      if (w <= 1399) {
        suitSize.width = `${(w * 0.9764).toFixed(2)}px`;
        suitSize.height = `${(h * 0.9764 - 300 - mius).toFixed(2)}px`;
        suitSize.top = "2vh";
      } else if (w > 1399 && w <= 1599) {
        suitSize.width = `${(w * 0.8571).toFixed(2)}px`;
        suitSize.height = `${(h * 0.8571 - 200 - mius).toFixed(2)}px`;
        suitSize.top = "5vh";
      } else {
        suitSize.width = `${(w * 0.75).toFixed(2)}px`;
        suitSize.height = `${(h * 0.75 - 200 - mius).toFixed(2)}px`;
        suitSize.top = "5vh";
      }
      return suitSize;
    };
  },
  getAgeByBirthDay(strAge) {
    if (strAge != null) {
      let birArr = strAge.split("-");
      let birYear = birArr[0];
      let birMonth = birArr[1];
      let birDay = birArr[2];

      d = new Date();
      let nowYear = d.getFullYear();
      let nowMonth = d.getMonth() + 1; //记得加1
      let nowDay = d.getDate();
      let returnAge;

      if (birArr == null) {
        return false;
      }
      let d = new Date(birYear, birMonth - 1, birDay);
      if (
        d.getFullYear() == birYear &&
        d.getMonth() + 1 == birMonth &&
        d.getDate() == birDay
      ) {
        if (nowYear == birYear) {
          returnAge = 0; //
        } else {
          let ageDiff = nowYear - birYear; //
          if (ageDiff > 0) {
            if (nowMonth == birMonth) {
              let dayDiff = nowDay - birDay; //
              if (dayDiff < 0) {
                returnAge = ageDiff - 1;
              } else {
                returnAge = ageDiff;
              }
            } else {
              let monthDiff = nowMonth - birMonth; //
              if (monthDiff < 0) {
                returnAge = ageDiff - 1;
              } else {
                returnAge = ageDiff;
              }
            }
          } else {
            return 0; //返回-1 表示出生日期输入错误 晚于今天
          }
        }
        return returnAge;
      } else {
        //Message.error(`日期格式输入错误,请重新输入!`);
        return 0;
      }
    } else return 0;
  },
};
