// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

$("input[type='checkbox'].teamSelect").change(function () {
    var a = $("input[type='checkbox'].teamSelect:checked");
    var truefalse = a.length % 2;
    if (truefalse == 0)
    {
        $("#simulateTeams").prop('disabled', false);
    }
    else {
        $("#simulateTeams").prop('disabled', true);
    }
});