import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);

const state = {
  USERINFO: {},// 用户信息
  visitedViews:[],
  cachedViews: [],
  TabKeepList:[],
  collapse:false,
  ActiveIndex:null,
  openList:[]
}
const mutations = {

}
const actions = {
 
}

export default new Vuex.Store({
  state,
  mutations,
  actions
})
