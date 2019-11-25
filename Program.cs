using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace filterLog
{
  class Program
  {
    static int Main(string[] args)
    {
      try
      {
        if (args.Length < 1)
          throw new Exception("First argument shuold be path to log file");
        if (args.Length > 2)
          throw new Exception("Too many arguments. Should be one or too");
        var logPath = args[0];
        var optionalFiltersConfigPath = args.Length > 1 ? args[1] : null;
        var settings = new Settings(optionalFiltersConfigPath);

        var prefixMatchRegexString = settings.Filters.CommonSettings.LogEntryPrefixRegex;
        var reactiveMatchRegexStrings = settings.Filters.Reactives.Select(r => r.FilterRegex);
        var supperMatchRegexString = prefixMatchRegexString + "((" + string.Join(")|(", reactiveMatchRegexStrings) + "))";

        var defaultRegexOptions = RegexOptions.Compiled | RegexOptions.ECMAScript;
        var supperMatch = new Regex(supperMatchRegexString, defaultRegexOptions);
        var prefixMatch = new Regex(prefixMatchRegexString, defaultRegexOptions);
        var reactiveMatches = reactiveMatchRegexStrings.Select(s => new Regex(s, defaultRegexOptions));

        bool wasMatching = false;
        foreach (var line in File.ReadLines(logPath))
        {
          if (wasMatching)
          {
            if (prefixMatch.IsMatch(line))
              wasMatching = false;
            else
            {
              Console.WriteLine(line);
              continue;
            }
          }
          if (!supperMatch.IsMatch(line))
            continue;
          if (reactiveMatches.Any(r => r.IsMatch(line)))
          {
            wasMatching = true;
            Console.WriteLine(line);
          }
        }
        //var options = new JsonSerializerOptions
        //{
        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //    WriteIndented = true
        //};
        //var jsonString = File.ReadAllText("my-model.json");
        //var jsonModel = JsonSerializer.Deserialize<MyModel>(jsonString, options);
        //var jsonModel = new FiltersDto(){ CommonSettings = new FiltersHeader(), Reactives = new List<Reactive>(){ new Reactive(), new Reactive()}};
        //var modelJson = JsonSerializer.Serialize(jsonModel, options);
        //Console.WriteLine(modelJson);
        //Console.WriteLine("Hello World!");
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        Console.WriteLine();
        Console.WriteLine(Help);
        return -1;
      }
      return 0;
    }

    private static string Help => $"Application filtering essential from log file. {Environment.NewLine}Usage example: {Environment.NewLine}filterLog pathToLog.log [pathToFiltersConfig.json]";
  }
}

