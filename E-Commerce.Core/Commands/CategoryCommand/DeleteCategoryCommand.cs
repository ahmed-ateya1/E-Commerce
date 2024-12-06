using MediatR;

namespace E_Commerce.Core.Commands.CategoryCommand
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public Guid Id { get; }

        public DeleteCategoryCommand(Guid id)
        {
            Id = id;
        }
    }
}
