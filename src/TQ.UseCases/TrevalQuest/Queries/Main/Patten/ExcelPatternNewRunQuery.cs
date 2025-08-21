using Ardalis.Result;
using MediatR;

namespace TQ.UseCases.TrevalQuest.Queries.Main.Patten
{
    public class ExcelPatternNewRunQuery : IRequest<Result<(MemoryStream, string)>>
    {
        public ExcelPatternNewRunQuery()
        {

        }
    }
}
