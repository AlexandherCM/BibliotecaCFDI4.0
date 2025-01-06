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
        private readonly bool withDomUrl;
        private readonly string _url;

        private void CreateHeader(HttpClient httpClient, string jwt)
            => httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {jwt}");

        public HttpService(string url = "")
        {
            withDomUrl = !string.IsNullOrEmpty(url);
            if (withDomUrl) _url = url;

            //true : Se conatena el prefijo a los end-points
            //false: Se define el end-point completo en las funciones http
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public async Task<string> GetAsync(string endPoint, string jwt = "")
        {
            using (HttpClient httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(jwt)) CreateHeader(httpClient, jwt);


                endPoint = withDomUrl ? $"{_url}/{endPoint}" : endPoint;
                HttpResponseMessage response = await httpClient.GetAsync(endPoint);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                return null;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public async Task<string> PostAsync(string endPoint, string content, string jwt = "")
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(content, Encoding.UTF8, contentType);

                if (!string.IsNullOrEmpty(jwt)) CreateHeader(httpClient, jwt);

                endPoint = withDomUrl ? $"{_url}/{endPoint}" : endPoint;
                HttpResponseMessage response = await httpClient.PostAsync(endPoint, httpContent);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                return null;
            }
        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public async Task<string> PutAsync(string endPoint, string content,  string jwt = "")
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(content, Encoding.UTF8, contentType);

                if (!string.IsNullOrEmpty(jwt)) CreateHeader(httpClient, jwt);

                endPoint = withDomUrl ? $"{_url}/{endPoint}" : endPoint;
                HttpResponseMessage response = await httpClient.PutAsync(endPoint, httpContent);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                return null;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        public async Task<string> DeleteAsync(string endPoint,  string jwt = "")
        {
            using (HttpClient httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(jwt)) CreateHeader(httpClient, jwt);


                endPoint = withDomUrl ? $"{_url}/{endPoint}" : endPoint;
                HttpResponseMessage response = await httpClient.DeleteAsync(endPoint);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                return null;
            }
        }


    }
}
