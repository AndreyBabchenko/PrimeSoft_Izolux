using Newtonsoft.Json;
using System;
using System.Text;
using System.Xml.Linq;

namespace NoPaper.Models
{
  internal class RemakeModel
  {
    [JsonProperty("AccountName")]
    public string    AccountName { get; private set; }
    [JsonProperty("TaskDate")]
    public DateTime? TaskDate    { get; private set; }
    [JsonProperty("SawTaskName")]
    public string    SawTaskName { get; private set; }
    [JsonProperty("SawTaskDate")]
    public DateTime? SawTaskDate { get; private set; }

    [JsonProperty("SectorName")]
    public string SectorName { get; private set; }

    [JsonProperty("GPName")]
    public string GPName { get; private set; }
    [JsonProperty("nCount")]
    public int    nCount { get; private set; }
    [JsonProperty("Width")]
    public int    Width  { get; private set; }
    [JsonProperty("Height")]
    public int    Height { get; private set; }
    [JsonProperty("Area")]
    public double Area { get; private set; }

    public RemakeModel(string _AccountName, DateTime? _TaskDate, string _SawTaskName, DateTime? _SawTaskDate, string _SectorName,  string _GPName, int _nCount, int _Width, int _Height, double _Area)
    {
      AccountName = _AccountName;
      TaskDate    = _TaskDate;
      SawTaskName = _SawTaskName;
      SawTaskDate = _SawTaskDate;
      SectorName  = _SectorName;
      GPName      = _GPName;
      nCount      = _nCount;
      Width       = _Width;
      Height      = _Height;
      Area        = _Area;
    }

    public RemakeModel()
    {
      AccountName = "";
      TaskDate    = null;
      SawTaskName = "";
      SawTaskDate = null;
      SectorName  = "";
      GPName      = "";
      nCount      = 0;
      Width       = 0;
      Height      = 0;
      Area        = 0;
    }
  }
}