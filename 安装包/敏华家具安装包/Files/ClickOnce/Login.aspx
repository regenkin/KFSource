<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Login.aspx.cs"  Inherits="Kf.WebLogin.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
<title>君飞智慧科技工作室</title>
<style>
.btn{ background:url('images/btnimage.jpg') no-repeat; border:0px;}
</style>
    <script type="text/javascript" src="js/jquery-1.8.0.js"></script>
    <script type="text/javascript" src="layer/layer.js"></script>
    <script language="javascript">
        var runtimeVersion = '2.0.0';
        var runtimeVersion4 = '4.0';
        var directLink = "Kf.WinStart.application";
        var timeID;
        function HasRuntimeVersion(v)
        {
            var va = GetVersion(v);
            var i;
            var n4 = navigator.userAgent.match(/\.NET[\.0-9]+/gi);
            if (n4 != null)
                for (i = 0; i < n4.length; ++i)
                    if (CompareVersions(va, GetVersion(n4[i])) <= 0)
                        return true;
            var a = navigator.userAgent.match(/\.NET CLR [0-9.]+/g);
            if (a != null)
                for (i = 0; i < a.length; ++i)
                    if (CompareVersions(va, GetVersion(a[i])) <= 0)
                        return true;
            if (va != null)
                for (i = 0; i < va.length; ++i)
                    if (va[i]==4)   return true;
            return false;
        }

        function GetVersion(v)
        {
            var a = v.match(/([0-9]+)\.([0-9]+)\.([0-9]+)/i);
            if (a != null)
            {
                return a.slice(1);
            }
            else
            {
                a = v.match(/([0-9]+)\.([0-9]+)/i);
                return a.slice(1);
            }
        }
        function CompareVersions(v1, v2)
        {
            for (i = 0; i < v1.length; ++i)
            {
                var n1 = new Number(v1[i]);
                var n2 = new Number(v2[i]);
                if (n1 < n2)
                    return -1;
                if (n1 > n2)
                    return 1;
            }
            return 0;
        }

        function getCPU()
        {
            var agent = navigator.userAgent.toLowerCase();
            if (agent.indexOf("win64") >= 0 || agent.indexOf("wow64") >= 0) return "x64";
            return navigator.cpuClass;
        }

        function load()
        {
            var userName = getCookie('UserName');
            if (userName && document.getElementById('UserName').value == '')
            {
                document.getElementById('UserName').value = userName;

                var db = getCookie('CorpNum');
                if (db)
                {
                    document.getElementById('CorpNum').value = db;
                }
            }
            document.all('HiddenField1').value = window.screen.availHeight;
            document.getElementById('Password').focus();
            var p = window;
            var p2 = p.parent;
            while (p2 != p)
            {
                p = p2;
                p2 = p.parent;
            }
            if (p2 != window)
            {
                p2.location.href = 'login.aspx?TimeOut=true';
            }
        }

        function ResetButton_Click()
        {
            document.getElementById('UserName').value = '';
            document.getElementById('UserName').focus();
            document.getElementById('Password').value = '';
            document.getElementById('progressIndicatorTemplate').style.display = 'none';
            document.getElementById('pi_image').style.visibility = 'hidden';
        }

        function showprogpress()
        {
            document.getElementById('progressIndicatorTemplate').style.display = 'block';
            document.getElementById('pi_image').style.visibility = 'visible';
        }

        function closeself()
        {
            window.opener = null;
            window.open('', '_self');
            window.close();
        }

        var clientupdatecheck = '0';
        var firstruncheck = '0';
        var clientruncheck = '0';

        function checkClientIsStart()
        {
            var navstop = getCookie('navstop');
            if (navstop)
            {
                window.clearInterval(timeID);
                window.setTimeout('closeself()', 1);
            }
            if (firstruncheck == '0')
            {
                var firstrun = getCookie('firstrun');
                if (firstrun)
                {
                    firstruncheck = '1';
                    document.getElementById('showProgress').innerText = '正在为第一次运行准备环境...';
                }
            }
            if (clientruncheck == '0')
            {
                var clientrun = getCookie('clientrun');
                if (clientrun)
                {
                    clientruncheck = '1';
                    document.getElementById('showProgress').innerText = '正在登录系统...';
                }
            }
        }
        function setCookie(name, value)
        {
            var Days = 30;
            var exp = new Date();
            exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
            document.cookie = name + "=" + value + ";expires=" + exp.toGMTString();
        }
        //读取cookies
        function getCookie(name)
        {
            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg)) return decodeURI(unescape(arr[2]));
            else return null;
        }
        function setcontroldisabled()
        {
            document.getElementById('LoginButton').disabled = 'disabled';
            document.getElementById('ResetButton').disabled = 'disabled';
            document.getElementById('CorpNum').disabled = 'disabled';
            document.getElementById('UserName').disabled = 'disabled';
            document.getElementById('Password').disabled = 'disabled';
        }
        function setup()
        {
            if (document.getElementById('IsFirstLogin').value == "false")
            {
                document.getElementById('showProgress').innerText = '正在安装.net环境...'
                document.getElementById('LoginButton').click();
                window.setTimeout('setcontroldisabled()', 3);
            }
        }

        window.onload = function () {
            var cpuver = getCPU();
            if (cpuver == 'x64') {
                document.getElementById('CPU64Version').value = true;
            }
            else {
                document.getElementById('CPU64Version').value = false
            }
            if (HasRuntimeVersion(runtimeVersion4)) {
                document.getElementById('HasRuntimeVersion4').value = true;
                document.getElementById('HasRuntimeVersion').value = true;
                document.body.style.cursor = "default";
            }
            else if (HasRuntimeVersion(runtimeVersion)) {
                document.getElementById('HasRuntimeVersion').value = true;
                document.getElementById('HasRuntimeVersion4').value = false;
                document.body.style.cursor = "default";
            }
            else {
                document.getElementById('HasRuntimeVersion').value = false;
                document.getElementById('HasRuntimeVersion4').value = false;
                if (document.getElementById('IsFirstLogin').value == "false") {
                    window.setTimeout('showprogpress()', 1);
                    timeID = window.setInterval(checkClientIsStart, 3);
                    setup();
                }
                else {
                    document.body.style.cursor = "default";
                }
            }
            load();
        }

        function form1_OnSubmit() {
            document.getElementById("errMsg").innerHTML = "";
            var CorpNum = document.getElementById('CorpNum').value;
            var UserName = document.getElementById('UserName').value;
            if (CorpNum == "") { alert("企业代码不能为空");return false;}
            if (UserName == "") {alert("登陆名不能为空"); return false;}
            var Days = 30;
            var exp = new Date();
            exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
            setCookie('UserName', escape(UserName));
            setCookie('CorpNum', escape(CorpNum));
            window.setTimeout('showprogpress()', 1);
            document.body.style.cursor = "wait";
            timeID = window.setInterval(checkClientIsStart, 3);
            window.setTimeout('setcontroldisabled()', 3);
            return true;
        }

        function defaultLoginInfo(type) 
        {
            if (type == 1) {
                document.getElementById('CorpNum').value = "SCM";
                document.getElementById('UserName').value = "test";
                document.getElementById('Password').value = "test";
            }
            else if (type == 2) {
                document.getElementById('CorpNum').value = "GL";
                document.getElementById('UserName').value = "test";
                document.getElementById('Password').value = "test";
            }
            else if (type == 3) {
                document.getElementById('CorpNum').value = "Demo";
                document.getElementById('UserName').value = "test";
                document.getElementById('Password').value = "test";
            }
        }
  </script>
  <script>
      function checkBrowser() {
          var agent = navigator.userAgent.toLowerCase();
          var isIe = /(msie\s|trident.*rv:)([\w.]+)/.test(agent);
          var isEdge = agent.indexOf('edge') > 0;
          if (!isEdge && !isIe) {
              document.getElementById("bsWarning").style.display = "";
              setcontroldisabled();
          }
      }

      var layerIndex = null;
      function showLayer() {
          layerIndex = layer.open({
              type: 1,
              title: '登录提示信息',
              area: ['593px', '220px'],
              skin: 'layui-layer-demo', //样式类名
              closeBtn: 0, //不显示关闭按钮
              shift: 2,
              shadeClose: false, //开启遮罩关闭
              content: '<div style="margin-left:80px;margin-top:20px;font-size:16px">'
             + '<span style="float:left;margin-right:25px"> <img src="images/working.gif"/></span>'
             + '<div style="float:left;width:360px;margin-top:-10px;color:#666;">'
             + '<p>登录完成前，请不要关闭此登录确认窗口。</p><p>登录完成后，请根据您登录的情况点击下面按钮。</p></div></div>'
             + '<div class="clearfix"></div><div style="margin-left:180px;margin-top:110px">'
             + '<a onclick="javascript:PayFailed();" style="text-decoration:none;display:inline-block;width:100px;height37px;background:#EDEBEB;line-height:37px;color:#000;text-align:center;font-size:14px;margin-right:20px;Cursor:Pointer">无法登录</a>'
             + '<a onclick="javascript:PaySuccess();" style="text-decoration;none;display:inline-block;width:100px;height37px;background:#FF8000;line-height:37px;color:#fff;text-align:center;font-size:14px;Cursor:Pointer">登录成功</a>'
             + '</div>'
          });
      }

      function PayFailed() {
          setCookie("Processing", null);
          layer.close(layerIndex);
          layer.alert("很抱歉给您带来的不便，请咨询普方客户提供技术支持");
      }

      function PaySuccess() {
          setCookie("Processing", null);
          layer.close(layerIndex);
          window.opener = null;
          window.open('', '_self');
          window.close();
      }
  </script>
</head>
<body bgcolor="Lavender" scroll="no">
<!-- 非IE警告提醒 -->
<style type="text/css">
    #bsWarning a, #bsWarning a:hover, #bsWarning a:visited
    {
        color:#28a3ef;
        text-decoration:none;
    }
    #bsWarning a:hover
    {
        color:#28a3ef;
        text-decoration:none;
    }
</style>
<div id="bsWarning" style="display:none;position:fixed;top:0px;left:0px;width:100%;height:53px;background-color:#FFFEEE;border-bottom:1px solid #ddd;padding-top:8px">
 <p style="width:1024px;font-size:13px;margin:0 auto;"><img style="width:24px;height:24px;vertical-align:middle;margin-right:3px;margin-top:-5px" src="images/warning.png"/><span style="color: red;">温馨提示：</span>
  <span style="color:red">发现当前浏览器不是IE兼容模式运行，将导致服务无法正常使用！建议您使用IE浏览器或把浏览器切换到IE兼容模式。</span>
   <br/>
  <span style="color:#333;margin-left:28px">1.支持但需要切换到兼容模式运行的浏览器包括
       <a target="_blank" href="https://www.baidu.com/s?ie=utf-8&f=3&rsv_bp=0&rsv_idx=1&tn=baidu&wd=360%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0%E5%85%BC%E5%AE%B9%E6%A8%A1%E5%BC%8F&rsv_pq=f16de30600002830&rsv_t=3a1cGkJWmTLmAnwDrp4SFxPvqhA7%2FTNcFPpF1zkJ1kM3rwvi3PQ%2BEF69Jgc&rqlang=cn&rsv_enter=1&rsv_sug3=10&rsv_sug1=9&rsv_sug7=100&rsv_sug2=0&prefixsug=%3C60%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0&rsp=1&inputT=9434&rsv_sug4=10071">360浏览器</a>
       <a target="_blank" href="https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=QQ%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0%E5%85%BC%E5%AE%B9%E6%A8%A1%E5%BC%8F&oq=QQ%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0%E5%85%BC%E5%AE%B9%E6%A8%A1%E5%BC%8F&rsv_pq=dc5b359a00001cfc&rsv_t=bd91SDWFT192HvMdQ7Zz0OZWF9Iqajptj72rLhJoVU99pxtK2vRoiAVfDkY&rqlang=cn&rsv_enter=0">QQ浏览器</a>、
       <a target="_blank" href="https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=%E7%8C%8E%E8%B1%B9%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0%E5%85%BC%E5%AE%B9%E6%A8%A1%E5%BC%8F&oq=QQ%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0%E5%85%BC%E5%AE%B9%E6%A8%A1%E5%BC%8F&rsv_pq=900defc700002187&rsv_t=b8aaKMPX3xGZpCLET93vGhxSPYrZolCD2YesleeD1N2qYhSfaiFIjIdNB80&rqlang=cn&rsv_enter=0&rsv_sug3=19&rsv_sug1=11&rsv_sug7=100&rsv_sug2=0&inputT=6180&rsv_sug4=6180">猎豹浏览器</a>、
       <a target="_blank" href="https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=%E6%90%9C%E7%8B%97%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0%E5%85%BC%E5%AE%B9%E6%A8%A1%E5%BC%8F&oq=%E7%8C%8E%E8%B1%B9%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0%E5%85%BC%E5%AE%B9%E6%A8%A1%E5%BC%8F&rsv_pq=9be715e100006801&rsv_t=b8aaKMPX3xGZpCLET93vGhxSPYrZolCD2YesleeD1N2qYhSfaiFIjIdNB80&rqlang=cn&rsv_enter=0&inputT=1326&rsv_sug3=21&rsv_sug1=13&rsv_sug7=100&rsv_sug2=0&rsv_sug4=1326">搜狗浏览器</a>、
       <a target="_blank" href="https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=%E7%99%BE%E5%BA%A6%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0%E5%85%BC%E5%AE%B9%E6%A8%A1%E5%BC%8F&oq=%E6%90%9C%E7%8B%97%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0%E5%85%BC%E5%AE%B9%E6%A8%A1%E5%BC%8F&rsv_pq=a441f2b10000704a&rsv_t=3f3cHgt6tjcfDKhVMqwSkWrFVxrGbXBo1ZLPS1eZ7SHPYH0Ucd4ftafjEwc&rqlang=cn&rsv_enter=0&inputT=1454&rsv_sug3=24&rsv_sug1=16&rsv_sug7=100&rsv_sug2=0&rsv_sug4=3973">百度浏览器</a>、
       <a target="_blank" href="https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=UC%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0%E5%85%BC%E5%AE%B9%E6%A8%A1%E5%BC%8F&oq=%E7%99%BE%E5%BA%A6%E6%B5%8F%E8%A7%88%E5%99%A8%E5%88%87%E6%8D%A2%E5%88%B0%E5%85%BC%E5%AE%B9%E6%A8%A1%E5%BC%8F&rsv_pq=e95e05f400008b06&rsv_t=4eb4%2FstUTq315AQ6%2Fse%2F5dIQ0r9Mvn1w%2Frp98ZDCEquNHpJJhmwqRwKALEA&rqlang=cn&rsv_enter=0&inputT=1115&rsv_sug3=27&rsv_sug1=17&rsv_sug7=100&rsv_sug2=0&rsv_sug4=1494&rsv_sug=1">UC浏览器</a>。</span>
    <br/>
  <span style="color:#333;margin-left:28px">2.不支持的浏览器包括<span style="color:#6c6c6c">遨游浏览器</span>、<span style="color:#6c6c6c">谷歌浏览器</span>、<span style="color:#6c6c6c">火狐浏览器</span>、<span style="color:#6c6c6c">欧朋浏览器</span>、<span style="color:#6c6c6c">苹果浏览器</span>。</span>
  </p>
</div>
<form method="post" action="login.aspx" onsubmit="javascript:return form1_OnSubmit();" id="form1">
<%--<div style="width: 360px;margin:auto; font-size:12px;"><a href="javascript:defaultLoginInfo(1)" style="cursor:pointer;margin:0px 20px 0px 20px;color:blue">供应链模块演示</a>|<a href="javascript:defaultLoginInfo(2)" style="cursor:pointer;margin:0px 20px 0px 20px;color:blue">财务模块演示</a>|<a href="javascript:defaultLoginInfo(3)" style="cursor:pointer;margin:0px 20px 0px 20px;color:blue">标准版演示</a></div>--%>
<div style="height: 400px; width: 600px; background-image: url(Images/login.png);background-repeat: no-repeat; margin:45px auto;">
    <div class="clear" style="height:140px;"></div>
    <div style="width:360px;height:230px;border:0px solid red; margin:auto;">
        <div class="clear" style="height:18px;text-align:center; font-size:12px;"><span style="color:Red;" id="errMsg"><%=errMsg%></span></div>
        <div style="height:40px; margin-top:10px;">
            <label for="CorpNum" id="DBLabel" style="display:inline-block; width:80px;text-align:right;margin-left:20px; font-size:12px;">企业号：</label>
            <input name="CorpNum" type="text" id="CorpNum" style="width:220px;height:26px; border:1px solid #b7d4ff;line-height:26px;"/>
        </div>
        <div style="height:40px;">
            <label for="UserName" id="UserNameLabel" style="display:inline-block; width:80px;text-align:right;margin-left:20px; font-size:12px;">用户名：</label>
            <input name="UserName" type="text" id="UserName" style="width:220px;height:26px; border:1px solid #b7d4ff;line-height:26px;"/>
        </div>
        <div style="height:40px;">
            <label for="Password" id="PasswordLabel" style="display:inline-block;width:80px;text-align:right;margin-left:20px; font-size:12px;">密码：</label>
            <input name="Password" type="password" id="Password" style="width:220px;height:26px; border:1px solid #b7d4ff;line-height:26px;"/>
        </div>
        <div  style="height:40px; text-align:right; margin-right:40px;">
            <input type="submit" name="LoginButton" value="登录" id="LoginButton" class="btn" style="height:27px;width:60px;"/>
            <input type="reset" name="ResetButton" value="重置" id="ResetButton" class="btn" style="height:27px;width:60px;"/>                                                
        </div>
        <div id="progressIndicatorTemplate" style="display:none;width:360px;margin:auto;text-align:center;">
            <div><img src="images/loading.gif" id="pi_image" style="visibility:hidden"; alt="加载中" /></div>
            <div><span id="showProgress" style="font-size:12px;color:White;">正在登录中 ...</span></div>
        </div>
    </div>
</div>
<div style="width: 600px;margin:auto; font-size:12px;">
<!--<marquee width="500px" scrolldelay="250" behavior="alternate">-->
<p>1、请使用IE内核浏览器登录系统,建议将本站点设为信任站点</p>
<p>2、如果不能正常登录，请禁用迅雷下载等浏览器加载项</p>
<p>3、如果不能正常登录，下载<a href="http://dl1sw.baidu.com/soft/9b/15910/Microsoft.NET.exe?version=585709662">微软.net4.0框架</a>运行环境</p>
<p>4、下载<a href="clientBin/云端桌面版.rar">云端桌面版</a>快速部署运行环境</p>
<!--</marquee>-->
</div>
<input type="hidden" name="HasRuntimeVersion" id="HasRuntimeVersion" />
<input type="hidden" name="HasRuntimeVersion4" id="HasRuntimeVersion4" />
<input type="hidden" name="CPU64Version" id="CPU64Version" />
<input type="hidden" name="IsFirstLogin" id="IsFirstLogin" value="<%=IsFirstLogin%>"/>
<input type="hidden" name="HiddenField1" id="HiddenField1"/>
<input type="hidden" name="Setup" id="Setup"/>
<input type="hidden" name="opt" id="opt" value="login" />
</form>
<script type="text/javascript">
    checkBrowser();
</script>
</body>
</html>