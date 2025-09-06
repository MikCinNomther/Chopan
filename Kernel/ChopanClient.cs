using Chopan.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Chopan.Kernel
{
    public class ChopanClient
    {
        public enum LoginState
        {
            Success, PasswordError, Locked, NetworkError, ServerError, UnknownError, UserError
        }

        public static async Task<LoginState> Login(string username, string password)
        {
            username = UrlEncoder.Default.Encode(username);
            password = UrlEncoder.Default.Encode(password);
            HttpClient httpClient = new HttpClient();
            string ResultStr = "";
            try
            {
                ResultStr = await httpClient.GetStringAsync($"{Properties.ServerIP}/Login.php?username={username}&password={password}");
            }
            catch
            {
                return LoginState.NetworkError;
            }
            LoginState Result = LoginState.UnknownError;
            if (ResultStr == "Success")
            {
                Result = LoginState.Success;
            }
            else if (ResultStr == "PasswordError")
            {
                Result = LoginState.PasswordError;
            }
            else if (ResultStr == "UserError")
            {
                Result = LoginState.UserError;
            }
            else if (ResultStr.IndexOf("Found") != -1)
            {
                Result = LoginState.ServerError;
            }
            else if (ResultStr == "Locked")
            {
                Result = LoginState.Locked;
            }

            return Result;
        }

        // 移除同步方法，全部用 async/await
        string Username, Password;
        public ChopanClient(string UserName, string PassWord)
        {
            this.Username = UserName;
            this.Password = PassWord;
        }

        // State 屬性改為 async 方法
        public async Task<LoginState> GetStateAsync()
        {
            return await Login(this.Username, this.Password);
        }
        public void CreateDirectory(String DirectoryPath)
        {
            new HttpClient().GetByteArrayAsync($"{Properties.ServerIP}/CreateDirectory.php?DirectoryPath={UrlEncoder.Default.Encode(DirectoryPath)}&username={UrlEncoder.Default.Encode(this.Username)}&password={UrlEncoder.Default.Encode(this.Password)}");
        }
        public async Task<string[]> GetDirectoriesAsync(string DirectoryRoot = "/")
        {
            DirectoryRoot = UrlEncoder.Default.Encode(DirectoryRoot);
            try
            {
                HttpClient httpClient = new HttpClient();
                String ResultStr = httpClient.GetStringAsync($"{Properties.ServerIP}/GetDirectories.php?DirectoryRoot={DirectoryRoot}&username={UrlEncoder.Default.Encode(this.Username)}&password={UrlEncoder.Default.Encode(this.Password)}").Result;
                return ResultStr.Split("\r\n");
            }
            catch
            {
                return null;
            }
        }

        public void MoveDirectory(String SourceDirectoryPath, String DirectoryNewPath)
        {
            new HttpClient().GetByteArrayAsync($"{Properties.ServerIP}/MoveDirectory.php?SourceDirectoryPath={UrlEncoder.Default.Encode(SourceDirectoryPath)}&DirectoryNewPath={UrlEncoder.Default.Encode(DirectoryNewPath)}&username={UrlEncoder.Default.Encode(this.Username)}&password={UrlEncoder.Default.Encode(this.Password)}").Wait();
        }

        public void RenameDirectory(string directoryPath, string newDirectoryName)
        {
            if (string.IsNullOrEmpty(directoryPath) || string.IsNullOrEmpty(newDirectoryName))
                return;

            // 去掉末尾斜線，保證路徑正確
            string path = directoryPath.TrimEnd('/');

            int lastSlash = path.LastIndexOf('/');
            string parentPath = lastSlash <= 0 ? "/" : path.Substring(0, lastSlash + 1);
            string newPath = parentPath + newDirectoryName;

            MoveDirectory(path, newPath);
        }

        public void RenameFile(string filePath, string newFileName)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(newFileName))
                return;
            int lastSlash = filePath.LastIndexOf('/');
            if (lastSlash < 0) return;
            string newFilePath = filePath.Substring(0, lastSlash + 1) + newFileName;
            MoveFile(filePath, newFilePath);
        }

        public string[] GetFiles(String DirectoryRoot)
        {
            DirectoryRoot = UrlEncoder.Default.Encode(DirectoryRoot);
            try
            {
                HttpClient httpClient = new HttpClient();
                String ResultStr = httpClient.GetStringAsync($"{Properties.ServerIP}/GetFiles.php?DirectoryRoot={DirectoryRoot}&username={UrlEncoder.Default.Encode(this.Username)}&password={UrlEncoder.Default.Encode(this.Password)}").Result;
                return ResultStr.Split("\r\n");
            }
            catch
            {
                return null;
            }
        }

        public void Upload(String FilePath, string SavePath)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(this.Username), "username");
                form.Add(new StringContent(this.Password), "password");
                form.Add(new StringContent(SavePath), "SavePath");
                byte[] Data = System.IO.File.ReadAllBytes(FilePath);
                form.Add(new ByteArrayContent(Data), "Data", new FileInfo(FilePath).Name);
                httpClient.PostAsync($"{Properties.ServerIP}/Upload.php", form).Wait();
            }
        }
        public byte[] DownloadData(String FilePath)
        {
            return new HttpClient().GetByteArrayAsync($"{Properties.ServerIP}/Download.php?FilePath={UrlEncoder.Default.Encode(FilePath)}&username={UrlEncoder.Default.Encode(this.Username)}&password={UrlEncoder.Default.Encode(this.Password)}").Result;
        }

        public void DeleteDirectory(String DirectoryPath)
        {
            new HttpClient().GetByteArrayAsync($"{Properties.ServerIP}/DeleteDirectory.php?DirectoryPath={UrlEncoder.Default.Encode(DirectoryPath)}&username={UrlEncoder.Default.Encode(this.Username)}&password={UrlEncoder.Default.Encode(this.Password)}");
        }

        public void DeleteFile(String FilePath)
        {
            new HttpClient().GetByteArrayAsync($"{Properties.ServerIP}/Delete.php?FilePath={UrlEncoder.Default.Encode(FilePath)}&username={UrlEncoder.Default.Encode(this.Username)}&password={UrlEncoder.Default.Encode(this.Password)}");
        }

        public void MoveFile(string sourceFilePath, string fileNewPath)
        {
            new HttpClient().GetByteArrayAsync(
                $"{Properties.ServerIP}/MoveFile.php?FilePath={UrlEncoder.Default.Encode(sourceFilePath)}" +
                $"&FileNewPath={UrlEncoder.Default.Encode(fileNewPath)}" +
                $"&username={UrlEncoder.Default.Encode(this.Username)}" +
                $"&password={UrlEncoder.Default.Encode(this.Password)}"
            ).Wait();
        }
    }
}
