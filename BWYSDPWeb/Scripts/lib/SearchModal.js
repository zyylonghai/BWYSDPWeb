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
        var param = button.data('whatever'); // 解析出data-whatever内容
        var Modalnm = button.data('modalnm');//模态框标题
        var deftb = button.data('deftb');//来源自定义表名
        var fromdsid = button.data('fromdsid');//来源数据源id
        var tbstruct = button.data('tbstruct');//来源表结构
        var controlnm = button.data('controlnm');//服务端的controller
        var flag = button.data('flag');//1标识单据的搜索，2标识来源主数据的搜索
        $('#searchModal .modal-title').text(Modalnm + "主数据");

        var o = $('#searchModal').find("select[name='sdp_smodalfield1']");
        o.children().remove();
        o.append("<option value=\"0\">默认选择</option>");
        $('#sdp_smodalCondition').children().first().siblings().remove();//移除所有条件元素，除第一条外。

        $('#sdp_smodalcondadd1').unbind('click');
        $('#sdp_smodalcondadd1').click(function () {
            AddCondition();
        });

        $('#sdp_smodalbtnSearch').unbind('click');
        $('#sdp_smodalbtnSearch').click(function () {
            DoSearch(flag, controlnm, fromdsid, deftb, tbstruct);
        });
        if (flag == 1)
            GetFields(tbstruct, controlnm);
    });
    drag("searchModal");
});

function GetFields(tbnm,ctrnm) {
    var o = $('#searchModal').find("select[name='sdp_smodalfield1']");
    $.ajax({
        async: false,
        type: "Get",
        url: '/' + ctrnm + '/GetSearchCondFields',
        data: "tbnm=" + tbnm + "",
        dataType: "Json",
        success: function (obj) {
            if (obj.flag == 0) {
                //o.children().remove();
                $.each(obj.data, function (index, row) {
                    o.append("<option value='" + row.AliasNm + "." + row.FieldNm + "'>" + row.TableNm + "." + row.FieldNm+"</option>");
                });
            }
        },
        error: function () {
        }
    });
   
}
var _condindex = 1;
//var _flag = 1;
function AddCondition() {
    let condition = $('#sdp_smodalCondition');
    _condindex++;
    //condition.append("<label class=\"form-inline\">" + condition.children().first().html() + "</label>");
    $.ajax({
        async: false,
        type: "Get",
        url: '/DataBase/GetSmodalCondition',
        data: "index=" + _condindex + "",
        dataType: "Json",
        success: function (obj) {
            condition.append(obj.data);
            condition.find("select[name='sdp_smodalfield" + _condindex + "']").append(condition.find("select[name='sdp_smodalfield1']").html());
            $('#sdp_smodalcondadd' + _condindex).click(function () {
                AddCondition();
            });

            $('#sdp_smodalconddelet' + _condindex).click(function () {
                DeletCondition($(this).parent());
            });
        },
        error: function () {
        }
    });
}

function DeletCondition(obj) {
    obj.remove();
}
function DoSearch(flag, ctrnm,dsid,deftb,tbstruct) {
    $.ajax({
        async: false,
        type: "Post",
        url: '/' + ctrnm + '/DoSearchData',
        data: $('#sdp_smodalform').serialize() + "&flag=" + flag + "&dsid=" + dsid + "&deftb=" + deftb + "&tb=" + tbstruct + "",
        dataType: "Json",
        success: function (obj) {
            
        },
        error: function () {
        }
    });
}
//function BindClick(id,func) {
//    $('#' + id).click(function () {
//        if (func != null)
//            func();
//    });
//}