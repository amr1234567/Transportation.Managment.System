using Core.Constants;
using Core.Dto;
using Core.Dto.Identity;
using Core.Dto.UserInput;
using Core.Dto.UserOutput;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    [ApiController]
    public class AdminController(IAdminServices adminServices, IManagerServices managerServices, IJourneysHistoryServices journeysHistoryServices, ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IAdminServices _adminServices = adminServices;
        private readonly IManagerServices _managerServices = managerServices;
        private readonly IJourneysHistoryServices _journeysHistoryServices = journeysHistoryServices;

        //[NonAction]
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<ActionResult<ResponseModel<string>>> SignUp(SignUpAsAdminDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var response = await _adminServices.SignUp(model);
                if (response)
                {
                    Log.Information($"Sign up Succeeded");
                    return Ok(new ResponseModel<string>
                    {
                        StatusCode = 200,
                        Message = "Every thing is good"
                    });
                }
                Log.Error($"Sign up Failed");
                return BadRequest(new ResponseModel<string>
                {
                    StatusCode = 400,
                    Message = "Wrong Email Or Password"
                });

            }
            catch (Exception ex)
            {
                Log.Error($"Sign up Failed");
                return BadRequest(new ResponseModel<string>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<ActionResult<ResponseModel<TokenModel>>> SignIn(LogInDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var response = await _adminServices.SignIn(model);
                if (response.StatusCode == 200)
                {
                    Log.Information($"Sign in Succeeded");
                    return Ok(new ResponseModel<TokenModel>
                    {
                        StatusCode = 200,
                        Message = "Every thing is good",
                        Body = response.TokenModel
                    });
                }
                Log.Error($"Sign in Failed");
                return BadRequest(new ResponseModel<string>
                {
                    StatusCode = 400,
                    Message = "Bad"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Sign in Failed ({ex.Message})");
                return BadRequest(new ResponseModel<TokenModel>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("CreateManager")]
        public async Task<ActionResult<ResponseModel<string>>> CreateManager(SignUpAsManagerDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var response = await _managerServices.SignUp(model);
                if (response)
                {
                    Log.Information($"Manager Created");
                    return Ok(new ResponseModel<string>
                    {
                        StatusCode = 200,
                        Message = "Every thing is good"
                    });
                }
                Log.Error($"Manager Creation Failed");
                return BadRequest(new ResponseModel<string>
                {
                    StatusCode = 400,
                    Message = "Bad"
                });

            }
            catch (Exception ex)
            {
                Log.Error($"Manager Creation Failed ({ex.Message})");
                return BadRequest(new ResponseModel<string>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("Add-BusStop-To-Another")]
        public async Task<ActionResult<ResponseModel<bool>>> AddBusStopToAnother(string Id, string BusStopId)
        {
            try
            {
                await _managerServices.enrollBusStop(Id, BusStopId);
                Log.Information($"Enrolled Process Succeeded");
                return Ok(new ResponseModel<bool>
                {
                    StatusCode = 200,
                    Body = true,
                    Message = "BusStop Added Successfully"

                });
            }
            catch (Exception ex)
            {
                Log.Error($"Enrolled Process Failed ({ex.Message})");
                return BadRequest(new ResponseModel<bool>
                {
                    StatusCode = 400,
                    Body = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("AllJourneys")]
        public async Task<ActionResult<ResponseModel<List<ReturnTimeTableDto>>>> GetAllJourneysInDb()
        {
            try
            {
                var time = DateTime.UtcNow;
                var HistoryJourneys = await _journeysHistoryServices.GetAllJourneys();
                Log.Information($"Get All Journeys Succeeded({time} -> {DateTime.UtcNow})");
                return Ok(new ResponseModel<List<ReturnTimeTableDto>>
                {
                    StatusCode = 200,
                    Body = HistoryJourneys.Select(hj => new ReturnTimeTableDto
                    {
                        ArrivalTime = hj.ArrivalTime,
                        BusId = hj.BusId,
                        LeavingTime = hj.LeavingTime,
                        NumberOfAvailableTickets = hj.Tickets.Count,
                        DestinationName = hj.Destination.Name,
                        StartBusStopName = hj.StartBusStop.Name,
                        TicketPrice = hj.TicketPrice
                    }).ToList(),
                    Message = "All Journeys"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Journeys Failed ({ex.Message})");
                return BadRequest(new ResponseModel<List<ReturnTimeTableDto>>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        #region Seeding Data By Api
        //[NonAction]
        [AllowAnonymous]
        [HttpGet("Zena")]
        public bool Zena7arfy()
        {
            //var buses = new List<Guid>
            //{
            //    Guid.Parse("b05638d5-100b-4f3f-8ab5-126d2bdbd289"),
            //    Guid.Parse("ee1df5b7-3113-4c2d-9dad-38790f82f3af"),
            //    Guid.Parse("bc19d1f9-c7b1-45e1-9a7c-4332daff6d80"),
            //    Guid.Parse("3863a12c-3206-49a6-8528-5615443802cf"),
            //    Guid.Parse("17ab682e-0b8e-46cc-8e38-57459ca77572"),
            //    Guid.Parse("166d4269-4763-41ba-9fd8-5b501cfd3588"),
            //    Guid.Parse("c8cfa728-3b3a-4582-84ac-837f3714f630"),
            //    Guid.Parse("8a7a1d11-1cdf-4be7-ab91-83fe0a25c16e"),
            //    Guid.Parse("4ed5c1e4-72e4-4c04-8415-84f5b4eb5936"),
            //    Guid.Parse("946bf7f8-beec-49ce-ada0-941b67b2eaea"),
            //    Guid.Parse("4fd8b5c4-932d-49d8-bf3a-9983615ef680"),
            //    Guid.Parse("5b1ed771-82c2-4656-9059-a8ccbd030104"),
            //    Guid.Parse("c7c1786a-caf7-4c39-9229-aca2da699da4"),
            //    Guid.Parse("95fdb0cf-23c9-49ca-a834-c251f22dbb6c"),
            //    Guid.Parse("b4fd1549-fe32-47ea-b230-c27e7765edf9"),
            //    Guid.Parse("3d51e59b-d1c6-4613-b7c3-c8bec8a76d67"),
            //    Guid.Parse("514220e8-bb1b-47bc-8b2d-d59223cdb3eb"),
            //    Guid.Parse("f5ddaad9-40ed-43ad-8eed-da79309534f0"),
            //    Guid.Parse("79caf2a0-3ab8-409e-9e28-de0056ac8f52"),
            //    Guid.Parse("85bb1eea-3a4b-48e7-9a71-f683113af154"),
            //};

            var busStops = _context.BusStopMangers.ToList();
            var journeys = new List<JourneyHistory>();

            for (int i = 0; i < 100000; i++)
            {
                Random random = new Random();
                var randomNum1 = random.Next(busStops.Count - 1);
                int randomNum2 = random.Next(busStops.Count - 1);

                while (randomNum1 == randomNum2)
                    randomNum2 = random.Next(busStops.Count - 1);

                var dest = busStops[randomNum1];
                var Start = busStops[randomNum2];

                var JourneyId = Guid.NewGuid();


                // Generate random values for year, month, day, hour, minute, second, and millisecond
                int year = 2024;
                int month = 3;
                int day = random.Next(1, 32);
                int hour = random.Next(0, 24);
                int minute = random.Next(0, 60);
                int second = random.Next(0, 60);
                int millisecond = random.Next(0, 1000);

                DateTime randomDateTime = new DateTime(year, month, day, hour, minute, second, millisecond);


                var arrivalTime = randomDateTime.AddHours(2);
                var LeavingTime = randomDateTime;

                var tickets = new List<Ticket>();

                for (int j = 0; j < 40; j++)
                {
                    var ticket = new Ticket
                    {
                        ArrivalTime = arrivalTime,
                        JourneyId = JourneyId,
                        DestinationId = dest.Id,
                        LeavingTime = LeavingTime,
                        Id = Guid.NewGuid(),
                        Price = 70,
                        SeatNum = j + 1,
                        CreatedTime = LeavingTime.AddHours(random.Next(-48, -1)),
                        StartBusStopId = Start.Id,
                        ReservedOnline = true,
                        ConsumerId = "b85c868b-0387-45d9-9802-f5b0c6c7faeb",
                        DestinationName = dest.Name,
                        StartBusStopName = Start.Name
                    };

                    tickets.Add(ticket);
                }

                var journey = new JourneyHistory
                {
                    ArrivalTime = arrivalTime,
                    BusId = Guid.Parse("6CA14563-32E0-4E43-929A-BFCE323DA86F"),
                    DestinationId = dest.Id,
                    StartBusStopId = Start.Id,
                    LeavingTime = LeavingTime,
                    TicketPrice = 70,
                    Id = JourneyId,
                    Tickets = tickets
                };

                //journeys.Add(journey);
                _context.Journeys.Add(journey);
                _context.SaveChanges();
            }

            return true;
        }

        [NonAction]
        [AllowAnonymous]
        [HttpGet("zena2")]
        public bool ZenaLEvel2()
        {
            var tickets = _context.Tickets;
            foreach (var ticket in tickets)
                tickets.Remove(ticket);

            var journeys = _context.Journeys;
            foreach (var journey in journeys)
                journeys.Remove(journey);

            _context.SaveChanges();
            return true;
        }
        #endregion
    }
}
