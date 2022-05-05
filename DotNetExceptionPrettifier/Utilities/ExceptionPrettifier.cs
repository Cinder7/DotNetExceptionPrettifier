using System.Text.RegularExpressions;

namespace DotNetExceptionPrettifier;

public static class ExceptionPrettifier
{

	public static async Task<string> Prettify(string filePath)
	{

		var fileContent = await File.ReadAllTextAsync(filePath);
		var errorMessage = ExtractErrorMessage(fileContent);

		var stacktrace = SimplifyStacktrace(fileContent);

		var result = errorMessage + stacktrace;

		var newFilepath = $"{filePath}.prettified";
		await File.WriteAllTextAsync(newFilepath, result);

		return result;
	}

	private static string SimplifyStacktrace(string text)
	{
		var fileNames = new List<string>();

		var filenameRegex = new Regex("[A-Za-z0-9]*.cs:line [0-9]*");


		var matches = filenameRegex.Matches(text).ToList();

		foreach (var match in matches)
		{
			var filename = text.Substring(match.Index, match.Length);
			filename = filename.Replace(".cs:line", "");
			fileNames.Add(filename);
		}

		var resultStr = "\n\nStacktrace: ========================================================\n";
		foreach (var filename in fileNames)
			resultStr += $"{filename}\n";

		return resultStr;
	}

	private static string ExtractErrorMessage(string text)
	{
		var filenameRegex = new Regex("[A-Za-z0-9]*.cs:line [0-9]*");
		var matches = filenameRegex.Matches(text).ToList();
		var index = matches.First().Index;
		var errorMessage = text.Substring(0, index + 1);
		var result = $"\nError message: =====================================================\n{errorMessage}";

		return result;
	}
}