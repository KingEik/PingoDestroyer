using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PingoDestroyer
{
    class Helper
    {
        public CookieContainer cookies = new CookieContainer();
        public IPAddress spoofIPAddress = null;
        public String userAgent = null;

        public bool acceptCookies = true;

        private void applyExtras(HttpWebRequest req)
        {
            if (spoofIPAddress != null && spoofIPAddress != IPAddress.None)
            {
                String ip = spoofIPAddress.ToString();
                req.Headers.Add("Client-IP", ip);
                req.Headers.Add("X-Forwarded-For", ip);
                req.Headers.Add("Forwarded", "for=" + ip);
            }
            if (userAgent != null)
            {
                req.UserAgent = userAgent;
            }
        }

        public HttpWebResponse post(Uri uri, String data)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(uri);
            wr.Method = "POST";
            wr.AllowAutoRedirect = false;
            wr.Timeout = 10000;
            applyExtras(wr);
            wr.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            wr.ContentLength = bytes.Length;
            if (acceptCookies)
                wr.CookieContainer = cookies;
            Stream request = wr.GetRequestStream();
            request.Write(bytes, 0, bytes.Length);
            request.Close();

            try
            {
                return (HttpWebResponse)wr.GetResponse();
            }
            catch (WebException e)
            {
                Program.LogException(e);
                return (HttpWebResponse)e.Response;
            }
        }

        public HttpWebResponse get(Uri uri)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(uri);
            wr.Method = "GET";
            wr.AllowAutoRedirect = false;
            wr.Timeout = 10000;
            applyExtras(wr);
            if (acceptCookies)
                wr.CookieContainer = cookies;

            try
            {
                return (HttpWebResponse)wr.GetResponse();
            }
            catch (WebException e)
            {
                Program.LogException(e);
                return (HttpWebResponse)e.Response;
            }
        }
    }
}
