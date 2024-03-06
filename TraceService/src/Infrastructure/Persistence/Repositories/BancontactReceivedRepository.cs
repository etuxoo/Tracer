using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using TraceService.Application.Interfaces.Repositories;
using TraceService.Domain.Entities.Bancontact;

namespace TraceService.Infrastructure.Persistence.Repositories
{
    public class BancontactReceivedRepository : IRepository<BancontactReceived>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BancontactReceivedRepository> _logger;
        private readonly IDbConnection _connection;

        public BancontactReceivedRepository(IConfiguration configuration, ILogger<BancontactReceivedRepository> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._connection = new NpgsqlConnection(this._configuration.GetConnectionString("TraceCS"));
        }

        public async Task<long> AddAsync(BancontactReceived entity)
        {
            long result = 0;
            string sql = @$"INSERT INTO public.bancontact_received ( dt, mti, msg, trn) VALUES ( @Dt, @MTI, @Message, @TRN )";
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
                this._logger.LogError("Insert in bancontact_received throw exception: {Message}", e.Message);
            }

            return result;
        }

        public async Task<BancontactReceived> GetByMtiAsync(string mti)
        {
            IEnumerable<BancontactReceived> result=new List<BancontactReceived>();
            string sql = @$"SELECT id, dt, mti, duration, msg, trn
	FROM public.bancontact_received
    WHERE mti='{mti}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                result = await this._connection.QueryAsync<BancontactReceived>(sql);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select from bancontact_received throw exception: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }

        public async Task<BancontactReceived> GetByTrnAsync(string trn)
        {
            IEnumerable<BancontactReceived> result = new List<BancontactReceived>();
            string sql = @$"SELECT id, dt, mti, duration, msg, trn
	FROM public.bancontact_received
    WHERE trn='{trn}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }

                result = await this._connection.QueryAsync<BancontactReceived>(sql);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select From bancontact_received throw exeption: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }
    }
}
