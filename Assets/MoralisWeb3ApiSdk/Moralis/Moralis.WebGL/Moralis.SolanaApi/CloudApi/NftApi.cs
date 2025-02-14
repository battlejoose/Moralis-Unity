﻿using Moralis.WebGL.SolanaApi.Client;
using Moralis.WebGL.SolanaApi.Interfaces;
using Moralis.WebGL.SolanaApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Net;

namespace Moralis.WebGL.SolanaApi.CloudApi
{
	public class NftApi : INftApi
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NativeApi"/> class.
		/// </summary>
		/// <param name="apiClient"> an instance of ApiClient (optional)</param>
		/// <returns></returns>
		public NftApi(ApiClient apiClient = null)
		{
			if (apiClient == null) // use the default one in Configuration
				this.ApiClient = Configuration.DefaultApiClient;
			else
				this.ApiClient = apiClient;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NativeApi"/> class.
		/// </summary>
		/// <returns></returns>
		public NftApi(String basePath)
		{
			this.ApiClient = new ApiClient(basePath);
		}

		/// <summary>
		/// Sets the base path of the API client.
		/// </summary>
		/// <param name="basePath">The base path</param>
		/// <value>The base path</value>
		public void SetBasePath(String basePath)
		{
			this.ApiClient.BasePath = basePath;
		}

		/// <summary>
		/// Gets the base path of the API client.
		/// </summary>
		/// <param name="basePath">The base path</param>
		/// <value>The base path</value>
		public String GetBasePath(String basePath)
		{
			return this.ApiClient.BasePath;
		}

		/// <summary>
		/// Gets or sets the API client.
		/// </summary>
		/// <value>An instance of the ApiClient</value>
		public ApiClient ApiClient { get; set; }


		public async UniTask<NftMetadata> GetNFTMetadata(NetworkTypes network, string address)
		{
			// Verify the required parameter 'pairAddress' is set
			if (address == null) throw new ApiException(400, "Missing required parameter 'address' when calling GetNFTMetadata");

			var postBody = new Dictionary<String, String>();
			var headerParams = new Dictionary<String, String>();

			var path = "/functions/sol-getNFTMetadata";
			postBody.Add("network", ApiClient.ParameterToString(network.ToString()));
			if (address != null) postBody.Add("address", ApiClient.ParameterToString(address));
			
			// Authentication setting, if any
			String[] authSettings = new String[] { "ApiKeyAuth" };

			string bodyData = postBody.Count > 0 ? JsonConvert.SerializeObject(postBody) : null;

			Tuple<HttpStatusCode, Dictionary<string, string>, string> response = await ApiClient.CallApi(path, Method.POST, null, bodyData, headerParams, null, null, authSettings);

			if (((int)response.Item1) >= 400)
				throw new ApiException((int)response.Item1, "Error calling GetNFTMetadata: " + response.Item3, response.Item3);
			else if (((int)response.Item1) == 0)
				throw new ApiException((int)response.Item1, "Error calling GetNFTMetadata: " + response.Item3, response.Item3);

			return ((CloudFunctionResult<NftMetadata>)ApiClient.Deserialize(response.Item3, typeof(CloudFunctionResult<NftMetadata>), response.Item2)).Result;
		}
	}
}
