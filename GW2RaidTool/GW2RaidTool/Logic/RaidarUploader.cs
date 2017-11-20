﻿using System.Net;
using System.Security.Cryptography;
using System.Text;
using RaidTool.Helper;
using RaidTool.Logic.Interfaces;
using RaidTool.Messages;
using RaidTool.Models;
using RaidTool.Properties;
using ReactiveUI;
using RestSharp;
using System.Collections.ObjectModel;

namespace RaidTool.Logic
{
    public class RaidarUploader : IEncounterUploader
    {
	    private readonly IMessageBus _messageBus;

	    public RaidarUploader(IMessageBus messageBus)
	    {
		    _messageBus = messageBus;
	    }

	    public void Upload(IEncounterLog log)
	    {
		    var restClient = new RestClient("https://www.gw2raidar.com");
		    var restRequest = new RestRequest("/api/upload.json");
			restRequest.Method = Method.POST;
		    restRequest.AddFile("file", log.EvtcPath);
		    restRequest.AddParameter("username", Settings.Default.RaidarUser);
			restRequest.AddParameter("password", Settings.Default.RaidarPassword.Unprotect());

		    var restResponse = restClient.Execute(restRequest);

		    if (restResponse.ResponseStatus == ResponseStatus.Completed && restResponse.StatusCode == HttpStatusCode.OK)
		    {
			    log.RaidarUploadComplete = true;
			    _messageBus.SendMessage(new UploadedReportMessage(log));
			}
		    else
		    {
			    log.RaidarUploadComplete = false;
				_messageBus.SendMessage(new LogMessage($"{restResponse.StatusCode} - {restResponse.Content}"));
			}

			
	    }
    }
}
