$('#aboutPageTitle').ready(function () {
    var canvas = $('#aboutPageTitle')[0];
    var ctx = canvas.getContext("2d");
    ctx.font = "35px Comic Sans MS";
    ctx.fillStyle = "black";
    ctx.textAlign = "center";
    ctx.fillText("About Us", canvas.width / 2, canvas.height / 2);
});

function initMap() {
    var centerCord = { lat: 31.969738, lng: 34.7727872 };

    var map = new google.maps.Map($('#map')[0], {
        zoom: 15,
        center: centerCord
    });

    var marker = new google.maps.Marker({
        position: centerCord,
        map: map
    });
}