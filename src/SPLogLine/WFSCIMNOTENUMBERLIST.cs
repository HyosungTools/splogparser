﻿using System;
using System.Collections.Generic;
using Contract;
using RegEx;

namespace LogLineHandler
{
   public class WFSCIMNOTENUMBERLIST : SPLine
   {
      public string[,] lppNoteNumbers { get; set; }

      public WFSCIMNOTENUMBERLIST(ILogFileHandler parent, string logLine, XFSType xfsType = XFSType.None) : base(parent, logLine, xfsType)
      {
      }

      protected override void Initialize(int lUnitCount = 1)
      {
         base.Initialize();
         int indexOfTable = logLine.IndexOf("lpNoteNumberList->");
         int indexOfList = logLine.IndexOf("lppNoteNumber =");

         if (indexOfTable > 0)
         {
            lppNoteNumbers = NoteNumberListFromTable(logLine, lUnitCount);
         }

         else if (indexOfList > 0 || logLine.Contains("usNoteID = ["))
         {
            lppNoteNumbers = NoteNumberListFromList(logLine, lUnitCount);
         }
      }

      public static string[,] NoteNumberListFromTable(string logLine, int lUnitCount = 1)
      {
         // resize the lpNoteNumberList array to hold all note numbers for all logical units
         string[,] lpNoteNumberList = new string[lUnitCount, 20];

         int indexOfTable = logLine.IndexOf("lpNoteNumberList->");
         string subLogLine = logLine.Substring(indexOfTable + "lpNoteNumberList->\r\n".Length);

         indexOfTable = subLogLine.IndexOf("\t\t\tLCU ETC");
         if (indexOfTable > 0)
         {
            // this is the note number list which we process
            subLogLine = subLogLine.Substring(0, indexOfTable);
         }

         // iterate over each line of the block and load up the array
         int colCount = 0;
         char[] trimChars = { '\t', '\r' };
         (bool found, string oneLine, string subLogLine) result = LogLine.ReadNextLine(subLogLine);

         // Discovered a scenario where following line can be NULL

         while (result.found && result.oneLine.Trim(trimChars).Trim() != "NULL")
         {
            result.oneLine = result.oneLine.TrimStart(trimChars).Replace("\t\t", ",");
            string[] match = result.oneLine.Split(',');
            for (int i = 0; i < lUnitCount; i++)
            {
               // i + 1 because the first column is throw away
               match[i + 1] = match[i + 1].TrimStart(trimChars);
               if (match[i + 1] == "")
               {
                  lpNoteNumberList[i, colCount] = string.Empty;
               }
               else
               {
                  // store as a string 'noteID:count'
                  lpNoteNumberList[i, colCount] = match[i + 1].Replace(']', ':').Replace("[", "");
               }
            }
            // move onto the next log line, to fill in the next column
            result = LogLine.ReadNextLine(result.subLogLine);
            colCount++;
         }
         return lpNoteNumberList;
      }

      public static string[,] NoteNumberListFromList(string logLine, int lUnitCount = 1)
      {
         // resize the lpNoteNumberList array to hold all note numbers for all logical units
         string[,] lpNoteNumberList = new string[lUnitCount, 20];

         // how many baknote types are there? 
         (bool success, string xfsMatch, string subLogLine) result = usNumOfNoteNumbers(logLine);
         if (result.success && int.Parse(result.xfsMatch.Trim()) == 0)
         {
            Console.WriteLine("Should not get here, it means usNumOfNoteNumbers = 0");
            return lpNoteNumberList;
         }

         (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(logLine);

         for (int i = 0; i < lUnitCount; i++)
         {
            Console.WriteLine($"logicalUnits.thisUnit :{logicalUnits.thisUnit} ");
            string[] usNoteIDs = usNoteIDsFromList(logicalUnits.thisUnit);
            string[] ulCounts = ulCountsFromList(logicalUnits.thisUnit);

            Console.WriteLine($"i = {i}, usNoteIDs.Length = {usNoteIDs.Length}, ulCounts.Length = {ulCounts.Length}");

            for (int j = 0; j < usNoteIDs.Length; j++)
            {
               lpNoteNumberList[i, j] = usNoteIDs[j] + ":" + ulCounts[j];
            }

            logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
         }

         return lpNoteNumberList; 
      }

      public static string[,] NoteNumberListFromList(string[] logicalParts, int lUnitCount = 1)
      {
         // resize the lpNoteNumberList array to hold all note numbers for all logical units
         string[,] lpNoteNumberList = new string[lUnitCount, 20];
         Console.WriteLine($"lpNoteNumberList size : {lUnitCount},20");

         for (int i = 0; i < lUnitCount; i++)
         {
            string[] usNoteIDs = usNoteIDsFromList(logicalParts[i]);
            string[] ulCounts = ulCountsFromList(logicalParts[i]);

            usNoteIDs = Util.Resize(usNoteIDs, 20, string.Empty);
            ulCounts = Util.Resize(ulCounts, 20, string.Empty);

            Console.WriteLine($"i = {i}, usNoteIDs.Length = {usNoteIDs.Length}, ulCounts.Length = {ulCounts.Length}");

            for (int j = 0; j < 20; j++)
            {
               lpNoteNumberList[i, j] = usNoteIDs[j] + ":" + ulCounts[j];
            }
         }

         return lpNoteNumberList;
      }

      // usNumOfNoteNumbers  - number of BankNote Types 
      protected static (bool success, string xfsMatch, string subLogLine) usNumOfNoteNumbers(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usNumOfNoteNumbers = \\[)(\\d+)");
      }

      protected static string[] usNoteIDsFromList(string logLine)
      {
         List<string> values = new List<string>();

         (bool success, string xfsMatch, string subLogLine) result = usNumOfNoteNumbers(logLine);
         int usCount = int.Parse(result.xfsMatch.Trim());

         (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(result.subLogLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(usNoteID(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), usCount));
      }

      protected static string[] ulCountsFromList(string logLine)
      {
         List<string> values = new List<string>();

         (bool success, string xfsMatch, string subLogLine) result = usNumOfNoteNumbers(logLine);
         int usCount = int.Parse(result.xfsMatch.Trim());

         (string thisUnit, string nextUnits) logicalUnits = Util.NextUnit(result.subLogLine);

         for (int i = 0; i < usCount; i++)
         {
            values.Add(ulCount(logicalUnits.thisUnit).xfsMatch.Trim());
            logicalUnits = Util.NextUnit(logicalUnits.nextUnits);
         }
         return Util.TrimAll(Util.Resize(values.ToArray(), usCount));
      }

      // usNoteID  
      protected static (bool success, string xfsMatch, string subLogLine) usNoteID(string logLine)
      {
         return Util.MatchList(logLine, "(?<=usNoteID = \\[)(\\d+)");
      }

      // ulCount  - singular search from a list-style log line
      protected static (bool success, string xfsMatch, string subLogLine) ulCount(string logLine)
      {
         return Util.MatchList(logLine, "(?<=ulCount = \\[)(\\d+)");
      }
   }
}
