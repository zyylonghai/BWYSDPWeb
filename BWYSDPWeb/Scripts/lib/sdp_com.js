$(function () {
    $('body').append("<div id=\"sdp_errorinfo\" class=\"container navbar-fixed-top\"> <div  class=\"alert alert-danger\"> <a class=\"close\" href=\"#\" onclick=\"closemsg()\">&times;</a> <p id=\"sdp_error_content\"></p></div></div>");
    $('body').append("<div id=\"sdp_warninginfo\" class=\"container navbar-fixed-top\"> <div  class=\"alert alert-warning\"> <a class=\"close\" href=\"#\" onclick=\"closemsg()\">&times;</a> <p id=\"sdp_warning_content\"></p></div></div>");
    //$('body').append("<div  class=\"alert alert-danger\">");
    //$('body').append("<a class=\"close\" href=\"#\" onclick=\"closemsg()\">&times;</a>");
    //$('body').append("<p>显示了错误信息提示框</p>");
    //$('body').append("</div>");
    //$('body').append("</div>");

    $('#sdp_errorinfo').hide();
    $('#sdp_warninginfo').hide();
});
var sdp_globModalzindex = 0;
function drag(id) {
    var x1, y1, x2, y2, offleft, offtop, isclik = 0;
    var wmax = $("#" + id).find('.modal-dialog').width();
    var hmax = $("#" + id).find('.modal-dialog').height();
    //$('#billno').val(wmax + "----" + hmax);
    $("#" + id).find('.modal-header').mousedown(function (e) {
        x1 = e.clientX;
        y1 = e.clientY;
        offleft = parseInt($("#" + id).find('.modal-dialog').css('margin-left'));
        offtop = parseInt($("#" + id).find('.modal-dialog').css('margin-top'));
        isclik = 1;
        //$('#billno').val(wmax + "----" + hmax + " :" + x1 + "-" + y1);
    });
    $("#" + id).find('.modal-header').mousemove(function (e) {
        if (isclik == 1) {
            x2 = e.clientX;
            y2 = e.clientY;
            var xx = x2 - x1 + offleft;
            var yy = y2 - y1 + offtop;
            //$('#matid').val(xx + "--" + yy + " :" + x2 + "-" + y2);
            //if ( xx < wmax) {
            $("#" + id).find('.modal-dialog').css('margin-left', xx + "px");
            //}
            if (yy <= 550) {
                $("#" + id).find('.modal-dialog').css('margin-top', yy + "px");
            }
        }

    }).mouseup(function () {
        isclik = 0;
    });
}

function ShowMsg(msg, msgtype) {
    if (msgtype == "error") {
        $('#sdp_error_content').html(msg);
        $('#sdp_errorinfo').show();
    }
    else if (msgtype == "warning") {
        $('#sdp_warning_content').html(msg);
        $('#sdp_warninginfo').show();
    }
}

function closemsg() {
    $('#sdp_errorinfo').hide();
    $('#sdp_warninginfo').hide();
}

function Serialobj(obj) {
    var o = { grid: obj.testid, addrows: obj.RowsOfAdd, editRows: obj.RowsOfEdit, removrows: obj.RowsOfRemov };
    return JSON.stringify(o);
}

function Save(datastr, ctrNm) {
    
    //$.ajax({
    //    url: "/" + ctrNm + "/Save",
    //    data: $('form').serialize() + "&datastr=" + datastr + "",
    //    type: 'Post',
    //    async: false,
    //    dataType: "json",
    //    success: function (obj) {
    //        alert(obj);
    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
    //    }
    //});
    showMask();
    $('#sdp_form').submit();
}

function TableBtnEdit(obj, grid) {
    let exist = $(obj).attr("data-toggle");
    if (!exist)
    {
        $(obj).attr("data-toggle", "modal");
    }
    var seletdr = $('#' + grid).bootstrapTable('getSelections');
    if (seletdr == null || seletdr.length == 0) {
        ShowMsg('未选择要编辑的行', 'error');
        $(obj).removeAttr("data-toggle");
        return false;
    }
    if (seletdr.length > 1) {
        ShowMsg('只能选择一行进行编辑', 'error');
        $(obj).removeAttr("data-toggle");
        return;
    }
    
    return false;
}

function TimeConverToStr(tm) {
    //var datetime = Date.parse(new Date(stringTime));
    //datetime.setTime(time);
    var datetm = new Date(tm);
    var year = datetm.getFullYear();
    var month = datetm.getMonth() + 1 < 10 ? "0" + (datetm.getMonth() + 1) : datetm.getMonth() + 1;
    var day = datetm.getDate() < 10 ? "0" + datetm.getDate() : datetm.getDate();
    return year + "-" + month + "-" + day;
}

function GetMsgForSave() {
    $.ajax({
        url: "/DataBase/GetMsgforSave",
        data:"" ,
        type: 'Post',
        async: false,
        dataType: "json",
        success: function (obj) {
            if (obj != null && obj != undefined && obj.Messagelist != null && obj.Messagelist != undefined) {
                let _errors = "";
                let _warnings = "";
                $.each(obj.Messagelist, function (index, o) {
                    if (o.MsgType == 1) {
                        _errors += o.Message + "<br/>";
                    }
                    else if (o.MsgType == 2) {
                        _warnings += o.Message + "<br/>";
                    }
                    

                });
                if (_errors.length > 0)
                    ShowMsg(_errors, 'error');
                if (_warnings.length > 0)
                    ShowMsg(_warnings, 'warning');
                
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
}
