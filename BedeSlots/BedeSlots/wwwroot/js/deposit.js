$(function () {
    const dform = $('#depositform');

    dform.on('submit', function (e) {
        e.preventDefault();
        debugger;
        var f = $(this);
        $.post(f.attr('action'), f.serialize(), function (res) {      
                //dform.find('input').val('0');
                //$('#balance-dropdown').empty();
            //$('#balance-dropdown').html(res);      
            if (typeof (res) === 'string') {
            $('#status-msg').html(res); 

            }

            let container = $("#component-balance");
            $.get("/Deposit/BalanceViewComponent", function (data) { container.html(data); });
        });
    });
});

