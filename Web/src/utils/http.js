import axios from 'axios';
import router from '../router';
import store from '../store'
import {
	Message
} from 'element-ui';

let baseURL = ''

if (process.env.NODE_ENV === 'production') {
	baseURL = window.g.ProDuctURL;
}
axios.defaults.baseURL = baseURL;
axios.defaults.headers.post['Content-Type'] = 'application/json';

// 请求拦截  设置统一header
axios.interceptors.request.use(config => {

	if (config.method == "get") {
		if (!config.params)
			config.params = {};
		config.params.UserID = window.UserID;
	}


	if (config.method == "post") {
		if (!config.data)
			config.data = {};
		config.data.UserID = window.UserID;
	}


	if (router.history.current.path !== '/login') {
		if (window.Token) {
			config.headers.Authorization = "Bearer " + window.Token;
			return config
		} else {
			Message.error('登录失效,请重新登录!');
			//router.push('/login');
		}
	} else {
		return config;
	}
}, err => {
	return Promise.reject(err)
})
//
axios.interceptors.response.use(res => {
	return res
}, err => {
	let timer = window.setTimeout(() => {
		if (err)
			Message.error({
				dangerouslyUseHTMLString: true,
				message: err.toString()
			});
	}, 0)
	return Promise.reject(err)
})

export default axios