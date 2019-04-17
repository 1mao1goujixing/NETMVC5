$(function () {
    $.ajax({
        type: 'POST',
        url: '/Task/GetMenu',
        data: { syscode: syscode },
        async: false,
        success: function (data) {
            //debugger
            if (data != null && data.length != 0)
            {
                var html = "";
                for (var i = 0; i < data.length; i++)
                {
                    html += '<li><a href="' + data[i].Url + '?syscode=' + syscode + '"><img src="' + data[i].Img + '" />' + data[i].Name + '</a></li>';

                }
                $('#nav').prepend(html);
            } else
            {
                layer.msg("无系统权限，请联系管理员", { icon: 2, time: 3000 });
                window.location.href = HomePage;
            }
        }
    });
    //debugger;
    $("#nav a").each(function (index, item) {
        $(item).removeClass("on");
    });
    $("#nav a[href*='" + url + "']").addClass("on");
})

function Exit() {
    //debugger
    layer.confirm('确认要退出?', { icon: 3, title: '提示' }, function (index) {
        window.location.href = '/Login/Logout';
    })
}
function updatepassword() {    
    var width = 300;
    var height = 500;
    window.open(SSO_URL + '/UpdatePassword/UpdatePassword/Index?UserId=' + UserId + '', 'newwindow', 'position: fixed,height=350, width=400, top=' + width + ', left=' + height + ', toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no');
}

function getMenuCodeBySysCode(menuCode)
{
    var moduleCode = "";
    $.ajax({
        type: 'POST',
        url: '/Task/GetMenu',
        data: { syscode: "018" },
        async: false,
        success: function (dataList) {
            //debugger
            if (dataList !=null && dataList.length != 0)
            {
                $.each(data, function (index, data) {
                    if (data.Url = url)
                    {
                        moduleCode = data.Url;
                    }
                })
            } else
            {
                layer.msg("无权限，请联系管理员", { icon: 2, time: 3000 });
                window.location.href = HomePage;
            }
        }
    });
    return moduleCode;
}