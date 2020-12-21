using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class BlogListViewModel : _BaseViewModel
    {
        public List<Models.Blog> Blogs { get; set; }
    }
}