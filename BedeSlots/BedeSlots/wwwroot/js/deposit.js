$(function () {
    const $dform = $('#depositform');

    $dform.on('submit', function (e) {
        e.preventDefault();
        debugger;
        var f = $(this);
        $.post(f.attr('action'), f.serialize(), function (serverData) {        
            $('#status-msg').html(serverData); 

            $('#deposit-amount').val('0');

            let container = $("#component-balance");
            $.get(MyAppUrlSettings.UserBalanceComponent, function (data) { container.html(data); });
        });
    });
});

