using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace TQ.UseCases.TrevalQuest.Queries.Main.Add
{
    public class ExcelNewRunCommand : IRequest<Result<(MemoryStream, string)>>
    {
        public ExcelNewRunCommand(IFormFile file)
        {
            File = file;
        }
        public IFormFile File { get; private set; }
    }
}
