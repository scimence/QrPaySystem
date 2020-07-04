<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductPay.aspx.cs" Inherits="QrPaySystem.Pages.ProductPay" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>付费资源</title>
    <link rel="icon" href="~/tools/Product_pic/favicon.ico" type="image/x-icon" />
    <style type="text/css">

        .MainDivStyle{
	        width:720px;
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 0;
	        margin-right: auto;
	        margin-left: auto;
	        background-image: url(../tools/Product_pic/bg.jpg);
	        float: none;
	        background-repeat: no-repeat;
	        background-position: top;
	        background-color: #019FE8;
        }
        .BtnDivStyle{
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 250px;
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

        .PriceDivStyle{
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 60px;
	        margin-right: auto;
	        margin-bottom: 0;
	        margin-left: auto;
        }


        /*.LinkDivStyle{
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 30px;
	        margin-right: auto;
	        margin-bottom: 0;
	        margin-left: auto;
        }*/

        
        .TipDivStyle{
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 20px;
	        margin-right: auto;
	        margin-bottom: 0;
	        margin-left: auto;
            font-size:20px
        }


        .LinkDivStyle{
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 120px;
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
          <asp:Label ID="LabelTittle" runat="server" Text="测试资源x1"></asp:Label>
        </p>  
    </div>

    
    <div class="PriceDivStyle">
        <p style="font-size:40px; color:white">
          <asp:Label ID="LabelPrice" runat="server" Text="待支付金额：0.01元"></asp:Label>
        </p>  
    </div>
    
    <div id="BtnDiv" runat="server" class="BtnDivStyle">
        <a href="http://www.baidu.com"><img src="../tools/Product_pic/btn_pay.png" /></a>        
    </div>

    <div id="TipDiv" class="TipDivStyle">
        <asp:Label ID="LabelTip" runat="server" Text="提示：支付后未显示，请再次扫码" ForeColor="White"></asp:Label>
    </div>

    <div id="LinkDiv" class="LinkDivStyle">
        <a id="LinkA" target="_blank" href="Product.aspx"><asp:Label ID="LabelLink" runat="server" Text="制作我的，付费资源二维码" ForeColor="White"></asp:Label></a>
    </div>

</body>
</html>