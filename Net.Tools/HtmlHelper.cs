using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Net.Tools
{
    public class HtmlHelper
    {
        public HtmlHelper()
        {
            Timeout = 100000;
            MaxRequest = 5;
            SleepTime = 500;
        }

        /// <summary>
        /// 请求超时，单位毫秒，默认10000
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// 最大请求次数，默认5次
        /// </summary>
        public int MaxRequest { get; set; }
        /// <summary>
        /// 请求失败后，睡眠时间，默认500
        /// </summary>
        public int SleepTime { get; set; }
        public string Get(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");

            int i = 0;
            while (true)
            {
                try
                {
                    var request = WebRequest.Create(url) as HttpWebRequest;
                    request.Method = "GET";
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                    request.Timeout = Timeout;

                    var response = request.GetResponse() as HttpWebResponse;
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    if (i > MaxRequest) throw ex;

                    Thread.Sleep(SleepTime);
                }
                finally
                {
                    i++;
                }
            }
        }
    }
}
