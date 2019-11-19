using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;

namespace filterLog
{
  class Program
  {
    static int Main(string[] args)
    {
      try
      {
        if(args.Length < 2)
          throw new Exception ("First argument shuold be path to log file");
        if(args.Length > 3)          
          throw new Exception ("Too many arguments. Should be one or too");
        var logPath = args[1];
        var optionalFiltersConfigPath = args.Length > 2 ? args[2] : null;
        var settings = new Settings(optionalFiltersConfigPath);

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
      catch(Exception ex)
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
