#pragma checksum "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Tools\Toolslist.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a0a62c25597aa1eabec1aac15b3572d2d38c997f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Tools_Toolslist), @"mvc.1.0.view", @"/Views/Tools/Toolslist.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Tools/Toolslist.cshtml", typeof(AspNetCore.Views_Tools_Toolslist))]
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
#line 3 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Tools\Toolslist.cshtml"
using DataTables.AspNetCore.Mvc;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a0a62c25597aa1eabec1aac15b3572d2d38c997f", @"/Views/Tools/Toolslist.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e50bf59ee35ef0f48e0d229ad9de8458361f9630", @"/Views/_ViewImports.cshtml")]
    public class Views_Tools_Toolslist : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<MyavanaAdminModels.ToolsModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/loader.gif"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/admin/tools.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(208, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 6 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Tools\Toolslist.cshtml"
  
    ViewData["Title"] = "Toolslist";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(302, 4420, true);
            WriteLiteral(@"
<style>
    .preloader {
        width: calc(100% - 200px);
        height: 100%;
        top: 0px;
        position: fixed;
        z-index: 99999;
        background: rgba(255, 255, 255, .8);
    }

    table#ProductList.clsProduct th:nth-child(6) {
        min-width: 120px !important;
        max-width: 120px !important;
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
        box-shadow: 0px 0px 12px 3px rgba(0, 0, 0, 0.2);");
            WriteLiteral(@"
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
        color: #1c6356;
        background-color: #a8ffc3;
        border-color: #13b755;
    }

        .alert-success hr {
            border-top-color: #b4e7dd;
        }

        .alert-success .alert-link {
            color: #113b33;
        }

    span.align-right {
        text-align: right;
    }

    .productClass {
        width: 100%
    }

    .addnewbtn {
        padding: 20px 10px 0;
    }

    div#ProductList_filter input[type=search] {
        border: 1px solid #ccc;
    }

    i.fa.fa-pencil {
        color: #1ab394;
        background: #e2e2e2;
        padding: 7px;
        border-radius: 4px;
    }

        i.fa.fa-pen");
            WriteLiteral(@"cil:hover {
            background: #cecece
        }

    i.fa.fa-trash:hover {
        background: #cecece
    }

    i.fa.fa-trash {
        color: red;
        background: #e2e2e2;
        padding: 7px;
        border-radius: 4px;
        margin-top: 10px;
    }

    tr.odd td {
        background: #f8f8f8 !important;
    }

    tr.odd:hover td, tr.even:hover td {
        background: #eee !important;
    }

    tr.even td {
        background: #fff !important;
    }

    #ProductList tr td {
        padding: 8px 10px;
        vertical-align: top;
    }

    #ProductList tr th {
        white-space: nowrap !important;
        padding: 8px 18px 8px 10px;
    }

    table#ProductList th:nth-child(1) {
        min-width: 80px !important;
    }

    table#ProductList th:nth-child(2) {
        min-width: 80px !important;
    }

    table#ProductList th:nth-child(3) {
        min-width: 80px !important;
    }

    table#ProductList th:nth-child(4) {
        min-");
            WriteLiteral(@"width: 80px !important;
    }

    table#ProductList th:nth-child(5) {
        min-width: 80px !important;
    }

    table#ProductList th:nth-child(6) {
        min-width: 250px !important;
        max-width: 300px !important
    }

    table#ProductList th:nth-child(7) {
        min-width: 400px !important;
    }

    table#ProductList th:nth-child(8) {
        min-width: 400px !important;
    }

    table#ProductList.no-footer {
        border-bottom: none !important;
    }

    table#ProductList tr td:nth-child(6) {
        word-break: break-all;
    }

    .ibox-content table {
        display: block;
        width: 100%;
        overflow-x: auto;
        overflow-y: auto;
        -webkit-overflow-scrolling: touch;
        height: calc(100vh - 385px);
    }
    /* td.commonlist span {
        border: 1px solid #939393;
        padding: 0px 8px;
        margin: 4px 7px 4px 0px !important;
        white-space: nowrap;
        line-height: 26px;
        background: ");
            WriteLiteral(@"#fff;
        display: inline-block;
    }*/
</style>

<script src=""//cdn.rawgit.com/rainabba/jquery-table2excel/1.1.0/dist/jquery.table2excel.min.js""></script>
<link rel=""stylesheet"" type=""text/css"" href=""https://cdn.datatables.net/v/dt/dt-1.10.16/b-1.4.2/b-html5-1.4.2/b-print-1.4.2/sl-1.2.3/datatables.min.css"" />
");
            EndContext();
            BeginContext(4798, 245, true);
            WriteLiteral("<script type=\"text/javascript\" src=\"https://cdn.datatables.net/v/dt/dt-1.10.16/b-1.4.2/b-html5-1.4.2/b-print-1.4.2/sl-1.2.3/datatables.min.js\"></script>\r\n<div class=\"addnewbtn\">\r\n    <span class=\"align-right\">\r\n        <a class=\"btn btn-primary\"");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 5043, "\"", 5084, 1);
#line 212 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Tools\Toolslist.cshtml"
WriteAttributeValue("", 5050, Url.Action("CreateTool", "Tools"), 5050, 34, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(5085, 663, true);
            WriteLiteral(@">Add New Tool</a>
    </span>
</div>
<div id=""alert-success"" class=""alert alert-success alert-dismissible"" style=""display:none; width:50%"" data-keyboard=""false"" data-backdrop=""static"">
    <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a>
    <span id=""successMessage""></span>
</div>

<div class=""alert alert-danger alert-dismissible"" style=""display:none; width:50%"" data-keyboard=""false"" data-backdrop=""static"">
    <a href=""#"" class=""close"" data-dismiss=""alert"" aria-label=""close"">&times;</a>
    <span id=""failureMessage""></span>
</div>

<div class=""preloader"" style=""display:none;"">
    <div class=""loader"">
        ");
            EndContext();
            BeginContext(5748, 33, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "a0a62c25597aa1eabec1aac15b3572d2d38c997f10685", async() => {
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
            BeginContext(5781, 105, true);
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<div class=\"wrapper wrapper-content animated fadeInRight\">\r\n    <div class=\"row\">\r\n");
            EndContext();
            BeginContext(5996, 850, true);
            WriteLiteral(@"        <div class=""col-lg-12"">
            <div class=""ibox "">
                <div class=""ibox-title"">
                    <h5>Tools</h5>
                    <div class=""ibox-tools d-flex"">

                        <input type=""file"" id=""excelUpload"" accept=""*"" onchange=""changeImage(this)"" style=""display:none;"" />
                        <a class=""dropdown-item p-1"" title=""Upload Products"" id=""excelUploadChange"" onclick=""UploadSelectedExcelsheet()"">
                            <i class=""fa fa-upload"" aria-hidden=""true""></i>
                        </a>
                        <a class=""dropdown-item p-1"" title=""Download Template"" id=""excelDownloadChange""><i class=""fa fa-download"" aria-hidden=""true""></i></a>

                    </div>
                </div>
                <div class=""ibox-content"">

                    ");
            EndContext();
            BeginContext(6848, 1933, false);
#line 249 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Tools\Toolslist.cshtml"
                Write(Html.Ext().Grid<MyavanaAdminModels.ToolsModel>().Name("ToolsList").ClassName("clsProduct")
                                                        .Columns(cols =>
                                                        {
                                                            cols.Add(c => c.ToolName).Title("Tool Name");
                                                            cols.Add(c => c.ActualName).Title("Actual Name");
                                                            cols.Add(c => c.BrandName).Title("Brand Name");
                                                            cols.Add(c => c.Image).Title("Image").Render(() => "onRenderImage").Searchable(false);
                                                            cols.Add(c => c.ToolLink).Title("Link").Searchable(false);
                                                            cols.Add(c => c.ToolDetails).Title("Tools Details").Searchable(false);
                                                            cols.Add(c => c.Price).Title("Price");
                                                            cols.Add(c => c.Id).Render(() => "onRender").Orderable(false);
                                                        })
                                                        .ServerSide(false)
                                                        .Ordering(false)
                                                        .DeferRender(true)
                                                        .Paging(true)
                                                        .Searching(true)
                                                        .Info(false)
                                                        .DataSource(c =>
                                                        c.Ajax().Url("GetProductsList").Method("Get")
                                                        )
                    );

#line default
#line hidden
            EndContext();
            BeginContext(8782, 1082, true);
            WriteLiteral(@"

                </div>
            </div>
        </div>
    </div>
</div>

<div class=""modal fade"" id=""confirmModal"" tabindex=""-1"" role=""dialog"" aria-labelledby=""confirmModal"" aria-hidden=""true"" data-keyboard=""false"" data-backdrop=""static"">
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h5 id=""confirmModalHeader"" class=""font-bold"">Delete</h5>
                <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                    <span aria-hidden=""true"">&times;</span>
                </button>
            </div>
            <div class=""modal-body"">
                <span id=""confirmModalText""></span>
            </div>
            <div class=""modal-footer"">
                <button type=""button"" class=""btn btn-secondary"" id=""cancelMethod"" data-dismiss=""modal"">Cancel</button>
                <button type=""button"" class=""btn btn-primary"" id=""confirmMethod"">Save</button>
  ");
            WriteLiteral("          </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n\r\n");
            EndContext();
            BeginContext(9864, 43, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a0a62c25597aa1eabec1aac15b3572d2d38c997f16433", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(9907, 113, true);
            WriteLiteral("\r\n<script>\r\n    $(document).ready(function () {\r\n        $(\"input[type=\'search\']\").val(\"\");\r\n    });\r\n</script>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<MyavanaAdminModels.ToolsModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
