using Domain.DatabaseC;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ThirdPartyChannel
{
    public class Create
    {
        public class Command : IRequest
        {
            public required GatewayInfo GatewayInfo { get; set; }
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
                try
                {
                    if (!string.IsNullOrEmpty(request.GatewayInfo.ContactNo))
                    {
                        //request.Messageinfo.SmsId = "";
                        //var data = _context.GatewayCheckInfo.FromSql($"select * from GatewayInfo").ToList();
                        request.GatewayInfo.ChannelReqDateTime = DateTime.Now;
                        _context.GatewayInfo.Add(request.GatewayInfo);
                        await _context.SaveChangesAsync();
                    }

                }
                catch (Exception ex)
                {
                    
                    //LogExceptionToDailyFile(ex, request.Messageinfo.Message, request.Messageinfo.BranchCode);

                }

                //return Unit.Value;
            }
        }
    }
}
