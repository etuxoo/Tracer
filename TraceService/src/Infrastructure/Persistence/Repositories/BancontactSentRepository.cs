using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TraceService.Application.Interfaces.Repositories;
using TraceService.Domain.Entities.Bancontact;

namespace TraceService.Infrastructure.Persistence.Repositories
{
    public class BancontactSentRepository : IRepository<BancontactSent>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BancontactSentRepository> _logger;
        private readonly IDbConnection _connection;

        public BancontactSentRepository(IConfiguration configuration, ILogger<BancontactSentRepository> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._connection = new NpgsqlConnection(this._configuration.GetConnectionString("TraceCS"));
            ;
        }

        public async Task<long> AddAsync(BancontactSent entity)
        {
            long result = 0;
            string sql = @$"INSERT INTO public.bancontact_sent(
	 dt, mti, msg, trn)
	VALUES ( @Dt, @MTI, @Message, @TRN)";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                result = await this._connection.ExecuteAsync(sql, entity);
            }
            catch (Exception e)
            {
                this._logger.LogError("Insert in bancontact_sent throw exception: {Message}", e.Message);
            }

            return result;
        }


        public async Task<BancontactSent> GetByMtiAsync(string mti)
        {
            IEnumerable<BancontactSent> result=new List<BancontactSent>();
            string sql = @$"SELECT id, dt, mti, duration, msg, trn
	FROM public.bancontact_sent
    WHERE mti='{mti}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                result = await this._connection.QueryAsync<BancontactSent>(sql, mti);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select from bancontact_sent throw exception: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }

        public async Task<BancontactSent> GetByTrnAsync(string trn)
        {
            IEnumerable<BancontactSent> result=new List<BancontactSent>();
            string sql = @$"SELECT id, dt, mti, duration, msg, trn
	FROM public.bancontact_sent
    WHERE trn='{trn}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }

                result = await this._connection.QueryAsync<BancontactSent>(sql, trn);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select from bancontact_sent throw exception: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }
    }
}
