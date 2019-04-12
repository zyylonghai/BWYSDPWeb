$(function () {
    $('body').append("<div id=\"sdp_errorinfo\" class=\"container navbar-fixed-top\"> <div  class=\"alert alert-danger\"> <a class=\"close\" href=\"#\" onclick=\"closemsg()\">&times;</a> <p id=\"sdp_error_content\">显示了错误信息提示框</p></div></div>");
    //$('body').append("<div  class=\"alert alert-danger\">");
    //$('body').append("<a class=\"close\" href=\"#\" onclick=\"closemsg()\">&times;</a>");
    //$('body').append("<p>显示了错误信息提示框</p>");
    //$('body').append("</div>");
    //$('body').append("</div>");

    $('#sdp_errorinfo').hide();
});
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
        $('#sdp_error_content').text(msg);
        $('#sdp_errorinfo').show();
    }
    else if (msgtype == "warning") {

    }
}

function closemsg() {
    $('#sdp_errorinfo').hide();
}

function Serialobj(obj) {
    var o = { grid: obj.testid, addrows: obj.RowsOfAdd, editRows: obj.RowsOfEdit, removrows: obj.RowsOfRemov };
    return JSON.stringify(o);
}

function Save(datastr) {
    $.ajax({
        url: "/DataBase/Save",
        data: $('form').serialize() + "&datastr=" + datastr + "",
        type: 'Post',
        async: false,
        dataType: "json",
        success: function (obj)
        {
            alert(obj);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown)
        {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
}