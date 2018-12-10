$(function () {
    const $rform = $('#retrieve-form');
    $rform.append("__RequestVerificationToken", "@HtmlHelper.GetAntiForgeryToken()");

    $rform.on('submit', function (e) {
        e.preventDefault();
        var f = $(this);
        $.post(f.attr('action'), f.serialize(), function (serverData) {
            $('#status-msg').html(serverData);

            $('#withdraw-amount').val('0');

            let container = $("#component-balance");
            $.get(MyAppUrlSettings.UserBalanceComponent, function (data) { container.html(data); });
        });
    });
});