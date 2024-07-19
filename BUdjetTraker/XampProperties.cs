using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BUdjetTraker {
    internal class XampProperties {

        string phpCommand;
        string ipAddress = "http://192.168.1.179:8080/";
        public XampProperties(string phpCommand) {
            this.phpCommand = phpCommand;
        }

        public string CreateResponse() {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ipAddress + phpCommand);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();
        }
    }
}