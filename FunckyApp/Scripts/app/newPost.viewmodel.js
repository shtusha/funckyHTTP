function NewPostViewModel(app, dataModel) {
    var self = this;

    self.init = function(options) {
        self.callback = options.callback;
        self.addPostUrl = options.addUrl;
        self.previewPostUrl = options.previewUrl;
        self.title(options.title.inflated);
    };


    // Data
    self.message = ko.observable();
    self.title = ko.observable();

    // UI State
    self.loading = ko.observable(false);
    self.preview = ko.observable();
    self.errors = ko.observableArray();

    //UI Functions
    self.cancel = function() {
        self.callback();
    };


    self.previewPost = function() {
        if (!self.loading()) {
            self.loading(true);

            dataModel.postData(self.previewPostUrl, {
                message: self.message(),
                inflationRate: 1
            })
                .done(function (data) {
                    self.loading(false);
                self.preview(data.inflated);

            }).failJSON(function (data) {
                    app.errors.push("Error generating preview.");
                });
        }

    };

    self.addPost = function() {
        if (!self.loading()) {
            self.loading(true);

            dataModel.postData(self.addPostUrl, {
                message: self.message(),
                inflationRate: 1
                })
                .done(function(data) {
                    self.loading(false);
                    self.callback();

                }).failJSON(function(data) {
                    app.errors.push("Error creating post.");
                });
        }
    };
}
app.addViewModel({
    name: "NewPost",
    bindingMemberName: "newPost",
    factory: NewPostViewModel
});

app.navigateToNewPost = function(options) {
    app.errors.removeAll();
    app.view(app.Views.NewPost);
    app.newPost().init(options);
};