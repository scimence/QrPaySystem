<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HbPay3.aspx.cs" Inherits="QrPaySystem.PageHB.HbPay3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>商户红包支付</title>
    <link rel="icon" href="~/tools/HB_pic/favicon.ico" type="image/x-icon" />
    <style type="text/css">

        .MainDivStyle{
	width:720px;
	text-align:center;
	border:0px solid #F00;
	margin-top: 0;
	margin-right: auto;
	margin-left: auto;
	background-image: url(../tools/HB_pic/bg.jpg);
	float: none;
	background-repeat: no-repeat;
	background-position: top;
	background-color: #019FE8;
        }
        .BtnDivStyle{
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 350px;
	        margin-right: auto;
	        margin-bottom: 0;
	        margin-left: auto;
        }
        .TittleDivStyle{
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 440px;
	        margin-right: auto;
	        margin-bottom: 0;
	        margin-left: auto;
        }
        .LinkDivStyle{
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 30px;
	        margin-right: auto;
	        margin-bottom: 0;
	        margin-left: auto;
        }

        .LinkDivStyle{
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 160px;
	        margin-right: auto;
	        margin-bottom: 0;
	        margin-left: auto;
            font-size:30px
        }
    </style>
</head>
<body id="Body1" runat="server" class="MainDivStyle">

    <div class="TittleDivStyle">
        <p style="font-size:40px; color:white">
          <asp:Label ID="LabelTittle" runat="server" Text="商铺名称（自定义）"></asp:Label>
        </p>  
    </div>
    
    <div id="BtnDiv" runat="server" class="BtnDivStyle">
        <a id="A1" href="http://www.baidu.com" onclick="reloadPage()" ><img src="../tools/HB_pic/btn.png" /></a>    
    </div>

    <div id="LinkDiv" runat="server" class="LinkDivStyle">
        <a id="LinkA" target="_blank" href="HB.aspx"><asp:Label ID="LabelLink" runat="server" Text="制作我的红包收款码" ForeColor="White"></asp:Label></a>
    </div>

    <%--<form runat="server" >
        <asp:Button ID="Button1" runat="server" Text="Button" onclick="document.getElementById('BtnName1').click();" />
    </form>--%>
    
</body>

<script type="text/javascript">

    var el = document.getElementById('BtnName1');
    //el.target = '_new'; //指定在新窗口打开
    el.click();//触发打开事件
    //el.tagName = 'BtnName0';

    // 点击链接后刷新页面
    function reloadPage()
    {
        //document.write("0.5s后刷新");
        var t = setTimeout(function () { window.location.reload(); }, 500);
    }

    // 点击链接后关闭页面
    function resetPage() {
        //document.write("0.5s后刷新");
        var t2 = setTimeout(function () { window.location.href = "alipays://" }, 2000);
    }
</script>

</html>
