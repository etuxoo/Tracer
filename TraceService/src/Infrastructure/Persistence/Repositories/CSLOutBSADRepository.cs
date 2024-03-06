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
    public class CslOutBsadRepository : IRepository<CslOutBsad>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CslOutBsadRepository> _logger;
        private readonly IDbConnection _connection;

        public CslOutBsadRepository(IConfiguration configuration, ILogger<CslOutBsadRepository> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._connection = new NpgsqlConnection(this._configuration.GetConnectionString("TraceCS"));
        }


        public async Task<long> AddAsync(CslOutBsad entity)
        {
            long result = 0;
            string sql = @$"INSERT INTO public.csl_out_bsad(
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
                this._logger.LogError("insert in csl_in_bsad throw exception: {Message}", e.Message);
            }

            return result;
        }

        public async Task<CslOutBsad> GetByMtiAsync(string mti)
        {
            IEnumerable<CslOutBsad> result=new List<CslOutBsad>();
            string sql = @$"SELECT id, dt, mti, rrn_card_scheme, duration, msg, trn
	FROM public.csl_out_bsad
    WHERE mti='{mti}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                    result = await this._connection.QueryAsync<CslOutBsad>(sql, mti);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select from csl_out_bsad throw exception: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }

        public async Task<CslOutBsad> GetByTrnAsync(string trn)
        {
            IEnumerable<CslOutBsad> result=new List<CslOutBsad>();
            string sql = @$"SELECT id, dt, mti, rrn_card_scheme, duration, msg, trn
	FROM public.csl_out_bsad
    WHERE trn='{trn}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }

                result = await this._connection.QueryAsync<CslOutBsad>(sql, trn);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select from csl_out_bsad throw exception: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }
    }
}
