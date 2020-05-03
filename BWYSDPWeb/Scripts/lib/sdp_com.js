$(function () {
    $('body').append("<div id=\"sdp_info\" class=\"container navbar-fixed-top\"> <div id=\"sdp_errorinfo\" class=\"alert alert-danger\"> <a class=\"close\" href=\"#\" onclick=\"closemsgbyobj(this)\">&times;</a> <p id=\"sdp_error_content\"></p></div><div id=\"sdp_warninginfo\" class=\"alert alert-warning\"> <a class=\"close\" href=\"#\" onclick=\"closemsgbyobj(this)\">&times;</a> <p id=\"sdp_warning_content\"></p></div><div id=\"sdp_successinfo\" class=\"alert alert-success\"> <a class=\"close\" href=\"#\" onclick=\"closemsgbyobj(this)\">&times;</a> <p id=\"sdp_success_content\"></p></div></div>");
    //$('body').append("<div id=\"sdp_warninginfo\" class=\"container navbar-fixed-top\"> <div  class=\"alert alert-warning\"> <a class=\"close\" href=\"#\" onclick=\"closemsg()\">&times;</a> <p id=\"sdp_warning_content\"></p></div></div>");
    //$('body').append("<div id=\"sdp_successinfo\" class=\"container navbar-fixed-top\"> <div  class=\"alert alert-success\"> <a class=\"close\" href=\"#\" onclick=\"closemsg()\">&times;</a> <p id=\"sdp_success_content\"></p></div></div>");
    //$('body').append("<div  class=\"alert alert-danger\">");
    //$('body').append("<a class=\"close\" href=\"#\" onclick=\"closemsg()\">&times;</a>");
    //$('body').append("<p>显示了错误信息提示框</p>");
    //$('body').append("</div>");
    //$('body').append("</div>");

    //$('#sdp_info').hide();
    $('#sdp_errorinfo').hide();
    $('#sdp_warninginfo').hide();
    $('#sdp_successinfo').hide();
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
    else if (msgtype == "promt") {
        $('#sdp_success_content').html(msg);
        $('#sdp_successinfo').show();
    }
}

function closemsg() {
    $('#sdp_errorinfo').hide();
    $('#sdp_warninginfo').hide();
    $('#sdp_successinfo').hide();
}

function closemsgbyobj(obj) {
    $(obj).parent().hide();
}

function Serialobj(obj) {
    var o = { grid: obj.testid, addrows: obj.RowsOfAdd, editRows: obj.RowsOfEdit, removrows: obj.RowsOfRemov };
    return JSON.stringify(o);
}

function SDP_Save(datastr, ctrNm) {
    
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
    $('#sdp_form').submit();
}

function SDP_Add(ctrNm) {
    $.ajax({
        url: "/" + ctrNm + "/Add",
        data: "",
        type: 'Post',
        async: false,
        dataType: "json",
        success: function (obj) {
            var grids = $("#sdp_form").find("table");
            $.each(grids, function (index, o) {
                $('#' + o.id).bootstrapTable('refresh');
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
}

function SDP_Edit(ctrNm) {
    $.ajax({
        url: "/" + ctrNm + "/Edit",
        data: "",
        type: 'Post',
        async: false,
        dataType: "json",
        success: function (obj) {
            var grids = $("#sdp_form").find("table");
            $.each(grids, function (index, o) {
                $('#' + o.id).bootstrapTable('refresh');
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
}

function SDP_datalogSearch(ctrNm) {
    $.ajax({
        url: "/Home/DataLogSearch",
        data: "",
        type: 'Post',
        async: false,
        success: function (content) {
            ShowComModal("数据日志明细", content, function () {
                CloseComModal();
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
}

function SDP_RptBtnSearch(ctrnm) {
    $.ajax({
        url: "/" + ctrnm + "/RptSearchData",
        data: $('#sdp_rptForm').serialize() + '&rptcols=' + $('#sdp_rptCols').val() + '&rptsumarycols=' + $('#sdp_rptsumaryCols').val(),
        type: 'Post',
        async: false,
        dataType: "json",
        success: function (obj) {
            $('#sdp_rptgridgroupbtn').children().each(function (i, o) {

            });
            var grids = $("#sdp_rptForm").find("table");
            $.each(grids, function (index, o) {
                //var $grid = $('#' + o.id);
                //var options = $grid.bootstrapTable('getOptions');
                //$grid.bootstrapTable('destroy').bootstrapTable({
                //    url: options.url,
                //    height: options.height,
                //    columns: options.columns,
                //    showColumns: options.showColumns,
                //    toolbar: options.toolbar,
                //    fixedColumns: options.fixedColumns,
                //    fixedNumber: options.fixedNumber,
                //    fixedRightNumber: options.fixedRightNumber
                //});
                $('#' + o.id).bootstrapTable('refreshOptions', { pageNumber: 1 });
                //$('#' + o.id).bootstrapTable('refresh');
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
}
function SDP_RptExportExcel(ctrnm, gridid, dsid, tbnm) {
    window.location.href = "/" + ctrnm + "/RptDataExportExcel?sdp_pageid=" + $('#bwysdp_progid').val() + "&sdp_dsid=" + $('#bwysdp_dsid').val()+"&gridid=" + gridid + "&dsid=" + dsid + "&tbnm=" + tbnm;
    //$.ajax({
    //    url: "/" + ctrnm + "/RptDataExportExcel",
    //    data: 'gridid=' + gridid + '&dsid=' + dsid + '&tbnm=' + tbnm,
    //    type: 'Post',
    //    async: false,
    //    dataType: "text",
    //    success: function (obj) {

    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
    //    }
    //});
}
function SDP_RptGroupBy(ctrnm, tableid) {
    let fields = [];
    let gridid = "";
    $('#sdp_rptgridgroupbtn').children().each(function (i, o) {
        gridid = $(o).find('input').data('gridid');
        if ($(o).find('input')[0].checked) {
            fields.push($(o).find('input').data('field'));
        }
    });
    $.ajax({
        url: "/" + ctrnm + "/RptSearchDataByGroup",
        data: { fields },
        type: 'Post',
        async: false,
        dataType: "json",
        success: function (obj) {
            $('#' + gridid).bootstrapTable('refreshOptions', { pageNumber: 1 });
            //var grids = $("#sdp_rptForm").find("table");
            //$.each(grids, function (index, o) {
            //    if (o.id != undefined && o.id != '')
            //        $('#' + o.id).bootstrapTable('refreshOptions', { pageNumber: 1 });
            //});
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
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

function TableBtnAdd(obj) {

}

function TableBtnDelete(obj, grid,tbnm,tablenm,ctrl) {
    var seletdr = $('#' + grid).bootstrapTable('getSelections');
    if (seletdr == null || seletdr.length == 0) {
        ShowMsg('未选择要删除的行', 'error');
        return false;
    }
    //if (seletdr.length > 1) {
    //    ShowMsg('只能选择一行进行删除', 'error');
    //    return;
    //}
    var rowids = [];
    $.each(seletdr, function (index, o) {
        rowids.push({ FieldNm: "sdp_rowid", FieldValue: o.sdp_rowid });
    });
    $.confirm({
        title: '提示!',
        content: '确定删除选中的记录吗？',
        buttons: {
            确定: function () {
                $.ajax({
                    type: "POST",
                    url: '/' + ctrl + '/TableAction',
                    data: 'gridid=' + grid + '&tbnm=' + tbnm + '&tableNm=' + tablenm + '&cmd=Delet&row=' + JSON.stringify(rowids)+ '',
                    //data: fileData,
                    dataType: "text",
                    success: function (data) {
                        $('#' + grid).bootstrapTable('refresh');
                    },
                    error: function () {
                    }
                });
            },
            取消: function () {
            }
            //somethingElse: {
            //    text: 'Something else',
            //    btnClass: 'btn-blue',
            //    keys: ['enter', 'shift'],
            //    action: function () {
            //        $.alert('Something else?');
            //    }
            //}
        }
    });

}

function TableBtnCopy(obj, grid, tbnm, tablenm, ctrl) {
    var seletdr = $('#' + grid).bootstrapTable('getSelections');
    if (seletdr == null || seletdr.length == 0) {
        ShowMsg('未选择被复制的行', 'error');
        return false;
    }
    if (seletdr.length > 1) {
        ShowMsg('只能选择一行进行复制', 'error');
        return false;
    }
    var rowids = [];
    $.each(seletdr, function (index, o) {
        rowids.push({ FieldNm: "sdp_rowid", FieldValue: o.sdp_rowid });
    });
    $.ajax({
        url: "/" + ctrl + "/TableAction",
        data: 'gridid=' + grid + '&tbnm=' + tbnm + '&tableNm=' + tablenm + '&cmd=Copy&row=' + JSON.stringify(rowids) + '',
        type: 'Post',
        async: false,
        dataType: "json",
        success: function (obj) {
            $('#' + grid).bootstrapTable('refresh');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
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

function ImgFormatter(img) {
    return "<img src=\"data:image/*;base64," + img+"\" class=\"img-responsive\"  width=\"100\" height=\"100\"/>";
}

function NullFormatter(value) {
    if (value == 'null' || value == undefined)
        return "";
    return value;
    
}

function Showfuzzydiv(id) {
    $("#sdp_fuzzySearch").css("position", "absolute");
    $("#sdp_fuzzySearch").css("top", $("#" + id).offset().top + $("#" + id).parent().height());
    $("#sdp_fuzzySearch").css("left", $("#" + id).offset().left);
    if ($("#sdp_fuzzySearch").css("display") == "none") {
        $("#sdp_fuzzySearch").show("slow");
    } else {
        //$("#sdp_fuzzySearch").hide("slow");
    }
}

function Closefuzzydiv() {
    $("#sdp_fuzzySearch").hide("slow");
}

function onpropertychange(id,ctrnm) {
    $.ajax({
        url: "/" + ctrnm + "/InternalFuzzySearch",
        data: "id=" + id + "&val=" + $('#' + id).val() + "",
        type: 'Post',
        async: false,
        dataType: "json",
        success: function (obj) {
            var o = JSON.parse(obj.data);
            if (o.length > 0) {
                var rows = o[0].Table;
                var fuzzytb = $('#fuzzysearchtable');
                fuzzytb.children().remove();
                var thead = "<thead style=\"background-color: deepskyblue\"><tr>";
                var tbody = "<tbody>";
                $.each(rows[0], function (nm, val) {
                    thead += "<th>" + nm + "</th>";
                });
                thead += "</tr></thead>";
                fuzzytb.append(thead);
                $.each(rows, function (index, dr) {
                    tbody += "<tr onmousemove=\"mousemoveup(this)\" ondblclick=\"dblclick(this, '" + id + "')\">";
                    $.each(dr, function (nm, val) {
                        tbody += "<td>" + val + "</td>";
                    });
                    tbody += "</tr>";
                });
                tbody += "</tbody>";
                fuzzytb.append(tbody);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
}

function mousemoveup(obj) {
    $.each($(obj).parent().children(), function (index, o) {
        $(o).removeClass("selectrow");
    });
    $(obj).addClass("selectrow");

}

function dblclick(obj, id) {
    $.each(obj.cells, function (index, col) {
        if (index == 0) {
            $('#' + id).val(col.innerText);
        }
        if (index == 1) {
            $('#' + id + '_desc').text(col.innerText);
        }
    });
    Closefuzzydiv();
}

function ShowImgFile(fileid) {
    $('#' + fileid).click();
}

function LoadImgToUI(fileid, imgid) {
    debugger
    var imgRead = new FileReader();
    var imgfilter = ""
    var imgfile = $('#' + fileid).get(0).files[0];
    imgRead.readAsDataURL(imgfile);
    imgRead.onload = function (et) {
        $('#' + imgid).attr("src", et.target.result);
    }
}

function RefreshAllGrid() {
    var grids = $("#sdp_form").find("table");
    $.each(grids, function (index, o) {
        $('#' + o.id).bootstrapTable('refresh');
    });
}

function ShowComModal(title,html,okEvent) {
    $('#sdp_Modal_com').on('show.bs.modal', function (e) {
        // 关键代码，如没将modal设置为 block，则$modala_dialog.height() 为零
        $(this).css('display', 'block');

        if (parseInt(sdp_globModalzindex) > 0) {
            $(this).css('z-index', parseInt(sdp_globModalzindex) + 1);
        }
        sdp_globModalzindex = $(this).css("z-index");
        if ($(window).width() > 780) {
            $('#sdp_Modal_com .modal-dialog').width($(window).width() - 200);
        }
        var modalHeight = $(window).height() / 2 - $('#sdp_Modal_com .modal-dialog').height() / 2;
        var modalWidth = $('#sdp_Modal_com .modal-dialog').width() / 2;
        if ($(window).width() > 780) {
            $(this).find('.modal-dialog').css({
                'margin-top': modalHeight,
                'margin-left': -modalWidth
            });
        }
        $('#sdp_comModal_body').html(html);
        //var button = $(e.relatedTarget); // 触发事件的按钮
        //var param = button.data('whatever'); // 解析出data-whatever内容
        //var Modalnm = button.data('modalnm');//模态框标题
        //var deftb = button.data('deftb');//来源自定义表名
        //var fromdsid = button.data('fromdsid');//来源数据源id
        //var tbstruct = button.data('tbstruct');//来源表结构
        //var controlnm = button.data('controlnm');//服务端的controller
        //var fieldnm = button.data('fieldnm');//搜索控件关联的字段
        //var flag = button.data('flag');//1标识单据的搜索，2标识来源主数据的搜索
        $('#sdp_Modal_com .modal-title').text(title);
        $('#sdp_comModal_btnok').unbind();
        $('#sdp_comModal_btnok').click(okEvent);
    });
    $("#sdp_Modal_com").modal("show");
    drag("sdp_Modal_com");
}
function CloseComModal() {
    $("#sdp_Modal_com").modal('hide');
}
function AuthorityCheck(authorityObjs, objid, groupid) {
    let result = false;
    $.each(authorityObjs, function (i, o) {
        if (o.ObjectId == objid && o.GroupId == groupid) {
            result = true;
        }
    });
    return result;

}
function Authorize(authorityObjs) {
    $.each(authorityObjs, function (i, o) {
        if (o.ObjectType == 1) {
            $('#' + o.ObjectId).hide();
        }
    });
}
function FormGroupAuthorize(authorityObjs,fields) {
    $.each(fields, function (i, o) {
        let result = AuthorityCheck(authorityObjs, o.objectid, o.groupid);
        if (result) {
            $("label[for='" + o.objectid + "']").hide();
            $('#' + o.id).parent().hide();
            $('#' + o.id).hide();
        }
    });
}
function SetPageDisabled() {
    DoSetPageStatus(true);
}
function RemovePageDisabled() {
    DoSetPageStatus(false);
}

function DoSetPageStatus(ispreview) {
    $('#sdp_form').find('input').each(function (i, o) {
        if (o.id != "sdp_searchinput") $(o).attr("disabled", ispreview);
    });
    $('#sdp_form').find('textarea').each(function (i, o) { $(o).attr("disabled", ispreview); });
    $('#sdp_form').find('select').each(function (i, o) { $(o).attr("disabled", ispreview); });
    $('#sdp_form').find('a').each(function (i, o) {
        $(o).attr("disabled", ispreview);
    });
    $('#sdp_form').find('button').each(function (i, o) { if (o.id != "bwysdp_btnsave" && o.id != "bwysdp_btnadd" && o.id != "bwysdp_btnedit" && o.id != "bwysdp_btndelet" && o.id != "bwysdp_btnSearch") $(o).attr("disabled", ispreview); });
    if (ispreview)
        $('#bwysdp_ispreview').val(1);
    else
        $('#bwysdp_ispreview').val(0);
}

function GetInputValue(fldobjNm) {
    let result;
    $.ajax({
        url: "/DataBase/GetInputID",
        data: { fieldobjnm:fldobjNm },
        type: 'Get',
        async: false,
        dataType: "json",
        success: function (obj) {
            result= $('#' + obj.id).val();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
    return result;
}

function GetInputId(fldobjNm) {
    let result;
    $.ajax({
        url: "/DataBase/GetInputID",
        data: { fieldobjnm:fldobjNm },
        type: 'Get',
        async: false,
        dataType: "json",
        success: function (obj) {
            result= obj.id;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
    return result;
}

function GetMessage(msgId) {
    let result;
    $.ajax({
        url: "/DataBase/GetMessageBymsgid",
        data: { msgId: msgId },
        type: 'Get',
        async: false,
        dataType: "json",
        success: function (obj) {
            result = obj.message;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
    return result;
}

function CurrentDate() {
    var datetm = new Date();
    var year = datetm.getFullYear();
    var month = datetm.getMonth() + 1 < 10 ? "0" + (datetm.getMonth() + 1) : datetm.getMonth() + 1;
    var day = datetm.getDate() < 10 ? "0" + datetm.getDate() : datetm.getDate();
    return year + "-" + month + "-" + day;
}

function FindKeyValue(keyvaluearray, key) {
    var result = { key: "", value:""};
    $.each(keyvaluearray, function (i, o) {
        if (o.key == key)
            result = o;
    });
    return result;
}


