function NewEducation(school, major, degree, start, end) {
    var self = this;
    self.school = ko.observable(school).extend({ required: true });
    self.major = ko.observable(major).extend({ required: true });
    self.degree = ko.observable(degree).extend({ required: true });
    self.start = ko.observable(start).extend({ required: true });
    self.end = ko.observable(end);

    self.errors = ko.validation.group(self);
}

DXSK8.Store.Education = function () {
    var isPhone = DevExpress.devices.current().screenSize === "small";

    var educations = ko.observableArray();

    function loadData() {
        $.getJSON("/api/EmployeeEducationWeb/" + curEmployeeId, function (data) {
            viewModel.educations().length = 0;
            $.each(data.EmployeeEducations, function (index, item) {
                viewModel.educations.push(new NewEducation(item.School, item.Major, item.Degree, item.Start, item.End));
            });
        });
    }

    var viewModel = {

        isPhone: isPhone,

        degrees: [{ "Id": 10, "Name": "初中及以下" }, { "Id": 20, "Name": "高中" }, { "Id": 30, "Name": "大专" }, { "Id": 40, "Name": "本科" }, { "Id": 50, "Name": "硕士" }, { "Id": 60, "Name": "博士" }],

        showLookup: function (e) {
            if (isPhone)
                return;
            $(".dx-viewport .dx-lookup-popup-wrapper:visible").addClass(e.element.closest(".billing").length ? "billing-popup" : "shipping-popup");
        },

        viewShown: function () {
            loadData();
            if (isPhone)
                $(".dx-viewport .profile").dxScrollView();
            else
                $(".dx-viewport .profile-address-info").dxScrollView();
        },

        educations: educations,

        add: function () {
            educations.push(new NewEducation("", "", "", "", ""));
        },

        remove: function (education) { educations.remove(education) },

        save: function () {
            var errorExists = false;

            for (var i = 0; i < educations().length; i++) {
                if (educations()[i].errors().length > 0) {
                    educations()[i].errors.showAllMessages();
                    errorExists = true;
                }
            }

            if (errorExists) {
                $("#toastWarn").dxToast('instance').show();
            }
            else {
                $.ajax("/api/EmployeeEducationWeb/" + curEmployeeId, {
                    data: ko.toJSON(educations),
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
    };

    return viewModel;
};


