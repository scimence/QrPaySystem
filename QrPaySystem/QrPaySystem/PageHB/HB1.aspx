<%@ Page Language="C#" ContentType="text/html" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>无标题文档</title>
<style type="text/css">
<!--
.MainPannelStyle {
	background-color: #FFFFFF;
	background-position: center top;
	height: auto;
	width: auto;
	text-align: center;
}
.STYLE1 {
	font-size: 24px;
	font-weight: bold;
	color: #04a4ec;
	margin-top: 10px;
	margin-bottom: 10px;
}
.STYLE3 {
	font-size: 24px;
	color: #04a4ec;
	margin-left: 100px;
	margin-right: 100px;
	margin-top: 10px;
	margin-bottom: 15px;
}
-->
</style>
</head>
<body>
<div class="MainPannelStyle" id="MainPannel">
  <table width="700" border="1" cellspacing="5" cellpadding="5">
  <tr>
    <td colspan="2"><div align="center" class="STYLE1">添加您的 红包码、收款码</div></td>
    </tr>
  <tr>
    <td><img src="../tools/HB_pic/adding_hb.png" alt="添加您的红包码" width="329" height="495" /></td>
    <td><img src="../tools/HB_pic/adding_sk.png" alt="添加您的收款码" width="334" height="499" /></td>
  </tr>
  <tr>
    <td colspan="2"><div align="left" class="STYLE3">商家名称：
        <asp:TextBox ID="TextBox_Tittle" Text="第9号当铺" ToolTip="请输入您的名称，会显示在收款界面" runat="server" TextMode="SingleLine" MaxLength="20" TabIndex="3" />
    </div></td>
    </tr>
  <tr>
    <td colspan="2"><form runat="server">
      <asp:ImageButton ID="ImageButton_Create" ImageUrl="../tools/HB_pic/combine_btn.png" ToolTip="商家收款同时，收红包；客户付款同时，领红包；更多用户，使用支付宝。" runat="server" TabIndex="4" BorderStyle="none" />      
</form></td>
    </tr>
  <tr>
    <td colspan="2">
      <asp:Label ID="Label_tip" Text="操作提示信息" runat="server" />     </td>
    </tr>
  <tr>
    <td colspan="2"><div align="center"><img src="../tools/HB_pic/eaxmple_alpha.png" alt="待生成红包收款码显示区" width="600" height="900" /></div></td>
  </tr>
</table>

</div>
</body>
</html>
