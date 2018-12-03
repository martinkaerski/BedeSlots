$(function () {
    const $addCardForm = $('#addCardForm');

    $addCardForm.on('submit', function (event) {
        event.preventDefault();

        const $cardnumberVal = $addCardForm.find('#CardNumber').val();
        const $tokenVal = $addCardForm.find('input[name="__RequestVerificationToken"]').val();

        const dataToSend = $addCardForm.serialize();

        $.post($addCardForm.attr('action'), dataToSend, function (serverData) {
            debugger;
            $('#AddCardModal').modal('hide');
            $addCardForm.find('input').val('');
            //$addCardForm.find('#select-card-dropdown').replaceWith(serverData);
            $("#select-card-dropdown").empty();
            $("#select-card-dropdown").html(serverData);

            return false;
        });

        $.validator.unobtrusive.parse("#addCardForm");
    });
});