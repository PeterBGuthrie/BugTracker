﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketStatus
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}