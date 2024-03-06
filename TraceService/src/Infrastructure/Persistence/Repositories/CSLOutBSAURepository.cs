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
    public class CslOutBsauRepository : IRepository<CslOutBsau>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CslOutBsauRepository> _logger;
        private readonly IDbConnection _connection;

        public CslOutBsauRepository(IConfiguration configuration, ILogger<CslOutBsauRepository> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._connection = new NpgsqlConnection(this._configuration.GetConnectionString("TraceCS"));
        }

        public async Task<long> AddAsync(CslOutBsau entity)
        {
            long result = 0;
            string sql = @$"INSERT INTO public.csl_out_bsau(
	dt, mti, rrn_card_scheme, msg, trn)
	VALUES ( @Dt, @MTI, @RRNCardScheme, @Message, @TRN);";
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
                this._logger.LogError("Insert in csl_out_bsau throw exception: {Message}", e.Message);
            }

            return result;
        }

        public async Task<CslOutBsau> GetByMtiAsync(string mti)
        {
            IEnumerable<CslOutBsau> result=new List<CslOutBsau>();
            string sql = @$"SELECT id, dt, mti, rrn_card_scheme, duration, msg, trn
	FROM public.csl_out_bsau
    WHERE mti='{mti}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                result = await this._connection.QueryAsync<CslOutBsau>(sql, mti);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select from csl_out_bsau throw exception: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }

        public async Task<CslOutBsau> GetByTrnAsync(string trn)
        {
            IEnumerable<CslOutBsau> result = new List<CslOutBsau>();
            string sql = @$"SELECT id, dt, mti, rrn_card_scheme, duration, msg, trn
	FROM public.csl_out_bsau
    WHERE trn='{trn}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }

                result = await this._connection.QueryAsync<CslOutBsau>(sql, trn);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select from csl_out_bsau throw exception: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }

    }
}
