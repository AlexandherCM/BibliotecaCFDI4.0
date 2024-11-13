using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CFDI4._0.ToolsXML
{
    public class HttpService
    {
        private readonly string contentType = "application/json";
        private readonly string _url = "";

        private void CreateHeader(HttpClient httpClient, string jwt)
            => httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {jwt}");

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public async Task<string> GetAsync(string endPoint, bool withDomUrl = false, string jwt = "")
        {
            using (HttpClient httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(jwt))
                    CreateHeader(httpClient, jwt);

                HttpResponseMessage response;
                if (withDomUrl)
                    response = await httpClient.GetAsync($"{_url}/{endPoint}");
                else
                    response = await httpClient.GetAsync(endPoint);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                return null;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public async Task<string> PostAsync(string endPoint, string content, bool withDomUrl = false, string jwt = "")
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(content, Encoding.UTF8, contentType);

                if (!string.IsNullOrEmpty(jwt))
                    CreateHeader(httpClient, jwt);

                HttpResponseMessage response;
                if (withDomUrl)
                    response = await httpClient.PostAsync($"{_url}/{endPoint}", httpContent);
                else
                    response = await httpClient.PostAsync(endPoint, httpContent);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                return null;
            }
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public async Task<string> PutAsync(string endPoint, string content, bool withDomUrl = false, string jwt = "")
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(content, Encoding.UTF8, contentType);

                if (!string.IsNullOrEmpty(jwt))
                    CreateHeader(httpClient, jwt);

                HttpResponseMessage response;
                if (withDomUrl)
                    response = await httpClient.PutAsync($"{_url}/{endPoint}", httpContent);
                else
                    response = await httpClient.PutAsync(endPoint, httpContent);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                return null;
            }

        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public async Task<string> DeleteAsync(string endPoint, bool withDomUrl = false, string jwt = "")
        {
            using (HttpClient httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(jwt))
                    CreateHeader(httpClient, jwt);

                HttpResponseMessage response;
                if (withDomUrl)
                    response = await httpClient.DeleteAsync($"{_url}/{endPoint}");
                else
                    response = await httpClient.DeleteAsync(endPoint);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                return null;
            }
        }

    }
}
