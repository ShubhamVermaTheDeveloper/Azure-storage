﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function ImagePreview(event) {
    var output = document.getElementById('outputImg');
    output.src = URL.createObjectURL(event.target.files[0]);
}

