$(function () {
    const $rform = $('#retrieve-form');
    $rform.append("__RequestVerificationToken", "@HtmlHelper.GetAntiForgeryToken()");

    $rform.on('submit', function (e) {
        e.preventDefault();
        debugger;
        var f = $(this);
        $.post(f.attr('action'), f.serialize(), function (serverData) {
            debugger;
            if (serverData.message === undefined) {
                $rform.find('input').val('');
                
                $('#balance-dropdown').empty();
                $('#balance-dropdown').html(serverData);
                alert('Retrieving successfull!');
                
            }
            else {
                $rform.find('input').val('0');
                alert(serverData.message);
            }
        });
    });
});