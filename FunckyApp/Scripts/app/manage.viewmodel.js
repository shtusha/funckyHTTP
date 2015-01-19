function ManageViewModel(app, dataModel) {
    var self = this,
        startedLoad = false;


    // Data
    self.userName = ko.observable();
    self.localLoginProvider = ko.observable();

    // UI state
    self.loading = ko.observable(true);
    self.message = ko.observable();
    self.errors = ko.observableArray();

    self.changePassword = ko.computed(function() {
        return new ChangePasswordViewModel(app, self, self.userName(), dataModel);
    });


    // Operations
    self.load = function() { // Load user management data
        if (!startedLoad) {
            startedLoad = true;

            dataModel.getManageInfo(dataModel.returnUrl, true /* generateState */)
                .done(function(data) {
                    if (typeof (data.localLoginProvider) !== "undefined" &&
                        typeof (data.userName) !== "undefined") {
                        self.userName(data.userName);
                        self.localLoginProvider(data.localLoginProvider);

                    } else {
                        app.errors.push("Error retrieving user information.");
                    }

                    self.loading(false);
                }).failJSON(function(data) {
                    var errors;

                    self.loading(false);
                    errors = dataModel.toErrorsArray(data);

                    if (errors) {
                        app.errors(errors);
                    } else {
                        app.errors.push("Error retrieving user information.");
                    }
                });
        }
    }

    self.home = function() {
        app.navigateToHome();
    };

}

function ChangePasswordViewModel(app, parent, name, dataModel) {
    var self = this;

    // Private operations
    function reset() {
        self.errors.removeAll();
        self.oldPassword(null);
        self.newPassword(null);
        self.confirmPassword(null);
        self.changing(false);
        self.validationErrors.showAllMessages(false);
    }

    // Data
    self.name = ko.observable(name);
    self.oldPassword = ko.observable("").extend({ required: true });
    self.newPassword = ko.observable("").extend({ required: true });
    self.confirmPassword = ko.observable("").extend({ required: true, equal: self.newPassword });

    // Other UI state
    self.changing = ko.observable(false);
    self.errors = ko.observableArray();
    self.validationErrors = ko.validation.group([self.oldPassword, self.newPassword, self.confirmPassword]);

    // Operations
    self.change = function () {
        self.errors.removeAll();
        if (self.validationErrors().length > 0) {
            self.validationErrors.showAllMessages();
            return;
        }
        self.changing(true);

        dataModel.changePassword({
            oldPassword: self.oldPassword(),
            newPassword: self.newPassword(),
            confirmPassword: self.confirmPassword()
        }).done(function (data) {
            self.changing(false);
            reset();
            parent.message("Your password has been changed.");
        }).failJSON(function (data) {
            var errors;

            self.changing(false);
            errors = dataModel.toErrorsArray(data);

            if (errors) {
                self.errors(errors);
            } else {
                self.errors.push("An unknown error occurred.");
            }
        });
    };
}

app.addViewModel({
    name: "Manage",
    bindingMemberName: "manage",
    factory: ManageViewModel,
    navigatorFactory: function (app) {
        return function () {
            app.errors.removeAll();
            app.view(app.Views.Manage);
                app.manage().load();
            };
        }
    }
);