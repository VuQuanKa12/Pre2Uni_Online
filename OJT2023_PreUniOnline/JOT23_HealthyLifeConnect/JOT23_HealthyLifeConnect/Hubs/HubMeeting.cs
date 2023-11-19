﻿using JOT23_Pre2UniOnline.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace JOT23_Pre2UniOnline.Hubs
{
	public class HubMetting : Hub
	{
		private static readonly List<string> MutedConnections = new List<string>();
		public async Task JoinRoom(string roomid, string userid)
		{
			if (!ListUserMeetingRoom.UserMeetingRooms.ContainsKey(Context.ConnectionId))
			{
				ListUserMeetingRoom.UserMeetingRooms.Add(Context.ConnectionId, new UserMeetingRoom() { RoomId = roomid, UserId = userid });
			}

			await Groups.AddToGroupAsync(Context.ConnectionId, roomid);
			await Clients.Group(roomid).SendAsync("user-connected", userid);

		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			if (ListUserMeetingRoom.UserMeetingRooms.ContainsKey(Context.ConnectionId))
			{
				var user = ListUserMeetingRoom.UserMeetingRooms[Context.ConnectionId];
				ListUserMeetingRoom.UserMeetingRooms.Remove(Context.ConnectionId);
				await Clients.OthersInGroup(user.RoomId).SendAsync("user-disconnected", user.UserId);
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.RoomId);
			}

			await base.OnDisconnectedAsync(exception);
		}

		public async Task MuteMicrophone(string userId, string RoomID)
		{
			var connectionId = Context.ConnectionId;
			if (!MutedConnections.Contains(connectionId))
			{
				MutedConnections.Add(connectionId);
			}

			await Clients.Group(RoomID).SendAsync("MuteMicrophone", userId);
		}

		public async Task UnmuteMicrophone(string userId, string RoomID)
		{
			var connectionId = Context.ConnectionId;
			if (MutedConnections.Contains(connectionId))
			{
				MutedConnections.Remove(connectionId);
			}

			await Clients.Group(RoomID).SendAsync("UnmuteMicrophone", userId);
		}

		public async Task MuteCamera(string userId, string RoomID)
		{
			await Clients.Group(RoomID).SendAsync("MuteCamera", userId);
		}

		public async Task UnmuteCamera(string userId, string RoomID)
		{
			await Clients.Group(RoomID).SendAsync("UnmuteCamera", userId);
		}

		public async Task StartScreenShare(string roomId, string userId, string jsonData)
		{
			await Clients.Group(roomId).SendAsync("start_screen_share", userId, jsonData);
		}

		public async Task StopScreenShare(string roomId, string userId)
		{
			await Clients.Group(roomId).SendAsync("stop_screen_share", userId);
		}
	}
}

