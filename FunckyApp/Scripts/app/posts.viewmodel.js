function PostsViewModel(app, dataModel) {
    var self = this


    // Data
    self.postItems = ko.observableArray();

    // UI State
    self.loading = ko.observable(false);
    self.message = ko.observable();
    self.errors = ko.observableArray();

    self.canAddPost = ko.computed(function() {
        return app.user() !== null;
    });

    self.addPost = function () {
        app.navigateToNewPost({
            addUrl: self.addPostUrl,
            previewUrl: self.previewPostUrl,
            callback: app.navigateToPosts,
            title: "Add New Post"
        });
    };

    self.load = function() { // Load posts
        if (!self.loading()) {
            self.loading(true);

            dataModel.getPosts()
                .done(function(data) {
                    for (var i = 0; i < data.posts.length; i++) {
                        self.postItems.push(new PostHeaderViewModel(app, data.posts[i], dataModel));
                    }

                    self.addPostUrl = data.links[0].href;
                    self.previewPostUrl = data.links[1].href;

                    self.loading(false);

                }).failJSON(function (data) {
                    var errors;

                    self.loading(false);
                    errors = dataModel.toErrorsArray(data);

                    if (errors) {
                        app.errors(errors);
                    } else {
                        app.errors.push("Error retrieving posts.");
                    }
                });
        }
    };

}

function PostHeaderViewModel(app, data, dataModel) {
    var self = this;

    self.author = ko.observable(data.author);
    self.createdOn = ko.observable(data.createdOn);
    self.title = ko.observable(data.title);

    self.detailsLocation = data.links[0].href;

    self.viewDetails = function () {
        app.navigateToPostDetails(self.detailsLocation);
    };
}

app.addViewModel({
    name: "Posts",
    bindingMemberName: "posts",
    factory: PostsViewModel,
    navigatorFactory: function (app) {
        return function () {
            app.errors.removeAll();
            app.view(app.Views.Posts);
            app.posts().load();
        };
    }
});
