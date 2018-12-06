$(function () {
    const dform = $('#depositform');

    dform.on('submit', function (e) {
        e.preventDefault();
        debugger;
        var f = $(this);
        $.post(f.attr('action'), f.serialize(), function (res) {
            debugger;            
                dform.find('input').val('0');

                $('#balance-dropdown').empty();
                $('#balance-dropdown').html(res);
                    
                $('#status-msg').html(res);
        });
    });
});

