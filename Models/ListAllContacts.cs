using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformationManagementMVC.Models
{
    public class ListAllContacts
    {
        public IPagedList<CallList> ListAllContact { get; set; }
    }
}