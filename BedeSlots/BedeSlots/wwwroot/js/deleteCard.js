$(function () {
    const $deleteCardBtn = $('#delete-card-btn');

    $('#delete-card-btn').on('click', function () {
        const id = $deleteCardBtn.attr('id');
        debugger;
        const dataToSend = { id: id };
        const url = '@Url.Action("Delete", "Card")';
        debugger;

        $.post($addCardForm.attr('action'), dataToSend, function (serverData) {
            $('#card-details').modal('hide');
            $("#select-card-dropdown").empty();
            $("#select-card-dropdown").html(serverData);

            return false;
        });
    });
});