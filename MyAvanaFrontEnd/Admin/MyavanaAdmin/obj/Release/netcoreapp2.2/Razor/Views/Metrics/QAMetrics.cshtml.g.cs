#pragma checksum "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Metrics\QAMetrics.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "14fe495e5d4eff7e600009dbd3059d754ca894f9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Metrics_QAMetrics), @"mvc.1.0.view", @"/Views/Metrics/QAMetrics.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Metrics/QAMetrics.cshtml", typeof(AspNetCore.Views_Metrics_QAMetrics))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"14fe495e5d4eff7e600009dbd3059d754ca894f9", @"/Views/Metrics/QAMetrics.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e50bf59ee35ef0f48e0d229ad9de8458361f9630", @"/Views/_ViewImports.cshtml")]
    public class Views_Metrics_QAMetrics : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<MyavanaAdminModels.QuestionGraph>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/chosen.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 4 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Metrics\QAMetrics.cshtml"
  
    ViewData["Title"] = "QAMetrics";
    Layout = "~/Views/Shared/_Layout.cshtml";
    var userId = HttpContextAccessor.HttpContext.Request.Cookies["UserId"].ToString();
    var questionList = questionaire.GetQuestionsForGraph(userId).Result;

#line default
#line hidden
            BeginContext(425, 380, true);
            WriteLiteral(@"<style>
    .chart_heading {
        font-size: 20px;
        width: 100%;
        /*overflow: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;*/
        padding: 0 20px;
        margin-bottom: 0;
        min-height: 48px;
    }
</style>
<h2>Analytics</h2>
<script type=""text/javascript"" src=""https://www.gstatic.com/charts/loader.js""></script>
");
            EndContext();
            BeginContext(805, 49, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "14fe495e5d4eff7e600009dbd3059d754ca894f94888", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(854, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(907, 127, true);
            WriteLiteral("<div class=\"d-flex w-100\">\r\n    <div class=\"main-layout-job\">\r\n        <div class=\"col-md-12\">\r\n            <div class=\"row\">\r\n");
            EndContext();
#line 30 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Metrics\QAMetrics.cshtml"
                 if (questionList.Count > 0)
                {
                    

#line default
#line hidden
#line 32 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Metrics\QAMetrics.cshtml"
                     foreach (var quest in questionList)
                    {

#line default
#line hidden
            BeginContext(1180, 236, true);
            WriteLiteral("                        <div class=\"col-lg-6 mb-4\">\r\n                            <div class=\"card card-cascade narrower overflow-hidden crate-page-outer-style min-card-height \">\r\n                                <h2 class=\"chart_heading\"");
            EndContext();
            BeginWriteAttribute("title", " title=\"", 1416, "\"", 1439, 1);
#line 36 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Metrics\QAMetrics.cshtml"
WriteAttributeValue("", 1424, quest.Question, 1424, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1440, 1, true);
            WriteLiteral(">");
            EndContext();
            BeginContext(1442, 14, false);
#line 36 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Metrics\QAMetrics.cshtml"
                                                                             Write(quest.Question);

#line default
#line hidden
            EndContext();
            BeginContext(1456, 81, true);
            WriteLiteral("</h2>\r\n                                <hr>\r\n                                <div");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1537, "\"", 1566, 2);
            WriteAttributeValue("", 1542, "rating_", 1542, 7, true);
#line 38 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Metrics\QAMetrics.cshtml"
WriteAttributeValue("", 1549, quest.QuestionId, 1549, 17, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1567, 95, true);
            WriteLiteral(" style=\"width: 500px; height: 420px; margin: 0 auto\">\r\n                                </div>\r\n");
            EndContext();
            BeginContext(1755, 68, true);
            WriteLiteral("                            </div>\r\n                        </div>\r\n");
            EndContext();
#line 43 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Metrics\QAMetrics.cshtml"
                    }

#line default
#line hidden
#line 43 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Metrics\QAMetrics.cshtml"
                     
                }

#line default
#line hidden
            BeginContext(1865, 308, true);
            WriteLiteral(@"
            </div>
        </div>
    </div>
</div>
<script type=""text/javascript"">
    google.charts.load('current', { packages: ['corechart'] });
    google.charts.load('current', { packages: ['bar'] });

    function drawChart() {
        // Define the chart to be drawn.
        var result = ");
            EndContext();
            BeginContext(2174, 38, false);
#line 56 "E:\Work2023\October2023\MyAvana\MyAvanaAdminV2\MyavanaAdmin\MyavanaAdmin\Views\Metrics\QAMetrics.cshtml"
                Write(Html.Raw(Json.Serialize(questionList)));

#line default
#line hidden
            EndContext();
            BeginContext(2212, 5018, true);
            WriteLiteral(@";
        for (var i = 0; i < result.length; i++) {
            var answerCount = result[i].answerCounts;
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Browser');
            data.addColumn('number', 'Percentage');

            // Set chart options
            if (result[i].questionId == 1 || result[i].questionId == 3 || result[i].questionId == 4 || result[i].questionId == 8 || result[i].questionId == 24) {
                var options = {
                    pieHole: 0.8, pieSliceText: 'value', is3D: false,
                    colors: ['#ebc7c7', '#efdbd4', '#f1e0da', '#f8f0ed'],
                    chartArea: {
                        left: 10,
                        right: 0, // !!! works !!!
                        bottom: 10,  // !!! works !!!
                        top: 10,
                        width: ""100%"",
                        height: ""100%""
                    },
                     pieSliceTextStyle: {
               ");
            WriteLiteral(@"         color: '#383838',
                    }
                }
                for (var j = 0; j < answerCount.length; j++) {
                    var temp = [];
                    temp.push(answerCount[j].answer);
                    temp.push(answerCount[j].count);

                    data.addRow(temp);
                }
                var chart = new google.visualization.PieChart(document.getElementById('rating_' + result[i].questionId));
                chart.draw(data, options);
            }else  if (result[i].questionId == 14 || result[i].questionId == 15 || result[i].questionId == 16 || result[i].questionId == 19 || result[i].questionId == 25 || result[i].questionId == 33) {
                var options = {
                    colors: ['#ebc7c7', '#efdbd4', '#f1e0da', ""#f8f0ed"", '#bababa',  ""#646464""],
                    ""hAxis"": { showTextEvery: 1, slantedText: true, slantedTextAngle: 90 },
                    ""vAxis"": { showTextEvery: 1, slantedText: true, slantedTextAngle: 90,");
            WriteLiteral(@" viewWindow: { max: answerCount.length } },
                    ""legend"": ""none"",
                    ""is3D"": true
                };
                for (var j = 0; j < answerCount.length; j++) {
                    var temp = [];
                    var answerText = answerCount[j].answer
                    if (answerText.length > 10) {
                        answerText = answerText.substring(0, 10);
                    }
                    temp.push(answerText);
                    temp.push(answerCount[j].count);

                    data.addRow(temp);
                }
                var chart = new google.visualization.BarChart(document.getElementById('rating_' + result[i].questionId));

                chart.draw(data, options);
            }
            else if (result[i].questionId == 2 || result[i].questionId == 17 || result[i].questionId == 18 || result[i].questionId == 30) {
                var options = {
                    colors: ['#ebc7c7', '#efdbd4', '#f1e0da', ""#f8f0");
            WriteLiteral(@"ed"", '#bababa', ""#646464""],
                    width: ""100%"",
                    height: ""100%"",
                    bar: { groupWidth: ""95%"" },
                    legend: { position: ""none"" },
                };
                for (var j = 0; j < answerCount.length; j++) {
                    var temp = [];
                    temp.push(answerCount[j].answer);
                    temp.push(answerCount[j].count);

                    data.addRow(temp);
                }
                var chart = new google.visualization.ColumnChart(document.getElementById('rating_' + result[i].questionId));
                chart.draw(data, options);
            } else {
                var options = {
                    colors: ['#ebc7c7', '#efdbd4', '#f1e0da', ""#f8f0ed"", '#bababa', ""#646464""],
                    pieHole: 0.8, pieSliceText: 'value', is3D: true, chartArea: {
                        left: 10,
                        right: 0, // !!! works !!!
                        bottom: 10,  // ");
            WriteLiteral(@"!!! works !!!
                        top: 10,
                        width: ""100%"",
                        height: ""100%""
                    },
                    pieSliceTextStyle: {
                        color: '#383838',
                    }
                }
                for (var j = 0; j < answerCount.length; j++) {
                    var temp = [];
                    temp.push(answerCount[j].answer);
                    temp.push(answerCount[j].count);

                    data.addRow(temp);
                }
                var chart = new google.visualization.PieChart(document.getElementById('rating_' + result[i].questionId));
                chart.draw(data, options);
            };
            //options.pieSliceTextStyle = { fontSize: 20 };
            // Instantiate and draw the chart.

        }
    }
    google.charts.setOnLoadCallback(drawChart);
</script>
");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public MyavanaAdminApiClient.ApiClient questionaire { get; private set; }
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<MyavanaAdminModels.QuestionGraph> Html { get; private set; }
    }
}
#pragma warning restore 1591