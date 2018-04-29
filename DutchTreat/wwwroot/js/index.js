$(document).ready(function() {
    var x = 0;
    var s = "";
    console.log("hellp plural sight");


    var theForm = $("#theForm");
    theForm.hide();

    var buyButton = $("#buyButton");
    buyButton.on("click",
        function() {
            console.log("buyin item");
        });


    var productInfo = $(".product-props li");
    productInfo.on("click",
        function() {
            console.log("you click on " + $(this).text());
        });

    var $logginToggle = $("#logginToggle");
    var $popupForm = $(".popup-form");

    $logginToggle.on("click",
        function() {
            $popupForm.toggle(1000);
        });
});