<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SoftReg.aspx.cs" Inherits="QrPaySystem.Pages.SoftReg" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>注册软件，详情</title>
    <link rel="icon" href="~/tools/HB_pic/favicon.ico" type="image/x-icon" />
    <meta name="Keywords" content="注册软件，详情"/>
    <meta name="Description" content="注册软件，详情" />
    <%--<meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=2.0, user-scalable=yes" />--%>
    <meta name="viewport" content="width=device-width, initial-scale=0.5, minimum-scale=0.5, maximum-scale=1.0, user-scalable=no" />
    <meta name="author" content="scimence"/>
    <meta itemprop="dateUpdate" content="2019-11-11 15:49:10" />
    <style type="text/css">
        
    .MainPannelStyle {
	width:720px;
	background-color: #FFFFFF;
	background-position: center top;
	height: auto;
	width: 720px;
	text-align: center;
	margin-left: auto;
	margin-right: auto;
	border: 1;
    }
	.STYLE0 {
	font-size: 20px;
	font-weight: bold;
	color: #000000;
	margin-top: 20px;
	margin-bottom: 20px;
	padding-left: 0px;
	padding-right: 0px;
	margin-left: 20px;
	margin-right: 20px;
    }
    .STYLE1 {
	    font-size: 24px;
	    font-weight: bold;
	    color: #04a4ec;
	    margin-top: 10px;
	    margin-bottom: 0px;
    }
    .STYLE3 {
	font-size: 28px;
	color: #04a4ec;
	margin-left: 100px;
	margin-right: 100px;
	margin-top: 10px;
	margin-bottom: 15px;
	text-align: center;
	vertical-align:middle;
	background-position: center;
    }
        .STYLE4 {font-size: 18px}
    .STYLE6 {color: #04A4EC}
    .STYLE8 {font-size: 24px}
        .auto-style2 {
            height: 71px;
            width: 706px;
        }
        .auto-style5 {
            height: 60px;
            width: 706px;
        }
        .auto-style7 {
            width: 706px;
        }
        .auto-style8 {
            height: 80px;
            width: 706px;
        }
        .auto-style9 {
            width: 706px;
            height: 66px;
        }
    </style>
</head>

    
<script type="text/javascript" src="./scan_files/alipayjsapi.inc.min.js"></script>

<body style="background-color:gray; margin-top:0px; margin-bottom:0px; margin-left:0px">
<form id="form1" runat="server" style="background-color:gray;" >

        <asp:scriptmanager id="scriptmanager1" runat="server">
        </asp:scriptmanager>

        <%--<div>
            <asp:TextBox id="Qr_Text" runat="server" OnTextChanged="Qr_Text_TextChanged"></asp:TextBox>
        </div>--%>
        
        <div style="font-family: 宋体, Arial, Helvetica, sans-serif; background-color: #4B6C9E; margin-right: auto; margin-left: auto; width: 720px; height: 50px; text-align: center; padding-top: 10px;">
          <asp:Label ID="LabelTittleName" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="楷体" Font-Size="30pt" Font-Strikeout="False" ForeColor="White" Text="注册软件，详情"></asp:Label>
        </div>            
        

        <asp:updatepanel id="updatepanel2" runat="server" updatemode="conditional">
                <contenttemplate>

                <div class="MainPannelStyle" id="MainPannel">
                    <table width="720" border="0" cellspacing="5" cellpadding="0">
                      <tr>
                        <td class="auto-style2">
                            <div align="center" class="STYLE0">
                                <p align="left" class="STYLE8">说明：</span></p>
                                <p align="left" class="STYLE4"> 1、在当前页，添加您的注册软件信息。</p>
                                
                                <p align="left" class="STYLE4"> 2、在您的软件中，接入注册功能。<a href="https://gitee.com/scimence/RegDemo" target="_blank">Windows应用，注册插件接入文档</a></p>
                            </div>
                        </td>
                      </tr>
                  
                      <tr>
                        <td class="auto-style7"><div align="left" class="STYLE3">支付宝账号: <asp:Label ID="LabelAccount" runat="server" ForeColor="Black" Text="LabelAccount"></asp:Label>
                            </div></td>
                      </tr>

                      <tr>
                        <td class="auto-style7"><div align="left" class="STYLE3">软件名称: <asp:TextBox ID="TextBox_Name" Text="RegDemo" ToolTip="请输入您的注册软件名称" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" />
                        </div></td>
                      </tr>
                      <tr>
                        <td class="auto-style5"><div align="left" class="STYLE3">软件价格: <asp:TextBox ID="TextBox_Price" Text="10.00" ToolTip="请设置注册价格" runat="server" TextMode="SingleLine" MaxLength="20" TabIndex="3" Width="301px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" />
                        元</div></td>
                      </tr>
                      <tr>
                        <td class="auto-style7"><div align="left" class="STYLE3">试用次数: <asp:TextBox ID="TextBox_Time" Text="3" ToolTip="请输入软件可免费试用的次数" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" />
                        </div></td>
                      </tr>

                      <tr>
                        <td class="auto-style9">
                            <div align="left" class="STYLE3">
                                <asp:Button ID="ButtonAdd" runat="server" Height="36px" Text="添加" Width="134px" Font-Size="X-Large" ForeColor="#333399" OnClick="ButtonAdd_Click" ToolTip="请添加软件名称、软件价格" />
                                &nbsp;
                                <asp:Button ID="ButtonUpdate" runat="server" Height="36px" Text="修改" Width="134px" Font-Size="X-Large" ForeColor="#333399" OnClick="ButtonUpdate_Click" ToolTip="请填写待修改的软件名称，以及待修改的价格、试用次数。空则不修改" />
                                &nbsp;
                                <asp:Button ID="ButtonDelet" runat="server" Height="36px" Text="删除" Width="134px" Font-Size="X-Large" ForeColor="#333399" OnClick="ButtonDelet_Click" ToolTip="请填写待删除的软件名称" />
                            
                            </div>
                        </td>
                      </tr>

                      <tr>
                        <td>

                          <asp:Label ID="Label_tip" Text="提示信息" runat="server" ForeColor="Red" Font-Size="14pt" />

                            <br />

                        </td>
                      </tr>

                      <caption>
                            <br />
                            </tr>
                            <tr>
                                <td class="auto-style8">
                                    <div id="DivTable" runat="server">
                                        <br />
                                    </div>
                                </td>
                            </tr>
                      </caption>
                          
                </table>

          </div>
                <div style="BottomDivStyle; margin-right: auto; margin-left: auto; width: 720px; height: 30px; background-color: #465C71;"></div>

            </ContentTemplate>
        </asp:updatepanel>   

    </form>
</body>


</html>