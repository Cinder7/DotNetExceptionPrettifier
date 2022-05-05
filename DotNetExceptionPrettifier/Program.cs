
using DotNetExceptionPrettifier;

var inputArgs = Environment.GetCommandLineArgs().ToList();
var filePath = inputArgs.Last();
var result = await ExceptionPrettifier.Prettify(filePath);
Console.Write(result);