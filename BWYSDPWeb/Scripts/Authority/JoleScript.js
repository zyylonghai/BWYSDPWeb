function ShowActionD() {
    var seletdr = $('#GridGroup2').bootstrapTable('getSelections');
    if (seletdr == null || seletdr.length == 0) {
        ShowMsg('未选择要编辑的行', 'error');
        return false;
    }
    if (seletdr.length > 1) {
        ShowMsg('只能选择一行进行编辑', 'error');
        return;
    }
    let progid = seletdr[0].ProgId;
    $.ajax({
        url: "/Authority/GetActionDetailView/",
        data: { progid: progid },
        type: 'Post',
        success: function (content) {
            ShowComModal("操作对象明细", content, OkClick);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
    
}

function OkClick() {
    var datas = [];
    var data;
    $('#action').find('li').each(function (index, o) {
        var checkobj = $(this).children().first();
        var v = checkobj.prop("checked");
        if (!v) {
            data = new Object();
            data.GroupId = checkobj.attr("name").split('.')[0];
            data.ObjectId = checkobj.attr("name").split('.')[1];
            data.ObjectType = "1";
            datas.push(data);
        }
    });

    $('#data').find('li').each(function (index, o) {
        var checkobj = $(this).children().first();
        var v = checkobj.prop("checked");
        if (!v) {
            data = new Object();
            data.GroupId = checkobj.attr("name").split('.')[0];
            data.ObjectId = checkobj.attr("name").split('.')[1];
            data.ObjectType = "2";
            datas.push(data);
        }
    });
    var seletdr = $('#GridGroup2').bootstrapTable('getSelections');
    let progid = seletdr[0].ProgId;
    $.ajax({
        url: "/Jole/SaveActionDetail/",
        data: { data: datas, progid: progid },
        type: 'Post',
        success: function (content) {
            CloseComModal();
            //ShowComModal("操作对象明细", content, OkClick);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status.toString() + ":" + XMLHttpRequest.readyState.toString() + "," + textStatus + errorThrown);
        }
    });
}