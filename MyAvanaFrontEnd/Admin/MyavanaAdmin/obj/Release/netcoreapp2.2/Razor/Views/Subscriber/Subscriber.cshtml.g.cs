#pragma checksum "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Subscriber\Subscriber.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5414dc1894b87a3923ba8482b9e6ef6734760138"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Subscriber_Subscriber), @"mvc.1.0.view", @"/Views/Subscriber/Subscriber.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Subscriber/Subscriber.cshtml", typeof(AspNetCore.Views_Subscriber_Subscriber))]
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
#line 3 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Subscriber\Subscriber.cshtml"
using DataTables.AspNetCore.Mvc;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5414dc1894b87a3923ba8482b9e6ef6734760138", @"/Views/Subscriber/Subscriber.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e50bf59ee35ef0f48e0d229ad9de8458361f9630", @"/Views/_ViewImports.cshtml")]
    public class Views_Subscriber_Subscriber : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<MyavanaAdminModels.SubscriberModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/admin/subscriber.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 5 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Subscriber\Subscriber.cshtml"
  
    Layout = "_Layout";

#line default
#line hidden
            BeginContext(245, 2712, true);
            WriteLiteral(@"<style>
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

    .preloader {
 ");
            WriteLiteral(@"       width: calc(100% - 200px);
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

    span.align-right {
        text-align: right;
    }

    .addnewbtn {
        padding: 20px 10px 0;
    }

    i.fa.fa-pencil {
        color: #1ab394;
        background: #e2e2e2;
        padding: 7px;
        border-radius: 4px;
    }

        i.fa.fa-pencil:hover {
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
     ");
            WriteLiteral(@"   margin-top: 10px;
    }

    div#QuestionnaireCustomerList_filter input[type=search] {
        border: 1px solid #ccc;
    }

    i.fa.fa-pencil {
        color: #1ab394;
    }

    i.fa.fa-trash {
        color: red;
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

    .viewuser {
        background: #eee;
        padding: 5px 7px;
        border-radius: 4px;
        border: 1px solid #ccc;
        margin-top: 5px;
        color: #1ab394;
    }
</style>
");
            EndContext();
            BeginContext(2957, 48, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5414dc1894b87a3923ba8482b9e6ef67347601386937", async() => {
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
            BeginContext(3005, 436, true);
            WriteLiteral(@"
<script>
    $(document).ready(function () {
        $(""input[type='search']"").val("""");
    });
</script>
<div class=""wrapper wrapper-content animated fadeInRight"">
    <div class=""row"">
        <div class=""col-lg-12"">
            <div class=""ibox "">
                <div class=""ibox-title"">
                    <h5>Subscribers</h5>
                </div>
                <div class=""ibox-content"">

                    ");
            EndContext();
            BeginContext(3443, 966, false);
#line 155 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Subscriber\Subscriber.cshtml"
                Write(Html.Ext().Grid<MyavanaAdminModels.SubscriberModel>().Name("SubscriberList")
                                .Columns(cols =>
                                {
                                    cols.Add(c => c.UserName).Title("User Name");
                                    cols.Add(c => c.UserEmail).Title("User Email");
                                    cols.Add(c => c.ProviderName).Title("Subscription Type");
                                    cols.Add(c => c.UserEmail).Render(() => "onRender");
                                })

                                .ServerSide(false)
                                .Ordering(false)
                                .Paging(true)
                                .Searching(true)
                                .Info(false)
                                .DataSource(c =>
                                c.Ajax().Url("GetSubscriberList").Method("Get")
                                )
	);

#line default
#line hidden
            EndContext();
            BeginContext(4410, 1080, true);
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
                <button type=""button"" class=""btn btn-primary"" id=""confirmMethod"">Save</button>");
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<MyavanaAdminModels.SubscriberModel> Html { get; private set; }
    }
}
#pragma warning restore 1591