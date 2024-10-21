using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
	public class Message<T>
	{
		public string message { get; set; }
		public string result { get; set; }
		public T Data { get; set; }
	}
}
