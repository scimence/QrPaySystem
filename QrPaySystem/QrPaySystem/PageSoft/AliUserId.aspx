<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AliUserId.aspx.cs" Inherits="QrPaySystem.PageSoft.AliUserId" %>



<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>获取支付宝用户id</title>
    <meta name="keywords" content="支付宝,授权码,获取用户id,支付宝用户id,IAopClient,Aop,DefaultAopClient,authorization_code,openapi.alipay.com,authcode,C#,java,小程序授权" />
    <style type="text/css">
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

        .auto-style2 {
            border: 0;
            width: 710px;
        }

        .auto-style3 {
            width: 197px;
        }

        .auto-style4 {
            width: 540px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="Div1" class="MainDiv">

            <div id="DivPanel">
                <div align="center">
                    <table style="width: 100%;">

                        <tr>
                            <td>
                                <table class="auto-style2">
                                    <tr>
                                        <td colspan="2">
                                            <div align="center" class="STYLE1 STYLE2">
                                                支付宝小程序，授权码 -&gt; 用户id
                                                <br />
                                                <br />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3"><span class="STYLE1">小程序Id:</span></td>
                                        <td class="auto-style4">
                                            <asp:TextBox ID="text_AppId" Text="" ToolTip="在此处填写您的小程序appId，如：201903266xxxxxx1" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="560px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" OnTextChanged="TextBox_AppId_TextChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3"><span class="STYLE1">小程序<br />
                                            私钥：</span></td>
                                        <td class="auto-style4">
                                            <asp:TextBox ID="text_privateKey" Text="" ToolTip="在此填写支付宝小程序，所使用的私钥" runat="server" TextMode="MultiLine" MaxLength="100000" TabIndex="3" Width="560px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" Height="100px" BorderColor="#999999" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style3"><span class="STYLE1">支付宝<br />
                                            公钥：</span></td>
                                        <td class="auto-style4">
                                            <asp:TextBox ID="text_aliPublicKey" Text="" ToolTip="在此填写支付宝小程序，所使用的支付宝公钥" runat="server" TextMode="MultiLine" MaxLength="100000" TabIndex="3" Width="560px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" Height="100px" BorderColor="#999999" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3"><span class="STYLE1">小程序<br />
                                            授权码：</span></td>
                                        <td class="auto-style4">
                                            <asp:TextBox ID="text_authCode" Text="" ToolTip="支付宝小程序中，获取到的授权码" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="560px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div align="left" class="STYLE1">
                                    <br />
                                    &nbsp; <asp:Button ID="Button_GetUserId" runat="server" Font-Size="X-Large" Height="41px" Text="获取支付宝用户Id" Width="235px" ToolTip="从授权码，获取用户id" OnClick="Button_GetUserId_Click" Style="margin-left: 0px" />

                                &nbsp;&nbsp;
                                    <asp:CheckBox ID="checkRaw" runat="server" Text="获取支付宝原始返回串" BorderStyle="None" Font-Size="X-Large" ToolTip="支付宝返回原始信息" />
                                    <br />

                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <br />
                                <div class="STYLE1 STYLE2">
                                    <p>
                                        支付宝用户id:
                                    </p>
                                    <p>
                                        <asp:TextBox ID="text_aliUserId" runat="server" BorderColor="#999999" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" Height="100px" MaxLength="100000" TabIndex="3" Text="" TextMode="MultiLine" ToolTip="获取到的支付宝用户id，或异常提示信息" Width="710px" />
                                    </p>
                                    <br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div align="left" ID="DivTipInfo" runat="server" class="STYLE1 STYLE2">

                                    <p>
					                    开通在线接口（<asp:LinkButton ID="HrefPay2" runat="server" OnClick="HrefPay2_Click">未开通</asp:LinkButton>）
				                    </p>

                                    <%--<p>
					                    开通在线接口（<a  ID="HrefPay" runat="server" href="https://scimence.gitee.io/AliUserId/files/Csharp获取支付宝用户id.zip" target="_blank">支付开通</a>）
				                    </p>--%>

                                    <p>
					                    <a ID="HrefLink" runat="server" href="https://scimence.gitee.io/AliUserId/files/Csharp获取支付宝用户id.zip" target="_blank"></a>
				                    </p>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div align="left">
                                    <br />
				                    <p>
					                    源码下载: <a href="https://scimence.gitee.io/AliUserId/files/Csharp获取支付宝用户id.zip" target="_blank">C#获取支付宝用户id.zip</a>
				                    </p>
				                    <p>
					                    源码下载: <a href="https://scimence.gitee.io/AliUserId/files/Java获取支付宝用户id.zip" target="_blank">Java获取支付宝用户id.zip</a>
				                    </p>
                                    <br />
			                    </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

    </form>

</body>
</html>

