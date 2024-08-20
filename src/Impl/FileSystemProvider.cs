using Contract;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

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

      private static Random random = new Random();

      public static string RandomString(int length)
      {
         const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
         return new string(Enumerable.Repeat(chars, length)
             .Select(s => s[random.Next(s.Length)]).ToArray());
      }

      public bool CreateDirectory(string path)
      {
         try
         {
            if (DirExists(path))
            {
               path = path + RandomString(4); 
            }
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
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string file in files)
            {
               File.SetAttributes(file, FileAttributes.Normal);
               File.Delete(file);
            }

            foreach (string dir in dirs)
            {
               DeleteDir(dir, recursive);
            }

            Directory.Delete(path, false);
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception: " + e.Message);
         }
      }

      public void ExtractToDirectory(string zipFilePath, string extractPath)
      {
         try
         {
            using (FileStream zipFileStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read))
            using (ZipArchive archive = new ZipArchive(zipFileStream, ZipArchiveMode.Read))
            {
               foreach (ZipArchiveEntry entry in archive.Entries)
               {
                  string fullPath = Path.Combine(extractPath, entry.FullName);

                  // Create the directory if it doesn't exist
                  if (entry.FullName.EndsWith("/"))
                  {
                     Directory.CreateDirectory(fullPath);
                  }
                  else
                  {
                     // Ensure the parent directory exists
                     Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                     // Extract the file
                     entry.ExtractToFile(fullPath, true);

                     // Check if the extracted file is a ZIP file
                     if (Path.GetExtension(fullPath).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                     {
                        // Recursively unzip the nested ZIP file
                        ExtractToDirectory(fullPath, Path.Combine(extractPath, Path.GetFileNameWithoutExtension(fullPath)));
                     }
                  }
               }
            }
         }
         catch (Exception e)
         {
            Console.WriteLine("Exception: " + e.Message);
         }
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

      public string GetFileName(string path)
      {
         return Path.GetFileName(path);
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
