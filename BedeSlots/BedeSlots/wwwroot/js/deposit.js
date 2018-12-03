$(function () {
    const dform = $('#depositform');

    dform.on('submit', function (e) {
        e.preventDefault();
        debugger;
        var f = $(this);
        $.post(f.attr('action'), f.serialize(), function (res) {
            debugger;
            if (res.message != null) {
                alert(res.message);
                window.location.href = '/Game/Index';
            }
        });
    });
});

