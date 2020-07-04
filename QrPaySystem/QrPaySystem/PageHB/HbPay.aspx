<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HbPay.aspx.cs" Inherits="QrPaySystem.PageHB.HbPay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
<body class="MainDivStyle">

    <div class="TittleDivStyle">
        <p style="font-size:40px; color:white">
          <asp:Label ID="LabelTittle" runat="server" Text="商铺名称（自定义）"></asp:Label>
        </p>  
    </div>
    
    <div id="BtnDiv" runat="server" class="BtnDivStyle">
        <a href="http://www.baidu.com"><img src="../tools/HB_pic/btn.png" /></a>        
    </div>

    <div id="LinkDiv" class="LinkDivStyle">
        <a id="LinkA" target="_blank" href="HB.aspx"><asp:Label ID="LabelLink" runat="server" Text="制作我的红包收款码" ForeColor="White"></asp:Label></a>
    </div>

</body>
</html>
