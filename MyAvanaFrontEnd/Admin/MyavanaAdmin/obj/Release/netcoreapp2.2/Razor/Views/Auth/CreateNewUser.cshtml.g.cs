#pragma checksum "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6d660b98dfb6e93cd30a94298075e7e3c082ee7b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Auth_CreateNewUser), @"mvc.1.0.view", @"/Views/Auth/CreateNewUser.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Auth/CreateNewUser.cshtml", typeof(AspNetCore.Views_Auth_CreateNewUser))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\_ViewImports.cshtml"
using MyavanaAdmin;

#line default
#line hidden
#line 2 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\_ViewImports.cshtml"
using MyavanaAdmin.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6d660b98dfb6e93cd30a94298075e7e3c082ee7b", @"/Views/Auth/CreateNewUser.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e50bf59ee35ef0f48e0d229ad9de8458361f9630", @"/Views/_ViewImports.cshtml")]
    public class Views_Auth_CreateNewUser : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<MyavanaAdminModels.WebLoginModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/loader.gif"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "text", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("txtEmail"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("placeholder", new global::Microsoft.AspNetCore.Html.HtmlString("Enter email"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("selectUserType"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("onchange", new global::Microsoft.AspNetCore.Html.HtmlString("setUserType()"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("js-example-basic-multiple form-control select2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_9 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("OwnerSalonId"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_10 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-white btn-sm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_11 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Auth", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_12 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Users", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_13 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/admin/Users.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(301, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
#line 8 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
  
    ViewData["Title"] = "CreateNewUser";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(401, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 13 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
 if (Model != null)
{

#line default
#line hidden
            BeginContext(427, 36, true);
            WriteLiteral("    <input type=\"hidden\" id=\"userId\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 463, "\"", 484, 1);
#line 15 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
WriteAttributeValue("", 471, Model.UserId, 471, 13, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(485, 5, true);
            WriteLiteral(" />\r\n");
            EndContext();
#line 17 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
     if (Model.UserType == true)
    {

#line default
#line hidden
            BeginContext(533, 57, true);
            WriteLiteral("        <input type=\"hidden\" id=\"userType\" value=\"1\" />\r\n");
            EndContext();
#line 20 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
    }
    else
    {

#line default
#line hidden
            BeginContext(614, 57, true);
            WriteLiteral("        <input type=\"hidden\" id=\"userType\" value=\"0\" />\r\n");
            EndContext();
#line 24 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
    }

#line default
#line hidden
#line 24 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
     
}

#line default
#line hidden
            BeginContext(681, 2410, true);
            WriteLiteral(@"<link href=""https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/css/select2.min.css"" rel=""stylesheet"" />
<script src=""https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/js/select2.min.js""></script>
<script>
    $(document).ready(function () {

    });
</script>
<style>
    .alert-success, .alert-danger, .alert-info {
        top: 110px !important;
        width: 60% !important;
        margin: 0 auto;
        z-index: 99;
        position: fixed;
        left: 50%;
        transform: translateX(-40%);
        text-align: center;
        padding: 12px;
        transition: all 1s linear;
        box-shadow: 0px 0px 12px 3px rgba(0, 0, 0, 0.2);
        font-size: 16px;
    }

    .alert-danger {
        color: #80172a;
        background-color: #fdd5dc;
        border-color: #fcc4ce;
    }

        .alert-danger hr {
            border-top-color: #fbacba;
        }

        .alert-danger .alert-link {
            color: #550f1c;
        }

    .alert-success {
        color: #1c6");
            WriteLiteral(@"356;
        background-color: #a8ffc3;
        border-color: #13b755;
    }

        .alert-success hr {
            border-top-color: #b4e7dd;
        }

        .alert-success .alert-link {
            color: #113b33;
        }

    .preloader {
        width: calc(100% - 200px);
        height: 100%;
        top: 0px;
        position: fixed;
        z-index: 99999;
        background: rgba(255, 255, 255, .8);
    }

        .preloader img {
            position: absolute;
            LEFT: 50%;
            top: 50%;
            transform: translate(-50%, -50%);
            width: 80px;
        }

    .loader {
        overflow: visible;
        padding-top: 2em;
        height: 0;
        width: 2em;
    }
</style>
<div id=""alert-success"" class=""alert alert-success alert-dismissible"" style=""display:none; width:50%"" data-keyboard=""false"" data-backdrop=""static"">
    <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a>
    <span id=""successMess");
            WriteLiteral(@"age""></span>
</div>
<div class=""alert alert-danger alert-dismissible"" style=""display:none; width:50%"" data-keyboard=""false"" data-backdrop=""static"">
    <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a>
    <span id=""failureMessage""></span>
</div>
<div class=""preloader"" style=""display:none;"">
    <div class=""loader"">
        ");
            EndContext();
            BeginContext(3091, 33, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "6d660b98dfb6e93cd30a94298075e7e3c082ee7b13413", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3124, 323, true);
            WriteLiteral(@"
    </div>
</div>
<div class=""wrapper wrapper-content animated fadeInRight"">
    <div class=""row"">
        <div class=""col-lg-12"">
            <div class=""ibox "">
                <div class=""ibox-title"">
                    <h5>Post User</h5>

                </div>
                <div class=""ibox-content"">
");
            EndContext();
            BeginContext(3493, 212, true);
            WriteLiteral("                    <div class=\"form-group  row\">\r\n                        <label class=\"col-sm-2 col-form-label\">User Email*</label>\r\n                        <div class=\"col-sm-10\">\r\n                            ");
            EndContext();
            BeginContext(3705, 100, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "6d660b98dfb6e93cd30a94298075e7e3c082ee7b15245", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
#line 127 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.UserEmail);

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3805, 62, true);
            WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n");
            EndContext();
#line 132 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
                         if (HttpContextAccessor.HttpContext.Request.Cookies["UserTypeId"].ToString() == "1")
                        {

#line default
#line hidden
            BeginContext(4073, 242, true);
            WriteLiteral("                            <div class=\"form-group row\">\r\n                                <label class=\"col-sm-2 col-form-label\">User Type*</label>\r\n                                <div class=\"col-sm-10\">\r\n                                    ");
            EndContext();
            BeginContext(4315, 345, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("select", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6d660b98dfb6e93cd30a94298075e7e3c082ee7b17973", async() => {
                BeginContext(4571, 80, true);
                WriteLiteral("\r\n                                        \r\n                                    ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
#line 138 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items = Salons.GetUserTypeList().Result.Select(x => new SelectListItem() { Text = x.Description, Value = x.UserTypeId.ToString() });

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-items", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(4660, 114, true);
            WriteLiteral("\r\n                                  \r\n                                </div>\r\n                            </div>\r\n");
            EndContext();
#line 144 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
                          }

                        //}
                        

#line default
#line hidden
            BeginContext(4937, 273, true);
            WriteLiteral(@"                <div id=""divSalonOwner"">
                    <div class=""form-group  row"">
                        <label class=""col-sm-2 col-form-label"" id=""lblSalonOwner"">Salon Owner</label>
                        <div class=""col-sm-10"">
                            ");
            EndContext();
            BeginContext(5210, 358, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("select", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6d660b98dfb6e93cd30a94298075e7e3c082ee7b20778", async() => {
                BeginContext(5457, 34, true);
                WriteLiteral("\r\n                                ");
                EndContext();
                BeginContext(5491, 38, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6d660b98dfb6e93cd30a94298075e7e3c082ee7b21197", async() => {
                    BeginContext(5508, 12, true);
                    WriteLiteral("Select Salon");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_7.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(5529, 30, true);
                WriteLiteral("\r\n                            ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_8);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_9);
#line 154 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items = Salons.GetSalonList().Result.Select(x => new SelectListItem() { Text = x.SalonName, Value = x.SalonId.ToString() });

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-items", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.Items, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(5568, 86, true);
            WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n");
            EndContext();
            BeginContext(5687, 216, true);
            WriteLiteral("                        <div class=\"hr-line-dashed\"></div>\r\n\r\n                        <div class=\"form-group row\">\r\n                            <div class=\"col-sm-4 col-sm-offset-2\">\r\n                                ");
            EndContext();
            BeginContext(5903, 83, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6d660b98dfb6e93cd30a94298075e7e3c082ee7b24626", async() => {
                BeginContext(5976, 6, true);
                WriteLiteral("Cancel");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_10);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_11.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_11);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_12.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_12);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(5986, 197, true);
            WriteLiteral("\r\n                                <button class=\"btn btn-primary btn-sm btnColorChange\" onclick=\"SaveUser()\">Save user</button>\r\n                            </div>\r\n                        </div>\r\n");
            EndContext();
            BeginContext(6220, 84, true);
            WriteLiteral("                    </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
            EndContext();
            BeginContext(6304, 43, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6d660b98dfb6e93cd30a94298075e7e3c082ee7b26695", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_13);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(6347, 243, true);
            WriteLiteral("\r\n<script>\r\n    var userSalonsList = [];\r\n    var salonsList = [];\r\n    var userList = [];\r\n    var showSalons = false;\r\n    var selectUserType = null;\r\n    $(document).ready(function () {\r\n        $(\"#divSalonOwner\").css(\"display\", \"none\");\r\n");
            EndContext();
#line 184 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
         if (Model != null)
        {   

#line default
#line hidden
            BeginContext(6633, 12, true);
            WriteLiteral("            ");
            EndContext();
            BeginContext(6647, 17, true);
            WriteLiteral("selectUserType = ");
            EndContext();
            BeginContext(6665, 16, false);
#line 186 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
                          Write(Model.UserTypeId);

#line default
#line hidden
            EndContext();
            BeginContext(6681, 3, true);
            WriteLiteral(";\r\n");
            EndContext();
#line 187 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
            
            if (Model.UserTypeId == 2)
            {

#line default
#line hidden
            BeginContext(6753, 16, true);
            WriteLiteral("                ");
            EndContext();
            BeginContext(6771, 20, true);
            WriteLiteral("showSalons = true;\r\n");
            EndContext();
#line 191 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
            }
            var stylishSpecialtyLength = @Model.userSalons.Count();
            if (stylishSpecialtyLength > 0) {
                        

#line default
#line hidden
#line 194 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
                         foreach (var d in Model.userSalons)
                        {

#line default
#line hidden
            BeginContext(7011, 28, true);
            WriteLiteral("                            ");
            EndContext();
            BeginContext(7041, 21, true);
            WriteLiteral("userSalonsList.push(\"");
            EndContext();
            BeginContext(7063, 9, false);
#line 196 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
                                              Write(d.SalonId);

#line default
#line hidden
            EndContext();
            BeginContext(7072, 5, true);
            WriteLiteral("\");\r\n");
            EndContext();
#line 197 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
                        }

#line default
#line hidden
#line 197 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Auth\CreateNewUser.cshtml"
                         
                }
            }

#line default
#line hidden
            BeginContext(7138, 1067, true);
            WriteLiteral(@"        if (showSalons) {
            $(""#divSalonOwner"").css(""display"", ""block"");
        }
        if (selectUserType) {
            $('#selectUserType').val(selectUserType);
        }
        if (userSalonsList.length > 0) {
            for (var i = 0; i < userSalonsList.length; i++) {
                    salonsList.push(userSalonsList[i]);
            }
            showSalons = true;
              $('.select2').val(salonsList);
              $('.select2').trigger('change');
            }
        $('.select2').select2({
            placeholder: ""--Select--""
        });

        $('.preloader').css('display', 'block');
        $.ajax({
            type: ""GET"",
            url: ""/Auth/GetAllUsers"",
            async: true,
            success: function (response) {
                userList = response;
                $('.preloader').css('display', 'none');
            },
            error: function (response) {

            },
            complete: function () {

            }");
            WriteLiteral("\r\n        });\r\n        });\r\n\r\n</script>\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public MyavanaAdminApiClient.ApiClient Salons { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Microsoft.AspNetCore.Http.IHttpContextAccessor HttpContextAccessor { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<MyavanaAdminModels.WebLoginModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
