/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 文件提供者接口
    /// </summary>
    public interface IThinkFileProvider :IFileProvider
    {
        /// <summary>
        /// 组合路径
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The combined paths</returns>
        string Combine(params string[] paths);

        /// <summary>
        /// 创建指定路径中的所有目录和子目录
        /// </summary>
        /// <param name="path">The directory to create</param>
        void CreateDirectory(string path);

        /// <summary>
        /// 创建或覆盖指定路径中的文件
        /// </summary>
        /// <param name="path">The path and name of the file to create</param>
        void CreateFile(string path);

        /// <summary>
        /// 深度优先递归删除，在Windows资源管理器中打开后代目录的处理。
        /// </summary>
        /// <param name="path">Directory path</param>
        void DeleteDirectory(string path);

        /// <summary>
        /// 删除指定的文件
        /// </summary>
        /// <param name="filePath">要删除的文件的名称。 不支持通配符</param>
        void DeleteFile(string filePath);

        /// <summary>
        /// 确定给定路径是否引用磁盘上的现有目录
        /// </summary>
        /// <param name="path">The path to test</param>
        /// <returns>
        /// 如果path指的是现有目录，则为true; 如果目录不存在或尝试确定指定的文件是否存在时发生错误，则返回false
        /// </returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// 将文件或目录及其内容移动到新位置
        /// </summary>
        /// <param name="sourceDirName">The path of the file or directory to move</param>
        /// <param name="destDirName">
        /// sourceDirName的新位置的路径。 如果sourceDirName是文件，则destDirName也必须是文件名
        /// </param>
        void DirectoryMove(string sourceDirName, string destDirName);

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
        IEnumerable<string> EnumerateFiles(string directoryPath, string searchPattern, bool topDirectoryOnly = true);

        /// <summary>
        /// 将现有文件复制到新文件。 允许覆盖同名文件
        /// </summary>
        /// <param name="sourceFileName">The file to copy</param>
        /// <param name="destFileName">The name of the destination file. This cannot be a directory</param>
        /// <param name="overwrite">true if the destination file can be overwritten; otherwise, false</param>
        void FileCopy(string sourceFileName, string destFileName, bool overwrite = false);

        /// <summary>
        /// 确定指定的文件是否存在
        /// </summary>
        /// <param name="filePath">The file to check</param>
        /// <returns>
        /// True if the caller has the required permissions and path contains the name of an existing file; otherwise,
        /// false.
        /// </returns>
        bool FileExists(string filePath);

        /// <summary>
        /// 获取文件的长度（以字节为单位），或者为目录或不存在的文件获取-1
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>The length of the file</returns>
        long FileLength(string path);

        /// <summary>
        /// 将指定文件移动到新位置，提供指定新文件名的选项
        /// </summary>
        /// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path</param>
        /// <param name="destFileName">The new path and name for the file</param>
        void FileMove(string sourceFileName, string destFileName);

        /// <summary>
        /// 返回目录的绝对路径
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The absolute path to the directory</returns>
        string GetAbsolutePath(params string[] paths);

        /// <summary>
        /// 获取一个System.Security.AccessControl.DirectorySecurity对象，该对象封装指定目录的访问控制列表（ACL）条目
        /// </summary>
        /// <param name="path">The path to a directory containing a System.Security.AccessControl.DirectorySecurity object that describes the file's access control list (ACL) information</param>
        /// <returns>An object that encapsulates the access control rules for the file described by the path parameter</returns>
        DirectorySecurity GetAccessControl(string path);

        /// <summary>
        /// 返回指定文件或目录的创建日期和时间
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information</param>
        /// <returns>
        /// A System.DateTime structure set to the creation date and time for the specified file or directory. This value
        /// is expressed in local time
        /// </returns>
        DateTime GetCreationTime(string path);

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
        string[] GetDirectories(string path, string searchPattern = "", bool topDirectoryOnly = true);

        /// <summary>
        /// 返回指定路径字符串的目录信息
        /// </summary>
        /// <param name="path">The path of a file or directory</param>
        /// <returns>
        /// Directory information for path, or null if path denotes a root directory or is null. Returns
        /// System.String.Empty if path does not contain directory information
        /// </returns>
        string GetDirectoryName(string path);

        /// <summary>
        /// 仅返回指定路径字符串的目录名称
        /// </summary>
        /// <param name="path">The path of directory</param>
        /// <returns>The directory name</returns>
        string GetDirectoryNameOnly(string path);

        /// <summary>
        /// 返回指定路径字符串的扩展名
        /// </summary>
        /// <param name="filePath">The path string from which to get the extension</param>
        /// <returns>The extension of the specified path (including the period ".")</returns>
        string GetFileExtension(string filePath);

        /// <summary>
        /// 返回指定路径字符串的文件名和扩展名
        /// </summary>
        /// <param name="path">The path string from which to obtain the file name and extension</param>
        /// <returns>The characters after the last directory character in path</returns>
        string GetFileName(string path);

        /// <summary>
        /// 返回没有扩展名的指定路径字符串的文件名
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>The file name, minus the last period (.) and all characters following it</returns>
        string GetFileNameWithoutExtension(string filePath);

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
        string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true);

        /// <summary>
        /// 返回上次访问指定文件或目录的日期和时间
        /// </summary>
        /// <param name="path">The file or directory for which to obtain access date and time information</param>
        /// <returns>A System.DateTime structure set to the date and time that the specified file</returns>
        DateTime GetLastAccessTime(string path);

        /// <summary>
        /// 返回上次写入指定文件或目录的日期和时间
        /// </summary>
        /// <param name="path">The file or directory for which to obtain write date and time information</param>
        /// <returns>
        /// A System.DateTime structure set to the date and time that the specified file or directory was last written to.
        /// This value is expressed in local time
        /// </returns>
        DateTime GetLastWriteTime(string path);

        /// <summary>
        /// 以协调世界时（UTC）返回上次写入指定文件或目录的日期和时间
        /// </summary>
        /// <param name="path">The file or directory for which to obtain write date and time information</param>
        /// <returns>
        /// A System.DateTime structure set to the date and time that the specified file or directory was last written to.
        /// This value is expressed in UTC time
        /// </returns>
        DateTime GetLastWriteTimeUtc(string path);

        /// <summary>
        /// 检索指定路径的父目录
        /// </summary>
        /// <param name="directoryPath">The path for which to retrieve the parent directory</param>
        /// <returns>The parent directory, or null if path is the root directory, including the root of a UNC server or share name</returns>
        string GetParentDirectory(string directoryPath);

        /// <summary>
        /// 检查路径是否是目录
        /// </summary>
        /// <param name="path">Path for check</param>
        /// <returns>True, if the path is a directory, otherwise false</returns>
        bool IsDirectory(string path);

        /// <summary>
        /// 将虚拟路径映射到物理磁盘路径
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        string MapPath(string path);

        /// <summary>
        /// 将文件的内容读入字节数组
        /// </summary>
        /// <param name="filePath">The file for reading</param>
        /// <returns>A byte array containing the contents of the file</returns>
        byte[] ReadAllBytes(string filePath);

        /// <summary>
        /// 打开文件，使用指定的编码读取文件的所有行，然后关闭该文件。
        /// </summary>
        /// <param name="path">The file to open for reading</param>
        /// <param name="encoding">The encoding applied to the contents of the file</param>
        /// <returns>A string containing all lines of the file</returns>
        string ReadAllText(string path, Encoding encoding);

        /// <summary>
        /// 以协调世界时（UTC）设置上次写入指定文件的日期和时间
        /// </summary>
        /// <param name="path">The file for which to set the date and time information</param>
        /// <param name="lastWriteTimeUtc">
        /// A System.DateTime containing the value to set for the last write date and time of path.
        /// This value is expressed in UTC time
        /// </param>
        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <summary>
        /// 将指定的字节数组写入文件
        /// </summary>
        /// <param name="filePath">The file to write to</param>
        /// <param name="bytes">The bytes to write to the file</param>
        void WriteAllBytes(string filePath, byte[] bytes);

        /// <summary>
        /// 创建新文件，使用指定的编码将指定的字符串写入文件，然后关闭该文件。 如果目标文件已存在，则会被覆盖。
        /// </summary>
        /// <param name="path">The file to write to</param>
        /// <param name="contents">The string to write to the file</param>
        /// <param name="encoding">The encoding to apply to the string</param>
        void WriteAllText(string path, string contents, Encoding encoding);

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <typeparam name="T">反序列化对象</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>反序列化对象</returns>
        T GetFileContent<T>(string filePath) where T : new();
    }
}