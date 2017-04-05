// Write your Javascript code.

$(document).ready(function () {
    function copyToClipboard(text) {
        window.prompt("Copy to clipboard: Ctrl+C, Enter", text);
    };
    $(".copy-to-clipboard").click(function () {
        var text = $(".url-result").text();
        copyToClipboard(text);
    });
});
