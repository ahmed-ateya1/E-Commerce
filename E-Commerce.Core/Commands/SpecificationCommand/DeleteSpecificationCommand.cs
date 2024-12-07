using MediatR;

namespace E_Commerce.Core.Commands.SpecificationCommand
{
    public class DeleteSpecificationCommand : IRequest<bool>
    {
        public Guid Id { get; }

        public DeleteSpecificationCommand(Guid id)
        {
            Id = id;
        }
    }
}
