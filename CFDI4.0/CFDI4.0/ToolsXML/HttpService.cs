using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CFDI4._0.ToolsXML
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class HttpService
    {
        private readonly string contentType = "application/json";
        private readonly bool withDomUrl;
        private readonly string _url;
        private readonly HttpClient _httpClient;

        private void CreateHeader(HttpClient httpClient, string jwt)
            => httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {jwt}");

        public HttpService(string url = "", TimeSpan? timeout = null)
        {
            withDomUrl = !string.IsNullOrEmpty(url);
            if (withDomUrl) _url = url;

            _httpClient = new HttpClient
            {
                Timeout = timeout ?? TimeSpan.FromMinutes(10) // Si no se especifica, usa 30 segundos por defecto
            };
        }

        public async Task<string> GetAsync(string endPoint, string jwt = "")
        {
            if (!string.IsNullOrEmpty(jwt)) CreateHeader(_httpClient, jwt);

            endPoint = withDomUrl ? $"{_url}/{endPoint}" : endPoint;
            HttpResponseMessage response = await _httpClient.GetAsync(endPoint);

            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }

        public async Task<string> PostAsync(string endPoint, string content, string jwt = "")
        {
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, contentType);
            if (!string.IsNullOrEmpty(jwt)) CreateHeader(_httpClient, jwt);

            endPoint = withDomUrl ? $"{_url}/{endPoint}" : endPoint;
            HttpResponseMessage response = await _httpClient.PostAsync(endPoint, httpContent);

            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }

        public async Task<string> PutAsync(string endPoint, string content, string jwt = "")
        {
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, contentType);
            if (!string.IsNullOrEmpty(jwt)) CreateHeader(_httpClient, jwt);

            endPoint = withDomUrl ? $"{_url}/{endPoint}" : endPoint;
            HttpResponseMessage response = await _httpClient.PutAsync(endPoint, httpContent);

            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }

        public async Task<string> DeleteAsync(string endPoint, string jwt = "")
        {
            if (!string.IsNullOrEmpty(jwt)) CreateHeader(_httpClient, jwt);

            endPoint = withDomUrl ? $"{_url}/{endPoint}" : endPoint;
            HttpResponseMessage response = await _httpClient.DeleteAsync(endPoint);

            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }
    }

}
