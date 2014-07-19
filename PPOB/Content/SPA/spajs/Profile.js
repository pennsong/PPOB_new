DXSK8.Store.Profile = function (params) {
    var isPhone = DevExpress.devices.current().screenSize === "small";

    var validationMapping = {
        // customize the creation of the name property so that it provides validation
        Mobile: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        },
        Sex: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        },
        DocumentType: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        },
        DocumentNumber: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        },
        Birthday: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        }
        ,
        Marriage: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        }
        ,
        Nation: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        }
        ,
        Email: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true, email: true });
            }
        }
        ,
        Degree: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        }
        ,
        EmergencyContactPerson: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        }
        ,
        EmergencyContactPhone: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        }
        ,
        EverPension: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        }
        ,
        EverAccumulation: {
            create: function (options) {
                return ko.observable(options.data).extend({ required: true });
            }
        }
    };

    var defaultData = {
        Id: null,
        Name: null,
        ClientName: null,
        Mobile: null,
        CityName: null,
        EmployeeOBStatus: null,
        EnglishName: null,
        Sex: null,
        DocumentType: null,
        DocumentNumber: null,
        Birthday: null,
        Marriage: null,
        Nation: null,
        Yhy: null,
        Ysy: null,
        FixPhone: null,
        Email: null,
        Degree: null,
        HukouType: null,
        HujiAddress: null,
        HujiZipCode: null,
        Address: null,
        Phone: null,
        ZipCode: null,
        EmergencyContactPerson: null,
        EmergencyContactPhone: null,
        EverPension: null,
        EverAccumulation: null,
        EnterDate: null,
        //EmployeeOBStatusOption: null,
        SexOption: [{ "Id": 10, "Name": "男" }, { "Id": 20, "Name": "女" }],
        DocumentTypeOption: [{ "Id": 10, "Name": "身份证" }, { "Id": 20, "Name": "护照" }],
        MarriageOption: [{ "Id": 10, "Name": "已婚" }, { "Id": 20, "Name": "未婚" }, { "Id": 30, "Name": "离异" }],
        DegreeOption: [{ "Id": 10, "Name": "初中及以下" }, { "Id": 20, "Name": "高中" }, { "Id": 30, "Name": "大专" }, { "Id": 40, "Name": "本科" }, { "Id": 50, "Name": "硕士" }, { "Id": 60, "Name": "博士" }],
        HukouTypeOption: [{ "Id": 10, "Name": "本地城镇" }, { "Id": 20, "Name": "外地城镇" }, { "Id": 30, "Name": "本地农村" }, { "Id": 40, "Name": "外地农村" }, { "Id": 50, "Name": "外籍" }]
    };

    var viewModel = ko.mapping.fromJS(defaultData, validationMapping);

    function mapServerData(serverData) {
        ko.mapping.fromJS(serverData, viewModel);
    }

    function loadData() {
        $.getJSON("/api/EmployeeWeb/" + curEmployeeId, function (data) {
            mapServerData(data);
        });
    }

    viewModel.isPhone = isPhone;
    viewModel.showLookup = function (e) {
        if (viewModel.isPhone)
            return;
        $(".dx-viewport .dx-lookup-popup-wrapper:visible").addClass(e.element.closest(".billing").length ? "billing-popup" : "shipping-popup");
    };

    viewModel.viewShown = function () {
        loadData();
        if (isPhone)
            $(".dx-viewport .profile").dxScrollView();
        else
            $(".dx-viewport .profile-address-info").dxScrollView();
    };

    viewModel.errors = ko.validation.group(viewModel);

    viewModel.save = function () {

        if (viewModel.errors().length > 0) {
            $("#toastWarn").dxToast('instance').show();
            viewModel.errors.showAllMessages();
        }
        else {
            $.ajax("/api/EmployeeWeb/" + curEmployeeId, {
                data: ko.toJSON(viewModel),
                type: "post", contentType: "application/json",
                success: function (data, textStatus) {
                    $("#toastOk").dxToast('instance').show();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $("#toastErr").dxToast('instance').show();
                }
            });
        }
    }

    return viewModel;
};