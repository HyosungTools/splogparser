using Contract;
using System;
using System.IO;
using System.IO.Compression;

namespace Impl
{
   /// <summary>
   /// Implementation of the IFileSystemProvider for interfacing with Windows
   /// Wrapper for Windows commands. 
   /// </summary>
   public class FileSystemProvider : IFileSystemProvider
   {
      public string GetCurrentDirectory()
      {
         return Directory.GetCurrentDirectory();
      }

      public bool SetCurrentDirectory(string path)
      {
         try
         {
            Directory.SetCurrentDirectory(path);
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception: " + e.Message);
            return false;
         }
         return true;
      }

      public bool DirExists(string path)
      {
         return Directory.Exists(path);
      }

      public string[] GetDirectories(string path)
      {
         try
         {
            return Directory.GetDirectories(path);
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception: " + e.Message);
            return new string[0];
         }
      }

      public string GetDirectoryName(string path)
      {
         try
         {
            return new DirectoryInfo(path).Name;
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception: " + e.Message);
            return "";
         }
      }

      public string[] GetFiles(string path, string searchPattern)
      {
         try
         {
            return Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception: " + e.Message);
            return new string[0];
         }
      }
      public bool CreateDirectory(string path)
      {
         try
         {
            _ = Directory.CreateDirectory(path);
            return true;
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception: " + e.Message);
            return false;
         }
      }
      public void DeleteDir(string path, bool recursive)
      {
         try
         {
            Directory.Delete(path, recursive);
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception: " + e.Message);
         }
      }

      public void ExtractToDirectory(string zipFilePath, string extractPath)
      {
         ZipFile.ExtractToDirectory(zipFilePath, extractPath);
      }

      public bool Exists(string path)
      {
         return File.Exists(path);
      }

      public void Delete(string fileName)
      {
         try
         {
            File.Delete(fileName);
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception: " + e.Message);
         }
      }

      public string GetFileNameWithoutExtension(string fullFileName)
      {
         return Path.GetFileNameWithoutExtension(fullFileName);
      }

      public StreamReader OpenTextFile(string fileName)
      {
         return new StreamReader(fileName);
      }
   }
}
