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
using TraceService.Application.Models;

namespace TraceService.Infrastructure.Persistence.Repositories
{
    public class BancontactSearchRepository : ISearchRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BancontactSearchRepository> _logger;
        private readonly IDbConnection _connection;

        public BancontactSearchRepository(IConfiguration configuration, ILogger<BancontactSearchRepository> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._connection = new NpgsqlConnection(this._configuration.GetConnectionString("TraceCS"));
        }

        public async Task<PaginatedList<SearchResultModel>> GetGeneralSearch(string mti, int PageSize, int Page, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            IEnumerable<SearchResultModel> data = new List<SearchResultModel>();
            int offset = (Page - 1) * PageSize;
            List<string> tables = [
            @"SELECT DISTINCT trn,mti,dt
FROM public.bancontact_received",
                @"SELECT DISTINCT trn,mti,dt
FROM public.bancontact_sent",
                @"SELECT DISTINCT trn,mti,dt
FROM public.csl_in_bsad",
                @"SELECT DISTINCT trn,mti,dt
FROM public.csl_in_bsau",
                @"SELECT DISTINCT trn,mti,dt
FROM public.csl_out_bsau",
                @"SELECT DISTINCT trn,mti,dt
FROM public.csl_out_bsau"
            ];
            string sql = @$"WITH bancontact_comunication AS (";

            for (int i = 0; i < tables.Count; i++)
            {
                sql += tables.ElementAt(i);
                sql += @$"
WHERE mti LIKE '%{mti}%'";

                if (dateFrom.HasValue)
                {
                    sql += $@"
AND dt > '{dateFrom}'";
                }
                if (dateTo.HasValue)
                {
                    sql += $@"
AND dt < '{dateTo}'";
                }
                if (i != tables.Count - 1)
                {
                    sql += @"
UNION";
                }
            }

            sql += $@"
)
SELECT DISTINCT trn,mti,dt
FROM bancontact_comunication
OFFSET {offset} ROWS
FETCH NEXT {PageSize} ROWS ONLY";

            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                data = await this._connection.QueryAsync<SearchResultModel>(sql);
            }
            catch (Exception e)
            {
                this._logger.LogError("General search throw exception: {Message}", e.Message);
            }

            PaginatedList<SearchResultModel> result = new(data, await this.GetTotalCount(), Page, PageSize);

            return result;
        }

        public async Task<PaginatedList<SearchResultModel>> GetCslInSearch(string mti, int PageSize, int Page, string mid, string tid, string panExpDate, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            IEnumerable<SearchResultModel> data = new List<SearchResultModel>();
            int offset = (Page - 1) * PageSize;
            List<string> tables = [
@"SELECT  *
FROM public.bancontact_received",
                @"SELECT  *
FROM public.bancontact_sent"
];

            string sql = @$"WITH bancontact_comunication AS (
";

            for (int i = 0; i < tables.Count; i++)
            {
                sql += tables.ElementAt(i);
                sql += @$"WHERE mti LIKE '%{mti}%'
AND mid LIKE '%{mid}%'
AND tid LIKE '%{tid}%'
AND pan_exp_date LIKE '%{panExpDate}%";
                if (dateFrom.HasValue)
                {
                    sql += $@"
AND dt > '{dateFrom}'";
                }
                if (dateTo.HasValue)
                {
                    sql += $@"
AND dt < '{dateTo}'";
                }
                if (i != tables.Count - 1)
                {
                    sql += @"
UNION";
                }
            }

            sql += $@"
)
SELECT DISTINCT trn,mti
FROM bancontact_comunication
OFFSET {offset} ROWS
FETCH NEXT {PageSize} ROWS ONLY";

            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                data = await this._connection.QueryAsync<SearchResultModel>(sql);
            }
            catch (Exception e)
            {
                this._logger.LogError("CSLIn search throw exception: {Message}", e.Message);
            }

            return new PaginatedList<SearchResultModel>(data.ToList(), await this.GetTotalCount(), Page, PageSize);
        }

        public async Task<PaginatedList<SearchResultModel>> GetRrnSearch(string mti, string rrn, int PageSize, int Page, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            IEnumerable<SearchResultModel> data = new List<SearchResultModel>();
            int offset = (Page - 1) * PageSize;
            List<string> tables = [
            @"SELECT DISTINCT trn,mti,dt,rrn_in,rrn_out
FROM public.csl_in_bsad",
                @"SELECT DISTINCT trn,mti,dt,rrn_in,rrn_out
FROM public.csl_in_bsau",
                @"SELECT DISTINCT trn,mti,dt,rrn_card_scheme as rrn_in,rrn_card_scheme as rrn_out
FROM public.csl_out_bsad",
                @"SELECT DISTINCT trn,mti,dt,rrn_card_scheme as rrn_in,rrn_card_scheme as rr_out
FROM public.csl_out_bsau"
            ];
            string sql = @$"WITH bancontact_comunication AS (
";


            for (int i = 0; i < tables.Count; i++)
            {
                sql += tables.ElementAt(i);
                sql += @$"WHERE rrn_in LIKE '%{rrn}%'
AND rrn_out LIKE '%{rrn}%'";

                if (dateFrom.HasValue)
                {
                    sql += $@"
AND dt > '{dateFrom}'";
                }
                if (dateTo.HasValue)
                {
                    sql += $@"
AND dt < '{dateTo}'";
                }
                if (i != tables.Count - 1)
                {
                    sql += @"
UNION";
                }
            }

            sql += $@"
)
SELECT DISTINCT trn,mti,dt
FROM bancontact_comunication
OFFSET {offset} ROWS
FETCH NEXT {PageSize} ROWS ONLY";

            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                data = await this._connection.QueryAsync<SearchResultModel>(sql);
            }
            catch (Exception e)
            {
                this._logger.LogError("RRN search throw exception: {Message}", e.Message);
            }

            return new PaginatedList<SearchResultModel>(data.ToList(), await this.GetTotalCount(), Page, PageSize);
        }

        private async Task<int> GetTotalCount()
        {
            int result = -1;
            string sql = @$"WITH bancontact_comunication AS (
SELECT COUNT(DISTINCT trn) as trn
FROM public.bancontact_received
UNION 
SELECT COUNT(DISTINCT trn) as trn
FROM public.bancontact_sent
UNION
SELECT COUNT(DISTINCT trn) as trn
FROM public.csl_in_bsad
UNION
SELECT COUNT(DISTINCT trn) as trn
FROM public.csl_in_bsau
UNION 
SELECT COUNT(DISTINCT trn) as trn
FROM public.csl_out_bsad
UNION
SELECT COUNT(DISTINCT trn) as trn
FROM public.csl_out_bsau
)
SELECT SUM(trn)
FROM bancontact_comunication";
            try
            {
                if (this._connection.State != ConnectionState.Open)
                {
                    this._connection.Open();
                }
                result = await this._connection.ExecuteScalarAsync<int>(sql);
            }
            catch (Exception e)
            {
                this._logger.LogError("Trn count throw exception: {Message}", e.Message);
            }

            return result;
        }

    }

}
