using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FilesUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            Uploader U = new Uploader("C:/users");
            U.Start();
            //Thread.Sleep(10000);
            //U.Suspend();
            //Thread.Sleep(5000);
            //U.Start();
        }
    }
}
