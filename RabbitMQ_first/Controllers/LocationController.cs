using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ_first.Models;
using System.Text;

namespace RabbitMQ_first.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        [HttpPost]
        public void  Post ([FromBody]Location location)
        {
            var factory = new ConnectionFactory() { HostName = "localhost:44368" };
            using (var connection =factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "locationSampleQueue",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null
                                            );

                string message = "Latitude: " + location.Latitude + ", Longitude: " + location.Longitude + " and Time: " + location.Date;
                var body = Encoding.UTF8.GetBytes(message);


                channel.BasicPublish(exchange: "",
                                     routingKey: "locationSampleQueue",
                                     basicProperties: null,
                                     body: body);

            }

        }


    }
}
