﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProxyKit;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private IServerPool serverPool;

        public ServersController(IServerPool serverPool)
        {
            this.serverPool = serverPool;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            List<string> servers = new List<string>();
            foreach (var server in serverPool)
            {
                servers.Add(server.Host.ToString());
            }

            return servers;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string port)
        {
            string address=HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (address == "0.0.0.1")
            {
                address = "127.0.0.1";
            }
            address = "http://"+address +":"+ port;
            serverPool.Add(new UpstreamHost(address));
            Console.WriteLine(address," is added");
        }
    }
}
