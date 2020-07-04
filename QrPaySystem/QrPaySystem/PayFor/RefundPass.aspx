<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundPass.aspx.cs" Inherits="QrPaySystem.PayFor.RefundPass" %>

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
            width: 582px;
        }
        .auto-style2 {
            height: 34px;
        }
.STYLE1 {
	font-size: x-large
}
.STYLE2 {color: #3399CC}
        -->
        .align-center{ position:fixed;left:50%;top:50%;margin-left:-255px;margin-top:-104px; width:510px; height:202px; 
background-color:silver; fill-opacity:20
        }
        .back-color { background:#019fe8;}

        .auto-style3 {
            width: 582px;
        }
        .auto-style4 {
            height: 34px;
            width: 582px;
        }

    </style>
</head>
<body style="background-color:#696969">
    <form id="form1" runat="server">
        <div id="Div1" class="align-center" align="center" runat="server">
            
            <table width="500" border="0" cellpadding="2" cellspacing="0" bordercolor="#666666">
                <tr>
                    <td class="auto-style3"><div align="center" class="STYLE1 STYLE2">重置密码</div></td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <div align="center">
                            <span class="STYLE1">帐  号：</span>
                            <asp:TextBox ID="TextBox_account" Text="" ToolTip="请输入您的帐号" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" />                        
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <div align="center">
                            <span class="STYLE1">手机号：</span>
                            <asp:TextBox ID="TextBox_phone" Text="" ToolTip="手机号用于身份验证" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" />                        
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <div align="center">
                            <span class="STYLE1">身份证：</span>
                            <asp:TextBox ID="TextBox_IdCard" Text="" ToolTip="身份证号用于身份验证" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999" />                        
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <div align="right" style="width: 453px" >
                            <asp:TextBox ID="TextBox_psw" Text="" ToolTip="请输入新的密码" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="324px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" BorderColor="#999999"  />
                            <asp:Button ID="Button_register" runat="server" Font-Size="X-Large" Height="41px" Text="重置密码" Width="114px" ToolTip="重置密码" OnClick="Button_register_Click"  />                
                        </div>
                    </td>
                </tr>
            </table>
            
            <div style="height:20px"></div>
            <div id="DivInfo" runat="server">   <asp:Label ID="Label_TipInfo" runat="server" Text="请输您的帐号、手机号、身份证。若正确则可修改密码!" ForeColor="#CC0000"></asp:Label>     </div>
            
        </div>

    </form>

</body>
</html>

