using NoPaper.Queries;

namespace NoPaper.View
{
  static class MainData
  {
    public static string GetMainQuery(int idSawTask) =>
      $@"{TempViewSawTaskGlassProcessingData.GetQuery()}
          where
            idSawTaskMain in ({idSawTask})";

    public static string GetMainQueryAssembly(int sIdSawTask) =>
      $@"{TempViewSawTaskGlassProcessingData.GetQuery()}
          where
            idSawTaskMain_Assembly in ({sIdSawTask})";
  }
}