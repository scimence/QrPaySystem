<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QrPaySystem.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Styles/stylesSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div style="font-family: 宋体, Arial, Helvetica, sans-serif; background-color: #4B6C9E; margin-right: auto; margin-left: auto; width: auto; height: 50px; text-align: center; padding-top: 10px;">
        <asp:Label ID="LabelTittleName" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="楷体" Font-Size="30pt" Font-Strikeout="False" ForeColor="White" Text="QrPay支付系统"></asp:Label>
    </div>
    <div style="width: 100px; margin-left: auto;">
        <asp:LinkButton ID="LinkButton1" runat="server" ForeColor="White">LinkButton</asp:LinkButton>
    </div>
    <div class="hideSkiplink">
        <asp:Menu ID="Menu1" runat="server" CssClass="menu" Orientation="Horizontal" IncludeStyleBlock="False" EnableViewState="False">
            <Items>
                <asp:MenuItem Text="简介" Value="简介"></asp:MenuItem>
                <asp:MenuItem Text="接入支付" Value="接入支付">
                    <asp:MenuItem Text="用于电脑软件（*.exe）" Value="用于电脑软件（*.exe）"></asp:MenuItem>
                    <asp:MenuItem Text="用于手机应用（*.apk）" Value="用于手机应用（*.apk）"></asp:MenuItem>
                    <asp:MenuItem Text="用于网页支付" Value="用于网页支付"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="个人中心" Value="个人中心">
                    <asp:MenuItem Text="个人信息" Value="个人信息"></asp:MenuItem>
                    <asp:MenuItem Text="支付管理" Value="支付管理"></asp:MenuItem>
                </asp:MenuItem>
            </Items>
        </asp:Menu>

    </div>
    <div style="BottomDivStyle; margin-right: auto; margin-left: auto; width: auto; height: 30px; background-color: #465C71;"></div>
</body>
</html>
