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

        public void DeleteFile(String FilePath)
        {
            new HttpClient().GetByteArrayAsync($"{Properties.ServerIP}/Delete.php?FilePath={UrlEncoder.Default.Encode(FilePath)}&username={UrlEncoder.Default.Encode(this.Username)}&password={UrlEncoder.Default.Encode(this.Password)}");
        }

        public void MoveFile(String SourceFilePath,String FileNewPath)
        {
            new HttpClient().GetByteArrayAsync($"{Properties.ServerIP}/Download.php?FilePath={UrlEncoder.Default.Encode(SourceFilePath)}&username={UrlEncoder.Default.Encode(this.Username)}&password={UrlEncoder.Default.Encode(this.Password)}");
        }
    }
}
