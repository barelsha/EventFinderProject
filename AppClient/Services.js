app.factory('Event', ['$http', function ($http) {
    function Event(event) {
        if (event)
            this.setData(event);
    }
    Event.prototype = {
        setData: function (eventData) {
            angular.extend(this, eventData);
        }
    };
    return Event;
}]);
//-------------------------------------------------------------------------------------------------------------------
app.factory('UserService', ['$http', function ($http) {
    var service = {};
    service.isLoggedIn = false;
    service.success = false;
    service.login = function (user) {
        var requestUrl = "http://eventfinder.cloudapp.net/Service1.svc/Login";
        return $http.post(requestUrl, user)
            .then(function (response) {
                var userID = response.data;
                $http.defaults.headers.common = {
                    'userID': userID,
                    'user': user.Email
                };
                service.isLoggedIn = true;

                return Promise.resolve(response);
            })
            .catch(function (e) {
                return Promise.reject(e);
            });
    };
    service.Register = function (user) {
        var requestUrl = "http://eventfinder.cloudapp.net/Service1.svc/Register";
        return $http.post(requestUrl, user)
            .then(function (response) {
                var userID = response.data;
                alert(userID);
                $http.defaults.headers.common = {
                    'userID': userID,
                    'user': user.Email
                };
                return Promise.resolve(response);
            })
            .catch(function (e) {
                return Promise.reject(e);
            });
    };
    return service;
}]);
//-------------------------------------------------------------------------------------------------------------------
app.service("EventService", function ($http) {

    this.GetEvents = function () {
        return $http.get("http://eventfinder.cloudapp.net/Service1.svc/Events");
    };
    this.GetEvent = function (eventID) {
        return $http.get("hhttp://eventfinder.cloudapp.net/Service1.svc/Events/" + eventID);
    };
    this.JoinEvent = function (eventID, userID) {
        return $http.get("http://eventfinder.cloudapp.net/Service1.svc/Events/" + eventID + "/" + userID);
    };
    this.AddEvent = function (EventDetails) {
        var request = $http({
            method: "post",
            url: "http://eventfinder.cloudapp.net/Service1.svc/Events",
            data: EventDetails
        });
        return request;
    }
}); 