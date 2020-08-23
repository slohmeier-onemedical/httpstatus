﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Teapot.Web;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers
{
    //[EnableCors]
    public class StatusController : Controller
    {
        private static readonly TeapotStatusCodeResults StatusCodes = new TeapotStatusCodeResults();

        [HttpGet("")]
        public IActionResult Index() => View(StatusCodes);

        [Route("{statusCode}", Name = "StatusCode")]
        public async Task<IActionResult> StatusCode(int statusCode, int? sleep)
        {
            var statusData = StatusCodes.ContainsKey(statusCode)
                ? StatusCodes[statusCode]
                : new TeapotStatusCodeResult { Description = $"{statusCode} Unknown Code" };

            await DoSleep(sleep);

            return new CustomHttpStatusCodeResult(statusCode, statusData);
        }

        [EnableCors(Constants.CorsPolicy)]
        [Route("{statusCode}/cors", Name = "Cors")]
        public async Task<IActionResult> Cors(int statusCode, int? sleep)
        {
            var statusData = StatusCodes.ContainsKey(statusCode)
                ? StatusCodes[statusCode]
                : new TeapotStatusCodeResult { Description = $"{statusCode} Unknown Code" };

            await DoSleep(sleep);

            return new CustomHttpStatusCodeResult(statusCode, statusData);
        }

        private static async Task DoSleep(int? sleep)
        {
            const int SLEEP_MIN = 0;
            const int SLEEP_MAX = 5 * 60 * 1000; // 5 mins in milliseconds

            var sleepData = Math.Clamp(sleep ?? 0, SLEEP_MIN, SLEEP_MAX);
            if (sleepData > 0)
            {
                await Task.Delay(sleepData);
            }
        }
    }
}
