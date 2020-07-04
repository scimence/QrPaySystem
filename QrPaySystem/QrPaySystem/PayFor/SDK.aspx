 <%@ Page Title="" Language="C#" MasterPageFile="~/PayFor/PayForMaster.Master" AutoEventWireup="true" CodeBehind="SDK.aspx.cs" Inherits="QrPaySystem.PayFor.SDKPage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentBodyHoder" runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="icon" href="~/tools/HB_pic/favicon.ico" type="image/x-icon" />
    <meta name="Keywords" content="关键字,收款,收款码"/>
    <meta name="Description" content="描述信息" />
    <%--<meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=2.0, user-scalable=yes" />--%>
    <meta name="viewport" content="width=device-width, initial-scale=0.5, minimum-scale=0.5, maximum-scale=1.0, user-scalable=no" />
    <meta name="author" content="scimence"/>
    <meta itemprop="dateUpdate" content="2018-10-08 23:45:10" />

    <style type="text/css">
        .MainPannelStyle 
        {
	        width:720px;
	        background-color: #FFFFFF;
	        background-position: center top;
	        height: auto;
	        width: 720px;
	        text-align: center;
	        margin-left: auto;
	        margin-right: auto;
	        border: 1px;
        }
    </style>

    <div style="width:960px; height:auto; margin-right: auto; margin-left: auto; text-align: left; background-color:white; margin-top:10px; font-size:20px; font-weight:bolder">
        <div style="margin-left:30px; padding-top:5px; padding-bottom:10px;">
            <p>1、下载并接入SDK，为您的应用提供支付功能</p>
            <p>2、发布您的应用到网络，等待用户使用并收益</p>
        </div>
    </div>

    <div style="font-family: 宋体, Arial, Helvetica, sans-serif; margin-right: auto; margin-left: auto; width: 960px; height: auto; text-align: center; margin-top:10px; margin-bottom:40px; padding-bottom:30px">
        
        <div style="width:300px; height:370px; left:0px; background-color:white; float:left;">

            <p style="height:40px"> </p>
            <img src="../tools/Payfor_res/sdk_icon_windows.png" />
            <p style="font-size:20px; font-weight:bolder">Windows应用</p>

            <div runat="server" id="div_windows">
                <p >版本：2018-12-30</p>
                <a href="../tools/Payfor_res/sdk_windows.zip"><img src="../tools/Payfor_res/sdk_download.png" /></a>
            </div>  
            <br />
            <div>
                <a href="https://blog.csdn.net/scimence/article/details/90445532" target="_blank">支付功能接入文档</a>
            </div>
        </div>

        <div style="width:300px; height:370px; left:0px; background-color:white; float:left; margin-left:30px">
            <p style="height:40px"> </p>
            <img src="../tools/Payfor_res/sdk_icon_android.png" />
            <p style="font-size:20px; font-weight:bolder">Android应用</p>

            <div runat="server" id="div_android">
                <p >版本：2018-12-30</p>
                <a href="../tools/Payfor_res/sdk_android.zip"><img src="../tools/Payfor_res/sdk_download.png" /></a>
            </div>  
        </div>

        <div style="width:300px; height:370px; left:0px; background-color:white; float:left; margin-left:30px">
            <p style="height:40px"> </p>
            <img src="../tools/Payfor_res/sdk_icon_web.png" />
            <p style="font-size:20px; font-weight:bolder">Web应用（通用）</p>

            <div runat="server" id="div_web">
                <p >版本：2018-12-30</p>
                <a href="../tools/Payfor_res/sdk_web.zip"><img src="../tools/Payfor_res/sdk_download.png" /></a>
            </div>  
        </div>
    </div>


</asp:Content>
