var _ajax = $.ajax;
var sdp_haserror = false;
//重写jquery的ajax方法
$.ajax = function (opt) {
    var fn = {
        data:"",
        error: function (XMLHttpRequest, textStatus, errorThrown) {
           
        },
        success: function (data, textStatus)
        {
        },
        complete: function (data) { },
        beforeSend: function (xhr, o) { },
    }
    if (opt.error) {
        fn.error = opt.error;
    }
    if (opt.success) {
        fn.success = opt.success;
    }
    if (opt.complete) {
        fn.complete = opt.complete;
    }
    if (opt.beforeSend) {
        fn.beforeSend = opt.beforeSend;
    }
    //if (opt.data)
    //{
        fn.data = opt.data;
    if (Object.prototype.toString.call(fn.data) != '[object String]') {
        if (Object.prototype.toString.call(fn.data) == '[object FormData]') {
            fn.data.append("sdp_pageid", $('#bwysdp_progid').val());
            fn.data.append("sdp_dsid", $('#bwysdp_dsid').val());
        }
        else {
            fn.data.sdp_pageid = $('#bwysdp_progid').val();
            fn.data.sdp_dsid = $('#bwysdp_dsid').val();
        }
    }
    else {
        fn.data += "&sdp_pageid=" + $('#bwysdp_progid').val() + "";
        fn.data += "&sdp_dsid=" + $('#bwysdp_dsid').val() + "";
    }
    //}
    //if (sdp_haserror)
    //    return;
    //扩展增强处理 
    var _opt = $.extend(opt, {
        data: fn.data,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (XMLHttpRequest.status == 500) {
                var responseobj = JSON.parse(XMLHttpRequest.responseText)
                if (responseobj != undefined || responseobj != null)
                    ShowMsg(responseobj.msg, 'error');
            }
            else {
                ShowMsg('ajax error: '+XMLHttpRequest.status+':' + textStatus+':' + errorThrown, 'error');
            }
            //错误方法增强处理 
            fn.error(XMLHttpRequest, textStatus, errorThrown);
        },
        success: function (data, textStatus) {
            if (data != null && (data.sdp_flag != null && data.sdp_flag != undefined && data.sdp_flag==0))
            {
                $.each(data.sdp_data, function (index, o) {
                    if (o.Isbinary) {
                        if (o.FieldValue != null && o.FieldValue != '' && o.FieldValue != undefined) {
                            $('#sdp_img_' + o.FieldNm).attr("src", "data:image/*;base64," + o.FieldValue);
                        }
                        else {
                            $('#sdp_img_' + o.FieldNm).attr("src", "/img/0.jpg");
                            //var fileobj = $('#' + o.FieldNm).get(0).files;
                            //if (fileobj.length > 0) {
                            //    fileobj.pop();
                            //}
                        }
                    }
                    else
                        $('#' + o.FieldNm).val(o.FieldValue);
                });
                if (data.sdp_preview) {
                    SetPageDisabled();
                }
                else {
                    RemovePageDisabled();
                }
               
            }
            if (data != null && data.sdp_msglist != null && data.sdp_msglist != undefined) {
                let _errors = "";
                let _warnings = "";
                let _promts = "";
                $.each(data.sdp_msglist, function (index, o) {
                    //_msg += o.Message + "<br/>";
                    if (o.MsgType == 1) {
                        _errors += o.Message + "<br/>";
                    }
                    else if (o.MsgType == 2) {
                        _warnings += o.Message + "<br/>";
                    }
                    else if (o.MsgType == 3) {
                        _promts += o.Message + "<br/>";
                    }

                });
                if (_errors.length > 0)
                    ShowMsg(_errors, 'error');
                else {
                    if (_warnings.length > 0)
                        ShowMsg(_warnings, 'warning');
                    if (_promts.length > 0) {
                        ShowMsg(_promts, 'promt');
                    }
                }
              
                sdp_haserror = true;
            }
            //else
                //成功回调方法增强处理 
                fn.success(data, textStatus);


        },
        complete: function (data) {
            fn.complete(data);
            hideMask();
        },
        beforeSend: function (xhr, o) {
            showMask();
            fn.beforeSend(xhr, o);

        },
    });
    return _ajax(_opt).done(function (e) {
        hideMask();
    });
}
//function FillVale(obj) {

//}