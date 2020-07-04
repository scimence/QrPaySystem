<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowInfo.aspx.cs" Inherits="QrPaySystem.PageHB.ShowInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>信息页</title>
    <link rel="icon" href="~/tools/HB_pic/favicon.ico" type="image/x-icon" />
    <meta name="viewport" content="width=device-width, initial-scale=0.5, minimum-scale=0.5, maximum-scale=1.0, user-scalable=no" />
    <style type="text/css">
        
        .MainDivStyle{
	        width:720px;
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 0;
	        margin-right: auto;
	        margin-left: auto;
	        float: none;
	        background-repeat: no-repeat;
	        background-position: top;
	        background-color: #019FE8;
                }
        .TittleDivStyle{
	        text-align:center;
	        border:0px solid #F00;
	        margin-top: 270px;
	        margin-right: auto;
	        margin-bottom: 0;
	        margin-left: auto;
	        padding-left: 20px;
	        padding-right: 20px;
                }
    </style>
</head>
<body class="MainDivStyle">
    <div class="TittleDivStyle">
        <p style="font-size:40px; color:white">
          <asp:Label ID="LabelInfo" runat="server" Text="待展示信息（自定义）"></asp:Label>
        </p>
    </div>
</body>
</html>
