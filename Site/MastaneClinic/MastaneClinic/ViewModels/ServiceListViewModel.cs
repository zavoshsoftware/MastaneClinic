using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class ServiceListViewModel:_BaseViewModel
    {
        public List<Models.Service> Services { get; set; }
    }
}