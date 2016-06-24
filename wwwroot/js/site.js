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

function saveJobIdToLocalStorage() {
    var job_id = $("#Id").val();
    var already_exists = false;

    var storage = window.localStorage;
    var domain = $('#User_Domain').val();
    var key = "jobids_" + domain;
    var joblist = storage.getItem(key);

    var jobs = []
    if (joblist) {
        jobs = joblist.split(',');
    }

    for (var i = 0; i < jobs.length; i++) {
        already_exists = jobs[i] == job_id;
        if (already_exists) {
            break;
        }
    }

    if (!already_exists) {
        jobs.push(job_id);
        storage.setItem(key, jobs.join(','));
    }
}

function showJobIds() {
    var storage = window.localStorage;
    var domain = $('#User_Domain').val();
    var key = "jobids_" + domain;
    var joblist = storage.getItem(key);

    var jobs = []
    if (joblist) {
        jobs = joblist.split(',');
        $("#Id").val(jobs[jobs.length -1]);
    } else {
        $("#h3prev").hide();
    }

    for (var i = 0; i < jobs.length; i++) {
        $('ul.prev_jobs').append("<li ><a class=\"maintainQueryParams\" href=\"/Job/Job/" + jobs[i] + "\">Job " + jobs[i] + "</a></li>");
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