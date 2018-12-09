$(function () {
    const $dform = $('#depositform');
    $dform.append("__RequestVerificationToken", "@HtmlHelper.GetAntiForgeryToken()");

    $dform.on('submit', function (e) {
        e.preventDefault();
        debugger;
        var f = $(this);
        $.post(f.attr('action'), f.serialize(), function (res) {        
            $('#status-msg').html(res); 

            $('#deposit-amount').val('0');

            let container = $("#component-balance");
            $.get(MyAppUrlSettings.UserBalanceComponent, function (data) { container.html(data); });
        });
    });
});

