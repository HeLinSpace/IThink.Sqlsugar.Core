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
    /// �ļ��ṩ��
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
        /// ���·��
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The combined paths</returns>
        public virtual string Combine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        /// <summary>
        /// ����ָ��·���е�����Ŀ¼����Ŀ¼
        /// </summary>
        /// <param name="path">The directory to create</param>
        public virtual void CreateDirectory(string path)
        {
            if (!DirectoryExists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// �����򸲸�ָ��·���е��ļ�
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
        /// ������ȵݹ�ɾ������Windows��Դ�������д򿪺��Ŀ¼�Ĵ���
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
        /// ɾ��ָ�����ļ�
        /// </summary>
        /// <param name="filePath">The name of the file to be deleted. Wildcard characters are not supported</param>
        public virtual void DeleteFile(string filePath)
        {
            if (!FileExists(filePath))
                return;

            File.Delete(filePath);
        }

        /// <summary>
        /// ȷ������·���Ƿ����ô����ϵ�����Ŀ¼
        /// </summary>
        /// <param name="path">The path to test</param>
        /// <returns>
        /// ���pathָ��������Ŀ¼����Ϊtrue; ���Ŀ¼�����ڻ���ȷ��ָ�����ļ��Ƿ����ʱ���������򷵻�false
        /// </returns>
        public virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// ���ļ���Ŀ¼���������ƶ�����λ��
        /// </summary>
        /// <param name="sourceDirName">The path of the file or directory to move</param>
        /// <param name="destDirName">
        /// sourceDirName����λ�õ�·���� ���sourceDirName���ļ�����destDirNameҲ�������ļ���
        /// </param>
        public virtual void DirectoryMove(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }

        /// <summary>
        /// ������ָ��·���е�����ģʽƥ��Ŀ�ö���ļ������ϣ�����ѡ��������Ŀ¼��
        /// </summary>
        /// <param name="directoryPath">Ҫ������Ŀ¼��·��</param>
        /// <param name="searchPattern">
        /// ����ƥ��·�����ļ����Ƶ������ַ����� �˲������԰�����Ч����·����ͨ�����*�ͣ����ַ�����ϣ�����֧��������ʽ��
        /// </param>
        /// <param name="topDirectoryOnly">
        /// ָ����������ǰĿ¼������������ǰĿ¼��������Ŀ¼
        /// </param>
        /// <returns>
        /// ����·��ָ����Ŀ¼�е��ļ���ȫ��������·�����Ŀ�ö�ټ��ϣ�������ָ��������ģʽƥ��
        /// </returns>
        public virtual IEnumerable<string> EnumerateFiles(string directoryPath, string searchPattern,
            bool topDirectoryOnly = true)
        {
            return Directory.EnumerateFiles(directoryPath, searchPattern,
                topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        /// <summary>
        /// �������ļ����Ƶ����ļ��� ������ͬ���ļ�
        /// </summary>
        /// <param name="sourceFileName">The file to copy</param>
        /// <param name="destFileName">The name of the destination file. This cannot be a directory</param>
        /// <param name="overwrite">true if the destination file can be overwritten; otherwise, false</param>
        public virtual void FileCopy(string sourceFileName, string destFileName, bool overwrite = false)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        /// <summary>
        /// ȷ��ָ�����ļ��Ƿ����
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
        /// ��ȡ�ļ��ĳ��ȣ����ֽ�Ϊ��λ��������ΪĿ¼�򲻴��ڵ��ļ���ȡ-1
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
        /// ��ָ���ļ��ƶ�����λ�ã��ṩָ�����ļ�����ѡ��
        /// </summary>
        /// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path</param>
        /// <param name="destFileName">The new path and name for the file</param>
        public virtual void FileMove(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

        /// <summary>
        /// ����Ŀ¼�ľ���·��
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
        /// ��ȡһ��System.Security.AccessControl.DirectorySecurity���󣬸ö����װָ��Ŀ¼�ķ��ʿ����б�ACL����Ŀ
        /// </summary>
        /// <param name="path">The path to a directory containing a System.Security.AccessControl.DirectorySecurity object that describes the file's access control list (ACL) information</param>
        /// <returns>An object that encapsulates the access control rules for the file described by the path parameter</returns>
        public virtual DirectorySecurity GetAccessControl(string path)
        {
            return new DirectoryInfo(path).GetAccessControl();
        }

        /// <summary>
        /// ����ָ���ļ���Ŀ¼�Ĵ������ں�ʱ��
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
        /// ������ָ��Ŀ¼��ָ������ģʽƥ�����Ŀ¼��������·����������
        /// </summary>
        /// <param name="path">The path to the directory to search</param>
        /// <param name="searchPattern">
        /// �����ַ�����·������Ŀ¼������ƥ�䡣 �˲������԰�����Ч���ֺ�ͨ�������ϣ�����֧��������ʽ��
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
        /// ����ָ��·���ַ�����Ŀ¼��Ϣ
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
        /// ������ָ��·���ַ�����Ŀ¼����
        /// </summary>
        /// <param name="path">The path of directory</param>
        /// <returns>The directory name</returns>
        public virtual string GetDirectoryNameOnly(string path)
        {
            return new DirectoryInfo(path).Name;
        }

        /// <summary>
        /// ����ָ��·���ַ�������չ��
        /// </summary>
        /// <param name="filePath">The path string from which to get the extension</param>
        /// <returns>The extension of the specified path (including the period ".")</returns>
        public virtual string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        /// ����ָ��·���ַ������ļ�������չ��
        /// </summary>
        /// <param name="path">The path string from which to obtain the file name and extension</param>
        /// <returns>The characters after the last directory character in path</returns>
        public virtual string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// ����û����չ����ָ��·���ַ������ļ���
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>The file name, minus the last period (.) and all characters following it</returns>
        public virtual string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// ������ָ��Ŀ¼��ָ������ģʽƥ����ļ�����������·������ʹ��ֵȷ���Ƿ�������Ŀ¼��
        /// </summary>
        /// <param name="directoryPath">The path to the directory to search</param>
        /// <param name="searchPattern">
        /// ����ƥ��·�����ļ����Ƶ������ַ����� �˲������԰�����Ч����·����ͨ�����*�ͣ����ַ�����ϣ�����֧��������ʽ��
        /// </param>
        /// <param name="topDirectoryOnly">
        /// ָ����������ǰĿ¼������������ǰĿ¼��������Ŀ¼
        /// </param>
        /// <returns>
        /// ָ��Ŀ¼����ָ������ģʽƥ����ļ���ȫ��������·�������飬���δ�ҵ��κ��ļ�����Ϊ������
        /// </returns>
        public virtual string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true)
        {
            if (string.IsNullOrEmpty(searchPattern))
                searchPattern = "*.*";

            return Directory.GetFiles(directoryPath, searchPattern,
                topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        /// <summary>
        /// �����ϴη���ָ���ļ���Ŀ¼�����ں�ʱ��
        /// </summary>
        /// <param name="path">The file or directory for which to obtain access date and time information</param>
        /// <returns>A System.DateTime structure set to the date and time that the specified file</returns>
        public virtual DateTime GetLastAccessTime(string path)
        {
            return File.GetLastAccessTime(path);
        }

        /// <summary>
        /// �����ϴ�д��ָ���ļ���Ŀ¼�����ں�ʱ��
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
        /// ��Э������ʱ��UTC�������ϴ�д��ָ���ļ���Ŀ¼�����ں�ʱ��
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
        /// ����ָ��·���ĸ�Ŀ¼
        /// </summary>
        /// <param name="directoryPath">The path for which to retrieve the parent directory</param>
        /// <returns>The parent directory, or null if path is the root directory, including the root of a UNC server or share name</returns>
        public virtual string GetParentDirectory(string directoryPath)
        {
            return Directory.GetParent(directoryPath).FullName;
        }

        /// <summary>
        /// ���·���Ƿ���Ŀ¼
        /// </summary>
        /// <param name="path">Path for check</param>
        /// <returns>True, if the path is a directory, otherwise false</returns>
        public virtual bool IsDirectory(string path)
        {
            return DirectoryExists(path);
        }

        /// <summary>
        /// ������·��ӳ�䵽�������·��
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string MapPath(string path)
        {
            path = path.Replace("~/", string.Empty).TrimStart('\\').Replace('\\', '/');
            return Path.Combine(BaseDirectory ?? string.Empty, path);
        }

        /// <summary>
        /// ���ļ������ݶ����ֽ�����
        /// </summary>
        /// <param name="filePath">The file for reading</param>
        /// <returns>A byte array containing the contents of the file</returns>
        public virtual byte[] ReadAllBytes(string filePath)
        {
            return File.Exists(filePath) ? File.ReadAllBytes(filePath) : new byte[0];
        }

        /// <summary>
        /// ���ļ���ʹ��ָ���ı����ȡ�ļ��������У�Ȼ��رո��ļ���
        /// </summary>
        /// <param name="path">The file to open for reading</param>
        /// <param name="encoding">The encoding applied to the contents of the file</param>
        /// <returns>A string containing all lines of the file</returns>
        public virtual string ReadAllText(string path, Encoding encoding)
        {
            return File.ReadAllText(path, encoding);
        }

        /// <summary>
        /// ��Э������ʱ��UTC�������ϴ�д��ָ���ļ������ں�ʱ��
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
        /// ��ָ�����ֽ�����д���ļ�
        /// </summary>
        /// <param name="filePath">The file to write to</param>
        /// <param name="bytes">The bytes to write to the file</param>
        public virtual void WriteAllBytes(string filePath, byte[] bytes)
        {
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// �������ļ���ʹ��ָ���ı��뽫ָ�����ַ���д���ļ���Ȼ��رո��ļ��� ���Ŀ���ļ��Ѵ��ڣ���ᱻ���ǡ�
        /// </summary>
        /// <param name="path">The file to write to</param>
        /// <param name="contents">The string to write to the file</param>
        /// <param name="encoding">The encoding to apply to the string</param>
        public virtual void WriteAllText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents, encoding);
        }

        /// <summary>
        /// ��ȡ�ļ�����
        /// </summary>
        /// <typeparam name="T">�����л�����</typeparam>
        /// <param name="filePath">�ļ�·��</param>
        /// <returns>�����л�����</returns>
        public T GetFileContent<T>(string filePath) where T : new()
        {
            var text = ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
                return new T();

            // ��JSON�ļ��л�ȡ���ϵͳ����
            return JsonConvert.DeserializeObject<T>(text);
        }

        /// <summary>
        /// ����·��
        /// </summary>
        protected string BaseDirectory { get; }
    }
}