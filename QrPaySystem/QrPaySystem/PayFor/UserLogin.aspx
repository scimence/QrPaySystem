<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="QrPaySystem.PayFor.UserLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
<!--
.MainDiv {
	width: 720px;
	margin-right: auto;
	margin-left: auto;
	margin-top: auto;
	margin-bottom: auto;
	border: 0px solid #999999;
}
        .auto-style1 {
            height: 37px;
        }
        .auto-style2 {
            height: 34px;
        }
.STYLE1 {
	font-size: x-large
}
.STYLE2 {color: #3399CC}
        -->
        .align-center{ position:fixed;left:50%;top:50%;margin-left:-255px;margin-top:-104px; width:510px; height:169px; 
background-color:silver; fill-opacity:20
        }
        .back-color { background:#019fe8;}

    </style>
</head>
<body style="background-color:#696969">
    <form id="form1" runat="server">
        <div id="Div1" class="align-center" align="center" runat="server">
            
            <table width="500" border="0" cellpadding="2" cellspacing="0" bordercolor="#666666">
                <tr>
                    <td><div align="center" class="STYLE1 STYLE2">用户登录</div></td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <div align="center">
                            <span class="STYLE1">帐号：</span>
                            <asp:TextBox ID="TextBox_account" Text="" ToolTip="请输入您的帐号" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" />                        
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <div align="center">
                            <span class="STYLE1">密码：</span>
                            <asp:TextBox ID="TextBox_psw" Text="" ToolTip="请输入您的密码" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999"  />
                            <%--<input ID="TextBox_psw" runat="server" TextMode="SingleLine" style="border: 2px solid #999999; font-family: 宋体, Arial, Helvetica, sans-serif; font-size: x-large; width: 330px;" type="password" />--%>

                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <div align="right" style="width: 453px" >
                            
                            <asp:Button ID="Button_login" runat="server" Font-Size="X-Large" Height="41px" Text="登录" Width="114px" ToolTip="登录" OnClick="Button_login_Click"  />                
                        </div>
                    </td>
                </tr>
            </table>
            
            <div style="height:20px"></div>
            <div>
                <a id="LinkRegister" target="_self" href="UserRegister.aspx"><asp:Label ID="LabelLink" runat="server" Text="注册" ForeColor="Blue" Width="200px"></asp:Label></a>
                <a id="LinkRePass" target="_self" href="RefundPass.aspx"><asp:Label ID="Label1" runat="server" Text="找回密码" ForeColor="Blue" Width="200px"></asp:Label></a>
            </div>
            <div id="DivInfo" runat="server">   <asp:Label ID="Label_TipInfo" runat="server" Text="" ForeColor="#CC0000"></asp:Label>     </div>
            
        </div>

    </form>

</body>
</html>
