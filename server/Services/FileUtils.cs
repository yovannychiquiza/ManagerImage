using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ManagerImage.Services
{
    public static class FileUtil
    {
        public static object SaveAs(this IJSRuntime js, string filename, byte[] data)
        {
            object da = js.InvokeAsync<object>(
                           "saveAsFile",
                           filename,
                           Convert.ToBase64String(data));
            return da;
        }

        public static async Task SaveAs1(this IJSRuntime js, string filename, byte[] data)
        {
            await js.InvokeAsync<object>(
                           "saveAsFile",
                           filename,
                           Convert.ToBase64String(data));
           // await js.InvokeAsync<string>("downloadFile", path);
         
        }
        public static object SaveAsNew(this IJSRuntime js, string fileName, byte[] data)
        {
            return js.InvokeAsync<object>(
                "saveAsFileNew",
                fileName,
                Convert.ToBase64String(data));
        }
        public static async Task DownloadFileAsync(this IJSRuntime js, string path)
        {
            await js.InvokeAsync<string>("downloadFile", path);
        }
    }

    public static class Util
    {
        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }
    }


}
