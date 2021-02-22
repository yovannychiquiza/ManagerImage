using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using ManagerImage.Models.ManagerImage;
using Microsoft.EntityFrameworkCore;
using Blazored.Toast.Services;

namespace ManagerImage.Pages
{
    public partial class BatchTaskComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected ManagerImageService ManagerImage { get; set; }
        [Inject]
        protected ELImageService eLImageService { get; set; }

        [Inject]
        protected IToastService toastService { get; set; }

        protected RadzenGrid<ManagerImage.Models.ManagerImage.BatchTask> grid0;

        IEnumerable<ManagerImage.Models.ManagerImage.BatchTask> _getBatchTasksResult;
        public string Result { get; set; }
        public string Loading { get; set; }
        protected IEnumerable<ManagerImage.Models.ManagerImage.BatchTask> getBatchTasksResult
        {
            get
            {
                return _getBatchTasksResult;
            }
            set
            {
                if(!object.Equals(_getBatchTasksResult, value))
                {
                    _getBatchTasksResult = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            var managerImageGetBatchTasksResult = await ManagerImage.GetBatchTasks();
            getBatchTasksResult = managerImageGetBatchTasksResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<AddBatchTask>("Add Batch Task", null);
              grid0.Reload();

              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(ManagerImage.Models.ManagerImage.BatchTask args)
        {
            var result = await DialogService.OpenAsync<EditBatchTask>("Edit Batch Task", new Dictionary<string, object>() { {"Id", args.Id} });
              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task ProcessUnsortedImages(MouseEventArgs args)
        {
            Result = "Processing please wait....";
            Loading = "loader";
            toastService.ShowInfo("Started. Processing please wait....");
            string message = await Task.Run(() => eLImageService.ProcessUnsortedImages());
            toastService.ShowInfo("Finished. the number of files changed is: " + message);
            Result = "";
            Loading = "";
            await InvokeAsync(() => { StateHasChanged(); });
        }
    }
}
