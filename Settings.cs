using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;

namespace filterLog
{
  [Serializable]
  public class FiltersHeader
  {
    public string LogEntryPrefixRegex {get; set;}
  }

  [Serializable]
  public class Reactive
  {
    public string FilterRegex {get; set;}
  }

  [Serializable]
  public class FiltersDto
  {
    public FiltersHeader CommonSettings {get; set;}
    public List<Reactive> Reactives {get; set;}
  }
  
  public class Settings
  {
    public Settings(string filterPath)
    {
      var options = new JsonSerializerOptions
      {
          PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
          WriteIndented = true
      };

      var jsonString = File.ReadAllText(FilterPath(filterPath));
      Filters = JsonSerializer.Deserialize<FiltersDto>(jsonString, options);
    }

    public FiltersDto Filters {get; private set;} 

    private string FilterPath(string filtersPath)
    {
      if(!string.IsNullOrWhiteSpace(filtersPath))
      {
        if(!File.Exists(filtersPath))
          throw new Exception($"Not found filters config file '{filtersPath}'");
        return filtersPath;
      }
      var defaultFilterFileName = "logFilters.json";
      if(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultFilterFileName) is var currentDirConfig && File.Exists(currentDirConfig))
        return currentDirConfig;
      if(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), defaultFilterFileName) is var homeDirConfig && File.Exists(homeDirConfig))
        return homeDirConfig;
      throw new Exception($"Filters config file not specified in commandline argument or not found in path '{currentDirConfig}' or '{homeDirConfig}'"); 
    }
  }
}
