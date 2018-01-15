var app = angular.module('myApp', ['ngRoute'/*,'LocalStorageModule'*/]);

//app.config(function (localStorageServiceProvider) {
//    localStorageServiceProvider.setPrefix('angular_App');
//});

//-------------------------------------------------------------------------------------------------------------------
app.controller('mainController', ['UserService','$http','$location'/*,'localStorageService'*/,function (UserService,$http,$location/*,localStorageService*/) {
    var self = this;
    self.text2="";
    self.header2 = "";
    self.userService = UserService;
    self.logOut = function () {
        self.userService.isLoggedIn = false;
        //self.deleteCookie('Password');
        //self.deleteCookie('Email');
        //self.deleteCookie('last entry date');
        $location.path('/');
    };

    //self.deleteCookie = function (parameter) {
    //    var cookieVal = localStorageService.cookie.get(parameter);
    //    if (cookieVal) {
    //        localStorageService.cookie.remove(parameter);
    //    }
    //};

    if( self.userService.isLoggedIn){
        //self.Email = $http.defaults.headers.common.user;
        //if( localStorageService.cookie.get('last entry date')!= null){
        //    self.lastDate =  "last entry date: "+ localStorageService.cookie.get('last entry date');
        //}
    }
        else
    {
        self.Email = 'Guest';
    }

}]);
//-------------------------------------------------------------------------------------------------------------------
app.controller('eventsController', ['$http', 'Event'/*, 'localStorageService'*/,'EventService', function ($http, Event/*, localStorageService*/, EventService) {
    var self = this;
    var self = this;
    self.sortBy = "Name";
    self.getEvents = function () {
        EventService.GetEvents().then(function (res) {
            response = res.data;
            if (response.success) {
                self.Events = [];
                angular.forEach(response.data, function (obj) {
                    self.Events.push(new Event(obj));
                });
            }
            else {
                alert(response.message);
            }
        }).catch(function (err) {
            alert(err);
        });
    };
    self.changeSortBy = function (property) {
        self.sortBy = property;
    };
    self.JoinEvent = function (event) {
        //var  userID = $http.defaults.headers.common.userID;
        EventService.JoinEvent(event.ID, userID).then(function (res) {
            response = res.data;
            if (response.success) {
                alert("join user to event success");
            }
            else {
                alert(response.message);
            }
        }).catch(function (err) {
            alert(err);
        });
    };
}]);
//-------------------------------------------------------------------------------------------------------------------
app.controller('loginController', ['UserService'/*,'localStorageService'*/, '$location', '$window','$http',
    function(UserService/*,localStorageService*/, $location, $window, $http) {
        var self = this;
        self.user = { Email: '', Password: ''};
        self.login = function(valid) {
            if (valid) {

                UserService.login(self.user).then(function (success) {
                    alert("fdd");
                    //if ((localStorageService.cookie.get('Email') != null && localStorageService.cookie.get('Password') != null) || (localStorageService.cookie.get('Email')!=self.user.username &&localStorageService.cookie.get('Password')!=self.user.password)){
                    //    self.deleteCookie('Email');
                    //    self.deleteCookie('Password');
                    //    self.deleteCookie('last entry date');
                    //    self.cookieKey ='Esername';
                    //    self.cookieValue = self.user.Email;
                    //    self.addCookie();
                    //    self.cookieKey ='Password';
                    //    self.cookieValue = self.user.Password;
                    //    self.addCookie();
                    //    self.cookieKey ='last entry date';
                    //    var d = new Date();
                    //    var today = d.toDateString();
                    //    self.cookieValue = today;
                    //    self.addCookie();
                    //}
                    $location.path('/');

                }, function (error) {
                    alert("dddd");
                    self.errorMessage = error.data;
                    $window.alert('Email not found in the system');
                });
            }
        };
        //self.deleteCookie = function (parameter) {
        //    var cookieVal = localStorageService.cookie.get(parameter);
        //    if (cookieVal) {
        //        localStorageService.cookie.remove(parameter);
        //    }
        //};

        //self.addCookie = function () {
        //    var cookieVal = localStorageService.cookie.get(self.cookieKey);
        //    if (!cookieVal)
        //        localStorageService.cookie.set(self.cookieKey,self.cookieValue, 3);
        //};
    }]);
//-------------------------------------------------------------------------------------------------------------------
app.controller('homeController', ['UserService', '$http'/*,'localStorageService'*/,'$location', function(UserService, $http/*, localStorageService*/, $location) {
    var self = this;
    
    self.login = function() {
        //self.user = {
        //    Email: localStorageService.cookie.get('Email'),
        //    Password: localStorageService.cookie.get('Password')
        //};
        //UserService.login(self.user).then(function () {
        //    $location.path('/');
        //}, function (error) {
        //    alert("ggg");
        //    self.errorMessage = error.data;
        //    $window.alert('Email not found in the system');
        //});
    };
}]);
//-------------------------------------------------------------------------------------------------------------------
app.controller('registerController',['UserService', '$location', '$window'/*,'localStorageService'*/,'$scope',
    function(UserService, $location, $window/*,localStorageService*/, $scope) {

        var self = this;
        self.user = {
            FirstName: '',
            LastName: '',
            PhoneNumber: '',
            Email: '',
            Password: ''
        };
        self.register = function(valid) {
            if (valid) {
                UserService.Register(self.user).then(function (success) {
                    alert("ggf");
                    if(success!=false){
                        $location.path('/login');
                        //self.cookieKey ='Email';
                        //self.cookieValue = self.user.Email;
                        //self.addCookie();
                        //self.cookieKey ='Password';
                        //self.cookieValue = self.user.Password;
                        //self.addCookie();
                        //self.cookieKey ='last entry date';
                        //var d = new Date();
                        //var today = d.toDateString();
                        //self.cookieValue = today;
                        //self.addCookie();
                    }
                    else {
                        $window.alert('Email already in system');
                    }
                }, function (error) {
                    debugger;
                    alert("ppp");
                    self.errorMessage = error.data;
                    $window.alert('Email already in system');
                })
            }
        };
        //self.addCookie = function () {
        //    var cookieVal = localStorageService.cookie.get(self.cookieKey);
        //    if (!cookieVal)
        //        localStorageService.cookie.set(self.cookieKey,self.cookieValue, 3);
        //};
    }]);
//-------------------------------------------------------------------------------------------------------------------
app.config(['$locationProvider', function($locationProvider) {
    $locationProvider.hashPrefix('');
}]);
app.config( ['$routeProvider', function($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl : "./Components/Home/Home.html",
            controller : "mainController"
        })
        .when("/login", {
            templateUrl : "./Components/Login/Login.html",
            controller : "loginController"
        })
        .when("/events", {
            templateUrl : "./Components/Events/Events.html",
            controller : "eventsController"
        })
        .when("/register", {
            templateUrl : "./Components/Register/Register.html",
            controller : "registerController"
        })
        .otherwise({redirect: '/'
        });
}]);
//-------------------------------------------------------------------------------------------------------------------