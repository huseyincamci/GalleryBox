using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Galeri.Web.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
    }
}