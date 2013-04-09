<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <div id="flashContent">

			<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" width="550" height="400" id="AliveChess" align="middle">

				<param name="movie" value="AliveChess.swf" />

				<param name="quality" value="high" />

				<param name="bgcolor" value="#ffffff" />

				<param name="play" value="true" />

				<param name="loop" value="true" />

				<param name="wmode" value="window" />

				<param name="scale" value="showall" />

				<param name="menu" value="true" />

				<param name="devicefont" value="false" />

				<param name="salign" value="" />

				<param name="allowScriptAccess" value="sameDomain" />

				<!--[if !IE]>-->

				<object type="application/x-shockwave-flash" data="AliveChess.swf" width="550" height="400">

					<param name="movie" value="AliveChess.swf" />

					<param name="quality" value="high" />

					<param name="bgcolor" value="#ffffff" />

					<param name="play" value="true" />

					<param name="loop" value="true" />

					<param name="wmode" value="window" />

					<param name="scale" value="showall" />

					<param name="menu" value="true" />

					<param name="devicefont" value="false" />

					<param name="salign" value="" />

					<param name="allowScriptAccess" value="sameDomain" />

				<!--<![endif]-->

					<a href="http://www.adobe.com/go/getflash">

						<img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="Загрузить Adobe Flash Player" />

					</a>

				<!--[if !IE]>-->

				</object>

				<!--<![endif]-->

			</object>

		</div>

</asp:Content>
