using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient
{
    public class APIService
    {
        public readonly string API_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public API_Account GetAccount(int id)
        {
            RestRequest request = new RestRequest(API_URL + "accounts/" + id);
            IRestResponse<API_Account> response = client.Get<API_Account>(request);
            if (ProcessResponse(response))
            {
                return response.Data;
            }
            return null;
                
            
        }
        public List<API_User> GetUsers()
        {
            RestRequest request = new RestRequest(API_URL + "user");
            IRestResponse<List<API_User>> response = client.Get<List<API_User>>(request);
            if (ProcessResponse(response))
            {
                return response.Data;
            }
            return null;

        }

        private bool ProcessResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new HttpRequestException("Error Occurred - Unable to reach server");
            }
            else if (!response.IsSuccessful)
            {
                throw new HttpRequestException("Error occurred - received non-success response; " + (int)response.StatusCode);
            }
            return true;
        }
    }
}
