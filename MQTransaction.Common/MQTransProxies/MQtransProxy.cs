using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MQTransaction.Common.MQTransProxies
{
    /// <summary>
    /// http请求类
    /// </summary>
    internal class MQtransProxy
    {
        private readonly IHttpClientFactory httpClientFactory;
        public MQtransProxy(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<string> Post(string url, object body)
        {
            try
            {
                HttpContent postContent = new StringContent(JsonConvert.SerializeObject(body));
                postContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await httpClientFactory.CreateClient().PostAsync(url, postContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    return responseStr;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<string> Get(string url)
        {
            try
            {
                var response = await httpClientFactory.CreateClient().GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    return responseStr;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


    }
}
