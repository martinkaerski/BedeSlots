$(function () {
    const $addCardForm = $('#addCardForm');

    $addCardForm.on('submit', function (event) {
        event.preventDefault();
        const dataToSend = $addCardForm.serialize();

        $.post($addCardForm.attr('action'), dataToSend, function (serverData) {
            $('#AddCardModal').modal('hide');
            $addCardForm.find('input').val('');
            $("#select-card-dropdown").empty();
            $("#select-card-dropdown").html(serverData);

            return false;
        });
    });
});