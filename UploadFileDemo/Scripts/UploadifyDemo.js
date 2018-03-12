$(function () {
    
    //$('#lightbox_2').lightbox({ ifChange: true }); //有左右箭头
    $('#lightbox_2').lightbox(); //无左右箭头   

    $("#uploadify").uploadify({
        'langFile': '/Scripts/uploadify/zhLanguage.js',
        //指定swf文件
        'swf': '/Scripts/uploadify/uploadify.swf',
        //后台处理的页面
        'uploader': '/home/Handle',
        //按钮显示的文字
        'buttonText': '选择文件',
        //显示的高度和宽度，默认 height 30；width 120
        //'height': 15,
        //'width': 80,
        //上传文件的类型  默认为所有文件    'All Files'  ;  '*.*'
        //在浏览窗口底部的文件类型下拉菜单中显示的文本
        'fileTypeDesc': '*.*',
        //允许上传的文件后缀
        'fileTypeExts': '*.gif; *.jpg; *.png; *.rar;',
        //上传文件的大小限制
        'fileSizeLimit': '300000MB',
        //上传数量
        'queueSizeLimit': 25,
        //发送给后台的其他参数通过formData指定
        //'formData': { 'someKey': 'someValue', 'someOtherKey': 1 },
        //上传文件页面中，你想要用来作为文件队列的元素的id, 默认为false  自动生成,  不带#
        //'queueID': 'fileQueue',
        //选择文件后自动上传
        'auto': false,
        //设置为true将允许多文件上传
        'multi': true,
        //浏览按钮的背景图片路径
        //'buttonImage': 'upbutton.gif',
        //上传后是否消失
        removeCompleted: true,
        //选择上传文件后调用
        'onSelect': function (file) {

        },
        //整个队列上传完的触发
        'onQueueComplete': function (queueData) {
            console.log(queueData);
        },
        'onUploadProgress': function (file, bytesUploaded, bytesTotal, totalBytesUploaded, totalBytesTotal) {
            //有时候上传进度什么想自己个性化控制，可以利用这个方法
            //使用方法见官方说明
        },
        //返回一个错误，选择文件的时候触发
        'onSelectError': function (file, errorCode, errorMsg) {
            switch (errorCode) {
                case -100:
                    alert("上传的文件数量已经超出系统限制的" + $('#file_upload').uploadify('settings', 'queueSizeLimit') + "个文件！");
                    break;
                case -110:
                    alert("文件 [" + file.name + "] 大小超出系统限制的" + $('#file_upload').uploadify('settings', 'fileSizeLimit') + "大小！");
                    break;
                case -120:
                    alert("文件 [" + file.name + "] 大小异常！");
                    break;
                case -130:
                    alert("文件 [" + file.name + "] 类型不正确！");
                    break;
            }
        },
        //检测FLASH失败调用
        'onFallback': function () {
            alert("您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。");
        },
        //上传到服务器，服务器返回相应信息到data里
        'onUploadSuccess': function (file, data, response) {
            //alert(data);
            console.log(data);
            var dataObj = $.parseJSON(data);
            if (dataObj.Success) {
                //data.FileName ThumbnailFile
                //$("#fileQueue").append($("<img src={0} style='border:1px solid red;pdding-right:5px;'/>".replace("{0}", dataObj.ThumbnailFile)));
                
                var s = "<img src='{0}' alt='#' class='lightbox-pic' data-role='lightbox' data-source='{1}' data-group='group-2' data-id='{2}' data-caption='{3}'>";
                $("#fileQueue").append($(
                                            s.replace('{0}', dataObj.ThumbnailFile)
                                             .replace('{1}', dataObj.File)
                                             .replace('{2}', dataObj.FileName)
                                             .replace('{3}', dataObj.FileName)
                                        ));
                $('#fileQueue').lightbox(); //无左右箭头 
            }
        }
    });
});