﻿//$(function () {
//    $('#sdp_tableModal').on('show.bs.modal', function (e) {
//        // 关键代码，如没将modal设置为 block，则$modala_dialog.height() 为零
//        $(this).css('display', 'block');
//        if ($(window).width() > 780) {
//            $('#sdp_tableModal .modal-dialog').width($(window).width() - 400);
//        }
//        var modalHeight = $(window).height() / 2 - $('#sdp_tableModal .modal-dialog').height() / 2;
//        var modalWidth = $('#sdp_tableModal .modal-dialog').width() / 2;
//        if ($(window).width() > 780) {
//            $(this).find('.modal-dialog').css({
//                'margin-top': modalHeight,
//                'margin-left': -modalWidth
//            });
//        }

//        var button = $(e.relatedTarget); // 触发事件的按钮
//        var param = button.data('deftbnm'); // 解析出data-whatever内容
//        var controlnm = button.data('controlnm');//
//        var gid = button.data("gridid");
//        var cmd = button.data("cmd");
//        $('#sdp_tableModal .modal-title').text(param + "新增");
//        if (controlnm == "" || controlnm == undefined) {
//            controlnm = "DataBase";
//        }
//        TableAction(gid, param, controlnm, cmd);
//    });
//    drag("sdp_tableModal");
//});

function libTableModal(id) {
    this.ModalID = id;
    this.ControlNm = "";
    this.GridId = "";
    this.DeftbNm = "";
    this.TableNm = "";
    this.Cmd = "";
    this.Currentrow = null;
    this.ProwId = "";
}

libTableModal.prototype = {
    constructor: libTableModal,
    initialModal: function () {
        var id = this.ModalID;
        var thisobj = this;
        $('#' + id).on('show.bs.modal', function (e) {
            // 关键代码，如没将modal设置为 block，则$modala_dialog.height() 为零
            $(this).css('display', 'block');
            if (parseInt(sdp_globModalzindex) > 0) {
                $(this).css('z-index', parseInt(sdp_globModalzindex) + 1);
            }
            sdp_globModalzindex = $(this).css("z-index");
            if ($(window).width() > 780) {
                $('#' + id + ' .modal-dialog').width($(window).width() - 400);
            }
            var modalHeight = $(window).height() / 2 - $('#' + id + ' .modal-dialog').height() / 2;
            var modalWidth = $('#' + id + ' .modal-dialog').width() / 2;
            if ($(window).width() > 780) {
                $(this).find('.modal-dialog').css({
                    'margin-top': modalHeight,
                    'margin-left': -modalWidth
                });
            }

            var button = $(e.relatedTarget); // 触发事件的按钮
            thisobj.DeftbNm = button.data('deftbnm'); // 解析出data-whatever内容
            thisobj.ControlNm = button.data('controlnm');//
            thisobj.GridId = button.data("gridid");
            thisobj.Cmd = button.data("cmd");
            thisobj.TableNm = button.data("tablenm");
            thisobj.ProwId = button.data("prowid");
            if (thisobj.ProwId == undefined)
                thisobj.ProwId = "";
            if (thisobj.Cmd == "Add") {
                $('#' + id + ' .modal-title').text(thisobj.DeftbNm + "新增");
            }
            else if (thisobj.Cmd == "Edit") {
                $('#' + id + ' .modal-title').text(thisobj.DeftbNm + "编辑");
            }
            else if (thisobj.Cmd == "Delet")
            {
                $('#' + id + ' .modal-title').text(thisobj.DeftbNm + "删除");
            }
            if (thisobj.ControlNm == "" || thisobj.ControlNm == undefined) {
                thisobj.ControlNm = "DataBase";
            }
            thisobj.GetTableRow();
            //TableAction(gid, thisobj.DeftbNm, controlnm, cmd);
        });
        drag(id);
    },
    GetTableRow: function () {
        var thisobj = this;
        var selectrowid="";
        if (this.Cmd == "Edit")
        {
            var seletdr = $('#' + thisobj.GridId).bootstrapTable('getSelections');
            selectrowid = seletdr[0].sdp_rowid;
        }
        $.ajax({
            async: false,
            type: "POST",
            url: '/' + this.ControlNm + '/GetTableRow',
            data: 'gridid=' + this.GridId + '&tbnm=' + this.DeftbNm + '&tableNm=' + this.TableNm + '&rowid=' + selectrowid + '&prowid=' + this.ProwId + '&cmd=' + this.Cmd + '',
            //dataType: "text",
            success: function (data) {
                thisobj.Currentrow = data.sdp_data;
                //$.each(data.sdp_data, function (index, o) {
                //    var reg = RegExp(/sdp_rowid/);
                //    if (reg.test(o.FieldNm)) {
                //        thisobj.Currentsdpid = o.FieldValue;
                //    }
                //});
            },
            error: function () {

            }
        });
    },
    Confirm: function () {
        var formid = $('#' + this.ModalID + ' form').attr("id");
        let thisobj = this;
        var fileData = new FormData($('#' + formid)[0]);
        fileData.append("gridid", this.GridId);
        fileData.append("tbnm", this.DeftbNm);
        fileData.append("tableNm", this.TableNm);
        fileData.append("cmd", this.Cmd);
        fileData.append("row", JSON.stringify(thisobj.Currentrow));
        //$("#" + formid + " input[type='file']").each(function () {
        //    var f = this.files[0];
        //    fileData.append("file", f);
        //});
        $.ajax({
            //async: false,
            type: "POST",
            //url: '${pageContext.request.contextPath}/link/apply',
            url: '/' + this.ControlNm + '/TableAction',
            //data: fileData + '&gridid=' + this.GridId + '&tbnm=' + this.DeftbNm + '&tableNm=' + this.TableNm + '&cmd=' + this.Cmd + '&row=' + JSON.stringify(thisobj.Currentrow) + '',
            data: fileData,
            //dataType: "text",
            processData: false,
            contentType: false,
            cache: false,
            success: function (data) {
                $("#" + thisobj.ModalID).modal('hide');
                $('#' + thisobj.GridId).bootstrapTable('refresh');
                $('#' + formid)[0].reset();
            },
            error: function () {
            }
        });

        //alert(formid);
    }
}


//function TableAction(gridid, tbnm, controlnm,cmd)
//{
//    $.ajax({
//        async: false,
//        type: "POST",
//        //url: '${pageContext.request.contextPath}/link/apply',
//        url: '/' + controlnm + '/TableAction',
//        data: 'gridid=' + gridid + '&tbnm=' + tbnm + '&cmd=' + cmd + '',
//        dataType: "text",
//        success: function () {
//        },
//        error: function () {
//        }
//    });
//}