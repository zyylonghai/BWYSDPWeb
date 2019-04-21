var _ajax = $.ajax;
//重写jquery的ajax方法
$.ajax = function (opt) {
    var fn = {
        data:"",
        error: function (XMLHttpRequest, textStatus, errorThrown) { },
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
            fn.data.sdp_pageid = $('#bwysdp_progid').val();
            fn.data.sdp_dsid = $('#bwysdp_dsid').val();
        }
        else
        {
            fn.data += "&sdp_pageid=" + $('#bwysdp_progid').val() + "";
            fn.data += "&sdp_dsid=" + $('#bwysdp_dsid').val() + "";
        }
    //}
    //扩展增强处理 
    var _opt = $.extend(opt, {
        data: fn.data,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //错误方法增强处理 
            fn.error(XMLHttpRequest, textStatus, errorThrown);
        },
        success: function (data, textStatus) {
            if (data != null && (data.sdp_flag != null && data.sdp_flag != undefined))
            {
                //for (var n = 0; n < data.sdp_data.length; n++)
                //{

                    $.each(data.sdp_data, function (index, o) {
                        $('#' + o.FieldNm).val(o.FieldValue);
                    });
                //}
               
            }
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