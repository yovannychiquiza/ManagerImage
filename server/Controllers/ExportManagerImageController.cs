using Microsoft.AspNetCore.Mvc;
using ManagerImage.Data;

namespace ManagerImage
{
    public partial class ExportManagerImageController : ExportController
    {
        private readonly ManagerImageContext context;

        public ExportManagerImageController(ManagerImageContext context)
        {
            this.context = context;
        }

        [HttpGet("/export/ManagerImage/batchtasks/csv")]
        public FileStreamResult ExportBatchTasksToCSV()
        {
            return ToCSV(ApplyQuery(context.BatchTasks, Request.Query));
        }

        [HttpGet("/export/ManagerImage/batchtasks/excel")]
        public FileStreamResult ExportBatchTasksToExcel()
        {
            return ToExcel(ApplyQuery(context.BatchTasks, Request.Query));
        }
        [HttpGet("/export/ManagerImage/serialMissing/excel")]
        public FileStreamResult ExportSerialMissingToExcel()
        {
            return ToExcel(ApplyQuery(context.BatchTasks, Request.Query));
        }

    }
}
