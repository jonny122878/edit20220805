using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace edit20210325.Function
{
    public class DealYoutubeUrl : IDisposable
    {
        private bool disposedValue;

        public string getyoutubeId(string youtubeUrl)
        {
            string Result = youtubeUrl;
            Result = Result.Replace("&feature=related", "");
            Result = Result.Replace("&feature=share", "");
            Regex regex = new Regex(@"^(?:https?:\/\/)?(?:(www|m)\.)?youtu(?:\.be\/|be.com\/\S*(?:watch|embed)(?:(?:(?=\/[^&\s\?]+(?!\S))\/)|(?:\S*v=|v\/)))([^&\s\?]+)");
            if(!regex.IsMatch(Result)) return "NG";
            Result = Result.Replace("https://www.youtube.com/watch?v=", "");
            Result = Result.Replace("http://www.youtube.com/watch?v=", "");
            Result = Result.Replace("https://m.youtube.com/watch?v=", "");
            Result = Result.Replace("http://m.youtube.com/watch?v=", "");
            Result = Result.Replace("https://youtu.be/", "");
            Result = Result.Replace("http://youtu.be/", "");         
            Result = Result.Replace("https://www.youtube.com/embed/watch?feature=player_embedded&v=", "");
            Result = Result.Replace("http://www.youtube.com/embed/watch?feature=player_embedded&v=", "");
            if (Result.Contains("&"))
            {
                return Result.Remove(Result.IndexOf("&"));
            }
            if(Result.Length == 11)
            {
                return Result;
            }
            return "NG";
        }
        public string dealYoutubeUrl(string youtubeUrl)
        {
            var url = youtubeUrl;
            if (url.Contains("&"))
            {
                return url.Remove(url.IndexOf("&"));
            }
            return youtubeUrl;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置受控狀態 (受控物件)
                }

                // TODO: 釋出非受控資源 (非受控物件) 並覆寫完成項
                // TODO: 將大型欄位設為 Null
                disposedValue = true;
            }
        }

        // // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        // ~DealYoutubeUrl()
        // {
        //     // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
