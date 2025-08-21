using Ardalis.Result;
using MediatR;

namespace TQ.UseCases.TrevalQuest.Queries.Main.Patten
{
    public class ExcelPatternNewTeamQuery : IRequest<Result<(MemoryStream, string)>>
    {
        public ExcelPatternNewTeamQuery()
        {

        }
    }
}
