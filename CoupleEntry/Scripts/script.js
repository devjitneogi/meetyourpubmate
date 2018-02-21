var usersOnline;
var map;
var lastMarker;
var myLat, myLong;
function initMap() {

}

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(SetMyPosition, showError);
    } else {
        x.innerHTML = "Geolocation is not supported by this browser.";
    }
}

function ShowOnMap(lat, lon, label, address) {
    var latLng = new google.maps.LatLng(lat, lon);
    var marker = new google.maps.Marker({
        position: latLng,
        map: map,
        label: label
    });

    map.setCenter(latLng)
    var infowindow = new google.maps.InfoWindow({
        content: address
    });
    marker.addListener('click', function () {
        infowindow.open(map, marker);
    });
    if (label != "You")
        lastMarker = marker;
}

function SetMyPosition(position) {
    myLat = position.coords.latitude;
    myLong = position.coords.longitude;
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 14,
        center: { lat: myLat, lng: myLong }
    });
    ShowOnMap(myLat, myLong, "You");
}

function FindNearbyPeople(users) {


    var destinations = [], origins = [];
    var origin = new google.maps.LatLng(myLat, myLong);
    usersOnline = users;
    origins[0] = origin;
    for (var i = 0; i < users.length; i++) {
        destinations[i] = new google.maps.LatLng(users[i].latitude, users[i].longitude);
    }

    if (origins.length > 0 && destinations.length > 0) {
        var service = new google.maps.DistanceMatrixService();
        service.getDistanceMatrix(
          {
              origins: origins,
              destinations: destinations,
              travelMode: 'DRIVING',
              unitSystem: google.maps.UnitSystem.METRIC,
          }, callback);
    }
    else {
        $('#mask').hide();
        alert('Sorry, No one is nearby!');
    }
}

function callback(response, status) {
    $("#yourAddress").html("Your Address:" + response.originAddresses[0]);
    if (response.rows.length > 0) {
        var meInList = false;
        $("#nearbyPeoplesList").html("");
        var distances = response.rows[0].elements;
        var uname = $("#uname")[0].value;
        var distance = $("#distanceFilter")[0].value;
        var gender = $("#genderFilter")[0].value;
        for (var i = 0; i < distances.length; i++) {
            if (usersOnline[i].uname == uname) {
                meInList = true;
                continue;
            }
            if (distances[i].distance && distances[i].distance.value / 1000 <= distance && (gender == "Both" || gender == usersOnline[i].gender)) {
                var btstrpCls = GetBootstrapClass(usersOnline[i].gender);
                var div = $('<div class="panel panel-' + btstrpCls + ' cursorPointer tile" lat="' + usersOnline[i].latitude + '" lon="' + usersOnline[i].longitude + '" label="' + usersOnline[i].uname + '"address="' + response.destinationAddresses[i] + '""></div>');
                div.html('<div class="panel-heading">' + usersOnline[i].uname + '<span class="pull-right glyphicon ' + (usersOnline[i].gender == "Male" ? "glyphicon-star" : "glyphicon-heart") + '"></span></div><div class="panel-body">Age:' + usersOnline[i].age + '<br>Distance:' + distances[i].distance.text + '<br> Time to reach:' + distances[i].duration.text + '</div>');
                $("#nearbyPeoplesList").append(div);
            }
        }
        if (distances.length == 1 && meInList) {
            alert('Sorry, No one is nearby!');
        }
    }
    $('#mask').hide();
}

function GetBootstrapClass(gender) {
    if (gender == "Male")
        return "info ";
    else
        return "danger";
}


function showError(error) {
    var x = document.getElementById("error-div");
    switch (error.code) {
        case error.PERMISSION_DENIED:
            x.innerHTML = "User denied the request for Geolocation."
            break;
        case error.POSITION_UNAVAILABLE:
            x.innerHTML = "Location information is unavailable."
            break;
        case error.TIMEOUT:
            x.innerHTML = "The request to get user location timed out."
            break;
        case error.UNKNOWN_ERROR:
            x.innerHTML = "An unknown error occurred."
            break;
    }
}

$(function () {

  
    $("#findPeopleBtn,#findPeopleBtnFilter").on("click", function () {
        if ($("#uname")[0].value != "" && $("#age")[0].value != "") {
            mask.show();
            GetOtherUsers(myLat, myLong);
        }
        else
            alert("Please fill all the fields!");
    });

    $("#uname,#age").on("keyup", function (e) {
        var keypressed = e.which || e.keyCode;
        if (keypressed === 13) {
            $("#findPeopleBtn").click();
        }
    });
    $('#nearbyPeoplesList').on('click', '.tile', function () {
        var user = $(this);
        if (lastMarker)
            lastMarker.setMap(null);
        ShowOnMap(user.attr("lat"), user.attr("lon"), user.attr("label"), user.attr("address"));
    });
    $('#signOut').on('click', function signOut() {
        //debugger;
        var auth2 = gapi.auth2.getAuthInstance();
        auth2.signOut().then(function () {
            console.log('User signed out.');
        });
    });

});