using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Image
    {
        public Guid ImageId { get; set; }
        public Guid BlogId { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }

        public virtual BlogPost Blog { get; set; }
    }
}
