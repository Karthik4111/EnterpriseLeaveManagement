using EnterpriseLeaveManagement.Application.Features.Departments.Common;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQuery : IRequest<DepartmentDto?>
{
    public Guid Id { get; }

    public GetDepartmentByIdQuery(Guid id)
    {
        Id = id;
    }
}