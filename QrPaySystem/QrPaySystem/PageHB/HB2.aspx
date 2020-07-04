<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HB2.aspx.cs" Inherits="QrPaySystem.PageHB.HB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>支付宝红包收款码</title>
    <link rel="icon" href="~/tools/HB_pic/favicon.ico" type="image/x-icon" />
    <meta name="Keywords" content="红包,收款,收款码,红包码,红包收款码,收款红包码,红包码收款码合并,收款码红包码合二为一,支付宝,如何申请红包码,如何获取收款码,二维码,红包收款码制作,制作收款码,支付宝红包收钱码在哪,支付宝收钱码不能用红包,支付宝收钱码不能用了,支付宝收钱码怎么变成商家的,支付宝怎么推荐收钱码,支付宝收钱码怎么用,收钱码合并"/>
    <meta name="Description" content="红包收款码：将红包码合并至收款码上。顾客：既能领红包，又能付款；商家：既能收款，又能收到红包返利。红包,收款,收款码,红包码,红包收款码,收款红包码,红包码收款码合并,收款码红包码合二为一,支付宝,如何申请红包码,如何获取收款码,二维码,红包收款码制作,制作收款码,支付宝红包收钱码在哪,支付宝收钱码不能用红包,支付宝收钱码不能用了,支付宝收钱码怎么变成商家的,支付宝怎么推荐收钱码,支付宝收钱码怎么用,收钱码合并" />
    <%--<meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=2.0, user-scalable=yes" />--%>
    <meta name="viewport" content="width=device-width, initial-scale=0.5, minimum-scale=0.5, maximum-scale=1.0, user-scalable=no" />
    <meta name="author" content="scimence"/>
    <meta itemprop="dateUpdate" content="2018-10-08 23:45:10" />
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
        .auto-style1 {
            width: 344px;
        }
    .STYLE4 {font-size: 18px}
    .STYLE6 {color: #04A4EC}
    .STYLE8 {font-size: 24px}
        .auto-style2 {
            height: 171px;
        }
        .auto-style3 {
            width: 353px;
        }
        .auto-style4 {
            width: 55px;
        }
    </style>
</head>
<body style="background-color:gray; margin-top:0px; margin-bottom:0px; margin-left:0px">
<form id="form1" runat="server" style="background-color:gray;" >

            <%--<asp:scriptmanager id="scriptmanager1" runat="server">
            </asp:scriptmanager>--%>

            <%--<asp:updatepanel id="updatepanel1" runat="server" updatemode="conditional">
                <contenttemplate>--%>

                    <%--<div>
                        <asp:TextBox id="Qr_Text" runat="server" OnTextChanged="Qr_Text_TextChanged"></asp:TextBox>
                    </div>--%>
                

        <div style="font-family: 宋体, Arial, Helvetica, sans-serif; background-color: #4B6C9E; margin-right: auto; margin-left: auto; width: 720px; height: 50px; text-align: center; padding-top: 10px;">
          <asp:Label ID="LabelTittleName" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="楷体" Font-Size="30pt" Font-Strikeout="False" ForeColor="White" Text="红包收款码制作工具"></asp:Label>
        </div>            
        <div style="font-family: 宋体, Arial, Helvetica, sans-serif; margin-right: auto; margin-left: auto; width: 720px; background-color: #3A4F63; vertical-align: middle; text-align: right; word-spacing: normal; height: 40px;">
            <div style="width: 302px; margin-left: auto;">
               <table width="300" border="1" cellspacing="3" cellpadding="3">
                <tr>
                  <td class="auto-style4"><a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=536400495&site=qq&menu=yes"><img border="0" src="http://pub.idqqimg.com/qconn/wpa/button/button_11.gif" alt="点击这里给我发消息" title="联系作者" /></a></td>
                  <td><a href="javascript:window.external.addFavorite('HB.aspx','红包收款码制作工具');" style="color: #FFFFFF; font-size: 20px; font-family: 宋体, Arial, Helvetica, sans-serif;">收藏工具</a></td>
                  <td><a onclick="this.style.behavior='url(#default#homepage)';this.setHomePage('HB.aspx');" href="#" style="color: #FFFFFF; font-size: 20px;">设为首页</a></td>
                </tr>
              </table>
            </div>
        </div>
        <div class="MainPannelStyle" id="MainPannel">
              <table width="720" border="0" cellspacing="5" cellpadding="0">
              <tr>
                <td colspan="2" class="auto-style2">
                    <div align="center" class="STYLE0">
                        <p align="left" class="STYLE8">支付宝，<span class="STYLE6">红包收款码：</span></p>
                        <p align="left" class="STYLE4"> 将红包码添加至收款码上。</p>
                        <p align="left" class="STYLE4">顾客：既能领红包，又能付款；</p>
                        <p align="left" class="STYLE4">商家：既能收款，又能收到红包返利。</p>
                    </div>                </td>
              </tr>
              <tr>
                <td colspan="2"><div align="center" class="STYLE1">点击添加您的 红包码、收款码</div></td>
              </tr>
              <tr>
                <td width="344" >
                <asp:FileUpload ID="FileUpload_HB" runat="server"  onchange="document.getElementById('Button_HB').click();" />
                    <asp:Button ID="Button_HB" runat="server" Text="上传红包码" ToolTip="选择红包码后，需要上传服务器解析" OnClick="Button_HB_Click" />                </td>
              <td class="auto-style3">
            <asp:FileUpload ID="FileUpload_SK" runat="server"  onchange="document.getElementById('Button_SK').click();"/>
                    <asp:Button ID="Button_SK" runat="server" Text="上传收款码" ToolTip="选择收款码后，需要上传服务器解析" OnClick="Button_SK_Click" />                </td>
              </tr>
              <tr>
                <td >
                    <a id="HelpLinkHB" target="_blank" href="../tools/HB_pic/help_hb.png">
                        <asp:Label runat="server" Text="红包码 如何获取？"></asp:Label>
                    </a>                </td>
                <td class="auto-style3" >
                    <a id="HelpLinkSK" target="_blank" href="../tools/HB_pic/help_sk.png">
                        <asp:Label runat="server" Text="收款码 如何获取？"></asp:Label>
                    </a>                </td>
              </tr>
              <tr>
                <td><div align="center"><img src="../tools/HB_pic/adding_hb.png" alt="添加您的红包码" name="img_HB" width="329" height="495" border="0" id="img_HB" onclick="javascript:FileUpload_HB_SelectFile();" runat="server" /></div></td>
                <td class="auto-style3"><div align="center"><img src="../tools/HB_pic/adding_sk.png" alt="添加您的收款码" name="img_SK" width="334" height="499" border="0" id="img_SK" onclick="javascript:FileUpload_SK_SelectFile();" runat="server" /></div></td>
              </tr>
                  
              <tr>
                <td class="auto-style1">
                    <asp:TextBox ID="TextBox_HB" Text="红包码" MaxLength="200" runat="server" ToolTip="上传您的红包码图像，会自动填写" Width="260px" BorderStyle="Dashed"  />                </td>
 
                    <%--<div>
                      <button id="J_btn_hb_scanQR" runat="server" class="btn btn-default">扫一扫</button>
                        <asp:updatepanel id="updatepanel1" runat="server" updatemode="conditional">
                        <contenttemplate>
                        <asp:TextBox ID="TextBox_HB" Text="红包码" MaxLength="200" runat="server" ToolTip="上传您的红包码图像，会自动填写" Width="260px" BorderStyle="Dashed"  />                </td>
                        </contenttemplate>
                        </asp:updatepanel>
                   </div>--%>
               <td class="auto-style3">
                  <%--<button id="J_btn_sk_scanQR" runat="server" class="btn btn-default">扫一扫</button>--%>
                   <asp:TextBox ID="TextBox_SK" Text="收款码" MaxLength="200" runat="server" ToolTip="上传您的收款码图像，会自动填写" Width="260px" BorderStyle="Dashed"/>                </td>
              </tr>

              <tr>
                <td colspan="2"><div align="left" class="STYLE3">商家名称: <asp:TextBox ID="TextBox_Tittle" Text="第8号当铺" ToolTip="请输入您的名称，会在收款界面显示" runat="server" TextMode="SingleLine" MaxLength="20" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" />
                </div></td>
              </tr>
              <tr>
                <td colspan="2">
                  <asp:ImageButton runat="server" ID="ImageButton_Create" ImageUrl="../tools/HB_pic/combine_btn.png" ToolTip="商家收款同时，收红包；顾客付款同时，领红包；更多用户，使用支付宝。" TabIndex="4" BorderStyle="none" OnClick="ImageButton_Create_Click" Height="84px" Width="538px" />                </td>
                </tr>
              <tr>
                <td colspan="2">
                  <asp:Label ID="Label_tip" Text="提示：添加红包码、收款码后 -> 点击合并按钮，生成您的专属红包收款码" runat="server" ForeColor="Red" Font-Size="14pt" />     </td>
                </tr>
              <tr>
                <td colspan="2"><div align="center"><img runat="server" id="img_Example" src="../tools/HB_pic/eaxmple_alpha.png" alt="待生成红包收款码显示区" width="600" height="900" border="0"/></div></td>
              </tr>
            </table>

  </div>
        <div style="BottomDivStyle; margin-right: auto; margin-left: auto; width: 720px; height: 30px; background-color: #465C71;"></div>

                    
              <%--</ContentTemplate>
            </asp:updatepanel>   --%>
    </form>
</body>

<script type="text/javascript" src="./scan_files/alipayjsapi.inc.min.js"></script>
<script type="text/javascript">
    var btnHbScanQR = document.querySelector('#J_btn_hb_scanQR');
    var btnSkScanQR = document.querySelector('#J_btn_sk_scanQR');

    btnHbScanQR.addEventListener('click', function () {
        ap.scan(function (res) {
            //ap.alert(res.code);
            //document.getElementById("Qr_Text").value = "Hb:" + res.code;
            //textHb.value = res.code;
            document.getElementById("TextBox_HB").value = res.code;
        });
    });

    btnSkScanQR.addEventListener('click', function () {
        ap.scan(function (res) {
            //ap.alert(res.code);
            document.getElementById("TextBox_SK").value = res.code;
            //document.getElementById("Qr_Text").value = "功能测试";
        });
    });

    //条形码
    //var btnScanBAR = document.querySelector('#J_btn_scanBAR');
    //btnScanBAR.addEventListener('click', function () {
    //    ap.scan({
    //        type: 'bar'
    //    }, function (res) {
    //        ap.alert(res.code);
    //    });
    //});
</script>

<script type="text/javascript">
    // 打开上传文件浏览
    function FileUpload_HB_SelectFile() {
        var ie = navigator.appName == "Microsoft Internet Explorer" ? true : false;
        if (ie) {
            document.getElementById("FileUpload_HB").click();     // 通过document元素点击FileUpload控件
            //document.getElementById("TextBox_HB").onchange = document.getElementById("FileUpload_HB").onchange;
        }
        else {
            var a = document.createEvent("MouseEvents");
            a.initEvent("click", true, true);
            document.getElementById("FileUpload_HB").dispatchEvent(a);
        }
    }

    function FileUpload_SK_SelectFile() {
        var ie = navigator.appName == "Microsoft Internet Explorer" ? true : false;
        if (ie) {
            document.getElementById("FileUpload_SK").click();     // 通过document元素点击FileUpload控件
            //document.getElementById("TextBox_SK").value = document.getElementById("FileUpload_SK").value;
        }
        else {
            var a = document.createEvent("MouseEvents");
            a.initEvent("click", true, true);
            document.getElementById("FileUpload_SK").dispatchEvent(a);
        }
    }

</script>

</html>
