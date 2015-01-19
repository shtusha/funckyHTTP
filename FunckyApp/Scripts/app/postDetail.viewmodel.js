function PostDetailsViewModel(app, dataModel) {
    var self = this;

    // Data
    self.message = ko.observable();
    self.replies = ko.observableArray();

    // UI State
    self.loading = ko.observable(false);
    self.message = ko.observable();
    self.errors = ko.observableArray();

    self.canReply = ko.computed(function () {
        return app.user() !== null;
    });

    self.addReplyCallback = function() {
        app.navigateToPostDetails(self.detailsUrl);
    };

    self.reply = function () {
        app.navigateToNewPost({
            addUrl: self.replyUrl,
            previewUrl: self.previewUrl,
            callback: self.addReplyCallback,
            title: "Reply to post"
        });
    };


    self.load = function(url) { // Load posts
        if (!self.loading()) {
            self.loading(true);

            dataModel.getData(url)
                .done(function (data) {

                    //hack. urls should be found based on rel
                    self.detailsUrl = data.links[0].href;
                    self.replyUrl = data.links[1].href;
                    self.previewUrl = data.links[2].href;


                    self.message(new MessageViewModel(data));

                    for (var i = 0; i < data.replies.length; i++) {
                        self.replies.push(new MessageViewModel(data.replies[i]));
                    }

                    self.loading(false);

                }).failJSON(function (data) {
                    var errors;

                    self.loading(false);
                    errors = dataModel.toErrorsArray(data);

                    if (errors) {
                        app.errors(errors);
                    } else {
                        app.errors.push("Error retrieving post.");
                    }
                });
        }
    };
}

function MessageViewModel(data) {
    var self = this;

    self.author = ko.observable(data.author);
    self.createdOn = ko.observable(data.createdOn);
    self.fragments = ko.observableArray();

    self.isPlainText = ko.observable(false);
    self.switchToPlainText = function() {
        self.isPlainText(true);
    };
    self.switchToInflatedText = function () {
        self.isPlainText(false);
    };

    for (var i = 0; i < data.fragments.length; i++) {
        self.fragments.push(new FragmentViewModel(data.fragments[i], self));
    }
}



function FragmentViewModel(data, message) {
    var self = this;

    self.originalText = ko.observable(data.originalText);
    self.inflatedText = ko.observable(data.inflatedText);
    self.isInflated = ko.observable(data.isInflated);

    self.inflatedTextDisplay = ko.computed(function() {
        if (message.isPlainText()) {
            return self.originalText();
        }
        return self.inflatedText();
    });
}

app.addViewModel({
    name: "PostDetails",
    bindingMemberName: "postDetails",
    factory: PostDetailsViewModel
});

app.navigateToPostDetails = function(url) {
        app.errors.removeAll();
        app.view(app.Views.PostDetails);
        app.postDetails().load(url);
    };