﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BrainTrain.API.Helpers.Learnosity;
using BrainTrain.Core.Models;
using BrainTrain.API.Models;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace BrainTrain.API.Controllers
{
    [Authorize(Roles = "Контент-менеджер")]
    public class MidtermController : ApiController
    {
        private BrainTrainContext db = new BrainTrainContext();

        [HttpGet]
        [Route("api/MidtermUserComplaints")]
        public IHttpActionResult MidtermUserComplaints()
        {
            return Ok(db.Midterm_UserComplaints.ToList());
        }

        [HttpGet]
        [Route("api/MidtermEntrants")]
        public IHttpActionResult MidtermEntrants(string search, int pageNum = 1, int perPage = 20)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Skip", (pageNum - 1) * perPage ),
                new SqlParameter("@Take", perPage ),
                new SqlParameter("@Search", string.IsNullOrEmpty(search) ? DBNull.Value : (object)search),
            };
            var response = db.Database.SqlQuery<MidtermEntrantViewModel>($@"
                SELECT [StudentID]
                      ,[EntryDate]
                      ,[LastName]
                      ,[FirstName]
                      ,[MiddleName]
                      ,[IIN]
                FROM [192.168.12.104].[KazNITU].[dbo].[vw_emtihunter]
                WHERE EntryDate IS NULL
				AND
				(
					@Search IS NULL OR
					LastName like N'%' + @Search +'%' OR
					FirstName like N'%' + @Search +'%' OR
					MiddleName like N'%' + @Search +'%' OR
					IIN like N'%' + @Search +'%' OR
					CONCAT(LastName, ' ' + FirstName, ' ' + MiddleName) like N'%' + @Search + '%'
				)
                ORDER BY LastName, FirstName, MiddleName
                OFFSET @Skip ROWS
                FETCH NEXT @Take ROWS ONLY", parameters.ToArray()).ToList();
            var parameters2 = new List<SqlParameter>
            {
                new SqlParameter("@Search", string.IsNullOrEmpty(search) ? DBNull.Value : (object)search ),
            };
            var count = db.Database.SqlQuery<int>($@"
                SELECT COUNT(*)
                FROM [192.168.12.104].[KazNITU].[dbo].[vw_emtihunter]
                WHERE EntryDate IS NULL
				AND
				(
					@Search IS NULL OR
					LastName like N'%' + @Search +'%' OR
					FirstName like N'%' + @Search +'%' OR
					MiddleName like N'%' + @Search +'%' OR
					IIN like N'%' + @Search +'%' OR
					CONCAT(LastName, ' ' + FirstName, ' ' + MiddleName) like N'%' + @Search + '%'
				)", parameters2.ToArray()).FirstOrDefault();

            return Ok(new { Data = response, Count = count });
        }

        [HttpGet]
        [Route("api/MidtermEntrantsCount")]
        public IHttpActionResult MidtermEntrantsCount()
        {
            var count = db.Database.SqlQuery<int>($@"
                SELECT COUNT(*)
                FROM [192.168.12.104].[KazNITU].[dbo].[vw_emtihunter]
                WHERE EntryDate IS NULL").FirstOrDefault();

            return Ok( count );
        }

    }
}