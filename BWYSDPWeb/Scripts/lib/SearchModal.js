$(function () {
    $('#searchModal').on('show.bs.modal', function (e) {
        // 关键代码，如没将modal设置为 block，则$modala_dialog.height() 为零
        $(this).css('display', 'block');

        if (parseInt(sdp_globModalzindex) > 0) {
            $(this).css('z-index', parseInt(sdp_globModalzindex) + 1);
        }
        sdp_globModalzindex = $(this).css("z-index");
        if ($(window).width() > 780) {
            $('#searchModal .modal-dialog').width($(window).width() - 400);
        }
        var modalHeight = $(window).height() / 2 - $('#searchModal .modal-dialog').height() / 2;
        var modalWidth = $('#searchModal .modal-dialog').width() / 2;
        if ($(window).width() > 780) {
            $(this).find('.modal-dialog').css({
                'margin-top': modalHeight,
                'margin-left': -modalWidth
            });
        }

        var button = $(e.relatedTarget); // 触发事件的按钮
        var param = button.data('whatever') // 解析出data-whatever内容
        $('#searchModal .modal-title').text(param+"主数据");
    });
    drag("searchModal");
});