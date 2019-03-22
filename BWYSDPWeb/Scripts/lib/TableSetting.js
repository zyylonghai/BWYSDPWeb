
//var $table = {
//    ElemtableID: "",
//    url: "",
//    method: 'GET',
//    toolbar:"",
//    striped: true,
//    cache: false,
//    pagination: true,
//    sortable: true,
//    sortOrder: "asc",
//    sidePagination: "server",
//    pageNumber: 1,
//    pageSize: 10,
//    pageList: [10, 25, 50, 100], 
//    search: false, 
//    strictSearch: true,
//    showColumns: true,                  //是否显示所有的列（选择显示的列）
//    showRefresh: true,                  //是否显示刷新按钮
//    minimumCountColumns: 2,             //最少允许的列数
//    clickToSelect: true,                //是否启用点击选中行
//    //height: 500,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
//    uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
//    showToggle: true,                   //是否显示详细视图和列表视图的切换按钮
//    cardView: false,                    //是否显示详细视图
//    detailView: false,
//    hasoperation: true,
//    columns: []
//};
//function InitMainTable() {
//    //记录页面bootstrap-table全局变量$table，方便应用
//    if ($table.hasoperation) {

//        $table.columns.push({
//            field: 'Operate',
//            title: '操作',
//            width: 120,
//            align: 'center',
//            valign: 'middle',
//            formatter: actionFormatter
//        });
//    }
//    $('#' + $table.ElemtableID).bootstrapTable({
//        url: $table.url,                      //请求后台的URL（*）
//        method: $table.method,                      //请求方式（*）
//        toolbar: $table.toolbar,              //工具按钮用哪个容器
//        striped: $table.striped,                      //是否显示行间隔色
//        cache: $table.cache,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
//        pagination: $table.pagination,                   //是否显示分页（*）
//        sortable: $table.sortable,                     //是否启用排序
//        sortOrder: $table.sortOrder,                   //排序方式
//        sidePagination: $table.sidePagination,           //分页方式：client客户端分页，server服务端分页（*）
//        pageNumber: $table.pageNumber,                      //初始化加载第一页，默认第一页,并记录
//        pageSize: $table.pageSize,                     //每页的记录行数（*）
//        pageList: $table.pageList,        //可供选择的每页的行数（*）
//        search: $table.search,                      //是否显示表格搜索
//        strictSearch: $table.strictSearch,
//        showColumns: $table.showColumns,                  //是否显示所有的列（选择显示的列）
//        showRefresh: $table.showRefresh,                  //是否显示刷新按钮
//        minimumCountColumns: $table.minimumCountColumns,             //最少允许的列数
//        clickToSelect: $table.clickToSelect,                //是否启用点击选中行
//        //height: 500,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
//        uniqueId: $table.uniqueId,                     //每一行的唯一标识，一般为主键列
//        showToggle: $table.showToggle,                   //是否显示详细视图和列表视图的切换按钮
//        cardView: $table.cardView,                    //是否显示详细视图
//        detailView: $table.detailView,                  //是否显示父子表
//        showExport: true, //是否显示导出
//        exportDataType: "basic", //默认basic：只导出当前页的表格数据；all：导出所有数据；selected：导出选中的数据
//        //exportTypes: ['excel'] ,//导出文件类型 ，支持多种类型文件导出
//        //exportOptions: {
//        //    ignoreColumn: [0, 1],  //忽略某一列的索引  
//        //    fileName: '贷款总表',  //文件名称设置  
//        //    worksheetName: 'sheet1',  //表格工作区名称  
//        //    tableName: '贷款总表',
//        //    excelstyles: ['background-color', 'color', 'font-size', 'font-weight', 'border-top']
//        //},
//        showFooter: true,
//        //得到查询的参数
//        queryParams: function (params) {
//            //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
//            var temp = {
//                rows: params.limit,                         //页面大小
//                page: (params.offset / params.limit) + 1,   //页码
//                sort: params.sort,      //排序列名  
//                sortOrder: params.order //排位命令（desc，asc） 
//            };
//            return temp;
//        },
//        columns: $table.columns,
//        onLoadSuccess: function () {
//        },
//        onLoadError: function () {
//            showTips("数据加载失败！");
//        },
//        onDblClickRow: function (row, $element) {
//            var id = row.ID;
//            //EditViewById(id, 'view');
//        },
//    });

//    $('#' + $table.ElemtableID).colResizable({
//        liveDrag: true,
//        gripInnerHtml: "",
//        draggingClass: "dragging",
//        resizeMode: 'overflow'//overflow,flex,fit
//    });
//}

function LibTable(id)
{
    this.$table = {
        ElemtableID: id,
        url: "",
        method: 'GET',
        toolbar: "",
        striped: true,       //显示表格条纹
        cache: false,
        pagination: true,    //启动分页
        sortable: true,
        sortOrder: "asc",
        sidePagination: "server",
        pageNumber: 1,
        pageSize: 10,
        pageList: [10, 25, 50, 100],
        search: false,
        strictSearch: true,
        showColumns: true,                  //是否显示所有的列（选择显示的列）
        showRefresh: true,                  //是否显示刷新按钮
        minimumCountColumns: 2,             //最少允许的列数
        clickToSelect: true,                //是否启用点击选中行
        height: 500,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
        uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
        showToggle: true,                   //是否显示详细视图和列表视图的切换按钮
        cardView: false,                    //是否显示详细视图
        detailView: false,
        hasoperation: true,                 //是否显示操作列
        showFooter: false,                  //表格最底部是否显示汇总行
        showExport: true,                  //是否显示导出按钮
        columns: []
    };
    this.testid = id;
}

LibTable.prototype = {
    constructor: LibTable,
    initialTable: function () {
        if (this.$table.hasoperation) {

            this.$table.columns.push({
                field: 'Operate',
                title: '操作',
                width: 120,
                align: 'center',
                valign: 'middle',
                formatter: actionFormatter
            });
        }
        $('#' + this.$table.ElemtableID).bootstrapTable({
            url: this.$table.url,                      //请求后台的URL（*）
            method: this.$table.method,                      //请求方式（*）
            toolbar: this.$table.toolbar,              //工具按钮用哪个容器
            striped: this.$table.striped,                      //是否显示行间隔色
            cache: this.$table.cache,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: this.$table.pagination,                   //是否显示分页（*）
            sortable: this.$table.sortable,                     //是否启用排序
            sortOrder: this.$table.sortOrder,                   //排序方式
            sidePagination: this.$table.sidePagination,           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: this.$table.pageNumber,                      //初始化加载第一页，默认第一页,并记录
            pageSize: this.$table.pageSize,                     //每页的记录行数（*）
            pageList: this.$table.pageList,        //可供选择的每页的行数（*）
            search: this.$table.search,                      //是否显示表格搜索
            strictSearch: this.$table.strictSearch,
            showColumns: this.$table.showColumns,                  //是否显示所有的列（选择显示的列）
            showRefresh: this.$table.showRefresh,                  //是否显示刷新按钮
            minimumCountColumns: this.$table.minimumCountColumns,             //最少允许的列数
            clickToSelect: this.$table.clickToSelect,                //是否启用点击选中行
            //height: this.$table.height,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: this.$table.uniqueId,                     //每一行的唯一标识，一般为主键列
            showToggle: this.$table.showToggle,                   //是否显示详细视图和列表视图的切换按钮
            cardView: this.$table.cardView,                    //是否显示详细视图
            detailView: this.$table.detailView,                  //是否显示父子表
            showExport: this.$table.showExport, //是否显示导出
            exportDataType: "basic", //默认basic：只导出当前页的表格数据；all：导出所有数据；selected：导出选中的数据
            //exportTypes: ['excel'] ,//导出文件类型 ，支持多种类型文件导出
            //exportOptions: {
            //    ignoreColumn: [0, 1],  //忽略某一列的索引  
            //    fileName: '贷款总表',  //文件名称设置  
            //    worksheetName: 'sheet1',  //表格工作区名称  
            //    tableName: '贷款总表',
            //    excelstyles: ['background-color', 'color', 'font-size', 'font-weight', 'border-top']
            //},
            showFooter: this.$table.showFooter,
            //得到查询的参数
            queryParams: function (params) {
                //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
                var temp = {
                    rows: params.limit,                         //页面大小
                    page: (params.offset / params.limit) + 1,   //页码
                    sort: params.sort,      //排序列名  
                    sortOrder: params.order //排位命令（desc，asc） 
                    //defindfield:"zyy"
                };
                return temp;
            },
            columns: this.$table.columns,
            onLoadSuccess: function () {
            },
            onLoadError: function () {
                showTips("数据加载失败！");
            },
            onDblClickRow: function (row, $element) {
                var id = row.ID;
                //EditViewById(id, 'view');
            },
        });

        $('#' + this.$table.ElemtableID).colResizable({
            liveDrag: true,
            gripInnerHtml: "",
            draggingClass: "dragging",
            resizeMode: 'overflow'//overflow,flex,fit
        });
    }
}

//操作栏的格式化
function actionFormatter(value, row, index) {
    var id = value;
    var result = "";
    result += "<a href='javascript:;' class='btn btn-xs green' onclick=\"EditViewById('" + id + "', view='view')\" title='查看'><span class='glyphicon glyphicon-search'></span></a>";
    result += "<a href='javascript:;' class='btn btn-xs blue' onclick=\"EditViewById('" + id + "')\" title='编辑'><span class='glyphicon glyphicon-pencil'></span></a>";
    result += "<a href='javascript:;' class='btn btn-xs red' onclick=\"DeleteByIds('" + id + "')\" title='删除'><span class='glyphicon glyphicon-remove'></span></a>";

    return result;
}

//var $table;
//var $ElemtableID;
//var $url;
////初始化bootstrap-table的内容
//function InitMainTable() {
//    //记录页面bootstrap-table全局变量$table，方便应用
//    var queryUrl = '/TestUser/FindWithPager?rnd=' + Math.random()
//    $table = $('#' + $ElemtableID).bootstrapTable({
//        url: $url,                      //请求后台的URL（*）
//        method: 'GET',                      //请求方式（*）
//        //toolbar: '#toolbar',              //工具按钮用哪个容器
//        striped: true,                      //是否显示行间隔色
//        cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
//        pagination: true,                   //是否显示分页（*）
//        sortable: true,                     //是否启用排序
//        sortOrder: "asc",                   //排序方式
//        sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
//        pageNumber: 1,                      //初始化加载第一页，默认第一页,并记录
//        pageSize: 10,                     //每页的记录行数（*）
//        pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
//        search: false,                      //是否显示表格搜索
//        strictSearch: true,
//        showColumns: true,                  //是否显示所有的列（选择显示的列）
//        showRefresh: true,                  //是否显示刷新按钮
//        minimumCountColumns: 2,             //最少允许的列数
//        clickToSelect: true,                //是否启用点击选中行
//        //height: 500,                      //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
//        uniqueId: "Name",                     //每一行的唯一标识，一般为主键列
//        showToggle: true,                   //是否显示详细视图和列表视图的切换按钮
//        cardView: false,                    //是否显示详细视图
//        detailView: false,                  //是否显示父子表
//        //得到查询的参数
//        queryParams: function (params) {
//            //这里的键的名字和控制器的变量名必须一致，这边改动，控制器也需要改成一样的
//            var temp = {
//                rows: params.limit,                         //页面大小
//                page: (params.offset / params.limit) + 1,   //页码
//                sort: params.sort,      //排序列名  
//                sortOrder: params.order //排位命令（desc，asc） 
//            };
//            return temp;
//        },
//        columns: [{
//            checkbox: true,
//            visible: true                  //是否显示复选框  
//        }, {
//            field: 'Name',
//            title: '姓名',
//            sortable: true
//        }, {
//            field: 'Mobile',
//            title: '手机',
//            sortable: true
//        }, {
//            field: 'Email',
//            title: '邮箱',
//            sortable: true
//            //formatter: emailFormatter
//        }, {
//            field: 'Homepage',
//            title: '主页'
//            //formatter: linkFormatter
//        }, {
//            field: 'Hobby',
//            title: '兴趣爱好'
//        }, {
//            field: 'Gender',
//            title: '性别',
//            sortable: true
//        }, {
//            field: 'Age',
//            title: '年龄'
//        }, {
//            field: 'BirthDate',
//            title: '出生日期'
//            //formatter: dateFormatter
//        }, {
//            field: 'Height',
//            title: '身高'
//        }, {
//            field: 'Note',
//            title: '备注'
//        }, {
//            field: 'ID',
//            title: '操作',
//            width: 120,
//            align: 'center',
//            valign: 'middle'
//            //formatter: actionFormatter
//        },],
//        onLoadSuccess: function () {
//        },
//        onLoadError: function () {
//            showTips("数据加载失败！");
//        },
//        onDblClickRow: function (row, $element) {
//            var id = row.ID;
//            EditViewById(id, 'view');
//        },
//    });
//};