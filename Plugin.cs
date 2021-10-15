using System;
using System.Collections.Generic;
using System.Linq;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using System.Net.Http;

namespace VPNPlugin
{

    public class VPNPlugin : TerrariaPlugin
    {

        public VPNPlugin(Main game) : base(game)
        {

        }

        private static readonly HttpClient client = new HttpClient();

        public override void Initialize()
        {
            ServerApi.Hooks.ServerJoin.Register(this, OnJoinAsync);
        }

        async void OnJoinAsync(JoinEventArgs args)
        {
            var response = await client.PostAsync("http://check.getipintel.net/check.php?ip=" + TShock.Players[args.Who].IP, null);

            var responseString = await response.Content.ReadAsStringAsync();

            int responseInt;

            int.TryParse(responseString, out responseInt);

            if (responseInt == 1)



            {
                TShock.Players[args.Who].Disconnect("AntiVPN: VPN connections are not permitted.");
            }




        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoinAsync);
            }
            base.Dispose(disposing);
        }
    }
}