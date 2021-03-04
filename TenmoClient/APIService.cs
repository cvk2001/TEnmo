﻿using RestSharp;
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
        private readonly RestClient client = new RestClient();

        public API_Transfer TransferSend(API_Transfer api_transfer)
        {
            API_Account apiAccount = GetAccount(api_transfer.Account_From);
            if (apiAccount.Balance >= api_transfer.Amount)
            {
                RestRequest request = new RestRequest(API_URL + "transfer");
                request.AddJsonBody(api_transfer);
               
                IRestResponse<API_Transfer> response = client.Post<API_Transfer>(request);

                if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
                {
                    ProcessResponse(response);
                }
                else
                {
                    Console.WriteLine("Transfer Successful");
                    return response.Data;
                }
                return null;
            }else
            {
                Console.WriteLine("You can't send more money than you currently have.");
                return null;
            }
        }
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
