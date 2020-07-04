<%@ Page Title="" Language="C#" MasterPageFile="~/PayFor/PayForMaster.Master" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="QrPaySystem.PayFor.UserInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentBodyHoder" runat="server">

    <div style="margin: 0; padding: 0;">
        <div style="margin: 0; padding: 0;">
            <h2 class="subheader">管理账号</h2>
        </div>
        <h3 class="information-list-title">账号信息</h3>
        <div style="margin: 0; padding: 0;">
            <div style="margin: 0; padding: 0;">
                <div style="margin: 0; padding: 0;">
                    开发者姓名</div>
                <div id="company_name" style="margin: 0; padding: 0;">
                王忠愿                <a href="https://open.cocos.com/user/edit" style="outline: none; blr: expression(this.onFocus=this.blur()); font-style: normal; font-variant: normal; font-weight: normal; font-size: 12px; line-height: 1.125; font-family: 'Helvetica Neue', Helvetica, STheiti, Arial, Tahoma, 微软雅黑, sans-serif, serif; color: #2d97f9; text-decoration: none;">修改姓名</a>
                </div>
            </div>
            <div style="margin: 0; padding: 0;">
                <div style="margin: 0; padding: 0;">
                    联系地址</div>
                <div id="contact_name" style="margin: 0; padding: 0;">
                    安徽&nbsp;&nbsp;合肥市&nbsp;&nbsp;合作化路与休宁路交口安粮城广场5栋2101                <a href="https://open.cocos.com/user/edit" style="outline: none; blr: expression(this.onFocus=this.blur()); font-style: normal; font-variant: normal; font-weight: normal; font-size: 12px; line-height: 1.125; font-family: 'Helvetica Neue', Helvetica, STheiti, Arial, Tahoma, 微软雅黑, sans-serif, serif; color: #2d97f9; text-decoration: none;">修改地址</a>
                </div>
            </div>
            <div style="margin: 0; padding: 0;">
                <div style="margin: 0; padding: 0;">
                    QQ</div>
                <div id="tel" style="margin: 0; padding: 0;">
                536400495                <a href="https://open.cocos.com/user/edit" style="outline: none; blr: expression(this.onFocus=this.blur()); font-style: normal; font-variant: normal; font-weight: normal; font-size: 12px; line-height: 1.125; font-family: 'Helvetica Neue', Helvetica, STheiti, Arial, Tahoma, 微软雅黑, sans-serif, serif; color: #2d97f9; text-decoration: none;">修改QQ</a>
                </div>
            </div>
            <div style="margin: 0; padding: 0;">
                <div style="margin: 0; padding: 0;">
                    手机号码</div>
                <div id="address" style="margin: 0; padding: 0;">
                187****0776                <a href="https://open.cocos.com/user/mobile_edit" style="outline: none; blr: expression(this.onFocus=this.blur()); font-style: normal; font-variant: normal; font-weight: normal; font-size: 12px; line-height: 1.125; font-family: 'Helvetica Neue', Helvetica, STheiti, Arial, Tahoma, 微软雅黑, sans-serif, serif; color: #2d97f9; text-decoration: none;">修改手机号码</a>
                </div>
            </div>
        </div>
        <h3 class="information-list-title">个人设置</h3>
        <div style="margin: 0; padding: 0;">
            <div style="margin: 0; padding: 0;">
                <div style="margin: 0; padding: 0;">
                    账号</div>
                <div style="margin: 0; padding: 0;">
                536400495@qq.com                <a href="https://open.cocos.com/user/change_email" style="outline: none; blr: expression(this.onFocus=this.blur()); font-style: normal; font-variant: normal; font-weight: normal; font-size: 12px; line-height: 1.125; font-family: 'Helvetica Neue', Helvetica, STheiti, Arial, Tahoma, 微软雅黑, sans-serif, serif; color: #2d97f9; text-decoration: none;">更换邮箱</a>
                </div>
            </div>
            <div style="margin: 0; padding: 0;">
                <div style="margin: 0; padding: 0;">
                    用户名</div>
                <div style="margin: 0; padding: 0;">
                scimence                            
                </div>
            </div>
        </div>
        <h3 class="information-list-title">安全设置</h3>
        <div style="margin: 0; padding: 0;">
            <div style="margin: 0; padding: 0;">
                <div style="margin: 0; padding: 0;">
                    登录密码</div>
                <div style="margin: 0; padding: 0;">
                    已设置登录密码<a href="https://open.cocos.com/user/change_password" style="outline: none; blr: expression(this.onFocus=this.blur()); font-style: normal; font-variant: normal; font-weight: normal; font-size: 12px; line-height: 1.125; font-family: 'Helvetica Neue', Helvetica, STheiti, Arial, Tahoma, 微软雅黑, sans-serif, serif; color: #2d97f9; text-decoration: none;">修改密码</a></div>
            </div>
            <div style="margin: 0; padding: 0;">
                <div style="margin: 0; padding: 0;">
                    支付密码</div>
                <div id="contact_name0" style="margin: 0; padding: 0;">
                    未设置支付密码
                                            <a href="https://open.cocos.com/paypassword/set" style="outline: none; blr: expression(this.onFocus=this.blur()); font-style: normal; font-variant: normal; font-weight: normal; font-size: 12px; line-height: 1.125; font-family: 'Helvetica Neue', Helvetica, STheiti, Arial, Tahoma, 微软雅黑, sans-serif, serif; color: #2d97f9; text-decoration: none;">添加支付密码</a>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
