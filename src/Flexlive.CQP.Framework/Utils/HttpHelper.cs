using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Flexlive.CQP.Framework.Utils
{
    /// <summary>
    /// Http访问的操作类。
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 向HTTP服务器发送Get请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="userAgent">User-Agent HTTP标头。</param>
        /// <param name="accept">Accept HTTP标头。</param>
        /// <param name="timeout">超时时间。</param>
        /// <param name="header">HTTP 标头。</param>
        /// <param name="cookies">Cookies。</param>
        /// <param name="encoding">文本编码。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Get(string url, string referer, string userAgent, string accept, int timeout, WebHeaderCollection header, CookieCollection cookies, Encoding encoding, DecompressionMethods decompression = DecompressionMethods.None)
        {
            string content = String.Empty;

            try
            {
                HttpWebRequest webRequest = null;

                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    //对服务端证书进行有效性校验（非第三方权威机构颁发的证书，如自己生成的，不进行验证，这里返回true）
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
                    webRequest.ProtocolVersion = HttpVersion.Version10;    //http版本，默认是1.1,这里设置为1.0
                }
                else
                {
                    webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
                }

                //初始WebRequest属性
                webRequest.UserAgent = userAgent;
                webRequest.Referer = referer;
                webRequest.Method = "GET";
                webRequest.Timeout = timeout;
                webRequest.Accept = accept;
                if (cookies != null)
                {
                    webRequest.CookieContainer = new CookieContainer();
                    webRequest.CookieContainer.Add(cookies);
                }
                if(header != null)
                {
                    webRequest.Headers = header;
                }
                webRequest.AutomaticDecompression = decompression;

                //接收返回字串
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                content = GetResponseString(webResponse, encoding);
            }
            catch
            {

            }

            return content;
        }

        /// <summary>
        /// 向HTTP服务器发送Get请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="header">HTTP 标头。</param>
        /// <param name="cookies">Cookies。</param>
        /// <param name="encoding">文本编码。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Get(string url, string referer, WebHeaderCollection header, CookieCollection cookies, Encoding encoding, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Get(url, referer, 
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.89 Safari/537.36", 
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                30000, header, cookies, encoding, decompression);
        }

        /// <summary>
        /// 向HTTP服务器发送Get请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="cookies">Cookies。</param>
        /// <param name="encoding">文本编码。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Get(string url, string referer, CookieCollection cookies, Encoding encoding, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Get(url, referer,
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.89 Safari/537.36",
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                30000, null, cookies, encoding, decompression);
        }

        /// <summary>
        /// 向HTTP服务器发送Get请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="cookies">Cookies。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Get(string url, string referer, CookieCollection cookies, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Get(url, referer,
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.89 Safari/537.36",
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                30000, null, cookies, Encoding.UTF8, decompression);
        }

        /// <summary>
        /// 向HTTP服务器发送Get请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="encoding">文本编码。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Get(string url, string referer, Encoding encoding, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Get(url, referer,
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.89 Safari/537.36",
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                30000, null, null, encoding, decompression);
        }

        /// <summary>
        /// 向HTTP服务器发送Get请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Get(string url, string referer, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Get(url, referer,
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.89 Safari/537.36",
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                30000, null, null, Encoding.UTF8, decompression);
        }

        /// <summary>
        /// 向HTTP服务器发送Get请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Get(string url, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Get(url, "",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.89 Safari/537.36",
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                30000, null, null, Encoding.UTF8, decompression);
        }

        /// <summary>
        /// 向HTTP服务器发送Post请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="parameters">请求参数。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="userAgent">User-Agent HTTP标头。</param>
        /// <param name="accept">Accept HTTP标头。</param>
        /// <param name="timeout">超时时间。</param>
        /// <param name="header">HTTP 标头。</param>
        /// <param name="cookies">Cookies。</param>
        /// <param name="encoding">文本编码。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> parameters, string referer, string userAgent, string accept, int timeout, WebHeaderCollection header, CookieCollection cookies, Encoding encoding, DecompressionMethods decompression = DecompressionMethods.None)
        {
            string content = String.Empty;

            try
            {
                HttpWebRequest webRequest = null;

                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    //对服务端证书进行有效性校验（非第三方权威机构颁发的证书，如自己生成的，不进行验证，这里返回true）
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
                    webRequest.ProtocolVersion = HttpVersion.Version10;    //http版本，默认是1.1,这里设置为1.0
                }
                else
                {
                    webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
                }

                //初始WebRequest属性
                webRequest.UserAgent = userAgent;
                webRequest.Referer = referer;
                webRequest.Method = "POST";
                webRequest.Timeout = timeout;
                webRequest.Accept = accept;
                if (cookies != null)
                {
                    webRequest.CookieContainer = new CookieContainer();
                    webRequest.CookieContainer.Add(cookies);
                }
                if (header != null)
                {
                    webRequest.Headers = header;
                }
                webRequest.AutomaticDecompression = decompression;
                webRequest.ContentType = "application/x-www-form-urlencoded";

                if (parameters != null && parameters.Count > 0)
                {
                    StringBuilder postData = new StringBuilder();

                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        postData.AppendFormat("{0}={1}", key, parameters[key]);

                        if(i != parameters.Keys.Count - 1)
                        {
                            postData.Append("&");
                        }

                        i++;
                    }

                    webRequest.ContentLength = postData.ToString().Length;

                    //获取请求数据的流
                    byte[] buffer = Encoding.ASCII.GetBytes(postData.ToString());
                    using (Stream stream = webRequest.GetRequestStream())
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }

                //接收返回字符串
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                content = GetResponseString(webResponse, encoding);
            }
            catch
            {

            }

            return content;
        }

        /// <summary>
        /// 向HTTP服务器发送Post请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="parameters">请求参数。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="header">HTTP 标头。</param>
        /// <param name="cookies">Cookies。</param>
        /// <param name="encoding">文本编码。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> parameters, string referer, WebHeaderCollection header, CookieCollection cookies, Encoding encoding, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Post(url, parameters, referer, "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)",
                "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
                30000, header, cookies, encoding, decompression);
        }

        /// <summary>
        /// 向HTTP服务器发送Post请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="parameters">请求参数。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="cookies">Cookies。</param>
        /// <param name="encoding">文本编码。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> parameters, string referer, CookieCollection cookies, Encoding encoding, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Post(url, parameters, referer, "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)",
                "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
                30000, null, cookies, encoding, decompression);
        }

        /// <summary>
        /// 向HTTP服务器发送Post请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="parameters">请求参数。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="cookies">Cookies。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> parameters, string referer, CookieCollection cookies, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Post(url, parameters, referer, "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)",
                "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
                30000, null, cookies, Encoding.UTF8, decompression);
        }

        /// <summary>
        /// 向HTTP服务器发送Post请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="parameters">请求参数。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="encoding">文本编码。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> parameters, string referer, Encoding encoding, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Post(url, parameters, referer, "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)",
                "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
                30000, null, null, encoding, decompression);
        }

        /// <summary>
        /// 向HTTP服务器发送Post请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="parameters">请求参数。</param>
        /// <param name="referer">参照页。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> parameters, string referer, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Post(url, parameters, referer, "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)",
                "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
                30000, null, null, Encoding.UTF8, decompression);
        }

        /// <summary>
        /// 向HTTP服务器发送Post请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="parameters">请求参数。</param>
        /// <param name="decompression">加密方式。</param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, string> parameters, DecompressionMethods decompression = DecompressionMethods.None)
        {
            return Post(url, parameters, "", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)",
                "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
                30000, null, null, Encoding.UTF8, decompression);
        }

        /// <summary>
        /// 获取图像信息
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <returns>Image对象</returns>
        public static Image GetImage(string url)
        {
            Image img = null;

            try
            {
                //声明WebClient对象
                WebClient client = new WebClient();

                //添加头信息
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                //client.Headers.Add(this.reqUserAgent);

                //获数数据流
                Stream streamData = client.OpenRead(url);

                //从流中创建Image
                img = Image.FromStream(streamData);

                //关闭数据流
                streamData.Close();
                //释放内存
                client = null;
            }
            catch
            {

            }

            //反回数据类型
            return img;
        }

        /// <summary>
        /// Url编码数据。
        /// </summary>
        /// <param name="data">要编码的数据。</param>
        /// <returns>编码后的数据</returns>
        public static string UrlEncode(string data)
        {
            return HttpUtility.UrlEncode(data);
        }

        /// <summary>
        /// Url解码。
        /// </summary>
        /// <param name="data">要解码的数据。</param>
        /// <returns>解码后的数据。</returns>
        public static string UrlDecode(string data)
        {
            return HttpUtility.UrlDecode(data);
        }

        /// <summary>
        /// 获取请求的数据。
        /// </summary>
        /// <param name="webResponse"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static string GetResponseString(HttpWebResponse webResponse, Encoding encoding)
        {
            //Gzip压缩方式的。
            if (webResponse.ContentEncoding.ToLower().Contains("gzip"))
            {
                using (GZipStream stream = new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            //Defalut压缩方式。
            else if (webResponse.ContentEncoding.ToLower().Contains("deflate"))
            {
                using (DeflateStream stream = new DeflateStream(webResponse.GetResponseStream(), CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            //没压缩。
            else
            {
                using (Stream s = webResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(s, encoding);
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 验证证书
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //为保证所有网站都能访问，直接返回True。
            return true;
        }
    }
}
