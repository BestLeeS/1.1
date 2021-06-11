var api;
var AppCore = {
	createNew: function () {
		var api = {};
		//刷身份证
		api.getIdcard = function (callback) {
			return bound.getIdcard(callback);
		}
		//拍照
		api.shot = function (callback) {
			return bound.shot(callback);
		}

		//打印
		api.print = function (cardNum) {
			return bound.print(cardNum);
		}
		//打印条码
		api.PrintBarCode = function (barcode, num) {
			return bound.printBarCode(barcode, num);
		}

		//打印姓名条码
		api.PrintNameCode = function (barcode) {
			return bound.printNameCode(barcode);
		}

		//打印发票
		api.PrintBill = function (data) {
			return bound.printBill(data);
		}

		//保存文件
		api.save = function (path) {
			return bound.save(path);
		}

		//获取打印机设置
		api.getPrinterSetting = function () {
			var list = bound.getPrinterSetting();
			return list;
		}
		//保存打印机设置
		api.savePrinterSetting = function (docPrinter, barcodePrinter, billPrinter) {
			bound.savePrinterSetting(docPrinter, barcodePrinter, billPrinter);
			return 0;
		}
		return api;
	}
};
if (!api) {
	api = AppCore.createNew();
}
window.api = api;
