﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NewsParser.Auth;
using NewsParser.BL.Services.Channels;
using NewsParser.DAL.Models;
using NewsParser.Helpers.ActionFilters.ModelValidation;
using NewsParser.Cache;
using newsparser.FeedParser.Services;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ChannelsController: BaseController
    {
        private readonly IChannelDataService _channelDataService;
        private readonly IAuthService _authService;
        private readonly IFeedUpdater _feedUpdater;

        public ChannelsController(
            IChannelDataService channelDataService,
            IAuthService authService,
            IFeedUpdater feedUpdater)
        {
            _channelDataService = channelDataService;
            _authService = authService;
            _feedUpdater = feedUpdater;
        }

        [HttpGet]
        [ResponseCache(Duration = 3600)]
        [Cache(Duration = 3600, DeferByUser = true)]
        public JsonResult Get(
            bool subscribed = false, 
            string search = null, int 
            pageIndex = 0, 
            int pageSize = 5)
        {
            var user = _authService.GetCurrentUser();
            int total;
            var channels = _channelDataService
                .GetPage(user.GetId(), out total, pageIndex, pageSize, search, subscribed)
                .ToList();
            var channelModels = Mapper.Map<List<ChannelSubscriptionModel>>(channels);
            return new JsonResult(new { data = channelModels, total });
        }

        [HttpGet("{id:int}")]
        [ResponseCache(Duration = 3600)]
        [Cache(Duration = 3600, DeferByUser = true)]
        public JsonResult Get(int id)
        {
            var channel = _channelDataService.GetById(id);
            var user = _authService.GetCurrentUser();
            if(!_channelDataService.IsVisibleToUser(channel.Id, user.GetId()))
            {
                return MakeErrorResponse(HttpStatusCode.NotFound, "Channel was not found");
            }
            return new JsonResult(Mapper.Map<ChannelSubscriptionModel>(channel));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<JsonResult> Post([FromBody]ChannelCreateModel channelModel)
        {
            var channel = _channelDataService.GetByUrl(channelModel.FeedUrl);
            var user = _authService.GetCurrentUser();
            Channel createdChannel;

            if(channel != null)
            {
                if(_channelDataService.IsUserSubscribed(channel.Id, user.GetId()))
                {
                    return MakeErrorResponse(HttpStatusCode.BadRequest, "You are already subscribed to this channel.");
                }

                createdChannel = channel;
                _channelDataService.SubscribeUser(channel.Id, user.GetId(), channelModel.IsPrivate);
            }
            else
            {
                createdChannel = await _feedUpdater.AddFeedChannel(channelModel.FeedUrl, channelModel.IsPrivate, user.GetId());
            }

            var createdChannedModel = Mapper.Map<Channel, ChannelSubscriptionModel>(createdChannel);
            return MakeSuccessResponse(HttpStatusCode.Created, 
                new { data = createdChannedModel, 
                    message = "Feed channel was added to the list of your subscriptions" });
        }
    }
}
