using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Exceptions.Catalogue;

public class CatalogueNotFoundException : Exception
{
    public CatalogueNotFoundException() : base() { }

    public CatalogueNotFoundException(string message) : base(message) { }

    public CatalogueNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
