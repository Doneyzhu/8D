/*
JQuery异步加载无限极下拉框级联功能
zjy
*/
(function ($) {
    $.ajaxSetup({ async: false });
    var url = "";
    var parameter = "";
    $.fn.extend({
        Load: function (urlPath) {
            url = urlPath.url;
            parameter = urlPath.parameter;
            $("#ddl1").append("<option value='0'selected='selected'>请选择</option>");
            $.getJSON(url, parameter, function (data) {
                $.each(data.rows, function (i, row) {
                    $("#ddl1").append($("<option></option>").val(row.id).html(row.text));
                });
                $("#ddl1").change(function () { $(this).Select($(this).val(), this); });
            });
            $(this).Selected(parameter.parentId, $("#ddl1"));
        },

        Select: function (parentId, obj) {
            //debugger;
            if (parentId == "0" || parentId == null) {
                return;
            }
            parameter.parentId = parentId;
            $.getJSON(url, parameter, function(data) {
                $(obj).nextAll(".ddl").remove();
                if (data != null && data.rows.length != 0) {
                    // alert(data.rows.length)
                    $("<select>", {
                        "class": "ddl",
                        change: function() {
                            $(this).Select($(this).val(), this);
                        }
                    }).appendTo($("#cascade"));

                    $($(".ddl")[$(".ddl").length - 1]).append("<option value='0' selected='selected'>请选择</option>");
                    $.each(data.rows, function(i, row) {
                        $($(".ddl")[$(".ddl").length - 1]).append($("<option></option>").val(row.id).html(row.text));
                    });
                }
            });
            $(this).Selected(parentId, $(obj).nextAll(".ddl"));
        },

        Selected: function (parentId, obj) {
            $(this).GetValue();
            //debugger;
            var selected = "0," + $("#loadselect").val();
            $.each(selected.split(","), function (i, row) {
                if (row == parentId) {
                    //debugger;
                    $(obj).val(selected.split(",")[i + 1]);
                    $(obj).change();
                }
            });
        },

        GetValue: function () {
            var ddlValue;
            var ddlCount = $(".ddl").length;
            for (var i = ddlCount - 1; i >= 0; i--) {
                if (i != 0) {
                    if ($($(".ddl")[i]).val() != 0) {
                        ddlValue = $($(".ddl")[i]).val();
                        break;
                    }
                } else {
                    if ($($(".ddl")[i]).val() == 0) {
                        ddlValue = 0;
                        break;
                    } else {
                        ddlValue = $($(".ddl")[i]).val();
                        break;
                    }
                }
            }
            $("#selectvalue").val(ddlValue);
        },
    });
})(jQuery);
