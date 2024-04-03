﻿using Core.Dto;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers.Functions
{
    public static class TicketConvertor
    {
        public static ReturnedTicketDto ConvertToDto(this Ticket ticket) =>
            new ReturnedTicketDto
            {
                BusId = ticket.BusId,
                CreatedTime = ticket.CreatedTime,
                IsFinished = ticket.IsFinished,
                JourneyId = ticket.JourneyId,
                Price = ticket.Price,
                SeatId = ticket.SeatId,
                SeatNumber = ticket.Seat.SeatNum,
                UserName = ticket.UserName,
            };
    }
}