using MediatR;

namespace E_Commerce.Core.Commands.ProductCommand
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteProductCommand(Guid id)
        {
            Id = id;
        }
    }
}
