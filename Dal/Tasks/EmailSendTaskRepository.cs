using Infrastructure.Entities.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Enum;
using MRApiCommon.Infrastructure.Interface;
using MRApiCommon.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Tasks
{
    public class EmailSendTaskRepository : MRMongoRepository<EmailSendTask>, IMRRepository<EmailSendTask>
    {
        public EmailSendTaskRepository(IOptions<MRDbOptions> options) : base(options) { }

        public async Task InsertEmail(string toEmail, string subject, string body, EmailTaskBot bot)
        {
            await Insert(new EmailSendTask
            {
                Bot = bot,
                Status = EmailSendStatus.New,
                Subject = subject,
                ToEmail = toEmail,
                Body = body
            });
        }

        public async Task<IEnumerable<EmailSendTask>> GetByStatus(EmailSendStatus status, int? limit = null)
        {
            var query = _builder
                .Eq(x => x.State, MREntityState.Active)
                .Eq(x => x.Status, status)
                .Sorting(x => x.CreateTime, false);

            if (limit.HasValue && limit.Value > 0)
            {
                query.Limit(limit.Value);
            }

            return await GetByQuery(query);
        }

        public async Task<UpdateResult> MultiplyUpdateStatus(IEnumerable<string> ids, EmailSendStatus newStatus)
        {
            if (ids == null || !ids.Any())
            {
                return UpdateResult.Unacknowledged.Instance;
            }

            var query = _builder
                .Eq(x => x.State, MREntityState.Active)
                .In(x => x.Id, ids)
                .UpdateSet(x => x.UpdateTime, DateTime.UtcNow)
                .UpdateSet(x => x.Status, newStatus);

            await UpdateManyByQuery(query);
            return new UpdateResult.Acknowledged(ids.Count(), ids.Count(), "");
        }

        public async Task<UpdateResult> UpdateStatus(string id, EmailSendStatus newStatus, string failMessage = null)
        {
            var query = _builder
                .Eq(x => x.State, MREntityState.Active)
                .Eq(x => x.Id, id)
                .UpdateSet(x => x.UpdateTime, DateTime.UtcNow)
                .UpdateSet(x => x.Status, newStatus)
                .UpdateSet(x => x.FailMessage, failMessage);

            await UpdateByQuery(query);
            return new UpdateResult.Acknowledged(1, 1, id);
        }
    }
}
