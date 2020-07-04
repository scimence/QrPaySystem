<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankOnlineRegister.aspx.cs" Inherits="QrPaySystem.boc.BankOnlineRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=0.5, minimum-scale=0.5, maximum-scale=0.5, user-scalable=no" />
    <title></title>
    <style type="text/css">
        <!--
        .MainDiv {
            width: 720px;
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
                                <table width="712" border="0" cellspacing="0" cellpadding="1">
                                    <tr>
                                        <td colspan="2">
                                            <div align="center" class="STYLE1 STYLE2">网银在线注册</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="73"><span class="STYLE1">姓名：</span></td>
                                        <td width="629">
                                            <asp:TextBox ID="TextBox_to" Text="李明" ToolTip="请输入用户姓名" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="616px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="73"><span class="STYLE1">卡号：</span></td>
                                        <td width="629">
                                            <asp:TextBox ID="TextBox_subject" Text="100000012345" ToolTip="请输入用户的银行卡号" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="616px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="73"><span class="STYLE1">协议：</span></td>
                                        <td width="629">
                                            <asp:TextBox ID="TextBox_body" Text="《网银在线开通 注册协议》

1、协议内容说明信息协议内容说明信息协议内容说明信息协议内容说明信息协议内容说明信息
2、协议内容说明信息协议内容说明信息协议内容说明信息协议内容说明信息协议内容说明信息

......

10、协议内容说明信息协议内容说明信息协议内容说明信息协议内容说明信息协议内容说明信息
11、协议内容说明信息协议内容说明信息协议内容说明信息协议内容说明信息协议内容说明信息

" ToolTip="网银在线开通协议" runat="server" TextMode="MultiLine" MaxLength="100000" TabIndex="3" Width="616px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" Height="210px" BorderColor="#999999" ReadOnly="True" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div align="right" class="STYLE1 STYLE2">
                                    <asp:CheckBox ID="CheckBox_sp" runat="server" Text="同意 " BorderStyle="None" Font-Size="X-Large" ToolTip="同意注册协议中的内容"  />
                                    <asp:Button ID="Button_send" runat="server" Font-Size="X-Large" Height="41px" Text="注册" Width="104px" ToolTip="注册网银" OnClick="Button_send_Click"  />
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
