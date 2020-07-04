<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HB_pre.aspx.cs" Inherits="QrPaySystem.PageHB.HB_pre" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <br />
        <br />
        <asp:Label ID="Label1" runat="server" Text="收款码："></asp:Label>
&nbsp;<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="红包码："></asp:Label>
&nbsp;<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="Label3" runat="server" Text="商铺名称："></asp:Label>
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        <br />
        <br />
        <br />
    
        <asp:Button ID="Button1" runat="server" Text="添加" OnClick="Button1_Click" />
    

        <br />
        <br />
        <br />
    
        <asp:HyperLink ID="HyperLink1" runat="server">红包收款码</asp:HyperLink>
    

        <br />
        <br />
        <br />
        
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        

        <asp:FileUpload ID="FileUpload1" runat="server" />
        <br />
        <br />

        <%--为按钮添加onclick响应逻辑--%>
        <input type="button" name="button" value="点我就像点击“浏览”按钮一样" onclick="javascript: Upload_openBrowse();" /> 
        <br />
        <br />
        <br />
        <img runat="server" id="ImgNew" src="../tools/HB_pic/adding_hb.png" alt="添加您的红包码" width="329" height="495" onclick="javascript:Upload_openBrowse();" />
        <br />
        <br />
        <asp:Button ID="UploadButton" runat="server" OnClick="UploadButton_Click" Text="上传文件" />
        &nbsp;<br />
        <br />
        <asp:Label ID="UploadStatusLabel" runat="server">文件路径：</asp:Label>
        <br />
        <br />
        <br />
        <br />
    </div>
    </form>
</body>
<script type="text/javascript">
    // 打开上传文件浏览
    function Upload_openBrowse()
    {
        var ie = navigator.appName == "Microsoft Internet Explorer" ? true : false;
        if (ie)
        {
            document.getElementById("FileUpload1").click();     // 通过document元素点击FileUpload控件
            //document.getElementById("filename").value = document.getElementById("FileUpload1").value;
        }
        else
        {
            var a = document.createEvent("MouseEvents");//FF的处理 
            a.initEvent("click", true, true);
            document.getElementById("FileUpload1").dispatchEvent(a);
        }
    }
</script> 

</html>
