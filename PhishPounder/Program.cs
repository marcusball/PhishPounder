using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace PhishPounder {
    
    public class FormMain {
        private static BackgroundWorker _BackgroundWorker;
        private static Queue<Func<string>> _Commands;
        private static Random _Random;
        private static HttpStatusCode? previousStatus = null;
        private static long bytesSent = 0;

        private static string _tempEmail;
        private static string _tempPassword;

        static void Main(string[] args) {
            _Random = new Random();
            _BackgroundWorker = new BackgroundWorker();

            _BackgroundWorker.WorkerReportsProgress = true;
            _BackgroundWorker.WorkerSupportsCancellation = true;
            _BackgroundWorker.DoWork += backgroundWorker_DoWork;
            _BackgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            _BackgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            _BackgroundWorker.RunWorkerAsync();

            Console.Out.WriteLine("Press any key to stop...\n\n");

            Console.ReadKey();

            _BackgroundWorker.CancelAsync();
        }

        private static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            int i = 0;
            while (!_BackgroundWorker.CancellationPending) {
                i += 1;
                SendPostRequest(i);
            }
            Console.Out.WriteLine();
        }

        public static void SendPostRequest(int requestNumber) {
            int num = _Random.Next(1, 4);
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("http://tumblr.com.accounts.login.userid." + _Random.Next(1, 50000) + ".opl9.pw/tm/" + num + "/log.php");

            ASCIIEncoding encoding = new ASCIIEncoding();

            _tempEmail = RandomString(_Random.Next(10,500));
            _tempPassword = RandomString(_Random.Next(5, 500));

            bytesSent += _tempPassword.Length + _tempPassword.Length;

            string postData = "email=" + _tempEmail;
            postData += "&pass=" + _tempPassword;

            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            httpWReq.AllowAutoRedirect = false;

            using (Stream stream = httpWReq.GetRequestStream()) {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
            if (response.StatusCode != previousStatus && previousStatus != null) {
                Console.Out.WriteLine("\a"); //Ring the bell
                string text = new StreamReader(response.GetResponseStream()).ReadToEnd();
                SetStatusMessage(requestNumber, response.StatusCode.ToString(), bytesSent, _tempEmail, _tempPassword);
                Console.Out.WriteLine("\n{0}\n", text);
            }
            else {
                SetStatusMessage(requestNumber, response.StatusCode.ToString(), bytesSent, _tempEmail, _tempPassword);
            }
            previousStatus = response.StatusCode;
        }

        public static void SetStatusMessage(int number, string statusCode, long sent,string email, string password) {
            Console.CursorVisible = false;
            Console.CursorLeft = 0;
            Console.CursorTop -= 1; //Go up one line

            Console.Out.Write("Sent request #{0}. Status: {1}. Total bytes sent: {2}.\nPartial email: \"{3}\". Partial pass: \"{4}\".", number, statusCode, sent, (email.Length > 15) ? email.Substring(0, 15) : email, (password.Length > 15) ? password.Substring(0, 15) : password);
        }

        private static string RandomString(int size) {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++) {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(58 * _Random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        static void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
        }

        private static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            AddMessage("Finished");
        }

        private static void AddMessage(string message) {
            Console.Out.WriteLine(message);
        }
    }
}
