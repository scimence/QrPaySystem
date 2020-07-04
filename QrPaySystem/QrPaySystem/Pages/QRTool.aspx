<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QRTool.aspx.cs" Inherits="QrPaySystem.Pages.QRTool" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>二维码在线生成</title>

    <style type="text/css"> 
        .align-center{ position:fixed;left:50%;top:50%;margin-left:-200px;margin-top:-200px; width:400px; height:400px; background-color:silver; fill-opacity:20}
        .back-color { background:#019fe8;}
    </style> 
</head>

<body class="back-color">
    <p style="font-size:16px; color:white">
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    </p>  

    <div class="align-center">
        <asp:Image ID="Image1" runat="server" Width="400" Height="400" />
    </div>
</body>
</html>