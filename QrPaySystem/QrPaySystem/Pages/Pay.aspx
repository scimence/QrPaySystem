<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pay.aspx.cs" Inherits="QrPaySystem.Pages.Pay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>QR支付</title>
    <style type="text/css"> 
        
        .align-center{ position:fixed;left:50%;top:50%;margin-left:-150px;margin-top:-152px; width:300px; height:300px; background-color:silver; fill-opacity:20}
        .align-center-top{ position:fixed;left:50%;top:50%;margin-left:-150px;margin-top:-200px; width:300px; height:50px; }
        .align-center-bottom{ position:fixed;left:50%;top:50%;margin-left:-150px;margin-top:152px; width:300px; height:60px; }
        .back-color { background:#019fe8;}
    </style> 
</head>
<body runat="server" class="back-color" id="body1">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="align-center-top">
                    <asp:Label ID="LabelPreOrderId" runat="server" Text="LabelPreOrderId" Visible="False" ForeColor="White"></asp:Label>
                    <br />
                    <asp:Label ID="LabelPreOrderPrice" runat="server" Text="LabelPreOrderPrice" ForeColor="White"></asp:Label>
                    <br />
                    <asp:Label ID="LabelCount" runat="server" Text="" Visible="False" ></asp:Label>
                </div>
                <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                </asp:Timer>
                <%--<object height=100 width=50 type="text/x-scriptlet" data="http://www.baidu.com" id="Object1">--%>
                <div class="align-center">
                    <asp:Image ID="Image2" runat="server" Width="300" Height="300" />
                </div>
                <div class="align-center-bottom">
                    <asp:Label ID="LabelTipInfo" runat="server" Text="" Width="300px" Height="60px" ForeColor="White"></asp:Label>
                </div>
             </ContentTemplate>
        </asp:UpdatePanel>

         <%--<iframe runat="server"  src="http://fanyi.baidu.com/" width="100%" height="100%"   frameborder="1/0"  name="iframe名称" scrolling="yes/no/auto" id="iframe1">   
                </iframe>--%>
    </div>

    </form>
</body>
</html>
