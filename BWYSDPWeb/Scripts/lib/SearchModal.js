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
        o.children().remove(); //清空字段元素列表。
        o.append("<option value=\"0\">默认选择</option>");
        $('#sdp_smodalCondition').children().first().siblings().remove();//移除所有条件元素，除第一条外。

        $('#sdp_smodalform').find("input").each(function () { $(this).val(""); });

        $('#sdp_smodalform').find(".bootstrap-table").remove();//清空表格。
        $('#sdp_smodalform').find(".clearfix").remove();
        if ($('#sdp_smodaldata') == undefined || $('#sdp_smodaldata') == null || $('#sdp_smodaldata').length==0) {
            $('#sdp_smodalform').append("<table id='sdp_smodaldata'></table>");
        }

        $('#sdp_smodalcondadd1').unbind('click');
        $('#sdp_smodalcondadd1').click(function () {
            AddCondition();
        });

        $('#sdp_smodalbtnSearch').unbind('click');
        $('#sdp_smodalbtnSearch').click(function () {
            DoSearch(flag, controlnm, fromdsid, deftb, tbstruct);
        });
        if (flag == 1) {
            $('#searchModal').find(".modal-footer").css("display","none");
            GetFields(tbstruct, controlnm);
        }
        else {
            $('#searchModal').find(".modal-footer").css("display", "block");
        }
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
                    o.append("<option value='" + row.AliasNm + "." + row.FieldNm + "'>" + row.DisplayNm + "(" + row.TableNm+")</option>");
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
            if (obj.flag == 0) {
                BindToTable(ctrnm, tbstruct);
                $('#sdp_smodaldata').bootstrapTable('refresh');
            }
        },
        error: function () {
        }
    });
}

function BindToTable(ctrnm,tbnm) {
    var sdp_searchtb = new LibTable("sdp_smodaldata");
    sdp_searchtb.$table.url = "/" + ctrnm + "/BindSmodalData?tableNm=" + tbnm + "";
    sdp_searchtb.$table.hasoperation = false;
    var cols = [];
    cols.push({
        checkbox: true,
        visible: true });
    var o = $('#searchModal').find("select[name='sdp_smodalfield1']");
    $.each(o.children(), function (index, option) {
        if (option.value != 0) {
            let arrary = option.value.split('.');
            cols.push({ field: arrary[1], title: option.outerText, align: 'center' });
        }
    });
    sdp_searchtb.$table.columns = cols;
    sdp_searchtb.initialTable();
    sdp_searchtb.testid = tbnm;
    sdp_searchtb.DbClickRow = function (row, elem, tbname) {
        $.ajax({
            async: false,
            type: "Post",
            url: '/DataBase/FillAndEdit',
            data: { row, "tablenm":tbname },
            dataType: "Json",
            success: function (obj) {


            },
            error: function () {
            }
        });
    }
}