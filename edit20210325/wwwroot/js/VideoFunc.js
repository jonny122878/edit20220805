const { Alert } = require("../lib/bootstrap/dist/js/bootstrap.bundle");


function BlogVideoUrl(videoCtrlId,classID, videoId, url) {
    var $source = $(videoId);
    $(classID).show();
    fetch(url)
        .then(function (response) {
            return response.blob();
        })
        .then(function (myBlob) {
            let objectURL = URL.createObjectURL(myBlob);
            $source[0].src = objectURL;
            $source.parent()[0].load();
            var videoId = document.getElementById(videoCtrlId);
            videoId.style.cssText = 'object-fit: fill;width:100%';
            videoId.preload = "none";
        });
}





