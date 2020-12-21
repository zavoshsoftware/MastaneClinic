using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;
using Models;

namespace ViewModels
{

    public class _BaseViewModel
    {
        private BaseViewModelHelper baseviewmodel = new BaseViewModelHelper();

        public List<Service> MenuItems { get { return baseviewmodel.GetMenuService(); } }
        public string Address { get { return baseviewmodel.GetTextItemByName("address"); } }
        public string Phone { get { return baseviewmodel.GetTextItemByName("phone"); } }
        public string Email { get { return baseviewmodel.GetTextItemByName("email"); } }
        public List<Blog> FooterBlogs { get { return baseviewmodel.GetFooterBlogs(); } }
    }

   
}