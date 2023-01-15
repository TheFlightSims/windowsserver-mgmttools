#pragma checksum "C:\Users\Administrator.THEFLIGHTSIMS\Documents\GitHub\windowsserver-mgmttools\database-manager\DatabaseManager\DatabaseManager.Web\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5eef93427e1155aa5828eac82966f7f3bda233a2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.razor-page", @"/Views/Home/Index.cshtml")]
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
#nullable restore
#line 1 "C:\Users\Administrator.THEFLIGHTSIMS\Documents\GitHub\windowsserver-mgmttools\database-manager\DatabaseManager\DatabaseManager.Web\Views\_ViewImports.cshtml"
using DatabaseManager.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Administrator.THEFLIGHTSIMS\Documents\GitHub\windowsserver-mgmttools\database-manager\DatabaseManager\DatabaseManager.Web\Views\_ViewImports.cshtml"
using DatabaseManager.Web.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Administrator.THEFLIGHTSIMS\Documents\GitHub\windowsserver-mgmttools\database-manager\DatabaseManager\DatabaseManager.Web\Views\Home\Index.cshtml"
using DatabaseInterpreter.Model;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5eef93427e1155aa5828eac82966f7f3bda233a2", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8ffdef540f74169871cf3f623f43b236dbb86ce3", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("translateForm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\Administrator.THEFLIGHTSIMS\Documents\GitHub\windowsserver-mgmttools\database-manager\DatabaseManager\DatabaseManager.Web\Views\Home\Index.cshtml"
  
    var databaseTypes = Enum.GetNames(typeof(DatabaseType));

    var databaseDropDownItems = new List<SelectListItem>();

    databaseDropDownItems.AddRange(databaseTypes.Select(item => new SelectListItem() { Text = item, Value = item }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<style>
    .contentPanel {
        display: inline;
        width: 505px;
        float: left;
    }

    .actionPanel {
        display: inline;
        width: 80px;
        float: left;
        height: 300px;
    }

    .actionButton {
        margin-top: 220px;
    }
</style>

<script type=""text/javascript"">
    var validate = function () {
        var sourceDatabaseType = $(""#sourceDatabaseType"").val();
        var targetDatabaseType = $(""#targetDatabaseType"").val();

        if (sourceDatabaseType == ""Unknown"" || targetDatabaseType == ""Unknown"") {
            alert('please specify source database type and target database type.');
            return false;
        }

        var source = $(""#source"").val();

        if (source == """") {
            alert('the source content can not be empty!');
            return false;
        }

        return true;
    }

    var post = function () {
        var isValid = validate();

        if (!isValid) {
            return;
        }

        var data = $(""#transl");
            WriteLiteral(@"ateForm"").serialize();

        $.ajax({
            type: ""POST"",
            url: ""/Home/Translate"",
            data: data,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (response) {
                if (response.hasError == false) {
                    $(""#target"").val(response.data);
                }
                else if (response.message) {
                    alert(response.message);
                }
                else {
                    $(""#target"").val(""N/A"");
                }
            },
            error: function (result) {
                alert(result);
            }
        });
    };
</script>

<div>
    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5eef93427e1155aa5828eac82966f7f3bda233a26381", async() => {
                WriteLiteral("\n        <div class=\"contentPanel\">\n            <div>\n                <span>Source:</span> ");
#nullable restore
#line 86 "C:\Users\Administrator.THEFLIGHTSIMS\Documents\GitHub\windowsserver-mgmttools\database-manager\DatabaseManager\DatabaseManager.Web\Views\Home\Index.cshtml"
                                Write(Html.DropDownList("sourceDatabaseType", databaseDropDownItems));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
            </div>
            <div style=""margin-top:5px;"">
                <textarea id=""source"" name=""source"" cols=""65"" rows=""20"" spellcheck=""false""></textarea>
            </div>
        </div>
        <div class=""actionPanel"">
            <input type=""button"" value=""Translate"" class=""actionButton"" onclick=""post()"" />
        </div>
        <div class=""contentPanel"">
            <div>
                <span>Target:</span> ");
#nullable restore
#line 97 "C:\Users\Administrator.THEFLIGHTSIMS\Documents\GitHub\windowsserver-mgmttools\database-manager\DatabaseManager\DatabaseManager.Web\Views\Home\Index.cshtml"
                                Write(Html.DropDownList("targetDatabaseType", databaseDropDownItems));

#line default
#line hidden
#nullable disable
                WriteLiteral("\n            </div>\n            <div style=\"margin-top:5px;\">\n                <textarea id=\"target\" name=\"target\" cols=\"65\" rows=\"20\" spellcheck=\"false\"></textarea>\n            </div>\n        </div>\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n</div>\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Views_Home_Index> Html { get; private set; } = default!;
        #nullable disable
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Views_Home_Index> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<Views_Home_Index>)PageContext?.ViewData;
        public Views_Home_Index Model => ViewData.Model;
    }
}
#pragma warning restore 1591