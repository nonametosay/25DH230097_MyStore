using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _25DH230097_MyStore.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
       
    }

    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
    }
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category { }

}