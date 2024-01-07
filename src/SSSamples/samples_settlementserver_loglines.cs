namespace Samples
{
   public static class samples_settlementserver_loglines
   {
      public const string EJ_StartOfFile_1 =
@"2023-10-27 01:00:09.1906||WARN|Settlement.Program|Settlement Server stopped. 
2023-10-27 01:00:13.8599||DEBUG|Settlement.Program|Settlement Server initializing 
2023-10-27 01:00:14.1002||INFO|Settlement.Startup|Current version: 1.1.4.1 
2023-10-27 01:00:14.1002||INFO|Settlement.Startup|Build date: 12/22/2022 4:08:28 PM (UTC) 
2023-10-27 01:00:15.3774||INFO|JournalImporter.JournalImporter|JournalImporter is running. 
2023-10-27 04:00:35.2952||INFO|Settlement.API.Controllers.JournalsController|Upload received for ej2_663259 
2023-10-27 04:00:36.4243||INFO|JournalImporter.JournalImporter|Discovered C:\ProgramData\Nautilus Hyosung\Settlement Server\Uploads\ImportsPending\ej2_663259.mdb 
2023-10-27 04:00:37.0651||INFO|Settlement.API.Controllers.JournalsController|Created C:\ProgramData\Nautilus Hyosung\Settlement Server\Uploads\ImportsPending\ej2_663259.mdb successfully. 
2023-10-27 04:00:37.1588||INFO|Settlement.API.Controllers.JournalsController|Upload received for ej2_663263 ";

      public const string EJ_EndOfFile_1 =
@"2023-10-27 16:32:50.8427||INFO|JournalImporter.JournalImporter|Import Succeeded: C:\ProgramData\Nautilus Hyosung\Settlement Server\Uploads\ImportsPending\ej2_663329.mdb 
2023-10-27 17:09:45.4375||INFO|Settlement.API.Controllers.JournalsController|Upload received for ej2_663329 
2023-10-27 17:09:45.4778||INFO|Settlement.API.Controllers.JournalsController|Created C:\ProgramData\Nautilus Hyosung\Settlement Server\Uploads\ImportsPending\ej2_663329.mdb successfully. 
2023-10-27 17:09:50.9320||INFO|JournalImporter.JournalImporter|Discovered C:\ProgramData\Nautilus Hyosung\Settlement Server\Uploads\ImportsPending\ej2_663329.mdb 
2023-10-27 17:10:01.0362||INFO|JournalImporter.JournalImporter|Import Succeeded: C:\ProgramData\Nautilus Hyosung\Settlement Server\Uploads\ImportsPending\ej2_663329.mdb 
";
   }
}
