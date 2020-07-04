<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Jump.aspx.cs" Inherits="QrPaySystem.Pages.Jump" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css"> 
        .align-btn{ position:fixed;left:50%;top:72%;margin-left:-252px;margin-top:-50px; width:504px; height:100px; background-color:transparent; fill-opacity:100}
        
        .align-center{ position:fixed;left:50%;top:50%;margin-left:-360px;margin-top:-640px; width:720px; height:1280px; background-color:silver; fill-opacity:20}
        .back-color { background:#019fe8;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <a href ="https://www.baidu.com" >百度</a>
            <br />
            <br />
            <a href ="https://qr.alipay.com/c1x01990gbhjvuvwaxwkqa3" >支付宝领红包</a>
            <br />
            <br />
            <a href ="https://qr.alipay.com/tsx031041ajtuiviwd978b6" >支付宝收款</a>
            <br />
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="领取红包" OnClick="Button1_Click" />
            <br />
            <br />
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="https://qr.alipay.com/c1x01990gbhjvuvwaxwkqa3">HyperLink_领取红包</asp:HyperLink>
            <br />
            <br />
            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="https://qr.alipay.com/tsx031041ajtuiviwd978b6">HyperLink_支付宝收款</asp:HyperLink>
            <br />
            <br />
            <br />
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" >LinkButton_领取红包</asp:LinkButton>
            <br />
            <br />
            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click"  >LinkButton_支付宝收款</asp:LinkButton>
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <br />
            <br />
            <div id="NewLinkDiv" runat="server"></div>
            <br />
            <br />
            <br />

            <a href="https://www.baidu.com"><img src="https://www.baidu.com/img/baidu_jgylogo3.gif" alt="" /></a>
            <br />
            <br />

    </div>
    </form>
</body>
</html>
