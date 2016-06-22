// Write your Javascript code.

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function inIframe() {
    try {
        return window.self !== window.top;
    } catch (e) {
        return true;
    }
}

$(document).ready(function () {
    var qsParams = getUrlVars();

    $("a.maintainQueryParams").each(function () {
        var $this = $(this);
        var _href = $this.attr("href");
        $this.attr("href", _href + "?session=" + qsParams["Session"] + "&domain=" + qsParams["Domain"]);
    });

    $("#code_url").text(function () {
        return $(this).text().replace("domain", document.domain);
    });

    if (inIframe()) {
        $(".when_embedded").show();
        $(".when_standalone").hide();
    }
});