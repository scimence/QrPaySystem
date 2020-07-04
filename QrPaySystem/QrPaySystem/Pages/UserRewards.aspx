<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserRewards.aspx.cs" Inherits="QrPaySystem.Pages.UserRewards" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>收益详情</title>
    <link rel="icon" href="~/tools/HB_pic/favicon.ico" type="image/x-icon" />
    <meta name="Keywords" content="收益详情"/>
    <meta name="Description" content="收益详情" />
    <%--<meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=2.0, user-scalable=yes" />--%>
    <meta name="viewport" content="width=device-width, initial-scale=0.5, minimum-scale=0.5, maximum-scale=1.0, user-scalable=no" />
    <meta name="author" content="scimence"/>
    <meta itemprop="dateUpdate" content="2019-10-15 09:35:10" />
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
            height: 171px;
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
          <asp:Label ID="LabelTittleName" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="楷体" Font-Size="30pt" Font-Strikeout="False" ForeColor="White" Text="收益详情"></asp:Label>
        </div>            
        

        <asp:updatepanel id="updatepanel2" runat="server" updatemode="conditional">
                <contenttemplate>

                <div class="MainPannelStyle" id="MainPannel">
                      <table width="720" border="0" cellspacing="5" cellpadding="0">
                      <tr>
                        <td class="auto-style2">
                            <div align="center" class="STYLE0">
                                <p align="left" class="STYLE8">说明：</span></p>
                                <p align="left" class="STYLE4"> 收益按1：9分成，用户9成。</p>
                                <p align="left" class="STYLE4"> 每周末，自动转账提现至用户支付宝账号。</p>
                                <%--<p align="left" class="STYLE4">支付宝，更新了逻辑，红包收款码已领不到红包了（2018-11-08 21:44）</p>--%>
                            </div>
                        </td>
                      </tr>
                  
                      <tr>
                        <td class="auto-style7"><div align="left" class="STYLE3">支付宝账号: <asp:Label ID="LabelAccount" runat="server" ForeColor="Black" Text="LabelAccount"></asp:Label>
                            </div></td>
                      </tr>
                      <tr>
                        <td class="auto-style5"><div align="left" class="STYLE3">将提现收益: 
                            <asp:Label ID="LabelReward" runat="server" ForeColor="Black" Text="LabelReward"></asp:Label>
                            &nbsp;元</div></td>
                      </tr>
                      <tr>
                        <td class="auto-style8"> 
                            <div ID="DivTable" runat="server"> 

                                <br />

                            </div>
                        </td>
                      </tr>

                          
                      <tr>
                        <td class="auto-style8"> 
                            <div ID="DivTableHistory" runat="server"> 

                                <br />

                            </div>
                        </td>
                      </tr>
                    </table>

          </div>
                <div style="BottomDivStyle; margin-right: auto; margin-left: auto; width: 720px; height: 30px; background-color: #465C71;"></div>

            </ContentTemplate>
        </asp:updatepanel>   

    </form>
</body>


</html>