﻿$(function () {
    const $addCardForm = $('#addCardForm');

    $addCardForm.on('submit', function (event) {
        event.preventDefault();

        const $cardnumberVal = $addCardForm.find('#CardNumber').val();
        const $tokenVal = $addCardForm.find('input[name="__RequestVerificationToken"]').val();

        const dataToSend = $addCardForm.serialize();

        $.post($addCardForm.attr('action'), dataToSend, function (serverData) {

            $('#exampleModal').modal('hide');
            $addCardForm.find('input').val(' ');
            return false;
        });
    });
});