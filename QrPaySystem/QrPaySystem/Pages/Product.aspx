<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="QrPaySystem.Pages.Product" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>付费资源,二维码制作</title>
    <link rel="icon" href="~/tools/HB_pic/favicon.ico" type="image/x-icon" />
    <meta name="Keywords" content="付费资源,二维码制作" />
    <meta name="Description" content="付费资源,二维码制作" />
    <%--<meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=2.0, user-scalable=yes" />--%>
    <meta name="viewport" content="width=device-width, initial-scale=0.5, minimum-scale=0.5, maximum-scale=1.0, user-scalable=no" />
    <meta name="author" content="scimence" />
    <meta itemprop="dateUpdate" content="2019-10-15 09:35:10" />
    <style type="text/css">
        .MainPannelStyle {
            width: 720px;
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
            vertical-align: middle;
            background-position: center;
        }

        .STYLE4 {
            font-size: 18px;
        }

        .STYLE6 {
            color: #04A4EC;
        }

        .STYLE8 {
            font-size: 24px;
        }

        .auto-style2 {
            height: 171px;
            width: 706px;
        }

        .auto-style4 {
            width: 80px;
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

<body style="background-color: gray; margin-top: 0px; margin-bottom: 0px; margin-left: 0px">
    <form id="form1" runat="server" style="background-color: gray;">

        <asp:ScriptManager ID="scriptmanager1" runat="server">
        </asp:ScriptManager>


        <%--<div>
                        <asp:TextBox id="Qr_Text" runat="server" OnTextChanged="Qr_Text_TextChanged"></asp:TextBox>
                    </div>--%>


        <div style="font-family: 宋体, Arial, Helvetica, sans-serif; background-color: #4B6C9E; margin-right: auto; margin-left: auto; width: 720px; height: 50px; text-align: center; padding-top: 10px;">
            <asp:Label ID="LabelTittleName" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="楷体" Font-Size="30pt" Font-Strikeout="False" ForeColor="White" Text="付费资源，二维码制作"></asp:Label>
        </div>
        <div style="font-family: 宋体, Arial, Helvetica, sans-serif; margin-right: auto; margin-left: auto; width: 720px; background-color: #3A4F63; vertical-align: middle; text-align: right; word-spacing: normal; height: 40px;">
            <div style="width: 302px; margin-left: auto;">
                <table width="300" border="1" cellspacing="3" cellpadding="3">
                    <tr>
                        <td class="auto-style4">
                            <div id="qqDiv" runat="server">
                                <a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=536400495&site=qq&menu=yes">
                                    <img border="0" src="../tools/HB_pic/qq_button.gif" alt="点击这里给我发消息" title="联系作者" /></a>
                                <%--<a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=536400495&site=qq&menu=yes"><img border="0" src="http://pub.idqqimg.com/qconn/wpa/button/button_11.gif" alt="点击这里给我发消息" title="联系作者" /></a>--%>
                            </div>

                        </td>
                        <td>
                            <div id="Div1" runat="server" style="text-align: center"><a style="color: #FFFFFF; font-size: 20px;">微信：scimence</a></div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <asp:UpdatePanel ID="updatepanel2" runat="server" UpdateMode="conditional">
            <ContentTemplate>

                <div class="MainPannelStyle" id="MainPannel">
                    <table width="720" border="0" cellspacing="5" cellpadding="0">
                        <tr>
                            <td colspan="2" class="auto-style2">
                                <div align="center" class="STYLE0">
                                    <p align="left" class="STYLE8">您的付费资源：用户需支付后，才能获取到密码。</span></p>
                                    <p align="left" class="STYLE4">用法：填写信息，生成二维码。将二维码添加至使用密码加密的资源压缩包中，用户下载资源后，扫码支付即可自动获取密码。</p>
                                    <%--<p align="left" class="STYLE4">支付宝，更新了逻辑，红包收款码已领不到红包了（2018-11-08 21:44）</p>--%>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" class="auto-style7">
                                <div align="left" class="STYLE3">
                                    支付宝账号:
                                    <asp:TextBox ID="TextBox_Account" Text="" ToolTip="请输入您的支付宝账号，可在支付宝我的个人信息页面查看" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="230px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" />
                                    <asp:Button ID="Button_Account" runat="server" Height="29px" Text="收益提现" Width="76px" OnClick="Button_Account_Click" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="auto-style7">
                                <div align="left" class="STYLE3">
                                    名称:
                                    <asp:TextBox ID="TextBox_Name" Text="测试资源xx1" ToolTip="请输入您的付费资源名称，会在收款界面显示" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="auto-style5">
                                <div align="left" class="STYLE3">
                                    价格:
                                    <asp:TextBox ID="TextBox_Price" Text="10.00" ToolTip="请设置付费资源价格，会在收款界面显示" runat="server" TextMode="SingleLine" MaxLength="20" TabIndex="3" Width="301px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" />
                                    元
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="auto-style7">
                                <div align="left" class="STYLE3">
                                    密码:
                                    <asp:TextBox ID="TextBox_Pass" Text="654321" ToolTip="请输入你设置付费资源的，解压密码 或 网址，会在支付后展示" runat="server" TextMode="SingleLine" MaxLength="200" TabIndex="3" Width="330px" BorderStyle="Solid" Font-Names="宋体" Font-Size="X-Large" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="auto-style8">
                                <asp:ImageButton runat="server" ID="ImageButton_Create" ImageUrl="../tools/Product_pic/combine_btn_qr.png" ToolTip="生成收款二维码，用户扫码支付，即可获取密码" TabIndex="4" BorderStyle="none" OnClick="ImageButton_Create_Click" Height="75px" Width="300px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="auto-style7">
                                <br />
                                <asp:Label ID="Label_tip" Text="提示：填写您的资源信息 -> 生成付费资源二维码" runat="server" ForeColor="Red" Font-Size="14pt" />
                            </td>
                            <caption>

                                <br />
                                <caption>
                                </caption>

                            </caption>
                        </tr>
                        <caption>
                            <tr>
                                <td colspan="2" class="auto-style7">
                                    <div align="center">
                                        <img runat="server" id="img_Example" src="../tools/Product_pic/eaxmple_product_alpha.png" alt="待生成红包收款码显示区" width="600" height="900" border="2" />
                                    </div>
                                </td>
                            </tr>
                        </caption>

                        <tr>
                            <td colspan="2" class="auto-style8">
                                <div id="DivSave" runat="server" class="BtnDivStyle">
                                    <a target="_blank" href="http://www.baidu.com">
                                        <img src="../tools/Product_pic/btn_save_qr.png" height="75px" width="311px" /></a>
                                </div>
                            </td>
                        </tr>
                    </table>

                </div>
                <div style="BottomDivStyle; margin-right: auto; margin-left: auto; width: 720px; height: 30px; background-color: #465C71;"></div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>

<script type="text/javascript">
    //var btnHbScanQR = document.querySelector('#J_btn_hb_scanQR');
    //var btnSkScanQR = document.querySelector('#J_btn_sk_scanQR');

    //btnHbScanQR.addEventListener('click', function () {
    //    ap.scan(function (res) {
    //        //ap.alert(res.code);
    //        //document.getElementById("Qr_Text").value = "Hb:" + res.code;
    //        //textHb.value = res.code;
    //        document.getElementById("TextBox_HB").value = res.code;
    //    });
    //});

    //btnSkScanQR.addEventListener('click', function () {
    //    ap.scan(function (res) {
    //        //ap.alert(res.code);
    //        document.getElementById("TextBox_SK").value = res.code;
    //        //document.getElementById("Qr_Text").value = "功能测试";
    //    });
    //});

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

    //执行扫码事件
    function Qr_Scan_Hb() {
        ap.scan(function (res) {
            //ap.alert(res.code);
            //document.getElementById("Qr_Text").value = "Hb:" + res.code;
            //textHb.value = res.code;
            document.getElementById("TextBox_HB").value = res.code;
            document.getElementById("ImageButton_Create").click();  // 检测红包码变动
        });
    }

    //执行扫码事件
    function Qr_Scan_Sk() {
        ap.scan(function (res) {
            //ap.alert(res.code);
            //document.getElementById("Qr_Text").value = "Hb:" + res.code;
            //textHb.value = res.code;
            document.getElementById("TextBox_SK").value = res.code;
            document.getElementById("ImageButton_Create").click();  // 检测收款码变动
        });
    }

</script>

</html>
