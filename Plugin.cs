using System;
using System.Collections.Generic;
using System.Linq;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using System.Net.Http;

namespace VPNBlock
{

    public class VPNPlugin : TerrariaPlugin
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly Config _config;

        public VPNPlugin(Main game) : base(game)
        {
            _config = Config.Read();
        }

        public override void Initialize()
        {
            ServerApi.Hooks.ServerJoin.Register(this, OnJoinAsync);
            Commands.ChatCommands.Add(new Command("vpndetector.useadvanced", CMD_ToggleDetector, "advancedvpndetector"));
        }

        async void OnJoinAsync(JoinEventArgs args)
        {
            if (!_config.Enabled || TShock.Players[args.Who] is null)
                return;

            var response = await _client.PostAsync("http://check.getipintel.net/check.php?" + $"ip={TShock.Players[args.Who].IP}&contact={_config.ContactEmail}" , null);
            var responseString = await response.Content.ReadAsStringAsync();

            if (double.TryParse(responseString, out var result) && result > .99)
            {
                TShock.Players[args.Who].Disconnect("Proxies/VPNs are not allowed on this server.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoinAsync);
                Commands.ChatCommands.RemoveAll(x => x.HasAlias("advancedvpndetector"));
            }

            base.Dispose(disposing);
        }

        private void CMD_ToggleDetector(CommandArgs args)
        {
            _config.Enabled = !_config.Enabled;
            args.Player.SendSuccessMessage("Detection is now {0}.", _config.Enabled ? "enabled" : "disabled");
        }
    }
}