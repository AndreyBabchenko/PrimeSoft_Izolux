namespace NoPaper.Queries
{
  internal class v_SawTaskMain
  {
    public static string GetData() =>
      $@"select 
         ID,
         Name,
         Data,
         Comment,
         ListGlass,
         GlassCount
       from v_SawTaskMain
       where cast(Data as date) = cast(GetDate() as date)
       order by Name";
  }
}