$rows = $('#rows').val();
$cols = $('#cols').val();

let directory = "/images/fruits/";
//images paths
let arr = new Array();

if ($rows == 4) {
    arr[0] = directory + "4b.png";
    arr[1] = directory + "4a.png";
    arr[2] = directory + "4w.png";
    arr[3] = directory + "4p.png";
}
else if ($rows == 5) {
    arr[0] = directory + "5b.png";
    arr[1] = directory + "5a.png";
    arr[2] = directory + "5w.png";
    arr[3] = directory + "5p.png";
}
else if ($rows == 8) {
    arr[0] = directory + "8b.png";
    arr[1] = directory + "8a.png";
    arr[2] = directory + "8w.png";
    arr[3] = directory + "8p.png";
}

var isStopped = false;

const $spinBtn = $('#spin-button');
const $spinForm = $('#spin-form');
$spinBtn.on('click', Spin);

function Stop() {
    isStopped = true;

    $spinBtn.off('click', Stop);
    $spinBtn.on('click', Spin);
    $spinBtn.text('Start');
    $spinBtn.removeClass('btn-danger');
}

function Spin (event) {
    event.preventDefault();
    $spinBtn.off('click', Spin);
    $spinBtn.on('click', Stop);
    $spinBtn.text('Stop');
    $spinBtn.addClass('btn-danger');
        
    const $spinForm = $("#spin-form");
    $spinForm.append("__RequestVerificationToken", "@HtmlHelper.GetAntiForgeryToken()");
    const dataToSend = $spinForm.serialize();
    document.getElementById('spin-audio').play();
       
    slot(function () {
        $.ajax({
            url: $spinForm.attr('action'),
            type: "Post",
            data: dataToSend,
            success: function (partialViewResult) {
                debugger;

                $("#partial").empty();
                $("#partial").html(partialViewResult);

                //$stake = $('#stake-earning').val();

                //debugger;
                //if ($stake > 0) {
                //    document.getElementById('win-audio').play();
                //}
                let container = $("#component-balance");
                $.get(MyAppUrlSettings.UserBalanceComponent, function (data) { container.html(data); });
            },
            error: function (res) {
                debugger;
                $("#status-msg").empty();                
                $("#status-msg").html(res);                
            }
        })
    });
}

function slot(requestFunction) {
    let Random = setInterval(function () {
        for (var i = 0; i < $rows; i++) {
            for (var j = 0; j < $cols; j++) {
                let idName = "" + i + j;
                let rnd = Math.floor(Math.random() * 4);
                $("#p" + idName).attr("src", arr[rnd]);
            }
        }

        if (isStopped) {
            document.getElementById('spin-audio').pause();
            requestFunction();
            isStopped = false;
            clearInterval(Random);
        }
    }, 100);
}