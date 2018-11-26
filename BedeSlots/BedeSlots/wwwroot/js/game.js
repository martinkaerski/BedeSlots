﻿var Directry = "images/fruits/";

var list = new Array();
//var list indicates full path to the images including its file name.
for (let i = 0; i < 4; i++) {
    list[i] = Directry + i + ".png";   
    new Image().src = list[i];
    //Create image of which src is list[i].
}

var counter = 0;

function slot() {
    var Random = setInterval(function () {
        
        counter++;

        let left = Math.floor(Math.random() * 4);
        let center = Math.floor(Math.random() * 4);
        let right = Math.floor(Math.random() * 4);
       
        document.left.src = list[left];
        document.center.src = list[center];
        document.right.src = list[right];

        document.sleft.src = list[center];
        document.scenter.src = list[right];
        document.sright.src = list[left];

        document.tleft.src = list[right];
        document.tcenter.src = list[left];
        document.tright.src = list[left];

        //Show the pictures during the process.
        if (counter > 10) {
            let final_left = list[left];
            let final_center = list[center];
            let final_right = list[right];
            
            if ((final_left == final_center) && (final_left == final_right)) {
                document.getElementById("all").innerHTML = "You won!";

            } else {
                document.getElementById("all").innerHTML = "You lost. Try Again!";
            }

            counter = 0;
            clearInterval(Random);
            // After the result will be displayed the random number generating process should be closed.
        }
    }, 140);  
}  