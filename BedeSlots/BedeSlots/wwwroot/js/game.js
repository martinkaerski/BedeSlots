var Directry = "images/fruits/";

var list = new Array();
//var list indicates full path to the images including its file name.
for (let i = 0; i < 4; i++) {
    list[i] = Directry + i + ".png";   
    new Image().src = list[i];
    //Create image of which src is list[i].
}

var counter = 0;

function spin() {
    
}  

﻿$(function () {
    const $spinForm = $('#spin-form');

    $spinForm.on('submit', function (event) {
        event.preventDefault();

        $.get($commentForm.attr('action'), function (serverData) {
            $('#comment-section').prepend(serverData);
        });
    });
});


$(document).ready(function () {
    $("#spin-btn").click(function () {
        $.post("demo_test_post.asp",
            {
                name: "Donald Duck",
                city: "Duckburg"
            },
            function (data, status) {
                alert("Data: " + data + "\nStatus: " + status);
            });
    });
});

