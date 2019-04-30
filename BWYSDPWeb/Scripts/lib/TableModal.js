//$(function () {
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
            $('#' + id + ' .modal-title').text(thisobj.DeftbNm + "新增");
            if (thisobj.ControlNm == "" || thisobj.ControlNm == undefined) {
                thisobj.ControlNm = "DataBase";
            }
            //TableAction(gid, thisobj.DeftbNm, controlnm, cmd);
        });
        drag(id);
    },
    Confirm: function () {
        var formid = $('#' + this.ModalID + ' form').attr("id");
        let thisobj = this;
        $.ajax({
            async: false,
            type: "POST",
            //url: '${pageContext.request.contextPath}/link/apply',
            url: '/' + this.ControlNm + '/TableAction',
            data: $('#' + formid).serialize() + '&gridid=' + this.GridId + '&tbnm=' + this.DeftbNm + '&tableNm=' + this.TableNm + '&cmd=' + this.Cmd + '',
            dataType: "text",
            success: function (data) {
                $("#" + thisobj.ModalID).modal('hide');
                $('#' + thisobj.GridId).bootstrapTable('refresh');
            },
            error: function () {
            }
        });

        //alert(formid);
    }
}


function TableAction(gridid, tbnm, controlnm,cmd)
{
    $.ajax({
        async: false,
        type: "POST",
        //url: '${pageContext.request.contextPath}/link/apply',
        url: '/' + controlnm + '/TableAction',
        data: 'gridid=' + gridid + '&tbnm=' + tbnm + '&cmd=' + cmd + '',
        dataType: "text",
        success: function () {
        },
        error: function () {
        }
    });
}