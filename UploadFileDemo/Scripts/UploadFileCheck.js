function submitClick() {
    fileChange(document.getElementById("fileInput"));
}

function fileChange(target) {
    if (target.files && target.files.length != 0) {

        if (checkFileType(target))
            checkFileSize(target);

    } else {
        showMsg("请选择文件");
    }
}

function checkFileType(target) {
    var fileType = getFileType(target);
    var fileTypes = [".pdf"];
    if (fileTypes.indexOf(fileType) == -1) {
        showMsg("文件类型不对");
        return false;
    }
    return true;
}

function getFileType(target) {

    var isIE = /msie/i.test(navigator.userAgent) && !window.opera;
    if (isIE) {
        var filePath = target.value;
        var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
        var file = fileSystem.GetFile(filePath);
        return file.Type;
    } else {
        var filename = target.files[0].name;
        return filename.substr(filename.lastIndexOf(".")).toLowerCase();
    }
}

function checkFileSize(target) {
    var fileSize = getFileSize(target);
    if (fileSize > 2) {
        showMsg("文件大小请小于2M");
        return false;
    }
    return true;
}

function getFileSize(target) {
    var fileSize = 0;

    var isIE = /msie/i.test(navigator.userAgent) && !window.opera;
    if (isIE) {
        var filePath = target.value;
        var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
        var file = fileSystem.GetFile(filePath);
        fileSize = file.Size;
    } else {
        fileSize = target.files[0].size;
    }

    return fileSize / 1024 / 1024;
}

function showMsg(msg) {
    //alert(msg);
    $("#fileMsg").html(msg);
}