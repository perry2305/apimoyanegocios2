#pragma checksum "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6831ef9736a4b518ed75a309d5916d5674fd7c65"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Contactos_Details), @"mvc.1.0.view", @"/Views/Contactos/Details.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Contactos/Details.cshtml", typeof(AspNetCore.Views_Contactos_Details))]
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
#line 1 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\_ViewImports.cshtml"
using ProyectoCRM;

#line default
#line hidden
#line 2 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\_ViewImports.cshtml"
using ProyectoCRM.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6831ef9736a4b518ed75a309d5916d5674fd7c65", @"/Views/Contactos/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"884812c7cd82280830cee784d1bc3232b6b3e061", @"/Views/_ViewImports.cshtml")]
    public class Views_Contactos_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ProyectoCRM.Models.Contacto>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "SubirFoto", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "POST", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(38, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
  
    ViewData["Title"] = "Details";

#line default
#line hidden
            BeginContext(83, 122, true);
            WriteLiteral("\r\n<h2>Details</h2>\r\n\r\n<div>\r\n    <h4>Contacto</h4>\r\n    <hr />\r\n    <dl class=\"dl-horizontal\">\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(206, 38, false);
#line 15 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.ID));

#line default
#line hidden
            EndContext();
            BeginContext(244, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(288, 34, false);
#line 18 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayFor(model => model.ID));

#line default
#line hidden
            EndContext();
            BeginContext(322, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(366, 42, false);
#line 21 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Nombre));

#line default
#line hidden
            EndContext();
            BeginContext(408, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(452, 38, false);
#line 24 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayFor(model => model.Nombre));

#line default
#line hidden
            EndContext();
            BeginContext(490, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(534, 43, false);
#line 27 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Colonia));

#line default
#line hidden
            EndContext();
            BeginContext(577, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(621, 39, false);
#line 30 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayFor(model => model.Colonia));

#line default
#line hidden
            EndContext();
            BeginContext(660, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(704, 42, false);
#line 33 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Numero));

#line default
#line hidden
            EndContext();
            BeginContext(746, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(790, 38, false);
#line 36 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayFor(model => model.Numero));

#line default
#line hidden
            EndContext();
            BeginContext(828, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(872, 41, false);
#line 39 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Email));

#line default
#line hidden
            EndContext();
            BeginContext(913, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(957, 37, false);
#line 42 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayFor(model => model.Email));

#line default
#line hidden
            EndContext();
            BeginContext(994, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1038, 40, false);
#line 45 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Tipo));

#line default
#line hidden
            EndContext();
            BeginContext(1078, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1122, 36, false);
#line 48 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayFor(model => model.Tipo));

#line default
#line hidden
            EndContext();
            BeginContext(1158, 43, true);
            WriteLiteral("\r\n        </dd>\r\n        <dt>\r\n            ");
            EndContext();
            BeginContext(1202, 42, false);
#line 51 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayNameFor(model => model.Ciudad));

#line default
#line hidden
            EndContext();
            BeginContext(1244, 43, true);
            WriteLiteral("\r\n        </dt>\r\n        <dd>\r\n            ");
            EndContext();
            BeginContext(1288, 38, false);
#line 54 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
       Write(Html.DisplayFor(model => model.Ciudad));

#line default
#line hidden
            EndContext();
            BeginContext(1326, 36, true);
            WriteLiteral("\r\n        </dd>\r\n    </dl>\r\n    <img");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 1362, "\"", 1379, 1);
#line 57 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
WriteAttributeValue("", 1368, Model.Foto, 1368, 11, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1380, 47, true);
            WriteLiteral(" alt=\"foto contacto\"/>\r\n</div>\r\n<h3>Foto</h3>\r\n");
            EndContext();
            BeginContext(1427, 414, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "770b8eda7eba497bbf36050a52c253eb", async() => {
                BeginContext(1500, 170, true);
                WriteLiteral("\r\n    <div class=\"form-group\">\r\n        <label for=\"foto\" class=\"control-label\"></label>\r\n        <input type=\"file\" name=\"foto\"/>\r\n        <input type=\"hidden\" name=\"id\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 1670, "\"", 1687, 1);
#line 64 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
WriteAttributeValue("", 1678, Model.ID, 1678, 9, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1688, 146, true);
                WriteLiteral("/>\r\n\r\n        <div class=\"form-group\">\r\n            <input type=\"submit\" value=\"subir\" class=\"btn btn-default\"/>\r\n        </div>    \r\n    </div>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1841, 15, true);
            WriteLiteral("\r\n<div>\r\n\r\n    ");
            EndContext();
            BeginContext(1857, 53, false);
#line 73 "C:\Users\ADM\Desktop\ProyectoCRM\ProyectoCRM\ProyectoCRM\Views\Contactos\Details.cshtml"
Write(Html.ActionLink("Edit", "Edit", new { id = Model.ID}));

#line default
#line hidden
            EndContext();
            BeginContext(1910, 8, true);
            WriteLiteral(" |\r\n    ");
            EndContext();
            BeginContext(1918, 38, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b98dc769de484d1daff8694e0e18ea86", async() => {
                BeginContext(1940, 12, true);
                WriteLiteral("Back to List");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1956, 10, true);
            WriteLiteral("\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ProyectoCRM.Models.Contacto> Html { get; private set; }
    }
}
#pragma warning restore 1591