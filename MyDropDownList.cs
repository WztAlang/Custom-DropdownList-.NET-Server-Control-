using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Security.Permissions;
namespace MySSDPHI
{

    [AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand,
        Level = AspNetHostingPermissionLevel.Minimal),
     DefaultProperty("DropDownCheckBoxList"),
    ToolboxData("<{0}:MyDropDownList runat=server Width='270px' Height='35px'></{0}:MyDropDownList>")]
    public class MyDropDownList : CompositeControl
    {
        public TextBox box;
        string prefix;
        String SCRIPT_ID;
        String ShowMList,HideMList,ChangeInfo,AllCheckOrNot,timeoutID,subBox,allCheckBox,divCheckBoxList;


        //组合控件包含子控件,在属性内要强制调用EnsureChildControls()
        public String generateListStr
        {
            get 
            {
                EnsureChildControls();
                return ViewState["_generateListStr"] == null ? "" : (String)ViewState["_generateListStr"]; 
            }
            set 
            {
                EnsureChildControls();
                ViewState["_generateListStr"] = value; 
            }
        }
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("270")]
        [Localizable(true)]
        public override Unit Width
        {
            set 
            {
                EnsureChildControls();
                ViewState["_Width"] = value;
            }
            get 
            {
                EnsureChildControls();
                return (ViewState["_Width"] == null) ? Unit.Pixel(270) : (Unit)ViewState["_Width"]; 
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("35")]
        [Localizable(true)]
        public override Unit Height
        {
            set
            {
                EnsureChildControls();
                ViewState["_Height"] = value;
            }
            get
            {
                EnsureChildControls();
                return (ViewState["_Height"] == null) ? Unit.Pixel(35) : (Unit)ViewState["_Height"];
            }
        }

        [Browsable(true)]
        [Bindable(true)]
        [Category("Data")]
        [Description("SelectedValues")]
        public ArrayList SelectedValues
        {
            get 
            {
                EnsureChildControls();
                return ViewState["_SelectedValues"] == null ? null : (ArrayList)ViewState["_SelectedValues"];
            }

            set
            {
                EnsureChildControls();
                ViewState["_SelectedValues"] = value; 
            }
        }

        [Browsable(true)]
        [Bindable(true)]
        [Category("Data")]
        [Description("TotalDataSource")]
        public ArrayList TotalBlocks
        {
            get 
            {
                EnsureChildControls();
                return ViewState["_TotalBlocks"] == null ? null : (ArrayList)ViewState["_TotalBlocks"]; 
            }

            set 
            {
                EnsureChildControls();
                ViewState["_TotalBlocks"] = value; 
            }
        }

        [Browsable(true)]
        [Bindable(true)]
        [Category("Data")]
        [Description("UsedDataSource")]
        public ArrayList DataSource
        {
            get 
            {
                EnsureChildControls(); 
                return (ViewState["_dataSource"] == null) ? null : (ArrayList)ViewState["_dataSource"];
            }
            set 
            {
                EnsureChildControls();
                ViewState["_dataSource"] = value;
            }
        }
        public void  initID()
        {
            prefix = this.ClientID;
            SCRIPT_ID = prefix + "_javascript";
            ShowMList = prefix + "_ShowMList()";
            HideMList = prefix + "_HideMList()";
            ChangeInfo = prefix + "_ChangeInfo()";
            AllCheckOrNot = prefix + "_AllCheckOrNot()";
            allCheckBox = prefix + "_allCheckBox";
            subBox = prefix + "_subBox";
            timeoutID = prefix + "_timeoutID";
            divCheckBoxList = prefix + "_divCheckBoxList";
        }
        protected override void CreateChildControls()
        {
            Controls.Clear();
            initID();
            box = new TextBox();
            box.ID = "in";//ClientID is delivered when ID provided  ,For that RenderJS() need ClientId so make textbox  here.
            box.AutoPostBack = true;
            box.TextChanged += Box_TextChanged;
            box.Attributes.Add("style", "position: absolute;top: 0px; left: 0px; " +
               // "width:" + Unit.Pixel((int)Width.Value) + ";" +
               // "height:" + Unit.Pixel((int)Height.Value - 6) + ";" +
                " z - index:500; ");         
            box.Attributes.Add("onfocus", ShowMList);
            this.Controls.Add(box);
        }
        private void Box_TextChanged(object sender, EventArgs e)
        {
            if (box.Text.Length == 0)
            {
                SelectedValues = null;
                return;
            }
            String[] strs = box.Text.Split(',');//"A,B,C," =>"A" "B" "C" ""
            ArrayList arr = new ArrayList();
            for (int i = 0; i < strs.Length - 1; i++)
            {
                arr.Add(strs[i]);
            }
            SelectedValues = arr;
        }
        public void clear()
        {
            if (this.box != null) this.box.Text = "";
            SelectedValues = null;
        }
        public override void DataBind()
        {
            generateListStr = "";        
            if (SelectedValues != null)
            {
                String all = "";
                if (SelectedValues.Count==DataSource.Count)
                    all = " checked=\"checked\"";
                generateListStr = "<div style=\"position:relative;align:left;\"><input type=\"checkbox\"  name=\""+allCheckBox+"\"" +all+
                 " onclick=\""+AllCheckOrNot+"\" value=\"" + allCheckBox + "\" />" + "<label style=\"position:absolute;top:0px;\">" + "All/Clear" + "</label>" + "</div>\r\n";

                for (int i = 0; i < DataSource.Count; i++)
                {
                    String s = "";
                    if (SelectedValues.Contains(DataSource[i]))
                        s = " checked=\"checked\"";
                    generateListStr += "<div style=\"position:relative;align:left;\"><input type=\"checkbox\"  name=\""+subBox+"\"" + s +
                  " onclick=\""+ChangeInfo+"\" value=\"" + DataSource[i] + "\" />" + "<label style=\"position:absolute;top:0px;\">" + DataSource[i] + "</label>" + "</div>\r\n";
                }
            }
            else
            {
                generateListStr = "<div style=\"position:relative;align:left;\"><input type=\"checkbox\"  name=\""+allCheckBox+"\" checked=\"checked\"" +
               " onclick=\""+AllCheckOrNot+"\" value=\"" + allCheckBox + "\" />" + "<label style=\"position:absolute;top:0px;\">" + "All/Clear" + "</label>" + "</div>\r\n";

                for (int i = 0; i < DataSource.Count; i++)
                {
                    String s = " checked=\"checked\"";
                    generateListStr += "<div style=\"position:relative;align:left;\"><input type=\"checkbox\"  name=\""+subBox+"\"" + s +
                  " onclick=\""+ChangeInfo+"\" value=\"" + DataSource[i] + "\" />" + "<label style=\"position:absolute;top:0px;\">" + DataSource[i] + "</label>" + "</div>\r\n";
                }
            }
           
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            initID();
            if (!Page.ClientScript.IsClientScriptBlockRegistered(SCRIPT_ID))
                RenderJS();
        }
        public void RenderJS()
        {
            String SCRIPT_CONTENT = "<script type=\"text/javascript\">\r\n" +
               " var "+timeoutID+";\r\n" +
               "function "+ShowMList+" {\r\n" +
               "  var divRef = document.getElementById(\""+divCheckBoxList+"\");\r\n" +
               "divRef.style.visibility = \"visible\";\r\n" +
               "document.getElementById(\"" + box.ClientID + "\").blur();\r\n" +
               "}\r\n" +

               " function "+HideMList+" {\r\n" +
               " if (document.getElementById(\""+divCheckBoxList+"\") != null)\r\n" +
               "document.getElementById(\"" + divCheckBoxList + "\").style.visibility = \"hidden\";\r\n " +
               "}\r\n" +

               "function "+ChangeInfo+" {\r\n" +
               "var ObjectText = \"\";\r\n" +
               "var r = document.getElementsByName(\""+subBox+"\");\r\n" +
               " for (var i = 0; i < r.length; i++)\r\n " +
               "  if (r[i].checked) ObjectText += r[i].value + \",\";   \r\n" + //A,B,C,
               " var txtbox = document.getElementById(\"" + box.ClientID + "\");\r\n" +
               "txtbox.value = ObjectText;\r\n" +
           "}\r\n" +

             "function "+AllCheckOrNot+"\r\n" +
              "{\r\n" +
                 "  var ObjectText=\"\";\r\n" +
                 "  var ch=document.getElementsByName(\""+subBox+"\");\r\n" +
                " if(document.getElementsByName(\""+allCheckBox+"\")[0].checked==true)\r\n" +
                 " {\r\n" +
                "	  for(var i=0;i<ch.length;i++)\r\n" +
                 "   {\r\n" +
                  "  	ch[i].checked=true;\r\n" +
                   " 	ObjectText+=ch[i].value+ \",\"; \r\n" +
		            " }"+
               "    }\r\n" +
                " else\r\n" +
                " {\r\n" +
                 "  	for(var i=0;i<ch.length;i++)\r\n" +
		          "     {"+
                   "        ch[i].checked=false;\r\n" +
                   "    }\r\n" +
                    "  ObjectText=\"\";\r\n" +
                "}\r\n" +
                 " var txtbox = document.getElementById(\"" + box.ClientID + "\");\r\n" +
                 "txtbox.value = ObjectText;\r\n" +
               "}\r\n" +
               "</script>\r\n";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), SCRIPT_ID, SCRIPT_CONTENT);

        }
        protected override void Render(HtmlTextWriter writer)
        {
            String Div = "<div style=\"position: relative\" onmouseover=\"clearTimeout("+timeoutID+"); \" onmouseout=\""+timeoutID+" = setTimeout('"+HideMList+"', 350); \">\r\n";
            writer.Write(Div);


            String select = " \r\n<select style=\"position: absolute;" +
           " top: 0px; left: 0px;" +
            "height:" + Height + ";" +
            "width: " + Width + ";" +
            "z - index:400; " + "\"" +
               " runat=\"server\"  onclick=\""+ShowMList+"\"></select>\r\n";
            writer.Write(select);

            box.Width = Unit.Pixel((int)Width.Value - 25);
            box.Height = Unit.Pixel((int)Height.Value - 6); ;
            box.RenderControl(writer);
      
            String checkboxList = "\r\n<div id=\""+divCheckBoxList+"\" style=\"" +//generate the hidden checkboxlist
                "position: absolute; " +
                  "top:" + Height + ";" +
                  " left:0px;" +
                "width:" + Width + ";" +
                " visibility: hidden;" +
                " border: 1px solid Gray; " +
                "background-color: White;" +
                "color:black;"+
                " height:250px;" +
                " overflow-y: auto; " +
                "overflow-x: hidden;\"  >\r\n" +
                  generateListStr +//generate checkboxlist HTML
                  "</div>\r\n" +
                "</div>\r\n";
            writer.Write(checkboxList);
        }
    }
}
