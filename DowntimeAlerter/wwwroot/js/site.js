// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var modal = document.getElementById('id01');

if (!!$.cookie('daTkn')) {
    $('#userName').css("display", "initial");
    $('#loginToggle').css("display", "none");
} else {
    $('#userName').css("display", "none");
    $('#loginToggle').css("display", "initial");
}

$("#loginToggle").click(function () {
    modal.style.display = "initial";
});

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}