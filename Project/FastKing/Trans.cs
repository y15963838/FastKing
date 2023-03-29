using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using LitJson;
using System.Security.Cryptography;
using System.Web;

namespace FastKing
{
    public static class Trans
    {
        public static string GetResult(string word, string target)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URLShit(word,target));
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.UserAgent = null;
                request.Timeout = 6000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                retString = HttpUtility.UrlDecode(retString);

                //System.Windows.Forms.MessageBox.Show(retString);

                JsonData data = JsonMapper.ToObject(retString);
                var c = data["trans_result"][0]["dst"];

                return (string)c;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("翻译错误：" + e.Message);
                return $"error:{e.Message}";
            }
        }


        private static string URLShit(string q,string target)
        {
            //string baseUrl = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            //string appid = "20201123000623849";
            //string salt = "1435660288";  //随机码
            //string password = "oeobjacqejIrSI0YhZOp";
            //var sign = MD5Encrypt32X(appid + q + salt + password);  //32位小写MD5

            // //在发送HTTP请求之前需要对各字段做URL encode
            // //在生成签名拼接 appid+q+salt+密钥 字符串之前，q不需要做URL encode。之后需要
            // q = HttpUtility.UrlEncode(q, Encoding.UTF8);

            //baseUrl += $"q={q}&from=auto&to={target}&appid={appid}&salt={salt}&sign={sign}";
            //System.Windows.Forms.MessageBox.Show(baseUrl);
            //return baseUrl;

            // 原文
            
            // 源语言
            string from = "auto";
            // 目标语言
            string to = target;
            // 改成您的APP ID
            string appId = "20201123000623849";
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            string secretKey = "oeobjacqejIrSI0YhZOp";
            string sign = EncryptString(appId + q + salt + secretKey);
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + from;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            return url;
        }


        // 计算MD5值
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }



        public static string GetHttpResponse(string url, int Timeout = 6000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = Timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }
}
