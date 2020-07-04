<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailTool.aspx.cs" Inherits="QrPaySystem.PageSoft.MailTool" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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

        .auto-style1 {
            height: 37px;
        }

        .auto-style2 {
            height: 34px;
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
        <div class="MainDiv" runat="server">
            <div id="DivInfo" runat="server">
                <asp:Label ID="Label_TipInfo" runat="server" Text="提示信息" ForeColor="#CC0000"></asp:Label>
            </div>

            <div id="DivPanel" runat="server">
                <div align="center">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="DivSender" runat="server">

                                    <table width="711" border="0" cellpadding="2" cellspacing="0" bordercolor="#666666">
                                        <tr>
                                            <td colspan="2">
                                                <div align="center" class="STYLE1 STYLE2">发件人</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2" colspan="2">
                                                <div align="center">
                                                    <span class="STYLE1">邮箱：</span>
                                                    <asp:TextBox ID="TextBox_from" Text="" ToolTip="你的邮箱帐号，作为发件方" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style2" colspan="2">
                                                <div align="center">
                                                    <span class="STYLE1">密码：</span>
                                                    <asp:TextBox ID="TextBox_psw" Text="" ToolTip="邮箱帐号的密码" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" />
                                                    <%--<input ID="TextBox_psw" runat="server" TextMode="SingleLine" style="border: 2px solid #999999; font-family: 宋体, Arial, Helvetica, sans-serif; font-size: x-large; width: 330px;" type="password" />--%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style1" colspan="2">
                                                <div align="right">
                                                    <asp:Button ID="Button_record" runat="server" Font-Size="X-Large" Height="41px" Text="记住密码" Width="114px" ToolTip="记住当前帐号密码信息" OnClick="Button_record_Click" />

                                                    <%--<asp:CheckBox ID="CheckBox_record" runat="server" Text="记住密码" BorderStyle="None" Font-Size="X-Large" OnCheckedChanged="CheckBox_record_CheckedChanged" />--%>
                                                </div>


                                            </td>
                                        </tr>
                                    </table>

                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="712" border="0" cellspacing="0" cellpadding="1">
                                    <tr>
                                        <td colspan="2">
                                            <div align="center" class="STYLE1 STYLE2">收件人</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="73"><span class="STYLE1">邮箱：</span></td>
                                        <td width="629">
                                            <asp:TextBox ID="TextBox_to" Text="" ToolTip="如：scimence@163.com多个收件人时请用';'分隔" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="616px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="73"><span class="STYLE1">标题：</span></td>
                                        <td width="629">
                                            <asp:TextBox ID="TextBox_subject" Text="" ToolTip="邮件标题，为空时自动添加日期时间作为标题" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="616px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="73"><span class="STYLE1">内容：</span></td>
                                        <td width="629">
                                            <asp:TextBox ID="TextBox_body" Text="" ToolTip="邮件的内容，可不填" runat="server" TextMode="MultiLine" MaxLength="100000" TabIndex="3" Width="616px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" Height="210px" BorderColor="#999999" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div align="right" class="STYLE1 STYLE2">
                                    <asp:CheckBox ID="CheckBox_sp" runat="server" Text="单独发送" BorderStyle="None" Font-Size="X-Large" ToolTip="当有多个收件人时，将分别对各个收件人逐一发送" />
                                    <asp:Button ID="Button_send" runat="server" Font-Size="X-Large" Height="41px" Text="发送" Width="104px" ToolTip="有多个收件人时，会以多收件人形式向所有人发送" OnClick="Button_send_Click" />
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
