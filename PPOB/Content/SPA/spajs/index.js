window.DXSK8 = window.DXSK8 || {};
window.DXSK8.Store = window.DXSK8.Store || {};

$(function() {
    var DX = DevExpress,
        currentDevice = DX.devices.current(),
        realDevice = DX.devices.real();
    currentDevice.platform = "generic";

    function setScreenSize() {
        var el = $("<div>").addClass("screen-size").appendTo(".dx-viewport");
        var size = getComputedStyle(el[0], ":after").content.replace(/"/g, "");
        el.remove();
        currentDevice.screenSize = size;
    }

    function exitApp() {
        if(confirm("Are you sure you want to exit?")) {
            switch(realDevice.platform) {
                case "tizen":
                    tizen.application.getCurrentApplication().exit();
                    break;
                case "android":
                    navigator.app.exitApp();
                    break;
                case "win8":
                    window.external.Notify("DevExpress.ExitApp");
                    break;
            }
        }
    }

    function onDeviceReady() {
        document.addEventListener("backbutton", onBackButton, false);
        DXSK8.app.navigatingBack.add(function() {
            if(!DXSK8.app.canBack()) {
                exitApp();
            }
        });
    }

    function onBackButton() {
        DevExpress.hardwareBackButton.fire();
    }

    DXSK8.app = new DevExpress.framework.html.HtmlApplication({
        namespace: DXSK8.Store,
        navigationType: DXSK8.config.navigationType,
        navigation: DXSK8.config.navigation,
        navigateToRootViewMode: "keepHistory"
    });
    DXSK8.app.router.register(":view/:type/:id", { view: "Summary", type: undefined, id: undefined });

    setScreenSize();
    $(window).bind("load resize", setScreenSize);

    DXSK8.app.navigate();

    document.addEventListener("deviceready", onDeviceReady, false);
    if(realDevice.platform == "tizen") {
        document.addEventListener("tizenhwkey", function(e) {
            if(e.keyName === "back")
                onBackButton();
        });
    }
    
    (function() {
		// Validation plugin support code
		var temp = window.document.createElement("div");

		function getObservableForProp(element, widgetName, propName) {
			temp.setAttribute("data-bind", element.getAttribute('data-bind'));
			var o = ko.bindingProvider.instance.getBindings(temp, ko.contextFor(element));
			return o[widgetName][propName];
		}

		var makedxBindingHandlerValidatable = function(handlerName) {

			var init = ko.bindingHandlers[handlerName].init;

			ko.bindingHandlers[handlerName].init = function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {

				init(element, valueAccessor, allBindingsAccessor);

				valueAccessor = function () { return getObservableForProp(element, handlerName, "value"); };
				return ko.bindingHandlers.validationCore.init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
			};
		};
		// Enable validation for any particular bindings if needed
		// NOTE: bindings MUST have valid "value" property
		makedxBindingHandlerValidatable("dxTextBox");
		makedxBindingHandlerValidatable("dxNumberBox");
		makedxBindingHandlerValidatable("dxSelectBox");
		makedxBindingHandlerValidatable("dxDateBox");
	})();
});
