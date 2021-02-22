using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using ManagerImage.Data;

namespace ManagerImage
{
    public partial class ManagerImageService
    {
        private readonly ManagerImageContext context;
        private readonly NavigationManager navigationManager;

        public ManagerImageService(ManagerImageContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public async Task ExportBatchTasksToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/managerimage/batchtasks/excel") : "export/managerimage/batchtasks/excel", true);
        }

        public async Task ExportBatchTasksToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/managerimage/batchtasks/csv") : "export/managerimage/batchtasks/csv", true);
        }

        partial void OnBatchTasksRead(ref IQueryable<Models.ManagerImage.BatchTask> items);

        public async Task<IQueryable<Models.ManagerImage.BatchTask>> GetBatchTasks(Query query = null)
        {
            var items = context.BatchTasks.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnBatchTasksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBatchTaskCreated(Models.ManagerImage.BatchTask item);

        public async Task<Models.ManagerImage.BatchTask> CreateBatchTask(Models.ManagerImage.BatchTask batchTask)
        {
            OnBatchTaskCreated(batchTask);

            context.BatchTasks.Add(batchTask);
            context.SaveChanges();

            return batchTask;
        }

        partial void OnBatchTaskDeleted(Models.ManagerImage.BatchTask item);

        public async Task<Models.ManagerImage.BatchTask> DeleteBatchTask(int? id)
        {
            var item = context.BatchTasks
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            OnBatchTaskDeleted(item);

            context.BatchTasks.Remove(item);
            context.SaveChanges();

            return item;
        }

        partial void OnBatchTaskGet(Models.ManagerImage.BatchTask item);

        public async Task<Models.ManagerImage.BatchTask> GetBatchTaskById(int? id)
        {
            var items = context.BatchTasks
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var item = items.FirstOrDefault();

            OnBatchTaskGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.ManagerImage.BatchTask> CancelBatchTaskChanges(Models.ManagerImage.BatchTask item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnBatchTaskUpdated(Models.ManagerImage.BatchTask item);

        public async Task<Models.ManagerImage.BatchTask> UpdateBatchTask(int? id, Models.ManagerImage.BatchTask batchTask)
        {
            OnBatchTaskUpdated(batchTask);

            var item = context.BatchTasks
                              .Where(i => i.Id == id)
                              .First();
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(batchTask);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return batchTask;
        }
    }
}
