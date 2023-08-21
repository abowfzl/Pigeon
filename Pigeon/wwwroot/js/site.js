// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const dataMiningForm = document.querySelector('#data-mining-form')

dataMiningForm.addEventListener('submit', async (event) => {
    event.preventDefault()
    const requestToken = $('input[name="__RequestVerificationToken"]').val()
    const dataMiningInput = $('#data-mining-input').val()
    $.ajax(
        {
            url: "/DataMining",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                "RequestVerificationToken": requestToken,
            },
            data: JSON.stringify({ "Input": dataMiningInput }),
            success: function (result) {

                $('#data-mining-input').val(dataMiningInput+ ', ' + result);
            },
            error: function (xhr, status, p3, p4) {
                var err = "Error " + " " + status + " " + p3;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).message;
                console.log(err);
            }
        });
})