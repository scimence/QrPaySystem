<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="regsuccess.aspx.cs" Inherits="QrPaySystem.boc.regsuccess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=0.5, minimum-scale=0.5, maximum-scale=0.5, user-scalable=no" />
    <title></title>
    <style type="text/css">
        <!--
        .MainDiv {
            width: 360px;
            margin-right: auto;
            margin-left: auto;
            margin-top: 20px;
            margin-bottom: 20px;
            border: 0px solid #999999;
        }

        .STYLE1 {
            font-size: x-large;
        }

        .STYLE2 {
            color: #3399CC;
        }
        -->
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="Div1" class="MainDiv" runat="server">
            <div id="DivInfo" runat="server">
                <asp:Label ID="Label_TipInfo" runat="server" Text="提示信息:此页面仅用于演示流程" ForeColor="#CC0000"></asp:Label>
            </div>

            <div id="DivPanel" runat="server">
                <div align="center">
                    <table style="width: 100%;">
                        
                        <tr>
                            <td>
                                <table width="356" border="0" cellspacing="0" cellpadding="1">
                                    <tr>
                                        <td colspan="2">
                                            <div align="center" class="STYLE1 STYLE2">网银在线注册成功！</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="73">
                                            <asp:Label ID="Label1" runat="server" Text="姓名"></asp:Label>
                                        <td width="310">
                                            <asp:TextBox ID="TextBox_to" Text="李明" ToolTip="请输入用户姓名" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="300px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" Enabled="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="73">
                                            <asp:Label ID="Label2" runat="server" Text="卡号"></asp:Label>
                                        </td>
                                        <td width="310">
                                            <asp:TextBox ID="TextBox_subject" Text="100000012345" ToolTip="请输入用户的银行卡号" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="300px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" Enabled="False" />
                                        </td>
                                    </tr>
                                    
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div align="right" class="STYLE1 STYLE2">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

    </form>

</body>
</html>
