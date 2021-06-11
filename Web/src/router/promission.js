import router from './index'
import Layout from './../components/common/Home'
import { Message } from 'element-ui';
import NProgress from 'nprogress';
import Store from './../store';
const _import = require('./_import_' + process.env.NODE_ENV);

var getRouter = [];

let BaseRouter = [{
  "title": "",
  "path": '/login',
  "name": 'Login',
  "componentpath": "pages/Login",
  "component": ""
}];

let RouteMode = false;

router.beforeEach((to, from, next) => {
  NProgress.start();
  if (RouteMode) {
    if (getRouter.length == 0) {
      LoadMenus([]);
      next({
        ...to,
        replace: true
      })
    } else
      next();
  } else {
    if (getRouter.length == 0) {
      LoadMenus([]);
      if ((from.path == '/' && to.path != '/login') || to.path == "/" || from.path == '/' && to.path == '/login') {
        next({
          ...{
            fullPath: "/login",
            hash: "",
            matched: [],
            meta: {},
            name: null,
            params: {},
            path: "/login",
            query: {}
          },
          replace: true
        });
      } else
        next();
    } else {
      if (localStorage.getItem("USERINFO")) {
        if (to.path == '/login') {
          ClearInfo();
        }
        next();
      } else {
        if (to.path !== '/login') {
          Message.error("请正确登录!");
          next('/login');
        } else {
          next();
        }
      }
    }
  }


})

router.afterEach((to, from, next) => {
	if (Store.state.TabKeepList.findIndex(x=>x.name == to.name) == -1 && to.name != "Login" && to.componentpath != "componentpath") {
    Store.state.TabKeepList.push(to)
	}
  NProgress.done();
})

function filterAsyncRouter(RouterMap) {
  let TmpRouterMap = _.cloneDeep(RouterMap);
  TmpRouterMap.forEach(route => {
    if (route.componentpath) {
      if (route.componentpath === 'Layout') {
        route.component = Layout
      } else {
        if (route.componentpath && route.componentpath != "")
          route.component = _import(route.componentpath)
      }
    }
    if (route.children && route.children.length) {
      route.children = filterAsyncRouter(route.children)
    }
  })
  return TmpRouterMap
}

function ClearInfo() {
  localStorage.removeItem("USERINFO");
  localStorage.removeItem("router");
  location.reload();
}

function LoadMenus(Menus,Logined) {
  if (Menus.length == 0 && localStorage.getItem("USERINFO"))
    getRouter = filterAsyncRouter(JSON.parse(localStorage.getItem('router')));
  else
    getRouter = filterAsyncRouter(_.cloneDeep([...Menus, ...BaseRouter]));
  localStorage.setItem('router', JSON.stringify(getRouter))
  if(Logined){
    getRouter = getRouter.filter(z=>z.name != "Login");
  }
  router.addRoutes(getRouter)
  global.antRouter = getRouter
}
export default {
  LoadMenus
};

