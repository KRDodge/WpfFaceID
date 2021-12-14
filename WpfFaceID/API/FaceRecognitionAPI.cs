using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace WpfFaceID.API
{
    public class FaceRecognitionAPI
    {
        public List<List<int>> GetFaceRectangleList(string _filePath)
        {
            List<List<int>> faceRectangle = new List<List<int>>();
            JToken json = GetFaceJson(_filePath);
            if (json == null)
                return faceRectangle;

            JToken BodyJToken = json.SelectToken("faces");
            if (BodyJToken == null)
                return faceRectangle;


            int i = 0;
            foreach (JToken bodyMembers in BodyJToken)
            {
                faceRectangle.Add(new List<int>());
                JToken roiToken = bodyMembers.SelectToken("roi");

                faceRectangle[i].Add((int)roiToken["x"]);
                faceRectangle[i].Add((int)roiToken["y"]);
                faceRectangle[i].Add((int)roiToken["width"]);
                faceRectangle[i].Add((int)roiToken["height"]);
                faceRectangle[i].Add((int)roiToken["height"]);

                JToken genderToken = bodyMembers.SelectToken("gender");
                string gender = genderToken["value"].ToString();
                string gconfidence = genderToken["confidence"].ToString();

                JToken ageToken = bodyMembers.SelectToken("age");
                string aeg = ageToken["value"].ToString();
                string aconfidence = ageToken["confidence"].ToString();
                //string landmark = members["roi"].ToString();
                //Console.WriteLine(members["roi"]);
                //Console.WriteLine(members["landmark"]);
                ++i;
            }

            return faceRectangle;
        }

        public JObject GetFaceJson(string _filePath)
        {
            JObject json = new JObject();

            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            string FilePath = _filePath;
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            byte[] fileData = new byte[fs.Length];
            fs.Read(fileData, 0, fileData.Length);
            fs.Close();

            string CRLF = "\r\n";
            string postData = "--" + boundary + CRLF + "Content-Disposition: form-data; name=\"image\"; filename=\"";
            postData += Path.GetFileName(FilePath) + "\"" + CRLF + "Content-Type: image/jpeg" + CRLF + CRLF;
            string footer = CRLF + "--" + boundary + "--" + CRLF;

            Stream DataStream = new MemoryStream();
            DataStream.Write(Encoding.UTF8.GetBytes(postData), 0, Encoding.UTF8.GetByteCount(postData));
            DataStream.Write(fileData, 0, fileData.Length);
            DataStream.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 2);
            DataStream.Write(Encoding.UTF8.GetBytes(footer), 0, Encoding.UTF8.GetByteCount(footer));
            DataStream.Position = 0;
            byte[] formData = new byte[DataStream.Length];
            DataStream.Read(formData, 0, formData.Length);
            DataStream.Close();

            //string url = "https://openapi.naver.com/v1/vision/celebrity"; // 유명인 얼굴 인식
            string url = "https://openapi.naver.com/v1/vision/face"; // 얼굴 감지
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", "pgfawO38TvY3Pqfm8HAO");
            request.Headers.Add("X-Naver-Client-Secret", "IHaTD_mTtf");
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.ContentLength = formData.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string result;
                result = reader.ReadToEnd();
                stream.Close();
                response.Close();
                reader.Close();

                json = JObject.Parse(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return json;
        }

        
    }
}