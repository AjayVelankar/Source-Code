using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opus.Victus.UI.Core
{
    public interface IValidater
    {
        string ViewModelName { get; set; }
        string ValidationAttribute { get; set; }
        string PropertyName { get; set; }
        string[] Parameters { get; }
        string Message { get; set; }
        string ErrorSymbol { get; set; }
    }
    public interface IValidationManager
    {
        List<IValidater> GetValidaters();
        List<IValidater> GetValidaters(string modelName);

    }

    [HtmlTargetElement(Attributes = "Validations")]
    public class ValidationsTagHelper : TagHelper
    {
        IValidationManager _validationManager;
        public ValidationsTagHelper(IValidationManager validationManager)
        {
            _validationManager = validationManager;
        }

        [HtmlAttributeName("Validations")]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression aspFor { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagHelperAttribute attribute;
            output.Attributes.TryGetAttribute("id", out attribute);
            var id = For.Metadata.PropertyName;
            if (attribute == null)
            {
                output.Attributes.Add(id, id);
            }
            else
            {
                id = attribute?.Value?.ToString() ?? id;
            }

            //var propertyName = For.Metadata.PropertyName;
            var list = _validationManager.GetValidaters(aspFor.ModelExplorer.Container.ModelType.Name)?.Where(c => c.PropertyName == id).ToList();
            if (list != null && list.Count > 0)
            {
                var dbvalidators = new Validators();
                dbvalidators.Validations = new List<Validator>();
                foreach (var item in list)
                {
                    var validator = Validator.GetObjectBystring(item.ValidationAttribute, item.Message, item.ErrorSymbol, item.Parameters);
                    dbvalidators.Validations.Add(validator);
                }
                output.Attributes.SetAttribute("validators", dbvalidators.ToString());
                output.PostElement.SetHtmlContent($"<span id='span.{id}' class='hidden'></span>");
                return;
            }
            var value = For.Model as string;
            var attributes = ((Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DefaultModelMetadata)For.Metadata).Attributes;
            var validators = new Validators();
            validators.Validations = attributes.Attributes
                                               .Where(c => c.GetType().IsSubclassOf(typeof(Validator)))
                                               .Select(c => (Validator)c).ToList<Validator>();

            //output.Attributes.Remove(attribute);
            output.Attributes.SetAttribute("validators", validators.ToString());
            output.PostElement.SetHtmlContent($"<span id='span.{id}' class='hidden'></span>");
        }
    }
}
