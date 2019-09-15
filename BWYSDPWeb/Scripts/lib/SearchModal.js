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
        var fieldnm = button.data('fieldnm');//搜索控件关联的字段
        var flag = button.data('flag');//1标识单据的搜索，2标识来源主数据的搜索
        $('#searchModal .modal-title').text(Modalnm + "主数据");

        var o = $('#searchModal').find("select[name='sdp_smodalfield1']");
        o.children().remove(); //清空字段元素列表。
        o.append("<option value=\"0\">默认选择</option>");
        $('#sdp_smodalCondition').children().first().siblings().remove();//移除所有条件元素，除第一条外。

        $('#sdp_smodalform').find("input").each(function () { $(this).val(""); });//清空条件值输入框。

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
        if (flag == 1) {/*调出的搜索模态框为单据上的搜索*/
            $('#searchModal').find(".modal-footer").css("display", "none");
            GetFields(tbstruct, controlnm, null, flag);
        }
        else {/*调出的搜索模态框为来源字段上的搜索*/
            $('#searchModal').find(".modal-footer").css("display", "block");
            GetFields(tbstruct, controlnm, fieldnm,flag);
        }
    });
    drag("searchModal");
});

function GetFields(tbnm, ctrnm, fieldnm, flag) {
    var o = $('#searchModal').find("select[name='sdp_smodalfield1']");
    $.ajax({
        async: false,
        type: "Get",
        url: '/' + ctrnm + '/GetSearchCondFields',
        data: "tbnm=" + tbnm + "&fieldnm=" + fieldnm + "&flag=" + flag + "",
        dataType: "Json",
        success: function (obj) {
            if (obj.flag == 0) {
                //o.children().remove();
                $.each(obj.data, function (index, row) {
                    o.append("<option value='" + (flag == 3 ? row.FieldNm: (row.TBAliasNm + "." + row.FieldNm)) + "'dsid='" + row.DSID + "' tbnm='" + row.TableNm + "' hid='" + row.Hidden + "' aliasnm='" + row.AliasNm + "' isdate='" + row.IsDateType + "'>" + row.DisplayNm + "(" + (flag == 3 ? row.FieldNm : (row.TBAliasNm + "." + row.FieldNm)) + ")</option>");
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
function DoSearch(flag, ctrnm, dsid, deftb, tbstruct) {
    //if ($('#sdp_smodaldata') != undefined || $('#sdp_smodaldata') != null || $('#sdp_smodaldata').length> 0) {
    //    $('#sdp_smodaldata').bootstrapTable('refreshOptions', { pageNumber: 1 });
    //}
    if (flag == 2) {
        var o = $('#searchModal').find("select[name='sdp_smodalfield1']");
        $.each(o.children(), function (index, option) {
            if (option.value != 0) {
                dsid = $(option).attr("dsid");
                tbstruct = $(option).attr("tbnm");
            }
        });
    }
    $.ajax({
        async: false,
        type: "Post",
        url: '/' + ctrnm + '/DoSearchData',
        data: $('#sdp_smodalform').serialize() + "&flag=" + flag + "&dsid=" + dsid + "&deftb=" + deftb + "&tb=" + tbstruct + "",
        dataType: "Json",
        success: function (obj) {
            if (obj.flag == 0) {
                BindToTable(ctrnm, tbstruct, dsid, flag);
                //$('#sdp_smodaldata').bootstrapTable('refresh');
                $('#sdp_smodaldata').bootstrapTable('refreshOptions', { pageNumber: 1 });
                closemsg();
            }
        },
        error: function () {
        }
    });
}

function BindToTable(ctrnm,tbnm,dsid,flag) {
    var sdp_searchtb = new LibTable("sdp_smodaldata");
    sdp_searchtb.$table.url = "/" + ctrnm + "/BindSmodalData?tableNm=" + tbnm + "&dsid=" + dsid + "&flag=" + flag + "";
    sdp_searchtb.$table.hasoperation = false;
    var cols = [];
    cols.push({
        checkbox: true,
        visible: true });
    var o = $('#searchModal').find("select[name='sdp_smodalfield1']");
    $.each(o.children(), function (index, option) {
        if (option.value != 0) {
            let vis = $(option).attr("hid") == "false" ? true : false;
            let aliasnm = $(option).attr("aliasnm");
            let arrary = flag == 3 ? option.value: option.value.split('.')[1];
            let date = $(option).attr("isdate") == "false" ? false : true;
            if (date) {
                cols.push({ field: (aliasnm == "" || aliasnm == undefined || aliasnm == "null") ? arrary: aliasnm, title: option.outerText, align: 'center', sortable: true, visible: vis, formatter: function (value, row, index) { return TimeConverToStr(value); } });
            }
            else
                cols.push({ field: (aliasnm == "" || aliasnm == undefined || aliasnm=="null") ? arrary: aliasnm, title: option.outerText, align: 'center', sortable: true, visible: vis });
        }
    });
    sdp_searchtb.$table.columns = cols;
    sdp_searchtb.initialTable();
    sdp_searchtb.testid = tbnm;
    sdp_searchtb.flag = flag;

    sdp_searchtb.DbClickRow = function (dr, elem, tbname, flag) {
        if (flag == 1) {
            $.ajax({
                async: false,
                type: "Post",
                url: '/DataBase/FillAndEdit',
                data: { dr, "tablenm": tbname, "flag": flag },
                dataType: "Json",
                success: function (obj) {
                    $("#searchModal").modal('hide');
                    var grids = $("#sdp_form").find("table");
                    $.each(grids, function (index, o) {
                        $('#' + o.id).bootstrapTable('refresh');
                    });
                },
                error: function () {
                   
                }
            });
        }
        else {
            let fromfieldnm;
            let fieldnm;
            let fromfielddesc
            $.each(dr, function (name, val) {
                let index = name.indexOf("_sdp_");
                if (index != -1) {
                    fieldnm = name.substring(0, index);
                    fromfieldnm = name.substring(index + 5);
                    //$('#' + tbname + '_' + fieldnm).val(dr.)
                }
                else {
                    index = name.indexOf("sdp_desc");
                    if (index != -1) {
                        fromfielddesc = name.substring(index + 8);
                    }
                }
            });
            $.each(dr, function (name, val) {
                if (name == fromfieldnm) {
                    $('#' + fieldnm).val(val);
                }
                else if (name == fromfielddesc) {
                    $('#' + fieldnm + '_desc').text(val);
                }
            });
            $("#searchModal").modal('hide');
        }
    }
}