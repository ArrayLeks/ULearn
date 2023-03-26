using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
	public string GetApiDescription()
	{
		var type = typeof(T);
		var attr = (ApiDescriptionAttribute) Attribute.GetCustomAttribute(type, typeof(ApiDescriptionAttribute));
		return attr?.Description;
	}

	public string[] GetApiMethodNames()
	{
		return typeof(T)
			.GetMethods()
			.Select(s => s)
			.Where(s => (ApiDescriptionAttribute)Attribute.GetCustomAttribute(s, typeof(ApiDescriptionAttribute)) != null
			&& (ApiMethodAttribute)Attribute.GetCustomAttribute(s, typeof(ApiMethodAttribute)) != null)
			.Select(s => s.Name)
			.ToArray();
	}

	public string GetApiMethodDescription(string methodName)
	{
		var method = typeof(T).GetMethods().Where(s => s.Name == methodName).FirstOrDefault();
		if (method == null) return null;
		var attr = (ApiDescriptionAttribute)Attribute.GetCustomAttribute(method, typeof(ApiDescriptionAttribute));
        return attr?.Description;
	}

	public string[] GetApiMethodParamNames(string methodName)
	{
		return typeof(T)
			.GetMethods()
			.Where(s => s.Name == methodName)
			.FirstOrDefault()
			.GetParameters()
			.Select(s => s.Name)
			.ToArray();
	}

	public string GetApiMethodParamDescription(string methodName, string paramName)
	{
		return typeof(T)
			.GetMethods()
			?.Where(s => s.Name == methodName)
			?.FirstOrDefault()
			?.GetParameters()
			?.Where(s => s.Name == paramName)
			?.Select(s => (ApiDescriptionAttribute)Attribute.GetCustomAttribute(s, typeof(ApiDescriptionAttribute)))
			?.FirstOrDefault()
			?.Description;
    }

	public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
	{
		var param = typeof(T)
            .GetMethods()
            ?.Where(s => s.Name == methodName)
            ?.FirstOrDefault()
            ?.GetParameters()
            ?.Where(s => s.Name == paramName)
            ?.FirstOrDefault();
        var result = new ApiParamDescription();
        result.ParamDescription = new CommonDescription(paramName);
        if(param != null) FillParamDescription(result, param);
		return result;
    }

	public ApiMethodDescription GetApiMethodFullDescription(string methodName)
	{
		var method = typeof(T)
			.GetMethods()
			?.Where(s => s.Name == methodName)
			?.FirstOrDefault();
		if ((ApiMethodAttribute)Attribute.GetCustomAttribute(method, typeof(ApiMethodAttribute)) == null)
			return null;
        var result = new ApiMethodDescription();
        result.MethodDescription = new CommonDescription(methodName);
        if (method != null)
		{
			result.MethodDescription.Description = 
				((ApiDescriptionAttribute)Attribute.GetCustomAttribute(method, typeof(ApiDescriptionAttribute)))?.Description ?? null;
			var list = new List<ApiParamDescription>();
			foreach (var item in method.GetParameters())
			{
                var desc = new ApiParamDescription();
                FillParamDescription(desc, item);
                list.Add(desc);
            }
			result.ParamDescriptions = list.ToArray();
			if(method.ReturnParameter.ParameterType != typeof(void))
				result.ReturnDescription = FillParamDescription(new ApiParamDescription(), method.ReturnParameter);
        }
		return result;
    }
	
	private static ApiParamDescription FillParamDescription(ApiParamDescription desc, ParameterInfo param)
	{
		desc.ParamDescription = param.Name == "" ? new CommonDescription() : new CommonDescription(param.Name);

        if (param != null)
        {
            if ((ApiIntValidationAttribute)Attribute.GetCustomAttribute(param, typeof(ApiIntValidationAttribute)) != null)
            {
                desc.MinValue = ((ApiIntValidationAttribute)Attribute.GetCustomAttribute(param, typeof(ApiIntValidationAttribute))).MinValue;
                desc.MaxValue = ((ApiIntValidationAttribute)Attribute.GetCustomAttribute(param, typeof(ApiIntValidationAttribute))).MaxValue;
            }
            if((ApiRequiredAttribute)Attribute.GetCustomAttribute(param, typeof(ApiRequiredAttribute)) != null)
			{
                desc.Required =
                ((ApiRequiredAttribute)Attribute.GetCustomAttribute(param, typeof(ApiRequiredAttribute))).Required;
            }
            desc.ParamDescription.Description =
            ((ApiDescriptionAttribute)Attribute.GetCustomAttribute(param, typeof(ApiDescriptionAttribute)))?.Description ?? null;
        }
		return desc;
    }
}