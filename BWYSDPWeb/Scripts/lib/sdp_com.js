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