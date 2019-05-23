#region Using Directives

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Kovai.AtomicScope.BusinessAccess.Implementation;
using Kovai.AtomicScope.BusinessAccess.Interfaces;
using Kovai.AtomicScope.Core.Extensions;
using Kovai.AtomicScope.Models.API;

#endregion

namespace Kovai.AtomicScope.APIs.Controllers
{
    [RoutePrefix("api/activities")]
    [EnableCors(methods: "*", origins: "*", headers: "*")]
    public class ActivitiesController : ApiController
    {
        private readonly IActivitiesService _activitiesService;
        public ActivitiesController()
        {
            _activitiesService = new ActivitiesService();
        }

        public ActivitiesController(IActivitiesService activitiesService)
        {
            _activitiesService = activitiesService;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(SearchActivityResult))]
        public IHttpActionResult GetMainActivities([FromUri]SearchActivityCriteria criteria)
        {
            try
            {
                if (criteria.IsNull())
                    return BadRequest("Invalid criteria received.");

                if (criteria.StartTime.IsNull() || criteria.StartTime == default(DateTime))
                    return BadRequest("Start time is required.");

                return Ok(_activitiesService.GetActivities(criteria));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{activityId}/stages")]
        public IHttpActionResult GetStageActivities([FromUri] Guid activityId)
        {
            try
            {
                //if (activityId.IsNullOrEmpty())
                //    return BadRequest("Invalid activity id received.");

                return Ok(_activitiesService.GetStageActivities(activityId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{activityId}/related")]
        public IHttpActionResult GetRelatedActivities([FromUri] Guid activityId)
        {
            try
            {
                //if (activityId.IsNullOrEmpty())
                //    return BadRequest("Invalid activity id received.");
                return Ok(_activitiesService.GetRelatedActivities(activityId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("archives/{stageActivityId}")]
        public IHttpActionResult GetArchiveSettings([FromUri] Guid stageActivityId)
        {
            try
            {
                //if (activityId.IsNullOrEmpty())
                //    return BadRequest("Invalid activity id received.");
                return Ok(_activitiesService.GetActivityArchive(stageActivityId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("archives/content/{archiveId}")]
        public IHttpActionResult GetArchiveContent([FromUri] Guid archiveId)
        {
            try
            {
                //  if (activityId.IsNullOrEmpty())
                //      return BadRequest("Invalid activity id received.");
                var content = _activitiesService.GetArchiveContent(archiveId);
                var toBytes = Encoding.Unicode.GetBytes(content);

                var stream = new MemoryStream(toBytes);

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(stream)
                };
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = archiveId+".txt"
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                var response = ResponseMessage(result);
                return response;
                
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("exceptions")]
        public IHttpActionResult GetExceptionInfo(Guid stageActivityId)
        {
            try
            {
                return Ok(_activitiesService.GetExceptionInfo(stageActivityId));

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}