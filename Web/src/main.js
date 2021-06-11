import Vue from 'vue'
import App from './App.vue'
import router from './router'
import './router/promission'
import store from './store'
import _ from "lodash";
import ElementUI from 'element-ui';
import "babel-polyfill";
import utils from './utils/utils';
import 'less';
import plTable from 'pl-table'

//CSS
import 'element-ui/lib/theme-chalk/index.css'; // 默认主题
import './assets/css/icon.css';
import './assets/css/WebSite.css';
import './assets/css/iconfont.css';
import 'nprogress/nprogress.css'

Vue.config.devtools = true
Vue.config.productionTip = false
Vue.use(ElementUI);
Vue.use(utils);
Vue.use(plTable);
new Vue({
	router,
	store,
	render: h => h(App)
}).$mount('#app')
