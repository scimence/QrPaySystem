<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Browser.aspx.cs" Inherits="QrPaySystem.Pages.Browser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>输入网址，打开网页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="TextBox_url" runat="server" Width="90%" Font-Names="宋体" Font-Size="X-Large" ></asp:TextBox> 
        <asp:Button ID="Button_jump" runat="server" Text="打开" Font-Names="宋体" Font-Size="X-Large" Width="8%" OnClick="Button_jump_Click" />
    </div>
    <div id="DivContent" runat="server">
        <%--<object width="100%" height="1280" type="text/x-scriptlet" data="http://www.baidu.com" id="Object1">--%>
    </div>
    </form>
</body>
</html>
