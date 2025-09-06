using Chopan.Pages;
using System;
using System.Collections.Generic;
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
            Success,PasswordError,Locked,NetworkError,ServerError,UnknownError,UserError
        }

        public static async Task<LoginState> Login(string username, string password)
        {
            username = UrlEncoder.Default.Encode(username);
            password = UrlEncoder.Default.Encode(password);
            HttpClient httpClient = new HttpClient();
            string ResultStr = "";
            try
            {
                ResultStr = (await httpClient.GetStringAsync($"{Properties.ServerIP}/Login.php?username={username}&password={password}"));
            }
            catch
            {
                return LoginState.NetworkError;
            }
            LoginState Result = LoginState.UnknownError;
            if (ResultStr == "Success")
            {
                Result = LoginState.Success;
            }else if(ResultStr == "PasswordError")
            {
                Result = LoginState.PasswordError;
            }else if (ResultStr == "UserError")
            {
                Result = LoginState.UserError;
            }else if(ResultStr.IndexOf("Found") != -1)
            {
                Result = LoginState.ServerError;
            }else if(ResultStr == "Locked")
            {
                Result = LoginState.Locked;
            }

            return Result;
        }

        public static LoginState LoginSync(string username,string password)
        {
            Task<LoginState> task = Login(username, password);
            return task.Result;
        }
        string Username, Password;
        public ChopanClient(String UserName,String PassWord)
        {
            this.Username = UserName;
            this.Password = PassWord;
        }

        public LoginState State
        {
            get
            {
                return LoginSync(Username, Password);
            }
        }

        public string[] GetDirectories(String DirectoryRoot = "/")
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
            new HttpClient().GetByteArrayAsync($"{Properties.ServerIP}/MoveFile.php?FilePath={UrlEncoder.Default.Encode(SourceFilePath)}&username={UrlEncoder.Default.Encode(this.Username)}&password={UrlEncoder.Default.Encode(this.Password)}");
        }
    }
}
