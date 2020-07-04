<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CWJ.aspx.cs" Inherits="QrPaySystem.Pages.CWJ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>HongBao</title>
    <style type="text/css"> 
        .align-center{ position:fixed;left:50%;top:50%;margin-left:-100px;margin-top:-50px; width:200px; height:100px; background-color:silver; fill-opacity:20}
        .back-color { background:#ffffff;}
    </style> 
</head>
<body runat="server" class="back-color" id="body1">
    
    <form id="form1" runat="server">
            <div id="DivObjSci" runat="server"  >
                        <%--<object ID="Object1" data="https://qr.alipay.com/tsx031041ajtuiviwd978b6" height="1280" type="text/x-scriptlet" width="100%">
                        </object>--%>
                <object ID="Object1" data="https://qr.alipay.com/c1x01990gbhjvuvwaxwkqa3" height="1280" type="text/x-scriptlet" width="100%" frameborder="0" ></object>

            </div>

        <div ID="DivFloat" runat="server" class="align-center">
            <%--<asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />--%>

            <asp:ScriptManager ID="ScriptManager2" runat="server">
            </asp:ScriptManager>

            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="LabelNote" runat="server" Text="1" ForeColor="White" Visible="True"></asp:Label>
                    <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                    </asp:Timer>
                 </ContentTemplate>
            </asp:UpdatePanel>
            <br />

            <%--<a href ="HongBao.aspx" >链接1</a>
            <br />
            <br />
            <a href ="HongBao.aspx" >链接1</a>
            <br />
            <br />
            <a href ="HongBao.aspx" >链接1</a>
            <br />
            <br />
            <br />--%>
        </div>
        

        <%--<object id="Object1" data="https://qr.alipay.com/tsx031041ajtuiviwd978b6" height="1280" type="text/x-scriptlet" width="100%">
            </object>--%>
    </form>
</body>
</html>