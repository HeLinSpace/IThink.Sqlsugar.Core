/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 文件提供者
    /// </summary>
    public class FileProvider : PhysicalFileProvider, IThinkFileProvider
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="hostingEnvironment">Hosting environment</param>
        public FileProvider(IWebHostEnvironment hostingEnvironment) 
            : base(File.Exists(hostingEnvironment.WebRootPath) ? Path.GetDirectoryName(hostingEnvironment.WebRootPath) : hostingEnvironment.ContentRootPath)
        {
            var path = hostingEnvironment.ContentRootPath ?? string.Empty;
            if (File.Exists(path))
                path = Path.GetDirectoryName(path);

            BaseDirectory = path;
        }

        #region Utilities
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private static void DeleteDirectoryRecursive(string path)
        {
            Directory.Delete(path, true);
            const int maxIterationToWait = 10;
            var curIteration = 0;

            //according to the documentation(https://msdn.microsoft.com/ru-ru/library/windows/desktop/aa365488.aspx) 
            //System.IO.Directory.Delete method ultimately (after removing the files) calls native 
            //RemoveDirectory function which marks the directory as "deleted". That's why we wait until 
            //the directory is actually deleted. For more details see https://stackoverflow.com/a/4245121
            while (Directory.Exists(path))
            {
                curIteration += 1;
                if (curIteration > maxIterationToWait)
                    return;
                Thread.Sleep(100);
            }
        }

        #endregion

        /// <summary>
        /// 组合路径
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The combined paths</returns>
        public virtual string Combine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        /// <summary>
        /// 创建指定路径中的所有目录和子目录
        /// </summary>
        /// <param name="path">The directory to create</param>
        public virtual void CreateDirectory(string path)
        {
            if (!DirectoryExists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 创建或覆盖指定路径中的文件
        /// </summary>
        /// <param name="path">The path and name of the file to create</param>
        public virtual void CreateFile(string path)
        {
            if (FileExists(path))
                return;

            //we use 'using' to close the file after it's created
            using (File.Create(path))
            {
            }
        }

        /// <summary>
        /// 深度优先递归删除，在Windows资源管理器中打开后代目录的处理。
        /// </summary>
        /// <param name="path">Directory path</param>
        public void DeleteDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(path);

            //find more info about directory deletion
            //and why we use this approach at https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true

            foreach (var directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                DeleteDirectoryRecursive(path);
            }
            catch (IOException)
            {
                DeleteDirectoryRecursive(path);
            }
            catch (UnauthorizedAccessException)
            {
                DeleteDirectoryRecursive(path);
            }
        }

        /// <summary>
        /// 删除指定的文件
        /// </summary>
        /// <param name="filePath">The name of the file to be deleted. Wildcard characters are not supported</param>
        public virtual void DeleteFile(string filePath)
        {
            if (!FileExists(filePath))
                return;

            File.Delete(filePath);
        }

        /// <summary>
        /// 确定给定路径是否引用磁盘上的现有目录
        /// </summary>
        /// <param name="path">The path to test</param>
        /// <returns>
        /// 如果path指的是现有目录，则为true; 如果目录不存在或尝试确定指定的文件是否存在时发生错误，则返回false
        /// </returns>
        public virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 将文件或目录及其内容移动到新位置
        /// </summary>
        /// <param name="sourceDirName">The path of the file or directory to move</param>
        /// <param name="destDirName">
        /// sourceDirName的新位置的路径。 如果sourceDirName是文件，则destDirName也必须是文件名
        /// </param>
        public virtual void DirectoryMove(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }

        /// <summary>
        /// 返回与指定路径中的搜索模式匹配的可枚举文件名集合，并可选择搜索子目录。
        /// </summary>
        /// <param name="directoryPath">要搜索的目录的路径</param>
        /// <param name="searchPattern">
        /// 用于匹配路径中文件名称的搜索字符串。 此参数可以包含有效文字路径和通配符（*和？）字符的组合，但不支持正则表达式。
        /// </param>
        /// <param name="topDirectoryOnly">
        /// 指定是搜索当前目录，还是搜索当前目录和所有子目录
        /// </param>
        /// <returns>
        /// 可由路径指定的目录中的文件的全名（包括路径）的可枚举集合，它们与指定的搜索模式匹配
        /// </returns>
        public virtual IEnumerable<string> EnumerateFiles(string directoryPath, string searchPattern,
            bool topDirectoryOnly = true)
        {
            return Directory.EnumerateFiles(directoryPath, searchPattern,
                topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        /// <summary>
        /// 将现有文件复制到新文件。 允许覆盖同名文件
        /// </summary>
        /// <param name="sourceFileName">The file to copy</param>
        /// <param name="destFileName">The name of the destination file. This cannot be a directory</param>
        /// <param name="overwrite">true if the destination file can be overwritten; otherwise, false</param>
        public virtual void FileCopy(string sourceFileName, string destFileName, bool overwrite = false)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        /// <summary>
        /// 确定指定的文件是否存在
        /// </summary>
        /// <param name="filePath">The file to check</param>
        /// <returns>
        /// True if the caller has the required permissions and path contains the name of an existing file; otherwise,
        /// false.
        /// </returns>
        public virtual bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 获取文件的长度（以字节为单位），或者为目录或不存在的文件获取-1
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>The length of the file</returns>
        public virtual long FileLength(string path)
        {
            if (!FileExists(path))
                return -1;

            return new FileInfo(path).Length;
        }

        /// <summary>
        /// 将指定文件移动到新位置，提供指定新文件名的选项
        /// </summary>
        /// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path</param>
        /// <param name="destFileName">The new path and name for the file</param>
        public virtual void FileMove(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

        /// <summary>
        /// 返回目录的绝对路径
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The absolute path to the directory</returns>
        public virtual string GetAbsolutePath(params string[] paths)
        {
            var allPaths = paths.ToList();
            allPaths.Insert(0, Root);

            return Path.Combine(allPaths.ToArray());
        }

        /// <summary>
        /// 获取一个System.Security.AccessControl.DirectorySecurity对象，该对象封装指定目录的访问控制列表（ACL）条目
        /// </summary>
        /// <param name="path">The path to a directory containing a System.Security.AccessControl.DirectorySecurity object that describes the file's access control list (ACL) information</param>
        /// <returns>An object that encapsulates the access control rules for the file described by the path parameter</returns>
        public virtual DirectorySecurity GetAccessControl(string path)
        {
            return new DirectoryInfo(path).GetAccessControl();
        }

        /// <summary>
        /// 返回指定文件或目录的创建日期和时间
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information</param>
        /// <returns>
        /// A System.DateTime structure set to the creation date and time for the specified file or directory. This value
        /// is expressed in local time
        /// </returns>
        public virtual DateTime GetCreationTime(string path)
        {
            return File.GetCreationTime(path);
        }

        /// <summary>
        /// 返回与指定目录中指定搜索模式匹配的子目录（包括其路径）的名称
        /// </summary>
        /// <param name="path">The path to the directory to search</param>
        /// <param name="searchPattern">
        /// 搜索字符串与路径中子目录的名称匹配。 此参数可以包含有效文字和通配符的组合，但不支持正则表达式。
        /// </param>
        /// <param name="topDirectoryOnly">
        /// Specifies whether to search the current directory, or the current directory and all
        /// subdirectories
        /// </param>
        /// <returns>
        /// An array of the full names (including paths) of the subdirectories that match
        /// the specified criteria, or an empty array if no directories are found
        /// </returns>
        public virtual string[] GetDirectories(string path, string searchPattern = "", bool topDirectoryOnly = true)
        {
            if (string.IsNullOrEmpty(searchPattern))
                searchPattern = "*";

            return Directory.GetDirectories(path, searchPattern,
                topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        /// <summary>
        /// 返回指定路径字符串的目录信息
        /// </summary>
        /// <param name="path">The path of a file or directory</param>
        /// <returns>
        /// Directory information for path, or null if path denotes a root directory or is null. Returns
        /// System.String.Empty if path does not contain directory information
        /// </returns>
        public virtual string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// 仅返回指定路径字符串的目录名称
        /// </summary>
        /// <param name="path">The path of directory</param>
        /// <returns>The directory name</returns>
        public virtual string GetDirectoryNameOnly(string path)
        {
            return new DirectoryInfo(path).Name;
        }

        /// <summary>
        /// 返回指定路径字符串的扩展名
        /// </summary>
        /// <param name="filePath">The path string from which to get the extension</param>
        /// <returns>The extension of the specified path (including the period ".")</returns>
        public virtual string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        /// 返回指定路径字符串的文件名和扩展名
        /// </summary>
        /// <param name="path">The path string from which to obtain the file name and extension</param>
        /// <returns>The characters after the last directory character in path</returns>
        public virtual string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// 返回没有扩展名的指定路径字符串的文件名
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>The file name, minus the last period (.) and all characters following it</returns>
        public virtual string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// 返回与指定目录中指定搜索模式匹配的文件名（包括其路径），使用值确定是否搜索子目录。
        /// </summary>
        /// <param name="directoryPath">The path to the directory to search</param>
        /// <param name="searchPattern">
        /// 用于匹配路径中文件名称的搜索字符串。 此参数可以包含有效文字路径和通配符（*和？）字符的组合，但不支持正则表达式。
        /// </param>
        /// <param name="topDirectoryOnly">
        /// 指定是搜索当前目录，还是搜索当前目录和所有子目录
        /// </param>
        /// <returns>
        /// 指定目录中与指定搜索模式匹配的文件的全名（包括路径）数组，如果未找到任何文件，则为空数组
        /// </returns>
        public virtual string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true)
        {
            if (string.IsNullOrEmpty(searchPattern))
                searchPattern = "*.*";

            return Directory.GetFiles(directoryPath, searchPattern,
                topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        /// <summary>
        /// 返回上次访问指定文件或目录的日期和时间
        /// </summary>
        /// <param name="path">The file or directory for which to obtain access date and time information</param>
        /// <returns>A System.DateTime structure set to the date and time that the specified file</returns>
        public virtual DateTime GetLastAccessTime(string path)
        {
            return File.GetLastAccessTime(path);
        }

        /// <summary>
        /// 返回上次写入指定文件或目录的日期和时间
        /// </summary>
        /// <param name="path">The file or directory for which to obtain write date and time information</param>
        /// <returns>
        /// A System.DateTime structure set to the date and time that the specified file or directory was last written to.
        /// This value is expressed in local time
        /// </returns>
        public virtual DateTime GetLastWriteTime(string path)
        {
            return File.GetLastWriteTime(path);
        }

        /// <summary>
        /// 以协调世界时（UTC）返回上次写入指定文件或目录的日期和时间
        /// </summary>
        /// <param name="path">The file or directory for which to obtain write date and time information</param>
        /// <returns>
        /// A System.DateTime structure set to the date and time that the specified file or directory was last written to.
        /// This value is expressed in UTC time
        /// </returns>
        public virtual DateTime GetLastWriteTimeUtc(string path)
        {
            return File.GetLastWriteTimeUtc(path);
        }

        /// <summary>
        /// 检索指定路径的父目录
        /// </summary>
        /// <param name="directoryPath">The path for which to retrieve the parent directory</param>
        /// <returns>The parent directory, or null if path is the root directory, including the root of a UNC server or share name</returns>
        public virtual string GetParentDirectory(string directoryPath)
        {
            return Directory.GetParent(directoryPath).FullName;
        }

        /// <summary>
        /// 检查路径是否是目录
        /// </summary>
        /// <param name="path">Path for check</param>
        /// <returns>True, if the path is a directory, otherwise false</returns>
        public virtual bool IsDirectory(string path)
        {
            return DirectoryExists(path);
        }

        /// <summary>
        /// 将虚拟路径映射到物理磁盘路径
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string MapPath(string path)
        {
            path = path.Replace("~/", string.Empty).TrimStart('\\').Replace('\\', '/');
            return Path.Combine(BaseDirectory ?? string.Empty, path);
        }

        /// <summary>
        /// 将文件的内容读入字节数组
        /// </summary>
        /// <param name="filePath">The file for reading</param>
        /// <returns>A byte array containing the contents of the file</returns>
        public virtual byte[] ReadAllBytes(string filePath)
        {
            return File.Exists(filePath) ? File.ReadAllBytes(filePath) : new byte[0];
        }

        /// <summary>
        /// 打开文件，使用指定的编码读取文件的所有行，然后关闭该文件。
        /// </summary>
        /// <param name="path">The file to open for reading</param>
        /// <param name="encoding">The encoding applied to the contents of the file</param>
        /// <returns>A string containing all lines of the file</returns>
        public virtual string ReadAllText(string path, Encoding encoding)
        {
            return File.ReadAllText(path, encoding);
        }

        /// <summary>
        /// 以协调世界时（UTC）设置上次写入指定文件的日期和时间
        /// </summary>
        /// <param name="path">The file for which to set the date and time information</param>
        /// <param name="lastWriteTimeUtc">
        /// A System.DateTime containing the value to set for the last write date and time of path.
        /// This value is expressed in UTC time
        /// </param>
        public virtual void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
        }

        /// <summary>
        /// 将指定的字节数组写入文件
        /// </summary>
        /// <param name="filePath">The file to write to</param>
        /// <param name="bytes">The bytes to write to the file</param>
        public virtual void WriteAllBytes(string filePath, byte[] bytes)
        {
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// 创建新文件，使用指定的编码将指定的字符串写入文件，然后关闭该文件。 如果目标文件已存在，则会被覆盖。
        /// </summary>
        /// <param name="path">The file to write to</param>
        /// <param name="contents">The string to write to the file</param>
        /// <param name="encoding">The encoding to apply to the string</param>
        public virtual void WriteAllText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents, encoding);
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <typeparam name="T">反序列化对象</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>反序列化对象</returns>
        public T GetFileContent<T>(string filePath) where T : new()
        {
            var text = ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
                return new T();

            // 从JSON文件中获取插件系统名称
            return JsonConvert.DeserializeObject<T>(text);
        }

        /// <summary>
        /// 基础路径
        /// </summary>
        protected string BaseDirectory { get; }
    }
}