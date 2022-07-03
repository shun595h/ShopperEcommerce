// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var images = ['/images/header.jpg', '/images/secondads.jpg','/images/thirdads.jpg'];
var current = 0;

function nextImage() {
    current++;
    if (current === images.length) {
        current = 0;
    }
    $('.header-container').css('background-image', 'url(' + images[current] + ')');
}

function prevImage() {
    current--;
    if (current < 0) {
        current = images.length - 1;
    }
    $('.header-container').css('background-image', 'url(' + images[current] + ')');
}