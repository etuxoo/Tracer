using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TraceService.Application.Interfaces.Repositories;
using TraceService.Domain.Entities.Bancontact;


namespace TraceService.Infrastructure.Persistence.Repositories
{
    public class CslInBsauRepository : IRepository<CslInBsau>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CslInBsauRepository> _logger;
        private readonly IDbConnection _connection;

        public CslInBsauRepository(IConfiguration configuration, ILogger<CslInBsauRepository> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._connection = new NpgsqlConnection(this._configuration.GetConnectionString("TraceCS"));
        }

        public async Task<long> AddAsync(CslInBsau entity)
        {
            long result = 0;
            string sql = @$"INSERT INTO public.csl_in_bsau(
	 dt, mti, tid, mid, rrn_in, rrn_out, proc_code, pan_exp_date, msg, trn)
	VALUES (@Dt, @Mti, @Tid, @Mid, @RrnIn, @RrnOut, @ProcCode, @PanExpDate, @Message, @Trn)";
            NpgsqlCommand command = new(sql);

            command.Parameters.Add(new NpgsqlParameter("Message", NpgsqlDbType.Jsonb) { Value = (object)entity.Message ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("Dt", NpgsqlDbType.Date) { Value = (object)entity.Dt ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("Mti", NpgsqlDbType.Char) { Value = (object)entity.Mti ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("Tid", NpgsqlDbType.Varchar) { Value = (object)entity.Tid ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("Mid", NpgsqlDbType.Varchar) { Value = (object)entity.Mid ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("RrnIn", NpgsqlDbType.Varchar) { Value = (object)entity.RrnIn ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("RrnOut", NpgsqlDbType.Varchar) { Value = (object)entity.RrnOut ?? DBNull.Value});
            command.Parameters.Add(new NpgsqlParameter("ProcCode", NpgsqlDbType.Char) { Value = (object)entity.ProcCode ?? DBNull.Value});
            command.Parameters.Add(new NpgsqlParameter("PanExpDate", NpgsqlDbType.Varchar) { Value = (object)entity.PanExpDate ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("Trn", NpgsqlDbType.Varchar) { Value = (object)entity.Trn ?? DBNull.Value });

            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                //result = await this._connection.ExecuteAsync(sql, entity);
                command.Connection= (NpgsqlConnection) this._connection;
                result = await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                this._logger.LogError("insert in csl_in_bsau throw exception: {Message}", e.Message);
            }

            return result;
        }


        public async Task<CslInBsau> GetByMtiAsync(string mti)
        {
            IEnumerable<CslInBsau> result = new List<CslInBsau>();
            string sql = @$"SELECT id, dt, mti, tid, mid, rrn_in, rrn_out, proc_code, duration, pan_exp_date, msg, trn
	FROM public.csl_in_bsau
    WHERE mti='{mti}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                result = await this._connection.QueryAsync<CslInBsau>(sql, mti);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select from csl_in_bsau throw exception: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }

        public async Task<CslInBsau> GetByTrnAsync(string trn)
        {
            IEnumerable<CslInBsau> result = new List<CslInBsau>();
            string sql = @$"SELECT id, dt, mti, tid, mid, rrn_in, rrn_out, proc_code, duration, pan_exp_date, msg, trn
	FROM public.csl_in_bsau
    WHERE trn='{trn}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }

                result = await this._connection.QueryAsync<CslInBsau>(sql, trn);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select from csl_in_bsau throw exception: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }
    }
}
