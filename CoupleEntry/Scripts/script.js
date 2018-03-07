
var usersOnline;
var map;
var lastMarker;
var myLat, myLong;
function initMap() {

}

function GetMyLocation() {
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
        jQuery('.gm-style-iw').prev('div').remove();
        infowindow.open(map, marker);
    });
    if (label != "You")
        lastMarker = marker;

    UpsertUserPosition();
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
    //debugger;

    var destinations = [], origins = [];
    var origin = new google.maps.LatLng(myLat, myLong);
    users.sort(CompareLastSeen);
    usersOnline = users;
    origins[0] = origin;
    for (var i = 0; i < users.length; i++) {
        destinations[i] = new google.maps.LatLng(users[i].Latitude, users[i].Longitude);
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
function CompareLastSeen(a, b) {
    if (parseInt(a.LastSeenDiff) < parseInt(b.LastSeenDiff))
        return -1;
    if (parseInt(a.LastSeenDiff) > parseInt(b.LastSeenDiff))
        return 1;
    return 0;
}

function callback(response, status) {
    // debugger;
    $("#yourAddress").html("Your Address:" + response.originAddresses[0]);
    if (response.rows.length > 0) {
        var meInList = false;
        $("#nearbyPeopleList").html("");
        var distances = response.rows[0].elements;
        var distance = $("#distanceFilter")[0].value;
        var gender = $("#genderFilter")[0].value;

        for (var i = 0; i < distances.length; i++) {
            if (distances[i].distance && distances[i].distance.value / 1000 <= distance && (gender == "Both" || gender == usersOnline[i].Gender)) {
                var lastSeen = CalculateLastSeen(parseInt(usersOnline[i].LastSeenDiff));
                var btstrpCls = GetBootstrapClass(usersOnline[i].Gender);
                var likeClass;
                if (myLikes.indexOf(usersOnline[i].UserId.toString()) > -1)
                { likeClass = "glyphicon-heart" }
                else
                { likeClass = "glyphicon-heart-empty" }

                var div = $('<div class="panel panel-' + btstrpCls + ' cursorPointer tile" userId="' + usersOnline[i].UserId + '" lat="' + usersOnline[i].Latitude + '" lon="' + usersOnline[i].Longitude + '" label="' + usersOnline[i].Name + '"address="' + response.destinationAddresses[i] + '""></div>');
                div.html('<div class="panel-heading">' + usersOnline[i].Name + '<span class="pull-right glyphicon ' + likeClass + ' heartIcon"></span></div><div class="panel-body"><span class="pull-right iconImageUrl" style="background:url(' + usersOnline[i].ImageUrl + ')"></span>Age:' + usersOnline[i].Age + '<br>Distance:' + distances[i].distance.text + '<br> Time to reach:' + distances[i].duration.text + '<br> Last Seen:' + lastSeen + '</div>');
                $("#nearbyPeopleList").append(div);
            }
        }

    }
    $('#mask').hide();
}

function CalculateLastSeen(difference) {

    var lastSeen = difference + " seconds ago.";
    if (difference > 59) {
        difference = Math.floor(difference / 60);
        lastSeen = difference + " minutes ago.";

        if (difference > 59) {
            difference = Math.floor(difference / 60);
            lastSeen = difference + " hours ago.";

            if (difference > 23) {
                difference = Math.floor(difference / 24);
                lastSeen = difference + " days ago.";

                if (difference > 29) {
                    difference = Math.floor(difference / 30);
                    lastSeen = difference + " months ago.";
                }
            }
        }
    }
    return lastSeen;
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
    // debugger;
    gapi.load('auth2', function () {
        //  debugger;
        gapi.auth2.init();
    });
    var mask = $('#mask');
    $("#findPeopleBtn").on("click", function () {
        mask.show();
        GetOtherUsers(myLat, myLong);
    });

    $('#nearbyPeopleList').on('click', '.tile', function () {
        var user = $(this);
        if (lastMarker)
            lastMarker.setMap(null);
        ShowOnMap(user.attr("lat"), user.attr("lon"), user.attr("label"), user.attr("address"));
    });
    $('#signOut').on('click', function() {
        //debugger;
        var auth2 = gapi.auth2.getAuthInstance();
        auth2.signOut().then(function () {
            console.log('User signed out.');
        });
    });

    $('#nearbyPeopleList').on('click', ".heartIcon", function (event) {
        //debugger;
        var targetId = $(event.target).parent().parent().attr("userid");
        var element = $(event.target);
        var liked=element.hasClass("glyphicon-heart-empty");
        AddOrRemoveNewLike(targetId, liked, element);
    });

});