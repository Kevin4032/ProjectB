﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetDepot.Registration.Model
{
	public class Reservation
	{
		public Reservation()
		{
			Reservations = new HashSet<string>();
		}
		public HashSet<string> Reservations { get; set; }
	}
}
