$(document).ready(function () {
    $.get("/Books/GetConversionRate", null, function (conversionRate) {
        var dNISPrice = conversionRate * parseInt($('#priceUSD').text());
        $('#priceNIS').text(dNISPrice.toFixed(2) + '₪');
    });
});