﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors
{
    public class GetById
    {
        public class Query : IRequest<DoctorDto?>
        {
            public string? DoctorId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.DoctorId)
                    .NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<Query, DoctorDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) =>
                _context = context;

            public async Task<DoctorDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.Doctors
                    .AsNoTracking()
                    .Where(d => d.DoctorId.Equals(request.DoctorId))
                    .Select(d => new DoctorDto
                    {
                        DoctorId = d.DoctorId,
                        Name = d.Name,
                        Location = d.Location
                    })
                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}