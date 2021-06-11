 let baseURL = 'http://localhost:8881'

const path = require('path');
function resolve(dir) {
	return path.join(__dirname, dir);
}

module.exports = {
    publicPath: './',
	productionSourceMap: false,
    chainWebpack: config => {
			config.resolve.alias
				.set('@', resolve('src'))
				.set('common', resolve('src/components/common'))
				.set('pages', resolve('src/components/pages'))
	},
	devServer: {
        port:9001,
        open: true,
        https: false,
        hotOnly: false, 
        proxy: {
            //配置跨域
            '/api': {
                target: `${baseURL}/api`,
                ws:true,
                changOrigin:true,
                pathRewrite:{
                    '^/api':''
                }
            }
        }
    }
}
