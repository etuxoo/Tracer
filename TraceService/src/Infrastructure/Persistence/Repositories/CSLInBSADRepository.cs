using System;
using System.Data;
using System.Threading.Tasks;
using TraceService.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Dapper;
using TraceService.Domain.Entities.Bancontact;
using System.Linq;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;

namespace TraceService.Infrastructure.Persistence.Repositories
{
    public class CslInBsadRepository : IRepository<CslInBsad>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CslInBsadRepository> _logger;
        private readonly IDbConnection _connection;
        public CslInBsadRepository(IConfiguration configuration, ILogger<CslInBsadRepository> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._connection = new NpgsqlConnection(this._configuration.GetConnectionString("TraceCS"));
        }

        public async Task<long> AddAsync(CslInBsad entity)
        {
            long result = 0;
            string sql = @$"INSERT INTO public.csl_in_bsad(
	 dt, mti, tid, mid, rrn_in, rrn_out, proc_code, pan_exp_date, msg, trn)
	VALUES ( @Dt, @Mti, @Tid, @Mid, @RrnIn, @RrnOut, @ProcCode, @PanExpDate, @Message, @Trn)";

            NpgsqlCommand command = new(sql);

            command.Parameters.Add(new NpgsqlParameter("Message", NpgsqlDbType.Jsonb) { Value = (object)entity.Message ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("Dt", NpgsqlDbType.Date) { Value = (object)entity.Dt ?? DBNull.Value});
            command.Parameters.Add(new NpgsqlParameter("Mti", NpgsqlDbType.Char) { Value = (object)entity.Mti ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("Tid", NpgsqlDbType.Varchar) { Value = (object)entity.TId ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("Mid", NpgsqlDbType.Varchar) { Value = (object)entity.Mid ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("RrnIn", NpgsqlDbType.Varchar) { Value = (object)entity.RrnIn ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("RrnOut", NpgsqlDbType.Varchar) { Value = (object)entity.RrnOut ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("ProcCode", NpgsqlDbType.Char) { Value = (object)entity.ProcCode ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("PanExpDate", NpgsqlDbType.Varchar) { Value = (object)entity.PanExpDate ?? DBNull.Value });
            command.Parameters.Add(new NpgsqlParameter("Trn", NpgsqlDbType.Varchar) { Value = (object)entity.Trn ?? DBNull.Value });

            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                //result = await this._connection.ExecuteAsync(sql, entity);
                command.Connection = (NpgsqlConnection)this._connection;
                result = await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                this._logger.LogError("Insert in csl_in_bsad throw exception: {Message}", e.Message);
            }

            return result;
        }

        public async Task<CslInBsad> GetByMtiAsync(string mti)
        {
            IEnumerable<CslInBsad> result= new List<CslInBsad>();
            string sql = @$"SELECT id, dt, mti, tid, mid, rrn_in, rrn_out, proc_code, duration, pan_exp_date, msg, trn
	FROM public.csl_in_bsad
    WHERE mti='{mti}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }

                result = await this._connection.QueryAsync<CslInBsad>(sql, mti);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select from csl_in_bsad throw exception: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }

        public async Task<CslInBsad> GetByTrnAsync(string trn)
        {
            IEnumerable<CslInBsad> result=new List<CslInBsad>();
            string sql = @$"SELECT id, dt, mti, tid, mid, rrn_in, rrn_out, proc_code, duration, pan_exp_date, msg, trn
	FROM public.csl_in_bsad
    WHERE trn='{trn}';";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }

                result = await this._connection.QueryAsync<CslInBsad>(sql, trn);
            }
            catch (Exception e)
            {
                this._logger.LogError("Select from csl_in_bsad throw exception: {Message}", e.Message);
            }

            return result.FirstOrDefault();
        }

    }
}
