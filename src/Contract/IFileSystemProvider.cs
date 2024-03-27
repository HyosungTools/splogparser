using System.IO;

namespace Contract
{
   /// <summary>
   /// Abstract interface to wrap System.IO calls. This allows us to mock out .net in unit tests. 
   /// At the same time it allows for the centralized handling of exceptions. 
   /// </summary>
   public interface IFileSystemProvider
   {
      /// <summary>
      /// Get the application's current working directory.
      /// </summary>
      /// <returns>true if the operation succeeded; false otherwise</returns>
      string GetCurrentDirectory();

      /// <summary>
      /// Sets the application's current working directory to the specified directory.
      /// </summary>
      /// <param name="path">new current working directory</param>
      /// <returns>true if the operation succeeded; false otherwise</returns>
      bool SetCurrentDirectory(string path);

      /// <summary>
      /// Determines whether the given path refers to an existing directory on disk.
      /// </summary>
      /// <param name="path">path to check for existence</param>
      /// <returns>true if the path exists; false otherwise.</returns>
      bool DirExists(string path);

      /// <summary>
      /// Unzip a zip file into a directory
      /// </summary>
      /// <param name="zipFilePath">zip file to unzip</param>
      /// <param name="extractPath">existing folder to unzip files into</param>
      void ExtractToDirectory(string zipFilePath, string extractPath);

      /// <summary>
      /// Returns the names of subdirectories
      /// </summary>
      /// <param name="path">absolute path to the directory to search (i.e. C:\\VISTAatm)</param>
      /// <returns>An array of the names of the subdirectories, or an empty array if no directories are found</returns>
      string[] GetDirectories(string path);

      /// <summary>
      /// Returns the names of of a directory (less path)
      /// </summary>
      /// <param name="path">full path of the directory</param>
      /// <returns>Name of the directory (no path)</returns>
      string GetDirectoryName(string path);

      /// <summary>
      /// Returns the names of files in a directory
      /// </summary>
      /// <param name="path">path to the directory to search</param>
      /// <param name="searchPattern">search pattern of files to look for (or *)</param>
      /// <param name="recursive">whether to recursively search the path</param>
      /// <returns>an array of file names, or an empty array if no files are found</returns>
      string[] GetFiles(string path, string searchPattern, bool recursive = true);

      /// <summary>
      /// Create a directory
      /// </summary>
      /// <param name="path">directory to create</param>
      /// <returns>true if the directory was created, false otherwise</returns>
      bool CreateDirectory(string path);

      /// <summary>
      /// Deletes a specified directory, and optionally any subdirectories
      /// </summary>
      /// <param name="path"></param>
      /// <param name="recusrive"></param>
      void DeleteDir(string path, bool recusrive);

      /// <summary>
      /// Determines whether a file exists.
      /// </summary>
      /// <param name="path">file to check for existence</param>
      /// <returns>true if the file exists; false otherwise.</returns>
      bool Exists(string path);

      /// <summary>
      /// Determines whether a file is not read-only.
      /// </summary>
      /// <param name="path">file to check for existence</param>
      /// <returns>true if the file is writable; false otherwise.</returns>
      bool FileInUse(string path);

      /// <summary>
      /// Deletes a specified file
      /// </summary>
      /// <param name="fileName"></param>
      void Delete(string fileName);

      /// <summary>
      /// Get the filename without the path
      /// </summary>
      /// <param name="path"></param>
      /// <returns>filename</returns>
      string GetFileName(string path);

      /// <summary>
      /// Return the filename without the extension
      /// </summary>
      /// <param name="path">full file name</param>
      /// <returns>file name prefix</returns>
      string GetFileNameWithoutExtension(string path);

      /// <summary>
      /// Open a Text File. Wrapper around the implementation
      /// </summary>
      /// <param name="filename">file to open</param>
      /// <returns>StreamReader for the opened file</returns>
      StreamReader OpenTextFile(string filename);

   }
}
