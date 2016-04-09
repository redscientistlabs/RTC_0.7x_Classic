using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using BizHawk.Client.EmuHawk;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;

namespace RTC
{
    static class RTC_CorruptCloud
    {
        static string CorruptCloudServer = "http://cc.zproduction.net/CC";

        public static BlastLayer CloudLoad(string CorruptCloudCode)
        {
            GlobalWin.Sound.StopSound();
            //string obj = SerializeObject(bl);
            //string Code = "";


            string remoteUri = CorruptCloudServer + "/FILES/";
            string ccc = CorruptCloudCode;

            WebClient myWebClient = new WebClient();

            try
            {
                GlobalWin.Sound.StopSound();
                myWebClient.DownloadFile(remoteUri + ccc, RTC_Core.rtcDir + "\\CORRUPTCLOUD\\" + ccc);
            }
            catch(Exception ex)
            {
                GlobalWin.Sound.StopSound();
                MessageBox.Show("Error: Couldn't get requested BlastLayer\n\n\n" + ex.ToString());
                GlobalWin.Sound.StartSound();
                return null;
            }

            BlastLayer bl;

            FileStream FS;
            BinaryFormatter bformatter = new BinaryFormatter();

            try
            {
                GlobalWin.Sound.StopSound();
                FS = File.Open(RTC_Core.rtcDir + "\\CORRUPTCLOUD\\" + ccc, FileMode.OpenOrCreate);
                bl = (BlastLayer)bformatter.Deserialize(FS);
                FS.Close();
            }
            catch(Exception ex)
            {
                GlobalWin.Sound.StopSound();
                MessageBox.Show("The BlastLayer file could not be loaded\n\n\n" + ex.ToString());
                GlobalWin.Sound.StartSound();
                return null;
            }

            GlobalWin.Sound.StartSound();

            return bl;
        }


        public static string CloudSave(BlastLayer bl)
        {
            FileStream FS;
            BinaryFormatter bformatter = new BinaryFormatter();

            string tempfile = RTC_Core.rtcDir + "\\CORRUPTCLOUD\\temp.bl";

            GlobalWin.Sound.StopSound();

            if (File.Exists(tempfile))
                File.Delete(tempfile);

            FS = File.Open(tempfile, FileMode.OpenOrCreate);
            bformatter.Serialize(FS, bl);
            FS.Close();
            

            string remoteUri = CorruptCloudServer + "/post.php?submit=true&action=upload";
            byte[] responseBinary;
            try{
            WebClient client = new WebClient();
            responseBinary = client.UploadFile(remoteUri, "POST", tempfile);
            }
            catch(Exception ex)
            {
                GlobalWin.Sound.StopSound();
                MessageBox.Show("Something went wrong with the upload. Try again. \n\n\n" + ex.ToString());
                GlobalWin.Sound.StartSound();
                return "";
            }

            string response = Encoding.UTF8.GetString(responseBinary);

            GlobalWin.Sound.StartSound();

            if (response == "ERROR")
                return "";
            else
                return response;

        }


        

        // String serializers
        public static string SerializeObject(object o)
        {
            if (!o.GetType().IsSerializable)
            {
                return "";
            }

            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, o);
                return Convert.ToBase64String(stream.ToArray());
            }
        }


        public static object DeserializeObject(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }
    }
}
