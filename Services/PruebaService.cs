using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pp3.dominio.Context;
using pp3.dominio.Models;
using Microsoft.EntityFrameworkCore;


namespace pp3.services.Services
{
    public class PruebaService
    {
        private readonly Pp3roContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PruebaService(Pp3roContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;

            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task<Object> PruebaGet()
        {
            var consulta = await context.CLAVEDIGITALIZACION.ToListAsync();
          
            return consulta;
        }
    }
}
