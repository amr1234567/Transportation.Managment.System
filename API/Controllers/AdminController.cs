using Core.Constants;
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
using Services.ApplicationServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    [ApiController]
    public class AdminController(IBusServices busService, IAdminServices adminServices, IManagerServices managerServices, IJourneysHistoryServices journeysHistoryServices, ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IAdminServices _adminServices = adminServices;
        private readonly IManagerServices _managerServices = managerServices;
        private readonly IJourneysHistoryServices _journeysHistoryServices = journeysHistoryServices;
        private readonly IBusServices _busService = busService;

        [NonAction]
        [HttpPost("sign-up")]
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
        [HttpPost("sign-in")]
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
                    Message = "Bad Input"
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

        [HttpPost("create-manager")]
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

        [HttpPost("enroll-bus-stop-to-bus-stop")]
        public async Task<ActionResult<ResponseModel<bool>>> EnrollBusStopToAnother(string StartBusStopId, string DestinationBusStopId)
        {
            try
            {
                await _managerServices.enrollBusStop(StartBusStopId, DestinationBusStopId);
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

        //[AllowAnonymous]
        //[HttpGet("gg/{desId}/{startId}")]
        //public IEnumerable<ReturnedHistoryJourneyDto> get(string desId, string startId)
        //{
        //    return [.. _context.Journeys
        //        .Include(x => x.Destination)
        //        .Include(x => x.StartBusStop)
        //        .Include(x => x.Tickets)
        //        .Where(x => x.DestinationId == desId && x.StartBusStopId == startId)
        //        .Select(hj => new ReturnedHistoryJourneyDto
        //            {
        //                ArrivalTime = hj.ArrivalTime,
        //                BusId = hj.BusId,
        //                LeavingTime = hj.LeavingTime,
        //                NumberOfAvailableTickets = hj.Tickets.Count(),
        //                DestinationName = hj.Destination.Name,
        //                StartBusStopName = hj.StartBusStop.Name,
        //                TicketPrice = hj.TicketPrice
        //            })];
        //}

        [HttpGet("get-all-history-journeys")]
        public async Task<ActionResult<ResponseModel<IEnumerable<ReturnedHistoryJourneyDto>>>> GetAllJourneysInDb()
        {
            try
            {
                var time = DateTime.UtcNow;
                var HistoryJourneys = await _journeysHistoryServices.GetAllJourneys();
                Log.Information($"Get All Journeys Succeeded({time} -> {DateTime.UtcNow})");
                return Ok(new ResponseModel<IEnumerable<ReturnedHistoryJourneyDto>>
                {
                    StatusCode = 200,
                    Body = HistoryJourneys.Select(hj => new ReturnedHistoryJourneyDto
                    {
                        ArrivalTime = hj.ArrivalTime,
                        BusId = hj.BusId,
                        LeavingTime = hj.LeavingTime,
                        NumberOfAvailableTickets = hj.Tickets.Count(),
                        DestinationName = hj.Destination.Name,
                        StartBusStopName = hj.StartBusStop.Name,
                        TicketPrice = hj.TicketPrice
                    }),
                    Message = "All Journeys"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Get All Journeys Failed ({ex.Message})");
                return BadRequest(new ResponseModel<List<ReturnedHistoryJourneyDto>>
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("get-all-buses")]
        public async Task<ActionResult<ResponseModel<IEnumerable<Bus>>>> GetAllBuses()
        {
            try
            {
                var model = new ResponseModel<IEnumerable<Bus>>
                {

                    StatusCode = 200,
                    Message = "Done",
                    Body = await _busService.GetAllBuses(),

                };
                Log.Information("Get All Buses Success");
                return model;
            }
            catch (Exception ex)
            {
                var model = new ResponseModel<IEnumerable<Bus>>
                {
                    StatusCode = 404,
                    Message = ex.Message,
                    Body = new List<Bus>()
                };
                Log.Error($"Get All Buses Error:{ex.Message}");
                return model;
            }

        }


        #region Seeding Data By Api
        [NonAction]
        [AllowAnonymous]
        [HttpGet("Zena")]
        public bool ManoalSeeding()
        {
            var busStops = _context.BusStopMangers.ToList();
            var buses = _context.Buses.ToList();

            for (int i = 0; i < 200; i++)
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
                int year = 2023;
                int month = random.Next(1, 13);
                int day = random.Next(1, 29);
                int hour = random.Next(0, 24);
                int minute = random.Next(0, 60);
                int second = random.Next(0, 60);
                int millisecond = random.Next(0, 1000);

                DateTime randomDateTime = new DateTime(year, month, day, hour, minute, second, millisecond);


                var arrivalTime = randomDateTime.AddHours(2);
                var LeavingTime = randomDateTime;

                var tickets = new List<Ticket>();

                for (int j = 0; j < random.Next(25, 40); j++)
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
                    BusId = buses[random.Next(buses.Count)].Id,
                    DestinationId = dest.Id,
                    StartBusStopId = Start.Id,
                    LeavingTime = LeavingTime,
                    TicketPrice = 70,
                    Id = JourneyId,
                    Tickets = tickets,
                    Date = new DateTime(LeavingTime.Year, LeavingTime.Month, LeavingTime.Day)
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
        public bool ManoalDeleteing()
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
