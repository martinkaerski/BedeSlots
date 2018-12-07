$rows = $('#rows').val();
$cols = $('#cols').val();

let directory = "/images/fruits/";

//var list indicates full path to the images including its file name.
let list = new Array();

if ($rows == 4) {
    list[0] = directory + "4b.png";
    list[1] = directory + "4a.png";
    list[2] = directory + "4w.png";
    list[3] = directory + "4p.png";
}
else if ($rows == 5) {
    list[0] = directory + "5b.png";
    list[1] = directory + "5a.png";
    list[2] = directory + "5w.png";
    list[3] = directory + "5p.png";
}
else if ($rows == 8) {
    list[0] = directory + "8b.png";
    list[1] = directory + "8a.png";
    list[2] = directory + "8w.png";
    list[3] = directory + "8p.png";
}

$("#spin-form").submit(function (event) {
    event.preventDefault();
    const $spinForm = $("#spin-form");
    const dataToSend = $spinForm.serialize();
    //$('#spin').play();
    document.getElementById('spin').play();

    let counter = 0;
    function slot(requestFunction) {

        let Random = setInterval(function () {
            counter++;
            for (var i = 0; i < $rows; i++) {
                for (var j = 0; j < $cols; j++) {
                    let idName = "" + i + j;
                    let rnd = Math.floor(Math.random() * 4);
                    $("#p" + idName).attr("src", list[rnd]);
                }
            }

            if (counter > 15) {
                document.getElementById('spin').pause();
                requestFunction();
                counter = 0;
                clearInterval(Random);
            }
        }, 100);
    }

    slot(function () {
        $.ajax({
            url: '@Url.Action("Spin", "Game")',
            type: "Post",
            data: dataToSend,
            success: function (partialViewResult) {
                $("#partial").empty();
                $("#partial").html(partialViewResult);

                $stake = $('#stake-earning').val();

                debugger;
                if ($stake > 0) {
                    document.getElementById('win').play();
                }
                let container = $("#component-balance");
                $.get("/Game/BalanceViewComponent", function (data) { container.html(data); });
            },
            error: function (arg, data, value) {
                console.log(arg + data + value);
            }
        })
    });
});