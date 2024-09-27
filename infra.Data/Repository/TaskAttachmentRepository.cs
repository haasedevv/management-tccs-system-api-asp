using domain.Entities;
using infra.Data.Context;

namespace infra.Data.Repository;

public class TaskAttachmentRepository
{
    protected readonly NpSqlContext _npSqlContext;

    public TaskAttachmentRepository(NpSqlContext npSqlContext)
    {
        _npSqlContext = npSqlContext;
    }
}
