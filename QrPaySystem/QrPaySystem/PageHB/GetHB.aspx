<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetHB.aspx.cs" Inherits="QrPaySystem.PageHB.GetHB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=0.5, minimum-scale=0.5, maximum-scale=1.0, user-scalable=no" />
    <title>保存图像，领取红包</title>

    <style type="text/css">
        .MainPannelStyle 
        {
	        width:720px;
	        background-color: #FFFFFF;
	        background-position: center top;
	        height: 1280px;
	        text-align: center;
	        margin-left: auto;
	        margin-right: auto;
	        border: 1px;
        }
        .STYLE1 {
	        font-size: 36px;
	        font-weight: bold;
	        color: #04a4ec;
	        margin-top: 10px;
	        margin-bottom: 0px;
        }
    </style>

</head>
<body style="background-color:gray; margin-top:0px; margin-bottom:0px; margin-left:0px">
    <div class="MainPannelStyle" id="MainPannel" runat="server" >

        <%--<form id="form1" runat="server">
        </form>--%>
        <br />
        <div id="DivPic" runat="server" align="center">
            <%--<img src="http://60.205.185.168:8001/pages/pic.aspx?path=http://60.205.185.168:8002/Uploads/HB/1534555451879.png&reName=#0.jpg" width="493" height="742"/> --%>
            <img src="../tools/HB_pic/example_hb.png" width="493" height="742"/> 
        </div>
        <div align="center" class="STYLE1"> 1、长按红包 -&gt; 保存图像</div>
        <br />
        <div id="DivDownload" runat="server" Font-Size="24px" >
            <a id="HelpLinkHB" target="_blank" href="../pages/pic.aspx?path=http://t2.hddhhn.com/uploads/tu/201707/115/56.jpg&reName=000.jpg&download=true"> 或 用浏览器下载</a>
        </div>

        <br /><br /><br /><br /><br />

        <div id="DivScan" runat="server" >
            <a href="http://www.baidu.com"><img src="../tools/HB_pic/scan_album.png" Height="84px" Width="538px"/></a>        
        </div>
    </div>
</body>
</html>
