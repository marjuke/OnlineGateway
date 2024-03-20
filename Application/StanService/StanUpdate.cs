using Domain.DatabaseC;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StanService
{
    public class StanUpdate
    {
        public class Command : IRequest
        {
            public string Stan { get; set; }
        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var stan = _context.stan.FirstOrDefault();
                if (stan == null)
                {
                    Stan stan1 = new Stan();
                    stan1.CounterDate = DateTime.Now.Date;
                    stan1.CounterValue = 0;
                    _context.stan.Add(stan1);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (stan.CounterDate != DateTime.Now.Date)
                    {
                        stan.CounterDate = DateTime.Now.Date;
                        stan.CounterValue = 0;
                        _context.Entry(stan).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
                
            }
        }
    }
}
