$(function () {
    $('#sdp_tableModal').on('show.bs.modal', function (e) {
        // 关键代码，如没将modal设置为 block，则$modala_dialog.height() 为零
        $(this).css('display', 'block');
        if ($(window).width() > 780) {
            $('#sdp_tableModal .modal-dialog').width($(window).width() - 400);
        }
        var modalHeight = $(window).height() / 2 - $('#sdp_tableModal .modal-dialog').height() / 2;
        var modalWidth = $('#sdp_tableModal .modal-dialog').width() / 2;
        if ($(window).width() > 780) {
            $(this).find('.modal-dialog').css({
                'margin-top': modalHeight,
                'margin-left': -modalWidth
            });
        }

        var button = $(e.relatedTarget); // 触发事件的按钮
        var param = button.data('deftbnm'); // 解析出data-whatever内容
        var controlnm = button.data('controlnm');//
        var gid = button.data("gridid");
        var cmd = button.data("cmd");
        $('#sdp_tableModal .modal-title').text(param + "新增");
        if (controlnm == "" || controlnm == undefined) {
            controlnm = "DataBase";
        }
        TableAction(gid, param, controlnm, cmd);
    });
    drag("sdp_tableModal");
});

function TableAction(gridid, tbnm, controlnm,cmd)
{
    debugger
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